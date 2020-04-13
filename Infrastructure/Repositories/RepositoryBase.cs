using Infrastructure.Data;
using Infrastructure.IRepositories;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseModel
    {
        private DataContext _context;

        public RepositoryBase(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;
            return true; ;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
