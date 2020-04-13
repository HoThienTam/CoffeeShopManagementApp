using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepositories
{
    public interface IRepositoryBase<T> where T : BaseModel
    {
        IEnumerable<T> GetAll();
        Task<T> GetByIdAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
