using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb.common
{
    class Constants
    {
        private string INI_SECTION = "OPTIONS";
        private string SESSION = "XA_Session.XASession";
        private string QUERY = "XA_DataSet.XAQuery";
        private string REAL_QUERY = "XA_DataSet.XAReal";
        private string ACCOUNT = "20071736901";
        private string ACCOUNT_PASS = "2788";
        private string ID = "elyts";
        private string PASS = "6tnscjf6";
        private string PUBLIC_KEY = "5Als6tj5#k";
        private string RES_PATH = "\\Res\\";
        private string RES_TAG = ".res";
        private string APPLICATION_PATH = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        private string LOG_FOLDERPATH = "/logs/";
        private string INTERLIST_FOLDERPATH = "/interlist/";
        private string CONFIG_FOLDERPATH = "/config/";
        private string SIMULATION_FOLDERPATH = "/simulation/";
        private string CONFIG_FILENAME = "config.ini";
        private string INTERLIST_FILENAME = "interestlist.txt";
        private string log_path = "logs";
        private string extension = "txt";
        private double TAX = 0.003;                 // 세금(매도시에만 발생)
        private double FEE = 0.00015;               // 수수료(매수/매도 모두 발생)
        private int volumeHistoryCnt = 0;           // 최근 거래량 몇일치 조회 할건지
        private double cutoffPercent = 0;           // 손절 기준 %
        private double profitCutoffPercent = 0;     // 익절 기준 %
        private double powerLowLimit = 0;           // 체결강도 최저 Limit
        private double powerHighLimit = 0;          // 체결강도 최대 Limit
        private int ignoreCheCnt = 0;               // 장시작후 최초 몇 거래 무시조건
        private int pierceHoCnt = 0;                // 구매조건(몇 호가 뚫으면)
        private int logTerm = 0;                    // 일정기간(일정 시점) 얼마 동안 로그로 구매 여부 판단할건지 설정
        private double logTermVolumeOver = 0;       // 일정기간(일정 시점) 로그의 양이 최근 며칠 전체 평균거래량의 몇% 넘으면 구매할건지 설정
        private double msmdRate = 0;                // 매도/매수 비율
        private double minVolume = 0;               // 전날 최소 거래량(전날 기준으로 이 기준 거래량이상 나와야 다음날 매수 고민을한다.. 우량주들 거래량이 쓰레기라 자꾸 걸림)
        private int orderSignCnt = 0;               // 근접한 매수 신호가 몇번이나 있었는가?
        private int sellSignCnt = 0;                // 근접한 매도 신호가 몇번이나 있었는가?
        private int msCutLine = 0;                  // 몇% 이상이면 매수하지 않는다.. ex)20% or 25%
        private int mdCutLine = 0;                  // 몇% 이상이면 매도해버린다. ex)25% or 27%
        private double differenceChePower = 0;      // 일정기간(일정 시점) 로그의 기간동안 설정된 체결강도의 %를 넘어 서면 구매할건지 설정
        private double satisfyProfit = 0;           // 매수 후 여기에 설정된 %만큼 오르면 무조건 매도 해서 수익을 취한다..
        private int dontAllowBuyInThisTime = 0;     // 한번 매수 후 여기 설정된 값 이내 시간안에는 재매수 금지(너무 자주 사는것 방지)
        private int dontAllowSellInThisTime = 0;    // 한번 매수 후 여기 설정된 값 이내 시간안에 매도 금지(사자마자 잠깐 흔드는것에 판매하는것 금지)

        public int AllCodePageNum { get; set; }     // 전체 종목 조회 할 경우에, 현재 실행된 프로그램은 몇번째 프로그램인가
        public int AllCodeTtlPage { get; set; }     // 전체 종목 조회 할 경우에, 전체 종목을 몇개의 프로그램으로 나눌 것인가..

        // 마감전 무조건 파는 시간
        //private int cutOffHour = 15;
        //private int cutOffMinute = 18;
        private int cutOffHour = 0;
        private int cutOffMinute = 0;

        // colors
        private Color buyCell = Color.YellowGreen;  // 실제로 매수 했을 Row
        private Color sellCell = Color.LightBlue;   // 실제로 매도 했을 Row
        private Color changeRate = Color.Orange;    // 비율이 이전과 다르게 한번 변경된 경우
        private Color changeRateOver2 = Color.Red;  // 비율이 두번 이상 변경된 경우
        private Color msSign = Color.Orange;        // 이번 틱이 매수인 경우(+) 기호 색상
        private Color mdSign = Color.LightBlue;     // 이번 틱이 매도인 경우(-) 기호 색상

        public Color MdSign
        {
            get { return mdSign; }
        }

        public Color MsSign
        {
            get { return msSign; }
        }
        
        public Color ChangeRateOver2
        {
            get { return changeRateOver2; }
        }
        
        public Color ChangeRate
        {
            get { return changeRate; }
        }
        
        public Color SellCell
        {
            get { return sellCell; }
        }
        
        public Color BuyCell
        {
            get { return buyCell; }
        }

        public string LOG_PATH
        {
            get { return log_path; }
        }
        public string EXTENSION
        {
            get { return extension; }
        }

        public double DifferenceChePower
        {
            get { return differenceChePower; }
            set { differenceChePower = value; }
        }

        public int MsCutLine
        {
            get { return msCutLine; }
            set { msCutLine = value; }
        }

        public int MdCutLine
        {
            get { return mdCutLine; }
            set { mdCutLine = value; }
        }

        public double getTax
        {
            get { return TAX; }
        }

        public double getFee
        {
            get { return FEE; }
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

        public double LogTermVolumeOver
        {
            get { return logTermVolumeOver; }
            set { logTermVolumeOver = value; }
        }
        public double MsmdRate
        {
            get { return msmdRate; }
            set { msmdRate = value; }
        }
        public double MinVolume
        {
            get { return minVolume; }
            set { minVolume = value; }
        }
        public int LogTerm
        {
            get { return logTerm; }
            set { logTerm = value; }
        }

        public int PierceHoCnt
        {
            get { return pierceHoCnt; }
            set { pierceHoCnt = value; }
        }

        public int IgnoreCheCnt
        {
            get { return ignoreCheCnt; }
            set { ignoreCheCnt = value; }
        }

        public double PowerLowLimit
        {
            get { return powerLowLimit; }
            set { powerLowLimit = value; }
        }

        public double PowerHighLimit
        {
            get { return powerHighLimit; }
            set { powerHighLimit = value; }
        }

        public double CutoffPercent
        {
            get { return cutoffPercent; }
            set { cutoffPercent = value; }
        }
        public double ProfitCutoffPercent
        {
            get { return profitCutoffPercent; }
            set { profitCutoffPercent = value; }
        }

        public int VolumeHistoryCnt
        {
            get { return volumeHistoryCnt; }
            set { volumeHistoryCnt = value; }
        }

        public string getSimulationPath
        {
            get { return SIMULATION_FOLDERPATH; }
        }
        public string getConfigPath
        {
            get { return CONFIG_FOLDERPATH; }
        }
        public string getConfigFileName
        {
            get { return CONFIG_FILENAME; }
        }
        public string getLogPath
        {
            get { return LOG_FOLDERPATH; }
        }
        public string getApplicationPath
        {
            get { return APPLICATION_PATH; }
        }
        public string getInterlistPath
        {
            get { return INTERLIST_FOLDERPATH; }
        }
        public string getInterlistFilename
        {
            get { return INTERLIST_FILENAME; }
        }
        public string getINISection
        {
            get { return INI_SECTION; }
        }
        public string getSession
        {
            get { return SESSION; }
        }
        public string getQuery
        {
            get { return QUERY; }
        }
        public string getRealQuery
        {
            get { return REAL_QUERY; }
        }
        public string getAccount
        {
            get { return ACCOUNT; }
        }
        public string getID
        {
            get { return ID; }
        }
        public string getPass
        {
            get { return PASS; }
        }
        public string getPublicKey
        {
            get { return PUBLIC_KEY; }
        }
        public string getResPath
        {
            get { return RES_PATH; }
        }
        public string getResTag
        {
            get { return RES_TAG; }
        }
        public string getAccountPass
        {
            get { return ACCOUNT_PASS; }
        }

        public int CutOffHour
        {
            get { return cutOffHour; }
            set { cutOffHour = value; }
        }
        public int CutOffMinute
        {
            get { return cutOffMinute; }
            set { cutOffMinute = value; }
        }

        public double SatisfyProfit
        {
            get { return satisfyProfit; }
            set { satisfyProfit = value; }
        }

        public int DontAllowBuyInThisTime
        {
            get { return dontAllowBuyInThisTime; }
            set { dontAllowBuyInThisTime = value; }
        }
        public int DontAllowSellInThisTime
        {
            get { return dontAllowSellInThisTime; }
            set { dontAllowSellInThisTime = value; }
        }
    }
}
