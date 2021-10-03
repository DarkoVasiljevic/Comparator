using Comparator.Base;
using Comparator.Dtos;
using Comparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Services
{
    public interface IComparatorService
    {
        Task<bool> InsertOrUpdateLeftAsync(int id, DataRequest data);
        Task<bool> InsertOrUpdateRightAsync(int id, DataRequest data);
        Task<ResultResponse> GetComparatorResultByIdAsync(int id);
        (TypeOfResult, Dictionary<int, int>) CompareLeftAndRight(string left, string right);
    }
}
