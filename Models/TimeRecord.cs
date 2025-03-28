using System.ComponentModel.DataAnnotations;

namespace EmployManager.Models
{
    public class TimeRecord
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [Display(Name = "Giờ vào")]
        public DateTime CheckInTime { get; set; }

        [Display(Name = "Giờ ra")]
        public DateTime? CheckOutTime { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; }

        [Display(Name = "Số giờ làm việc")]
        public decimal WorkHours
        {
            get
            {
                if (CheckOutTime.HasValue)
                {
                    return (decimal)(CheckOutTime.Value - CheckInTime).TotalHours;
                }
                return 0;
            }
        }
    }
}
