using AutoMapper;
using CompaneyMvcDAL.Modules;
using CompaneyMvcPL.Models;

namespace CompaneyMvcPL.MappingProfiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile() 
        {
                CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
