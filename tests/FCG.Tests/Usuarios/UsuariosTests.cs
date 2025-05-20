using System.ComponentModel.DataAnnotations;
using FCG.Application.Usuarios.Services;
using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;
using FCG.Domain.Usuarios.Interfaces;
using Moq;

namespace FCG.Tests.Usuarios;

public class UsuariosTests
{
    [Fact(DisplayName = "Deve validar um Usuario válido")]
    public void Usuario_Valido_DeveSerValido()
    {
        // Given: Um UsuarioDTO válido
        var usuarioDto = new UsuarioViewModel
        {
            Id = Guid.NewGuid(),
            Nome = "João Silva",
            Email = "joao.silva@email.com",
            Senha = "Senha@123",
            TipoUsuario = TipoUsuarioViewModel.Administrador
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(usuarioDto);

        // When: Validamos o objeto
        var isValid = Validator.TryValidateObject(usuarioDto, validationContext, validationResults, true);

        // Then: O objeto deve ser válido
        Assert.True(isValid, "O UsuarioDTO deveria ser válido.");
        Assert.Empty(validationResults);
    }

    [Fact(DisplayName = "Deve falhar ao validar um Usuario inválido")]
    public void Usuario_Invalido_DeveRetornarErrosDeValidacao()
    {
        // Given: Um UsuarioDTO inválido
        var usuarioDto = new UsuarioViewModel
        {
            Id = Guid.NewGuid(),
            Nome = "", // Nome inválido (vazio)
            Email = "email-invalido", // Email inválido
            Senha = "123", // Senha inválida (não atende aos requisitos)
            TipoUsuario = (TipoUsuarioViewModel)99 // TipoUsuario inválido
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(usuarioDto);

        // When: Validamos o objeto
        var isValid = Validator.TryValidateObject(usuarioDto, validationContext, validationResults, true);

        // Then: O objeto deve ser inválido e conter erros de validação
        Assert.False(isValid, "O UsuarioDTO deveria ser inválido.");
        Assert.NotEmpty(validationResults);

        // Verifica mensagens de erro específicas
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O campo Nome é obrigatório."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O Email informado não é válido."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("A Senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O TipoUsuario informado não é válido."));
    }

    [Fact(DisplayName = "Deve adicionar um novo usuário com sucesso")]
    public async Task Adicionar_DeveAdicionarUsuarioComSucesso()
    {
        // Given: Um DTO de usuário válido e um mock do repositório
        var id = Guid.NewGuid();

        var usuarioDto = new UsuarioViewModel
        {
            Id = id,
            Nome = "João Silva",
            Email = "joao.silva@email.com",
            Senha = "senha123",
            TipoUsuario = TipoUsuarioViewModel.Administrador
        };

        var usuarioAdicionado = new Usuario
        {
            Id = id,
            Nome = "João Silva",
            Email = "joao.silva@email.com",
            Senha = "senha123",
            TipoUsuario = TipoUsuario.Administrador
        };

        var mockUsuarioRepository = new Mock<IUsuarioRepository>();
        mockUsuarioRepository
            .Setup(repo => repo.Adicionar(It.IsAny<Usuario>()))
            .ReturnsAsync(usuarioAdicionado);

        var usuarioService = new UsuarioService(mockUsuarioRepository.Object);

        // When: O método Adicionar é chamado
        var resultado = await usuarioService.Adicionar(usuarioDto);

        // Then: O resultado deve conter os dados esperados
        Assert.NotNull(resultado);
        Assert.Equal(usuarioAdicionado.Id, resultado.Id);
        Assert.Equal(usuarioAdicionado.Nome, resultado.Nome);
        Assert.Equal(usuarioAdicionado.Email, resultado.Email);
        Assert.Equal((int)usuarioAdicionado.TipoUsuario, (int)resultado.TipoUsuario);

        // Verifica se o método Adicionar do repositório foi chamado uma vez
        mockUsuarioRepository.Verify(repo => repo.Adicionar(It.IsAny<Usuario>()), Times.Once);
    }
}
