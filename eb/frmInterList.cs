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
        XA_DATASETLib.XAQuery cspat00600Query;  // 정상주문
        XA_DATASETLib.XAQuery cspat00700Query;  // 정정주문
        XA_DATASETLib.XAQuery cspat00800Query;  // 취소주문
        XA_DATASETLib.XAQuery currQuery;        // 현재 Query를 받아서 처리

        XA_DATASETLib.XAReal queryReal;         // 실시간 시세

        List<string> kindList = new List<string>();

        Hashtable hQueryKind = new Hashtable();
        Hashtable hKindKeyMap = new Hashtable();

        Constants cont = new Constants();

        public frmInterList()
        {
            InitializeComponent();
        }

        private void frmInterList_Load(object sender, EventArgs e)
        {
            kindList.Add("계좌 거래내역");
            kindList.Add("현재가 호가조회");
            kindList.Add("현재가 시세조회");
            kindList.Add("시간대별 체결 조회");
            kindList.Add("주식분별 주가조회");
            kindList.Add("현물 정상주문");
            kindList.Add("현물 정정주문");
            kindList.Add("현물 취소주문");

            hQueryKind.Add("계좌 거래내역", "CDPCQ04700");
            hQueryKind.Add("현재가 호가조회", "t1101");
            hQueryKind.Add("현재가 시세조회", "t1102");
            hQueryKind.Add("시간대별 체결 조회", "t1301");
            hQueryKind.Add("주식분별 주가조회", "t1302");
            hQueryKind.Add("현물 정상주문", "CSPAT00600");
            hQueryKind.Add("현물 정정주문", "CSPAT00700");
            hQueryKind.Add("현물 취소주문", "CSPAT00800");

            hKindKeyMap.Add("CDPCQ04700", cdpcq04700Query);
            hKindKeyMap.Add("t1101", t1101Query);
            hKindKeyMap.Add("t1102", t1102Query);
            hKindKeyMap.Add("t1301", t1301Query);
            hKindKeyMap.Add("t1302", t1302Query);
            hKindKeyMap.Add("CSPAT00600", cspat00600Query);
            hKindKeyMap.Add("CSPAT00700", cspat00700Query);
            hKindKeyMap.Add("CSPAT00800", cspat00800Query);

            for (int i = 0; i < kindList.Count; i++)
            {
                cmbQueryKind.Items.Add(kindList[i]);
            }
        }

        private void btnInquiry_Click(object sender, EventArgs e)
        {
            getRealQuery();

            //for (int i = 0; i < 2; i++)
            //{
            //    currQuery = getCurrQuery(cmbQueryKind.Text);

            //    if (currQuery == null)
            //        return;

            //    if(i == 0)
            //        setQueryFieldData(cmbQueryKind.Text, currQuery, "078020");
            //    else
            //        setQueryFieldData(cmbQueryKind.Text, currQuery, "000300");

            //    if (currQuery.Request(false) < 0)
            //    {
            //        MessageBox.Show("전송오류");
            //    }
            //}
        }

        private void setQueryFieldData(string kind, XA_DATASETLib.XAQuery query, string shCode) 
        {
            switch (kind)
            {
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
                    query.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, cont.getAccount);
                    query.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, cont.getAccountPass);
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
                //getRealQuery
                queryCls = new XA_DATASETLib.XAQuery(cont.getQuery);
                queryCls.ResFileName = cont.getResPath + (string)hQueryKind[query] + cont.getResTag;
                setEventListener(query, queryCls);
                hKindKeyMap[kind] = queryCls;
            }

            return queryCls;
        }

        private void getRealQuery()
        {
            queryReal = new XA_DATASETLib.XAReal(cont.getRealQuery);
            queryReal.ResFileName = cont.getResPath + "K3_" + cont.getResTag;
            queryReal.ReceiveRealData += realdata;
            queryReal.SetFieldData("InBlock", "shcode", "035720");
            queryReal.AdviseRealData();
            queryReal.SetFieldData("InBlock", "shcode", "068270");
            queryReal.AdviseRealData();
        }

        StreamWriter file = null;

        private void realdata(string szTrCode)
        {
            string shcode = queryReal.GetFieldData("OutBlock", "shcode");
            string chetime = queryReal.GetFieldData("OutBlock", "chetime");
            string sign = queryReal.GetFieldData("OutBlock", "sign");
            string change = queryReal.GetFieldData("OutBlock", "change");
            string drate = queryReal.GetFieldData("OutBlock", "drate");
            string price = queryReal.GetFieldData("OutBlock", "price");
            string opentime = queryReal.GetFieldData("OutBlock", "opentime");
            string open = queryReal.GetFieldData("OutBlock", "open");
            string hightime = queryReal.GetFieldData("OutBlock", "hightime");
            string high = queryReal.GetFieldData("OutBlock", "high");
            string lowtime = queryReal.GetFieldData("OutBlock", "lowtime");
            string low = queryReal.GetFieldData("OutBlock", "low");
            string cgubun = queryReal.GetFieldData("OutBlock", "cgubun");
            string cvolume = queryReal.GetFieldData("OutBlock", "cvolume");
            string volume = queryReal.GetFieldData("OutBlock", "volume");
            string value = queryReal.GetFieldData("OutBlock", "value");
            string mdvolume = queryReal.GetFieldData("OutBlock", "mdvolume");
            string mdchecnt = queryReal.GetFieldData("OutBlock", "mdchecnt");
            string msvolume = queryReal.GetFieldData("OutBlock", "msvolume");
            string mschecnt = queryReal.GetFieldData("OutBlock", "mschecnt");
            string cpower = queryReal.GetFieldData("OutBlock", "cpower");
            string w_avrg = queryReal.GetFieldData("OutBlock", "w_avrg");
            string offerho = queryReal.GetFieldData("OutBlock", "offerho");
            string bidho = queryReal.GetFieldData("OutBlock", "bidho");
            string status = queryReal.GetFieldData("OutBlock", "status");
            string jnilvolume = queryReal.GetFieldData("OutBlock", "jnilvolume");

            string filename = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/logs/" + shcode + ".txt";

            FileInfo fileInfo = new FileInfo(filename);

            //K3_
            //shcode:035720     // 코드
            //chetime:141241    // 체결시간
            //sign:2            // 전일대비 구분
            //change:2700       // 전일대비
            //drate:2.49
            //price:111000
            //opentime:090005
            //open:109700
            //hightime:090948
            //high:112300
            //lowtime:090005
            //low:109700
            //cgubun:-
            //cvolume:1
            //volume:169402
            //value:18795
            //mdvolume:79015
            //mdchecnt:2920
            //msvolume:86029
            //mschecnt:2318
            //cpower:108.88
            //w_avrg:110952
            //offerho:111100
            //bidho:111000
            //status:00
            //jnilvolume:236400

            if (!fileInfo.Exists)
            {
                //fileInfo.Create();
                //fileInfo.
                file = new StreamWriter(filename, true);
                file.AutoFlush = true;                file.WriteLine("start");
                file.WriteLine("code".PadRight(7, ' ') + "chetime".PadLeft(8, ' ') + "sign ".PadLeft(5, ' ') + "change".PadLeft(9, ' ') + "drate".PadLeft(9, ' ') 
                                + "price".PadLeft(9, ' ') + "open".PadLeft(9, ' ') + "high".PadLeft(9, ' ') + "low".PadLeft(9, ' ') + "gubun".PadLeft(6, ' ') 
                                + "cvolume".PadLeft(9, ' ') + "volume".PadLeft(13, ' ') + "value".PadLeft(13, ' ') + "mdvolume".PadLeft(13, ' ') + "mdchecnt".PadLeft(9, ' ')
                                + "msvolume".PadLeft(13, ' ') + "mschecnt".PadLeft(9, ' ') + "cpower".PadLeft(9, ' ') + "offerho".PadLeft(9, ' ') + "bidho".PadLeft(9, ' ')
                                + "status".PadLeft(7, ' ') + "jnilvolume".PadLeft(13, ' '));
                file.Close();
            }

            file = new StreamWriter(filename, true);

            file.WriteLine(shcode.PadLeft(7, ' ') + chetime.PadLeft(8, ' ') + sign.PadLeft(5, ' ') + change.PadLeft(9, ' ') + drate.PadLeft(9, ' ')
                                + price.PadLeft(9, ' ') + open.PadLeft(9, ' ') + high.PadLeft(9, ' ') + low.PadLeft(9, ' ') + cgubun.PadLeft(6, ' ')
                                + cvolume.PadLeft(9, ' ') + volume.PadLeft(13, ' ') + value.PadLeft(13, ' ') + mdvolume.PadLeft(13, ' ') + mdchecnt.PadLeft(9, ' ')
                                + msvolume.PadLeft(13, ' ') + mschecnt.PadLeft(9, ' ') + cpower.PadLeft(9, ' ') + offerho.PadLeft(9, ' ') + bidho.PadLeft(9, ' ')
                                + status.PadLeft(7, ' ') + jnilvolume.PadLeft(13, ' '));
            file.Close();

            //Console.WriteLine(szTrCode);

            //Console.WriteLine("shcode:" + queryReal.GetFieldData("OutBlock", "shcode"));
            //Console.WriteLine("chetime:" + queryReal.GetFieldData("OutBlock", "chetime"));
            //Console.WriteLine("sign:" + queryReal.GetFieldData("OutBlock", "sign"));
            //Console.WriteLine("change:" + queryReal.GetFieldData("OutBlock", "change"));
            //Console.WriteLine("drate:" + queryReal.GetFieldData("OutBlock", "drate"));
            //Console.WriteLine("price:" + queryReal.GetFieldData("OutBlock", "price"));
            //Console.WriteLine("opentime:" + queryReal.GetFieldData("OutBlock", "opentime"));
            //Console.WriteLine("open:" + queryReal.GetFieldData("OutBlock", "open"));
            //Console.WriteLine("hightime:" + queryReal.GetFieldData("OutBlock", "hightime"));
            //Console.WriteLine("high:" + queryReal.GetFieldData("OutBlock", "high"));
            //Console.WriteLine("lowtime:" + queryReal.GetFieldData("OutBlock", "lowtime"));
            //Console.WriteLine("low:" + queryReal.GetFieldData("OutBlock", "low"));
            //Console.WriteLine("cgubun:" + queryReal.GetFieldData("OutBlock", "cgubun"));
            //Console.WriteLine("cvolume:" + queryReal.GetFieldData("OutBlock", "cvolume"));
            //Console.WriteLine("volume:" + queryReal.GetFieldData("OutBlock", "volume"));
            //Console.WriteLine("value:" + queryReal.GetFieldData("OutBlock", "value"));
            //Console.WriteLine("mdvolume:" + queryReal.GetFieldData("OutBlock", "mdvolume"));
            //Console.WriteLine("mdchecnt:" + queryReal.GetFieldData("OutBlock", "mdchecnt"));
            //Console.WriteLine("msvolume:" + queryReal.GetFieldData("OutBlock", "msvolume"));
            //Console.WriteLine("mschecnt:" + queryReal.GetFieldData("OutBlock", "mschecnt"));
            //Console.WriteLine("cpower:" + queryReal.GetFieldData("OutBlock", "cpower"));
            //Console.WriteLine("w_avrg:" + queryReal.GetFieldData("OutBlock", "w_avrg"));
            //Console.WriteLine("offerho:" + queryReal.GetFieldData("OutBlock", "offerho"));
            //Console.WriteLine("bidho:" + queryReal.GetFieldData("OutBlock", "bidho"));
            //Console.WriteLine("status:" + queryReal.GetFieldData("OutBlock", "status"));
            //Console.WriteLine("jnilvolume:" + queryReal.GetFieldData("OutBlock", "jnilvolume"));
            //Console.WriteLine("===============================");
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
        #endregion

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            frm.Owner = this;
            frm.ShowDialog();
        }
    }
}
