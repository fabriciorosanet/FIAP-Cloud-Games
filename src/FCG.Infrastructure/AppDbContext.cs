using FCG.Domain.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure {

	public class AppDbContext : DbContext {

		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options) { }

		public DbSet<Usuario> Usuarios => Set<Usuario>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
		}

	}

}