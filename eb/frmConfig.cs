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
                }
            }

            return retString;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(ChkNullText())
            {
                MessageBox.Show("값이 없습니다.");
                return;
            }

            try
            {
                Program.cont.VolumeHistoryCnt = Common.getIntValue(txtDays.Text);
                Program.cont.CutoffPercent = Common.getDoubleValue(txtCutoff.Text);
                Program.cont.ProfitCutoffPercent = Common.getDoubleValue(txtProfitCutOff.Text);
                Program.cont.PowerLowLimit = Common.getDoubleValue(txtPowerLowLimit.Text);
                Program.cont.PowerHighLimit = Common.getDoubleValue(txtPowerHighLimit.Text);
                Program.cont.IgnoreCheCnt = Common.getIntValue(txtIgnoreCheCnt.Text);
                Program.cont.PierceHoCnt = Common.getIntValue(txtPierceHoCnt.Text);
                Program.cont.LogTerm = Common.getIntValue(txtTermLog.Text);
                Program.cont.MsmdRate = Common.getDoubleValue(txtMsMdRate.Text);
                Program.cont.LogTermVolumeOver = Common.getDoubleValue(txtAvgVolumeOverRate.Text);
                Program.cont.OrderSignCnt = Common.getIntValue(txtContinueOrderCnt.Text);
                Program.cont.SellSignCnt = Common.getIntValue(txtContinueSellCnt.Text);
                Program.cont.MsCutLine = Common.getIntValue(txtMsCutLine.Text);
                Program.cont.MdCutLine = Common.getIntValue(txtMdCutLine.Text);

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
