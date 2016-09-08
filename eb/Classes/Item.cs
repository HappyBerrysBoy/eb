using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb.Classes
{
    class Item
    {
        private string code;                // 종목코드
        private string name;                // 종목이름인가?
        private string beforeRate;          // 이전 등락율
        private List<string> rateHistory;   // 등락율 히스토리
        private List<ClsRealChe> logs;      // 종목 거래 로그
        private int fromTimeIdx;            // 매수/매도를 위한 기준시간(from)
        private int toTimeIdx;              // 매수/매도를 위한 기준시간(to)
        private long avgVolumeFewDays;      // 설정된 일만큼의 평균 거래량
        private int msVolume;               // 매수량
        private long price;                 // 매수시 단가
        private bool isPurchased;           // 매수 되었는가?
        private double orderPerRate;        // 1회 주문시 주문 비율
        private double totalOrderRate;      // 전체 구매된 %(예수금 대비)
        private double purchasedRate;       // 몇%에서 구매 되었는가
        private double highRate;            // 구매 이후 최고 Rate
        private int orderSignCnt;           // 근접한 매수 신호 횟수(설정된 값 이상으로 신호가 오면 매수한다.)
        private int sellSignCnt;            // 근접한 매도 신호 횟수(설정된 값 이상으로 신호가 오면 매도한다.)

        public Item()
        {
            code = "";
            name = "";
            beforeRate = "";
            rateHistory = new List<string>();
            logs = new List<ClsRealChe>();
            fromTimeIdx = -1;
            toTimeIdx = -1;
            avgVolumeFewDays = 0;
            isPurchased = false;
            orderPerRate = 0;
            totalOrderRate = 0;
            purchasedRate = 0;
            highRate = 0;
            orderSignCnt = 0;
            sellSignCnt = 0;
        }

        public int SellSignCnt
        {
            get { return sellSignCnt; }
            set { sellSignCnt = value; }
        }

        public int OrderSignCnt
        {
            get { return orderSignCnt; }
            set { orderSignCnt = value; }
        }

        public double TotalOrderRate
        {
            get { return totalOrderRate; }
            set { totalOrderRate = value; }
        }

        public double OrderPerRate
        {
            get { return orderPerRate; }
            set { orderPerRate = value; }
        }

        public double HighRate
        {
            get { return highRate; }
            set { highRate = value; }
        }

        public double PurchasedRate
        {
            get { return purchasedRate; }
            set { purchasedRate = value; }
        }

        public bool IsPurchased
        {
            get { return isPurchased; }
            set { isPurchased = value; }
        }

        public long AvgVolumeFewDays
        {
            get { return avgVolumeFewDays; }
            set { avgVolumeFewDays = value; }
        }

        public int ToTimeIdx
        {
            get { return toTimeIdx; }
            set { toTimeIdx = value; }
        }

        public int FromTimeIdx
        {
            get { return fromTimeIdx; }
            set { fromTimeIdx = value; }
        }
        
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<string> RateHistory
        {
            get { return rateHistory; }
            set { rateHistory = value; }
        }

        internal List<ClsRealChe> Logs
        {
            get { return logs; }
            set { logs = value; }
        }

        public string BeforeRate
        {
            get { return beforeRate; }
            set { beforeRate = value; }
        }

        public int MsVolume
        {
            get { return msVolume; }
            set { msVolume = value; }
        }

        public long Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
