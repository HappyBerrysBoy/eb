using eb.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb.Classes
{
    class ChkSellOrder
    {
        public static bool chkMdCutLine(ClsRealChe realCls)
        {
            if (Common.getDoubleValue(realCls.Drate) > Program.cont.MdCutLine)
                return true;
            else
                return false;
        }

        public static string chkCutOff(double purchasedRate, double currRate, double highRate)
        {
            if (purchasedRate - Program.cont.CutoffPercent >= currRate)
                return "1";
            else if (highRate - Program.cont.ProfitCutoffPercent >= currRate)
                return "3";
            else
                return "2";
        }

        public static string chkSellSignCnt(Item item, string sign)
        {
            if (!sign.Contains("2"))
            {
                item.SellSignCnt++;
                if (item.SellSignCnt >= Program.cont.SellSignCnt)
                    return "1";
                else
                    return "2";
            }
            else
            {
                if (item.SellSignCnt > 0)
                    item.SellSignCnt--;
                return "2";
            }
        }
    }
}
