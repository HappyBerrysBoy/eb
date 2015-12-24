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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDays.Text.Trim() == "" || txtCutoff.Text.Trim() == "" || txtProfitCutOff.Text.Trim() == ""
                || txtPowerLowLimit.Text.Trim() == "" || txtPowerHighLimit.Text.Trim() == "" || txtIgnoreCheCnt.Text.Trim() == ""
                || txtTermLog.Text.Trim() == "" || txtMsMdRate.Text.Trim() == "" || txtAvgVolumeOverRate.Text.Trim() == "")
            {
                MessageBox.Show("값이 없습니다.");
                return;
            }

            try
            {
                Program.cont.VolumeHistoryCnt = Convert.ToInt32(txtDays.Text);
                Program.cont.CutoffPercent = Convert.ToDouble(txtCutoff.Text);
                Program.cont.ProfitCutoffPercent = Convert.ToDouble(txtProfitCutOff.Text);
                Program.cont.PowerLowLimit = Convert.ToDouble(txtPowerLowLimit.Text);
                Program.cont.PowerHighLimit = Convert.ToDouble(txtPowerHighLimit.Text);
                Program.cont.IgnoreCheCnt = Convert.ToInt32(txtIgnoreCheCnt.Text);
                Program.cont.PierceHoCnt = Convert.ToInt32(txtPierceHoCnt.Text);
                Program.cont.LogTerm = Convert.ToInt32(txtTermLog.Text);
                Program.cont.MsmdRate = Convert.ToDouble(txtMsMdRate.Text);
                Program.cont.LogTermVolumeOver = Convert.ToDouble(txtAvgVolumeOverRate.Text);

                StreamWriter sw = new StreamWriter(Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName);
                sw.WriteLine(txtDays.Text + "/" + txtCutoff.Text + "/" + txtProfitCutOff.Text + "/" + txtPowerLowLimit.Text + "/" + txtPowerHighLimit.Text
                                + "/" + txtIgnoreCheCnt.Text + "/" + txtPierceHoCnt.Text + "/" + txtTermLog.Text + "/" + txtMsMdRate.Text
                                + "/" + txtAvgVolumeOverRate.Text);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("숫자 값이 아닙니다. 또는 설정 저장에 실패하였습니다.");
            }
        }
    }
}
