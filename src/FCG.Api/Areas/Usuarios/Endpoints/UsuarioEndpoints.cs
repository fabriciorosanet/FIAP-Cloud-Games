using System.ComponentModel.DataAnnotations;
using FCG.Application.Usuarios.Interfaces;
using FCG.Application.Usuarios.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Api.Areas.Usuarios.Endpoints;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/usuario/consultar", async (IUsuarioService service, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");
            logger.LogInformation("Requisição recebida para /usuario/consultar");

            try
            {
                var listaUsuario = await service.Consultar();
                if (listaUsuario == null)
                {
                    logger.LogWarning("Nenhum usuário cadastrado");
                    return Results.NotFound($"Nenhum usuário cadastrado");
                }

                return Results.Ok(listaUsuario);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao consultar os usuários.");
                return Results.Problem("Erro ao consultar os usuários.");
            }
        })
        .RequireAuthorization("Admin")
        .WithName("ConsultarUsuarios")
        .WithSummary("Consultar usuarios");

        routes.MapPost("/usuario/adicionar", async (IUsuarioService service, ILoggerFactory loggerFactory, [FromBody] UsuarioViewModel novoUsuario) =>
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

            if(service.ObterUsuario(u => u.Email == novoUsuario.Email) != null)
            {
                logger.LogWarning("Usuário com o email {Email} já cadastrado.", novoUsuario.Email);
                return Results.Conflict($"Usuário com o email {novoUsuario.Email} já cadastrado.");
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
        .RequireAuthorization("Admin")
        .WithName("AdicionarUsuario")
        .WithSummary("Adiciona um novo usuário ao sistema");

        routes.MapPut("/usuario/atualizar", async (IUsuarioService service, ILoggerFactory loggerFactory, [FromBody] UsuarioViewModel usuarioAtualizado) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");
            logger.LogInformation("Requisição recebida para /usuario/atualizar");

            var validationContext = new ValidationContext(usuarioAtualizado);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(usuarioAtualizado, validationContext, validationResults, true))
            {
                logger.LogWarning("Modelo inválido: {@ValidationErrors}", validationResults);
                return Results.BadRequest(validationResults);
            }

            try
            {
                var usuario = await service.Atualizar(usuarioAtualizado);
                if (usuario == null)
                {
                    logger.LogWarning("Usuário com ID {Id} não encontrado.", usuarioAtualizado.Id);
                    return Results.NotFound($"Usuário com ID {usuarioAtualizado.Id} não encontrado.");
                }

                return Results.Ok(usuario);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar o usuário com ID {Id}.", usuarioAtualizado.Id);
                return Results.Problem("Erro interno ao atualizar o usuário.");
            }
        })
        .RequireAuthorization("Admin")   
        .WithName("AtualizarUsuario")
        .WithSummary("Atualiza um usuário existente no sistema");

        routes.MapDelete("/usuario/excluir/{id:guid}", async (IUsuarioService service, ILoggerFactory loggerFactory, Guid id) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");
            logger.LogInformation("Requisição recebida para /usuario/excluir/{id}", id);

            try
            {
                var sucesso = await service.Excluir(id);
                if (!sucesso)
                {
                    logger.LogWarning("Usuário com ID {Id} não encontrado.", id);
                    return Results.NotFound($"Usuário com ID {id} não encontrado.");
                }

                return Results.Ok($"Usuário com ID {id} foi excluído com sucesso.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao excluir o usuário com ID {Id}.", id);
                return Results.Problem("Erro interno ao excluir o usuário.");
            }
        })
        .RequireAuthorization("Admin")
        .WithName("ExcluirUsuario")
        .WithSummary("Exclui um usuário existente no sistema");




    }
}
