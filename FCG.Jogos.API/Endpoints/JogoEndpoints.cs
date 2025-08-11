using FCG.Jogos.Application.Jogos.Interfaces;
using FCG.Jogos.Application.Jogos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FCG.Jogos.API.Endpoints;

public static class JogoEndpoints
{
    public static void MapJogoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/jogos")
            .WithTags("Jogos")
            .WithOpenApi();

        // Endpoints para Jogos
        group.MapPost("/", async (IJogoService service, [FromBody] CriarJogoRequest request) =>
        {
            try
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
                {
                    return Results.BadRequest(validationResults);
                }

                var jogo = await service.CriarAsync(request);
                return Results.Created($"/api/jogos/{jogo.Id}", jogo);
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
        .WithName("CriarJogo")
        .WithSummary("Cria um novo jogo")
        .WithOpenApi();

        group.MapGet("/", async (IJogoService service) =>
        {
            try
            {
                var jogos = await service.ObterTodosAsync();
                return Results.Ok(jogos);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterTodosJogos")
        .WithSummary("Lista todos os jogos")
        .WithOpenApi();

        group.MapGet("/{id:guid}", async (IJogoService service, Guid id) =>
        {
            try
            {
                var jogo = await service.ObterPorIdAsync(id);
                if (jogo == null)
                    return Results.NotFound();

                return Results.Ok(jogo);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterJogoPorId")
        .WithSummary("Obtém um jogo por ID")
        .WithOpenApi();

        group.MapGet("/buscar", async (IJogoService service, [FromQuery] BuscarJogosRequest request) =>
        {
            try
            {
                var jogos = await service.BuscarAsync(request);
                return Results.Ok(jogos);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("BuscarJogos")
        .WithSummary("Busca jogos com filtros")
        .WithOpenApi();

        group.MapGet("/categoria/{categoria}", async (IJogoService service, string categoria) =>
        {
            try
            {
                var jogos = await service.ObterPorCategoriaAsync(categoria);
                return Results.Ok(jogos);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterJogosPorCategoria")
        .WithSummary("Obtém jogos por categoria")
        .WithOpenApi();

        group.MapGet("/plataforma/{plataforma}", async (IJogoService service, string plataforma) =>
        {
            try
            {
                var jogos = await service.ObterPorPlataformaAsync(plataforma);
                return Results.Ok(jogos);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterJogosPorPlataforma")
        .WithSummary("Obtém jogos por plataforma")
        .WithOpenApi();

        group.MapGet("/populares", async (IJogoService service, [FromQuery] int quantidade = 10) =>
        {
            try
            {
                var jogos = await service.ObterMaisPopularesAsync(quantidade);
                return Results.Ok(jogos);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterJogosPopulares")
        .WithSummary("Obtém os jogos mais populares")
        .WithOpenApi();

        group.MapGet("/recomendacoes/{usuarioId:guid}", async (IJogoService service, Guid usuarioId) =>
        {
            try
            {
                var jogos = await service.ObterRecomendacoesAsync(usuarioId);
                return Results.Ok(jogos);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ObterRecomendacoes")
        .WithSummary("Obtém recomendações de jogos para um usuário")
        .WithOpenApi();

        group.MapPut("/{id:guid}", async (IJogoService service, Guid id, [FromBody] AtualizarJogoRequest request) =>
        {
            try
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
                {
                    return Results.BadRequest(validationResults);
                }

                var jogo = await service.AtualizarAsync(id, request);
                return Results.Ok(jogo);
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
        .WithName("AtualizarJogo")
        .WithSummary("Atualiza um jogo existente")
        .WithOpenApi();

        group.MapDelete("/{id:guid}", async (IJogoService service, Guid id) =>
        {
            try
            {
                var resultado = await service.ExcluirAsync(id);
                if (resultado)
                    return Results.NoContent();

                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro interno: {ex.Message}");
            }
        })
        .WithName("ExcluirJogo")
        .WithSummary("Exclui um jogo")
        .WithOpenApi();
    }
} 