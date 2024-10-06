using System.ComponentModel.DataAnnotations;

namespace CompaneyMvcPL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password is invalid must contaion speacial character like #$% and numbers  ")]
        
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "cofirm password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password is invalid must contaion speacial character like #$% and numbers like 123  ")]
        [Compare("NewPassword", ErrorMessage = "confirmed password dosen't match ")]
        public string ConfirmNewPassword { get; set; }

    }
}
