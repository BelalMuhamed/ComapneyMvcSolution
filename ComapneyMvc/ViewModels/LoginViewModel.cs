using System.ComponentModel.DataAnnotations;

namespace CompaneyMvcPL.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "Not valid email ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password is invalid must contaion speacial character like #$% and numbers  ")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
