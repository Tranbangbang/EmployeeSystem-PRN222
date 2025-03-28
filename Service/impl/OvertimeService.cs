using EmployManager.Dal;
using EmployManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployManager.Service.impl
{
    public class OvertimeService : IOvertimeService
    {
        private readonly ApplicationDbContext _context;

        public OvertimeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OvertimeRequest>> GetOvertimeRequestsByEmployeeIdAsync(int employeeId)
        {
            return await _context.OvertimeRequests
                .Where(or => or.EmployeeId == employeeId)
                .OrderByDescending(or => or.RequestDate)
                .ToListAsync();
        }

        public async Task<List<OvertimeRequest>> GetPendingOvertimeRequestsAsync()
        {
            return await _context.OvertimeRequests
                .Include(or => or.Employee)
                .Where(or => or.Status == "Chờ duyệt")
                .OrderByDescending(or => or.RequestDate)
                .ToListAsync();
        }

        public async Task<OvertimeRequest> GetOvertimeRequestByIdAsync(int id)
        {
            return await _context.OvertimeRequests
                .Include(or => or.Employee)
                .FirstOrDefaultAsync(or => or.Id == id);
        }

        public async Task<bool> CreateOvertimeRequestAsync(OvertimeRequest overtimeRequest)
        {
            try
            {
                _context.OvertimeRequests.Add(overtimeRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOvertimeRequestAsync(OvertimeRequest overtimeRequest)
        {
            try
            {
                _context.Entry(overtimeRequest).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ApproveOvertimeRequestAsync(int id, string managerComments)
        {
            try
            {
                var overtimeRequest = await _context.OvertimeRequests.FindAsync(id);
                if (overtimeRequest == null)
                    return false;

                overtimeRequest.Status = "Đã duyệt";
                overtimeRequest.ManagerComments = managerComments;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RejectOvertimeRequestAsync(int id, string managerComments)
        {
            try
            {
                var overtimeRequest = await _context.OvertimeRequests.FindAsync(id);
                if (overtimeRequest == null)
                    return false;

                overtimeRequest.Status = "Từ chối";
                overtimeRequest.ManagerComments = managerComments;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteOvertimeRequestAsync(int id)
        {
            try
            {
                var overtimeRequest = await _context.OvertimeRequests.FindAsync(id);
                if (overtimeRequest == null)
                    return false;

                _context.OvertimeRequests.Remove(overtimeRequest);
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
