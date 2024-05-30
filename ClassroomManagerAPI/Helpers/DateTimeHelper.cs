namespace ClassroomManagerAPI.Helpers
{
    public static class DateTimeHelper
    {
        public static string GetDate(this DateTime dateTime)
        {
            string format = "dd/MM/yyyy";
            return dateTime.ToString(format);
        }
        public static string GetTime(this DateTime dateTime)
        {
            string format = "hh:mm";
            return dateTime.ToString(format);
        }
        public static DateTime AddDateTime(this DateTime dateTime, string time)
        {
            if (double.TryParse(time, out double result))
            {
                DateTime timeParse = DateTime.FromOADate(result);
                TimeSpan timeToAdd = new TimeSpan(timeParse.Hour, timeParse.Minute, timeParse.Second);
                return dateTime.Add(timeToAdd);
            }else
            {
                if (TimeSpan.TryParse(time, out TimeSpan parsedTime))
                {
                    TimeSpan timeToAdd = new TimeSpan(parsedTime.Hours, parsedTime.Minutes, parsedTime.Seconds);
                    return dateTime.Add(timeToAdd);
                }
                else
                {
                    return dateTime;
                }
            }
        }
    }
}
