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

        public List<TimeRecord> TimeRecords { get; set; }
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public MyTimeRecordsModel(IAuthService authService, ITimeRecordService timeRecordService)
        {
            _authService = authService;
            _timeRecordService = timeRecordService;
        }

        public async Task OnGetAsync(int? month, int? year)
        {
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);

            Month = month ?? DateTime.Now.Month;
            Year = year ?? DateTime.Now.Year;

            TimeRecords = await _timeRecordService.GetTimeRecordsByMonthAsync(EmployeeId, Month, Year);
        }
    }
}
