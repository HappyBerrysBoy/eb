using eb.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            //SetTextBoxCnt();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

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
            txtAllCodePageNum.Text = Program.cont.AllCodePageNum.ToString();
            txtAllCodeTtlPage.Text = Program.cont.AllCodeTtlPage.ToString();
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
                string section = Program.cont.getINISection;
                string filename = Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName;
                //StreamWriter sw = new StreamWriter(Program.cont.getApplicationPath + Program.cont.getConfigPath + Program.cont.getConfigFileName);
                // 설정을 일렬로 나눠서 저장하는건 나중에 알아보기 힘듦으로 설명을 붙여서 다시 저장하자.
                //sw.WriteLine(GetConfigString());

                WritePrivateProfileString(section, "VOLUME_HISTORY_CNT", txtDays.Text, filename);
                WritePrivateProfileString(section, "CUT_OFF_PERCENT", txtCutoff.Text, filename);
                WritePrivateProfileString(section, "PROFIT_CUT_OFF_PERCENT", txtProfitCutOff.Text, filename);
                WritePrivateProfileString(section, "POWER_LOW_LIMIT", txtPowerLowLimit.Text, filename);
                WritePrivateProfileString(section, "POWER_HIGH_LIMIT", txtPowerHighLimit.Text, filename);
                WritePrivateProfileString(section, "IGNORE_CHE_CNT", txtIgnoreCheCnt.Text, filename);
                WritePrivateProfileString(section, "PIERCE_HO_CNT", txtPierceHoCnt.Text, filename);
                WritePrivateProfileString(section, "LOG_TERM", txtTermLog.Text, filename);
                WritePrivateProfileString(section, "MS_MD_RATE", txtMsMdRate.Text, filename);
                WritePrivateProfileString(section, "LOG_TERM_VOLUME_OVER", txtAvgVolumeOverRate.Text, filename);
                WritePrivateProfileString(section, "ORDER_SIGN_CNT", txtContinueOrderCnt.Text, filename);
                WritePrivateProfileString(section, "SELL_SIGN_CNT", txtContinueSellCnt.Text, filename);
                WritePrivateProfileString(section, "MS_CUT_LINE", txtMsCutLine.Text, filename);
                WritePrivateProfileString(section, "MD_CUT_LINE", txtMdCutLine.Text, filename);
                WritePrivateProfileString(section, "DIFFERENCE_CHEPOWER", txtDifferenceChePower.Text, filename);
                WritePrivateProfileString(section, "SATISFY_PROFIT", txtSatisfyProfit.Text, filename);
                WritePrivateProfileString(section, "CUT_OFF_HOUR", txtCutOffHour.Text, filename);
                WritePrivateProfileString(section, "CUT_OFF_MIN", txtCutOffMin.Text, filename);
                WritePrivateProfileString(section, "ALL_CODE_PAGE_NUM", txtAllCodePageNum.Text, filename);
                WritePrivateProfileString(section, "ALL_CODE_TTL_PAGE", txtAllCodeTtlPage.Text, filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show("숫자 값이 아닙니다. 또는 설정 저장에 실패하였습니다.");
            }
        }
    }
}
