using AutoMapper;
using CompaneyMvcDAL.Models;
using CompaneyMvcPL.Models;
using CompaneyMvcPL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CompaneyMvcPL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> usermanager;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper,UserManager<ApplicationUser> manageRole) 
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.usermanager = manageRole;
        }
        #region index
        public async Task<IActionResult> Index(string SearchValue)
        {

            if (string.IsNullOrEmpty(SearchValue))
            {
                var Roles = await roleManager.Roles.ToListAsync();
                var MappedRoles = mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(Roles);
                return View(MappedRoles);
            }
            else
            {
                var roles = await roleManager.FindByNameAsync(SearchValue);
                var mappedroles = mapper.Map<IdentityRole, RoleViewModel>(roles);
                return View(new List<RoleViewModel>() { mappedroles});
            }

            

            
        }
        #endregion

        #region createget
        public IActionResult Create()
        {
            return View();
        }
        #endregion


        #region createpost
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel Role)
        {
            if (ModelState.IsValid) 
            {
                var mappedrole = mapper.Map<RoleViewModel,IdentityRole>(Role);
                 var result =await roleManager.CreateAsync(mappedrole);
                if(result.Succeeded)
                {
                    TempData["RoleAdded"] = "Role Added";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);

                    }
                }
            }
            return View(Role);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details([FromRoute]string id,string ViewName="Details")
        {
            if (id == null)
                return BadRequest();
            else
            {
                
                var role = await roleManager.FindByIdAsync(id);
                if (role is not null)
                {
                    var MappedRole = mapper.Map<IdentityRole, RoleViewModel>(role);
                    return View(MappedRole);
                }
                else
                {
                    return NotFound();
                }
            }

        }
        #endregion

        #region DeleteGet
        public async Task<IActionResult> Delete(string?id)
        {
            return await Details(id);
        }
        #endregion

        #region DeletePost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RoleViewModel role,[FromRoute]string id)
        {
            if(id != role.Id)
                return BadRequest();
            if (role is not null) 
            {
                try
                {
                    var Role =await roleManager.FindByIdAsync(id);
                    await roleManager.DeleteAsync(Role);

                    return RedirectToAction("Index");


                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError("empty",ex.Message);
                }
            }
            return View(role);
        }

        #endregion

        #region EditGet
        public async  Task<IActionResult> Edit([FromRoute]string id)
        {
            return await Details(id, "Edit");
        }
        #endregion

        #region EditPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit(RoleViewModel role, string id )
        {
            if(id != role.Id)
                return BadRequest();
            if (ModelState.IsValid) 
            {
                try
                {
                    var IRole = await roleManager.FindByIdAsync(id);
                    IRole.Name=role.roleName;
                    await roleManager.UpdateAsync(IRole);
                    return RedirectToAction("Index");
                    
                }
                catch (Exception ex) 
                {
                    ModelState.AddModelError("empty", ex.Message);
                }

            }
            return View(role);
        }
        #endregion
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role =await roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return BadRequest();

            }
            ViewData["RoleId"]=roleId;
            var usersInRole =new List<AddOrRemoveUser>();
            var users = await usermanager.Users.ToListAsync();
            foreach(var user in users)
            {
                var userInrole = new AddOrRemoveUser()
                {
                    UsrId = user.Id,
                    UserName = user.UserName,
                };

                if (await usermanager.IsInRoleAsync(user, role.Name))
                {
                    userInrole.IsSelected = true;
                }
                else
                {
                    userInrole.IsSelected =false;
                }
               usersInRole.Add(userInrole);
            }
            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleid, List<AddOrRemoveUser> users)
        {
            var role = await roleManager.FindByIdAsync(roleid);
            if (role is null)
            {
                return BadRequest();

            }
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var Appuser = await usermanager.FindByIdAsync(user.UsrId);
                    if (user.IsSelected && ! await usermanager.IsInRoleAsync(Appuser,role.Name))
                    {
                        await usermanager.AddToRoleAsync(Appuser, role.Name);
                    }
                    else
                    {
                        await usermanager.RemoveFromRoleAsync(Appuser, role.Name);
                    }
                }

                return RedirectToAction("Edit", new { id = roleid });
            }
            return View(users);
        }

        }
}
