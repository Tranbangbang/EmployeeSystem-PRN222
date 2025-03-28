using EmployManager.Models;

namespace EmployManager.Service
{
    public interface IAuthService
    {
        Task<bool> SignInAsync(HttpContext httpContext, Employee employee);
        void SignOut(HttpContext httpContext);
        int GetCurrentEmployeeId(HttpContext httpContext);
        bool IsAdmin(HttpContext httpContext);
    }
}
