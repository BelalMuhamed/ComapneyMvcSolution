using AutoMapper;
using CompaneyMvcDAL.Models;
using CompaneyMvcPL.Models;
using CompaneyMvcPL.ViewModels;

namespace CompaneyMvcPL.MappingProfiles
{
    public class UserProfile:Profile
    {
        public  UserProfile()
        {
            CreateMap<UsersViewModel, ApplicationUser>().ReverseMap();


        }
    }
}
