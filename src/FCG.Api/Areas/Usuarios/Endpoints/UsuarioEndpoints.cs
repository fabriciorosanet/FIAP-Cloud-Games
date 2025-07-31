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

            try
            {
                var listaUsuario = await service.Consultar();
                if (listaUsuario == null || !listaUsuario.Any())
                {
                    return Results.NotFound("Nenhum usuário cadastrado");
                }

                var response = listaUsuario.Select(u => new UsuarioResponse
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    TipoUsuario = u.TipoUsuario
                });

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao consultar os usuários.");
                return Results.Problem("Erro ao consultar os usuários.");
            }
        })
        .WithName("ConsultarUsuarios")
        .WithSummary("Consultar usuários");

        routes.MapPost("/usuario/adicionar", async (IUsuarioService service, ILoggerFactory loggerFactory, [FromBody] CriarUsuarioRequest novoUsuario) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");

            var validationContext = new ValidationContext(novoUsuario);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(novoUsuario, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            try
            {
                var usuario = await service.Adicionar(new CriarUsuarioRequest
                {
                    Nome = novoUsuario.Nome,
                    Email = novoUsuario.Email,
                    Senha = novoUsuario.Senha,
                    TipoUsuario = novoUsuario.TipoUsuario
                });

                var response = new UsuarioResponse
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    TipoUsuario = usuario.TipoUsuario
                };

                return Results.Created($"/usuario/{response.Id}", response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar o usuário.");
                return Results.Problem("Erro interno ao adicionar o usuário.");
            }
        })
        .WithName("AdicionarUsuario")
        .WithSummary("Adiciona um novo usuário");

        routes.MapPut("/usuario/atualizar/{id:guid}", async (IUsuarioService service, ILoggerFactory loggerFactory, Guid id, [FromBody] AtualizarUsuarioRequest request) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");

            var usuarioAtualizado = new AtualizarUsuarioRequest
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Senha,
                TipoUsuario = request.TipoUsuario
            };

            var validationContext = new ValidationContext(usuarioAtualizado);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(usuarioAtualizado, validationContext, validationResults, true))
            {
                return Results.BadRequest(validationResults);
            }

            try
            {
                var usuario = await service.Atualizar(id, usuarioAtualizado);
                if (usuario == null)
                {
                    return Results.NotFound($"Usuário com ID {id} não encontrado.");
                }

                var response = new UsuarioResponse
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    TipoUsuario = usuario.TipoUsuario
                };

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar o usuário com ID {Id}.", id);
                return Results.Problem("Erro interno ao atualizar o usuário.");
            }
        })
        .WithName("AtualizarUsuarioPorId")
        .WithSummary("Atualiza um usuário existente");

        routes.MapDelete("/usuario/excluir/{id:guid}", async (IUsuarioService service, ILoggerFactory loggerFactory, Guid id) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");

            try
            {
                var sucesso = await service.Excluir(id);
                if (!sucesso)
                {
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
        .WithName("ExcluirUsuario")
        .WithSummary("Exclui um usuário existente");
    }
}
