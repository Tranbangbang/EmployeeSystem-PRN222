using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EmployManager.Pages.Admin.Employees
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public Employee Employee { get; set; }

        public DeleteModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (Employee == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Nh�n vi�n ?� ???c v� hi?u h�a th�nh c�ng";
            return RedirectToPage("./Index");
        }
    }
}