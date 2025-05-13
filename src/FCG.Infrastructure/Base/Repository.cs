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

    public Task Atualizar(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        Db?.Dispose();
    }

    public Task<TEntity> ObterPorId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> ObterTodos()
    {
        throw new NotImplementedException();
    }

    public Task Remover(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChanges()
    {
        throw new NotImplementedException();
    }
}
