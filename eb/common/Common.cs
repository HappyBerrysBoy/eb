using eb.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
            DIFFERENCE_CHEPOWER,
            SATISFY_PROFIT,
            CUT_OFF_HOUR,
            CUT_OFF_MIN
        }

        public static void SetConfig()
        {
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName);
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split('/').Length > 2)
                    {
                        string[] strs = line.Split('/');
                        Program.cont.VolumeHistoryCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.VOLUME_HISTORY_CNT]);
                        Program.cont.CutoffPercent = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.CUT_OFF_PERCENT]);
                        Program.cont.ProfitCutoffPercent = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.PROFIT_CUT_OFF_PERCENT]);
                        Program.cont.PowerLowLimit = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.POWER_LOW_LIMIT]);
                        Program.cont.PowerHighLimit = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.POWER_HIGH_LIMIT]);
                        Program.cont.IgnoreCheCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.IGNORE_CHE_CNT]);
                        Program.cont.PierceHoCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.PIERCE_HO_CNT]);
                        Program.cont.LogTerm = Common.getIntValue(strs[(int)Common.CONFIG_IDX.LOG_TERM]);
                        Program.cont.MsmdRate = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.MS_MD_RATE]);
                        Program.cont.LogTermVolumeOver = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.LOG_TERM_VOLUME_OVER]);
                        Program.cont.OrderSignCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.ORDER_SIGN_CNT]);
                        Program.cont.SellSignCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.SELL_SIGN_CNT]);
                        Program.cont.MsCutLine = Common.getIntValue(strs[(int)Common.CONFIG_IDX.MS_CUT_LINE]);
                        Program.cont.MdCutLine = Common.getIntValue(strs[(int)Common.CONFIG_IDX.MD_CUT_LINE]);
                        Program.cont.DifferenceChePower = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.DIFFERENCE_CHEPOWER]);
                        Program.cont.SatisfyProfit = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.SATISFY_PROFIT]);
                        Program.cont.CutOffHour = Common.getIntValue(strs[(int)Common.CONFIG_IDX.CUT_OFF_HOUR]);
                        Program.cont.CutOffMinute = Common.getIntValue(strs[(int)Common.CONFIG_IDX.CUT_OFF_MIN]);
                    }
                }

                sr.Close();
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

    }
}
