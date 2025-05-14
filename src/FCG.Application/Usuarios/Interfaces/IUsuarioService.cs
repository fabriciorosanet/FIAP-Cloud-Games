using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;

namespace FCG.Application.Usuarios.Interfaces;

public interface IUsuarioService : IDisposable
{
    Task<Usuario?> AutenticarUsuarioAsync(string email, string senha);
    Task<DadosUsuarioViewModel> Adicionar(UsuarioViewModel usuario);
    Task<bool> Excluir(Guid usuarioId);
    Task<DadosUsuarioViewModel> Atualizar(UsuarioViewModel usuario);
    Task<List<DadosUsuarioViewModel>> Consultar();
}
