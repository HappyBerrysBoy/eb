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

        StreamWriter file = null;   // log 수집용 stream

        List<T1305> lstT1305 = new List<T1305>();

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

            // 연속 조회 컨트롤 여부 입력
            hContinueMap.Add("계좌 거래내역", false);
            hContinueMap.Add("현재가 호가조회", false);
            hContinueMap.Add("현재가 시세조회", false);
            hContinueMap.Add("시간대별 체결 조회", false);
            hContinueMap.Add("주식분별 주가조회", false);
            hContinueMap.Add("기간별 주가", true);
            hContinueMap.Add("현물 정상주문", false);
            hContinueMap.Add("현물 정정주문", false);
            hContinueMap.Add("현물 취소주문", false);

            hQueryKind.Add("계좌 거래내역", "CDPCQ04700");
            hQueryKind.Add("현재가 호가조회", "t1101");
            hQueryKind.Add("현재가 시세조회", "t1102");
            hQueryKind.Add("시간대별 체결 조회", "t1301");
            hQueryKind.Add("주식분별 주가조회", "t1302");
            hQueryKind.Add("기간별 주가", "t1305");
            hQueryKind.Add("현물 정상주문", "CSPAT00600");
            hQueryKind.Add("현물 정정주문", "CSPAT00700");
            hQueryKind.Add("현물 취소주문", "CSPAT00800");

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

        private void writeLog(XA_DATASETLib.XAReal query)
        {
            string shcode = query.GetFieldData("OutBlock", "shcode");
            string chetime = query.GetFieldData("OutBlock", "chetime");
            string sign = query.GetFieldData("OutBlock", "sign");
            string change = query.GetFieldData("OutBlock", "change");
            string drate = query.GetFieldData("OutBlock", "drate");
            string price = query.GetFieldData("OutBlock", "price");
            string opentime = query.GetFieldData("OutBlock", "opentime");
            string open = query.GetFieldData("OutBlock", "open");
            string hightime = query.GetFieldData("OutBlock", "hightime");
            string high = query.GetFieldData("OutBlock", "high");
            string lowtime = query.GetFieldData("OutBlock", "lowtime");
            string low = query.GetFieldData("OutBlock", "low");
            string cgubun = query.GetFieldData("OutBlock", "cgubun");
            string cvolume = query.GetFieldData("OutBlock", "cvolume");
            string volume = query.GetFieldData("OutBlock", "volume");
            string value = query.GetFieldData("OutBlock", "value");
            string mdvolume = query.GetFieldData("OutBlock", "mdvolume");
            string mdchecnt = query.GetFieldData("OutBlock", "mdchecnt");
            string msvolume = query.GetFieldData("OutBlock", "msvolume");
            string mschecnt = query.GetFieldData("OutBlock", "mschecnt");
            string cpower = query.GetFieldData("OutBlock", "cpower");
            string w_avrg = query.GetFieldData("OutBlock", "w_avrg");
            string offerho = query.GetFieldData("OutBlock", "offerho");
            string bidho = query.GetFieldData("OutBlock", "bidho");
            string status = query.GetFieldData("OutBlock", "status");
            string jnilvolume = query.GetFieldData("OutBlock", "jnilvolume");

            DateTime dt = DateTime.Now;
            string filename = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/logs/" + shcode + "(" + dt.ToShortDateString() + ").txt";

            FileInfo fileInfo = new FileInfo(filename);

            //if (!fileInfo.Exists)
            //{
            //    file = new StreamWriter(filename, true);
            //    file.AutoFlush = true;
            //    file.WriteLine("code".PadRight(7, ' ') + "chetime".PadLeft(8, ' ') + "sign ".PadLeft(5, ' ') + "change".PadLeft(9, ' ') + "drate".PadLeft(9, ' ')
            //                    + "price".PadLeft(9, ' ') + "open".PadLeft(9, ' ') + "high".PadLeft(9, ' ') + "low".PadLeft(9, ' ') + "gubun".PadLeft(6, ' ')
            //                    + "cvolume".PadLeft(9, ' ') + "volume".PadLeft(13, ' ') + "value".PadLeft(13, ' ') + "mdvolume".PadLeft(13, ' ') + "mdchecnt".PadLeft(9, ' ')
            //                    + "msvolume".PadLeft(13, ' ') + "mschecnt".PadLeft(9, ' ') + "cpower".PadLeft(9, ' ') + "offerho".PadLeft(9, ' ') + "bidho".PadLeft(9, ' ')
            //                    + "status".PadLeft(7, ' ') + "jnilvolume".PadLeft(13, ' '));
            //    file.Close();
            //}

            file = new StreamWriter(filename, true);

            file.WriteLine(shcode.PadLeft(7, ' ') + chetime.PadLeft(8, ' ') + sign.PadLeft(5, ' ') + change.PadLeft(9, ' ') + drate.PadLeft(9, ' ')
                                + price.PadLeft(9, ' ') + open.PadLeft(9, ' ') + high.PadLeft(9, ' ') + low.PadLeft(9, ' ') + cgubun.PadLeft(6, ' ')
                                + cvolume.PadLeft(9, ' ') + volume.PadLeft(13, ' ') + value.PadLeft(13, ' ') + mdvolume.PadLeft(13, ' ') + mdchecnt.PadLeft(9, ' ')
                                + msvolume.PadLeft(13, ' ') + mschecnt.PadLeft(9, ' ') + cpower.PadLeft(9, ' ') + offerho.PadLeft(9, ' ') + bidho.PadLeft(9, ' ')
                                + status.PadLeft(7, ' ') + jnilvolume.PadLeft(13, ' '));
            file.Close();
        }

        private void realdataPI(string szTrCode)
        {
            writeLog(queryRealPI);
        }

        private void realdataDAK(string szTrCode)
        {
            writeLog(queryRealDAK);
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

            if ((bool)hContinueMap[cmbQueryKind.Text])
            {
                if (lstT1305.Count >= Program.cont.VolumeHistoryCnt)
                {
                    setAvgVolume(lstT1305);
                }
                else
                {
                    XA_DATASETLib.XAQuery query = getCurrQuery(cmbQueryKind.Text);
                    query.SetFieldData("t1305InBlock", "idx", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "idx", 0));
                    query.SetFieldData("t1305InBlock", "cnt", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "cnt", 0));
                    query.SetFieldData("t1305InBlock", "date", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "date", 0));
                    Thread.Sleep(500);
                    
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
                break;
            }

            lstT1305.Clear();
        }
        #endregion

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.Owner = this;
            frm.ShowDialog();
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

        private void btnLoadInterestList_Click(object sender, EventArgs e)
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

                sr.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (spsInterest.ActiveRowIndex < 0) return;
            spsInterest.Rows[spsInterest.ActiveRowIndex].Remove();
        }

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Program.cont.getApplicationPath + Program.cont.getLogPath;
            ofd.ShowDialog();

            if (ofd.FileName == "") return;

            StreamReader sr = null;

            try
            {
                sr = new StreamReader(ofd.FileName);
                string line;
                spsLog.RowCount = 0;
                string currRate = "";
                string beforeRate = "";

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("chetime")) continue;

                    if (line.Split(' ').Length > 10)
                    {
                        spsLog.RowCount++;
                        string[] strs = line.Split(' ');
                        List<string> strList = new List<string>();

                        for (int i = 0; i < strs.Length; i++)
                        {
                            if (strs[i].Equals("")) continue;
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
                            beforeRate = strList[(int)LOG_COL.DRATE];
                        }
                        else if (!currRate.Equals(strList[(int)LOG_COL.DRATE]) && !beforeRate.Equals(strList[(int)LOG_COL.DRATE]))
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

                sr.Close();
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

            XA_DATASETLib.XAQuery query = getCurrQuery(cmbQueryKind.Text);
            setQueryFieldData(cmbQueryKind.Text, query, spsInterest.Cells[spsInterest.ActiveRowIndex, (int)INTER_COL.CODE].Text);
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
    }
}
