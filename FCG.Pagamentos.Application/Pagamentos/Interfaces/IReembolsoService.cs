using FCG.Pagamentos.Application.Pagamentos.ViewModels;

namespace FCG.Pagamentos.Application.Pagamentos.Interfaces;

public interface IReembolsoService
{
    Task<ReembolsoResponse> CriarAsync(CriarReembolsoRequest request);
    Task<ReembolsoResponse?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ReembolsoResponse>> ObterTodosAsync();
    Task<IEnumerable<ReembolsoResponse>> ObterPorUsuarioAsync(Guid usuarioId);
    Task<IEnumerable<ReembolsoResponse>> ObterPorTransacaoAsync(Guid transacaoId);
    Task<ReembolsoResponse> AtualizarStatusAsync(Guid id, string novoStatus);
    Task<bool> CancelarReembolsoAsync(Guid id);
    Task<ReembolsoResponse> ProcessarReembolsoAsync(Guid id);
} 