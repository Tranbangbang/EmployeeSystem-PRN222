using EmployManager.Dal;
using EmployManager.Dto;
using Microsoft.EntityFrameworkCore;

namespace EmployManager.Service.impl
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyTimeReport>> GetMonthlyReportAsync(int month, int year)
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.IsActive)
                .ToListAsync();

            var reports = new List<MonthlyTimeReport>();

            foreach (var employee in employees)
            {
                var report = await GetEmployeeMonthlyReportAsync(employee.Id, month, year);
                reports.Add(report);
            }

            return reports;
        }

        public async Task<MonthlyTimeReport> GetEmployeeMonthlyReportAsync(int employeeId, int month, int year)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return null;

            var timeRecords = await _context.TimeRecords
                .Where(tr => tr.EmployeeId == employeeId &&
                       tr.CheckInTime.Month == month &&
                       tr.CheckInTime.Year == year &&
                       tr.CheckOutTime != null)
                .ToListAsync();

            var overtimeRequests = await _context.OvertimeRequests
                .Where(or => or.EmployeeId == employeeId &&
                       or.StartTime.Month == month &&
                       or.StartTime.Year == year &&
                       or.Status == "Đã duyệt")
                .ToListAsync();

            var totalDays = timeRecords.Select(tr => tr.CheckInTime.Date).Distinct().Count();
            var totalHours = timeRecords.Sum(tr => tr.WorkHours);
            var overtimeHours = overtimeRequests.Sum(or => or.OvertimeHours);

            return new MonthlyTimeReport
            {
                EmployeeId = employee.Id,
                EmployeeName = employee.FullName,
                Department = employee.Department?.Name,
                Month = month,
                Year = year,
                TotalDays = totalDays,
                TotalHours = totalHours,
                OvertimeHours = overtimeHours
            };
        }
    }
}
