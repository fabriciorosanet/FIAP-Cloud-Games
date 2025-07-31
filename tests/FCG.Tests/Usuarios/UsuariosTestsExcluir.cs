using System.ComponentModel.DataAnnotations;
using FCG.Application.Usuarios.Services;
using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;
using FCG.Domain.Usuarios.Interfaces;
using Moq;

namespace FCG.Tests.Usuarios;

public class UsuariosTestsExcluir
{
    [Fact(DisplayName = "Deve excluir um usuário com sucesso")]
    public async Task Excluir_DeveExcluirUsuarioComSucesso()
    {
        var id = Guid.NewGuid();

        var usuarioExistente = new Usuario
        {
            Id = id,
            Nome = "Gustavo Mendonça",
            Email = "gustavo.mendonca@email.com",
            Senha = "Senha@123",
            TipoUsuario = TipoUsuario.Administrador
        };

        var mockUsuarioRepository = new Mock<IUsuarioRepository>();
        mockUsuarioRepository
            .Setup(repo => repo.ObterPorId(id))
            .ReturnsAsync(usuarioExistente);

        mockUsuarioRepository
            .Setup(repo => repo.Remover(id))
            .Returns(Task.CompletedTask);

        var usuarioService = new UsuarioService(mockUsuarioRepository.Object);

        var resultado = await usuarioService.Excluir(id);

        Assert.True(resultado);

        mockUsuarioRepository.Verify(repo => repo.ObterPorId(id), Times.Once);
        mockUsuarioRepository.Verify(repo => repo.Remover(id), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar false ao tentar excluir um usuário inexistente")]
    public async Task Excluir_UsuarioNaoExistente_DeveRetornarFalse()
    {
        var id = Guid.NewGuid();

        var mockUsuarioRepository = new Mock<IUsuarioRepository>();
        mockUsuarioRepository
            .Setup(repo => repo.ObterPorId(id))
            .ReturnsAsync((Usuario)null);

        var usuarioService = new UsuarioService(mockUsuarioRepository.Object);
        var resultado = await usuarioService.Excluir(id);

        Assert.False(resultado);

        mockUsuarioRepository.Verify(repo => repo.ObterPorId(id), Times.Once);
        mockUsuarioRepository.Verify(repo => repo.Remover(It.IsAny<Guid>()), Times.Never);
    }
}