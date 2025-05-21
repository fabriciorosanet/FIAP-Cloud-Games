using System.ComponentModel.DataAnnotations;
using FCG.Application.Usuarios.Services;
using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;
using FCG.Domain.Usuarios.Interfaces;
using Moq;

namespace FCG.Tests.Usuarios
{
    public class UsuariosTestsConsultar
    {
        [Fact(DisplayName = "Deve consultar um usuário com sucesso")]
        public async Task Consultar_DeveConsultarUsuarioComSucesso()
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

            var usuarioService = new UsuarioService(mockUsuarioRepository.Object);

            var resultado = await usuarioService.ConsultarUsuario(id);

            Assert.Equal(resultado.Id, id);

            mockUsuarioRepository.Verify(repo => repo.ObterPorId(id), Times.Once);            
        }

        [Fact(DisplayName = "Deve retornar null ao tentar consultar um usuário inexistente")]
        public async Task Excluir_UsuarioNaoExistente_DeveRetornarFalse()
        {
            var id = Guid.NewGuid();

            var mockUsuarioRepository = new Mock<IUsuarioRepository>();
            mockUsuarioRepository
                .Setup(repo => repo.ObterPorId(id))
                .ReturnsAsync((Usuario)null);

            var usuarioService = new UsuarioService(mockUsuarioRepository.Object);
            var resultado = await usuarioService.ConsultarUsuario(id);

            Assert.Null(resultado);

            mockUsuarioRepository.Verify(repo => repo.ObterPorId(id), Times.Once);
        }
    }
}
