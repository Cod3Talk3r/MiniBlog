using FluentAssertions;
using MiniBlog.Application.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace MiniBlog.IntegrationTests
{
    public class PostsApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PostsApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenAnonymous()
        {
            var response = await _client.GetAsync("/api/posts");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreatePost_ReturnsUnauthorized_WhenNoToken()
        {
            var dto = new CreatePostDto("عنوان تست", "این یک متن تست است");

            var response = await _client.PostAsJsonAsync("/api/posts", dto);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RegisterThenCreatePost_ReturnsCreated()
        {
            var registerDto = new RegisterDto("testuser", "test@example.com", "Password123!");
            var registerResponse = await _client.PostAsJsonAsync("/api/account/register", registerDto);
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var authResult = await registerResponse.Content.ReadFromJsonAsync<AuthResultDto>();
            authResult.Should().NotBeNull();
            authResult!.Token.Should().NotBeNullOrEmpty();

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.Token);

            var createDto = new CreatePostDto("پست از تست", "محتوای پست تستی برای بررسی جریان کامل");
            var createResponse = await _client.PostAsJsonAsync("/api/posts", createDto);

            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}