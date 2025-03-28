using EmployManager.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployManager.Service.impl
{
    public class AuthService : IAuthService
    {
        private readonly IEmployeeService _employeeService;

        public AuthService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<bool> SignInAsync(HttpContext httpContext, Employee employee)
        {
            try
            {
                // Đảm bảo Role được load
                if (employee.Role == null)
                {
                    // Nếu Role chưa được load, lấy thông tin employee đầy đủ
                    employee = await _employeeService.GetEmployeeByIdAsync(employee.Id);
                    if (employee == null || employee.Role == null)
                    {
                        return false;
                    }
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{employee.FirstName} {employee.LastName}"),
                    new Claim(ClaimTypes.Email, employee.Email),
                    new Claim(ClaimTypes.Role, employee.Role.Name)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddHours(24)
                    });

                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                Console.WriteLine($"Lỗi đăng nhập: {ex.Message}");
                return false;
            }
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public int GetCurrentEmployeeId(HttpContext httpContext)
        {
            var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out int employeeId))
            {
                return employeeId;
            }
            return 0;
        }

        public bool IsAdmin(HttpContext httpContext)
        {
            return httpContext.User.IsInRole("Admin");
        }
    }
}