using System;

namespace FCG.Application.Usuarios.ViewModels;

public class DadosUsuarioViewModel
{
    public Guid Id { get; set; }
        
    public required string Nome { get; set; }

    public required string Email { get; set; }

    public TipoUsuarioViewModel TipoUsuario { get; set; }
}
