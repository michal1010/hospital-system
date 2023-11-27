using System.Globalization;

namespace ClinicApp.Models
{
    public record Parameters
    {
        public Parameters()
        {
            var currentDate = DateTime.Now;
            CurrentWeekNumber = CultureInfo.CurrentCulture.Calendar
                                            .GetWeekOfYear(currentDate,
                                            CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static int CurrentWeekNumber { get; set; }

        public enum Roles
        {
            Director,
            Patient,
            Doctor
        }
    }
}
