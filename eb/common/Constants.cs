using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb.common
{
    class Constants
    {
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
        private string CONFIG_FILENAME = "config.ini";
        private string INTERLIST_FILENAME = "interestlist.txt";
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
        private int orderSignCnt = 0;               // 근접한 매수 신호가 몇번이나 있었는가?
        private int sellSignCnt = 0;                // 근접한 매도 신호가 몇번이나 있었는가?
        private int msCutLine = 0;                  // 몇% 이상이면 매수하지 않는다.. ex)20% or 25%
        private int mdCutLine = 0;                  // 몇% 이상이면 매도해버린다. ex)25% or 27%

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
    }
}
