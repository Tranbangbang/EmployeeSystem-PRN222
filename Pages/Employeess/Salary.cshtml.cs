using EmployManager.Models;
using EmployManager.Service.impl;
using EmployManager.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace EmployManager.Pages.Employeess
{
    [Authorize]
    public class SalaryModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeService _employeeService;

        [BindProperty]
        public Leave LeaveRequest { get; set; }

        public List<Leave> MyLeaves { get; set; }
        public List<Employee> Employees { get; set; }
        public int EmployeeId { get; set; }

        public SalaryModel(IAuthService authService, ILeaveService leaveService, IEmployeeService employeeService)
        {
            _authService = authService;
            _leaveService = leaveService;
            _employeeService = employeeService;
        }


        public async Task<List<Employee>> getAll()
        {
            return await _employeeService.GetAllEmployeesAsync();
        }

        /// <summary>
        /// ----------------------------------------------------
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            Console.WriteLine("-------------------------------- get get evgtegeyee");
            Employees = await _employeeService.GetAllEmployeesAsync();

            // neu k co nhan vien 
            if (Employees == null)
            {
                Employees = new List<Employee>();  
            }

            //return Page();
        }

        [BindProperty]
        public int employeeId { get; set; }

        [BindProperty]
        public double UpdatedSalary { get; set; }

        [BindProperty]
        public double UpdatedBonus { get; set; }

        [BindProperty]
        public double UpdatedDeduction { get; set; }

        public async Task<IActionResult> OnPostUpdateSalaryAsync()
        {
             Console.WriteLine(">>>> Entering OnPostUpdateSalaryAsync");
            Console.WriteLine($"EmployeeId: {employeeId}");
            Console.WriteLine($"UpdatedSalary: {UpdatedSalary}");
            Console.WriteLine($"UpdatedBonus: {UpdatedBonus}");
            Console.WriteLine($"UpdatedDeduction: {UpdatedDeduction}");

            // Tìm nhân viên trong db
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee != null)
            {
                 employee.BasicSalary = UpdatedSalary;
                employee.Bonus = UpdatedBonus;
                employee.Deduction = UpdatedDeduction;

                // Update trong db 
                await _employeeService.UpdateEmployeeAsync(employee);
            }
            else
            {
                 Console.WriteLine($"can not find ID: {employeeId}");
            }

             return RedirectToPage();
        }
    }
}
