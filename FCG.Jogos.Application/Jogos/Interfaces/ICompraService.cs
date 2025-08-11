using FCG.Jogos.Application.Jogos.ViewModels;

namespace FCG.Jogos.Application.Jogos.Interfaces;

public interface ICompraService
{
    Task<CompraResponse> CriarCompraAsync(CompraRequest request);
    Task<CompraResponse?> ObterCompraPorIdAsync(Guid id);
    Task<IEnumerable<CompraResponse>> ObterComprasPorUsuarioAsync(Guid usuarioId);
    Task<IEnumerable<CompraResponse>> ObterComprasPorJogoAsync(Guid jogoId);
    Task<CompraResponse> AtualizarStatusCompraAsync(Guid id, string novoStatus);
    Task<bool> CancelarCompraAsync(Guid id);
    Task<string> GerarCodigoAtivacaoAsync(Guid compraId);
    Task<bool> ValidarCodigoAtivacaoAsync(string codigo);
} 