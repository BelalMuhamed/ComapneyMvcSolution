using AutoMapper;
using CompaneyMvcBLL.Interfaces;
using CompaneyMvcDAL.Migrations;
using CompaneyMvcDAL.Modules;
using CompaneyMvcPL.Helper;
using CompaneyMvcPL.MappingProfiles;
using CompaneyMvcPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CompaneyMvcPL.Controllers
{
    [Authorize]

    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOFwork;
        private readonly IMapper mapper;

        public EmployeeController(IUnitOfWork unitOFwork,IMapper _mapper)
        {
            _unitOFwork = unitOFwork;
            mapper = _mapper;
        }
        #region index
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                employees =await  _unitOFwork.EmployeeRepo.GetAllAsync();
            else
                employees= _unitOFwork.EmployeeRepo.GetByName(SearchValue);

            var mappedemployee=mapper.Map<IEnumerable<Employee>,IEnumerable<EmployeeViewModel>>(employees);
            
            return  View(mappedemployee);
            
        }
        #endregion

        #region create
        public async Task<IActionResult> Create()
        {
            ViewBag.departments = await _unitOFwork.DepartmentRepo.GetAllAsync();
            return View();
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel em)
        {
            if (em.Image != null && em.Image.Length > 0)
            {
                em.ImageName = DocumentSetting.Upload(em.Image, "Images");
            }
            var mappedEmp =mapper.Map<EmployeeViewModel,Employee>(em);
            if (ModelState.IsValid)
            {
                await _unitOFwork.EmployeeRepo.AddAsync(mappedEmp);
                int res = await _unitOFwork.CompleteAsync();
                if (res > 0)
                {
                    TempData["EmpAdded"] = "Employee Added";
                }
                return RedirectToAction("Index");
                
            }


            else
                return View(em);
        }
        #endregion

        #region details
        public async Task<IActionResult> Details(int?id,string ViewName="Details") 
        {
            if (id == null)
                return BadRequest();
            else
            {
                var emp =await _unitOFwork.EmployeeRepo.GetByIdAsync(id.Value);
                if (emp is not null) 
                {
                    var mappedemploee= mapper.Map<Employee, EmployeeViewModel>(emp);
                    return View(mappedemploee);
                }
                else { return NotFound(); }    
            }
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit([FromRoute] int?id)
        {
            ViewBag.departments =await _unitOFwork.EmployeeRepo.GetAllAsync();

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Edit(EmployeeViewModel employee,[FromRoute] int? id)
        {
            if (id != employee.Id)
                return BadRequest();
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        string DeleteImage = employee.ImageName;
                        if (employee.Image != null && employee.Image.Length > 0)
                        {
                            employee.ImageName = DocumentSetting.Upload(employee.Image, "Images");
                        }
                        var mappedEmp = mapper.Map<EmployeeViewModel, Employee>(employee);
                        _unitOFwork.EmployeeRepo.Update(mappedEmp);

                       int result=await _unitOFwork.CompleteAsync();
                        if (result > 0 && DeleteImage is not null&&employee.Image != null && employee.Image.Length > 0)
                        {
                            DocumentSetting.DeleteFile(DeleteImage, "Images");
                        }

                        return RedirectToAction(nameof(Index));
                    }
                    catch (System.Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }


                return View(employee);

            }
        }
        #endregion

        #region Delete
        public  async Task<IActionResult> Delete([FromRoute] int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Delete(EmployeeViewModel employee, [FromRoute] int? id)
        {
            if (id != employee.Id)
                return BadRequest();
            else
            {


                try
                {
                    var em = mapper.Map<EmployeeViewModel,Employee>(employee);
                    _unitOFwork.EmployeeRepo.Delete(em);
                    int result=await _unitOFwork.CompleteAsync();
                    if(result>0 && employee.ImageName is not null)
                    {
                        DocumentSetting.DeleteFile(employee.ImageName, "Images");
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }



                return View(employee);

            }
        }
        #endregion

        
    }
}
