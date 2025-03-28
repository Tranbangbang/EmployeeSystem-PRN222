using EmployManager.Models;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployManager.Pages.TimeTracking
{
    [Authorize]
    public class OvertimeRequestModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IOvertimeService _overtimeService;

        [BindProperty]
        public OvertimeRequest OvertimeRequest { get; set; }

        public List<OvertimeRequest> MyOvertimeRequests { get; set; }
        public int EmployeeId { get; set; }

        public OvertimeRequestModel(IAuthService authService, IOvertimeService overtimeService)
        {
            _authService = authService;
            _overtimeService = overtimeService;
        }

        public async Task OnGetAsync()
        {
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            MyOvertimeRequests = await _overtimeService.GetOvertimeRequestsByEmployeeIdAsync(EmployeeId);

            OvertimeRequest = new OvertimeRequest
            {
                EmployeeId = EmployeeId,
                RequestDate = DateTime.Now,
                StartTime = DateTime.Now.Date.AddHours(18), 
                EndTime = DateTime.Now.Date.AddHours(20)  
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            OvertimeRequest.EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            OvertimeRequest.RequestDate = DateTime.Now;
            OvertimeRequest.Status = "Ch? duy?t";

            await _overtimeService.CreateOvertimeRequestAsync(OvertimeRequest);

            return RedirectToPage();
        }
    }
}
