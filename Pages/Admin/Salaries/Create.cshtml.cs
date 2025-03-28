using EmployManager.Models;
using EmployManager.Service.impl;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployManager.Pages.Admin.Salaries
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ISalaryService _salaryService;
        private readonly IEmployeeService _employeeService;

        [BindProperty]
        public Salary Salary { get; set; }

        public SelectList Employees { get; set; }

        public CreateModel(ISalaryService salaryService, IEmployeeService employeeService)
        {
            _salaryService = salaryService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadEmployeesAsync();
            Salary = new Salary
            {
                Month = DateTime.Now.Month,
                Year = DateTime.Now.Year,
                BasicSalary = 0,
                Allowance = 0,
                Bonus = 0,
                Deduction = 0,
                Status = "Ch?a thanh toán"
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployeesAsync();
                return Page();
            }
            var existingSalaries = await _salaryService.GetSalariesByMonthYearAsync(Salary.Month, Salary.Year);
            if (existingSalaries.Any(s => s.EmployeeId == Salary.EmployeeId))
            {
                ModelState.AddModelError(string.Empty, "?ã t?n t?i b?ng l??ng cho nhân viên này trong tháng ?ã ch?n");
                await LoadEmployeesAsync();
                return Page();
            }

            Salary.Status = "Ch?a thanh toán";
            await _salaryService.CreateSalaryAsync(Salary);

            TempData["SuccessMessage"] = "T?o b?ng l??ng thành công";
            return RedirectToPage("./Index", new { month = Salary.Month, year = Salary.Year });
        }

        private async Task LoadEmployeesAsync()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var activeEmployees = employees.Where(e => e.IsActive).ToList();
            Employees = new SelectList(activeEmployees, "Id", "FullName");
        }
    }
}