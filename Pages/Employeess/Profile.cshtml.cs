using EmployManager.Models;
using EmployManager.Service.impl;
using EmployManager.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace EmployManager.Pages.Employeess
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeProfileService _profileService;

        [BindProperty]
        public Models.Employee Employee { get; set; }

        [BindProperty]
        public EmployeeProfile Profile { get; set; }

        [BindProperty]
        public IFormFile ProfileImage { get; set; }

        public ProfileModel(
            IAuthService authService,
            IEmployeeService employeeService,
            IEmployeeProfileService profileService)
        {
            _authService = authService;
            _employeeService = employeeService;
            _profileService = profileService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int employeeId = _authService.GetCurrentEmployeeId(HttpContext);
            Employee = await _employeeService.GetEmployeeByIdAsync(employeeId);

            if (Employee == null)
            {
                return NotFound();
            }

            Profile = await _profileService.GetProfileByEmployeeIdAsync(employeeId);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            int employeeId = _authService.GetCurrentEmployeeId(HttpContext);

            if (employeeId != Employee.Id)
            {
                return Forbid();
            }

            // C?p nh?t thông tin nhân viên
            var existingEmployee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            existingEmployee.PhoneNumber = Employee.PhoneNumber;
            await _employeeService.UpdateEmployeeAsync(existingEmployee);

            // C?p nh?t profile
            await _profileService.UpdateProfileAsync(Profile);

            TempData["SuccessMessage"] = "C?p nh?t thông tin cá nhân thành công";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUploadImageAsync()
        {
            int employeeId = _authService.GetCurrentEmployeeId(HttpContext);

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var result = await _profileService.UploadProfileImageAsync(employeeId, ProfileImage);
                if (result)
                {
                    TempData["SuccessMessage"] = "C?p nh?t ?nh ??i di?n thành công";
                }
                else
                {
                    TempData["ErrorMessage"] = "Không th? t?i lên ?nh ??i di?n";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng ch?n ?nh ?? t?i lên";
            }

            return RedirectToPage();
        }
    }
}
