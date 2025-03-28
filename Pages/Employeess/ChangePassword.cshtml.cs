using EmployManager.Service;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace EmployManager.Pages.Employeess
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IEmployeeService _employeeService;

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "M?t kh?u hi?n t?i là b?t bu?c")]
            [DataType(DataType.Password)]
            [Display(Name = "M?t kh?u hi?n t?i")]
            public string CurrentPassword { get; set; }

            [Required(ErrorMessage = "M?t kh?u m?i là b?t bu?c")]
            [StringLength(100, ErrorMessage = "M?t kh?u ph?i có ít nh?t {2} ký t?", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "M?t kh?u m?i")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Xác nh?n m?t kh?u m?i")]
            [Compare("NewPassword", ErrorMessage = "M?t kh?u m?i và xác nh?n m?t kh?u không kh?p")]
            public string ConfirmPassword { get; set; }
        }

        public ChangePasswordModel(IAuthService authService, IEmployeeService employeeService)
        {
            _authService = authService;
            _employeeService = employeeService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int employeeId = _authService.GetCurrentEmployeeId(HttpContext);
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            // Ki?m tra m?t kh?u hi?n t?i
            bool isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(Input.CurrentPassword, employee.PasswordHash);
            if (!isCurrentPasswordValid)
            {
                ModelState.AddModelError(string.Empty, "M?t kh?u hi?n t?i không ?úng");
                return Page();
            }

            // C?p nh?t m?t kh?u m?i
            employee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Input.NewPassword);
            var result = await _employeeService.UpdateEmployeeAsync(employee);

            if (result)
            {
                TempData["SuccessMessage"] = "??i m?t kh?u thành công";
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "??i m?t kh?u th?t b?i. Vui lòng th? l?i.");
                return Page();
            }
        }
    }
}
