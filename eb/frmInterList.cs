﻿using eb.Classes;
using eb.common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private class LogMemoryClass{
            public string shcode { get; set; }
            public List<string> fileNameList { get; set; }
            public ArrayList logData { get; set; }

            public LogMemoryClass()
            {
                shcode = "";
                fileNameList = new List<string>();
                logData = new ArrayList();
            }
        }

        ArrayList logMemory = new ArrayList();

        List<string> kindList = new List<string>();     // Query 종류들..
        Hashtable hQueryKind = new Hashtable();         // Query
        Hashtable hKindKeyMap = new Hashtable();
        Hashtable hContinueMap = new Hashtable();
        Hashtable hItemLogs = new Hashtable();      // 종목별 Class

        private StreamWriter file = null;   // log 수집용 stream
        private bool chkSimulation = false;
        private bool chkLongTermSimulation = false;
        private string currFileName = "";
        private bool isRecording = false;
        private bool assignOnlyOneItem = false;
        
        private List<T1305> lstT1305 = new List<T1305>();
        private List<T8430> lstT8430 = new List<T8430>();

        private double ttlbuy = 0;                  // 예수금 사용%
        private long initMoney = 0;                 // 시작시 예수금
        private long money = 0;                     // 현재 예수금
        Hashtable hBuyVolume = new Hashtable();     // 구매시 몇주 구매 했는지 기록

        private long initMoneyForSimulation;        // Long Term Simulation 시 투입 금액
        private int firstRowIdx = 0;                // Long Term Simulation 시 항목마다 첫 시작 Row
        private int lasttRowIdx = 0;                // Long Term Simulation 시 항목마다 마지막 Row
        private long preDayVolume = 0;              // 시뮬레이션 시에 전일자 거래량

        private Color SUBTOTAL_COLOR = Color.LightGreen;

        XA_DATASETLib.XAQuery cdpcq04700Query;      // 계좌 거래내역
        XA_DATASETLib.XAQuery t1101Query;           // 현재가 호가 조회
        XA_DATASETLib.XAQuery t1102Query;           // 현재가 시세 조회
        XA_DATASETLib.XAQuery t1301Query;           // 시간대별 체결 조회 1초당 2건
        XA_DATASETLib.XAQuery t1302Query;           // 주식분별 주가 조회 1초당 1건
        XA_DATASETLib.XAQuery t1310Query;           // 주식당일전일분틱조회 1초당 2건
        XA_DATASETLib.XAQuery t1305Query;           // 기간별 주가
        XA_DATASETLib.XAQuery t8430Query;           // 주식종목조회
        XA_DATASETLib.XAQuery cspat00600Query;      // 정상주문
        XA_DATASETLib.XAQuery cspat00700Query;      // 정정주문
        XA_DATASETLib.XAQuery cspat00800Query;      // 취소주문
        XA_DATASETLib.XAQuery currQuery;            // 현재 Query를 받아서 처리
        XA_DATASETLib.XAQuery cspaq12200Query;      // 계좌 예수금/주문가능금액 등

        XA_DATASETLib.XAReal queryRealPI;         // 코스피 실시간 시세
        XA_DATASETLib.XAReal queryRealDAK;         // 코스닥 실시간 시세

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

        enum LONG_SIMUL_COL
        {
            DATE,
            TIME,
            MSMD,
            PRICE,
            RATE,
            CPOWER,
            REASON,
            DIFFERENCE_RATE,
            DIFFERENCE_AMOUNT,
            MSMD_VOLUME,
            MSMD_AMOUNT,
            FEE,
            TAX,
            BALANCE,
            PROFIT
        }

        public frmInterList()
        {
            InitializeComponent();
        }

        private void frmInterList_Load(object sender, EventArgs e)
        {
            Common.SetConfig();

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
            kindList.Add("주식종목조회");

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
            hQueryKind.Add("주식종목조회", "t8430");

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
            hContinueMap.Add("주식종목조회", false);

            hKindKeyMap.Add("CDPCQ04700", cdpcq04700Query);
            hKindKeyMap.Add("t1101", t1101Query);
            hKindKeyMap.Add("t1102", t1102Query);
            hKindKeyMap.Add("t1301", t1301Query);
            hKindKeyMap.Add("t1302", t1302Query);
            hKindKeyMap.Add("t1305", t1305Query);
            hKindKeyMap.Add("t8430", t8430Query);
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
                case "주식종목조회":
                    query.SetFieldData("t8430InBlock", "gubun", 0, "0");
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
                queryCls.ResFileName = Program.cont.getResPath + kind + Program.cont.getResTag;
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
            string fileName = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/" + Program.cont.LOG_PATH + "/" + realCls.Shcode + "(" + dt.ToShortDateString() + ")." + Program.cont.EXTENSION;
            writeLogFile(fileName, lstLog);
        }

        private void writeLogFile(string fileName, List<string> logData)
        {
            try
            {
                //FileInfo fileInfo = new FileInfo(fileName);
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

        private void BuyProcess(Item item, ClsRealChe cls)
        {
            if (item.IsPurchased) return;

            ChkBuyOrder chkBuy = new ChkBuyOrder(item);
            string canOrder = chkBuy.ChkBuySign();
            cls.BuySign = canOrder;

            if (chkSimulation && !chkLongTermSimulation)
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.BUY_SIGN].Text = canOrder;

            // 2는 구매 요건에 부합되지 않는 경우가 있을 경우 포함됨
            if (canOrder.Contains("2")) return;

            // 장 마감 근처인 경우 안삼..
            if (Common.chkCutOffTime(cls, chkSimulation)) return;
            
            if (!chkSimulation)
            {
                // 실제로 매수 주문 보내는 로직
                if (chkReal.Checked)
                    buyStock(item, cls);
            }
            else
            {
                if (chkLongTermSimulation)
                {
                    spsLongTermSimulation.RowCount++;
                    WriteSimulationData(item, cls, true);
                }
                else
                {
                    spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER].Text = "매수ㄱㄱ";
                    spsLog.Rows[spsLog.RowCount - 1].BackColor = Program.cont.BuyCell;
                }
            }

            cls.Order = "매수";
            item.IsPurchased = true;        // 사게 되면..
            item.PurchasedRate = Common.getDoubleValue(cls.Drate);
            item.HighRate = Common.getDoubleValue(cls.Drate);
        }

        private void WriteSimulationData(Item item, ClsRealChe cls, bool isBuy)
        {
            spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DATE].Text = currFileName;
            spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TIME].Text = cls.Chetime;
            spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.PRICE].Text = cls.Price;
            spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.RATE].Text = cls.Drate;
            spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.CPOWER].Text = cls.Cpower;
            spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD].Text = isBuy == true ? "매수" : "매도";

            long price = Convert.ToInt64(cls.Price);

            if (isBuy)
            {
                int msVolume = Common.SimpleChkCanBuyVolume(initMoneyForSimulation, price);
                long msAmount = price * msVolume;
                long fee = Common.GetFee(price * msVolume);
                initMoneyForSimulation = initMoneyForSimulation - (msAmount + fee);

                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD_VOLUME].Text = Convert.ToString(msVolume);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD_AMOUNT].Text = Convert.ToString(msAmount);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.FEE].Text = Convert.ToString(fee);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.BALANCE].Text = Convert.ToString(initMoneyForSimulation);

                item.MsVolume = msVolume;
                item.Price = Convert.ToInt64(cls.Price);
            }
            else
            {
                string beforeRate = spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 2, (int)LONG_SIMUL_COL.RATE].Text;
                string beforeAmount = spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 2, (int)LONG_SIMUL_COL.PRICE].Text;

                long mdAmount = price * item.MsVolume;
                long fee = Common.GetFee(price * item.MsVolume);
                long tax = Common.GetTax(price * item.MsVolume);
                initMoneyForSimulation = initMoneyForSimulation + mdAmount - fee - tax;

                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DIFFERENCE_RATE].Text = Convert.ToString(Convert.ToDouble(cls.Drate) - Convert.ToDouble(beforeRate));
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DIFFERENCE_AMOUNT].Text = Convert.ToString(Convert.ToInt32(cls.Price) - Convert.ToInt32(beforeAmount));

                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.REASON].Text = cls.SellSign;
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD_VOLUME].Text = Convert.ToString(item.MsVolume);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD_AMOUNT].Text = Convert.ToString(mdAmount);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TAX].Text = Convert.ToString(tax);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.FEE].Text = Convert.ToString(fee);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.BALANCE].Text = Convert.ToString(initMoneyForSimulation);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.PROFIT].Text = Convert.ToString(Math.Round((double)initMoneyForSimulation / Common.getDoubleValue(txtInputMoney.Text), 3) * 100) + "%";

                item.MsVolume = 0;      // 다 팔았으니 현재 매수된 양을 0으로 설정
                item.Price = 0;         // 다 팔았으니 단가도 0원으로 설정
            }
        }

        private void SellProcess(Item item, ClsRealChe cls)
        {
            if (!item.IsPurchased) return;

            ChkSellOrder chkSell = new ChkSellOrder(item);
            string canSell = chkSell.ChkSellSign(chkSimulation);
            cls.SellSign = canSell;

            if (chkSimulation && !chkLongTermSimulation)
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.SELL_SIGN].Text = canSell;

            // 2는 매도 요건에 부합되지 않는 경우가 있을 경우 포함됨
            if (canSell.Contains("2")) return;
            
            if (!chkSimulation)
            {
                // 실제로 매도 주문 보내는 로직
                if (chkReal.Checked)
                    sellAllStock(cls.Shcode, cls);
            }
            else
            {
                if (chkLongTermSimulation)
                {
                    spsLongTermSimulation.RowCount++;
                    WriteSimulationData(item, cls, false);
                }
                else
                {
                    spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER].Text = "매도 ㄱㄱ";
                    spsLog.Rows[spsLog.RowCount - 1].BackColor = Program.cont.SellCell;
                }
            }

            cls.Order = "매도";
            item.IsPurchased = false;       // 팔게되면.. 싹다 팔고나면..
            item.PurchasedRate = 0;
            item.HighRate = 0;
        }

        private string CalcProfit()
        {
            long amount = 0;

            for (int i = 0; i < spsLongTermSimulation.RowCount; i++)
            {
                if (spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.DIFFERENCE_AMOUNT].Text.Trim() == "") continue;

                amount += Convert.ToInt32(spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.DIFFERENCE_AMOUNT].Text);
            }

            return Convert.ToString(amount);
        }

        private void chkOrderLogic(ClsRealChe cls)
        {
            Item item = (Item)hItemLogs[cls.Shcode];
            setItemLog(cls);
            setTimeIndex(item);
            BuyProcess(item, cls);
            SellProcess(item, cls);
        }

        private int calcBuyVolume(Item item, ClsRealChe cls)
        {
            long availMoney = initMoney * item.OrderPerRate / 100 < money ? (long)Math.Truncate(initMoney * item.OrderPerRate / 100) : money;

            return Common.ChkCanBuyVolume(money, availMoney, Common.getLongValue(cls.Price), true);
        }

        private void buyStock(Item item, ClsRealChe cls)
        {
            int volume = calcBuyVolume(item, cls);
            if (volume < 1) return;

            hBuyVolume[cls.Shcode] = volume;
            doOrder(cls.Shcode, "MS", Convert.ToString(volume));

            cls.OrderVolume = Convert.ToString(volume);
            cls.OrderPrice = cls.Price;
            cls.Fee = Convert.ToString(Common.GetFee(Convert.ToInt64(cls.Price)));
        }

        private void sellAllStock(string shCode, ClsRealChe cls)
        {
            int volume = (int)hBuyVolume[shCode];
            if (volume < 1) return;
            doOrder(shCode, "MD", Convert.ToString(volume));
            hBuyVolume[shCode] = 0;

            cls.OrderVolume = Convert.ToString(volume);
            cls.OrderPrice = cls.Price;
            cls.Fee = Convert.ToString(Common.GetFee(Convert.ToInt64(cls.Price)));
            cls.Tax = Convert.ToString(Common.GetTax(Convert.ToInt64(cls.Price)));
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
                case "주식종목조회":
                    query.ReceiveData += t8430Query_ReceiveData;
                    query.ReceiveMessage += t8430Query_ReceiveMessage;
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

                    // 하나의 항목만 평균 값 가져오면, 다른건 조회 안하도록 return..!!
                    if (assignOnlyOneItem) return;

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

        private void t8430Query_ReceiveMessage(bool bIsSystemError, string nMessageCode, string szMessage)
        {
            Console.WriteLine(bIsSystemError);
            Console.WriteLine(nMessageCode);
            Console.WriteLine(szMessage);
        }

        private void t8430Query_ReceiveData(string szTrCode)
        {
            Console.WriteLine(szTrCode);
            T8430 cls = new T8430();
            int cnt = 5000;

            try
            {
                for (int i = 0; i < cnt; i++)
                {
                    string shcode = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "shcode", i);
                    string etfgubun = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "etfgubun", i);

                    if (shcode.Length < 1) break;
                    if (etfgubun == "1") continue;      // ETF는 뭐... 펀드 같은계좌 같네??
                    cls.shcode = shcode;
                    cls.etfgubun = etfgubun;
                    cls.hname = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "hname", i);
                    cls.expcode = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "expcode", i);
                    cls.uplmtprice = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "uplmtprice", i);
                    cls.dnlmtprice = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "dnlmtprice", i);
                    cls.jnilclose = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "jnilclose", i);
                    cls.memedan = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "memedan", i);
                    cls.recprice = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "recprice", i);
                    cls.gubun = ((XA_DATASETLib.XAQuery)hKindKeyMap["t8430"]).GetFieldData("t8430OutBlock", "gubun", i);
                    lstT8430.Add(cls);

                    spsInterest.RowCount++;
                    spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.GUBUN].Text = cls.gubun == "1" ? "P" : "Q";
                    spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.CODE].Text = cls.shcode;
                    spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.NAME].Text = cls.hname;
                    spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.USE].Text = "Y";
                    spsInterest.Cells[spsInterest.RowCount - 1, (int)INTER_COL.RATE].Text = "20";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("더는 없구먼..");
            }
        }

        private void setAvgVolume(List<T1305> lst)
        {
            if (lst.Count == 0) return;

            double ttlVolume = 0;

            // 주식 거래가 있는 날 아침에 하면, 당일 거래량이 0으로 나와서 이전날 기준으로 평균 값을 매긴다.
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
                item.AvgVolumeFewDays = (long)Math.Round(ttlVolume / (lst.Count - 1), 0);

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

        private void btnShowLog_Click(object sender, EventArgs e)
        {
            spdLog.Visible = true;
            spdLongTermSimulation.Visible = false;
            LoadLogData(false);
        }

        private void btnDoLog_Click(object sender, EventArgs e)
        {
            DoRecording();
        }

        private void DoRecording()
        {
            isRecording = true;
            btnShowLog.Enabled = false;
            btnFindPoint.Enabled = false;
            btnSimulation.Enabled = false;
            getRealLog();
            if (chkAvgVolume.Checked)
            {
                getAvgVolume();
            }
            if(!chkAutoRecording.Checked)
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
            //if (spsInterest.ActiveRowIndex < 0) return;

            string shcode = "";
            if(spsInterest.ActiveRowIndex > -1)
            {
                shcode = spsInterest.Cells[spsInterest.ActiveRowIndex, (int)INTER_COL.CODE].Text;
            }
                
            setDoQuery(cmbQueryKind.Text, shcode);
        }

        private void setDoQuery(string kind, string shcode)
        {
            XA_DATASETLib.XAQuery query = getCurrQuery(kind);
            setQueryFieldData(kind, query, shcode);
            doQuery(query, false);
            //doQuery(query, true);
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
                if (spsLog.Rows[i].BackColor == Program.cont.BuyCell || spsLog.Rows[i].BackColor == Program.cont.SellCell)
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

        private void simulationProcess(ClsRealChe list)
        {
            chkOrderLogic(list);
        }

        private void setSpread(ClsRealChe strList)
        {
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CODE].Text = strList.Shcode;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CHETIME].Text = strList.Chetime;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.SIGN].Text = strList.Sign;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CHANGE].Text = strList.Change;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].Text = strList.Drate;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.PRICE].Text = strList.Price;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.OPEN].Text = strList.Open;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.HIGH].Text = strList.High;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.LOW].Text = strList.Low;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].Text = strList.Cgubun;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CVOLUME].Text = strList.Cvolume;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.VOLUME].Text = strList.Volume;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.VALUE].Text = strList.Value;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MDVOLUME].Text = strList.Mdvolume;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MDCHECNT].Text = strList.Mdchecnt;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MSVOLUME].Text = strList.Msvolume;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.MSCHECNT].Text = strList.Mschecnt;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.CPOWER].Text = strList.Cpower;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.OFFERHO].Text = strList.Offerho;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.BIDHO].Text = strList.Bidho;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.STATUS].Text = strList.Status;
            spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.JNILVOLUME].Text = strList.Jnilvolume;

            if (strList.cnt > 22)
            {
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.BUY_SIGN].Text = strList.BuySign;
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.SELL_SIGN].Text = strList.SellSign;
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER].Text = strList.Order;
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER_VOLUME].Text = strList.OrderVolume;
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.ORDER_PRICE].Text = strList.OrderPrice;
                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.TAX].Text = strList.Tax;
            }
        }

        private void LoadLogData(bool isSimulations)
        {
            string filename = Common.getDialogFile();
            List<ClsRealChe> lstData = LoadLogData(filename);
            simulateLogData(isSimulations, lstData);
        }

        private ClsRealChe SetClsRealChe(string[] strs)
        {
            ClsRealChe data = new ClsRealChe();
            data.cnt = strs.Length;

            for (int i = 0; i < strs.Length; i++)
            {
                //strList.Add(strs[i]);
                switch (i)
                {
                    case (int)LOG_COL.CODE: data.Shcode = strs[i]; break;
                    case (int)LOG_COL.CHETIME: data.Chetime = strs[i]; break;
                    case (int)LOG_COL.SIGN: data.Sign = strs[i]; break;
                    case (int)LOG_COL.CHANGE: data.Change = strs[i]; break;
                    case (int)LOG_COL.DRATE: data.Drate = strs[i]; break;
                    case (int)LOG_COL.PRICE: data.Price = strs[i]; break;
                    case (int)LOG_COL.OPEN: data.Open = strs[i]; break;
                    case (int)LOG_COL.HIGH: data.High = strs[i]; break;
                    case (int)LOG_COL.LOW: data.Low = strs[i]; break;
                    case (int)LOG_COL.GUBUN: data.Cgubun = strs[i]; break;
                    case (int)LOG_COL.CVOLUME: data.Cvolume = strs[i]; break;
                    case (int)LOG_COL.VOLUME: data.Volume = strs[i]; break;
                    case (int)LOG_COL.VALUE: data.Value = strs[i]; break;
                    case (int)LOG_COL.MDVOLUME: data.Mdvolume = strs[i]; break;
                    case (int)LOG_COL.MDCHECNT: data.Mdchecnt = strs[i]; break;
                    case (int)LOG_COL.MSVOLUME: data.Msvolume = strs[i]; break;
                    case (int)LOG_COL.MSCHECNT: data.Mschecnt = strs[i]; break;
                    case (int)LOG_COL.CPOWER: data.Cpower = strs[i]; break;
                    case (int)LOG_COL.OFFERHO: data.Offerho = strs[i]; break;
                    case (int)LOG_COL.BIDHO: data.Bidho = strs[i]; break;
                    case (int)LOG_COL.STATUS: data.Status = strs[i]; break;
                    case (int)LOG_COL.JNILVOLUME: data.Jnilvolume = strs[i]; break;
                    case (int)LOG_COL.BUY_SIGN: data.BuySign = strs[i]; break;
                    case (int)LOG_COL.SELL_SIGN: data.SellSign = strs[i]; break;
                    case (int)LOG_COL.ORDER: data.Order = strs[i]; break;
                    case (int)LOG_COL.ORDER_VOLUME: data.OrderVolume = strs[i]; break;
                    case (int)LOG_COL.ORDER_PRICE: data.OrderPrice = strs[i]; break;
                    case (int)LOG_COL.TAX: data.Tax = strs[i]; break;
                }
            }

            return data;
        }

        // 하나의 파일 데이터를 List<ClsRealChe> 형태로 옮겨담는다.
        private List<ClsRealChe> LoadLogData(string filename)
        {
            if (filename == "") return new List<ClsRealChe>();

            List<ClsRealChe> lstData = new List<ClsRealChe>();

            StreamReader sr = null;

            try
            {
                currFileName = filename;

                sr = new StreamReader(filename);
                string line;
                
                Cursor.Current = Cursors.WaitCursor;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split('/').Length > 10)
                    {
                        string[] strs = line.Split('/');
                        lstData.Add(SetClsRealChe(strs));
                    }
                }

                return lstData;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                sr.Close();
            }

            return lstData;
        }

        // 파일데이터 로드된 List를 이용하여 실제 시뮬레이션
        private void simulateLogData(bool isSimulations, List<ClsRealChe> lstData)
        {
            chkSimulation = isSimulations;

            try
            {
                spsLog.RowCount = 0;
                bool isFirst = true;
                string shcode = "";

                Cursor.Current = Cursors.WaitCursor;
                foreach (ClsRealChe data in lstData)
                {
                    shcode = data.Shcode;
                    doSimulation(data.Shcode, data, isFirst);

                    isFirst = false;
                }

                int logLength = ((Item)hItemLogs[shcode]).Logs.Count;
                preDayVolume = Convert.ToInt32(((ClsRealChe)((Item)hItemLogs[shcode]).Logs[logLength - 1]).Volume);     // 이전날 거래량

                chkSimulation = false;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {

            }
        }

        private void doSimulation(string shcode, ClsRealChe strList, bool isFirst)
        {
            string currRate = "";
            string beforeRate = "";

            string drate = strList.Drate;

            if (!chkLongTermSimulation)
            {
                spsLog.RowCount++;

                setSpread(strList);

                if (currRate == "")
                {
                    currRate = drate;
                }
                else if (!currRate.Equals(drate) && !beforeRate.Equals(drate) && beforeRate != "")
                {
                    beforeRate = currRate;
                    currRate = drate;
                    spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Program.cont.ChangeRateOver2;
                }
                else if (!currRate.Equals(drate))
                {
                    beforeRate = currRate;
                    currRate = drate;
                    spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.DRATE].BackColor = Program.cont.ChangeRate;
                }

                if (strList.Cgubun.Equals("+"))
                    spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Program.cont.MsSign;
                else
                    spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Program.cont.MdSign;
            }

            if (chkSimulation)
            {
                if (hItemLogs[strList.Shcode] == null)
                {
                    if (spsLog.RowCount == 1)
                    {
                        MessageBox.Show("선택된 로그의 종목이 종목리스트에 없습니다.");
                    }
                    //continue;
                    return;
                }

                Item item = ((Item)hItemLogs[strList.Shcode]);

                if (!isFirst)
                {
                    // 최초 1줄은 시장 시작전 동시호가 거래량이라서 통계에서 제외
                    simulationProcess(strList);
                }
                else
                {
                    double avgVolFewDays = item.AvgVolumeFewDays;
                    double orderPerRate = item.OrderPerRate;
                    hItemLogs[strList.Shcode] = new Item();         // 최초기 때문에 초기화...
                    //((Item)hItemLogs[strList[(int)LOG_COL.CODE]]).AvgVolumeFewDays = avgVolFewDays;
                    ((Item)hItemLogs[strList.Shcode]).AvgVolumeFewDays = preDayVolume;
                    ((Item)hItemLogs[strList.Shcode]).OrderPerRate = orderPerRate;
                }
            }
        }

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            if (spsInterest.ActiveRowIndex < 0) return;
            spdLog.Visible = true;
            spdLongTermSimulation.Visible = false;
            LoadLogData(true);
        }

        private void btnGetAvgVolume_Click(object sender, EventArgs e)
        {
            if (spsInterest.ActiveRowIndex > -1)
            {
                DialogResult result = MessageBox.Show("선택된 관심종목만 평균거래량을 가져 올까요?\r\n(No:All, Cancel:Cancel)", "Question", MessageBoxButtons.YesNoCancel);
                if (DialogResult.Yes == result)
                {
                    assignOnlyOneItem = true;
                    setDoQuery("기간별 주가", spsInterest.Cells[spsInterest.ActiveRowIndex, (int)INTER_COL.CODE].Text);
                }
                else if (DialogResult.No == result)
                {
                    assignOnlyOneItem = false;
                    getAvgVolume();
                }
            }
        }

        private void getAvgVolume()
        {
            if (Program.LoggedIn && spsInterest.RowCount > 0)
            {
                setDoQuery("기간별 주가", spsInterest.Cells[0, (int)INTER_COL.CODE].Text);
                //setDoQuery("기간별 주가", "097520");
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

        // 선택된 폴더에 선택된 코드의 모든 파일 가져오기
        private List<string> GetSelectedFileList(string path, string code)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles();
            List<string> fileList = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Name.Contains(code)) continue;

                fileList.Add(files[i].FullName);
            }

            fileList.Sort();

            return fileList;
        }

        private void OneItemLongTermSimulation(string folder, string shcode)
        {
            initMoneyForSimulation = Common.getLongValue(txtInputMoney.Text);
            firstRowIdx = spsLongTermSimulation.RowCount;
            preDayVolume = 0;       // 최초 파일의 전날은 데이터가 없으므로 거래량은 0으로 설정

            // memory로 도는 경우에는 메모리에 저장된 값들을 가져오고 아닌경우 파일을 그때그때 읽어서 처리
            if (chkSimulateMemory.Checked)
            {
                for (int i = 0; i < logMemory.Count; i++)
                {
                    LogMemoryClass cls = (LogMemoryClass)logMemory[i];
                    if (shcode != cls.shcode) continue;

                    for (int j = 0; j < cls.fileNameList.Count; j++)
                    {
                        currFileName = cls.fileNameList[j];
                        Common.SetCutOffTime(cls.fileNameList[j]);
                        simulateLogData(true, (List<ClsRealChe>)cls.logData[j]);
                    }

                    break;
                }
            }
            else
            {
                List<string> fileList = GetSelectedFileList(folder, shcode);

                for (int i = 0; i < fileList.Count; i++)
                {
                    Common.SetCutOffTime(fileList[i]);
                    List<ClsRealChe> dataList = LoadLogData(fileList[i]);
                    simulateLogData(true, dataList);
                }
            }

            hItemLogs[shcode] = new Item();                 // 완료된 항목은 다시 초기화 해서 메모리 해제 시키자
            lasttRowIdx = spsLongTermSimulation.RowCount;
        }

        private string GetPercentbyInitMoney(string val)
        {
            return Convert.ToString(Math.Round((double)(Convert.ToInt32(val)/Convert.ToDouble(txtInputMoney.Text)*100), 2));
        }

        // Spread 특정 칼럼 특정 범위만큼 Summary
        private string GetColumnSummary(int colIdx)
        {
            long sum = 0;

            for (int i = firstRowIdx; i < lasttRowIdx; i++)
            {
                try
                {
                    sum += Convert.ToUInt32(spsLongTermSimulation.Cells[i, colIdx].Text);
                }
                catch (Exception e)
                {

                }
            }

            return Convert.ToString(sum);
        }

        private string GetItemName(string code)
        {
            for (int i = 0; i < spsInterest.RowCount; i++)
                if (spsInterest.Cells[i, (int)INTER_COL.CODE].Text == code)
                    return spsInterest.Cells[i, (int)INTER_COL.NAME].Text;

            return "코드 없음";
        }

        // 모든 종목, 모든 로그를 메모리에 다 담는 함수
        private void SetAllItemLoadMemory(string folderPath)
        {
            for (int i = 0; i < spsInterest.RowCount; i++)
            {
                logMemory.Add(SetItemLoadMemory(folderPath, spsInterest.Cells[i, (int)INTER_COL.CODE].Text));
            }
        }

        // 한종목, 모든 로그를 메모리에 다 담는 함수
        private LogMemoryClass SetItemLoadMemory(string folderPath, string shcode)
        {
            LogMemoryClass fileLog = new LogMemoryClass();
            fileLog.shcode = shcode;

            List<string> fileList = GetSelectedFileList(folderPath, shcode);

            for (int fileIdx = 0; fileIdx < fileList.Count; fileIdx++)
            {
                List<ClsRealChe> dataList = LoadLogData(fileList[fileIdx]);
                // LogMemoryClass 클래스에 filename과, logdata를 같은 index로 담는다.
                fileLog.fileNameList.Add(fileList[fileIdx]);
                fileLog.logData.Add(dataList);
            }
            
            return fileLog;
        }

        private void btnLongTermSimulation_Click(object sender, EventArgs e)
        {
            if (spsInterest.ActiveRowIndex < 0) return;
            chkLongTermSimulation = true;
            chkSimulation = true;
            spdLog.Visible = false;
            spdLongTermSimulation.Visible = true;
            spsLongTermSimulation.RowCount = 0;

            //ResetItemTable();

            string folderPath = Common.GetDialogFolder();

            // 프로세스 실행시간 측정
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (chkSimulateAllItem.Checked) 
            {
                // Memory로 시뮬레이션 하려는 경우, 
                // 모든 로그파일의 데이터를 logMemory에 LogMemoryClass형식으로 다 담는다.
                if (chkSimulateMemory.Checked)
                {
                    SetAllItemLoadMemory(folderPath);
                }

                if (chkSimulationOption.Checked)
                    SimulateOptionLongTerm(folderPath);
                else
                    SimulateLongTerm(folderPath);
            }
            else
            {
                string code = spsInterest.Cells[spsInterest.ActiveRowIndex, (int)INTER_COL.CODE].Text;
                OneItemLongTermSimulation(folderPath, code);
                WriteSubTotalSummary(spsInterest.Cells[spsInterest.ActiveRowIndex, (int)INTER_COL.CODE].Text);
            }

            chkLongTermSimulation = false;
            if (chkExportExcel.Checked)
            {
                ExportExcel("");
            }

            // 프로세스 실행시간 종료
            sw.Stop();
            //MessageBox.Show(((sw.ElapsedMilliseconds)/1000).ToString() + "ms");
        }

        private void SimulateLongTerm(string folderPath)
        {
            spsLongTermSimulation.RowCount = 0;

            for (int i = 0; i < spsInterest.RowCount; i++)
            {
                string shcode = spsInterest.Cells[i, (int)INTER_COL.CODE].Text;

                OneItemLongTermSimulation(folderPath, shcode);
                Application.DoEvents();
                WriteSubTotalSummary(shcode);
            }

            WriteTotalResult();
        }

        private void SimulateOptionLongTerm(string folderPath)
        {
            int logFrom = Common.getIntValue(txtLogSimulFrom.Text);
            int logTo = Common.getIntValue(txtLogSimulTo.Text);
            int logInter = Common.getIntValue(txtLogSimulInter.Text);
            double avgFrom = Common.getDoubleValue(txtAvgSimulFrom.Text);
            double avgTo = Common.getDoubleValue(txtAvgSimulTo.Text);
            double avgInter = Common.getDoubleValue(txtAvgSimulInter.Text);
            double rateFrom = Common.getDoubleValue(txtRateSimulFrom.Text);
            double rateTo = Common.getDoubleValue(txtRateSimulTo.Text);
            double rateInter = Common.getDoubleValue(txtRateSimulInter.Text);
            double cutFrom = Common.getDoubleValue(txtCutSimulFrom.Text);
            double cutTo = Common.getDoubleValue(txtCutSimulTo.Text);
            double cutInter = Common.getDoubleValue(txtCutSimulInter.Text);
            double proCutFrom = Common.getDoubleValue(txtProCutSimulFrom.Text);
            double proCutTo = Common.getDoubleValue(txtProCutSimulTo.Text);
            double proCutInter = Common.getDoubleValue(txtProCutSimulInter.Text);
            double profitFrom = Common.getDoubleValue(txtProfitSimulFrom.Text);
            double profitTo = Common.getDoubleValue(txtProfitSimulTo.Text);
            double profitInter = Common.getDoubleValue(txtProfitSimulInter.Text);

            string filename = Program.cont.getApplicationPath + Program.cont.getSimulationPath + DateTime.Now.ToShortDateString().Replace("-", "") + Convert.ToString(DateTime.Now.Hour).PadLeft(2, '0') + Convert.ToString(DateTime.Now.Minute).PadLeft(2, '0') + Convert.ToString(DateTime.Now.Second).PadLeft(2, '0') + ".txt";
            string filenameSummary = Program.cont.getApplicationPath + Program.cont.getSimulationPath + DateTime.Now.ToShortDateString().Replace("-", "") + Convert.ToString(DateTime.Now.Hour).PadLeft(2, '0') + Convert.ToString(DateTime.Now.Minute).PadLeft(2, '0') + Convert.ToString(DateTime.Now.Second).PadLeft(2, '0') + "_summary.txt";
            StreamWriter sw = null;

            // 로그 Interval(60초부터..)
            for (int log = logFrom; log < logTo; log += logInter)
            {
                Program.cont.LogTerm = log;
                txtLogSimulCurr.Text = Convert.ToString(Program.cont.LogTerm);

                // 전일 기준 거래량이 몇% 될때..
                for (double avg = avgFrom; avg < avgTo; avg += avgInter)
                {
                    Program.cont.LogTermVolumeOver = avg;
                    txtAvgSimulCurr.Text = Convert.ToString(Program.cont.LogTermVolumeOver);

                    // 매수/매도 비율, 매수가 매도보다 얼마나 많은가..
                    for (double rate = rateFrom; rate < rateTo; rate += rateInter)
                    {
                        Program.cont.MsmdRate = rate;
                        txtRateSimulCurr.Text = Convert.ToString(Program.cont.MsmdRate);

                        // 손절기준
                        for (double cut = cutFrom; cut < cutTo; cut += cutInter)
                        {
                            Program.cont.CutoffPercent = cut;
                            txtCutSimulCurr.Text = Convert.ToString(Program.cont.CutoffPercent);

                            // 익절기준
                            for (double proCut = proCutFrom; proCut < proCutTo; proCut += proCutInter)
                            {
                                Program.cont.ProfitCutoffPercent = proCut;
                                txtProCutSimulCurr.Text = Convert.ToString(Program.cont.ProfitCutoffPercent);

                                // 몇%일때 무조건 수익 내도록 하는 로직
                                for (double profit = profitFrom; profit < profitTo; profit += profitInter)
                                {
                                    Program.cont.SatisfyProfit = profit;
                                    txtProfitSimulCurr.Text = Convert.ToString(Program.cont.SatisfyProfit);
                                    SimulateLongTerm(folderPath);
                                    string condition = "" + log + "_" + avg + "_" + rate + "_" + cut + "_" + proCut + "_" + profit;
                                    if (chkExportExcel.Checked)
                                    {
                                        ExportExcel(condition);
                                    }
                                    string ttlSummaryStr = ("" + log + " " + avg + " " + rate + " " + cut + " " + proCut + " " + profit + " " + spsLongTermSimulation.Cells[2, (int)LONG_SIMUL_COL.BALANCE].Text + " " + spsLongTermSimulation.Cells[2, (int)LONG_SIMUL_COL.TAX].Text + " " + spsLongTermSimulation.Cells[2, (int)LONG_SIMUL_COL.FEE].Text).Replace("%", "");
                                    WriteSummaryList(filenameSummary, ttlSummaryStr);
                                    try
                                    {
                                        sw = new StreamWriter(filename, true);
                                        sw.WriteLine(ttlSummaryStr);
                                        //sw.WriteLine("" + log + " " + avg + " " + rate + " " + cut + " " + proCut + " " + profit);
                                    }
                                    catch (Exception e)
                                    {
                                        sw = new StreamWriter(filename, true);
                                        sw.WriteLine("" + log + " " + avg + " " + rate + " " + cut + " " + proCut + " " + profit + " " + "로그 기록 실패");
                                    }
                                    finally
                                    {
                                        sw.Close();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // 종목별 거래 완료된 결과만 따로 저장
        private void WriteSummaryList(string filename, string summaryData)
        {
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter(filename, true);
                sw.WriteLine(summaryData);

                for (int i = 0; i < spsLongTermSimulation.RowCount; i++)
                {
                    if (spsLongTermSimulation.Rows[i].BackColor != SUBTOTAL_COLOR) continue;

                    string str = "";
                    str += spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.DATE].Text + " ";
                    str += spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.TIME].Text + " ";
                    str += spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.MSMD_VOLUME].Text + " ";
                    str += spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.MSMD_AMOUNT].Text + " ";
                    str += spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.BALANCE].Text + " ";
                    str += spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.PROFIT].Text;
                    str = str.Replace("%", "");
                    
                    sw.WriteLine(str);
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                sw.Close();
            }
        }

        private void WriteSubTotalSummary(string code)
        {
            spsLongTermSimulation.RowCount++;
            spsLongTermSimulation.Rows[spsLongTermSimulation.RowCount - 1].BackColor = SUBTOTAL_COLOR;

            if (firstRowIdx == lasttRowIdx)
            {
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DATE].Text = GetItemName(code);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TIME].Text = "거래 내역 없음";
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.FEE].Text = "0";
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TAX].Text = "0";
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.BALANCE].Text = txtInputMoney.Text;
            }
            else
            {
                string ttlFee = GetColumnSummary((int)LONG_SIMUL_COL.FEE);
                string ttlTax = GetColumnSummary((int)LONG_SIMUL_COL.TAX);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DATE].Text = GetItemName(code);
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TIME].Text = code;
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.FEE].Text = ttlFee;
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD_VOLUME].Text = GetPercentbyInitMoney(ttlFee) + "%";
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TAX].Text = ttlTax;
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD_AMOUNT].Text = GetPercentbyInitMoney(ttlTax) + "%";
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.BALANCE].Text = spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 2, (int)LONG_SIMUL_COL.BALANCE].Text;
                spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.PROFIT].Text = spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 2, (int)LONG_SIMUL_COL.PROFIT].Text;
            }
        }

        private void WriteTotalResult()
        {
            long ttlCnt = 0;
            double ttlFeeSum = 0;
            double ttlTaxSum = 0;
            double ttlBalanceSum = 0;

            spsLongTermSimulation.Rows.Add(0, 1);   // 첫번째 줄에 전체 결과 추가
            spsLongTermSimulation.Rows[0].BackColor = Color.Orange;

            // 시뮬레이션 종료 후 전체 종목에 대한 전체 합계..
            for (int i = 0; i < spsLongTermSimulation.RowCount; i++)
            {
                if (spsLongTermSimulation.Rows[i].BackColor != SUBTOTAL_COLOR) continue;

                ttlCnt++;
                ttlFeeSum += Convert.ToInt32(spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.FEE].Text);
                ttlTaxSum += Convert.ToInt32(spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.TAX].Text);
                ttlBalanceSum += Convert.ToInt32(spsLongTermSimulation.Cells[i, (int)LONG_SIMUL_COL.BALANCE].Text);
            }

            double ttlInputSum = Convert.ToInt32(txtInputMoney.Text) * ttlCnt;

            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.DATE].Text = "총 " + ttlCnt + "개의 종목 전체 결과";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.FEE].Text = Convert.ToString(Math.Round(ttlFeeSum / ttlInputSum * 100, 2)) + "%";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.TAX].Text = Convert.ToString(Math.Round(ttlTaxSum / ttlInputSum * 100, 2)) + "%";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.BALANCE].Text = Convert.ToString(Math.Round(ttlBalanceSum / ttlInputSum * 100, 2)) + "%";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.PROFIT].Text = "";

            spsLongTermSimulation.Rows.Add(0, 2);   // 첫번째 줄에 전체 결과 추가
            spsLongTermSimulation.Rows[0].BackColor = Color.Orange;
            spsLongTermSimulation.Rows[1].BackColor = Color.Orange;

            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.DATE].Text = "로그기간";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.TIME].Text = "평균거래량";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.MSMD].Text = "매도/매수";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.PRICE].Text = "손절";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.RATE].Text = "익절";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.CPOWER].Text = "몇%수익매도";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.REASON].Text = "몇호가관통";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.DIFFERENCE_RATE].Text = "매수신호연속";
            spsLongTermSimulation.Cells[0, (int)LONG_SIMUL_COL.DIFFERENCE_AMOUNT].Text = "매도신호연속";

            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.DATE].Text = Convert.ToString(Program.cont.LogTerm);
            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.TIME].Text = Convert.ToString(Program.cont.LogTermVolumeOver);
            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.MSMD].Text = Convert.ToString(Program.cont.MsmdRate);
            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.PRICE].Text = Convert.ToString(Program.cont.CutoffPercent);
            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.RATE].Text = Convert.ToString(Program.cont.ProfitCutoffPercent);
            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.CPOWER].Text = Convert.ToString(Program.cont.SatisfyProfit);
            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.REASON].Text = Convert.ToString(Program.cont.PierceHoCnt);
            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.DIFFERENCE_RATE].Text = Convert.ToString(Program.cont.OrderSignCnt);
            spsLongTermSimulation.Cells[1, (int)LONG_SIMUL_COL.DIFFERENCE_AMOUNT].Text = Convert.ToString(Program.cont.SellSignCnt);
        }

        // Items를 모두 리셋(메모리 줄임)
        private void ResetItemTable()
        {
            List<string> cols = hItemLogs.Keys.Cast<string>().ToList();

            foreach (string col in cols)
            {
                hItemLogs[col] = new Item();
            }
        }

        private void chkAutoRecording_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoRecording.Checked)
            {
                tmrRecord.Enabled = true;
            }
            else
            {
                tmrRecord.Enabled = false;
            }
        }

        private void tmrRecord_Tick(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;

            if (time.Hour == 8 && time.Minute >= 31 && time.Minute < 36)
            {
                ResetItemTable();
                DoRecording();
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportExcel("");
        }

        private void ExportExcel(string name)
        {
            spdLongTermSimulation.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\" + DateTime.Now.ToShortDateString().Replace("-", "") + Convert.ToString(DateTime.Now.Hour).PadLeft(2, '0') + Convert.ToString(DateTime.Now.Minute).PadLeft(2, '0') + Convert.ToString(DateTime.Now.Second).PadLeft(2, '0') + "(" + name + ").xls");
        }

        private void btnSimulationConfig_Click(object sender, EventArgs e)
        {
            if (pnlSimulationOptions.Visible)
            {
                pnlSimulationOptions.Visible = false;
            }
            else
            {
                pnlSimulationOptions.Visible = true;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string summary = Common.getDialogFile();
            string itemlist = Common.getDialogFile();

            StreamReader srSummary = null;
            StreamReader itemSummary = null;
            int itemLength = 74;
            StreamWriter sw = null;

            try
            {
                sw = new StreamWriter("summary_test.txt", true);

                srSummary = new StreamReader(summary);

                string srLine;
                string itemLine;
                int idx = 0;

                while ((srLine = srSummary.ReadLine()) != null)
                {
                    if (srLine.Split(' ').Length > 8)
                    {
                        string[] strs = srLine.Split(' ');
                        List<string> strList = new List<string>();

                        for (int i = 0; i < strs.Length; i++)
                        {
                            strList.Add(strs[i]);
                        }

                        itemSummary = new StreamReader(itemlist);

                        int idx2 = -1;

                        while ((itemLine = itemSummary.ReadLine()) != null)
                        {
                            if (++idx2 < idx * itemLength) continue;
                            if (idx2 >= (idx + 1) * itemLength - 1) break;
                            sw.WriteLine(srLine + " ==> " + itemLine.Replace("%", ""));
                        }

                        itemSummary.Close();
                    }

                    idx++;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                srSummary.Close();
                sw.Close();
            }
        }
    }
}
