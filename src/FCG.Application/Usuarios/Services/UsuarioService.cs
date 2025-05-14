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

    public async Task<Usuario?> AutenticarUsuarioAsync(string email, string senha)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
        return (usuario is not null && usuario.Senha == senha) ?  usuario : null;
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

    public async Task<bool> Excluir(Guid usuarioId)
    {
        var usuarioExistente = await _usuarioRepository.ObterPorId(usuarioId);
        if (usuarioExistente == null)
        {
            return false;
        }

        await _usuarioRepository.Remover(usuarioId);
        return true;
    }

    public async Task<DadosUsuarioViewModel> Atualizar(UsuarioViewModel usuario)
    {
        var usuarioExistente = await _usuarioRepository.ObterPorId(usuario.Id);
        if (usuarioExistente == null)
        {
            return null;
        }

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.Senha = usuario.Senha;
        usuarioExistente.TipoUsuario = (TipoUsuario)usuario.TipoUsuario;

        await _usuarioRepository.Atualizar(usuarioExistente);

        return new DadosUsuarioViewModel
        {
            Id = usuarioExistente.Id,
            Nome = usuarioExistente.Nome,
            Email = usuarioExistente.Email,
            TipoUsuario = (TipoUsuarioViewModel)usuarioExistente.TipoUsuario
        };
    }

    public void Dispose()
    {
        _usuarioRepository?.Dispose();
    }

    public async Task<List<DadosUsuarioViewModel>> Consultar()
    {
        var listaUsuario = await _usuarioRepository.ObterTodos();
        if (listaUsuario == null || !listaUsuario.Any())
        {
            return null;
        }

        return listaUsuario.Select(usuario => new DadosUsuarioViewModel
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = (TipoUsuarioViewModel)usuario.TipoUsuario
        }).ToList();
    }
}
