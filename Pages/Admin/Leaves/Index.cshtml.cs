using EmployManager.Dal;
using EmployManager.Models;
using EmployManager.Service.impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmployManager.Pages.Admin.Leaves
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ILeaveService _leaveService;
        private readonly ApplicationDbContext _context;

        public List<Leave> PendingLeaves { get; set; }
        public List<Leave> ApprovedLeaves { get; set; }
        public List<Leave> RejectedLeaves { get; set; }

        public IndexModel(ILeaveService leaveService, ApplicationDbContext context)
        {
            _leaveService = leaveService;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            PendingLeaves = await _context.Leaves
                .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
                .Where(l => l.Status == "Ch? duy?t")
                .OrderByDescending(l => l.RequestDate)
                .ToListAsync();

            ApprovedLeaves = await _context.Leaves
                .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
                .Where(l => l.Status == "?� duy?t")
                .OrderByDescending(l => l.RequestDate)
                .Take(20)
                .ToListAsync();

            RejectedLeaves = await _context.Leaves
                .Include(l => l.Employee)
                .ThenInclude(e => e.Department)
                .Where(l => l.Status == "T? ch?i")
                .OrderByDescending(l => l.RequestDate)
                .Take(20)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id, string comments)
        {
            await _leaveService.ApproveLeaveAsync(id, comments);
            TempData["SuccessMessage"] = "?� duy?t y�u c?u ngh? ph�p th�nh c�ng";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(int id, string comments)
        {
            if (string.IsNullOrEmpty(comments))
            {
                TempData["ErrorMessage"] = "Vui l�ng nh?p l� do t? ch?i";
                return RedirectToPage();
            }

            await _leaveService.RejectLeaveAsync(id, comments);
            TempData["SuccessMessage"] = "?� t? ch?i y�u c?u ngh? ph�p";
            return RedirectToPage();
        }
    }
}