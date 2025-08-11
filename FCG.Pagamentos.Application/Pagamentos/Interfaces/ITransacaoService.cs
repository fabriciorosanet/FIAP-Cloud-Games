using FCG.Pagamentos.Application.Pagamentos.ViewModels;

namespace FCG.Pagamentos.Application.Pagamentos.Interfaces;

public interface ITransacaoService
{
    Task<TransacaoResponse> CriarAsync(CriarTransacaoRequest request);
    Task<TransacaoResponse?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<TransacaoResponse>> ObterTodasAsync();
    Task<IEnumerable<TransacaoResponse>> ObterPorUsuarioAsync(Guid usuarioId);
    Task<TransacaoResponse> AtualizarAsync(Guid id, AtualizarTransacaoRequest request);
    Task<bool> ExcluirAsync(Guid id);
    Task<TransacaoResponse> ProcessarPagamentoAsync(ProcessarPagamentoRequest request);
    Task<TransacaoResponse> CancelarTransacaoAsync(Guid id);
    Task<IEnumerable<TransacaoResponse>> BuscarAsync(BuscarTransacoesRequest request);
    Task<TransacaoResponse> ObterPorReferenciaAsync(string referencia);
} 