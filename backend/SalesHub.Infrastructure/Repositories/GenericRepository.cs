using Microsoft.EntityFrameworkCore;
using SalesHub.Domain.Common;
using SalesHub.Domain.Interfaces;
using SalesHub.Infrastructure.Persistence;

namespace SalesHub.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class, IEntityBase
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _context.Set<T>().ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _context.Set<T>().FindAsync(id);

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        var existing = await _context.Set<T>().FindAsync(entity.Id);

        if (existing != null)
            _context.Entry(existing).CurrentValues.SetValues(entity);
        else
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
