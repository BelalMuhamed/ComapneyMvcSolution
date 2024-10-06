using System;
using System.ComponentModel.DataAnnotations;

namespace CompaneyMvcPL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage ="Name is required !!")]
        public string roleName { get; set; }
        public RoleViewModel() 
        {
            Id=Guid.NewGuid().ToString();   
        }
    }
}
