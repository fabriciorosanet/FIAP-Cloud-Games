using FCG.Domain.Usuarios.Entities;
using FCG.Domain.Usuarios.Interfaces;
using FCP.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Usuarios.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    private readonly AppDbContext _context;
    public UsuarioRepository(AppDbContext context) : base(context) => _context = context;

    public Task<Usuario?> ObterPorEmailAsync(string email) =>
        _context.Set<Usuario>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
}

