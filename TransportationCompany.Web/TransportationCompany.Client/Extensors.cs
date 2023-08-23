namespace TransportationCompany.Client
{
    public static class Extensors
    {
        public static Boolean IsNullOrEmpty(this string value)
        {
            Boolean result = String.IsNullOrEmpty(value);

            return result;
        }

        public static Int64 ToInt64(this string value, Int64 DefaultValue = 0)
        {
            return ToInt64Nullable(value) ?? DefaultValue;
        }

        public static Int64? ToInt64Nullable(this string value)
        {
            Int64? result = null;

            if (value.IsNullOrEmpty())
            {
                return result;
            }

            value = value.Trim();
            Int64 newInt64;

            if (Int64.TryParse(value, out newInt64))
            {
                result = newInt64;
            }

            return result;
        }

        public static DateTime ToUnixEpochDate(this Int64 unixTime)
        {
            var result = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);

            return result;
        }
    }
}
