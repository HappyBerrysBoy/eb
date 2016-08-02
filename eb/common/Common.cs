using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb.common
{
    static class Common
    {
        public enum CONFIG_IDX
        {
            VOLUME_HISTORY_CNT,
            CUT_OFF_PERCENT,
            PROFIT_CUT_OFF_PERCENT,
            POWER_LOW_LIMIT,
            POWER_HIGH_LIMIT,
            IGNORE_CHE_CNT,
            PIERCE_HO_CNT,
            LOG_TERM,
            MS_MD_RATE,
            LOG_TERM_VOLUME_OVER,
            ORDER_SIGN_CNT,
            SELL_SIGN_CNT,
            MS_CUT_LINE,
            MD_CUT_LINE
        }

        public static string getSign(string sign)
        {
            if (sign.Equals("1"))
                return "↑";
            else if (sign.Equals("2"))
                return "▲";
            else if (sign.Equals("4"))
                return "↓";
            else if (sign.Equals("5"))
                return "▼";
            return "";
        }

        public static int getIntValue(string value)
        {
            int result;

            if (int.TryParse(value, out result))
                return result;

            return 0;
        }

        public static long getLongValue(string value)
        {
            long result;

            if (long.TryParse(value, out result))
                return result;

            return 0;
        }

        public static double getDoubleValue(string value)
        {
            double result;

            if (double.TryParse(value, out result))
                return result;

            return 0;
        }
    }
}
