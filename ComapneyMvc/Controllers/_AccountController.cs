using CompaneyMvcDAL.Models;
using CompaneyMvcPL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;

namespace CompaneyMvcPL.Controllers
{
    public class _AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _useManage;
        private readonly SignInManager<ApplicationUser> _signInManag;

        public  _AccountController(UserManager<ApplicationUser> UseManage,SignInManager<ApplicationUser> SignInManag)
        {
            _useManage = UseManage;
            _signInManag = SignInManag;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel RegModel)
        {
            if (ModelState.IsValid) 
            {
                var User = new ApplicationUser()
                {
                    UserName = RegModel.Email.Split('@')[0],
                    Email = RegModel.Email,
                   FName=RegModel.FName,
                   LName = RegModel.LName,
                   IsAgree = RegModel.IsAgree
                };
                var Result = await _useManage.CreateAsync(User,RegModel.Password);
                if(Result.Succeeded)
                {
                    return RedirectToAction("Index","Employee");
                }
                else
                {
                    foreach (var item in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                        
                    }
                }
            }
            return View(RegModel);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel LogModel)
        {
            if (ModelState.IsValid) 
            {
                var User = await _useManage.FindByEmailAsync(LogModel.Email);
                if (User is not null) 
                {
                    var Flag = await _useManage.CheckPasswordAsync(User, LogModel.Password);
                    if(Flag==true)
                    {
                        var result = await _signInManag.PasswordSignInAsync(User, LogModel.Password,LogModel.RememberMe,false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("empty", "invalid email");
                }

            }
            return View(LogModel);
        }
       
        public async Task<IActionResult>Signout()
        {
             await _signInManag.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult ForgetPassword()
        {
            return View();  
        }
        [HttpPost]
        public  async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _useManage.FindByEmailAsync(model.Email);
                if (result is not null) 
                {
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "reset password",
                        Body = "reset password link"
                    };
                   
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "this is email not exist");
                }
            }
            return View("ForgetPassword",model);
        }
    }
}
