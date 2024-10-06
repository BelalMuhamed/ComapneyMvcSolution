using AutoMapper;
using CompaneyMvcDAL.Models;
using CompaneyMvcPL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaneyMvcPL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> UserManager,IMapper mapper)
        {
            userManager = UserManager;
            this.mapper = mapper;
        }
        #region Index
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                var Users = await userManager.Users.Select(u => new UsersViewModel
                {
                    Id = u.Id,
                    FName = u.FName,
                    LName = u.LName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = userManager.GetRolesAsync(u).Result
                }).ToListAsync();
                return View(Users);
            }
            else
            {
                var users = await userManager.FindByEmailAsync(SearchValue);

                var modelUser = new UsersViewModel()
                {
                    Id = users.Id,
                    FName = users.FName,
                    LName = users.LName,
                    Email = users.Email,
                    PhoneNumber = users.PhoneNumber,
                    Roles = userManager.GetRolesAsync(users).Result

                };
                return View(new List<UsersViewModel> { modelUser });

            }

        }
        #endregion

        #region details
        public async Task<IActionResult> Details([FromRoute]string? id,string ViewName="Details" )
        {
            if (string.IsNullOrEmpty(id)) 
            {
                return BadRequest();
            }
            else
            {
               var user =await userManager.FindByIdAsync(id);
                if (user != null) 
                {
                    var modelUser = mapper.Map<ApplicationUser, UsersViewModel>(user);
                    return View(modelUser);
                }
                else
                {
                    return NotFound();
                }

            }
        }
        #endregion

        #region EditGet
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id,"Edit");
        }
        #endregion

        #region EditPost
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(UsersViewModel model, [FromRoute] string? id)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(id);
                    user.FName= model.FName;
                    user.LName= model.LName;
                    user.PhoneNumber= model.PhoneNumber;
                    await userManager.UpdateAsync(user);
                    
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("empty", ex.Message);
                }
            }
           return View(model);   
        }
        #endregion

        #region Delete 
        public async Task<IActionResult> Delete(string? id)
        {
            return await  Details(id, "Delete");
        }
        #endregion

        #region DeletePost 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(UsersViewModel model ,[FromRoute]string? id)
        {
            if(id != model.Id)
            {
                return  BadRequest();
            }
            var user= await userManager.FindByIdAsync(id);
            
            if (user is not null)
            {
                try
                {
                    await userManager.DeleteAsync(user);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Empty", "cannot delete this user ");
                }
            }
            
                return  View(model); ;
            
        }
        #endregion


    }
}
