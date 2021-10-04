using AutoMapper;
using Comparator.Base;
using Comparator.Dtos;
using Comparator.Models;
using Comparator.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Comparator.Services
{
    public class ComparatorService : IComparatorService
    {
        private readonly ILeftRepo _leftRepo;
        private readonly IRightRepo _rightRepo;
        private readonly IResultRepo _resultRepo;
        private readonly IDiffRepo _diffRepo;
        private readonly IMapper _mapper;

        public ComparatorService(ILeftRepo leftRepo, IRightRepo rightRepo, IResultRepo resultRepo, IDiffRepo diffRepo, IMapper mapper)
        {
            _leftRepo = leftRepo;
            _rightRepo = rightRepo;
            _resultRepo = resultRepo;
            _diffRepo = diffRepo;
            _mapper = mapper;
        }

        public async Task<bool> InsertOrUpdateLeftAsync(int id, DataRequest data)
        {
            try
            {
                var left = await GetLeftByIdAsync(id);
                if (left == null)
                    return await InsertLeftAsync(id, data);

                return await UpdateLeftAsync(id, data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> InsertOrUpdateRightAsync(int id, DataRequest data)
        {
            try
            {
                var right = await GetRightByIdAsync(id);
                if (right == null) 
                    return await InsertRightAsync(id, data);

                return await UpdateRightAsync(id, data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> InsertLeftAsync(int id, DataRequest data)
        {
            try
            {
                var one = _mapper.Map<Left>(data);

                return await _leftRepo.InsertOneWithCustomId(id, one);
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }

        private async Task<bool> InsertRightAsync(int id, DataRequest data)
        {
            try
            {
                var one = _mapper.Map<Right>(data);

                return await _rightRepo.InsertOneWithCustomId(id, one);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> UpdateLeftAsync(int id, DataRequest data)
        {
            try
            {
                var result = _mapper.Map<Left>(data);

                return await _leftRepo.UpdateOne(id, result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> UpdateRightAsync(int id, DataRequest data)
        {
            try
            {
                var result = _mapper.Map<Right>(data);

                return await _rightRepo.UpdateOne(id, result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultResponse> GetComparatorResultByIdAsync(int id)
        {
            try
            {
                var left = await GetLeftByIdAsync(id);
                if (left == null) return null;

                var right = await GetRightByIdAsync(id);
                if (right == null) return null;
                
                var existingResult = await GetResultIfExistAndIsValid(id, left, right);
                if (existingResult != null)
                     return _mapper.Map<ResultResponse>(existingResult);

                var insertedResult = await CompareAndInsertResult(id, left, right);
                if (insertedResult != null)
                    return _mapper.Map<ResultResponse>(insertedResult);

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<Left> GetLeftByIdAsync(int id)
        {
            var left = await _leftRepo.GetById(id);
            if (left == null) return null;

            return left;
        }

        private async Task<Right> GetRightByIdAsync(int id)
        {
            var right = await _rightRepo.GetById(id);
            if (right == null) return null;

            return right;
        }

        private async Task<Result> CompareAndInsertResult(int id, Left left, Right right)
        {
            if (left == null || right == null) return null;

            var (resultType, diffs) = await Task.Run(() => CompareLeftAndRightEncoded(left.Data, right.Data));
            await Task.Run(() => InsertResultAndDiffs(id, resultType, diffs));

            var result = await _resultRepo.GetLatestResultByLeftAndRightId(id);

            return result;
        }

        private async Task<Result> GetResultIfExistAndIsValid(int id, Left left, Right right)
        {
            if (left == null || right == null) return null;

            var result = await _resultRepo.GetLatestResultByLeftAndRightId(id);
            var IfExistsAndIsValid = result != null && left.ModifiedDate <= result.ComparationDate && right.ModifiedDate <= result.ComparationDate;
            if (!IfExistsAndIsValid) return null;

            return result;
        }

        private List<byte> Base64Decode(string encoded)
        {
            var bytes = Convert.FromBase64String(encoded);

            var list = new List<byte>();
            bytes.ToList().ForEach(b => list.Add(b));

            return list;
        }

        public (TypeOfResult, Dictionary<int, int>) CompareLeftAndRightEncoded(string leftEncoded, string rightEncoded)
        {
            var left = Base64Decode(leftEncoded);
            var right = Base64Decode(rightEncoded);

            var diffs = new Dictionary<int, int>();

            if (!left.Count.Equals(right.Count))
                return (TypeOfResult.SizeDoNotMatch, diffs);

            var isEqual = left.SequenceEqual(right);
            if (isEqual)
                return (TypeOfResult.Equals, diffs);

            var allDiffs = new Dictionary<int, byte>();

            for (int i = 0; i < left.Count; i++)
            {
                if (!left[i].Equals(right[i]))
                    allDiffs.Add(i, right[i]);
            }

            var keys = allDiffs.Keys.ToList(); keys.Add(-1);
            var diffsLength = allDiffs.Keys.Count;

            for (int i = 0; i < diffsLength; i++)
            {
                var offset = keys[i];
                var startPos = i;

                while (keys[i] + 1 == keys[i + 1]) i++;

                var length = i - startPos + 1;

                diffs.Add(offset, length);
            }

            return (TypeOfResult.ContentDoNotMatch, diffs);
        }

        private void InsertResultAndDiffs(int id, TypeOfResult resultType, IDictionary<int, int> diffs)
        {
            Result result = new Result 
            { 
                ResultType = resultType, 
                LeftId = id,
                RightId = id,
            };

            if (resultType == TypeOfResult.SizeDoNotMatch || resultType == TypeOfResult.Equals)
            {
                _resultRepo.InsertOne(result);
                return;
            }

            if (resultType == TypeOfResult.ContentDoNotMatch)
            {
                var (success, resultId) = _resultRepo.InsertOne(result).Result;

                var diffResult = new List<Diff>();

                foreach (var item in diffs)
                {
                    var offset = item.Key;
                    var length = item.Value;

                    diffResult.Add(new Diff
                    {
                        Offset = offset,
                        Length = length,
                        ResultId = resultId
                    });
                }

                if (success == true)
                    _diffRepo.InsertMany(diffResult);
            }
        }
    }
}
