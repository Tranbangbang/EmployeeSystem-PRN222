using System.ComponentModel.DataAnnotations;

namespace EmployManager.Models
{
    public class Salary
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [Display(Name = "Tháng")]
        public int Month { get; set; }

        [Required]
        [Display(Name = "Năm")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Lương cơ bản")]
        [DataType(DataType.Currency)]
        public decimal BasicSalary { get; set; }

        [Display(Name = "Phụ cấp")]
        [DataType(DataType.Currency)]
        public decimal Allowance { get; set; }

        [Display(Name = "Thưởng")]
        [DataType(DataType.Currency)]
        public decimal Bonus { get; set; }

        [Display(Name = "Khấu trừ")]
        [DataType(DataType.Currency)]
        public decimal Deduction { get; set; }

        [Display(Name = "Ghi chú")]
        public string Notes { get; set; }

        [Display(Name = "Tổng thu nhập")]
        [DataType(DataType.Currency)]
        public decimal TotalSalary => BasicSalary + Allowance + Bonus - Deduction;

        [Display(Name = "Ngày thanh toán")]
        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Chưa thanh toán";
    }
}
