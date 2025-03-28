using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmployManager.Pages.Admin.Employees
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IRoleService _roleService;

        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList Departments { get; set; }
        public SelectList Roles { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "M� nh�n vi�n l� b?t bu?c")]
            [Display(Name = "M� nh�n vi�n")]
            [StringLength(20, ErrorMessage = "M� nh�n vi�n kh�ng ???c v??t qu� 20 k� t?")]
            public string EmployeeCode { get; set; }

            [Required(ErrorMessage = "H? l� b?t bu?c")]
            [Display(Name = "H?")]
            [StringLength(50, ErrorMessage = "H? kh�ng ???c v??t qu� 50 k� t?")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "T�n l� b?t bu?c")]
            [Display(Name = "T�n")]
            [StringLength(50, ErrorMessage = "T�n kh�ng ???c v??t qu� 50 k� t?")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email l� b?t bu?c")]
            [EmailAddress(ErrorMessage = "Email kh�ng h?p l?")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "S? ?i?n tho?i kh�ng h?p l?")]
            [Display(Name = "S? ?i?n tho?i")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Ng�y v�o l�m l� b?t bu?c")]
            [DataType(DataType.Date)]
            [Display(Name = "Ng�y v�o l�m")]
            public DateTime HireDate { get; set; } = DateTime.Now;

            [Required(ErrorMessage = "Ph�ng ban l� b?t bu?c")]
            [Display(Name = "Ph�ng ban")]
            public int DepartmentId { get; set; }

            [Required(ErrorMessage = "Vai tr� l� b?t bu?c")]
            [Display(Name = "Vai tr�")]
            public int RoleId { get; set; }

            [Required(ErrorMessage = "M?t kh?u l� b?t bu?c")]
            [StringLength(100, ErrorMessage = "M?t kh?u ph?i c� �t nh?t {2} k� t?", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "M?t kh?u")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "X�c nh?n m?t kh?u")]
            [Compare("Password", ErrorMessage = "M?t kh?u v� x�c nh?n m?t kh?u kh�ng kh?p")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Tr?ng th�i ho?t ??ng")]
            public bool IsActive { get; set; } = true;
        }

        public CreateModel(
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            IRoleService roleService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _roleService = roleService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadSelectListsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var employee = new Employee
            {
                EmployeeCode = Input.EmployeeCode,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                HireDate = Input.HireDate,
                DepartmentId = Input.DepartmentId,
                RoleId = Input.RoleId,
                IsActive = Input.IsActive
            };

            var result = await _employeeService.CreateEmployeeAsync(employee, Input.Password);

            if (result)
            {
                TempData["SuccessMessage"] = "Th�m nh�n vi�n th�nh c�ng";
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Th�m nh�n vi�n th?t b?i. Vui l�ng th? l?i.");
                await LoadSelectListsAsync();
                return Page();
            }
        }

        private async Task LoadSelectListsAsync()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            Departments = new SelectList(departments, "Id", "Name");

            var roles = await _roleService.GetAllRolesAsync();
            Roles = new SelectList(roles, "Id", "Name");
        }
    }
}