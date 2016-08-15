using eb.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            MD_CUT_LINE,
            DIFFERENCE_CHEPOWER
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

        public static bool chkCutOffTime(ClsRealChe cls, bool isSimulation)
        {
            if (isSimulation)
            {
                string hour = cls.Chetime.Substring(0, 2);
                string minute = cls.Chetime.Substring(2, 2);

                if (Common.getIntValue(hour) >= 15 && Common.getIntValue(minute) >= 18)
                    return true;
                else
                    return false;
            }
            else
            {
                DateTime time = DateTime.Now;
                if (time.Hour == 15 && time.Minute > 18)
                    return true;
                else
                    return false;
            }
        }

        public static string getDialogFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Program.cont.getApplicationPath + Program.cont.getLogPath;
            ofd.ShowDialog();

            return ofd.FileName;
        }

        public static string GetDialogFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = System.Windows.Forms.Application.StartupPath + @"\logs\";
            fbd.ShowDialog();

            return fbd.SelectedPath;
        }
    }
}
