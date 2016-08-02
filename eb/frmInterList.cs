﻿using eb.Classes;
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
        XA_DATASETLib.XAQuery cspaq12200Query;        // 계좌 예수금/주문가능금액 등

        XA_DATASETLib.XAReal queryRealPI;         // 코스피 실시간 시세
        XA_DATASETLib.XAReal queryRealDAK;         // 코스닥 실시간 시세

        List<string> kindList = new List<string>();

        Hashtable hQueryKind = new Hashtable();
        Hashtable hKindKeyMap = new Hashtable();
        Hashtable hContinueMap = new Hashtable();
        Hashtable hItemLogs = new Hashtable();      // 종목별 Class

        StreamWriter file = null;   // log 수집용 stream
        bool chkSimulation = false;
        bool isRecording = false;
        private const string LOG_PATH = "logs";
        private const string EXTENSION = "txt";

        List<T1305> lstT1305 = new List<T1305>();

        private double ttlbuy = 0;                  // 예수금 사용%
        private long initMoney = 0;                 // 시작시 예수금
        private long money = 0;                     // 현재 예수금
        Hashtable hBuyVolume = new Hashtable();     // 구매시 몇주 구매 했는지 기록

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
            BUY_SIGN,
            SELL_SIGN,
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
                        Program.cont.VolumeHistoryCnt = Common.getIntValue(strs[0]);
                        Program.cont.CutoffPercent = Common.getDoubleValue(strs[1]);
                        Program.cont.ProfitCutoffPercent = Common.getDoubleValue(strs[2]);
                        Program.cont.PowerLowLimit = Common.getDoubleValue(strs[3]);
                        Program.cont.PowerHighLimit = Common.getDoubleValue(strs[4]);
                        Program.cont.IgnoreCheCnt = Common.getIntValue(strs[5]);
                        Program.cont.PierceHoCnt = Common.getIntValue(strs[6]);
                        Program.cont.LogTerm = Common.getIntValue(strs[7]);
                        Program.cont.MsmdRate = Common.getDoubleValue(strs[8]);
                        Program.cont.LogTermVolumeOver = Common.getDoubleValue(strs[9]);
                        Program.cont.OrderSignCnt = Common.getIntValue(strs[10]);
                        Program.cont.SellSignCnt = Common.getIntValue(strs[11]);
                    }
                }

                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("설정되지 않은 설정값이 있습니다. 설정 후 다시 시작하여 주십시오.");
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
            kindList.Add("계좌조회");

            hQueryKind.Add("계좌 거래내역", "CDPCQ04700");
            hQueryKind.Add("현재가 호가조회", "t1101");
            hQueryKind.Add("현재가 시세조회", "t1102");
            hQueryKind.Add("시간대별 체결 조회", "t1301");
            hQueryKind.Add("주식분별 주가조회", "t1302");
            hQueryKind.Add("기간별 주가", "t1305");
            hQueryKind.Add("현물 정상주문", "CSPAT00600");
            hQueryKind.Add("현물 정정주문", "CSPAT00700");
            hQueryKind.Add("현물 취소주문", "CSPAT00800");
            hQueryKind.Add("계좌조회", "CSPAQ12200");

            hContinueMap.Add("계좌 거래내역", false);
            hContinueMap.Add("현재가 호가조회", false);
            hContinueMap.Add("현재가 시세조회", false);
            hContinueMap.Add("시간대별 체결 조회", false);
            hContinueMap.Add("주식분별 주가조회", false);
            hContinueMap.Add("기간별 주가", true);
            hContinueMap.Add("현물 정상주문", false);
            hContinueMap.Add("현물 정정주문", false);
            hContinueMap.Add("현물 취소주문", false);
            hContinueMap.Add("계좌조회", false);

            hKindKeyMap.Add("CDPCQ04700", cdpcq04700Query);
            hKindKeyMap.Add("t1101", t1101Query);
            hKindKeyMap.Add("t1102", t1102Query);
            hKindKeyMap.Add("t1301", t1301Query);
            hKindKeyMap.Add("t1302", t1302Query);
            hKindKeyMap.Add("t1305", t1305Query);
            hKindKeyMap.Add("CSPAT00600", cspat00600Query);
            hKindKeyMap.Add("CSPAT00700", cspat00700Query);
            hKindKeyMap.Add("CSPAT00800", cspat00800Query);
            hKindKeyMap.Add("CSPAQ12200", cspaq12200Query);

            for (int i = 0; i < kindList.Count; i++)
            {
                cmbQueryKind.Items.Add(kindList[i]);
            }

            setRealQueryPI();
            setRealQueryDAK();
            loadInterestList();

            for (int i = 0; i < spsInterest.RowCount; i++)
            {
                Item item = new Item();
                item.OrderPerRate = Common.getDoubleValue(spsInterest.Cells[i, (int)INTER_COL.RATE].Text);
                hItemLogs.Add(spsInterest.Cells[i, (int)INTER_COL.CODE].Text, item);
                hBuyVolume.Add(spsInterest.Cells[i, (int)INTER_COL.CODE].Text, 0);
            }

            if (Program.LoggedIn)
            {
                btnDoLog.Enabled = true;
                btnSelectQuery.Enabled = true;
            }

            // 예수금 설정
            setMoney();
        }

        private void setMoney()
        {
            setDoQuery("계좌조회", "");
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
                    query.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, shCode);
                    query.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, "1");             // 매매수량
                    query.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, "");          // 주문가.. 시장가면 안넣어도 될까? ==> 안넣어도 됨
                    query.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, "1");          // 1:매도, 2:매수
                    query.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03");     // 00:지정가, 03:시장가
                    query.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000");       // 신용거래, 000:보통
                    query.SetFieldData("CSPAT00600InBlock1", "LoanDt", 0, "");              // 대출일? 그냥 ""으로..
                    query.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0");      // 주문조건구분, 0:없음
                    break;
                case "계좌조회":
                    query.SetFieldData("CSPAQ12200InBlock1", "RecCnt", 0, "1");
                    query.SetFieldData("CSPAQ12200InBlock1", "MgmtBrnNo", 0, " ");
                    query.SetFieldData("CSPAQ12200InBlock1", "AcntNo", 0, Program.cont.getAccount);
                    query.SetFieldData("CSPAQ12200InBlock1", "Pwd", 0, Program.cont.getAccountPass);
                    query.SetFieldData("CSPAQ12200InBlock1", "BalCreTp", 0, "0");
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

            lstLog.Add(realCls.BuySign);
            lstLog.Add(realCls.SellSign);
            lstLog.Add(realCls.Order);
            lstLog.Add(realCls.OrderVolume);
            lstLog.Add(realCls.OrderPrice);
            lstLog.Add(realCls.Tax);

            DateTime dt = DateTime.Now;
            string fileName = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/" + LOG_PATH + "/" + realCls.Shcode + "(" + dt.ToShortDateString() + ")." + EXTENSION;
            writeLogFile(fileName, lstLog);
        }

        private void writeLogFile(string fileName, List<string> logData)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                file = new StreamWriter(fileName, true);
                file.WriteLine(getLogFormat(logData));
            }
            catch (Exception e)
            {
                file = new StreamWriter(fileName, true);
                file.WriteLine(e.Message);
            }
            finally
            {
                file.Close();
            }
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

            if (item.IsPurchased && item.HighRate < Common.getDoubleValue(realCls.Drate))
            {
                item.HighRate = Common.getDoubleValue(realCls.Drate);
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

        private string getOrderVolume()
        {

            return "";
        }

        private void realdataProcess(XA_DATASETLib.XAReal query)
        {
            ClsRealChe cls = setRealDataClass(query);
            chkOrderLogic(cls);
            writeLog(cls);
        }

        private void chkOrderLogic(ClsRealChe cls)
        {
            Item item = (Item)hItemLogs[cls.Shcode];
            setItemLog(cls);
            setTimeIndex(item);

            if (!item.IsPurchased)
            {
                string canOrder = chkBuyOrder(item);

                cls.BuySign = canOrder;
                if (chkSimulation)
                {
                    spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.BUY_SIGN].Text = canOrder;
                }
                
                if (!canOrder.Contains("2"))
                {
                    if (!chkCutOffTime(cls))
                    {
                        if (!chkSimulation)
                        {
                            // 실제로 매수 주문 보내는 로직
                            if(chkReal.Checked)
                                buyStock(item, cls);
                        }
                        else
                        {
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER].Text = "구매ㄱㄱ";
                            spsLog.Rows[spsLog.RowCount - 1].BackColor = Color.YellowGreen;
                        }

                        cls.Order = "매수";
                        item.IsPurchased = true;        // 사게 되면..
                        item.PurchasedRate = Common.getDoubleValue(cls.Drate);
                        item.HighRate = Common.getDoubleValue(cls.Drate);
                    }
                }
            }

            if (item.IsPurchased)
            {
                string canSell = chkSellSign(item);

                cls.SellSign = canSell;
                if (chkSimulation)
                {
                    spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.SELL_SIGN].Text = canSell;
                }

                if (!canSell.Contains("2"))
                {
                    if (!chkSimulation)
                    {
                        // 실제로 매도 주문 보내는 로직
                        if (chkReal.Checked)
                            sellAllStock(cls.Shcode, cls);
                    }
                    else
                    {
                        spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER].Text = "판매 ㄱㄱ";
                        spsLog.Rows[spsLog.RowCount - 1].BackColor = Color.LightBlue;
                    }

                    cls.Order = "매도";
                    item.IsPurchased = false;       // 팔게되면.. 싹다 팔고나면..
                    item.PurchasedRate = 0;
                    item.HighRate = 0;
                }
            }
        }

        private int calcBuyVolume(Item item, ClsRealChe cls)
        {
            long price = Common.getLongValue(cls.Price);
            long moneyPerStock = (long)Math.Ceiling(price * (1 + Program.cont.getFee));

            if (moneyPerStock == 0) return 0;
            long availMoney = initMoney * item.OrderPerRate / 100 < money ? (long)Math.Truncate(initMoney * item.OrderPerRate / 100) : money;

            if (moneyPerStock > availMoney)
            {
                // 1주 사는데, 가능한 비용보다 비싼 주식이라면, 전체 금액에서 이 주식을 살수 있는지 체크
                if (moneyPerStock > money)
                {
                    // 살 수 없다면 0
                    return 0;
                }
                else
                {
                    // 살 수 있다면 1주만 산다..
                    return 1;
                }
            }
            else
            {
                double cnt = availMoney / moneyPerStock;
                return (int)cnt;
            }
        }

        private void buyStock(Item item, ClsRealChe cls)
        {
            int volume = calcBuyVolume(item, cls);
            if (volume < 1) return;

            hBuyVolume[cls.Shcode] = volume;
            doOrder(cls.Shcode, "MS", Convert.ToString(volume));

            cls.OrderVolume = Convert.ToString(volume);
            cls.OrderPrice = cls.Price;
            cls.Tax = Convert.ToString(Math.Truncate(Common.getDoubleValue(cls.Price) * Program.cont.getFee));
        }

        private void sellAllStock(string shCode, ClsRealChe cls)
        {
            int volume = (int)hBuyVolume[shCode];
            if (volume < 1) return;
            doOrder(shCode, "MD", Convert.ToString(volume));
            hBuyVolume[shCode] = 0;

            cls.OrderVolume = Convert.ToString(volume);
            cls.OrderPrice = cls.Price;
            cls.Tax = Convert.ToString(Math.Truncate(Common.getDoubleValue(cls.Price) * (Program.cont.getFee + Program.cont.getTax)));
        }

        private void doOrder(string shCode, string kind, string volume) 
        {
            XA_DATASETLib.XAQuery query = getCurrQuery("현물 정상주문");
            query.SetFieldData("CSPAT00600InBlock1", "AcntNo", 0, Program.cont.getAccount);
            query.SetFieldData("CSPAT00600InBlock1", "InptPwd", 0, Program.cont.getAccountPass);
            query.SetFieldData("CSPAT00600InBlock1", "IsuNo", 0, shCode);
            query.SetFieldData("CSPAT00600InBlock1", "OrdQty", 0, volume);             // 매매수량
            query.SetFieldData("CSPAT00600InBlock1", "OrdPrc", 0, "");          // 주문가.. 시장가면 안넣어도 될까? ==> 안넣어도 됨
            query.SetFieldData("CSPAT00600InBlock1", "BnsTpCode", 0, kind == "MD" ? "1" : "2");          // MS:매수, MD:매도, 1:매도, 2:매수
            query.SetFieldData("CSPAT00600InBlock1", "OrdprcPtnCode", 0, "03");     // 00:지정가, 03:시장가
            query.SetFieldData("CSPAT00600InBlock1", "MgntrnCode", 0, "000");       // 신용거래, 000:보통
            query.SetFieldData("CSPAT00600InBlock1", "LoanDt", 0, "");              // 대출일? 그냥 ""으로..
            query.SetFieldData("CSPAT00600InBlock1", "OrdCndiTpCode", 0, "0");      // 주문조건구분, 0:없음

            doQuery(query, false);
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
                case "계좌조회":
                    query.ReceiveData += cspaq12200Query_ReceiveData;
                    query.ReceiveMessage += cspaq12200Query_ReceiveMessage;
                    break;
            }
        }

        #region EventListener
        private void cspaq12200Query_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            Console.WriteLine(bIsSystemError);
            Console.WriteLine(nMessageCode);
            Console.WriteLine(szMessage);
        }

        private void cspaq12200Query_ReceiveData(string szTrCode)
        {
            Console.WriteLine(szTrCode);
            string ableAmt = ((XA_DATASETLib.XAQuery)hKindKeyMap["CSPAQ12200"]).GetFieldData("CSPAQ12200OutBlock2", "MgnRat100pctOrdAbleAmt", 0);     //==> 주문가능 계좌금액 
            Console.WriteLine(ableAmt);
            money = Common.getLongValue(ableAmt);
            if (initMoney == 0)
            {
                initMoney = Common.getLongValue(ableAmt);
            }
        }

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
            setMoney();
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
                            Thread.Sleep(1010);
                            setDoQuery("기간별 주가", spsInterest.Cells[i, (int)INTER_COL.CODE].Text);
                            break;
                        }
                    }
                }
                else
                {
                    XA_DATASETLib.XAQuery query = getCurrQuery("기간별 주가");
                    query.SetFieldData("t1305InBlock", "idx", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "idx", 0));
                    query.SetFieldData("t1305InBlock", "cnt", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "cnt", 0));
                    query.SetFieldData("t1305InBlock", "date", 0, ((XA_DATASETLib.XAQuery)hKindKeyMap["t1305"]).GetFieldData("t1305OutBlock", "date", 0));
                    Thread.Sleep(1010);
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
                
                spsInterest.Cells[i, (int)INTER_COL.AVG_VOL].Text = Math.Round(ttlVolume/(lst.Count - 1), 0).ToString();
                Item item = (Item)hItemLogs[shcode];
                item.AvgVolumeFewDays = Math.Round(ttlVolume / (lst.Count - 1), 0);

                Console.WriteLine(shcode + " Total :" + Math.Round(ttlVolume / (lst.Count - 1), 0).ToString());
                break;
            }

            lstT1305.Clear();
        }
        #endregion

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
            LoadLogData(false);
            //string filename = getDialogFile();
            //if (filename == "") return;

            //StreamReader sr = null;

            //try
            //{
            //    sr = new StreamReader(filename);
            //    string line;
            //    spsLog.RowCount = 0;
            //    string currRate = "";
            //    string beforeRate = "";

            //    Cursor.Current = Cursors.WaitCursor;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        if (line.Split('/').Length > 10)
            //        {
            //            spsLog.RowCount++;
            //            string[] strs = line.Split('/');
            //            List<string> strList = new List<string>();

            //            for (int i = 0; i < strs.Length; i++)
            //            {
            //                strList.Add(strs[i]);
            //            }

            //            string drate = strList[(int)LOG_COL.DRATE];
            //            string gubun = strList[(int)LOG_COL.GUBUN];

            //            setSpread(strList);

            //            if (currRate == "")
            //            {
            //                currRate = drate;
            //            }
            //            else if (!currRate.Equals(drate) && !beforeRate.Equals(drate) && beforeRate != "")
            //            {
            //                beforeRate = currRate;
            //                currRate = drate;
            //                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Color.Red;
            //            }
            //            else if (!currRate.Equals(drate))
            //            {
            //                beforeRate = currRate;
            //                currRate = drate;
            //                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Color.Orange;
            //            }
            //            if (gubun.Equals("+"))
            //                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Color.Orange;
            //            else
            //                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Color.LightBlue;
            //        }
            //    }

            //    Cursor.Current = Cursors.Default;
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    sr.Close();
            //}
        }

        private void btnDoLog_Click(object sender, EventArgs e)
        {
            isRecording = true;
            btnShowLog.Enabled = false;
            btnFindPoint.Enabled = false;
            btnSimulation.Enabled = false;
            getRealLog();
            getAvgVolume();
            MessageBox.Show("recording...");
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
                if (spsLog.Rows[i].BackColor == Color.YellowGreen || spsLog.Rows[i].BackColor == Color.LightBlue)
                {
                    spdLog.SetViewportTopRow(0, i);
                    lblSTS.Text = "Cell(" + i.ToString() + "," + (((int)LOG_COL.DRATE).ToString()) + ")";
                    spsLog.SetActiveCell(i, (int)LOG_COL.CODE, false);
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

        private string chkBuyOrder(Item item)
        {
            // 현재 오른 %가 너무 높으면... 사지 말까? 예를들어 20%라든지... 
            // 3분 5분 이동 평균선상 내려가는 추세라면 사지 말자~?

            ClsRealChe realCls = item.Logs[item.Logs.Count - 1];

            // 1분정도 로그보고 졸라 많이 들어오면 Ok
            // 3분정도 로그보고 꾸준히 들어와도 Ok 일단 위아래 둘중에 어떤게 나은지 모니터링 해보자..
            // 일정기간 매수량이 매도량를 압도
            string msVolumeDueTime = chkMsVolumeDueTime(item);
            // 일정기간 거래량이 일별 평균 거래량의 특정 비율을 넘어서야함
            string overVolume = chkOverVolume(item);
            // 호가를 2개~3개 정도 뚫어주거나 % 기준으로 어느정도 올랐을 경우
            string pierce = pierceUp(item);
            // 체결강도가 너무 낮지 않아야 함
            //string chePower = getChePower(realCls);
            // 체결강도가 0이거나 1000 이상 올라 가는건 최초 동시호가 근처이므로 배제해야 할듯(매도누적체결건수로 해도 될듯.. 순서대로 체결건수만큼 올라감)
            string initTime = chkInitOrder(realCls);
            // 구매신호가 설정된 값 이상 연속으로 왔는가..
            string orderSignCnt = chkOrderSignCnt(item, msVolumeDueTime + overVolume + pierce + initTime);

            return msVolumeDueTime + overVolume + pierce + initTime + orderSignCnt;
        }

        private string chkOrderSignCnt(Item item, string sign)
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
                if(item.OrderSignCnt > 0)
                    item.OrderSignCnt--;
                return "2";
            }
        }

        private string chkMsVolumeDueTime(Item item)
        {
            double msVolume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Msvolume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Msvolume);
            double mdVolume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Mdvolume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Mdvolume);

            if (msVolume == 0) return "2";

            if (mdVolume / msVolume < Program.cont.MsmdRate)
                return "1";
            else
                return "2";
        }

        private string chkOverVolume(Item item)
        {
            double volume = Common.getDoubleValue(item.Logs[item.ToTimeIdx].Volume) - Common.getDoubleValue(item.Logs[item.FromTimeIdx].Volume);

            if (item.AvgVolumeFewDays < 1)
                return "2";
            else if (item.AvgVolumeFewDays * Program.cont.LogTermVolumeOver < volume)
                return "1";
            else
                return "2";
        }

        private void setTimeIndex(Item item)
        {
            item.ToTimeIdx = item.Logs.Count - 1;               // 제일 최근 로그 Index
            if (item.FromTimeIdx < 0 || item.ToTimeIdx < 0)
            {
                item.FromTimeIdx = 0;
            }

            // From Time이랑 toTime 사이가 설정 간격을 넘어 가는가?
            if (chkTimeInterval(item))
            {
                // 넘어갔다.. 다시 설정
                item.FromTimeIdx = getFromIdx(item);
            }
        }

        private int getFromIdx(Item item)
        {
            TimeSpan toTime = getTime(item.Logs[item.ToTimeIdx].Chetime);

            for (int i = item.FromTimeIdx; i < item.Logs.Count; i++)
            {
                TimeSpan fromTime = getTime(item.Logs[i].Chetime);
                TimeSpan gap = toTime - fromTime;
                if (gap.TotalSeconds <= Program.cont.LogTerm)
                    return i;
            }

            return item.FromTimeIdx;
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
            double power = Common.getDoubleValue(cls.Cpower);

            if (Program.cont.PowerLowLimit > power)
                return "2";
            else
                return "1";
        }

        private string pierceUp(Item item)
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

        private string chkSellSign(Item item)
        {
            ClsRealChe realCls = item.Logs[item.Logs.Count - 1];

            double currRate = Common.getDoubleValue(realCls.Drate);

            // 2시 48분 이후에는 무조건 판매
            if (chkCutOffTime(realCls))
            {
                return "시간제한";
            }
            else
            {
                // 구매가격 기준 손절% 또는 최고가 대비 익절% 이하로 내려 가는 경우
                string cutoff = chkCutOff(item.PurchasedRate, currRate, item.HighRate);

                // 판매 신호가 연속 몇회 이상 날아 오는 경우
                string sellSignCnt = chkSellSignCnt(item, cutoff);

                // 28% 이상 넘어가면 팔아버리자..
                string nearByTop = ChkNearByTopPrice(item, "");

                // 거래량을 동반한 매도총량이 어느정도 기준을 넘어서는 경우
                // 체결강도가 어느정도 이상 떨어진다던지..

                return cutoff + sellSignCnt;
            }
        }

        private string ChkNearByTopPrice(Item item, string dd)
        {

        }

        private string chkSellSignCnt(Item item, string sign)
        {
            if (!sign.Contains("2"))
            {
                item.SellSignCnt++;
                if (item.SellSignCnt >= Program.cont.SellSignCnt)
                    return "1";
                else
                    return "2";
            }
            else
            {
                if (item.SellSignCnt > 0)
                    item.SellSignCnt--;
                return "2";
            }
        }

        private bool chkCutOffTime(ClsRealChe cls)
        {
            if (chkSimulation)
            {
                string hour = cls.Chetime.Substring(0, 2);
                string minute = cls.Chetime.Substring(2, 2);

                if (Common.getIntValue(hour) >= 14 && Common.getIntValue(minute) >= 48)
                    return true;
                else
                    return false;
            }
            else
            {
                DateTime time = DateTime.Now;
                if (time.Hour == 14 && time.Minute > 48)
                    return true;
                else
                    return false;
            }
        }

        private string chkCutOff(double purchasedRate, double currRate, double highRate)
        {
            if (purchasedRate - Program.cont.CutoffPercent >= currRate)
                return "1";
            else if (highRate - Program.cont.ProfitCutoffPercent >= currRate)
                return "3";
            else
                return "2";
        }

        private void simulationProcess(List<string> list)
        {
            ClsRealChe cls = setRealDataClass(list);
            Item item = (Item)hItemLogs[cls.Shcode];
            setItemLog(cls);

            chkOrderLogic(cls);

            writeLog(cls);
        }

        private ClsRealChe setRealDataClass(List<string> list)
        {
            ClsRealChe cls = new ClsRealChe();
            cls.Shcode = list[(int)LOG_COL.CODE];
            cls.Chetime = list[(int)LOG_COL.CHETIME];
            cls.Sign = list[(int)LOG_COL.SIGN];
            cls.Change = list[(int)LOG_COL.CHANGE];
            cls.Drate = list[(int)LOG_COL.DRATE];
            cls.Price = list[(int)LOG_COL.PRICE];
            cls.Open = list[(int)LOG_COL.OPEN];
            cls.High = list[(int)LOG_COL.HIGH];
            cls.Low = list[(int)LOG_COL.LOW];
            cls.Cgubun = list[(int)LOG_COL.GUBUN];
            cls.Cvolume = list[(int)LOG_COL.CVOLUME];
            cls.Volume = list[(int)LOG_COL.VOLUME];
            cls.Value = list[(int)LOG_COL.VALUE];
            cls.Mdvolume = list[(int)LOG_COL.MDVOLUME];
            cls.Mdchecnt = list[(int)LOG_COL.MDCHECNT];
            cls.Msvolume = list[(int)LOG_COL.MSVOLUME];
            cls.Mschecnt = list[(int)LOG_COL.MSCHECNT];
            cls.Cpower = list[(int)LOG_COL.CPOWER];
            cls.Offerho = list[(int)LOG_COL.OFFERHO];
            cls.Bidho = list[(int)LOG_COL.BIDHO];
            cls.Status = list[(int)LOG_COL.STATUS];
            cls.Jnilvolume = list[(int)LOG_COL.JNILVOLUME];

            return cls;
        }

        private void setSpread(List<string> strList)
        {
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CODE].Text = strList[(int)LOG_COL.CODE];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CHETIME].Text = strList[(int)LOG_COL.CHETIME];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.SIGN].Text = strList[(int)LOG_COL.SIGN];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CHANGE].Text = strList[(int)LOG_COL.CHANGE];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].Text = strList[(int)LOG_COL.DRATE];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.PRICE].Text = strList[(int)LOG_COL.PRICE];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.OPEN].Text = strList[(int)LOG_COL.OPEN];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.HIGH].Text = strList[(int)LOG_COL.HIGH];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.LOW].Text = strList[(int)LOG_COL.LOW];
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].Text = strList[(int)LOG_COL.GUBUN];
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
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.BUY_SIGN].Text = strList[(int)LOG_COL.BUY_SIGN];
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.SELL_SIGN].Text = strList[(int)LOG_COL.SELL_SIGN];
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER].Text = strList[(int)LOG_COL.ORDER];
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER_VOLUME].Text = strList[(int)LOG_COL.ORDER_VOLUME];
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER_PRICE].Text = strList[(int)LOG_COL.ORDER_PRICE];
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.TAX].Text = strList[(int)LOG_COL.TAX];
            }
        }

        private void LoadLogData(bool isSimulations)
        {
            string filename = getDialogFile();
            if (filename == "") return;

            StreamReader sr = null;
            chkSimulation = isSimulations;

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

                        string drate = strList[(int)LOG_COL.DRATE];
                        string gubun = strList[(int)LOG_COL.GUBUN];

                        setSpread(strList);

                        if (currRate == "")
                        {
                            currRate = drate;
                        }
                        else if (!currRate.Equals(drate) && !beforeRate.Equals(drate) && beforeRate != "")
                        {
                            beforeRate = currRate;
                            currRate = drate;
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Color.Red;
                        }
                        else if (!currRate.Equals(drate))
                        {
                            beforeRate = currRate;
                            currRate = drate;
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Color.Orange;
                        }

                        if (gubun.Equals("+"))
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Color.Orange;
                        else
                            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Color.LightBlue;

                        if (chkSimulation)
                        {
                            if (hItemLogs[strList[(int)LOG_COL.CODE]] == null)
                            {
                                if (spsLog.RowCount == 1)
                                {
                                    MessageBox.Show("선택된 로그의 종목이 종목리스트에 없습니다.");
                                }
                                continue;
                            }

                            Item item = ((Item)hItemLogs[strList[(int)LOG_COL.CODE]]);

                            if (spsLog.RowCount != 1)
                            {
                                // 최초 1줄은 시장 시작전 동시호가 거래량이라서 통계에서 제외
                                simulationProcess(strList);
                            }
                            else
                            {
                                double avgVolFewDays = item.AvgVolumeFewDays;
                                double orderPerRate = item.OrderPerRate;
                                hItemLogs[strList[(int)LOG_COL.CODE]] = new Item();
                                ((Item)hItemLogs[strList[(int)LOG_COL.CODE]]).AvgVolumeFewDays = avgVolFewDays;
                                ((Item)hItemLogs[strList[(int)LOG_COL.CODE]]).OrderPerRate = orderPerRate;
                            }
                        }
                    }
                }

                chkSimulation = false;
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

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            LoadLogData(true);
        }

        private void btnGetAvgVolume_Click(object sender, EventArgs e)
        {
            getAvgVolume();
        }

        private void getAvgVolume()
        {
            if (Program.LoggedIn && spsInterest.RowCount > 0)
            {
                setDoQuery("기간별 주가", spsInterest.Cells[0, (int)INTER_COL.CODE].Text);
            }
        }

        private void spdInterest_EditModeOff(object sender, EventArgs e)
        {
            chkSameCode();
        }

        private void chkSameCode()
        {
            if (spsInterest.RowCount < 1) return;
            if (spsInterest.ActiveRowIndex < 0 || spsInterest.ActiveColumnIndex < 0) return;
            if (spsInterest.ActiveColumnIndex != (int)INTER_COL.CODE) return;

            for (int i = 0; i < spsInterest.RowCount; i++)
            {
                if (i == spsInterest.ActiveRowIndex) continue;

                if (spsInterest.Cells[i, (int)INTER_COL.CODE].Text == spsInterest.Cells[spsInterest.ActiveRowIndex, (int)INTER_COL.CODE].Text)
                {
                    MessageBox.Show(Convert.ToString(i + 1) + "번째 Row에 동일한 종목이 존재합니다.");
                }
            }
        }
    }
}
