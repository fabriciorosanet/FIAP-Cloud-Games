using FCG.Jogos.Application.Jogos.Interfaces;
using FCG.Jogos.Application.Jogos.ViewModels;
using FCG.Jogos.Domain.Jogos.Entities;
using FCG.Jogos.Domain.Jogos.Interfaces;

namespace FCG.Jogos.Application.Jogos.Services;

public class CompraService : ICompraService
{
    private readonly ICompraRepository _compraRepository;
    private readonly IJogoRepository _jogoRepository;

    public CompraService(ICompraRepository compraRepository, IJogoRepository jogoRepository)
    {
        _compraRepository = compraRepository;
        _jogoRepository = jogoRepository;
    }

    public async Task<CompraResponse> CriarCompraAsync(CompraRequest request)
    {
        var jogo = await _jogoRepository.ObterPorIdAsync(request.JogoId);
        if (jogo == null)
            throw new InvalidOperationException("Jogo não encontrado");

        if (jogo.Estoque <= 0)
            throw new InvalidOperationException("Jogo sem estoque disponível");

        var compra = new Compra
        {
            UsuarioId = request.UsuarioId,
            JogoId = request.JogoId,
            PrecoPago = jogo.Preco,
            DataCompra = DateTime.UtcNow,
            Status = StatusCompra.Pendente,
            CodigoAtivacao = GerarCodigoAtivacao()
        };

        var compraCriada = await _compraRepository.AdicionarAsync(compra);

        // Atualizar estoque do jogo
        jogo.Estoque--;
        await _jogoRepository.AtualizarAsync(jogo);

        return MapearParaResponse(compraCriada);
    }

    public async Task<CompraResponse?> ObterCompraPorIdAsync(Guid id)
    {
        var compra = await _compraRepository.ObterPorIdAsync(id);
        return compra != null ? MapearParaResponse(compra) : null;
    }

    public async Task<IEnumerable<CompraResponse>> ObterComprasPorUsuarioAsync(Guid usuarioId)
    {
        var compras = await _compraRepository.ObterPorUsuarioAsync(usuarioId);
        return compras.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<CompraResponse>> ObterComprasPorJogoAsync(Guid jogoId)
    {
        var compras = await _compraRepository.ObterPorJogoAsync(jogoId);
        return compras.Select(MapearParaResponse);
    }

    public async Task<CompraResponse> AtualizarStatusCompraAsync(Guid id, string novoStatus)
    {
        var compra = await _compraRepository.ObterPorIdAsync(id);
        if (compra == null)
            throw new InvalidOperationException("Compra não encontrada");

        if (Enum.TryParse<StatusCompra>(novoStatus, out var status))
        {
            compra.Status = status;
            compra.DataAtualizacao = DateTime.UtcNow;
        }
        else
        {
            throw new InvalidOperationException("Status inválido");
        }

        var compraAtualizada = await _compraRepository.AtualizarAsync(compra);
        return MapearParaResponse(compraAtualizada);
    }

    public async Task<bool> CancelarCompraAsync(Guid id)
    {
        var compra = await _compraRepository.ObterPorIdAsync(id);
        if (compra == null)
            return false;

        if (compra.Status == StatusCompra.Cancelada)
            return false;

        compra.Status = StatusCompra.Cancelada;
        compra.DataAtualizacao = DateTime.UtcNow;

        await _compraRepository.AtualizarAsync(compra);

        // Restaurar estoque do jogo
        var jogo = await _jogoRepository.ObterPorIdAsync(compra.JogoId);
        if (jogo != null)
        {
            jogo.Estoque++;
            await _jogoRepository.AtualizarAsync(jogo);
        }

        return true;
    }

    public async Task<string> GerarCodigoAtivacaoAsync(Guid compraId)
    {
        var compra = await _compraRepository.ObterPorIdAsync(compraId);
        if (compra == null)
            throw new InvalidOperationException("Compra não encontrada");

        if (string.IsNullOrEmpty(compra.CodigoAtivacao))
        {
            compra.CodigoAtivacao = GerarCodigoAtivacao();
            await _compraRepository.AtualizarAsync(compra);
        }

        return compra.CodigoAtivacao;
    }

    public async Task<bool> ValidarCodigoAtivacaoAsync(string codigo)
    {
        var compra = await _compraRepository.ObterPorCodigoAtivacaoAsync(codigo);
        if (compra == null)
            return false;

        return compra.Status == StatusCompra.Concluida;
    }

    private static string GerarCodigoAtivacao()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 16)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static CompraResponse MapearParaResponse(Compra compra)
    {
        return new CompraResponse
        {
            Id = compra.Id,
            UsuarioId = compra.UsuarioId,
            JogoId = compra.JogoId,
            PrecoPago = compra.PrecoPago,
            DataCompra = compra.DataCompra,
            Status = compra.Status.ToString(),
            CodigoAtivacao = compra.CodigoAtivacao,
            DataAtualizacao = compra.DataAtualizacao
        };
    }
} 