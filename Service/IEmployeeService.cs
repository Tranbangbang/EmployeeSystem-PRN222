using EmployManager.Dto;
using EmployManager.Models;
using System.Collections.Generic;
namespace EmployManager.Service
{
    public interface IEmployeeService
    {
       Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> GetEmployeeByEmailAsync(string email);
        Task<bool> CreateEmployeeAsync(Employee employee, string password);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<bool> VerifyPasswordAsync(string email, string password);
        Task<PaginatedList<Employee>> GetPaginatedEmployeesAsync(int pageIndex, int pageSize, string searchString = null, int? departmentId = null, bool? activeOnly = null);
    }
}
