using FCG.Jogos.Application.Jogos.Interfaces;
using FCG.Jogos.Application.Jogos.ViewModels;
using FCG.Jogos.Domain.Jogos.Entities;
using FCG.Jogos.Domain.Jogos.Interfaces;

namespace FCG.Jogos.Application.Jogos.Services;

public class JogoService : IJogoService
{
    private readonly IJogoRepository _jogoRepository;

    public JogoService(IJogoRepository jogoRepository)
    {
        _jogoRepository = jogoRepository;
    }

    public async Task<JogoResponse> CriarAsync(CriarJogoRequest request)
    {
        var jogo = new Jogo
        {
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Desenvolvedor = request.Desenvolvedor,
            Editora = request.Editora,
            Preco = request.Preco,
            Tags = request.Tags?.ToList() ?? new List<string>(),
            Plataformas = request.Plataformas?.ToList() ?? new List<string>(),
            Categoria = request.Categoria,
            ClassificacaoIndicativa = request.ClassificacaoIndicativa,
            Estoque = request.Estoque
        };

        var jogoCriado = await _jogoRepository.AdicionarAsync(jogo);
        return MapearParaResponse(jogoCriado);
    }

    public async Task<JogoResponse?> ObterPorIdAsync(Guid id)
    {
        var jogo = await _jogoRepository.ObterPorIdAsync(id);
        return jogo != null ? MapearParaResponse(jogo) : null;
    }

    public async Task<IEnumerable<JogoResponse>> ObterTodosAsync()
    {
        var jogos = await _jogoRepository.ObterTodosAsync();
        return jogos.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<JogoResponse>> BuscarAsync(BuscarJogosRequest request)
    {
        var jogos = await _jogoRepository.BuscarAsync(
            request.Termos,
            request.Categoria,
            request.Plataforma,
            request.PrecoMinimo,
            request.PrecoMaximo,
            request.ClassificacaoIndicativa);

        return jogos.Select(MapearParaResponse);
    }

    public async Task<JogoResponse> AtualizarAsync(Guid id, AtualizarJogoRequest request)
    {
        var jogo = await _jogoRepository.ObterPorIdAsync(id);
        if (jogo == null)
            throw new InvalidOperationException("Jogo não encontrado");

        jogo.Titulo = request.Titulo ?? jogo.Titulo;
        jogo.Descricao = request.Descricao ?? jogo.Descricao;
        jogo.Desenvolvedor = request.Desenvolvedor ?? jogo.Desenvolvedor;
        jogo.Editora = request.Editora ?? jogo.Editora;
        jogo.Preco = request.Preco ?? jogo.Preco;
        jogo.Tags = request.Tags?.ToList() ?? jogo.Tags;
        jogo.Plataformas = request.Plataformas?.ToList() ?? jogo.Plataformas;
        jogo.Categoria = request.Categoria ?? jogo.Categoria;
        jogo.ClassificacaoIndicativa = request.ClassificacaoIndicativa ?? jogo.ClassificacaoIndicativa;
        jogo.Estoque = request.Estoque ?? jogo.Estoque;
        jogo.DataAtualizacao = DateTime.UtcNow;

        var jogoAtualizado = await _jogoRepository.AtualizarAsync(jogo);
        return MapearParaResponse(jogoAtualizado);
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        return await _jogoRepository.ExcluirAsync(id);
    }

    public async Task<IEnumerable<JogoResponse>> ObterPorCategoriaAsync(string categoria)
    {
        var jogos = await _jogoRepository.ObterPorCategoriaAsync(categoria);
        return jogos.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<JogoResponse>> ObterPorPlataformaAsync(string plataforma)
    {
        var jogos = await _jogoRepository.ObterPorPlataformaAsync(plataforma);
        return jogos.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<JogoResponse>> ObterMaisPopularesAsync(int quantidade = 10)
    {
        var jogos = await _jogoRepository.ObterMaisPopularesAsync(quantidade);
        return jogos.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<JogoResponse>> ObterRecomendacoesAsync(Guid usuarioId)
    {
        var jogos = await _jogoRepository.ObterRecomendacoesAsync(usuarioId);
        return jogos.Select(MapearParaResponse);
    }

    private static JogoResponse MapearParaResponse(Jogo jogo)
    {
        return new JogoResponse
        {
            Id = jogo.Id,
            Titulo = jogo.Titulo,
            Descricao = jogo.Descricao,
            Desenvolvedor = jogo.Desenvolvedor,
            Editora = jogo.Editora,
            Preco = jogo.Preco,
            Tags = jogo.Tags,
            Plataformas = jogo.Plataformas,
            Categoria = jogo.Categoria,
            ClassificacaoIndicativa = jogo.ClassificacaoIndicativa,
            AvaliacaoMedia = jogo.AvaliacaoMedia,
            Estoque = jogo.Estoque,
            DataCriacao = jogo.DataCriacao,
            DataAtualizacao = jogo.DataAtualizacao
        };
    }
} 