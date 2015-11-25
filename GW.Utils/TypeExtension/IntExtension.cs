using System;

namespace GW.Utils.TypeExtension
{
    public static class IntExtension
    {
        public static string FormatKB(this int kb)
        {
            return ((long)kb).FormatKB();
        }

        public static DateTime TicketToDateTime(this int ticket, DateTimeKind kind)
        {
            DateTime baseTime = new DateTime(1970, 1, 1);
            if (kind == DateTimeKind.Local)
                baseTime = baseTime.ToLocalTime();
            else if (kind == DateTimeKind.Utc)
                baseTime = baseTime.ToUniversalTime();

            return baseTime.AddSeconds(ticket);
        }
    }
}
