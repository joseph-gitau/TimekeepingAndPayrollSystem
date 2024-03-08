using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimekeepingAndPayrollSystem
{
    public class WeeklyHoursData : IDataErrorInfo
    {
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

        public string Error => null; // Not implemented

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case "HoursMonday":
                        if (HoursMonday > 24)
                            result = "Cannot exceed 24 hours.";
                        break;
                        // Add other cases for other day properties
                }
                return result;
            }
        }
    }

}
