using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FCG.Application.Usuarios.Interfaces;

public interface IUsuarioService : IDisposable
{
    Task<Usuario?> AutenticarUsuarioAsync(string email, string senha);
    Task<DadosUsuarioViewModel> Adicionar(CriarUsuarioRequest usuario);
    Task<bool> Excluir(Guid usuarioId);
    Task<DadosUsuarioViewModel?> Atualizar(Guid id, AtualizarUsuarioRequest usuario);
    Task<List<DadosUsuarioViewModel>> Consultar();
    Task<DadosUsuarioViewModel?> ConsultarUsuario(Guid usuarioId);
    Task<Usuario?> ObterUsuario(Expression<Func<Usuario, bool>> predicate);
}