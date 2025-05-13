using FCG.Application.Usuarios.Interfaces;
using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;
using FCG.Domain.Usuarios.Interfaces;

namespace FCG.Application.Usuarios.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<DadosUsuarioViewModel> Adicionar(UsuarioViewModel usuario)
    {
        var usuarioAdicionado = await _usuarioRepository.Adicionar(new Usuario
        {
            Nome = usuario.Nome,
            Email = usuario.Email,
            Senha = usuario.Senha,
            TipoUsuario = (TipoUsuario)usuario.TipoUsuario
        });

        return new DadosUsuarioViewModel
        {
            Id = usuarioAdicionado.Id,
            Nome = usuarioAdicionado.Nome,
            Email = usuarioAdicionado.Email,
            TipoUsuario = (TipoUsuarioViewModel)usuarioAdicionado.TipoUsuario
        };
    }

    public void Dispose()
    {
        _usuarioRepository?.Dispose();
    }
}
