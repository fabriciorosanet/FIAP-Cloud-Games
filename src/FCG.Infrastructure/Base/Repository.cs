using System.Linq.Expressions;
using FCG.Infrastructure;
using FCP.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace FCP.Infrastructure.Base;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly AppDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(AppDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public async Task<TEntity> Adicionar(TEntity entity)
    {
        var addedEntity = DbSet.Add(entity).Entity;
        await SaveChanges();
        return addedEntity;
    }

    public async Task Atualizar(TEntity entity)
    {
        var addedEntity = DbSet.Update(entity);
        await SaveChanges();
    }

    public Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        Db?.Dispose();
    }

    public async Task<TEntity> ObterPorId(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> ObterTodos()
    {
        return await DbSet.ToListAsync();
    }

    public async Task Remover(Guid id)
    {
        var entity = await ObterPorId(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            await SaveChanges();
        }
    }

    public async Task<int> SaveChanges()
    {
        return await Db.SaveChangesAsync();
    }
}
