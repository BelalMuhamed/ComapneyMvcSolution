using CompaneyMvcDAL.Models;
using CompaneyMvcPL.Helper;
using CompaneyMvcPL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
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
                    return RedirectToAction("Login", "_Account");
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
                var user = await _useManage.FindByEmailAsync(model.Email);
                if (user is not null) 
                {
                    var token = await _useManage.GeneratePasswordResetTokenAsync(user);
                    var PasswordLink = Url.Action("ResetPassword", "_Account", new { email = user.Email, Token = token }, Request.Scheme);
                   
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "reset password",
                        Body = PasswordLink
                    };
                    EmailSttings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourEmail));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "this is email not exist");
                }
            }
            return View("ForgetPassword",model);
        }
        public IActionResult CheckYourEmail()
        {
            
            return View();
        }
        public IActionResult ResetPassword(string email,string token)
        {
            TempData["Email"] = email;
            TempData["token"] = token;


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel PassModel)
        {
            if(ModelState.IsValid)
            {
                var email = TempData["Email"] as string;
                var token = TempData["token"] as string;
                var user =await _useManage.FindByEmailAsync(email);
               var result =   await _useManage.ResetPasswordAsync(user, token,PassModel.NewPassword);
                if (result.Succeeded) 
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty,item.Description);
                    }
                }
            }
           

            return View(PassModel);
        }

        public IActionResult AccessDenied()
        {

        return View();
        }    
    
    }
}
