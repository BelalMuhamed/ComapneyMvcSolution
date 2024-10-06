using CompaneyMvcBLL.Interfaces;
using CompaneyMvcDAL.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompaneyMvcBLL.Repositries
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly ComapneyMvcDbContext _context;

        public IEmployeeRepositry EmployeeRepo { get ; set; }
        public IDepartmentRepositry DepartmentRepo { get ; set ; }
        public UnitOfWork(ComapneyMvcDbContext context) 
        {
            EmployeeRepo = new EmployeeRepositry(context);
            DepartmentRepo = new DepartmentRepositry(context);

            _context = context;
        }

        public async Task<int> CompleteAsync()
        => await _context.SaveChangesAsync();

        public void Dispose()
        { _context.Dispose(); }
      
    }
}
