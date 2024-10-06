using CompaneyMvcDAL.Migrations;
using CompaneyMvcDAL.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompaneyMvcBLL.Interfaces
{
    public interface IEmployeeRepositry:IGenericRepositry<Employee>
    {
        public IQueryable<Employee> GetByAddress(string address);
        public IQueryable<Employee> GetByName(string name);

    }
}
