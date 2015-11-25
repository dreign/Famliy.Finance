
namespace GW.Utils.TypeExtension
{
    public static class LongExtension
    {

        public static string FormatKB(this long kb)
        {
            return FormatBytes(kb * 1024);
        }

        public static string FormatBytes(this long bytes)
        {
            const double ONE_KB = 1024;
            const double ONE_MB = ONE_KB * 1024;
            const double ONE_GB = ONE_MB * 1024;
            const double ONE_TB = ONE_GB * 1024;
            const double ONE_PB = ONE_TB * 1024;
            const double ONE_EB = ONE_PB * 1024;
            const double ONE_ZB = ONE_EB * 1024;
            const double ONE_YB = ONE_ZB * 1024;

            if ((double)bytes <= 999)
                return bytes.ToString() + " bytes";
            else if ((double)bytes <= ONE_KB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_KB) + " KB";
            else if ((double)bytes <= ONE_MB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_MB) + " MB";
            else if ((double)bytes <= ONE_GB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_GB) + " GB";
            else if ((double)bytes <= ONE_TB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_TB) + " TB";
            else if ((double)bytes <= ONE_PB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_PB) + " PB";
            else if ((double)bytes <= ONE_EB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_EB) + " EB";
            else if ((double)bytes <= ONE_ZB * 999)
                return ThreeNonZeroDigits((double)bytes / ONE_ZB) + " ZB";
            else
                return ThreeNonZeroDigits((double)bytes / ONE_YB) + " YB";
        }

        static string ThreeNonZeroDigits(double value)
        {
            if (value >= 100)
                return ((int)value).ToString();
            else if (value >= 10)
                return value.ToString("0.0");
            else
                return value.ToString("0.00");
        }

    }
}
