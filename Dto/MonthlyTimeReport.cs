namespace EmployManager.Dto
{
    public class MonthlyTimeReport
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Department { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalDays { get; set; }
        public decimal TotalHours { get; set; }
        public decimal OvertimeHours { get; set; }
    }
}
