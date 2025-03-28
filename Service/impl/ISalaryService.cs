using EmployManager.Models;

namespace EmployManager.Service.impl
{
    public interface ISalaryService
    {
        Task<List<Salary>> GetSalariesByEmployeeIdAsync(int employeeId);
        Task<List<Salary>> GetSalariesByMonthYearAsync(int month, int year);
        Task<Salary> GetSalaryByIdAsync(int id);
        Task<bool> CreateSalaryAsync(Salary salary);
        Task<bool> UpdateSalaryAsync(Salary salary);
        Task<bool> DeleteSalaryAsync(int id);
        Task<bool> ProcessPaymentAsync(int id);
    }
}
