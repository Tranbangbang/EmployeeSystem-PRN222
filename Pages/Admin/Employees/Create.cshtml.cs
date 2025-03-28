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
            [Required(ErrorMessage = "Mã nhân viên là b?t bu?c")]
            [Display(Name = "Mã nhân viên")]
            [StringLength(20, ErrorMessage = "Mã nhân viên không ???c v??t quá 20 ký t?")]
            public string EmployeeCode { get; set; }

            [Required(ErrorMessage = "H? là b?t bu?c")]
            [Display(Name = "H?")]
            [StringLength(50, ErrorMessage = "H? không ???c v??t quá 50 ký t?")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Tên là b?t bu?c")]
            [Display(Name = "Tên")]
            [StringLength(50, ErrorMessage = "Tên không ???c v??t quá 50 ký t?")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email là b?t bu?c")]
            [EmailAddress(ErrorMessage = "Email không h?p l?")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Phone(ErrorMessage = "S? ?i?n tho?i không h?p l?")]
            [Display(Name = "S? ?i?n tho?i")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Ngày vào làm là b?t bu?c")]
            [DataType(DataType.Date)]
            [Display(Name = "Ngày vào làm")]
            public DateTime HireDate { get; set; } = DateTime.Now;

            [Required(ErrorMessage = "Phòng ban là b?t bu?c")]
            [Display(Name = "Phòng ban")]
            public int DepartmentId { get; set; }

            [Required(ErrorMessage = "Vai trò là b?t bu?c")]
            [Display(Name = "Vai trò")]
            public int RoleId { get; set; }

            [Required(ErrorMessage = "M?t kh?u là b?t bu?c")]
            [StringLength(100, ErrorMessage = "M?t kh?u ph?i có ít nh?t {2} ký t?", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "M?t kh?u")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nh?n m?t kh?u")]
            [Compare("Password", ErrorMessage = "M?t kh?u và xác nh?n m?t kh?u không kh?p")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Tr?ng thái ho?t ??ng")]
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
                TempData["SuccessMessage"] = "Thêm nhân viên thành công";
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Thêm nhân viên th?t b?i. Vui lòng th? l?i.");
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