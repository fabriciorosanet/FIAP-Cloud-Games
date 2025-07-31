using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;
using System;

namespace FCG.Application.Usuarios.Interfaces;

public interface IUsuarioService : IDisposable
{
    Task<Usuario?> AutenticarUsuarioAsync(string email, string senha);

    Task<UsuarioResponse> Adicionar(CriarUsuarioRequest usuario);

    Task<bool> Excluir(Guid usuarioId);

    Task<UsuarioResponse?> Atualizar(Guid id, AtualizarUsuarioRequest usuario);

    Task<List<UsuarioResponse>> Consultar();
}
