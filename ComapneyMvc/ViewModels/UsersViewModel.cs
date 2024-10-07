using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompaneyMvcPL.ViewModels
{
    public class UsersViewModel
    {
        
        public string Id { get; set; }
        [Required(ErrorMessage ="First name is required")]
        public string FName { get; set; }
        [Required(ErrorMessage ="Last name is required")]
        public string LName { get; set; }
        
        public string Email { get; set; }
        [Phone(ErrorMessage ="this must be numbers ")]
        public string PhoneNumber { get; set; }
        public IEnumerable<string>  Roles { get; set; }
    }
}
