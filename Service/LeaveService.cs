using EmployManager.Dal;
using EmployManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployManager.Service.impl
{
    public class LeaveService : ILeaveService
    {
        private readonly ApplicationDbContext _context;

        public LeaveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Leave>> GetLeavesByEmployeeIdAsync(int employeeId)
        {
            return await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.RequestDate)
                .ToListAsync();
        }

        public async Task<List<Leave>> GetPendingLeavesAsync()
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .Where(l => l.Status == "Chờ duyệt")
                .OrderByDescending(l => l.RequestDate)
                .ToListAsync();
        }

        public async Task<Leave> GetLeaveByIdAsync(int id)
        {
            return await _context.Leaves
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<bool> CreateLeaveAsync(Leave leave)
        {
            try
            {
                _context.Leaves.Add(leave);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateLeaveAsync(Leave leave)
        {
            try
            {
                _context.Entry(leave).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ApproveLeaveAsync(int id, string managerComments)
        {
            try
            {
                var leave = await _context.Leaves.FindAsync(id);
                if (leave == null)
                    return false;

                leave.Status = "Đã duyệt";
                leave.ManagerComments = managerComments;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RejectLeaveAsync(int id, string managerComments)
        {
            try
            {
                var leave = await _context.Leaves.FindAsync(id);
                if (leave == null)
                    return false;

                leave.Status = "Từ chối";
                leave.ManagerComments = managerComments;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteLeaveAsync(int id)
        {
            try
            {
                var leave = await _context.Leaves.FindAsync(id);
                if (leave == null)
                    return false;

                _context.Leaves.Remove(leave);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}