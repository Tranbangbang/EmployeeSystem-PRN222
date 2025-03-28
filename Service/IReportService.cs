using EmployManager.Dto;

namespace EmployManager.Service
{
    public interface IReportService
    {
        Task<List<MonthlyTimeReport>> GetMonthlyReportAsync(int month, int year);
        Task<MonthlyTimeReport> GetEmployeeMonthlyReportAsync(int employeeId, int month, int year);
    }
}
