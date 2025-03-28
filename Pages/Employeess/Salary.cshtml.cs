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
        private readonly ISalaryService _salaryService;

        [BindProperty]
        public Leave LeaveRequest { get; set; }

        [BindProperty]
        public int EmployeeId { get; set; }

        [BindProperty]
        public double UpdatedSalary { get; set; }

        [BindProperty]
        public double UpdatedBonus { get; set; }

        [BindProperty]
        public double UpdatedDeduction { get; set; }

        [BindProperty]
        public double UpdatedAllowance { get; set; }

        public List<Leave> MyLeaves { get; set; }
        public List<Employee> Employees { get; set; }
        //public int EmployeeId { get; set; }

        public SalaryModel(IAuthService authService, ILeaveService leaveService, IEmployeeService employeeService, ISalaryService salaryService)
        {
            _authService = authService;
            _leaveService = leaveService;
            _employeeService = employeeService;
            _salaryService = salaryService;
        }


        public async Task<List<Employee>> getAll()
        {
            return await _employeeService.GetAllEmployeesAsync();
        }

        /// ----------------------------------------------------

        public async Task OnGetAsync()
        {
            Employees = await _employeeService.GetAllEmployeesAsync();

            // neu k co nhan vien 
            if (Employees == null)
            {
                Employees = new List<Employee>();  
            }

            //return Page();
        }


        public async Task<IActionResult> OnPostUpdateSalaryAsync()
        {
            //Console.WriteLine(">>>> Entering OnPostUpdateSalaryAsync");
            //Console.WriteLine($"EmployeeId: {EmployeeId}");
            //Console.WriteLine($"UpdatedSalary: {UpdatedSalary}");
            //Console.WriteLine($"UpdatedBonus: {UpdatedBonus}");
            //Console.WriteLine($"UpdatedDeduction: {UpdatedDeduction}");

            Console.WriteLine("______________________________________________________________");
            List<EmployeeSalaryResponse> datas = await _salaryService.GetAllSalaty(3, 2025);
            foreach (var data in datas) {
                Console.WriteLine(data.ToString());
            }

            // tim trong db
            var employee = await _employeeService.GetEmployeeByIdAsync(EmployeeId);
            if (employee != null)
            {
                employee.BasicSalary = UpdatedSalary;
                employee.Bonus = UpdatedBonus;
                employee.Deduction = UpdatedDeduction;
                employee.Allowance = UpdatedAllowance;

                // Update trong db 
                await _employeeService.UpdateEmployeeAsync(employee);
            }
            else
            {
                Console.WriteLine($"can not find ID: {EmployeeId}");
            }

            return RedirectToPage();
        }
    }
}
