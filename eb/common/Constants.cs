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
        private int volumeHistoryCnt = 0;           // 최근 거래량 몇일치 조회 할건지
        private double cutoffPercent = 0;           // 손절 기준 %
        private double profitCutoffPercent = 0;     // 익절 기준 %

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
