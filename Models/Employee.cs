using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EmployManager.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Mã nhân viên")]
        public string EmployeeCode { get; set; }

        [Required]
        [Display(Name = "Họ")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string LastName { get; set; }

        [Display(Name = "Họ và tên")]
        public string FullName => $"{FirstName} {LastName}";

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Column("BasicSalary")]
        public double BasicSalary { get; set; }

        [Column("Allowance")]
        public double Allowance { get; set; }

        [Column("Bonus")]
        public double Bonus { get; set; }

        [Column("Deduction")]
        public double Deduction { get; set; }

        [Phone]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày vào làm")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Phòng ban")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Display(Name = "Vai trò")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; } = true;

        public ICollection<TimeRecord>? TimeRecords { get; set; }
        public ICollection<OvertimeRequest>? OvertimeRequests { get; set; }
    }
}
