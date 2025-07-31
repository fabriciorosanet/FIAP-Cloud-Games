using System.Linq.Expressions;
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
        return (usuario is not null && usuario.Senha == senha) ? usuario : null;
    }

    public async Task<DadosUsuarioViewModel> Adicionar(CriarUsuarioRequest usuario)
    {
        var novoUsuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = usuario.Nome,
            Email = usuario.Email,
            Senha = usuario.Senha,
            TipoUsuario = (TipoUsuario)usuario.TipoUsuario
        };

        var usuarioAdicionado = await _usuarioRepository.Adicionar(novoUsuario);

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

    public async Task<DadosUsuarioViewModel?> Atualizar(Guid id, AtualizarUsuarioRequest request)
    {
        var usuarioExistente = await _usuarioRepository.ObterPorId(id);
        if (usuarioExistente == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.Nome))
        {
            usuarioExistente.Nome = request.Nome;
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            usuarioExistente.Email = request.Email;
        }

        if (!string.IsNullOrWhiteSpace(request.Senha))
        {
            usuarioExistente.Senha = request.Senha;
        }

        if (request.TipoUsuario.HasValue)
        {
            usuarioExistente.TipoUsuario = (TipoUsuario)request.TipoUsuario.Value;
        }

        await _usuarioRepository.Atualizar(usuarioExistente);

        return new DadosUsuarioViewModel
        {
            Id = usuarioExistente.Id,
            Nome = usuarioExistente.Nome,
            Email = usuarioExistente.Email,
            TipoUsuario = (TipoUsuarioViewModel)usuarioExistente.TipoUsuario
        };
    }

    public async Task<List<DadosUsuarioViewModel>> Consultar()
    {
        var listaUsuario = await _usuarioRepository.ObterTodos();
        if (listaUsuario == null || !listaUsuario.Any())
        {
            return new List<DadosUsuarioViewModel>();
        }

        return listaUsuario.Select(usuario => new DadosUsuarioViewModel
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = (TipoUsuarioViewModel)usuario.TipoUsuario
        }).ToList();
    }

    public async Task<DadosUsuarioViewModel?> ConsultarUsuario(Guid usuarioId)
    {
        var usuarioExistente = await _usuarioRepository.ObterPorId(usuarioId);
        if (usuarioExistente == null)
        {
            return null;
        }

        return new DadosUsuarioViewModel
        {
            Id = usuarioExistente.Id,
            Nome = usuarioExistente.Nome,
            Email = usuarioExistente.Email,
            TipoUsuario = (TipoUsuarioViewModel)usuarioExistente.TipoUsuario
        };
    }

    public async Task<Usuario?> ObterUsuario(Expression<Func<Usuario, bool>> predicate)
    {
        var usuarios = await _usuarioRepository.Buscar(predicate);

        if (usuarios == null || !usuarios.Any()) return null;

        return usuarios.ToList().FirstOrDefault();
    }

    public void Dispose()
    {
        _usuarioRepository?.Dispose();
    }
}