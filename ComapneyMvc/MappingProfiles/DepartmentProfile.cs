using AutoMapper;
using CompaneyMvcDAL.Modules;
using CompaneyMvcPL.Models;
using CompaneyMvcPL.ViewModels;

namespace CompaneyMvcPL.MappingProfiles
{
    public class DepartmentProfile:Profile
    {
        public  DepartmentProfile()
        {
            CreateMap<DeparmentViewModel,Department>().ReverseMap();
        }
    }
}
