using EmployManager.Dto;
using EmployManager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployManager.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportsModel : PageModel
    {
        private readonly IReportService _reportService;

        public List<MonthlyTimeReport> MonthlyReports { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public ReportsModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task OnGetAsync(int? month, int? year)
        {
            Month = month ?? DateTime.Now.Month;
            Year = year ?? DateTime.Now.Year;

            MonthlyReports = await _reportService.GetMonthlyReportAsync(Month, Year);
        }
    }
}
