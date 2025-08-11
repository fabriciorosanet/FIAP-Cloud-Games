using FCG.Pagamentos.Application.Pagamentos.Interfaces;
using FCG.Pagamentos.Application.Pagamentos.ViewModels;
using FCG.Pagamentos.Domain.Pagamentos.Entities;
using FCG.Pagamentos.Domain.Pagamentos.Interfaces;

namespace FCG.Pagamentos.Application.Pagamentos.Services;

public class ReembolsoService : IReembolsoService
{
    private readonly IReembolsoRepository _reembolsoRepository;
    private readonly ITransacaoRepository _transacaoRepository;

    public ReembolsoService(IReembolsoRepository reembolsoRepository, ITransacaoRepository transacaoRepository)
    {
        _reembolsoRepository = reembolsoRepository;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<ReembolsoResponse> CriarAsync(CriarReembolsoRequest request)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(request.TransacaoId);
        if (transacao == null)
            throw new InvalidOperationException("Transação não encontrada");

        if (transacao.StatusTransacao != StatusTransacao.Aprovada)
            throw new InvalidOperationException("Apenas transações aprovadas podem ser reembolsadas");

        var reembolso = new Reembolso
        {
            TransacaoId = request.TransacaoId,
            UsuarioId = request.UsuarioId,
            ValorReembolso = request.ValorReembolso ?? transacao.Valor,
            Motivo = request.Motivo,
            StatusReembolso = StatusReembolso.Pendente,
            DataCriacao = DateTime.UtcNow
        };

        var reembolsoCriado = await _reembolsoRepository.AdicionarAsync(reembolso);
        return MapearParaResponse(reembolsoCriado);
    }

    public async Task<ReembolsoResponse?> ObterPorIdAsync(Guid id)
    {
        var reembolso = await _reembolsoRepository.ObterPorIdAsync(id);
        return reembolso != null ? MapearParaResponse(reembolso) : null;
    }

    public async Task<IEnumerable<ReembolsoResponse>> ObterTodosAsync()
    {
        var reembolsos = await _reembolsoRepository.ObterTodosAsync();
        return reembolsos.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<ReembolsoResponse>> ObterPorUsuarioAsync(Guid usuarioId)
    {
        var reembolsos = await _reembolsoRepository.ObterPorUsuarioAsync(usuarioId);
        return reembolsos.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<ReembolsoResponse>> ObterPorTransacaoAsync(Guid transacaoId)
    {
        var reembolsos = await _reembolsoRepository.ObterPorTransacaoAsync(transacaoId);
        return reembolsos.Select(MapearParaResponse);
    }

    public async Task<ReembolsoResponse> AtualizarStatusAsync(Guid id, string novoStatus)
    {
        var reembolso = await _reembolsoRepository.ObterPorIdAsync(id);
        if (reembolso == null)
            throw new InvalidOperationException("Reembolso não encontrado");

        if (Enum.TryParse<StatusReembolso>(novoStatus, out var status))
        {
            reembolso.StatusReembolso = status;
            reembolso.DataAtualizacao = DateTime.UtcNow;

            if (status == StatusReembolso.Aprovado)
            {
                reembolso.DataAprovacao = DateTime.UtcNow;
            }
            else if (status == StatusReembolso.Processado)
            {
                reembolso.DataProcessamento = DateTime.UtcNow;
            }
        }
        else
        {
            throw new InvalidOperationException("Status inválido");
        }

        var reembolsoAtualizado = await _reembolsoRepository.AtualizarAsync(reembolso);
        return MapearParaResponse(reembolsoAtualizado);
    }

    public async Task<bool> CancelarReembolsoAsync(Guid id)
    {
        var reembolso = await _reembolsoRepository.ObterPorIdAsync(id);
        if (reembolso == null)
            return false;

        if (reembolso.StatusReembolso == StatusReembolso.Cancelado)
            return false;

        reembolso.StatusReembolso = StatusReembolso.Cancelado;
        reembolso.DataAtualizacao = DateTime.UtcNow;

        await _reembolsoRepository.AtualizarAsync(reembolso);
        return true;
    }

    public async Task<ReembolsoResponse> ProcessarReembolsoAsync(Guid id)
    {
        var reembolso = await _reembolsoRepository.ObterPorIdAsync(id);
        if (reembolso == null)
            throw new InvalidOperationException("Reembolso não encontrado");

        if (reembolso.StatusReembolso != StatusReembolso.Aprovado)
            throw new InvalidOperationException("Apenas reembolsos aprovados podem ser processados");

        // Simular processamento do reembolso
        reembolso.StatusReembolso = StatusReembolso.Processado;
        reembolso.DataProcessamento = DateTime.UtcNow;
        reembolso.DataAtualizacao = DateTime.UtcNow;

        var reembolsoProcessado = await _reembolsoRepository.AtualizarAsync(reembolso);
        return MapearParaResponse(reembolsoProcessado);
    }

    private static ReembolsoResponse MapearParaResponse(Reembolso reembolso)
    {
        return new ReembolsoResponse
        {
            Id = reembolso.Id,
            TransacaoId = reembolso.TransacaoId,
            UsuarioId = reembolso.UsuarioId,
            ValorReembolso = reembolso.ValorReembolso,
            Motivo = reembolso.Motivo,
            StatusReembolso = reembolso.StatusReembolso.ToString(),
            DataCriacao = reembolso.DataCriacao,
            DataAprovacao = reembolso.DataAprovacao,
            DataProcessamento = reembolso.DataProcessamento,
            DataAtualizacao = reembolso.DataAtualizacao
        };
    }
} 