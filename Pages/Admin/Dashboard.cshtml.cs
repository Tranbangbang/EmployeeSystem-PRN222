using EmployManager.Dal;
using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployManager.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly ITimeRecordService _timeRecordService;
        private readonly IOvertimeService _overtimeService;
        private readonly ApplicationDbContext _context;

        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int TodayCheckins { get; set; }
        public int PendingOvertimeRequests { get; set; }
        public List<OvertimeRequest> RecentOvertimeRequests { get; set; } = new List<OvertimeRequest>();

        public DashboardModel(
            IEmployeeService employeeService,
            ITimeRecordService timeRecordService,
            IOvertimeService overtimeService,
            ApplicationDbContext context) // Thêm ApplicationDbContext vào constructor
        {
            _employeeService = employeeService;
            _timeRecordService = timeRecordService;
            _overtimeService = overtimeService;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            TotalEmployees = employees.Count;
            ActiveEmployees = employees.Count(e => e.IsActive);

            // L?y s? l??ng check-in hôm nay
            var today = DateTime.Today;
            var allTimeRecords = await _context.TimeRecords
                .Where(tr => tr.CheckInTime.Date == today)
                .ToListAsync();
            TodayCheckins = allTimeRecords.Select(tr => tr.EmployeeId).Distinct().Count();

            // L?y yêu c?u OT ?ang ch? duy?t
            var pendingRequests = await _overtimeService.GetPendingOvertimeRequestsAsync();
            PendingOvertimeRequests = pendingRequests.Count;
            RecentOvertimeRequests = pendingRequests.Take(5).ToList();
        }
    }
}