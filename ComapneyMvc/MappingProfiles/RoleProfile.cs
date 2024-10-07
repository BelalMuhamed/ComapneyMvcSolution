using AutoMapper;
using CompaneyMvcPL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CompaneyMvcPL.MappingProfiles
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
           CreateMap<IdentityRole,RoleViewModel>().ForMember(d=>d.roleName,o=>o.MapFrom(s=>s.Name)).ReverseMap();
        }
    }
}
