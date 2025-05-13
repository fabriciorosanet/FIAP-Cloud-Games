using System.ComponentModel.DataAnnotations;
using FCG.Application.Usuarios.Interfaces;
using FCG.Application.Usuarios.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Api.Areas.Usuarios.Endpoints;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/usuario/adicionar", async (IUsuarioService service, ILoggerFactory loggerFactory, [FromBody]UsuarioViewModel novoUsuario) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");
            logger.LogInformation("Requisição recebida para /usuario/adicionar");

            var validationContext = new ValidationContext(novoUsuario);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(novoUsuario, validationContext, validationResults, true))
            {
                logger.LogWarning("Modelo inválido: {@ValidationErrors}", validationResults);
                return Results.BadRequest(validationResults);
            }
            
            try
            {
                var usuario = await service.Adicionar(novoUsuario);
                return Results.Ok(usuario);
            } 
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar o usuário.");
                return Results.Problem("Erro interno ao adicionar o usuário.");
            }

        })
        .WithName("AdicionarUsuario")
        .WithSummary("Adiciona um novo usuário ao sistema");
    }
}
