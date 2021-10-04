using Comparator.Services;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using Comparator.Dtos;
using System.Threading.Tasks;
using Comparator.Controllers.v1;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Comparator.UnitTests
{
    public class ComparatorControllerTest
    {

        private readonly Mock<ComparatorController> _controllerMock = new Mock<ComparatorController>();
        private readonly Mock<IComparatorService> _serviceMock = new Mock<IComparatorService>();

        private readonly ComparatorController _comparatorController;

        public ComparatorControllerTest()
        {
            _comparatorController = new ComparatorController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetComparatorResultById_WhenResultDoesNotExist_ShouldReturnStatusCode404NotFound()
        {
            // Arrange
            _serviceMock.Setup(e => e.GetComparatorResultByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var actionResult = await _comparatorController.GetComparatorResultById(It.IsAny<int>());

            //Assert
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetComparatorResultById_WhenResultDoExist_ShouldReturnStatusCode200OK()
        {
            // Arrange
            ResultResponse response = new ResultResponse
            {
                ResultType = "Equals",
                Diffs = new List<DiffResponse>()
            };
            _serviceMock.Setup(e => e.GetComparatorResultByIdAsync(It.IsAny<int>())).ReturnsAsync(() => response);

            // Act
            var actionResult = await _comparatorController.GetComparatorResultById(It.IsAny<int>());

            //Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public async Task PutLeft_WhenInsertOrUpdateLeftSucceed_ShouldReturnStatusCode201Created()
        {
            // Arrange
            DataRequest request = new DataRequest
            {
                Data = "YXV0b21vYmls"
            };
            _serviceMock.Setup(e => e.InsertOrUpdateLeftAsync(It.IsAny<int>(), request)).ReturnsAsync(() => true);

            // Act
            var actionResult = await _comparatorController.PutLeft(It.IsAny<int>(), request);

            //Assert
            Assert.IsType<CreatedResult>(actionResult);
        }

        [Fact]
        public async Task PutLeft_WhenInsertOrUpdateLeftNotSucceed_ShouldReturnStatusCode400BadRequest()
        {
            // Arrange
            DataRequest request = new DataRequest
            {
                Data = "string"
            };
            _serviceMock.Setup(e => e.InsertOrUpdateLeftAsync(It.IsAny<int>(), It.IsAny<DataRequest>())).ReturnsAsync(() => false);

            // Act
            var actionResult = await _comparatorController.PutLeft(It.IsAny<int>(), request);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task PutLeft_WhenTryInsertOrUpdateNullLeft_ShouldReturnStatusCode400BadRequest()
        {
            // Arrange
            DataRequest request = new DataRequest
            {
                Data = null
            };

            // Act
            var actionResult = await _comparatorController.PutLeft(It.IsAny<int>(), request);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);

        }

        [Fact]
        public async Task PutRight_WhenInsertOrUpdateRightSucceed_ShouldReturnStatusCode201Created()
        {
            // Arrange
            DataRequest request = new DataRequest
            {
                Data = "YXV0b21vYmls"
            };
            _serviceMock.Setup(e => e.InsertOrUpdateRightAsync(It.IsAny<int>(), request)).ReturnsAsync(() => true);

            // Act
            var actionResult = await _comparatorController.PutRight(It.IsAny<int>(), request);

            //Assert
            Assert.IsType<CreatedResult>(actionResult);
        }

        [Fact]
        public async Task PutRight_WhenInsertOrUpdateRightNotSucceed_ShouldReturnStatusCode400BadRequest()
        {
            // Arrange
            DataRequest request = new DataRequest
            {
                Data = "string"
            };
            _serviceMock.Setup(e => e.InsertOrUpdateRightAsync(It.IsAny<int>(), It.IsAny<DataRequest>())).ReturnsAsync(() => false);

            // Act
            var actionResult = await _comparatorController.PutRight(It.IsAny<int>(), request);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task PutRight_WhenTryInsertOrUpdateNullRight_ShouldReturnStatusCode400BadRequest()
        {
            // Arrange
            DataRequest request = new DataRequest
            {
                Data = null
            };

            // Act
            var actionResult = await _comparatorController.PutRight(It.IsAny<int>(), request);

            //Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);

        }
    }
}
