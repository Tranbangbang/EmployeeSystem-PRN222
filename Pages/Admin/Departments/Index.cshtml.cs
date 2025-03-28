using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployManager.Pages.Admin.Departments
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IDepartmentService _departmentService;

        public List<Department> Departments { get; set; }

        [BindProperty]
        public Department Department { get; set; }

        public IndexModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task OnGetAsync()
        {
            Departments = await _departmentService.GetAllDepartmentsAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            await _departmentService.CreateDepartmentAsync(Department);
            TempData["SuccessMessage"] = "Th�m ph�ng ban th�nh c�ng";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            await _departmentService.UpdateDepartmentAsync(Department);
            TempData["SuccessMessage"] = "C?p nh?t ph�ng ban th�nh c�ng";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            if (department.Employees != null && department.Employees.Count > 0)
            {
                TempData["ErrorMessage"] = "Kh�ng th? x�a ph�ng ban ?ang c� nh�n vi�n";
                return RedirectToPage();
            }

            await _departmentService.DeleteDepartmentAsync(id);
            TempData["SuccessMessage"] = "X�a ph�ng ban th�nh c�ng";
            return RedirectToPage();
        }
    }
}
