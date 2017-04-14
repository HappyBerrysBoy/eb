using eb.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb.common
{
    static class Common
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // config.ini 설정 내용들..
        //VOLUME_HISTORY_CNT,
        //CUT_OFF_PERCENT,
        //PROFIT_CUT_OFF_PERCENT,
        //POWER_LOW_LIMIT,
        //POWER_HIGH_LIMIT,
        //IGNORE_CHE_CNT,
        //PIERCE_HO_CNT,
        //LOG_TERM,
        //MS_MD_RATE,
        //LOG_TERM_VOLUME_OVER,
        //ORDER_SIGN_CNT,
        //SELL_SIGN_CNT,
        //MS_CUT_LINE,
        //MD_CUT_LINE,
        //DIFFERENCE_CHEPOWER,
        //SATISFY_PROFIT,
        //CUT_OFF_HOUR,
        //CUT_OFF_MIN,
        //ALL_CODE_PAGE_NUM,
        //ALL_CODE_TTL_PAGE

        public static void SetConfig()
        {
            try
            {
                string filename = Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName;
                string section = Program.cont.getINISection;

                StringBuilder temp = new StringBuilder(255);
                GetPrivateProfileString(section, "VOLUME_HISTORY_CNT", "", temp, 255, filename);
                Program.cont.VolumeHistoryCnt = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "CUT_OFF_PERCENT", "", temp, 255, filename);
                Program.cont.CutoffPercent = Common.getDoubleValue(temp.ToString());
                GetPrivateProfileString(section, "PROFIT_CUT_OFF_PERCENT", "", temp, 255, filename);
                Program.cont.ProfitCutoffPercent = Common.getDoubleValue(temp.ToString());
                GetPrivateProfileString(section, "POWER_LOW_LIMIT", "", temp, 255, filename);
                Program.cont.PowerLowLimit = Common.getDoubleValue(temp.ToString());
                GetPrivateProfileString(section, "POWER_HIGH_LIMIT", "", temp, 255, filename);
                Program.cont.PowerHighLimit = Common.getDoubleValue(temp.ToString());
                GetPrivateProfileString(section, "IGNORE_CHE_CNT", "", temp, 255, filename);
                Program.cont.IgnoreCheCnt = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "PIERCE_HO_CNT", "", temp, 255, filename);
                Program.cont.PierceHoCnt = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "LOG_TERM", "", temp, 255, filename);
                Program.cont.LogTerm = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "MS_MD_RATE", "", temp, 255, filename);
                Program.cont.MsmdRate = Common.getDoubleValue(temp.ToString());
                GetPrivateProfileString(section, "LOG_TERM_VOLUME_OVER", "", temp, 255, filename);
                Program.cont.LogTermVolumeOver = Common.getDoubleValue(temp.ToString());
                GetPrivateProfileString(section, "ORDER_SIGN_CNT", "", temp, 255, filename);
                Program.cont.OrderSignCnt = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "SELL_SIGN_CNT", "", temp, 255, filename);
                Program.cont.SellSignCnt = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "MS_CUT_LINE", "", temp, 255, filename);
                Program.cont.MsCutLine = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "MD_CUT_LINE", "", temp, 255, filename);
                Program.cont.MdCutLine = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "DIFFERENCE_CHEPOWER", "", temp, 255, filename);
                Program.cont.DifferenceChePower = Common.getDoubleValue(temp.ToString());
                GetPrivateProfileString(section, "SATISFY_PROFIT", "", temp, 255, filename);
                Program.cont.SatisfyProfit = Common.getDoubleValue(temp.ToString());
                GetPrivateProfileString(section, "CUT_OFF_HOUR", "", temp, 255, filename);
                Program.cont.CutOffHour = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "CUT_OFF_MIN", "", temp, 255, filename);
                Program.cont.CutOffMinute = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "ALL_CODE_PAGE_NUM", "", temp, 255, filename);
                Program.cont.AllCodePageNum = Common.getIntValue(temp.ToString());
                GetPrivateProfileString(section, "ALL_CODE_TTL_PAGE", "", temp, 255, filename);
                Program.cont.AllCodeTtlPage = Common.getIntValue(temp.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정되지 않은 설정값이 있습니다. 설정 후 다시 시작하여 주십시오.");
            }
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

                if (Common.getIntValue(hour) >= Program.cont.CutOffHour && Common.getIntValue(minute) >= Program.cont.CutOffMinute)
                    return true;
                else
                    return false;
            }
            else
            {
                DateTime time = DateTime.Now;
                // 실제 장인 경우 15:18분에 팔자
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

        public static long GetFee(long price)
        {
            return (long)Math.Truncate(price * Program.cont.getFee);
        }

        public static long GetTax(long price)
        {
            return (long)Math.Truncate(price * Program.cont.getTax);
        }

        // 구매가능 수량 계산
        // Price : 단가
        // TotalMoney : 전체금액
        // AssignMoney : 현재 종목을 사는데 배정된 금액
        // overAssignMoney : 현재 종목을 사는데 배정된 금액을 넘어 서는 경우 예산을 오버해서 살 것이면, True, 아니면 False
        public static int ChkCanBuyVolume(long totalMoney, long assignMoney, long price, bool overAssignMoney)
        {
            long moneyPerStock = price + Common.GetFee(price);  // 한주 사는데 드는 비용(Price + fee)

            if (moneyPerStock == 0) return 0;

            if (moneyPerStock > assignMoney)    // 1주 사는데, 가능한 비용보다 비싼 주식이라면, 전체 금액에서 이 주식을 살수 있는지 체크
            {
                if (!overAssignMoney) return 0;

                if (moneyPerStock > totalMoney)
                {
                    // 전체 금액을 다 털어도 살 수 없다면 0
                    return 0;
                }
                else
                {
                    // 살 수 있다면 1주만 산다..
                    return 1;
                }
            }
            else
            {
                double cnt = assignMoney / moneyPerStock;
                return (int)cnt;
            }
        }

        // 단순 구매가능 수량 계산
        // Price : 단가
        // AssignMoney : 현재 종목을 사는데 배정된 금액
        public static int SimpleChkCanBuyVolume(long assignMoney, long price)
        {
            long moneyPerStock = price + Common.GetFee(price);  // 한주 사는데 드는 비용(Price + fee)

            if (moneyPerStock == 0) return 0;

            double cnt = assignMoney / moneyPerStock;
            return (int)cnt;
        }

        // 시뮬레이션 시에 무조건 파는 시간 설정
        public static void SetCutOffTime(string time)
        {
            if (time.Split('(').Length > 1)
            {
                try
                {
                    // 2016년 8월 이전에는 3시가 장 종료, 이후는 3시 30분이 장 종료
                    if (Convert.ToInt32(time.Split('(')[1].Split(')')[0].Replace("-", "")) > 20160800)
                    {
                        Program.cont.CutOffHour = 15;
                        Program.cont.CutOffMinute = 18;
                    }
                    else
                    {
                        Program.cont.CutOffHour = 14;
                        Program.cont.CutOffMinute = 48;
                    }
                }
                catch (Exception e)
                {
                    Program.cont.CutOffHour = 14;
                    Program.cont.CutOffMinute = 48;
                }
            }
        }
    }
}
