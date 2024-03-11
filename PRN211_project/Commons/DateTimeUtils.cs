namespace PRN211_project.Commons
{
    public class DateTimeUtils
    {
        public static string CheckedDate(DateTime time)
        {
            DateTime now = DateTime.Now;
            if (time < now)
            {
                return "past";
            }
            else if(time > now)
            {
                return "future";
            }
            return "today";
        }



    }
}
