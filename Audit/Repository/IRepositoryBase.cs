using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Audit.Repository
{
    public interface IRepositoryBase<T>
    {
        Task<List<T>> FindAll();
        Task<T> FindById(string id,bool closeConnection);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
