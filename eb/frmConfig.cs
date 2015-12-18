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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDays.Text.Trim() == "" || txtCutoff.Text.Trim() == "" || txtProfitCutOff.Text.Trim() == "")
            {
                MessageBox.Show("값이 없습니다.");
                return;
            }

            try
            {
                Program.cont.VolumeHistoryCnt = Convert.ToInt32(txtDays.Text);
                Program.cont.CutoffPercent = Convert.ToDouble(txtCutoff.Text);
                Program.cont.ProfitCutoffPercent = Convert.ToDouble(txtProfitCutOff.Text);

                StreamWriter sw = new StreamWriter(Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName);
                sw.WriteLine(txtDays.Text + "/" + txtCutoff.Text + "/" + txtProfitCutOff.Text);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("숫자 값이 아닙니다. 또는 설정 저장에 실패하였습니다.");
            }
        }
    }
}
