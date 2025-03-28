using System.ComponentModel.DataAnnotations;

namespace EmployManager.Models
{
    public class EmployeeProfile
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Giới tính")]
        public string Gender { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "CMND/CCCD")]
        public string IdentityCard { get; set; }

        [Display(Name = "Ngày cấp")]
        [DataType(DataType.Date)]
        public DateTime? IdentityCardIssueDate { get; set; }

        [Display(Name = "Nơi cấp")]
        public string IdentityCardIssuePlace { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string ProfileImage { get; set; }

        [Display(Name = "Chức vụ")]
        public string Position { get; set; }

        [Display(Name = "Trình độ học vấn")]
        public string Education { get; set; }

        [Display(Name = "Kinh nghiệm")]
        public string Experience { get; set; }

        [Display(Name = "Người liên hệ khẩn cấp")]
        public string EmergencyContact { get; set; }

        [Display(Name = "SĐT liên hệ khẩn cấp")]
        public string EmergencyPhone { get; set; }
    }
}
