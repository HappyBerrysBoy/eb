using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eb.Classes
{
    class ClsRealChe
    {
        private string buySign;         // 매수 싸인
        private string sellSign;        // 매도 싸인
        private string order;           // 매수/매도 텍스트
        private string orderVolume;     // 매수량
        private string orderPrice;      // 매수금액
        private string tax;             // 세금
        private string chetime;         // 체결시간
        private string sign;            // 전일대비 +/-/보합 (2:+, 3:보합, 5:-)
        private string change;          // 전일대비 차이금액(절대값임, 전일대비 -100원이라도 100으로 표시됨, 전일대비 100원 올라도 100으로 표시)
        private string drate;           // 등락율(현재 몇오른금액 몇%인지)
        private string price;           // 현재 금액
        private string opentime;        // 거래시작시간
        private string open;            // 시가
        private string hightime;        // 고가시간
        private string high;            // 고가
        private string lowtime;         // 저가시간
        private string low;             // 저가
        private string cgubun;          // 현재 +인지 -인지 +-기호로 표시
        private string cvolume;         // 체결량
        private string volume;          // 거래량
        private string value;           // 누적 거래대금(백만단위)
        private string mdvolume;        // 매도량
        private string mdchecnt;        // 매도 횟수
        private string msvolume;        // 매수량
        private string mschecnt;        // 매수 횟수
        private string cpower;          // 체결강도
        private string w_avrg;          // 가중평균가
        private string offerho;         // 매도호가
        private string bidho;           // 매수호가
        private string status;          // 장정보
        private string jnilvolume;      // 전일동시간대거래량
        private string shcode;          // 종목코드
        private string fee;             // 수수료

        public ClsRealChe()
        {
            buySign = "";
            sellSign = "";
            order = "";
            orderVolume = "";
            orderPrice = "";
            tax = "";
            chetime = "";
            sign = "";
            change = "";
            drate = "";
            price = "";
            opentime = "";
            open = "";
            hightime = "";
            high = "";
            lowtime = "";
            low = "";
            cgubun = "";
            cvolume = "";
            volume = "";
            value = "";
            mdvolume = "";
            mdchecnt = "";
            msvolume = "";
            mschecnt = "";
            cpower = "";
            w_avrg = "";
            offerho = "";
            bidho = "";
            status = "";
            jnilvolume = "";
            shcode = "";
            fee = "";
        }

        public string BuySign
        {
            get { return buySign; }
            set { buySign = value; }
        }

        public string SellSign
        {
            get { return sellSign; }
            set { sellSign = value; }
        }

        public string Order
        {
            get { return order; }
            set { order = value; }
        }

        public string OrderVolume
        {
            get { return orderVolume; }
            set { orderVolume = value; }
        }

        public string OrderPrice
        {
            get { return orderPrice; }
            set { orderPrice = value; }
        }

        public string Tax
        {
            get { return tax; }
            set { tax = value; }
        }

        public string Chetime
        {
            get { return chetime; }
            set { chetime = value; }
        }

        public string Sign
        {
            get { return sign; }
            set { sign = value; }
        }

        public string Change
        {
            get { return change; }
            set { change = value; }
        }

        public string Drate
        {
            get { return drate; }
            set { drate = value; }
        }

        public string Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Opentime
        {
            get { return opentime; }
            set { opentime = value; }
        }

        public string Open
        {
            get { return open; }
            set { open = value; }
        }

        public string Hightime
        {
            get { return hightime; }
            set { hightime = value; }
        }

        public string High
        {
            get { return high; }
            set { high = value; }
        }

        public string Lowtime
        {
            get { return lowtime; }
            set { lowtime = value; }
        }

        public string Low
        {
            get { return low; }
            set { low = value; }
        }

        public string Cgubun
        {
            get { return cgubun; }
            set { cgubun = value; }
        }

        public string Cvolume
        {
            get { return cvolume; }
            set { cvolume = value; }
        }

        public string Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public string Mdvolume
        {
            get { return mdvolume; }
            set { mdvolume = value; }
        }

        public string Mdchecnt
        {
            get { return mdchecnt; }
            set { mdchecnt = value; }
        }

        public string Msvolume
        {
            get { return msvolume; }
            set { msvolume = value; }
        }

        public string Mschecnt
        {
            get { return mschecnt; }
            set { mschecnt = value; }
        }

        public string Cpower
        {
            get { return cpower; }
            set { cpower = value; }
        }

        public string W_avrg
        {
            get { return w_avrg; }
            set { w_avrg = value; }
        }

        public string Offerho
        {
            get { return offerho; }
            set { offerho = value; }
        }

        public string Bidho
        {
            get { return bidho; }
            set { bidho = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Jnilvolume
        {
            get { return jnilvolume; }
            set { jnilvolume = value; }
        }

        public string Shcode
        {
            get { return shcode; }
            set { shcode = value; }
        }

        public string Fee
        {
            get { return fee; }
            set { fee = value; }
        }
    }
}
