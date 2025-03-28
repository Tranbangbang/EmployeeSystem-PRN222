using EmployManager.Models;
using EmployManager.Service.impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployManager.Pages.Admin.Salaries
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ISalaryService _salaryService;

        public List<Salary> Salaries { get; set; }
        public List<EmployeeSalaryResponse> EmployeeSalaries { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public IndexModel(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }

        //public async Task OnGetAsync(int? month, int? year)
        //{
        //    Month = month ?? DateTime.Now.Month;
        //    Year = year ?? DateTime.Now.Year;

        //    Salaries = await _salaryService.GetSalariesByMonthYearAsync(Month, Year);
        //}


        //Month = month ?? DateTime.Now.Month;
        //    Year = year ?? DateTime.Now.Year;
        public async Task OnGetAsync(int month, int year)
        {

            Month =  DateTime.Now.Month;
            if(month != null)
                Month = month;
            Year =  DateTime.Now.Year;
            if(year != null)
                Year = year;

            int month_int = 1;
            if (month != null) month_int = month;

            int year_int = 1;
            if(year != null) year_int = year;

            Console.WriteLine(">>>>>>>>>>month = " + month_int);
            Console.WriteLine(">>>>>>>>>>>year = " + year_int);

            EmployeeSalaries = await _salaryService.GetAllSalaty(month_int, year_int);
        }

        public async Task<IActionResult> OnPostProcessPaymentAsync(int id)
        {
            await _salaryService.ProcessPaymentAsync(id);
            TempData["SuccessMessage"] = "Xác nh?n thanh toán l??ng thành công";
            return RedirectToPage(new { month = Month, year = Year });
        }
    }
}