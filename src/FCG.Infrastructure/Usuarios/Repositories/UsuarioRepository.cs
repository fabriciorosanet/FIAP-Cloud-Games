using FCG.Domain.Usuarios.Entities;
using FCG.Domain.Usuarios.Interfaces;
using FCP.Infrastructure.Base;

namespace FCG.Infrastructure.Usuarios.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context)
    {
        
    }
}
