using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployManager.Pages.Admin.Departments
{
    [Authorize(Roles = "Admin")]
    public class DetailDepartmentModel : PageModel
    {
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;

        public Department Department { get; set; }
        public List<Employee> EmployeeList { get; set; }

        public DetailDepartmentModel(IDepartmentService departmentService, IEmployeeService employeeService)
        {
            _departmentService = departmentService;
            _employeeService = employeeService;
        }

        public async Task OnGetAsync(int id)
        {
            Department = await _departmentService.GetDepartmentByIdAsync(id);
            EmployeeList = await _employeeService.GetEmployeeByDepartmentAsync(id);
        }

    }
}
