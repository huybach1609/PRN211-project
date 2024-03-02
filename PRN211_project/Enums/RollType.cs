namespace PRN211_test.Enums
{
    public enum RollType
    {
        Admin,
        User,
        None 
    }
    public class RollTypeConverter
    {
        public static RollType ConvertIntToRollType(int value)
        {
            switch (value)
            {
                case 0:
                    return RollType.Admin;
                case 1:
                    return RollType.User;
                default:
                    throw new ArgumentException("Invalid value for RollType");
            }
        }
    }
}
