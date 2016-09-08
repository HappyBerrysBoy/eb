using eb.common;
using System;
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
    public partial class frmConfig : Form
    {
        private int txtCnt = 0;

        public frmConfig()
        {
            InitializeComponent();
            SetTextBoxCnt();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            txtDays.Text = Program.cont.VolumeHistoryCnt.ToString();
            txtCutoff.Text = Program.cont.CutoffPercent.ToString();
            txtProfitCutOff.Text = Program.cont.ProfitCutoffPercent.ToString();
            txtPowerLowLimit.Text = Program.cont.PowerLowLimit.ToString();
            txtPowerHighLimit.Text = Program.cont.PowerHighLimit.ToString();
            txtIgnoreCheCnt.Text = Program.cont.IgnoreCheCnt.ToString();
            txtPierceHoCnt.Text = Program.cont.PierceHoCnt.ToString();
            txtTermLog.Text = Program.cont.LogTerm.ToString();
            txtMsMdRate.Text = Program.cont.MsmdRate.ToString();
            txtAvgVolumeOverRate.Text = Program.cont.LogTermVolumeOver.ToString();
            txtContinueOrderCnt.Text = Program.cont.OrderSignCnt.ToString();
            txtContinueSellCnt.Text = Program.cont.SellSignCnt.ToString();
            txtMsCutLine.Text = Program.cont.MsCutLine.ToString();
            txtMdCutLine.Text = Program.cont.MdCutLine.ToString();
            txtDifferenceChePower.Text = Program.cont.DifferenceChePower.ToString();
            txtSatisfyProfit.Text = Program.cont.SatisfyProfit.ToString();
            txtCutOffHour.Text = Program.cont.CutOffHour.ToString();
            txtCutOffMin.Text = Program.cont.CutOffMinute.ToString();
        }

        private void SetTextBoxCnt()
        {
            txtCnt = 0;

            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is TextBox)
                    txtCnt++;
            }
        }

        private bool ChkNullText()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is TextBox)
                {
                    TextBox txt = (TextBox)this.Controls[i];
                    if (txt.Text.Trim() == "")
                        return true;
                }
            }

            return false;
        }

        private string GetConfigString()
        {
            string retString = "";

            for (int i = 0; i < txtCnt; i++)
            {
                if (i > 0)
                {
                    retString += "/";
                }

                switch (i)
                {
                    case (int)Common.CONFIG_IDX.VOLUME_HISTORY_CNT:
                        retString += txtDays.Text;
                        break;
                    case (int)Common.CONFIG_IDX.CUT_OFF_PERCENT:
                        retString += txtCutoff.Text;
                        break;
                    case (int)Common.CONFIG_IDX.PROFIT_CUT_OFF_PERCENT:
                        retString += txtProfitCutOff.Text;
                        break;
                    case (int)Common.CONFIG_IDX.POWER_LOW_LIMIT:
                        retString += txtPowerLowLimit.Text;
                        break;
                    case (int)Common.CONFIG_IDX.POWER_HIGH_LIMIT:
                        retString += txtPowerHighLimit.Text;
                        break;
                    case (int)Common.CONFIG_IDX.IGNORE_CHE_CNT:
                        retString += txtIgnoreCheCnt.Text;
                        break;
                    case (int)Common.CONFIG_IDX.PIERCE_HO_CNT:
                        retString += txtPierceHoCnt.Text;
                        break;
                    case (int)Common.CONFIG_IDX.LOG_TERM:
                        retString += txtTermLog.Text;
                        break;
                    case (int)Common.CONFIG_IDX.MS_MD_RATE:
                        retString += txtMsMdRate.Text;
                        break;
                    case (int)Common.CONFIG_IDX.LOG_TERM_VOLUME_OVER:
                        retString += txtAvgVolumeOverRate.Text;
                        break;
                    case (int)Common.CONFIG_IDX.ORDER_SIGN_CNT:
                        retString += txtContinueOrderCnt.Text;
                        break;
                    case (int)Common.CONFIG_IDX.SELL_SIGN_CNT:
                        retString += txtContinueSellCnt.Text;
                        break;
                    case (int)Common.CONFIG_IDX.MS_CUT_LINE:
                        retString += txtMsCutLine.Text;
                        break;
                    case (int)Common.CONFIG_IDX.MD_CUT_LINE:
                        retString += txtMdCutLine.Text;
                        break;
                    case (int)Common.CONFIG_IDX.DIFFERENCE_CHEPOWER:
                        retString += txtDifferenceChePower.Text;
                        break;
                    case (int)Common.CONFIG_IDX.SATISFY_PROFIT:
                        retString += txtSatisfyProfit.Text;
                        break;
                    case (int)Common.CONFIG_IDX.CUT_OFF_HOUR:
                        retString += txtCutOffHour.Text;
                        break;
                    case (int)Common.CONFIG_IDX.CUT_OFF_MIN:
                        retString += txtCutOffMin.Text;
                        break;
                }
            }

            return retString;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
            Common.SetConfig();
        }

        private void SaveConfig()
        {
            if (ChkNullText())
            {
                MessageBox.Show("값이 없는 항목이 존재합니다.");
                return;
            }

            try
            {
                StreamWriter sw = new StreamWriter(Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName);
                sw.WriteLine(GetConfigString());
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("숫자 값이 아닙니다. 또는 설정 저장에 실패하였습니다.");
            }
        }
    }
}
