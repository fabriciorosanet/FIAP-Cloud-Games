using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FCG.Application.Usuarios.ViewModels
{
public class CriarUsuarioRequest
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
    public required string Nome { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O Email informado não é válido.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [StringLength(12, MinimumLength = 8, ErrorMessage = "A Senha deve ter entre 8 e 12 caracteres.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "A Senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais.")]
    public required string Senha { get; set; }

    [Required(ErrorMessage = "O campo TipoUsuario é obrigatório.")]
    [EnumDataType(typeof(TipoUsuarioViewModel), ErrorMessage = "O TipoUsuario informado não é válido.")]
    public TipoUsuarioViewModel TipoUsuario { get; set; }
}

public class AtualizarUsuarioRequest
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
    public required string Nome { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O Email informado não é válido.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [StringLength(12, MinimumLength = 8, ErrorMessage = "A Senha deve ter entre 8 e 12 caracteres.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "A Senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais.")]
    public required string Senha { get; set; }

    [Required(ErrorMessage = "O campo TipoUsuario é obrigatório.")]
    [EnumDataType(typeof(TipoUsuarioViewModel), ErrorMessage = "O TipoUsuario informado não é válido.")]
    public TipoUsuarioViewModel TipoUsuario { get; set; }
}

public class UsuarioResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TipoUsuarioViewModel TipoUsuario { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipoUsuarioViewModel
{
    Administrador = 1,
    Usuario = 2
}
}