using FCG.Pagamentos.Application.Pagamentos.Interfaces;
using FCG.Pagamentos.Application.Pagamentos.ViewModels;
using FCG.Pagamentos.Domain.Pagamentos.Entities;
using FCG.Pagamentos.Domain.Pagamentos.Interfaces;

namespace FCG.Pagamentos.Application.Pagamentos.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _transacaoRepository;

    public TransacaoService(ITransacaoRepository transacaoRepository)
    {
        _transacaoRepository = transacaoRepository;
    }

    public async Task<TransacaoResponse> CriarAsync(CriarTransacaoRequest request)
    {
        var transacao = new Transacao
        {
            UsuarioId = request.UsuarioId,
            JogoId = request.JogoId,
            Valor = request.Valor,
            Moeda = request.Moeda,
            TipoPagamento = request.TipoPagamento,
            Observacoes = request.Observacoes,
            StatusTransacao = StatusTransacao.Pendente,
            Referencia = GerarReferencia(),
            DataCriacao = DateTime.UtcNow
        };

        var transacaoCriada = await _transacaoRepository.AdicionarAsync(transacao);
        return MapearParaResponse(transacaoCriada);
    }

    public async Task<TransacaoResponse?> ObterPorIdAsync(Guid id)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        return transacao != null ? MapearParaResponse(transacao) : null;
    }

    public async Task<IEnumerable<TransacaoResponse>> ObterTodasAsync()
    {
        var transacoes = await _transacaoRepository.ObterTodasAsync();
        return transacoes.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<TransacaoResponse>> ObterPorUsuarioAsync(Guid usuarioId)
    {
        var transacoes = await _transacaoRepository.ObterPorUsuarioAsync(usuarioId);
        return transacoes.Select(MapearParaResponse);
    }

    public async Task<TransacaoResponse> AtualizarAsync(Guid id, AtualizarTransacaoRequest request)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        if (transacao == null)
            throw new InvalidOperationException("Transação não encontrada");

        transacao.Valor = request.Valor ?? transacao.Valor;
        transacao.Moeda = request.Moeda ?? transacao.Moeda;
        transacao.TipoPagamento = request.TipoPagamento ?? transacao.TipoPagamento;
        transacao.Observacoes = request.Observacoes ?? transacao.Observacoes;
        transacao.DataAtualizacao = DateTime.UtcNow;

        var transacaoAtualizada = await _transacaoRepository.AtualizarAsync(transacao);
        return MapearParaResponse(transacaoAtualizada);
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        return await _transacaoRepository.ExcluirAsync(id);
    }

    public async Task<TransacaoResponse> ProcessarPagamentoAsync(ProcessarPagamentoRequest request)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(request.TransacaoId);
        if (transacao == null)
            throw new InvalidOperationException("Transação não encontrada");

        if (transacao.StatusTransacao != StatusTransacao.Pendente)
            throw new InvalidOperationException("Transação não está pendente");

        // Simular processamento de pagamento
        if (ValidarDadosPagamento(request.DadosCartao))
        {
            transacao.StatusTransacao = StatusTransacao.Aprovada;
            transacao.DataProcessamento = DateTime.UtcNow;
            transacao.ReferenciaProcessamento = GerarReferenciaProcessamento();
        }
        else
        {
            transacao.StatusTransacao = StatusTransacao.Rejeitada;
            transacao.DataProcessamento = DateTime.UtcNow;
        }

        transacao.DataAtualizacao = DateTime.UtcNow;
        var transacaoProcessada = await _transacaoRepository.AtualizarAsync(transacao);
        return MapearParaResponse(transacaoProcessada);
    }

    public async Task<TransacaoResponse> CancelarTransacaoAsync(Guid id)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        if (transacao == null)
            throw new InvalidOperationException("Transação não encontrada");

        if (transacao.StatusTransacao == StatusTransacao.Cancelada)
            throw new InvalidOperationException("Transação já está cancelada");

        transacao.StatusTransacao = StatusTransacao.Cancelada;
        transacao.DataAtualizacao = DateTime.UtcNow;

        var transacaoCancelada = await _transacaoRepository.AtualizarAsync(transacao);
        return MapearParaResponse(transacaoCancelada);
    }

    public async Task<IEnumerable<TransacaoResponse>> BuscarAsync(BuscarTransacoesRequest request)
    {
        var transacoes = await _transacaoRepository.BuscarAsync(
            request.UsuarioId,
            request.Status,
            request.TipoPagamento,
            request.DataInicio,
            request.DataFim);

        return transacoes.Select(MapearParaResponse);
    }

    public async Task<TransacaoResponse> ObterPorReferenciaAsync(string referencia)
    {
        var transacao = await _transacaoRepository.ObterPorReferenciaAsync(referencia);
        return transacao != null ? MapearParaResponse(transacao) : null;
    }

    private static bool ValidarDadosPagamento(DadosCartaoRequest dadosCartao)
    {
        // Simulação de validação de cartão
        if (string.IsNullOrEmpty(dadosCartao.NumeroCartao) || 
            string.IsNullOrEmpty(dadosCartao.CVV) ||
            dadosCartao.DataExpiracao <= DateTime.Now)
            return false;

        // Simular falha aleatória para demonstração
        return new Random().Next(1, 10) > 2; // 80% de sucesso
    }

    private static string GerarReferencia()
    {
        return $"TXN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
    }

    private static string GerarReferenciaProcessamento()
    {
        return $"PROC-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper()}";
    }

    private static TransacaoResponse MapearParaResponse(Transacao transacao)
    {
        return new TransacaoResponse
        {
            Id = transacao.Id,
            UsuarioId = transacao.UsuarioId,
            JogoId = transacao.JogoId,
            Valor = transacao.Valor,
            Moeda = transacao.Moeda,
            StatusTransacao = transacao.StatusTransacao.ToString(),
            TipoPagamento = transacao.TipoPagamento.ToString(),
            Referencia = transacao.Referencia,
            ReferenciaProcessamento = transacao.ReferenciaProcessamento,
            Observacoes = transacao.Observacoes,
            DataCriacao = transacao.DataCriacao,
            DataProcessamento = transacao.DataProcessamento,
            DataAtualizacao = transacao.DataAtualizacao
        };
    }
} 