using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace CompaneyMvcPL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="first name is required")]
       
        public string FName { get; set; }
        [Required(ErrorMessage = "first name is required")]

        public string LName { get; set; }
        [Required(ErrorMessage ="email is required")]
        [EmailAddress(ErrorMessage ="Not valid email ")]
        public string Email { get; set; }
        [Required(ErrorMessage ="password is required")]
        [DataType(DataType.Password,ErrorMessage ="Password is invalid must contaion speacial character like #$% and numbers  ")]
        public string Password { get; set; }
        [Required(ErrorMessage = "password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password is invalid must contaion speacial character like #$% and numbers  ")]
        [Compare("Password",ErrorMessage ="confirmed password is not equal password ")]
        public string ConfirmedPassword { get; set; }
        [Required(ErrorMessage ="You Must accept our rules ")]
        public bool IsAgree { get; set; }

    }
}
