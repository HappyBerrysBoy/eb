using eb.Classes;
using eb.common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb
{
    public partial class frmInterList : Form
    {
        XA_DATASETLib.XAQuery cdpcq04700Query;  // 계좌 거래내역
        XA_DATASETLib.XAQuery t1101Query;       // 현재가 호가 조회
        XA_DATASETLib.XAQuery t1102Query;       // 현재가 시세 조회
        XA_DATASETLib.XAQuery t1301Query;       // 시간대별 체결 조회 1초당 2건
        XA_DATASETLib.XAQuery t1302Query;       // 주식분별 주가 조회 1초당 1건
        XA_DATASETLib.XAQuery t1310Query;       // 주식당일전일분틱조회 1초당 2건
        XA_DATASETLib.XAQuery t1305Query;       // 기간별 주가
        XA_DATASETLib.XAQuery cspat00600Query;  // 정상주문
        XA_DATASETLib.XAQuery cspat00700Query;  // 정정주문
        XA_DATASETLib.XAQuery cspat00800Query;  // 취소주문
        XA_DATASETLib.XAQuery currQuery;        // 현재 Query를 받아서 처리

        XA_DATASETLib.XAReal queryRealPI;         // 코스피 실시간 시세
        XA_DATASETLib.XAReal queryRealDAK;         // 코스닥 실시간 시세

        List<string> kindList = new List<string>();

        Hashtable hQueryKind = new Hashtable();
        Hashtable hKindKeyMap = new Hashtable();
        Hashtable hContinueMap = new Hashtable();
        Hashtable hItemLogs = new Hashtable();      // 종목별 Class

        StreamWriter file = null;   // log 수집용 stream

        List<T1305> lstT1305 = new List<T1305>();

        private double ttlbuy = 0;

        enum INTER_COL
        {
            GUBUN,
            CODE,
            NAME,
            USE,
            RATE,
            AVG_VOL
        }

        enum LOG_COL
        {
            CODE,
            CHETIME,
            SIGN,
            CHANGE,
            DRATE,
            PRICE,
            OPEN,
            HIGH,
            LOW,
            GUBUN,
            CVOLUME,
            VOLUME,
            VALUE,
            MDVOLUME,
            MDCHECNT,
            MSVOLUME,
            MSCHECNT,
            CPOWER,
            OFFERHO,
            BIDHO,
            STATUS,
            JNILVOLUME,
            ORDER,
            ORDER_VOLUME,
            ORDER_PRICE,
            TAX
        }

        public frmInterList()
        {
            InitializeComponent();
        }

        private void setConfig()
        {
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName);
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split('/').Length > 2)
                    {
                        string[] strs = line.Split('/');
                        Program.cont.VolumeHistoryCnt = Convert.ToInt32(strs[0]);
                        Program.cont.CutoffPercent = Convert.ToDouble(strs[1]);
                        Program.cont.ProfitCutoffPercent = Convert.ToDouble(strs[2]);
                        Program.cont.PowerLowLimit = Convert.ToDouble(strs[3]);
                        Program.cont.PowerHighLimit = Convert.ToDouble(strs[4]);
                        Program.cont.IgnoreCheCnt = Convert.ToInt32(strs[5]);
                        Program.cont.PierceHoCnt = Convert.ToInt32(strs[6]);
                        Program.cont.LogTerm = Convert.ToInt32(strs[7]);
                        Program.cont.MsmdRate = Convert.ToDouble(strs[8]);
                    }
                }

                sr.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void frmInterList_Load(object sender, EventArgs e)
        {
            setConfig();

            kindList.Add("계좌 거래내역");
            kindList.Add("현재가 호가조회");
            kindList.Add("현재가 시세조회");
            kindList.Add("시간대별 체결 조회");
            kindList.Add("주식분별 주가조회");
            kindList.Add("현물 정상주문");
            kindList.Add("현물 정정주문");
            kindList.Add("현물 취소주문");
            kindList.Add("기간별 주가");

            hQueryKind.Add("계좌 거래내역", "CDPCQ04700");
            hQueryKind.Add("현재가 호가조회", "t1101");
            hQueryKind.Add("현재가 시세조회", "t1102");
            hQueryKind.Add("시간대별 체결 조회", "t1301");
            hQueryKind.Add("주식분별 주가조회", "t1302");
            hQueryKind.Add("기간별 주가", "t1305");
            hQueryKind.Add("현물 정상주문", "CSPAT00600");
            hQueryKind.Add("현물 정정주문", "CSPAT00700");
            hQueryKind.Add("현물 취소주문", "CSPAT00800");

            hContinueMap.Add("계좌 거래내역", false);
            hContinueMap.Add("현재가 호가조회", false);
            hContinueMap.Add("현재가 시세조회", false);
            hContinueMap.Add("시간대별 체결 조회", false);
            hContinueMap.Add("주식분별 주가조회", false);
            hContinueMap.Add("기간별 주가", true);
            hContinueMap.Add("현물 정상주문", false);
            hContinueMap.Add("현물 정정주문", false);
            hContinueMap.Add("현물 취소주문", false);

            hKindKeyMap.Add("CDPCQ04700", cdpcq04700Query);
            hKindKeyMap.Add("t1101", t1101Query);
            hKindKeyMap.Add("t1102", t1102Query);
            hKindKeyMap.Add("t1301", t1301Query);
            hKindKeyMap.Add("t1302", t1302Query);
            hKindKeyMap.Add("t1305", t1305Query);
            hKindKeyMap.Add("CSPAT00600", cspat00600Query);
            hKindKeyMap.Add("CSPAT00700", cspat00700Query);
            hKindKeyMap.Add("CSPAT00800", cspat00800Query);

            for (int i = 0; i < kindList.Count; i++)
            {
                cmbQueryKind.Items.Add(kindList[i]);
            }

            setRealQueryPI();
            setRealQueryDAK();
            loadInterestList();

            for (int i = 0; i < spsInterest.RowCount; i++)
            {
                hItemLogs.Add(spsInterest.Cells[i, (int)INTER_COL.CODE].Text, new Item());
            }

            if (Program.LoggedIn)
            {
                btnDoLog.Enabled = true;
                btnSelectQuery.Enabled = true;
            }

            if (Program.LoggedIn && spsInterest.RowCount > 0)
            {
                setDoQuery("기간별 주가", spsInterest.Cells[0, (int)INTER_COL.CODE].Text);
            }
        }

        private void setRealQueryPI()
        {
            queryRealPI = new XA_DATASETLib.XAReal(Program.cont.getRealQuery);
            queryRealPI.ResFileName = Program.cont.getResPath + "S3_" + Program.cont.getResTag;
            queryRealPI.ReceiveRealData += realdataPI;
        }

        private void setRealQueryDAK()
        {
            queryRealDAK = new XA_DATASETLib.XAReal(Program.cont.getRealQuery);
            queryRealDAK.ResFileName = Program.cont.getResPath + "K3_" + Program.cont.getResTag;
            queryRealDAK.ReceiveRealData += realdataDAK;
        }

        private void setQueryFieldData(string kind, XA_DATASETLib.XAQuery query, string shCode) 
        {
            switch (kind)
            {
                case "기간별 주가":
                    query.SetFieldData("t1305InBlock", "shcode", 0, shCode);
                    query.SetFieldData("t1305InBlock", "dwmcode", 0, "1");
                    query.SetFieldData("t1305InBlock", "date", 0, " ");
                    query.SetFieldData("t1305InBlock", "idx", 0, " ");
                    query.SetFieldData("t1305InBlock", "cnt", 0, "1");
                    break;
                case "현재가 호가조회":
                    query.SetFieldData("t1101InBlock", "shcode", 0, "078020");
                    break;
                case "현재가 시세조회":
                    query.SetFieldData("t1102InBlock", "shcode", 0, "078020");
                    break;
                case "주식분별 주가조회":
                    query.SetFieldData("t1302InBlock", "shcode", 0, shCode);
                    query.SetFieldData("t1302InBlock", "gubun", 0, "0");
                    query.SetFieldData("t1302InBlock", "time", 0, " ");
                    query.SetFieldData("t1302InBlock", "cnt", 0, "1");
                    break;
                case "현물 정상주문":
                    query.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, Program.cont.getAccount);
                    query.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, Program.cont.getAccountPass);
                    query.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, "000300");
                    query.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, "1");
                    query.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, "1100");
                    query.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "2");
                    query.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "00");
                    query.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000");
                    query.SetFieldData("CSPAT00600InBlock1", "LoanDt", 0, "");
                    query.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0");
                    break;
            }
        }

        private XA_DATASETLib.XAQuery getCurrQuery(string query)
        {
            string kind = (string)hQueryKind[query];
            if (kind == null || kind.Equals(""))
            {
                MessageBox.Show("선택한 값이 등록되어 있지 않습니다.");
                return null;
            }

            XA_DATASETLib.XAQuery queryCls = (XA_DATASETLib.XAQuery)hKindKeyMap[kind];
            
            if (queryCls == null)
            {
                queryCls = new XA_DATASETLib.XAQuery(Program.cont.getQuery);
                queryCls.ResFileName = Program.cont.getResPath + (string)hQueryKind[query] + Program.cont.getResTag;
                setEventListener(query, queryCls);
                hKindKeyMap[kind] = queryCls;
            }

            return queryCls;
        }

        private void adviseRealData(XA_DATASETLib.XAReal query, string code)
        {
            query.SetFieldData("InBlock", "shcode", code);
            query.AdviseRealData();
        }

        private ClsRealChe setRealDataClass(XA_DATASETLib.XAReal query)
        {
            ClsRealChe realCls = new ClsRealChe();
            realCls.Shcode = query.GetFieldData("OutBlock", "shcode");
            realCls.Opentime = query.GetFieldData("OutBlock", "opentime");
            realCls.Hightime = query.GetFieldData("OutBlock", "hightime");
            realCls.Lowtime = query.GetFieldData("OutBlock", "lowtime");
            realCls.W_avrg = query.GetFieldData("OutBlock", "w_avrg");
            realCls.Chetime = query.GetFieldData("OutBlock", "chetime");
            realCls.Sign = query.GetFieldData("OutBlock", "sign");
            realCls.Change = query.GetFieldData("OutBlock", "change");
            realCls.Drate = query.GetFieldData("OutBlock", "drate");
            realCls.Price = query.GetFieldData("OutBlock", "price");
            realCls.Open = query.GetFieldData("OutBlock", "open");
            realCls.High = query.GetFieldData("OutBlock", "high");
            realCls.Low = query.GetFieldData("OutBlock", "low");
            realCls.Cgubun = query.GetFieldData("OutBlock", "cgubun");
            realCls.Cvolume = query.GetFieldData("OutBlock", "cvolume");
            realCls.Volume = query.GetFieldData("OutBlock", "volume");
            realCls.Value = query.GetFieldData("OutBlock", "value");
            realCls.Mdvolume = query.GetFieldData("OutBlock", "mdvolume");
            realCls.Mdchecnt = query.GetFieldData("OutBlock", "mdchecnt");
            realCls.Msvolume = query.GetFieldData("OutBlock", "msvolume");
            realCls.Mschecnt = query.GetFieldData("OutBlock", "mschecnt");
            realCls.Cpower = query.GetFieldData("OutBlock", "cpower");
            realCls.Offerho = query.GetFieldData("OutBlock", "offerho");
            realCls.Bidho = query.GetFieldData("OutBlock", "bidho");
            realCls.Status = query.GetFieldData("OutBlock", "status");
            realCls.Jnilvolume = query.GetFieldData("OutBlock", "jnilvolume");

            return realCls;
        }

        private void writeLog(ClsRealChe realCls)
        {
            List<string> lstLog = new List<string>();
            lstLog.Add(realCls.Shcode);
            lstLog.Add(realCls.Chetime);
            lstLog.Add(realCls.Sign);
            lstLog.Add(realCls.Change);
            lstLog.Add(realCls.Drate);
            lstLog.Add(realCls.Price);
            lstLog.Add(realCls.Open);
            lstLog.Add(realCls.High);
            lstLog.Add(realCls.Low);
            lstLog.Add(realCls.Cgubun);
            lstLog.Add(realCls.Cvolume);
            lstLog.Add(realCls.Volume);
            lstLog.Add(realCls.Value);
            lstLog.Add(realCls.Mdvolume);
            lstLog.Add(realCls.Mdchecnt);
            lstLog.Add(realCls.Msvolume);
            lstLog.Add(realCls.Mschecnt);
            lstLog.Add(realCls.Cpower);
            lstLog.Add(realCls.Offerho);
            lstLog.Add(realCls.Bidho);
            lstLog.Add(realCls.Status);
            lstLog.Add(realCls.Jnilvolume);

            DateTime dt = DateTime.Now;
            string filename = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/logs/" + realCls.Shcode + "(" + dt.ToShortDateString() + ").txt";

            FileInfo fileInfo = new FileInfo(filename);

            file = new StreamWriter(filename, true);
            file.WriteLine(getLogFormat(lstLog));
            file.Close();
        }

        private void setItemLog(ClsRealChe realCls)
        {
            Item item = (Item)hItemLogs[realCls.Shcode];
            item.Logs.Add(realCls);

            if (!item.BeforeRate.Equals(realCls.Drate))
            {
                item.BeforeRate = realCls.Drate;
                item.RateHistory.Add(realCls.Drate);
            }
        }

        private string getLogFormat(List<string> lst)
        {
            string result = "";
            for (int i = 0; i < lst.Count; i++)
            {
                if(i == lst.Count -1)
                    result += lst[i];
                else
                    result += lst[i] + "/";
            }
            return result;
        }

        private void realdataProcess(XA_DATASETLib.XAReal query)
        {
            ClsRealChe cls = setRealDataClass(query);
            Item item = (Item)hItemLogs[cls.Shcode];
            writeLog(cls);
            setItemLog(cls);
            chkBuyOrder(item);
        }

        private void realdataPI(string szTrCode)
        {
            realdataProcess(queryRealPI);
        }

        private void realdataDAK(string szTrCode)
        {
            realdataProcess(queryRealDAK);
        }

        private void setEventListener(string kind, XA_DATASETLib.XAQuery query)
        {
            switch (kind)
            {
                case "현재가 호가조회":
                    query.ReceiveData += t1101Query_ReceiveData;
                    query.ReceiveMessage += t1101Query_ReceiveData;
                    break;
                case "현재가 시세조회":
                    query.ReceiveData += t1102Query_ReceiveData;
                    query.ReceiveMessage += t1102Query_ReceiveMessage;
                    break;
                case "주식분별 주가조회":
                    query.ReceiveData += t1302Query_ReceiveData;
                    query.ReceiveMessage += t1302Query_ReceiveMessage;
                    break;
                case "현물 정상주문":
                    query.ReceiveData += cspat00600Query_ReceiveData;
                    query.ReceiveMessage += cspat00600Query_ReceiveMessage;
                    break;
                case "기간별 주가":
                    query.ReceiveData += t1305Query_ReceiveData;
                    query.ReceiveMessage += t1305Query_ReceiveMessage;
                    break;
            }
        }

        #region EventListener
        private void t1101Query_ReceiveData(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            Console.WriteLine(bIsSystemError);
            Console.WriteLine(nMessageCode);
            Console.WriteLine(szMessage);
        }

        private void t1101Query_ReceiveData(string szTrCode)
        {
            Console.WriteLine(szTrCode);
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1101"]).GetFieldData("t1101OutBlock", "hname", 0));
            Console.WriteLine(Common.getSign(((XA_DATASETLib.XAQuery)hKindKeyMap["t1101"]).GetFieldData("t1101OutBlock", "price", 0)));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1101"]).GetFieldData("t1101OutBlock", "change", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1101"]).GetFieldData("t1101OutBlock", "diff", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1101"]).GetFieldData("t1101OutBlock", "volume", 0));
        }

        private void t1102Query_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            Console.WriteLine(bIsSystemError);
            Console.WriteLine(nMessageCode);
            Console.WriteLine(szMessage);
        }

        private void t1102Query_ReceiveData(string szTrCode)
        {
            Console.WriteLine(szTrCode);
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1102"]).GetFieldData("t1102OutBlock", "hname", 0));
        }

        private void cspat00600Query_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            Console.WriteLine(bIsSystemError);
            Console.WriteLine(nMessageCode);
            Console.WriteLine(szMessage);
        }

        private void cspat00600Query_ReceiveData(string szTrCode)
        {
            Console.WriteLine(szTrCode);
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["CSPAT00600"]).GetFieldData("CSPAT00600OutBlock2", "OrdNo", 0));
        }

        private void t1302Query_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            Console.WriteLine(bIsSystemError);
            Console.WriteLine(nMessageCode);
            Console.WriteLine(szMessage);
        }

        private void t1302Query_ReceiveData(string szTrCode)
        {
            Console.WriteLine(szTrCode);
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock", "cts_time", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "chetime", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "close", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "sign", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "change", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "diff", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "chdegree", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "mdvolume", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "msvolume", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "revolume", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "mdchecnt", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "mschecnt", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "rechecnt", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "volume", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "open", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "high", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "low", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "cvolume", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "mdchecnttm", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "mschecnttm", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "totofferrem", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "totbidrem", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "mdvolumetm", 0));
            Console.WriteLine(((XA_DATASETLib.XAQuery)hKindKeyMap["t1302"]).GetFieldData("t1302OutBlock1", "msvolumetm", 0));
        }

        private void t1305Query_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            Console.WriteLine(bIsSystemError);
            Console.WriteLine(nMessageCode);
            Console.WriteLine(szMessage);
        }

        private void t1305Query_ReceiveData(string szTrCode)
        {
            Console.WriteLine(szTrCode);
            T1305 cls = new T1305();
            cls.Date = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "date", 0);
            cls.Open = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "open", 0);
            cls.High = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "high", 0);
            cls.Low = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "low", 0);
            cls.Close = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "close", 0);
            cls.Sign = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "sign", 0);
            cls.Change = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "change", 0);
            cls.Diff = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "diff", 0);
            cls.Volume = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "volume", 0);
            cls.Diff_vol = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "diff_vol", 0);
            cls.Chdegree = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "chdegree", 0);
            cls.Sojinrate = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "sojinrate", 0);
            cls.Changerate = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "changerate", 0);
            cls.Fpvolume = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "fpvolume", 0);
            cls.Covolume = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "covolume", 0);
            cls.Shcode = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "shcode", 0);
            cls.Value = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "value", 0);
            cls.Ppvolume = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "ppvolume", 0);
            cls.O_sign = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "o_sign", 0);
            cls.O_change = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "o_change", 0);
            cls.O_diff = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "o_diff", 0);
            cls.H_sign = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "h_sign", 0);
            cls.H_change = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "h_change", 0);
            cls.H_diff = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "h_diff", 0);
            cls.L_sign = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "l_sign", 0);
            cls.L_change = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "l_change", 0);
            cls.L_diff = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "l_diff", 0);
            cls.Marketcap = ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock1", "marketcap", 0);
            lstT1305.Add(cls);

            if ((bool)hContinueMap["기간별 주가"])
            {
                if (lstT1305.Count >= Program.cont.VolumeHistoryCnt)
                {
                    Console.WriteLine(cls.Shcode + "(" + ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "date", 0) + "):" + cls.Volume);

                    setAvgVolume(lstT1305);



                    for (int i = 0; i < spsInterest.RowCount; i++)
                    {
                        if (spsInterest.Cells[i, (int)INTER_COL.AVG_VOL].Text.Trim().Equals(""))
                        {
                            Thread.Sleep(1100);
                            setDoQuery("기간별 주가", spsInterest.Cells[i, (int)INTER_COL.CODE].Text);
                            break;
                            //XA_DATASETLib.XAQuery query = getCurrQuery("기간별 주가");
                            //query.SetFieldData("t1305InBlock", "shcode", 0, cls.Shcode);
                            //query.SetFieldData("t1305InBlock", "dwmcode", 0, "1");
                            //query.SetFieldData("t1305InBlock", "date", 0, " ");
                            //query.SetFieldData("t1305InBlock", "idx", 0, " ");
                            //query.SetFieldData("t1305InBlock", "cnt", 0, "1");
                            
                            //doQuery(query, false);
                        }
                    }
                }
                else
                {
                    XA_DATASETLib.XAQuery query = getCurrQuery("기간별 주가");
                    query.SetFieldData("t1305InBlock", "idx", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "idx", 0));
                    query.SetFieldData("t1305InBlock", "cnt", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "cnt", 0));
                    query.SetFieldData("t1305InBlock", "date", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "date", 0));
                    Thread.Sleep(1100);
                    Console.WriteLine(cls.Shcode + "(" + ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "date", 0) + "):" + cls.Volume);
                    doQuery(query, true);
                }
            }
        }

        private void setAvgVolume(List<T1305> lst)
        {
            if (lst.Count == 0) return;

            double ttlVolume = 0;

            for (int i = 1; i < lst.Count; i++)
            {
                ttlVolume += Convert.ToDouble(lst[i].Volume);
            }

            string shcode = lst[0].Shcode;

            for (int i = 0; i < spsInterest.RowCount; i++)
            {
                if (!spsInterest.Cells[i, (int)INTER_COL.CODE].Text.Equals(shcode)) continue;
                
                spsInterest.Cells[i, (int)INTER_COL.AVG_VOL].Text = Math.Round(ttlVolume/(lst.Count-1), 0).ToString();
                Item item = (Item)hItemLogs[shcode];
                item.AvgVolumeFewDays = Math.Round(ttlVolume / (lst.Count - 1), 0);

                Console.WriteLine(shcode + " Total :" + Math.Round(ttlVolume / (lst.Count - 1), 0).ToString());
                break;
            }

            lstT1305.Clear();
        }
        #endregion

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            spsInterest.RowCount++;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fileName = Program.cont.getInterlistFilename;
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(Program.cont.getApplicationPath + Program.cont.getInterlistPath + Program.cont.getInterlistFilename);
                for (int i = 0; i < spsInterest.RowCount; i++)
                {
                    string gubun = spsInterest.Cells[i, (int)INTER_COL.GUBUN].Text;
                    string code = spsInterest.Cells[i, (int)INTER_COL.CODE].Text;
                    string name = spsInterest.Cells[i, (int)INTER_COL.NAME].Text;
                    string use = spsInterest.Cells[i, (int)INTER_COL.USE].Text;
                    string rate = spsInterest.Cells[i, (int)INTER_COL.RATE].Text;
                    sw.WriteLine(gubun + "/" + code + "/" + name + "/" + use + "/" + rate);
                }
                sw.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sw.Close();
            }
        }

        private void loadInterestList()
        {
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(Program.cont.getApplicationPath + Program.cont.getInterlistPath + Program.cont.getInterlistFilename);
                string line;
                spsInterest.RowCount = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split('/').Length > 4)
                    {
                        spsInterest.RowCount++;
                        string[] strs = line.Split('/');
                        spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.GUBUN].Text = strs[0];
                        spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.CODE].Text = strs[1];
                        spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.NAME].Text = strs[2];
                        spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.USE].Text = strs[3];
                        spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.RATE].Text = strs[4];
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sr.Close();
            }
        }

        private void btnLoadInterestList_Click(object sender, EventArgs e)
        {
            loadInterestList();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (spsInterest.ActiveRowIndex < 0) return;
            spsInterest.Rows[spsInterest.ActiveRowIndex].Remove();
        }

        private string getDialogFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Program.cont.getApplicationPath + Program.cont.getLogPath;
            ofd.ShowDialog();

            return ofd.FileName;
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            string filename = getDialogFile();
            if (filename == "") return;

            StreamReader sr = null;

            try
            {
                sr = new StreamReader(filename);
                string line;
                spsLog.RowCount = 0;
                string currRate = "";
                string beforeRate = "";

                Cursor.Current = Cursors.WaitCursor;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("chetime")) continue;

                    if (line.Split('/').Length > 10)
                    {
                        spsLog.RowCount++;
                        string[] strs = line.Split('/');
                        List<string> strList = new List<string>();

                        for (int i = 0; i < strs.Length; i++)
                        {
                            //if (strs[i].Equals("")) continue;
                            strList.Add(strs[i]);
                        }

                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CODE].Text = strList[(int)LOG_COL.CODE];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CHETIME].Text = strList[(int)LOG_COL.CHETIME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.SIGN].Text = strList[(int)LOG_COL.SIGN];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CHANGE].Text = strList[(int)LOG_COL.CHANGE];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].Text = strList[(int)LOG_COL.DRATE];
                        if (currRate == "")
                        {
                            currRate = strList[(int)LOG_COL.DRATE];
                        }
                        else if (!currRate.Equals(strList[(int)LOG_COL.DRATE]) && !beforeRate.Equals(strList[(int)LOG_COL.DRATE]) && beforeRate != "")
                        {
                            beforeRate = currRate;
                            currRate = strList[(int)LOG_COL.DRATE];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Color.Red;
                        }
                        else if (!currRate.Equals(strList[(int)LOG_COL.DRATE]))
                        {
                            beforeRate = currRate;
                            currRate = strList[(int)LOG_COL.DRATE];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Color.Orange;
                        }
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.PRICE].Text = strList[(int)LOG_COL.PRICE];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.OPEN].Text = strList[(int)LOG_COL.OPEN];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.HIGH].Text = strList[(int)LOG_COL.HIGH];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.LOW].Text = strList[(int)LOG_COL.LOW];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].Text = strList[(int)LOG_COL.GUBUN];
                        if (strList[(int)LOG_COL.GUBUN].Equals("+"))
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Color.Orange;
                        else
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Color.LightBlue;
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CVOLUME].Text = strList[(int)LOG_COL.CVOLUME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.VOLUME].Text = strList[(int)LOG_COL.VOLUME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.VALUE].Text = strList[(int)LOG_COL.VALUE];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MDVOLUME].Text = strList[(int)LOG_COL.MDVOLUME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MDCHECNT].Text = strList[(int)LOG_COL.MDCHECNT];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MSVOLUME].Text = strList[(int)LOG_COL.MSVOLUME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MSCHECNT].Text = strList[(int)LOG_COL.MSCHECNT];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CPOWER].Text = strList[(int)LOG_COL.CPOWER];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.OFFERHO].Text = strList[(int)LOG_COL.OFFERHO];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.BIDHO].Text = strList[(int)LOG_COL.BIDHO];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.STATUS].Text = strList[(int)LOG_COL.STATUS];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.JNILVOLUME].Text = strList[(int)LOG_COL.JNILVOLUME];
                        if (strList.Count > 22)
                        {
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER].Text = strList[(int)LOG_COL.ORDER];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER_VOLUME].Text = strList[(int)LOG_COL.ORDER_VOLUME];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER_PRICE].Text = strList[(int)LOG_COL.ORDER_PRICE];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.TAX].Text = strList[(int)LOG_COL.TAX];
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sr.Close();
            }
        }

        private void btnDoLog_Click(object sender, EventArgs e)
        {
            getRealLog();
        }

        private void getRealLog()
        {
            for (int i = 0; i < spsInterest.RowCount; i++)
            {
                if (spsInterest.Cells[i, (int)INTER_COL.GUBUN].Text.Equals("P"))    // KOSPI
                    adviseRealData(queryRealPI, spsInterest.Cells[i, (int)INTER_COL.CODE].Text);
                else // KOSDAQ
                    adviseRealData(queryRealDAK, spsInterest.Cells[i, (int)INTER_COL.CODE].Text);
            }
        }

        private void btnSelectQuery_Click(object sender, EventArgs e)
        {
            if (cmbQueryKind.Text.Equals("")) return;
            if (spsInterest.ActiveRowIndex < 0) return;

            setDoQuery(cmbQueryKind.Text, spsInterest.Cells[spsInterest.ActiveRowIndex, (int)INTER_COL.CODE].Text);
        }

        private void setDoQuery(string kind, string shcode)
        {
            XA_DATASETLib.XAQuery query = getCurrQuery(kind);
            setQueryFieldData(kind, query, shcode);
            doQuery(query, false);
        }

        private void doQuery(XA_DATASETLib.XAQuery query, bool seq)
        {
            if (query.Request(seq) < 0)
            {
                Console.WriteLine("전송오류");
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            frmConfig frm = new frmConfig();
            frm.ShowDialog();
        }

        private void btnFindPoint_Click(object sender, EventArgs e)
        {
            if (spsLog.ActiveRowIndex < 0) return;

            for (int i = spsLog.ActiveRowIndex + 1; i < spsLog.RowCount; i++)
            {
                if (spsLog.Cells[i, (int)LOG_COL.DRATE].BackColor == Color.Red)
                {
                    lblSTS.Text = "Cell(" + i.ToString() + "," + (((int)LOG_COL.DRATE).ToString()) + ")";
                    spsLog.ActiveRowIndex = i;
                    spsLog.ActiveColumnIndex = (int)LOG_COL.DRATE;
                    spsLog.SetActiveCell(i, (int)LOG_COL.DRATE, false);
                    break;
                }
            }
        }

        private void spdLog_MouseUp(object sender, MouseEventArgs e)
        {
            FarPoint.Win.Spread.Model.CellRange range = spsLog.GetSelection(0);
            if (range == null) return;

            if (range.Row < 0 || range.Column < 0) return;
            if (range.ColumnCount > 1) return;

            double sum = 0;
            int msCnt = 0;
            double msSum = 0;
            int mdCnt = 0;
            double mdSum = 0;

            for (int i = range.Row; i < range.Row + range.RowCount; i++) 
            {
                try
                {
                    double val = Convert.ToDouble(spsLog.Cells[i, range.Column].Text);
                    sum += val;
                    if (spsLog.Cells[i, (int)LOG_COL.GUBUN].Text.Equals("+"))
                    {
                        msCnt++;
                        msSum += val;
                    }
                    else if (spsLog.Cells[i, (int)LOG_COL.GUBUN].Text.Equals("-"))
                    {
                        mdCnt++;
                        mdSum += val;
                    }
                }
                catch (Exception ex)
                {

                }
            }

            lblSum.Text = "SUM:" + Convert.ToString(sum);
            lblSum.Text += ", AVG:" + Convert.ToString(Math.Round(sum / range.RowCount, 2));
            lblSum.Text += "(매수횟수:" + msCnt.ToString() + ", 매수량:" + msSum.ToString() + ", 매수평균량:" + Convert.ToString(Math.Round(msSum / msCnt, 2));
            lblSum.Text += ", 매도횟수:" + mdCnt.ToString() + ", 매도량:" + mdSum.ToString() + ", 매도평균량:" + Convert.ToString(Math.Round(mdSum / mdCnt, 2)) + ")";
            if(msCnt != 0 && mdCnt != 0)
                lblSum.Text += ", 매도/매수:" + Convert.ToString(Math.Round(mdSum / msSum, 3));
        }

        private void chkBuyOrder(Item item)
        {
            ClsRealChe realCls = item.Logs[item.Logs.Count - 1];

            // 1분정도 로그보고 졸라 많이 들어오면 Ok
            // 3분정도 로그보고 꾸준히 들어와도 Ok 일단 위아래 둘중에 어떤게 나은지 모니터링 해보자..
            // 일정기간 매수량이 매도량를 압도
            string msVolumeDueTime = chkMsVolumeDueTime(item);
            // 일정기간 거래량이 일별 평균 거래량의 특정 비율을 넘어서야함
            string overVolume = chkOverVolume(item);
            // 체결강도가 0이거나 1000 이상 올라 가는건 최초 동시호가 근처이므로 배제해야 할듯(매도누적체결건수로 해도 될듯.. 순서대로 체결건수만큼 올라감)
            string initTime = chkInitOrder(realCls);
            // 호가를 2개~3개 정도 뚫어주거나 % 기준으로 어느정도 올랐을 경우
            string pierce = pierceUp(item);
            // 체결강도가 너무 낮지 않아야 함
            string chePower = getChePower(realCls);
        }

        private string chkMsVolumeDueTime(Item item)
        {
            setTimeIndex(item);
            double msVolume = getDoubleValue(item.Logs[item.ToTimeIdx].Msvolume) - getDoubleValue(item.Logs[item.FromTimeIdx].Msvolume);
            double mdVolume = getDoubleValue(item.Logs[item.ToTimeIdx].Mdvolume) - getDoubleValue(item.Logs[item.FromTimeIdx].Mdvolume);

            if (mdVolume / msVolume < Program.cont.MsmdRate)
                return "1";
            else
                return "2";
        }

        private string chkOverVolume(Item item)
        {

            return "1";
        }

        private void setTimeIndex(Item item)
        {
            ClsRealChe cls = item.Logs[item.Logs.Count - 1];

            item.ToTimeIdx = item.Logs.Count - 1;               // 제일 최근 로그 Index
            if (item.FromTimeIdx < 0 || item.ToTimeIdx < 0)
            {
                item.FromTimeIdx = 0;
            }

            // From Time이랑 toTime 사이가 설정 간격을 넘어 가는가?
            if (chkTimeInterval(item))
            {
                // 넘어갔다.. 다시 설정
                item.FromTimeIdx++;
            }
        }

        private bool chkTimeInterval(Item item)
        {
            TimeSpan t1 = getTime(item.Logs[item.FromTimeIdx].Chetime);
            TimeSpan t2 = getTime(item.Logs[item.ToTimeIdx].Chetime);
            TimeSpan t3 = t2 - t1;

            if (Program.cont.LogTerm <= t3.TotalSeconds)
                return true;
            
            return false;
        }

        private TimeSpan getTime(string time)
        {
            if (time.Length != 6) return new TimeSpan();
            return TimeSpan.Parse(time.Substring(0, 2) + ":" + time.Substring(2, 2) + ":" + time.Substring(4, 2));
        }

        private string getChePower(ClsRealChe cls)
        {
            double power = getDoubleValue(cls.Cpower);

            if (Program.cont.PowerLowLimit > power)
                return "2";
            else
                return "1";
        }

        private string pierceUp(Item item)
        {
            // 호가 뚫는거 체크를 안하면 무조건 O
            if (Program.cont.PierceHoCnt < 2) return "0";
            // 아직 데이터가 충분히 안쌓였으면 X
            if (Program.cont.PierceHoCnt > item.RateHistory.Count) return "X";

            List<string> hoList = new List<string>();

            for (int i = 0; i < Program.cont.PierceHoCnt; i++)
            {
                hoList.Add(item.RateHistory[item.RateHistory.Count - 1 - i]);
            }

            string beforeHo = hoList[0];

            for (int i = 1; i < hoList.Count; i++)
            {
                if (getIntValue(beforeHo) < getIntValue(hoList[i]))
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

        // 체결강도가 0이거나 1000 이상 올라 가는건 최초 동시호가 근처이므로 배제해야 할듯(매도누적체결건수로 해도 될듯.. 순서대로 체결건수만큼 올라감)
        private string chkInitOrder(ClsRealChe realCls)
        {
            double power = getDoubleValue(realCls.Cpower);
            double mdcnt = getDoubleValue(realCls.Mdchecnt);
            double mscnt = getDoubleValue(realCls.Mschecnt);

            if (power < Program.cont.PowerLowLimit || power > Program.cont.PowerHighLimit)
                return "2";

            if (mdcnt < Program.cont.IgnoreCheCnt)
                return "2";

            if (mscnt < Program.cont.IgnoreCheCnt)
                return "2";

            return "1";
        }

        private int getIntValue(string value)
        {
            int result;

            if (int.TryParse(value, out result))
                return result;

            return 0;
        }

        private long getLongValue(string value)
        {
            long result;

            if (long.TryParse(value, out result))
                return result;

            return 0;
        }

        private double getDoubleValue(string value)
        {
            double result;

            if (double.TryParse(value, out result))
                return result;

            return 0;
        }

        private void chkSellOrder()
        {
            // 구매가격 기준 손절% 이하로 내려 가는 경우
            // 익절 판매 조건 탐색 필요(일정 기간 기준 고가대비 특정 % 이하로 떨어지는 경우)
            // 거래량을 동반한 매도총량이 어느정도 기준을 넘어서는 경우
            // 2시 40분 넘으면 판매
        }

        private void setSpread(String line)
        {
            
        }

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            string filename = getDialogFile();
            if (filename == "") return;

            StreamReader sr = null;

            try
            {
                sr = new StreamReader(filename);
                string line;
                spsLog.RowCount = 0;
                string currRate = "";
                string beforeRate = "";

                Cursor.Current = Cursors.WaitCursor;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split('/').Length > 10)
                    {
                        spsLog.RowCount++;
                        string[] strs = line.Split('/');
                        List<string> strList = new List<string>();

                        for (int i = 0; i < strs.Length; i++)
                        {
                            strList.Add(strs[i]);
                        }

                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CODE].Text = strList[(int)LOG_COL.CODE];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CHETIME].Text = strList[(int)LOG_COL.CHETIME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.SIGN].Text = strList[(int)LOG_COL.SIGN];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CHANGE].Text = strList[(int)LOG_COL.CHANGE];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].Text = strList[(int)LOG_COL.DRATE];
                        if (currRate == "")
                        {
                            currRate = strList[(int)LOG_COL.DRATE];
                        }
                        else if (!currRate.Equals(strList[(int)LOG_COL.DRATE]) && !beforeRate.Equals(strList[(int)LOG_COL.DRATE]) && beforeRate != "")
                        {
                            beforeRate = currRate;
                            currRate = strList[(int)LOG_COL.DRATE];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Color.Red;
                        }
                        else if (!currRate.Equals(strList[(int)LOG_COL.DRATE]))
                        {
                            beforeRate = currRate;
                            currRate = strList[(int)LOG_COL.DRATE];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Color.Orange;
                        }
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.PRICE].Text = strList[(int)LOG_COL.PRICE];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.OPEN].Text = strList[(int)LOG_COL.OPEN];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.HIGH].Text = strList[(int)LOG_COL.HIGH];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.LOW].Text = strList[(int)LOG_COL.LOW];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].Text = strList[(int)LOG_COL.GUBUN];
                        if (strList[(int)LOG_COL.GUBUN].Equals("+"))
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Color.Orange;
                        else
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Color.LightBlue;
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CVOLUME].Text = strList[(int)LOG_COL.CVOLUME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.VOLUME].Text = strList[(int)LOG_COL.VOLUME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.VALUE].Text = strList[(int)LOG_COL.VALUE];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MDVOLUME].Text = strList[(int)LOG_COL.MDVOLUME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MDCHECNT].Text = strList[(int)LOG_COL.MDCHECNT];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MSVOLUME].Text = strList[(int)LOG_COL.MSVOLUME];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MSCHECNT].Text = strList[(int)LOG_COL.MSCHECNT];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CPOWER].Text = strList[(int)LOG_COL.CPOWER];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.OFFERHO].Text = strList[(int)LOG_COL.OFFERHO];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.BIDHO].Text = strList[(int)LOG_COL.BIDHO];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.STATUS].Text = strList[(int)LOG_COL.STATUS];
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.JNILVOLUME].Text = strList[(int)LOG_COL.JNILVOLUME];
                        if (strList.Count > 22)
                        {
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER].Text = strList[(int)LOG_COL.ORDER];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER_VOLUME].Text = strList[(int)LOG_COL.ORDER_VOLUME];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER_PRICE].Text = strList[(int)LOG_COL.ORDER_PRICE];
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.TAX].Text = strList[(int)LOG_COL.TAX];
                        }
                    }

                    Application.DoEvents();
                    Thread.Sleep(1000);
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sr.Close();
            }
        }
    }
}
