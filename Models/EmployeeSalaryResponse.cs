namespace EmployManager.Models
{
    public class EmployeeSalaryResponse
    {
        public int IdEmployee { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public int dayWorks { get; set; }
        public int dayOff { get; set; }
        public double basicSalary { get; set; }
        public double allowance { get; set; }
        public double bounus { get; set; }
        public double deduction { get; set; }
        public double totalSalary { get; set; }

        public override string ToString()
        {
            return $"Id: {IdEmployee}, " +
                   $"Mã nhân viên: {EmployeeCode}, " +
                   $"Tên nhân viên: {EmployeeName}, " +
                   $"Phòng ban: {DepartmentName}, " +
                   $"Actice: {IsActive}, " +
                   $"Ngày làm việc: {dayWorks}, " +
                   $"Ngày nghỉ: {dayOff}, " +
                   $"Lương cơ bản: {basicSalary:N0} VND, " +
                   $"Thưởng: {bounus:N0} VND, " +
                   $"Khấu trừ: {deduction:N0} VND, " +
                   $"Khấu trừ: {deduction:N0} VND, " +
                   $"Tổng lương: {totalSalary:N0} VND";
        }
    }
}
