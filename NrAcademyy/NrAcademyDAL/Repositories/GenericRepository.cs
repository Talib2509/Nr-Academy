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

    public class GenericRepository<T>(AppDbContext _context) : IGenericRepository<T> where T : BaseEntity, new()
    {
        protected DbSet<T> Table = _context.Set<T>();
        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
        }
        public void Delete(T entity)
        {
            Table.Remove(entity);
        }
        public void Update(T entity)
        {
            Table.Update(entity);
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
    }
}
