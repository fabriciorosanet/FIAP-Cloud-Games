using System.ComponentModel.DataAnnotations;
using FCG.Application.Usuarios.Services;
using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;
using FCG.Domain.Usuarios.Interfaces;
using Moq;

namespace FCG.Tests.Usuarios;

public class UsuariosTestsAtualizar
{
    [Fact(DisplayName = "Deve atualizar um usuário com sucesso")]
    public async Task Atualizar_DeveAtualizarUsuarioComSucesso()
    {
        var id = Guid.NewGuid();

        var atualizarUsuarioRequest = new AtualizarUsuarioRequest
        {
            Nome = "Gustavo Mendonça",
            Email = "gustavo.mendonca@email.com",
            Senha = "Senha@123",
            TipoUsuario = TipoUsuarioViewModel.Usuario
        };

        var usuarioExistente = new Usuario
        {
            Id = id,
            Nome = "Gustavo Arcemide",
            Email = "gustavo.arcemide@email.com",
            Senha = "Senha@456",
            TipoUsuario = TipoUsuario.Administrador
        };

        var mockUsuarioRepository = new Mock<IUsuarioRepository>();
        mockUsuarioRepository
            .Setup(repo => repo.ObterPorId(id))
            .ReturnsAsync(usuarioExistente);

        mockUsuarioRepository
            .Setup(repo => repo.Atualizar(It.IsAny<Usuario>()))
            .Returns(Task.CompletedTask);

        var usuarioService = new UsuarioService(mockUsuarioRepository.Object);
        var resultado = await usuarioService.Atualizar(id, atualizarUsuarioRequest);

        Assert.NotNull(resultado);
        Assert.Equal(atualizarUsuarioRequest.Nome, resultado.Nome);
        Assert.Equal(atualizarUsuarioRequest.Email, resultado.Email);
        Assert.Equal((int)atualizarUsuarioRequest.TipoUsuario, (int)resultado.TipoUsuario);

        mockUsuarioRepository.Verify(repo => repo.ObterPorId(id), Times.Once);
        mockUsuarioRepository.Verify(repo => repo.Atualizar(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar null ao tentar atualizar um usuário inexistente")]
    public async Task Atualizar_UsuarioNaoExistente_DeveRetornarNull()
    {
        var id = Guid.NewGuid();

        var atualizarUsuarioRequest = new AtualizarUsuarioRequest
        {
            Nome = "Gustavo Mendonça",
            Email = "gustavo.mendonca@email.com",
            Senha = "Senha@123",
            TipoUsuario = TipoUsuarioViewModel.Usuario
        };

        var mockUsuarioRepository = new Mock<IUsuarioRepository>();
        mockUsuarioRepository
            .Setup(repo => repo.ObterPorId(id))
            .ReturnsAsync((Usuario)null);

        var usuarioService = new UsuarioService(mockUsuarioRepository.Object);
        var resultado = await usuarioService.Atualizar(id, atualizarUsuarioRequest);

        Assert.Null(resultado);

        mockUsuarioRepository.Verify(repo => repo.ObterPorId(id), Times.Once);
        mockUsuarioRepository.Verify(repo => repo.Atualizar(It.IsAny<Usuario>()), Times.Never);
    }
}