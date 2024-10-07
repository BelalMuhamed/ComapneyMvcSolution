using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompaneyMvcBLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IEmployeeRepositry EmployeeRepo { get; set; }
        public IDepartmentRepositry DepartmentRepo { get; set; }
        public Task<int> CompleteAsync();
    }
}
