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
        XA_DATASETLib.XAQuery cspaq12200Query;        // 계좌 예수금/주문가능금액 등

        XA_DATASETLib.XAReal queryRealPI;         // 코스피 실시간 시세
        XA_DATASETLib.XAReal queryRealDAK;         // 코스닥 실시간 시세

        List<string> kindList = new List<string>();

        Hashtable hQueryKind = new Hashtable();
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

        enum LONG_SIMUL_COL
        {
            DATE,
            TIME,
            MSMD,
            PRICE,
            RATE,
            CPOWER,
            DIFFERENCE_RATE,
            DIFFERENCE_AMOUNT,
            PROFIT,
            TOTAL_PROFIT
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
                        Program.cont.VolumeHistoryCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.VOLUME_HISTORY_CNT]);
                        Program.cont.CutoffPercent = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.CUT_OFF_PERCENT]);
                        Program.cont.ProfitCutoffPercent = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.PROFIT_CUT_OFF_PERCENT]);
                        Program.cont.PowerLowLimit = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.POWER_LOW_LIMIT]);
                        Program.cont.PowerHighLimit = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.POWER_HIGH_LIMIT]);
                        Program.cont.IgnoreCheCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.IGNORE_CHE_CNT]);
                        Program.cont.PierceHoCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.PIERCE_HO_CNT]);
                        Program.cont.LogTerm = Common.getIntValue(strs[(int)Common.CONFIG_IDX.LOG_TERM]);
                        Program.cont.MsmdRate = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.MS_MD_RATE]);
                        Program.cont.LogTermVolumeOver = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.LOG_TERM_VOLUME_OVER]);
                        Program.cont.OrderSignCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.ORDER_SIGN_CNT]);
                        Program.cont.SellSignCnt = Common.getIntValue(strs[(int)Common.CONFIG_IDX.SELL_SIGN_CNT]);
                        Program.cont.MsCutLine = Common.getIntValue(strs[(int)Common.CONFIG_IDX.MS_CUT_LINE]);
                        Program.cont.MdCutLine = Common.getIntValue(strs[(int)Common.CONFIG_IDX.MD_CUT_LINE]);
                        Program.cont.DifferenceChePower = Common.getDoubleValue(strs[(int)Common.CONFIG_IDX.DIFFERENCE_CHEPOWER]);
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
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DATE].Text = currFileName;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TIME].Text = cls.Chetime;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD].Text = "매수";
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.PRICE].Text = cls.Price;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.RATE].Text = cls.Drate;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.CPOWER].Text = cls.Cpower;
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
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DATE].Text = currFileName;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TIME].Text = cls.Chetime;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.MSMD].Text = "매도";
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.PRICE].Text = cls.Price;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.RATE].Text = cls.Drate;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.CPOWER].Text = cls.Cpower;
                    string beforeRate = spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 2, (int)LONG_SIMUL_COL.RATE].Text;
                    string beforeAmount = spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 2, (int)LONG_SIMUL_COL.PRICE].Text;
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DIFFERENCE_RATE].Text = Convert.ToString(Convert.ToDouble(cls.Drate) - Convert.ToDouble(beforeRate));
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.DIFFERENCE_AMOUNT].Text = Convert.ToString(Convert.ToInt32(cls.Price) - Convert.ToInt32(beforeAmount));
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.PROFIT].Text = CalcProfit();
                    spsLongTermSimulation.Cells[spsLongTermSimulation.RowCount - 1, (int)LONG_SIMUL_COL.TOTAL_PROFIT].Text = "";
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
            getAvgVolume();
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

        private void simulationProcess(List<string> list)
        {
            ClsRealChe cls = setRealDataClass(list);
            chkOrderLogic(cls);
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
            string filename = Common.getDialogFile();
            LoadLogData(isSimulations, filename);
        }

        private void LoadLogData(bool isSimulations, string filename)
        {
            if (filename == "") return;

            StreamReader sr = null;
            chkSimulation = isSimulations;

            try
            {
                currFileName = filename;

                sr = new StreamReader(filename);
                string line;
                spsLog.RowCount = 0;
                string currRate = "";
                string beforeRate = "";
                bool isFirst = true;

                Cursor.Current = Cursors.WaitCursor;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split('/').Length > 10)
                    {
                        string[] strs = line.Split('/');
                        List<string> strList = new List<string>();

                        for (int i = 0; i < strs.Length; i++)
                        {
                            strList.Add(strs[i]);
                        }

                        string drate = strList[(int)LOG_COL.DRATE];
                        string gubun = strList[(int)LOG_COL.GUBUN];

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

                            if (gubun.Equals("+"))
                                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Program.cont.MsSign;
                            else
                                spsLog.Cells[spsLog.RowCount - 1, (int)LOG_COL.GUBUN].BackColor = Program.cont.MdSign;
                        }

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

                            if (!isFirst)
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
                                isFirst = false;
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

        private void btnLongTermSimulation_Click(object sender, EventArgs e)
        {
            if (spsInterest.ActiveRowIndex < 0) return;
            spsLongTermSimulation.RowCount = 0;
            chkLongTermSimulation = true;
            chkSimulation = true;
            spdLog.Visible = false;
            spdLongTermSimulation.Visible = true;

            string code = spsInterest.Cells[spsInterest.ActiveRowIndex, (int)INTER_COL.CODE].Text;
            string folderPath = Common.GetDialogFolder();

            List<string> fileList = GetSelectedFileList(folderPath, code);

            for (int i = 0; i < fileList.Count; i++)
            {
                LoadLogData(true, fileList[i]);
            }

            chkLongTermSimulation = false;
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
                DoRecording();
            }
        }
    }
}
