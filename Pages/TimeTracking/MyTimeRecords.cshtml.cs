using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployManager.Pages.TimeTracking
{
    [Authorize]
    public class MyTimeRecordsModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ITimeRecordService _timeRecordService;
        private readonly IEmployeeService _employeeService;

        public List<TimeRecord> TimeRecords { get; set; }
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public MyTimeRecordsModel(IAuthService authService, ITimeRecordService timeRecordService, IEmployeeService employeeService)
        {
            _authService = authService;
            _timeRecordService = timeRecordService;
            _employeeService = employeeService;
        }

        public async Task OnGetAsync(int? month, int? year)
        {
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            Employee employeeCurrent = await _employeeService.GetEmployeeByIdAsync(EmployeeId);

            Month = month ?? DateTime.Now.Month;
            Year = year ?? DateTime.Now.Year;

            if (employeeCurrent != null && employeeCurrent.RoleId == 1)
            {
                TimeRecords = await _timeRecordService.GetAllTimeRecordsByMonthAsync( Month, Year);
            }
            else if (employeeCurrent != null && employeeCurrent.RoleId == 2)
            {
                TimeRecords = await _timeRecordService.GetTimeRecordsByMonthAsync(EmployeeId, Month, Year);
            }
        }
    }
}
