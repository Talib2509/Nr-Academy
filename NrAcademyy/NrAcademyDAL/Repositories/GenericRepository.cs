using Microsoft.EntityFrameworkCore;
using NrAcademyCORE.Entities.Common;
using NrAcademyCORE.Repositories;
using NrAcademyDAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NrAcademyDAL.Repositories
{

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table { get => _context.Set<T>(); }
        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
            _context.SaveChanges();
        }
        public void Delete(T entity)
        {
            Table.Remove(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            Table.Update(entity);
            _context.SaveChanges();
        }





        public async Task<T> GetByIdAsync(int id)
            => await Table.FindAsync(id);
        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = Table;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }
            return await query.ToListAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }
    }
}
