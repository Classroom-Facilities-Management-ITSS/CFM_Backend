namespace ClassroomManagerAPI.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime AddDateTime(this DateTime dateTime, string time)
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
