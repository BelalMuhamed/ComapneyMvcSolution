using CompaneyMvcDAL.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompaneyMvcBLL.Interfaces
{
    public interface IGenericRepositry<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task AddAsync(T item);
        public void Update(T item);
        public void Delete(T item);
    }
}
