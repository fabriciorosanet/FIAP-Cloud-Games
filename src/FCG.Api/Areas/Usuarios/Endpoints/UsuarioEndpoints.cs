using System.ComponentModel.DataAnnotations;
using FCG.Application.Usuarios.Interfaces;
using FCG.Application.Usuarios.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using System.Linq; // Adicionar para .Any() e .Select()

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
                    logger.LogWarning("Nenhum usuário cadastrado.");
                    return Results.NotFound("Nenhum usuário cadastrado.");
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
        .RequireAuthorization("Admin")
        .WithName("ConsultarTodosUsuarios")
        .WithSummary("Consultar todos os usuários");

        routes.MapPost("/usuario/adicionar", async (IUsuarioService service, ILoggerFactory loggerFactory, [FromBody] CriarUsuarioRequest novoUsuario) =>
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

            if (service.ObterUsuario(u => u.Email == novoUsuario.Email).Result != null)
            {
                logger.LogWarning("Usuário com o email {Email} já cadastrado.", novoUsuario.Email);
                return Results.Conflict($"Usuário com o email {novoUsuario.Email} já cadastrado.");
            }

            try
            {
                var usuarioAdicionado = await service.Adicionar(novoUsuario);

                var response = new UsuarioResponse
                {
                    Id = usuarioAdicionado.Id,
                    Nome = usuarioAdicionado.Nome,
                    Email = usuarioAdicionado.Email,
                    TipoUsuario = usuarioAdicionado.TipoUsuario
                };

                return Results.Created($"/usuario/{response.Id}", response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao adicionar o usuário.");
                return Results.Problem("Erro interno ao adicionar o usuário.");
            }
        })
        .AllowAnonymous()
        .WithName("AdicionarUsuario")
        .WithSummary("Adiciona um novo usuário ao sistema");

        routes.MapPut("/usuario/atualizar/{id:guid}", async (IUsuarioService service, ILoggerFactory loggerFactory, Guid id, [FromBody] AtualizarUsuarioRequest request) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");
            logger.LogInformation("Requisição recebida para /usuario/atualizar/{id}", id);

            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            {
                logger.LogWarning("Modelo inválido: {@ValidationErrors}", validationResults);
                return Results.BadRequest(validationResults);
            }

            try
            {
                var existingUserById = await service.ConsultarUsuario(id);
                if (existingUserById == null)
                {
                    logger.LogWarning("Usuário com ID {Id} não encontrado para atualização.", id);
                    return Results.NotFound($"Usuário com ID {id} não encontrado.");
                }

                // Verifica se o email está sendo alterado e se o novo email já pertence a outro usuário
                if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != existingUserById.Email)
                {
                    var userWithSameEmail = await service.ObterUsuario(u => u.Email == request.Email);
                    if (userWithSameEmail != null && userWithSameEmail.Id != id)
                    {
                        logger.LogWarning("Email {Email} já está em uso por outro usuário.", request.Email);
                        return Results.Conflict($"O email '{request.Email}' já está cadastrado para outro usuário.");
                    }
                }

                var usuarioAtualizado = await service.Atualizar(id, request);
                if (usuarioAtualizado == null)
                {
                    logger.LogError("Falha inesperada ao atualizar usuário com ID {Id}.", id);
                    return Results.Problem("Erro interno ao atualizar o usuário.");
                }

                var response = new UsuarioResponse
                {
                    Id = usuarioAtualizado.Id,
                    Nome = usuarioAtualizado.Nome,
                    Email = usuarioAtualizado.Email,
                    TipoUsuario = usuarioAtualizado.TipoUsuario
                };

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar o usuário com ID {Id}.", id);
                return Results.Problem("Erro interno ao atualizar o usuário.");
            }
        })
        .RequireAuthorization("Admin")
        .WithName("AtualizarUsuario")
        .WithSummary("Atualiza um usuário existente no sistema por ID");

        routes.MapGet("/usuario/consultarUsuario/{id:guid}", async (IUsuarioService service, ILoggerFactory loggerFactory, Guid id) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");
            logger.LogInformation("Requisição recebida para /usuario/consultarUsuario/{id}", id);

            try
            {
                var usuario = await service.ConsultarUsuario(id);
                if (usuario == null)
                {
                    logger.LogWarning("Usuário com ID {Id} não encontrado.", id);
                    return Results.NotFound($"Usuário com ID {id} não encontrado.");
                }

                return Results.Ok(usuario);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao consultar o usuário com ID {Id}.", id);
                return Results.Problem("Erro interno ao consultar o usuário.");
            }
        })
        .RequireAuthorization("Admin")
        .WithName("ConsultarUsuarioPorId")
        .WithSummary("Consultar usuário específico por ID");

        routes.MapDelete("/usuario/excluir/{id:guid}", async (IUsuarioService service, ILoggerFactory loggerFactory, Guid id) =>
        {
            var logger = loggerFactory.CreateLogger("UsuarioEndpoint");
            logger.LogInformation("Requisição recebida para /usuario/excluir/{id}", id);

            try
            {
                var sucesso = await service.Excluir(id);
                if (!sucesso)
                {
                    logger.LogWarning("Usuário com ID {Id} não encontrado para exclusão.", id);
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
        .WithSummary("Exclui um usuário existente no sistema por ID");
    }
}