using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployManager.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ITimeRecordService _timeRecordService;

        public bool IsAdmin { get; set; }
        public int EmployeeId { get; set; }
        public bool IsCheckedIn { get; set; }

        public IndexModel(IAuthService authService, ITimeRecordService timeRecordService)
        {
            _authService = authService;
            _timeRecordService = timeRecordService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            IsAdmin = _authService.IsAdmin(HttpContext);
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);

            var openTimeRecord = await _timeRecordService.GetOpenTimeRecordAsync(EmployeeId);
            IsCheckedIn = openTimeRecord != null;

            return Page();
        }

        public async Task<IActionResult> OnPostCheckInAsync()
        {
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            await _timeRecordService.CheckInAsync(EmployeeId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            await _timeRecordService.CheckOutAsync(EmployeeId);
            return RedirectToPage();
        }
    }
}
