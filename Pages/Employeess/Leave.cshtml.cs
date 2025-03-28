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
        private readonly IEmployeeService _employeeService;

        [BindProperty]
        public Leave LeaveRequest { get; set; }

        [BindProperty]
        public int IdDayOff { get; set; }

        [BindProperty]
        public int idLeave { get; set; }
        

        public List<Leave> MyLeaves { get; set; }
        public int EmployeeId { get; set; }
        public int roleUser { get; set; }

        public LeaveModel(IAuthService authService, ILeaveService leaveService, IEmployeeService employeeService)
        {
            _authService = authService;
            _leaveService = leaveService;
            _employeeService = employeeService;
        }

        public async Task OnGetAsync()
        {
            EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            var entity = await _employeeService.GetEmployeeByIdAsync(EmployeeId);
            roleUser = entity.RoleId;
            
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


            LeaveRequest.EmployeeId = _authService.GetCurrentEmployeeId(HttpContext);
            LeaveRequest.RequestDate = DateTime.Now;
            LeaveRequest.Status = "Pending";
            LeaveRequest.ManagerComments = "";

            if (LeaveRequest.StartDate > LeaveRequest.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày b?t ??u không th? sau ngày k?t thúc");
                await OnGetAsync();
                return Page();
            }

            Console.WriteLine(">>>>>>>>>>>>>>>" + IdDayOff);
            Console.WriteLine("start date: " + LeaveRequest.StartDate);
            Console.WriteLine("start date: " + LeaveRequest.EndDate);
            Console.WriteLine("start date: " + LeaveRequest.Reason);

            await _leaveService.CreateLeaveAsync(LeaveRequest);
            //TempData["SuccessMessage"] = "??ng ký ngh? phép thành công";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApproveAsync()
        {
            Leave leave = await _leaveService.GetLeaveByIdAsync(idLeave);
            leave.Status = "APPROVE";
            await _leaveService.UpdateLeaveAsync(leave);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            Leave leave = await _leaveService.GetLeaveByIdAsync(idLeave);
            leave.Status = "Reject";
            await _leaveService.UpdateLeaveAsync(leave);

            return RedirectToPage();
        }
    }
}
