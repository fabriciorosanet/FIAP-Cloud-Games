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
            CancellationToken ct) =>
        {
            var usuario = await usuarioSvc.AutenticarUsuarioAsync(req.Email, req.Password);
            if (usuario is null) return Results.Unauthorized();

            var (token, exp) = tokenSvc.GenerateToken(usuario);
            return Results.Ok(new { access_token = token, expires_at = exp });
        })
        .AllowAnonymous()
        .WithSummary("Autenticacao");
    }
}

public record LoginRequest(string Email, string Password);