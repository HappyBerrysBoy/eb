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
        private Item item;

        public ChkBuyOrder(Item item)
        {
            this.item = item;
        }

        public string ChkBuySign()
        {
            // 현재 오른 %가 너무 높으면... 사지 말까? 예를들어 20%라든지... 
            // 3분 5분 이동 평균선상 내려가는 추세라면 사지 말자~?

            ClsRealChe realCls = item.Logs[item.Logs.Count - 1];

            if (Program.cont.MsOnlyOnce)
            {
                if (item.MsOverOne)
                {
                    // 하루에 한번 이상은 거래 안함..
                    return "22222";
                }
            }

            // 너무 높은 %에서는 구매하지 말자.. 20% 이상이라던지..(앞에 조건들 무시하고 안삼..)
            if (chkMsCutLine(realCls))
                return "222";

            // 전날 거래량이 어느정도 이상 거래되어야만 거래 진행
            if (chkMinVolume(realCls))
                return "2222";

            // 매수 후 일정시간내에 또 재매수 하는것 금지(너무 자주 구매하는것 금지)
            string chkDontAllowBuyThisTime = ChkDontAllowBuyThisTime(realCls);

            string chkCheCntBetweenGap = ChkCheCntBetweenGap(realCls);

            // 1분정도 로그보고 졸라 많이 들어오면 Ok
            // 3분정도 로그보고 꾸준히 들어와도 Ok 일단 위아래 둘중에 어떤게 나은지 모니터링 해보자..
            // 일정기간 매수량이 매도량를 압도
            string msVolumeDueTime = chkMsVolumeDueTime();
            // 일정기간 거래량이 일별 평균 거래량의 특정 비율을 넘어서야함
            string overVolume = chkOverVolume();          // 일정기간 평균 거래량 보다 높은거..
            //string overVolume = ChkChePowerDueTime();       // 하루내에 일정기간동안 체결강도가 설정된 값보다 넘어서는 부분
            // 호가를 2개~3개 정도 뚫어주거나 % 기준으로 어느정도 올랐을 경우
            string pierce = pierceUp();
            // 체결강도가 너무 낮지 않아야 함
            string chePower = getChePower(realCls);
            // 체결강도가 0이거나 1000 이상 올라 가는건 최초 동시호가 근처이므로 배제해야 할듯(매도누적체결건수로 해도 될듯.. 순서대로 체결건수만큼 올라감)
            string initTime = chkInitOrder(realCls);
            // 구매신호가 설정된 값 이상 연속으로 왔는가..
            string orderSignCnt = chkOrderSignCnt(msVolumeDueTime + overVolume + pierce + initTime);

            return msVolumeDueTime + overVolume + pierce + chePower + initTime + orderSignCnt + chkCheCntBetweenGap;
        }

        // 로그기간 동안 거래 횟수가 최소 설정된 값을 넘어서는가..
        private string ChkCheCntBetweenGap(ClsRealChe realCls)
        {
            if (item.ToTimeIdx - item.FromTimeIdx >= Program.cont.MinCheCntBetweenGap)
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }

        // 매수 후 일정시간내에 또 재매수 하는것 금지(너무 자주 구매하는것 금지)
        private string ChkDontAllowBuyThisTime(ClsRealChe realCls)
        {
            TimeSpan mstime = Common.getTime(item.MsTime);
            TimeSpan currtime = Common.getTime(realCls.Chetime);
            TimeSpan t3 = currtime - mstime;

            if (Program.cont.DontAllowBuyInThisTime <= t3.TotalSeconds)
            {
                return "1";
            }
            else
            {
                return "2T";
            }
        }

        // 전날 거래량이 어느정도 이상 거래되어야만 거래 진행
        // 몇몇 우량주나 이런건.. 전날 거래량이 몇천주 밖에 안되서 그냥 100주만 사도 거래대상에 포함되어 버림
        private bool chkMinVolume(ClsRealChe realCls)
        {
            if (item.AvgVolumeFewDays > Program.cont.MinVolume)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Rule 0. 너무 높은 가격에서는 매수하지 않는다.. 매수 최대 % 설정
        // 이 조건에 안맞으면 뒤에 조건이 매수신호라고 해도 구매 안함..
        private bool chkMsCutLine(ClsRealChe realCls)
        {
            if (Common.getDoubleValue(realCls.Drate) > Program.cont.MsCutLine)
                return true;
            else
                return false;
        }

        // Rule 1.
        // 1분정도 로그보고 졸라 많이 들어오면 Ok
        // 3분정도 로그보고 꾸준히 들어와도 Ok 일단 위아래 둘중에 어떤게 나은지 모니터링 해보자..
        // 일정기간 매수량이 매도량를 압도
        private string chkMsVolumeDueTime()
        {
            double msVolume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Msvolume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Msvolume);
            double mdVolume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Mdvolume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Mdvolume);

            if (msVolume == 0) return "2";

            if (mdVolume / msVolume < Program.cont.MsmdRate)
                return "1";
            else
                return "2";
        }

        // Rule 2-1.
        // 체결 강도의 급변화를 탐지해서 구매해보는건... 2-2이랑 비교해보자..
        private string ChkChePowerDueTime()
        {
            double differenceChePower = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Cpower) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Cpower);

            if (differenceChePower > Program.cont.DifferenceChePower)
                return "1";
            else
                return "2";
        }

        // Rule 2-2.
        // 일정기간 거래량이 일별 평균 거래량의 특정 비율을 넘어서야함
        private string chkOverVolume()
        {
            double volume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Volume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Volume);

            if (item.AvgVolumeFewDays < 1)
                return "2";
            else if (item.AvgVolumeFewDays * Program.cont.LogTermVolumeOver / 100 < volume)
                return "1";
            else
                return "2";
        }

        // Rule 3.
        // 호가를 2개~3개 정도 뚫어주거나 % 기준으로 어느정도 올랐을 경우
        private string pierceUp()
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

        // Rule 4.
        // 체결강도가 너무 낮지 않아야 함
        private string getChePower(ClsRealChe cls)
        {
            double power = Common.getDoubleValue(cls.Cpower);

            if (Program.cont.PowerLowLimit > power)
                return "2";
            else
                return "1";
        }

        // Rule 5.
        // 체결강도가 0이거나 1000 이상 올라 가는건 최초 동시호가 근처이므로 배제해야 할듯(매도누적체결건수로 해도 될듯.. 순서대로 체결건수만큼 올라감)
        private string chkInitOrder(ClsRealChe realCls)
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

        // Rule 6.
        // 구매신호가 설정된 값 이상 연속으로 왔는가..
        private string chkOrderSignCnt(string sign)
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
    }
}
