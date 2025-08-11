using FCG.Jogos.Application.Jogos.ViewModels;

namespace FCG.Jogos.Application.Jogos.Interfaces;

public interface IJogoService
{
    Task<JogoResponse> CriarAsync(CriarJogoRequest request);
    Task<JogoResponse?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<JogoResponse>> ObterTodosAsync();
    Task<IEnumerable<JogoResponse>> BuscarAsync(BuscarJogosRequest request);
    Task<JogoResponse> AtualizarAsync(Guid id, AtualizarJogoRequest request);
    Task<bool> ExcluirAsync(Guid id);
    Task<IEnumerable<JogoResponse>> ObterPorCategoriaAsync(string categoria);
    Task<IEnumerable<JogoResponse>> ObterPorPlataformaAsync(string plataforma);
    Task<IEnumerable<JogoResponse>> ObterMaisPopularesAsync(int quantidade = 10);
    Task<IEnumerable<JogoResponse>> ObterRecomendacoesAsync(Guid usuarioId);
} 