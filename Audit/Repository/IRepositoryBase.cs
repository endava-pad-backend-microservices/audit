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
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
