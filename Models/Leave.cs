using System.ComponentModel.DataAnnotations;

namespace EmployManager.Models
{
    public class Leave
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Loại nghỉ phép")]
        public string LeaveType { get; set; }

        [Required]
        [Display(Name = "Lý do")]
        public string Reason { get; set; }

        [Display(Name = "Ngày yêu cầu")]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = "Chờ duyệt";

        [Display(Name = "Ý kiến quản lý")]
        public string ManagerComments { get; set; }

        [Display(Name = "Số ngày nghỉ")]
        public int LeaveDays
        {
            get
            {
                return (EndDate - StartDate).Days + 1;
            }
        }
    }
}
