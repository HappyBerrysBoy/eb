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
        private Item item;

        public ChkSellOrder(Item item)
        {
            this.item = item;
        }

        public string ChkSellSign(bool isSimulation)
        {
            ClsRealChe realCls = item.Logs[item.Logs.Count - 1];

            double currRate = Common.getDoubleValue(realCls.Drate);

            // 2시 48분 이후에는 무조건 판매
            if (Common.chkCutOffTime(realCls, isSimulation))
            {
                return "시간제한";
            }
            else
            {
                // 기준 %이상 오르면 무조건 팔아버리자..(앞 조건들 무시..)
                if (ChkMdSatisfy(item.PurchasedRate, currRate, item.HighRate))
                    return "기준%이상오름";

                // 맥스 기준 %이상 넘어가면 무조건 팔아버리자..(앞 조건들 무시..)
                if (chkMdCutLine(realCls))
                    return "MAX기준이상오름";

                // 구매가격 기준 손절% 또는 최고가 대비 익절% 이하로 내려 가는 경우
                string cutoff = chkCutOff(item.PurchasedRate, currRate, item.HighRate);

                // 판매 신호가 연속 몇회 이상 날아 오는 경우
                string sellSignCnt = chkSellSignCnt(item, cutoff);

                // 거래량을 동반한 매도총량이 어느정도 기준을 넘어서는 경우
                // 체결강도가 어느정도 이상 떨어진다던지..

                return cutoff + sellSignCnt;
            }
        }

        private bool ChkMdSatisfy(double purchasedRate, double currRate, double highRate)
        {
            if (Program.cont.SatisfyProfit == 0) return false;

            if (purchasedRate + Program.cont.SatisfyProfit <= currRate)
                return true;
            else
                return false;
        }

        private bool chkMdCutLine(ClsRealChe realCls)
        {
            if (Common.getDoubleValue(realCls.Drate) > Program.cont.MdCutLine)
                return true;
            else
                return false;
        }

        private string chkCutOff(double purchasedRate, double currRate, double highRate)
        {
            if (purchasedRate - Program.cont.CutoffPercent >= currRate)
                return "1";
            else if (highRate - Program.cont.ProfitCutoffPercent >= currRate)
                return "3";
            else
                return "2";
        }

        private string chkSellSignCnt(Item item, string sign)
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
