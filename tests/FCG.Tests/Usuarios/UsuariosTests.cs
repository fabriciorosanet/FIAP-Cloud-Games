using System.ComponentModel.DataAnnotations;
using FCG.Application.Usuarios.Services;
using FCG.Application.Usuarios.ViewModels;
using FCG.Domain.Usuarios.Entities;
using FCG.Domain.Usuarios.Interfaces;
using Moq;

namespace FCG.Tests.Usuarios;

public class UsuariosTests
{
    [Fact(DisplayName = "Deve validar um CriarUsuarioRequest válido")]
    public void CriarUsuarioRequest_Valido_DeveSerValido()
    {
        var criarUsuarioRequest = new CriarUsuarioRequest
        {
            Nome = "Maria Souza",
            Email = "maria.souza@email.com",
            Senha = "Senha@456",
            TipoUsuario = TipoUsuarioViewModel.Usuario
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(criarUsuarioRequest);

        var isValid = Validator.TryValidateObject(criarUsuarioRequest, validationContext, validationResults, true);

        Assert.True(isValid, "O CriarUsuarioRequest deveria ser válido.");
        Assert.Empty(validationResults);
    }

    [Fact(DisplayName = "Deve falhar ao validar um CriarUsuarioRequest inválido")]
    public void CriarUsuarioRequest_Invalido_DeveRetornarErrosDeValidacao()
    {
        var criarUsuarioRequest = new CriarUsuarioRequest
        {
            Nome = "",
            Email = "email-invalido",
            Senha = "abc",
            TipoUsuario = (TipoUsuarioViewModel)99
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(criarUsuarioRequest);

        var isValid = Validator.TryValidateObject(criarUsuarioRequest, validationContext, validationResults, true);

        Assert.False(isValid, "O CriarUsuarioRequest deveria ser inválido.");
        Assert.NotEmpty(validationResults);

        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O campo Nome é obrigatório."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O Email informado não é válido."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("A Senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O TipoUsuario informado não é válido."));
    }

    [Fact(DisplayName = "Deve validar um AtualizarUsuarioRequest válido")]
    public void AtualizarUsuarioRequest_Valido_DeveSerValido()
    {
        var atualizarUsuarioRequest = new AtualizarUsuarioRequest
        {
            Nome = "Carlos Santos",
            Email = "carlos.santos@email.com",
            Senha = "Senha@789",
            TipoUsuario = TipoUsuarioViewModel.Administrador
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(atualizarUsuarioRequest);

        var isValid = Validator.TryValidateObject(atualizarUsuarioRequest, validationContext, validationResults, true);

        Assert.True(isValid, "O AtualizarUsuarioRequest deveria ser válido.");
        Assert.Empty(validationResults);
    }

    [Fact(DisplayName = "Deve falhar ao validar um AtualizarUsuarioRequest inválido")]
    public void AtualizarUsuarioRequest_Invalido_DeveRetornarErrosDeValidacao()
    {
        var atualizarUsuarioRequest = new AtualizarUsuarioRequest
        {
            Nome = "Um nome muito, muito, muito, muito, muito, muito, muito, muito, muito, muito, muito, muito, muito, muito, muito, muito, muito, muito longo que excede o limite de 100 caracteres",
            Email = "invalid-email",
            Senha = "curta",
            TipoUsuario = (TipoUsuarioViewModel)0
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(atualizarUsuarioRequest);

        var isValid = Validator.TryValidateObject(atualizarUsuarioRequest, validationContext, validationResults, true);

        Assert.False(isValid, "O AtualizarUsuarioRequest deveria ser inválido.");
        Assert.NotEmpty(validationResults);

        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O Nome deve ter no máximo 100 caracteres."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O Email informado não é válido."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("A Senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais."));
        Assert.Contains(validationResults, v => v.ErrorMessage != null && v.ErrorMessage.Contains("O TipoUsuario informado não é válido."));
    }

    [Fact(DisplayName = "Deve adicionar um novo usuário com sucesso")]
    public async Task Adicionar_DeveAdicionarUsuarioComSucesso()
    {
        var id = Guid.NewGuid();

        var criarUsuarioRequest = new CriarUsuarioRequest
        {
            Nome = "João Silva",
            Email = "joao.silva@email.com",
            Senha = "Senha@123",
            TipoUsuario = TipoUsuarioViewModel.Administrador
        };

        var usuarioAdicionado = new Usuario
        {
            Id = id,
            Nome = "João Silva",
            Email = "joao.silva@email.com",
            Senha = "Senha@123",
            TipoUsuario = TipoUsuario.Administrador
        };

        var mockUsuarioRepository = new Mock<IUsuarioRepository>();
        mockUsuarioRepository
            .Setup(repo => repo.Adicionar(It.IsAny<Usuario>()))
            .ReturnsAsync(usuarioAdicionado);

        var usuarioService = new UsuarioService(mockUsuarioRepository.Object);

        var resultado = await usuarioService.Adicionar(criarUsuarioRequest);

        Assert.NotNull(resultado);
        Assert.Equal(usuarioAdicionado.Id, resultado.Id);
        Assert.Equal(usuarioAdicionado.Nome, resultado.Nome);
        Assert.Equal(usuarioAdicionado.Email, resultado.Email);
        Assert.Equal((int)usuarioAdicionado.TipoUsuario, (int)resultado.TipoUsuario);

        mockUsuarioRepository.Verify(repo => repo.Adicionar(It.IsAny<Usuario>()), Times.Once);
    }
}