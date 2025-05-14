using System.Text;
using FCG.Application.Authentication;
using FCG.Domain.Usuarios.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FCG.Api.Configurations.Authentication;

public static class JwtConfiguration
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration cfg)
    {
        services.Configure<JwtSettings>(cfg.GetSection("Jwt"));
        var settings = cfg.GetSection("Jwt").Get<JwtSettings>()!;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer   = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("User",  p => p.RequireRole(TipoUsuario.Usuario.ToString(),
                TipoUsuario.Administrador.ToString()));
            options.AddPolicy("Admin", p => p.RequireRole(TipoUsuario.Administrador.ToString()));
        });

        return services;
    }
}