using EmployManager.Models;
using EmployManager.Service;
using EmployManager.Service.impl;
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
        private readonly IEmployeeService _employeeService;

        [BindProperty]
        public OvertimeRequest OvertimeRequest { get; set; }

        [BindProperty]
        public int idOT { get; set; }

        public List<OvertimeRequest> MyOvertimeRequests { get; set; }
        public int EmployeeId { get; set; }
        public int roleUser { get; set; }

        public OvertimeRequestModel(IAuthService authService, IOvertimeService overtimeService, IEmployeeService employeeService)
        {
            _authService = authService;
            _overtimeService = overtimeService;
            _employeeService = employeeService;
        }

        public async Task OnGetAsync()
        {
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);

            var entity = await _employeeService.GetEmployeeByIdAsync(EmployeeId);
            roleUser = entity.RoleId;


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

        public async Task<IActionResult> OnPostApproveAsync()
        {
            OvertimeRequest request = await _overtimeService.GetOvertimeRequestByIdAsync(idOT);
            request.Status = "APPROVE";
            await _overtimeService.UpdateOvertimeRequestAsync(request);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            OvertimeRequest request = await _overtimeService.GetOvertimeRequestByIdAsync(idOT);
            request.Status = "Reject";
            await _overtimeService.UpdateOvertimeRequestAsync(request);

            return RedirectToPage();
        }
    }
}
