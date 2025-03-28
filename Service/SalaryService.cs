using EmployManager.Dal;
using EmployManager.Migrations;
using EmployManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployManager.Service.impl
{
    public class SalaryService : ISalaryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeService _employeeService;
        private readonly ITimeRecordService _timeRecordService;

        public SalaryService(ApplicationDbContext context, IEmployeeService employeeService, ITimeRecordService timeRecordService)
        {
            _context = context;
            _employeeService = employeeService;
            _timeRecordService = timeRecordService;
        }

        public async Task<List<Salary>> GetSalariesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Salaries
                .Where(s => s.EmployeeId == employeeId)
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToListAsync();
        }

        public async Task<List<Salary>> GetSalariesByMonthYearAsync(int month, int year)
        {
            return await _context.Salaries
                .Include(s => s.Employee)
                .Where(s => s.Month == month && s.Year == year)
                .ToListAsync();
        }

        public async Task<Salary> GetSalaryByIdAsync(int id)
        {
            return await _context.Salaries
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> CreateSalaryAsync(Salary salary)
        {
            try
            {
                _context.Salaries.Add(salary);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateSalaryAsync(Salary salary)
        {
            try
            {
                _context.Entry(salary).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteSalaryAsync(int id)
        {
            try
            {
                var salary = await _context.Salaries.FindAsync(id);
                if (salary == null)
                    return false;

                _context.Salaries.Remove(salary);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ProcessPaymentAsync(int id)
        {
            try
            {
                var salary = await _context.Salaries.FindAsync(id);
                if (salary == null)
                    return false;

                salary.Status = "Đã thanh toán";
                salary.PaymentDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<EmployeeSalaryResponse>> GetAllSalaty(int month, int year)
        {
            try
            {
                List<EmployeeSalaryResponse> result = new List<EmployeeSalaryResponse>();

                List<Employee> employeeList = await _employeeService.GetAllEmployeesAsync();
                foreach (Employee employee in employeeList)
                {
                    List<TimeRecord> records = await _timeRecordService.GetTimeRecordsByMonthAsync(employee.Id, month, year);
                    if(records == null)
                        records = new List<TimeRecord>();

                    EmployeeSalaryResponse tmp = new EmployeeSalaryResponse
                    {
                        IdEmployee = employee.Id,
                        EmployeeCode = employee.EmployeeCode,
                        EmployeeName = employee.FullName,
                        DepartmentName = employee.Department.Name,
                        IsActive = employee.IsActive,
                        dayWorks = records.Count,
                        dayOff = DateTime.DaysInMonth(year, month) - records.Count,
                        basicSalary = employee.BasicSalary,
                        allowance = employee.Allowance,
                        bounus = employee.Bonus,
                        deduction = employee.Deduction,
                        totalSalary = records.Count * employee.BasicSalary + employee.Bonus + employee.Allowance - employee.Deduction
                    };

                    result.Add(tmp);
                }

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}