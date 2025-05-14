using FCG.Application.Usuarios.ViewModels;

namespace FCG.Application.Usuarios.Interfaces;

public interface IUsuarioService : IDisposable
{
    Task<DadosUsuarioViewModel> Adicionar(UsuarioViewModel usuario);
    Task<bool> Excluir(Guid usuarioId);
    Task<DadosUsuarioViewModel> Atualizar(UsuarioViewModel usuario); 
}
