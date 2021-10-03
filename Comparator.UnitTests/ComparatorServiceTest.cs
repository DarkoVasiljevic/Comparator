using Comparator.Services;
using Comparator.Repositories;
using Moq;
using System;
using Xunit;
using AutoMapper;
using Comparator.Base;
using System.Collections.Generic;
using Comparator.Dtos;
using Comparator.Models;
using System.Threading.Tasks;

namespace Comparator.UnitTests
{
    public class ComparatorServiceTest
    {
        private readonly Mock<ILeftRepo> _leftRepoMock = new Mock<ILeftRepo>();
        private readonly Mock<IRightRepo> _rightRepoMock = new Mock<IRightRepo>();
        private readonly Mock<IResultRepo> _resultRepoMock = new Mock<IResultRepo>();
        private readonly Mock<IDiffRepo> _diffRepoMock = new Mock<IDiffRepo>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IComparatorService> _serviceMock = new Mock<IComparatorService>();

        private readonly IComparatorService _comparatorService;

        public ComparatorServiceTest()
        {
            _comparatorService = new ComparatorService(
                _leftRepoMock.Object, _rightRepoMock.Object, _resultRepoMock.Object, _diffRepoMock.Object, _mapperMock.Object);
        }

        [Theory]
        [InlineData("AAABBB", "AABB")]
        [InlineData("AABB", "AAABBB")]
        public void CompareLeftAndRight_WhenStringsAreWithDifferentSizes_ShouldReturnSizeDoNotMatch(string left, string right)
        {
            // Arrange

            // Act
            var (resultType, _) = _comparatorService.CompareLeftAndRight(left, right);

            //Assert
            Assert.Equal(TypeOfResult.SizeDoNotMatch, resultType);
        }

        [Theory]
        [InlineData("AAABBB", "AABB")]
        [InlineData("AABB", "AAABBB")]
        public void CompareLeftAndRight_WhenStringsAreWithDifferentSizes_ShouldReturnEmptyDictionary(string left, string right)
        {
            // Arrange

            // Act
            var (_, diffs) = _comparatorService.CompareLeftAndRight(left, right);

            //Assert
            Assert.True(diffs.Count == 0);
        }

        [Fact]
        public void CompareLeftAndRight_WhenStringsAreDifferentButWithEqualSize_ShouldReturnContentDoNotMatch()
        {
            // Arrange
            string left = "AAABBB";
            string right = "AABBCC";

            // Act
            var (resultType, _) = _comparatorService.CompareLeftAndRight(left, right);

            //Assert
            Assert.Equal(TypeOfResult.ContentDoNotMatch, resultType);
        }

        [Theory]
        [InlineData("AAABBB", "AABBAA")]
        [InlineData("AABBAA", "AAABBB")]
        public void CompareLeftAndRight_WhenStringsAreDifferentButWithEqualSize_ShouldReturnDictionaryWithDiffs(string left, string right)
        {
            // Arrange
            var diffsMock = new Dictionary<int, int>();
            diffsMock.Add(2, 1);
            diffsMock.Add(4, 2);

            // Act
            var (_, diffs) = _comparatorService.CompareLeftAndRight(left, right);

            //Assert
            Assert.True(diffsMock.Count == 2);
            Assert.Equal(diffs, diffsMock);
        }

        [Fact]
        public void CompareLeftAndRight_WhenStringsAreEqualAndWithEqualSize_ShouldReturnEqual()
        {
            // Arrange
            string left = "AAABBB";
            string right = "AAABBB";

            // Act
            var (resultType, _) = _comparatorService.CompareLeftAndRight(left, right);

            //Assert
            Assert.Equal(TypeOfResult.Equals, resultType);
        }

        [Fact]
        public void CompareLeftAndRight_WhenStringsAreEqualAndWithEqualSize_ShouldReturnEmptyDictionary()
        {
            // Arrange
            string left = "AAABBB";
            string right = "AAABBB";

            // Act
            var (_, diffs) = _comparatorService.CompareLeftAndRight(left, right);

            //Assert
            Assert.True(diffs.Count == 0);
        }

        [Fact]
        public async Task GetComparatorResultByIdAsync_WhenLeftStringDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _leftRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _comparatorService.GetComparatorResultByIdAsync(It.IsAny<int>());

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetComparatorResultByIdAsync_WhenRightStringDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _rightRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await _comparatorService.GetComparatorResultByIdAsync(It.IsAny<int>());

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task InsertOrUpdateLeftAsync_WhenLeftStringDoesNotExist_ShouldInsertDataAndRetunTrue()
        {
            // Arrange
            int id = 1;
            DataRequest data = new DataRequest { Data = "AABBCC" };
            Left left = new Left
            { 
                Id = id,
                Data = data.Data
            };

            _leftRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => null);
            _mapperMock.Setup(e => e.Map<Left>(data)).Returns(left);
            _leftRepoMock.Setup(e => e.InsertOneWithCustomId(id, left)).ReturnsAsync(true);

            // Act
            var result = await _comparatorService.InsertOrUpdateLeftAsync(id, data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task InsertOrUpdateLeftAsync_WhenLeftStringExist_ShouldUpdateDataAndRetunTrue()
        {
            // Arrange
            int id = 1;
            DataRequest data = new DataRequest { Data = "AABBCC" };
            Left left = new Left
            {
                Id = id,
                Data = data.Data
            };

            _leftRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => left);
            _mapperMock.Setup(e => e.Map<Left>(data)).Returns(left);
            _leftRepoMock.Setup(e => e.UpdateOne(id, left)).ReturnsAsync(true);

            // Act
            var result = await _comparatorService.InsertOrUpdateLeftAsync(id, data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task InsertOrUpdateRightAsync_WhenRightStringDoesNotExist_ShouldInsertDataAndRetunTrue()
        {
            // Arrange
            int id = 1;
            DataRequest data = new DataRequest { Data = "AABBCC" };
            Right right = new Right
            {
                Id = id,
                Data = data.Data
            };

            _rightRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => null);
            _mapperMock.Setup(e => e.Map<Right>(data)).Returns(right);
            _rightRepoMock.Setup(e => e.InsertOneWithCustomId(id, right)).ReturnsAsync(true);

            // Act
            var result = await _comparatorService.InsertOrUpdateRightAsync(id, data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task InsertOrUpdateRightAsync_WhenRightStringExist_ShouldUpdateDataAndRetunTrue()
        {
            // Arrange
            int id = 1;
            DataRequest data = new DataRequest { Data = "AABBCC" };
            Right right = new Right
            {
                Id = id,
                Data = data.Data
            };

            _rightRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => right);
            _mapperMock.Setup(e => e.Map<Right>(data)).Returns(right);
            _rightRepoMock.Setup(e => e.UpdateOne(id, right)).ReturnsAsync(true);

            // Act
            var result = await _comparatorService.InsertOrUpdateRightAsync(id, data);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetComparatorResultByIdAsync_WhenLeftAndRightStringsExistAndResultIsValid_ShouldReturnResult()
        {
            // Arrange
            int id = 1;
            string data = "AABBCC";
            Left left = new Left
            {
                Id = id,
                Data = data,
                ModifiedDate = DateTime.Now.AddMinutes(-7),
            };
            Right right = new Right
            {
                Id = id,
                Data = data,
                ModifiedDate = DateTime.Now.AddMinutes(-7),
            };
            Result result = new Result
            {
                Id = id,
                ResultType = TypeOfResult.Equals,
                ComparationDate = DateTime.Now,
                LeftId = id,
                RightId = id
            };
            ResultResponse response = new ResultResponse
            {
                ResultType = "Equals",
                Diffs = new List<DiffResponse>()
            };

            // GetResultIfExistAndIsValid
            _leftRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => left);
            _rightRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => right);
            _resultRepoMock.Setup(e => e.GetLatestResultByLeftAndRightId(It.IsAny<int>())).ReturnsAsync(result);
            _mapperMock.Setup(e => e.Map<ResultResponse>(result)).Returns(response);

            // Act
            var expected = await _comparatorService.GetComparatorResultByIdAsync(id);

            //Assert
            Assert.Equal(expected.ResultType, response.ResultType);
        }

        [Fact]
        public async Task GetComparatorResultByIdAsync_WhenLeftAndRightStringsExistAndResultIsNotValid_ShouldCompareStringsAndReturnResult()
        {
            // Arrange
            int id = 1;
            string leftData  = "AABBCC";
            string rightData = "ABBBCC";

            Left left = new Left
            {
                Id = id,
                Data = leftData,
                ModifiedDate = DateTime.Now.AddMinutes(+7),
            };
            Right right = new Right
            {
                Id = id,
                Data = rightData,
                ModifiedDate = DateTime.Now.AddMinutes(-7),
            };

            Result result = new Result
            {
                Id = id,
                ResultType = TypeOfResult.ContentDoNotMatch,
                ComparationDate = DateTime.Now,
                LeftId = id,
                RightId = id
            };

            int offset = 1, length = 1;
            ResultResponse response = new ResultResponse
            {
                ResultType = "ContentDoNotMatch",
                Diffs = new List<DiffResponse> { new DiffResponse { Offset = offset, Length = length } }
            };

            // GetResultIfExistAndIsValid
            _leftRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => left);
            _rightRepoMock.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(() => right);
            _resultRepoMock.Setup(e => e.GetLatestResultByLeftAndRightId(It.IsAny<int>())).ReturnsAsync(() => null);

            // CompareAndInsertResult
            var diffs = new Dictionary<int, int> { { offset, length } };
            _serviceMock.Setup(e => e.CompareLeftAndRight(left.Data, right.Data)).Returns((TypeOfResult.ContentDoNotMatch, diffs));

            // InsertResultAndDiffs
            _resultRepoMock.Setup(e => e.InsertOne(result)).ReturnsAsync((true, id));
            var diffResult = new List<Diff>();
            diffResult.Add(new Diff { Offset = offset, Length = length });
            _diffRepoMock.Setup(e => e.InsertMany(diffResult)).ReturnsAsync(true);

            // CompareAndInsertResult
            _resultRepoMock.Setup(e => e.GetLatestResultByLeftAndRightId(It.IsAny<int>())).ReturnsAsync(() => result);

            _mapperMock.Setup(e => e.Map<ResultResponse>(result)).Returns(response);

            // Act
            var expected = await _comparatorService.GetComparatorResultByIdAsync(id);

            //Assert
            Assert.Equal(expected.ResultType, response.ResultType);
            Assert.Equal(expected.Diffs, response.Diffs);
        }

    }
}
