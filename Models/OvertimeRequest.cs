using System.ComponentModel.DataAnnotations;

namespace EmployManager.Models
{
    public class OvertimeRequest
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required]
        [Display(Name = "Ngày yêu cầu")]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "Thời gian kết thúc")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Lý do")]
        public string? Reason { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Chờ duyệt"; 

        [Display(Name = "Ý kiến quản lý")]
        public string? ManagerComments { get; set; }

        [Display(Name = "Số giờ OT")]
        public decimal OvertimeHours => (decimal)(EndTime - StartTime).TotalHours;
    }
}
