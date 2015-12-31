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
        public frmConfig()
        {
            InitializeComponent();
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDays.Text.Trim() == "" || txtCutoff.Text.Trim() == "" || txtProfitCutOff.Text.Trim() == ""
                || txtPowerLowLimit.Text.Trim() == "" || txtPowerHighLimit.Text.Trim() == "" || txtIgnoreCheCnt.Text.Trim() == ""
                || txtTermLog.Text.Trim() == "" || txtMsMdRate.Text.Trim() == "" || txtAvgVolumeOverRate.Text.Trim() == ""
                || txtContinueOrderCnt.Text.Trim() == "" || txtContinueSellCnt.Text.Trim() == "")
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

                StreamWriter sw = new StreamWriter(Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName);
                sw.WriteLine(txtDays.Text + "/" + txtCutoff.Text + "/" + txtProfitCutOff.Text + "/" + txtPowerLowLimit.Text + "/" + txtPowerHighLimit.Text
                                + "/" + txtIgnoreCheCnt.Text + "/" + txtPierceHoCnt.Text + "/" + txtTermLog.Text + "/" + txtMsMdRate.Text
                                + "/" + txtAvgVolumeOverRate.Text + "/" + txtContinueOrderCnt.Text + "/" + txtContinueSellCnt.Text);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("숫자 값이 아닙니다. 또는 설정 저장에 실패하였습니다.");
            }
        }
    }
}
