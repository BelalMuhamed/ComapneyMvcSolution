using CompaneyMvcBLL.Interfaces;
using CompaneyMvcDAL.DbContext;
using CompaneyMvcDAL.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompaneyMvcBLL.Repositries
{
    public class DepartmentRepositry :GenericRepositry<Department>, IDepartmentRepositry
    {
       

        public DepartmentRepositry(ComapneyMvcDbContext dbContext):base(dbContext) 
        {
            
        }

       
    }
}
