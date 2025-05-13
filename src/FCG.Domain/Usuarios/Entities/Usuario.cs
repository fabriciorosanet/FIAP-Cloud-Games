using FCP.Domain.Base;

namespace FCG.Domain.Usuarios.Entities;

public class Usuario : Entity
{
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Senha { get; set; }
    public TipoUsuario? TipoUsuario { get; set; }
}

public enum TipoUsuario
{
    Administrador = 1,
    Usuario = 2
}

