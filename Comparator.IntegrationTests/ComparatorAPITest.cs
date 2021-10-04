using Comparator.Database;
using Comparator.Dtos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Comparator.IntegrationTests
{

    public class ComparatorAPITest : IClassFixture<TestFactory>, IDisposable
    {
        protected readonly TestFactory _factory;
        protected readonly HttpClient _client;

        public ComparatorAPITest(TestFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
        }

        [Theory]
        [InlineData(int.MaxValue)]
        public async Task GetComparatorResultById_WhenLeftOrRightDoesNotExist_ShouldReturnStatusCodeNotFound(int id)
        {
            // Arrange
            var url = $"v1/diff/{id}";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/1/left")]
        public async Task PutLeftEqual_WhenNotExist_ShouldBeCreatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "AQABAQ==",
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/2/left")]
        public async Task PutLeftSizeDoNotMatch_WhenNotExist_ShouldBeCreatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "YXV0bw==", // auto
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/3/left")]
        public async Task PutLeftContentDoNotMatch_WhenNotExist_ShouldBeCreatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "bW90b3M=", // motos
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/1/right")]
        public async Task PutRightEqual_WhenNotExist_ShouldBeCreatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "AQABAQ==",
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/2/right")]
        public async Task PutRightSizeDoNotMatch_WhenNotExist_ShouldBeCreatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "bW90b3Jz", // motors
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/3/right")]
        public async Task PutRightContentDoNotMatch_WhenNotExist_ShouldBeCreatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "YXV0b3M=", // autos
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/1")]
        public async Task GetComparatorResultById_WhenLeftAndRightAreEqual_ShouldReturnEqualAndStatusCodeOK(string url)
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseResult = JsonConvert.DeserializeObject<ResultResponse>(responseString);
            Assert.Contains("Equal", responseResult.ResultType);
        }

        [Theory]
        [InlineData("v1/diff/2")]
        public async Task GetComparatorResultById_WhenLeftAndRightExistButWithDifferentSize_ShouldReturnSizeDoNotMatchAndStatusCodeOK(string url)
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseResult = JsonConvert.DeserializeObject<ResultResponse>(responseString);
            Assert.Contains("SizeDoNotMatch", responseResult.ResultType);
        }

        [Theory]
        [InlineData("v1/diff/3")]
        public async void GetComparatorResultById_WhenLeftAndRightExistAndNotEqual_ShouldReturnContentDoNotMatchAndStatusCodeOK(string url)
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseResult = JsonConvert.DeserializeObject<ResultResponse>(responseString);
            Assert.Contains("ContentDoNotMatch", responseResult.ResultType);
        }

        [Theory]
        [InlineData("v1/diff/4/left")]
        public async Task PutLeft_WhenNotExist_ShouldBeCreatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "YXV0b3M=", // autos
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/4/left")]
        public async Task PutLeft_WhenExist_ShouldBeUpdatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "YXV0bw==", // auto
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/4/right")]
        public async Task PutRight_WhenNotExist_ShouldBeCreatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "YXV0b3M=", // autos
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("v1/diff/4/right")]
        public async Task PutRight_WhenExist_ShouldBeUpdatedAndReturnStatusCode201(string url)
        {
            // Arrange
            DataRequest data = new DataRequest
            {
                Data = "YXV0bw==", // auto
            };

            // Act
            var contents = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, contents);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
