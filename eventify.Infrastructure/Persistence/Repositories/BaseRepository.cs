using eventify.Application.Common.Interfaces;
using eventify.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eventify.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly EventsDbContext _context;

    public BaseRepository(EventsDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IList<T>> GetAllAsync(bool includeDeleted = false)
    {
        if (includeDeleted)
        {
            return await _context.Set<T>().ToListAsync();
        }

        return await _context.Set<T>().Where(e => EF.Property<bool>(e, "IsDeleted") == false).ToListAsync();
    }

    public async Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public Task SaveChangesAsync() => _context.SaveChangesAsync();

    //updateASync
    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    

}
