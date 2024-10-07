using AutoMapper;
using CompaneyMvcBLL.Interfaces;
using CompaneyMvcDAL.Modules;
using CompaneyMvcPL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompaneyMvcPL.Controllers
{
    [Authorize]

    public class DepartmentController : Controller
    {
        
        private readonly IUnitOfWork _unitOfwork;
        private readonly IMapper mapper;

        public DepartmentController(IUnitOfWork unitOfwork,IMapper _mapper)
        {
          
            _unitOfwork = unitOfwork;
            mapper = _mapper;
        }
        public async Task<IActionResult> Index()
        {
            var departments =await _unitOfwork.DepartmentRepo.GetAllAsync();

            var Mapped = mapper.Map<IEnumerable<Department>,IEnumerable<DeparmentViewModel>>(departments); 
            return View(Mapped);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DeparmentViewModel department)
        {
            if (ModelState.IsValid)
            {
                
                var Mapped = mapper.Map<DeparmentViewModel,Department>(department);
                    await _unitOfwork.DepartmentRepo.AddAsync(Mapped);
                int res =await  _unitOfwork.CompleteAsync();

                if (res >0)
                {
                    if (res > 0)
                    {
                        TempData["EmpAdded"] = "Department Added sucessfuly";
                    }
                }

                return RedirectToAction("Index");
            }
            else
                return View(department);
        }
        public async  Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            else
            {
                var department = await _unitOfwork.DepartmentRepo.GetByIdAsync(id.Value);
                if (department is null)
                    return NotFound();
                else
                {
                    var Mapped = mapper.Map<Department, DeparmentViewModel>(department);
                    return View(Mapped);
                }
            }
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DeparmentViewModel department, [FromRoute] int? id)
        {
            if (id != department.Id)
                return BadRequest();
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var Mapped = mapper.Map<DeparmentViewModel, Department>(department);

                        _unitOfwork.DepartmentRepo.Update(Mapped);
                       await _unitOfwork.CompleteAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (System.Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }


                return View(department);

            }
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeparmentViewModel department, int? id)
        {
            if (id != department.Id)
                return BadRequest();
            else
            {
                
                
                    try
                    {
                    var Mapped = mapper.Map<DeparmentViewModel, Department>(department);
                    _unitOfwork.DepartmentRepo.Delete(Mapped);
                   await _unitOfwork.CompleteAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (System.Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                


                return View(department);

            }
        }
    }
}
