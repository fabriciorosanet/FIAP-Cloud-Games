using System.Text.Json;
using FCG.Application.Authentication.Interface;
using FCG.Application.Usuarios.Interfaces;

namespace FCG.Api.Areas.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", async (
                LoginRequest req,
                IUsuarioService usuarioSvc,
                ITokenService tokenSvc,
                HttpContext    context,
                CancellationToken ct) =>
            {
                var usuario = await usuarioSvc.AutenticarUsuarioAsync(req.Email, req.Password);
                if (usuario is null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var (token, exp) = tokenSvc.GenerateToken(usuario);
                var responseObj = new { access_token = token, expires_at = exp };

                var json = JsonSerializer.Serialize(responseObj);
                context.Response.StatusCode = StatusCodes.Status200OK;
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(json, ct);
            })
            .AllowAnonymous()
            .WithSummary("Autenticacao");
    }
}

public record LoginRequest(string Email, string Password);