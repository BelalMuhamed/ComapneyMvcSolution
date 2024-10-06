namespace CompaneyMvcDAL.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompaneyMvcDAL.Models;
using CompaneyMvcDAL.Modules;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


    public class ComapneyMvcDbContext : IdentityDbContext<ApplicationUser>
    {
    public ComapneyMvcDbContext (DbContextOptions<ComapneyMvcDbContext> options):base(options)
    {

    }
       public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }

}

