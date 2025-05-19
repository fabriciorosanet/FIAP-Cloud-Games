using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FCG.Api.Areas.Auth;
using FCG.Application.Usuarios.Interfaces;
using FCG.Application.Authentication.Interface;
using FCG.Domain.Usuarios.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;


namespace FCG.Tests.Authentication
{
    public class AuthEndpointTests
    {
        private readonly HttpClient _client;

        public AuthEndpointTests()
        {
            var mockUser = new Mock<IUsuarioService>();
            mockUser
                .Setup(s => s.AutenticarUsuarioAsync("u@u.com", "pwd"))
                .ReturnsAsync(new Usuario
                {
                    Id         = System.Guid.NewGuid(),
                    Nome       = "Teste",
                    Email      = "u@u.com",
                    Senha      = "irrelevante",
                    TipoUsuario= TipoUsuario.Usuario
                });

            var mockToken = new Mock<ITokenService>();
            mockToken
                .Setup(s => s.GenerateToken(It.IsAny<Usuario>()))
                .Returns(("fake-token", System.DateTime.UtcNow.AddHours(1)));

            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddSingleton(mockUser.Object);
                    services.AddSingleton(mockToken.Object);
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapAuthEndpoints();
                    });
                });

            var server = new TestServer(builder);
            _client = server.CreateClient();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            var response = await _client.PostAsJsonAsync("/api/auth/login", new { Email = "u@u.com", Password = "pwd" });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal("fake-token", json.GetProperty("access_token").GetString());
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            
            var response = await _client.PostAsJsonAsync(
                "/api/auth/login",
                new { Email = "x@x.com", Password = "bad" }
            );

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
