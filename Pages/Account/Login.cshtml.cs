using EmployManager.Dal;
using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmployManager.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email l� b?t bu?c")]
            [EmailAddress(ErrorMessage = "Email kh�ng h?p l?")]
            public string Email { get; set; }

            [Required(ErrorMessage = "M?t kh?u l� b?t bu?c")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public LoginModel(IEmployeeService employeeService, IAuthService authService, ApplicationDbContext context)
        {
            _employeeService = employeeService;
            _authService = authService;
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isValid = await _employeeService.VerifyPasswordAsync(Input.Email, Input.Password);

            if (!isValid)
            {
                ErrorMessage = "Email ho?c m?t kh?u kh�ng ?�ng";
                return Page();
            }

            var employee = await _context.Employees
                .Include(e => e.Role)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Email == Input.Email);

            if (employee == null)
            {
                ErrorMessage = "Kh�ng t�m th?y t�i kho?n";
                return Page();
            }

            if (!employee.IsActive)
            {
                ErrorMessage = "T�i kho?n ?� b? v� hi?u h�a";
                return Page();
            }

            var result = await _authService.SignInAsync(HttpContext, employee);

            if (!result)
            {
                ErrorMessage = "??ng nh?p th?t b?i. Vui l�ng th? l?i.";
                return Page();
            }

            Console.WriteLine($"??ng nh?p th�nh c�ng: {employee.Email}, Role: {employee.Role?.Name}");

            return RedirectToPage("/Index");
        }
    }
}