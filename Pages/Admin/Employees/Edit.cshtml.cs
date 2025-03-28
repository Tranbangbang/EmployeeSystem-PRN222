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
    public class EditModel : PageModel
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
            public int Id { get; set; }

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
            public DateTime HireDate { get; set; }

            [Required(ErrorMessage = "Phòng ban là b?t bu?c")]
            [Display(Name = "Phòng ban")]
            public int DepartmentId { get; set; }

            [Required(ErrorMessage = "Vai trò là b?t bu?c")]
            [Display(Name = "Vai trò")]
            public int RoleId { get; set; }

            [StringLength(100, ErrorMessage = "M?t kh?u ph?i có ít nh?t {2} ký t?", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "M?t kh?u m?i (?? tr?ng n?u không ??i)")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nh?n m?t kh?u m?i")]
            [Compare("Password", ErrorMessage = "M?t kh?u và xác nh?n m?t kh?u không kh?p")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Tr?ng thái ho?t ??ng")]
            public bool IsActive { get; set; }
        }

        public EditModel(
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            IRoleService roleService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _roleService = roleService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = employee.Id,
                EmployeeCode = employee.EmployeeCode,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                HireDate = employee.HireDate,
                DepartmentId = employee.DepartmentId,
                RoleId = employee.RoleId,
                IsActive = employee.IsActive
            };

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

            var employee = await _employeeService.GetEmployeeByIdAsync(Input.Id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.EmployeeCode = Input.EmployeeCode;
            employee.FirstName = Input.FirstName;
            employee.LastName = Input.LastName;
            employee.Email = Input.Email;
            employee.PhoneNumber = Input.PhoneNumber;
            employee.HireDate = Input.HireDate;
            employee.DepartmentId = Input.DepartmentId;
            employee.RoleId = Input.RoleId;
            employee.IsActive = Input.IsActive;

            if (!string.IsNullOrEmpty(Input.Password))
            {
                employee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Input.Password);
            }

            var result = await _employeeService.UpdateEmployeeAsync(employee);

            if (result)
            {
                TempData["SuccessMessage"] = "C?p nh?t nhân viên thành công";
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "C?p nh?t nhân viên th?t b?i. Vui lòng th? l?i.");
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