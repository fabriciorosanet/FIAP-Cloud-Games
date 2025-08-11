using FCG.Jogos.Application.Jogos.Interfaces;
using FCG.Jogos.Application.Jogos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FCG.Jogos.API.Endpoints;

public static class CompraEndpoints
{
    public static void MapCompraEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/compras")
            .WithTags("Compras")
            .WithOpenApi();

        // Endpoints para Compras
        group.MapPost("/", async (ICompraService service, [FromBody] CompraRequest request) =>
        {
            try
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
                {
                    return Results.BadRequest(validationResults);
                }

                var compra = await service.CriarCompraAsync(request);
                return Results.Created($"/api/compras/{compra.Id}", compra);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("CriarCompra")
        .WithSummary("Cria uma nova compra")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (ICompraService service, Guid id) =>
        {
            try
            {
                var compra = await service.ObterCompraPorIdAsync(id);
                if (compra == null)
                    return Results.NotFound();

                return Results.Ok(compra);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterCompraPorId")
        .WithSummary("Obtém uma compra por ID")
        .WithOpenApi();

        group.MapGet("/usuario/{usuarioId:guid}", async (ICompraService service, Guid usuarioId) =>
        {
            try
            {
                var compras = await service.ObterComprasPorUsuarioAsync(usuarioId);
                return Results.Ok(compras);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterComprasPorUsuario")
        .WithSummary("Obtém compras de um usuário")
        .WithOpenApi();

        group.MapGet("/jogo/{jogoId:guid}", async (ICompraService service, Guid jogoId) =>
        {
            try
            {
                var compras = await service.ObterComprasPorJogoAsync(jogoId);
                return Results.Ok(compras);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterComprasPorJogo")
        .WithSummary("Obtém compras de um jogo")
        .WithOpenApi();

        group.MapPut("/{id:guid}/status", async (ICompraService service, Guid id, [FromBody] string novoStatus) =>
        {
            try
            {
                if (string.IsNullOrEmpty(novoStatus))
                    return Results.BadRequest("Status é obrigatório");

                var compra = await service.AtualizarStatusCompraAsync(id, novoStatus);
                return Results.Ok(compra);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("AtualizarStatusCompra")
        .WithSummary("Atualiza o status de uma compra")
        .WithOpenApi();

        group.MapPost("/{id:guid}/cancelar", async (ICompraService service, Guid id) =>
        {
            try
            {
                var resultado = await service.CancelarCompraAsync(id);
                if (resultado)
                    return Results.Ok("Compra cancelada com sucesso");

                return Results.BadRequest("Não foi possível cancelar a compra");
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("CancelarCompra")
        .WithSummary("Cancela uma compra")
        .WithOpenApi();

        group.MapGet("/{id:guid}/codigo-ativacao", async (ICompraService service, Guid id) =>
        {
            try
            {
                var codigo = await service.GerarCodigoAtivacaoAsync(id);
                return Results.Ok(new { CodigoAtivacao = codigo });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("GerarCodigoAtivacao")
        .WithSummary("Gera código de ativação para uma compra")
        .WithOpenApi();

        group.MapPost("/validar-codigo", async (ICompraService service, [FromBody] string codigo) =>
        {
            try
            {
                if (string.IsNullOrEmpty(codigo))
                    return Results.BadRequest("Código é obrigatório");

                var valido = await service.ValidarCodigoAtivacaoAsync(codigo);
                return Results.Ok(new { Valido = valido });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ValidarCodigoAtivacao")
        .WithSummary("Valida um código de ativação")
        .WithOpenApi();
    }
} 