using CompaneyMvcBLL.Interfaces;
using CompaneyMvcDAL.DbContext;
using CompaneyMvcDAL.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompaneyMvcBLL.Repositries
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : class
    {
        private readonly ComapneyMvcDbContext dbContext;

        public GenericRepositry(ComapneyMvcDbContext _DbContext) 
        {
            dbContext = _DbContext;
        }

        public async Task AddAsync(T item)
        {
            await dbContext.AddAsync(item);    
           
        }

       
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T).IsAssignableFrom(typeof(Employee)))
           { return (IEnumerable<T>) await dbContext.Employees.Include(e => e.Department).ToListAsync(); }
            else
            {
                return await dbContext.Set<T>().ToListAsync();
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
           return await dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
             dbContext.Set<T>().Update(item);
           
        }
        public void Delete(T item)
        {
            dbContext.Remove(item);

        }

    }
}
