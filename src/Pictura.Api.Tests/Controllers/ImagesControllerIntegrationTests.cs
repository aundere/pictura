using System.Net;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Pictura.Api.Tests.Infrastructure;

namespace Pictura.Api.Tests.Controllers
{
    public class ImagesControllerIntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory<Program> _factory;
        
        public ImagesControllerIntegrationTests(TestWebApplicationFactory<Program> factory)
        {
            this._client = factory.CreateClient();
            this._factory = factory;
        }

        [Fact]
        public async Task GetImage_ReturnsImages_WhenOneImageExists()
        {
            // Arrange
            this._factory.SeedData(this._factory.Services);
            
            // Act
            var response = await this._client.GetAsync("/images/");

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var actualJson = JToken.Parse(content);

            actualJson.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetImage_ReturnsImage_WhenImageExists()
        {
            // Arrange
            const int imageId = 1;

            this._factory.SeedData(this._factory.Services);
            
            // Act
            var response = await this._client.GetAsync($"/images/{imageId}");

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var actualJson = JToken.Parse(content);
            var expectedJson = JToken.Parse($@"{{
                ""id"": {imageId},
                ""url"": ""example.com"",
                ""tags"": [ ""test"", ""tag"" ]
            }}");

            actualJson.Should().BeEquivalentTo(expectedJson);
        }

        [Fact]
        public async Task GetImage_ReturnsNotFound_WhenImageDoesNotExist()
        {
            // Arrange
            const int imageId = 999; // Non-existent image ID
            
            this._factory.SeedData(this._factory.Services);
            
            // Act
            var response = await this._client.GetAsync($"/images/{imageId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostImage_CreatesImage_WhenValidRequest()
        {
            // Arrange
            var requestContent = new StringContent(
                @"{ ""url"": ""https://newimage.com"", ""tags"": [""newtag""] }",
                System.Text.Encoding.UTF8,
                "application/json");

            // Act
            var response = await this._client.PostAsync("/images", requestContent);

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var actualJson = JToken.Parse(content);
            actualJson.Should().HaveCount(3); // id, url, tags
        }
        
        [Fact]
        public async Task DeleteImage_ReturnsNoContent_WhenImageExists()
        {
            // Arrange
            const int imageId = 1;
            
            this._factory.SeedData(this._factory.Services);

            // Act
            var response = await this._client.DeleteAsync($"/images/{imageId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public async Task GetRandomImage_ReturnsRandomImage_WhenImagesExist()
        {
            // Arrange
            this._factory.SeedData(this._factory.Services);
            
            // Act
            var response = await this._client.GetAsync("/images/random?tags=test");

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var actualJson = JToken.Parse(content);
            actualJson.Should().HaveCount(3); // id, url, tags
        }

        [Fact]
        public async Task GetRandomImage_ReturnsNotFound_WhenNoImagesMatchTags()
        {
            // Arrange
            this._factory.SeedData(this._factory.Services);

            // Act
            var response = await this._client.GetAsync("/images/random?tags=nonexistent");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
