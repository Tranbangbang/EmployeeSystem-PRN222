using EmployManager.Models;
using EmployManager.Service.impl;
using EmployManager.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace EmployManager.Pages.Employeess
{
    [Authorize]
    public class LeaveModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ILeaveService _leaveService;

        [BindProperty]
        public Leave LeaveRequest { get; set; }

        public List<Leave> MyLeaves { get; set; }
        public int EmployeeId { get; set; }

        public LeaveModel(IAuthService authService, ILeaveService leaveService)
        {
            _authService = authService;
            _leaveService = leaveService;
        }

        public async Task OnGetAsync()
        {
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            MyLeaves = await _leaveService.GetLeavesByEmployeeIdAsync(EmployeeId);

            LeaveRequest = new Leave
            {
                EmployeeId = EmployeeId,
                RequestDate = DateTime.Now,
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            LeaveRequest.EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            LeaveRequest.RequestDate = DateTime.Now;
            LeaveRequest.Status = "Ch? duy?t";

            if (LeaveRequest.StartDate > LeaveRequest.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày b?t ??u không th? sau ngày k?t thúc");
                await OnGetAsync();
                return Page();
            }

            await _leaveService.CreateLeaveAsync(LeaveRequest);
            TempData["SuccessMessage"] = "??ng ký ngh? phép thành công";

            return RedirectToPage();
        }
    }
}
