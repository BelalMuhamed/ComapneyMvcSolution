using System.ComponentModel.DataAnnotations;

namespace CompaneyMvcPL.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage ="you must add your email")]
        [EmailAddress(ErrorMessage ="must be email address")]
        public string Email { get; set; }
    }
}
