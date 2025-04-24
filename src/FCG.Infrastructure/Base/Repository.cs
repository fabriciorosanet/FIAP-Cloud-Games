using System.Linq.Expressions;
using FCP.Domain.Base;

namespace FCP.Infrastructure.Base;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    public Task Adicionar(TEntity entity)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
