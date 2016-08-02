using eb.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb.Classes
{
    class ChkBuyOrder
    {
        public static string chkMsVolumeDueTime(Item item)
        {
            double msVolume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Msvolume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Msvolume);
            double mdVolume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Mdvolume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Mdvolume);

            if (msVolume == 0) return "2";

            if (mdVolume / msVolume < Program.cont.MsmdRate)
                return "1";
            else
                return "2";
        }

        public static string chkOverVolume(Item item)
        {
            double volume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Volume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Volume);

            if (item.AvgVolumeFewDays < 1)
                return "2";
            else if (item.AvgVolumeFewDays * Program.cont.LogTermVolumeOver < volume)
                return "1";
            else
                return "2";
        }

        public static string pierceUp(Item item)
        {
            // 호가 뚫는거 체크를 안하면 무조건 O
            if (Program.cont.PierceHoCnt < 2) return "1";
            // 아직 데이터가 충분히 안쌓였으면 X
            if (Program.cont.PierceHoCnt > item.RateHistory.Count) return "2";

            List<string> hoList = new List<string>();

            for (int i = 0; i < Program.cont.PierceHoCnt; i++)
            {
                hoList.Add(item.RateHistory[item.RateHistory.Count - 1 - i]);
            }

            string beforeHo = hoList[0];

            for (int i = 1; i < hoList.Count; i++)
            {
                if (Common.getDoubleValue(beforeHo) > Common.getDoubleValue(hoList[i]))
                {
                    beforeHo = hoList[i];
                }
                else
                {
                    return "2";
                }
            }

            return "1";
        }

        public static string getChePower(ClsRealChe cls)
        {
            double power = Common.getDoubleValue(cls.Cpower);

            if (Program.cont.PowerLowLimit > power)
                return "2";
            else
                return "1";
        }

        // 체결강도가 0이거나 1000 이상 올라 가는건 최초 동시호가 근처이므로 배제해야 할듯(매도누적체결건수로 해도 될듯.. 순서대로 체결건수만큼 올라감)
        public static string chkInitOrder(ClsRealChe realCls)
        {
            double power = Common.getDoubleValue(realCls.Cpower);
            double mdcnt = Common.getDoubleValue(realCls.Mdchecnt);
            double mscnt = Common.getDoubleValue(realCls.Mschecnt);

            if (power < Program.cont.PowerLowLimit || power > Program.cont.PowerHighLimit)
                return "2";

            if (mdcnt < Program.cont.IgnoreCheCnt)
                return "2";

            if (mscnt < Program.cont.IgnoreCheCnt)
                return "2";

            return "1";
        }

        public static string chkOrderSignCnt(Item item, string sign)
        {
            if (!sign.Contains("2"))
            {
                item.OrderSignCnt++;
                if (item.OrderSignCnt >= Program.cont.OrderSignCnt)
                    return "1";
                else
                    return "2";
            }
            else
            {
                if (item.OrderSignCnt > 0)
                    item.OrderSignCnt--;
                return "2";
            }
        }

        public static bool chkMsCutLine(ClsRealChe realCls)
        {
            if (Common.getDoubleValue(realCls.Drate) > Program.cont.MsCutLine)
                return true;
            else
                return false;
        }
    }
}
