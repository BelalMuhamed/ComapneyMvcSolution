using CompaneyMvcBLL.Interfaces;
using CompaneyMvcDAL.DbContext;
using CompaneyMvcDAL.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CompaneyMvcBLL.Repositries
{
    public class EmployeeRepositry:GenericRepositry<Employee>,IEmployeeRepositry
    {
        private readonly ComapneyMvcDbContext context;

        public EmployeeRepositry(ComapneyMvcDbContext _context):base(_context)  
        {
            context = _context;
        }

        public IQueryable<Employee> GetByAddress(string address)
        {
            return context.Employees.Where( e=> e.Address == address);
        }

        public IQueryable<Employee> GetByName(string name)
        {
            return context.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower()));
        }
    }
}
