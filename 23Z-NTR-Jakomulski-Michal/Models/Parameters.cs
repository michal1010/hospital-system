using System.Globalization;

namespace ClinicApp.Models
{
    public record Parameters
    {
        private static Parameters? _instance;
        public static Parameters GetInstance()
        {
            if(_instance == null)
                _instance = new Parameters();
            return _instance;
        }
        private Parameters()
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
