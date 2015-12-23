namespace eb
{
    partial class frmConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfig));
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCutoff = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProfitCutOff = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPowerLowLimit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPowerHighLimit = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtIgnoreCheCnt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPierceHoCnt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTermLog = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMsMdRate = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "평균거래량을 위한 날짜수";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(244, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "Save Config";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDays
            // 
            this.txtDays.Location = new System.Drawing.Point(185, 34);
            this.txtDays.MaxLength = 3;
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(51, 21);
            this.txtDays.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "손절 %";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCutoff
            // 
            this.txtCutoff.Location = new System.Drawing.Point(185, 62);
            this.txtCutoff.MaxLength = 5;
            this.txtCutoff.Name = "txtCutoff";
            this.txtCutoff.Size = new System.Drawing.Size(51, 21);
            this.txtCutoff.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "익절 %";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProfitCutOff
            // 
            this.txtProfitCutOff.Location = new System.Drawing.Point(185, 88);
            this.txtProfitCutOff.MaxLength = 5;
            this.txtProfitCutOff.Name = "txtProfitCutOff";
            this.txtProfitCutOff.Size = new System.Drawing.Size(51, 21);
            this.txtProfitCutOff.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "체결강도 최저 Limit";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPowerLowLimit
            // 
            this.txtPowerLowLimit.Location = new System.Drawing.Point(185, 115);
            this.txtPowerLowLimit.MaxLength = 5;
            this.txtPowerLowLimit.Name = "txtPowerLowLimit";
            this.txtPowerLowLimit.Size = new System.Drawing.Size(51, 21);
            this.txtPowerLowLimit.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(167, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "체경강도 최대 Limit";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPowerHighLimit
            // 
            this.txtPowerHighLimit.Location = new System.Drawing.Point(185, 142);
            this.txtPowerHighLimit.MaxLength = 5;
            this.txtPowerHighLimit.Name = "txtPowerHighLimit";
            this.txtPowerHighLimit.Size = new System.Drawing.Size(51, 21);
            this.txtPowerHighLimit.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "장시작 후 최초 몇거래 무시";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIgnoreCheCnt
            // 
            this.txtIgnoreCheCnt.Location = new System.Drawing.Point(185, 169);
            this.txtIgnoreCheCnt.MaxLength = 5;
            this.txtIgnoreCheCnt.Name = "txtIgnoreCheCnt";
            this.txtIgnoreCheCnt.Size = new System.Drawing.Size(51, 21);
            this.txtIgnoreCheCnt.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "몇호가 뚫으면 구매 할건지";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPierceHoCnt
            // 
            this.txtPierceHoCnt.Location = new System.Drawing.Point(185, 196);
            this.txtPierceHoCnt.MaxLength = 5;
            this.txtPierceHoCnt.Name = "txtPierceHoCnt";
            this.txtPierceHoCnt.Size = new System.Drawing.Size(51, 21);
            this.txtPierceHoCnt.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 226);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(167, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "로그 기간 설정(초)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTermLog
            // 
            this.txtTermLog.Location = new System.Drawing.Point(185, 223);
            this.txtTermLog.MaxLength = 5;
            this.txtTermLog.Name = "txtTermLog";
            this.txtTermLog.Size = new System.Drawing.Size(51, 21);
            this.txtTermLog.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(12, 253);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(167, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "매도/매수 기준";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMsMdRate
            // 
            this.txtMsMdRate.Location = new System.Drawing.Point(185, 250);
            this.txtMsMdRate.MaxLength = 5;
            this.txtMsMdRate.Name = "txtMsMdRate";
            this.txtMsMdRate.Size = new System.Drawing.Size(51, 21);
            this.txtMsMdRate.TabIndex = 2;
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 287);
            this.Controls.Add(this.txtMsMdRate);
            this.Controls.Add(this.txtTermLog);
            this.Controls.Add(this.txtPierceHoCnt);
            this.Controls.Add(this.txtIgnoreCheCnt);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPowerHighLimit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtPowerLowLimit);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtProfitCutOff);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCutoff);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDays);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmConfig";
            this.Text = "frmConfig";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TextBox txtDays;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCutoff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProfitCutOff;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPowerLowLimit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPowerHighLimit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtIgnoreCheCnt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPierceHoCnt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTermLog;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMsMdRate;
    }
}