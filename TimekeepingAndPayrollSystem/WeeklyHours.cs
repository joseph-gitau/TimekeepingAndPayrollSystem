namespace TimekeepingAndPayrollSystem
{
    public class WeeklyHours
    {
        public bool IsGap { get; set; }
        public string Day { get; set; }
        public double HoursWorked { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsVacation { get; set; }
        public string ClientName { get; set; }
        public string ContractType { get; set; }
        public string ProjectDetails { get; set; }
        public string BillingLevel { get; set; }
        public int HoursMonday { get; set; }
        public int HoursTuesday { get; set; }
        public int HoursWednesday { get; set; }
        public int HoursThursday { get; set; }
        public int HoursFriday { get; set; }
        public int HoursSaturday { get; set; }
        public int HoursSunday { get; set; }
        public int TotalHours { get; set; }
    }
}