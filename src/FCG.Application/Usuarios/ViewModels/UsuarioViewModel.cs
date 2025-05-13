using System.ComponentModel.DataAnnotations;

namespace FCG.Application.Usuarios.ViewModels;

public class UsuarioViewModel
{
    [Key]
    public Guid Id { get; set; }
        
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
    public required string Nome { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O Email informado não é válido.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [StringLength(112, MinimumLength = 8, ErrorMessage = "A Senha deve ter entre 8 e 12 caracteres.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "A Senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais.")]
    public required string Senha { get; set; }

    [Required(ErrorMessage = "O campo TipoUsuario é obrigatório.")]
    [EnumDataType(typeof(TipoUsuarioViewModel), ErrorMessage = "O TipoUsuario informado não é válido.")]
    public TipoUsuarioViewModel TipoUsuario { get; set; }
}

public enum TipoUsuarioViewModel
{
    Administrador = 1,
    Usuario = 2
}
