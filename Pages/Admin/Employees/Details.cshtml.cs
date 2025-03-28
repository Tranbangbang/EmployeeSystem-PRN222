using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EmployManager.Pages.Admin.Employees
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly ITimeRecordService _timeRecordService;
        private readonly IOvertimeService _overtimeService;

        public Employee Employee { get; set; }
        public int TotalTimeRecords { get; set; }
        public int TotalOvertimeRequests { get; set; }
        public decimal TotalWorkHours { get; set; }
        public decimal TotalOvertimeHours { get; set; }

        public DetailsModel(
            IEmployeeService employeeService,
            ITimeRecordService timeRecordService,
            IOvertimeService overtimeService)
        {
            _employeeService = employeeService;
            _timeRecordService = timeRecordService;
            _overtimeService = overtimeService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (Employee == null)
            {
                return NotFound();
            }

            // L?y thông tin th?ng kê
            var timeRecords = await _timeRecordService.GetTimeRecordsByEmployeeIdAsync(id);
            TotalTimeRecords = timeRecords.Count;
            TotalWorkHours = timeRecords.Sum(tr => tr.WorkHours);

            var overtimeRequests = await _overtimeService.GetOvertimeRequestsByEmployeeIdAsync(id);
            TotalOvertimeRequests = overtimeRequests.Count;
            TotalOvertimeHours = overtimeRequests
                .Where(or => or.Status == "?ã duy?t")
                .Sum(or => or.OvertimeHours);

            return Page();
        }
    }
}