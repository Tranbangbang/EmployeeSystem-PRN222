using EmployManager.Dal;
using EmployManager.Dto;
using EmployManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployManager.Service.impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<bool> CreateEmployeeAsync(Employee employee, string password)
        {
            try
            {
                employee.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                _context.Entry(employee).State = EntityState.Modified;
                if (string.IsNullOrEmpty(employee.PasswordHash))
                {
                    _context.Entry(employee).Property(x => x.PasswordHash).IsModified = false;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    return false;

                employee.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            var employee = await GetEmployeeByEmailAsync(email);
            if (employee == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, employee.PasswordHash);
        }

        public async Task<PaginatedList<Employee>> GetPaginatedEmployeesAsync(
            int pageIndex,
            int pageSize,
            string searchString = null,
            int? departmentId = null,
            bool? activeOnly = null)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(e =>
                    e.EmployeeCode.ToLower().Contains(searchString) ||
                    e.FirstName.ToLower().Contains(searchString) ||
                    e.LastName.ToLower().Contains(searchString) ||
                    e.Email.ToLower().Contains(searchString) ||
                    (e.PhoneNumber != null && e.PhoneNumber.Contains(searchString))
                );
            }

            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            if (activeOnly.HasValue && activeOnly.Value)
            {
                query = query.Where(e => e.IsActive);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(e => e.EmployeeCode)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Employee>(items, totalCount, pageIndex, pageSize);
        }
    }
}