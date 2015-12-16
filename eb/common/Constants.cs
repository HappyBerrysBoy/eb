using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
