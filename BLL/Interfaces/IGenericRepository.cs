using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGenericRepository<T> where T : EntityBase
    {
        Task<T>GetEntityById(int? id);
        Task<IEnumerable<T>> GetAll();
        Task Add (T entity);
        Task Update (T entity);
        Task Delete (T entity);

    }
}
