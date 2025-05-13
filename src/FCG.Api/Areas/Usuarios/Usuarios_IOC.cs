using FCG.Application.Usuarios.Interfaces;
using FCG.Application.Usuarios.Services;
using FCG.Domain.Usuarios.Interfaces;
using FCG.Infrastructure.Usuarios.Repositories;

namespace FCG.Api.Areas.Usuarios;

public static class Usuarios_IOC
{
    public static void AddApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
    }
}
