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
            this.label10 = new System.Windows.Forms.Label();
            this.txtAvgVolumeOverRate = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtContinueOrderCnt = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtContinueSellCnt = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMsCutLine = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMdCutLine = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 65);
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
            this.toolStrip1.Size = new System.Drawing.Size(636, 25);
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
            this.txtDays.Location = new System.Drawing.Point(185, 62);
            this.txtDays.MaxLength = 7;
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(51, 21);
            this.txtDays.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "손절 %";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCutoff
            // 
            this.txtCutoff.Location = new System.Drawing.Point(185, 116);
            this.txtCutoff.MaxLength = 7;
            this.txtCutoff.Name = "txtCutoff";
            this.txtCutoff.Size = new System.Drawing.Size(51, 21);
            this.txtCutoff.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "익절 %";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProfitCutOff
            // 
            this.txtProfitCutOff.Location = new System.Drawing.Point(185, 142);
            this.txtProfitCutOff.MaxLength = 7;
            this.txtProfitCutOff.Name = "txtProfitCutOff";
            this.txtProfitCutOff.Size = new System.Drawing.Size(51, 21);
            this.txtProfitCutOff.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "체결강도 최저 Limit";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPowerLowLimit
            // 
            this.txtPowerLowLimit.Location = new System.Drawing.Point(185, 169);
            this.txtPowerLowLimit.MaxLength = 7;
            this.txtPowerLowLimit.Name = "txtPowerLowLimit";
            this.txtPowerLowLimit.Size = new System.Drawing.Size(51, 21);
            this.txtPowerLowLimit.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(167, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "체경강도 최대 Limit";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPowerHighLimit
            // 
            this.txtPowerHighLimit.Location = new System.Drawing.Point(185, 196);
            this.txtPowerHighLimit.MaxLength = 7;
            this.txtPowerHighLimit.Name = "txtPowerHighLimit";
            this.txtPowerHighLimit.Size = new System.Drawing.Size(51, 21);
            this.txtPowerHighLimit.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 226);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "장시작 후 최초 몇거래 무시";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIgnoreCheCnt
            // 
            this.txtIgnoreCheCnt.Location = new System.Drawing.Point(185, 223);
            this.txtIgnoreCheCnt.MaxLength = 7;
            this.txtIgnoreCheCnt.Name = "txtIgnoreCheCnt";
            this.txtIgnoreCheCnt.Size = new System.Drawing.Size(51, 21);
            this.txtIgnoreCheCnt.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "몇호가 뚫으면 구매 할건지";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPierceHoCnt
            // 
            this.txtPierceHoCnt.Location = new System.Drawing.Point(185, 250);
            this.txtPierceHoCnt.MaxLength = 7;
            this.txtPierceHoCnt.Name = "txtPierceHoCnt";
            this.txtPierceHoCnt.Size = new System.Drawing.Size(51, 21);
            this.txtPierceHoCnt.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 280);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(167, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "로그 기간 설정(초)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTermLog
            // 
            this.txtTermLog.Location = new System.Drawing.Point(185, 277);
            this.txtTermLog.MaxLength = 7;
            this.txtTermLog.Name = "txtTermLog";
            this.txtTermLog.Size = new System.Drawing.Size(51, 21);
            this.txtTermLog.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(12, 307);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(167, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "매도/매수 기준";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMsMdRate
            // 
            this.txtMsMdRate.Location = new System.Drawing.Point(185, 304);
            this.txtMsMdRate.MaxLength = 7;
            this.txtMsMdRate.Name = "txtMsMdRate";
            this.txtMsMdRate.Size = new System.Drawing.Size(51, 21);
            this.txtMsMdRate.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(12, 92);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(167, 23);
            this.label10.TabIndex = 0;
            this.label10.Text = "평균거래량의 몇% 넘겨야";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAvgVolumeOverRate
            // 
            this.txtAvgVolumeOverRate.Location = new System.Drawing.Point(185, 89);
            this.txtAvgVolumeOverRate.MaxLength = 7;
            this.txtAvgVolumeOverRate.Name = "txtAvgVolumeOverRate";
            this.txtAvgVolumeOverRate.Size = new System.Drawing.Size(51, 21);
            this.txtAvgVolumeOverRate.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(31, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(167, 23);
            this.label11.TabIndex = 0;
            this.label11.Text = "구매관련 설정값";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(12, 334);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(167, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "매수신호 연속 몇번";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContinueOrderCnt
            // 
            this.txtContinueOrderCnt.Location = new System.Drawing.Point(185, 331);
            this.txtContinueOrderCnt.MaxLength = 7;
            this.txtContinueOrderCnt.Name = "txtContinueOrderCnt";
            this.txtContinueOrderCnt.Size = new System.Drawing.Size(51, 21);
            this.txtContinueOrderCnt.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(12, 361);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(167, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "매도신호 연속 몇번";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContinueSellCnt
            // 
            this.txtContinueSellCnt.Location = new System.Drawing.Point(185, 358);
            this.txtContinueSellCnt.MaxLength = 7;
            this.txtContinueSellCnt.Name = "txtContinueSellCnt";
            this.txtContinueSellCnt.Size = new System.Drawing.Size(51, 21);
            this.txtContinueSellCnt.TabIndex = 12;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(242, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(167, 23);
            this.label14.TabIndex = 0;
            this.label14.Text = "몇% 이상이면 매수안함";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMsCutLine
            // 
            this.txtMsCutLine.Location = new System.Drawing.Point(415, 62);
            this.txtMsCutLine.MaxLength = 7;
            this.txtMsCutLine.Name = "txtMsCutLine";
            this.txtMsCutLine.Size = new System.Drawing.Size(51, 21);
            this.txtMsCutLine.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(242, 92);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(167, 23);
            this.label15.TabIndex = 0;
            this.label15.Text = "몇% 이상이면 매도함";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMdCutLine
            // 
            this.txtMdCutLine.Location = new System.Drawing.Point(415, 89);
            this.txtMdCutLine.MaxLength = 7;
            this.txtMdCutLine.Name = "txtMdCutLine";
            this.txtMdCutLine.Size = new System.Drawing.Size(51, 21);
            this.txtMdCutLine.TabIndex = 1;
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 394);
            this.Controls.Add(this.txtContinueSellCnt);
            this.Controls.Add(this.txtContinueOrderCnt);
            this.Controls.Add(this.txtMsMdRate);
            this.Controls.Add(this.txtTermLog);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtPierceHoCnt);
            this.Controls.Add(this.label12);
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
            this.Controls.Add(this.txtAvgVolumeOverRate);
            this.Controls.Add(this.txtMdCutLine);
            this.Controls.Add(this.txtMsCutLine);
            this.Controls.Add(this.txtDays);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label11);
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
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAvgVolumeOverRate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtContinueOrderCnt;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtContinueSellCnt;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtMsCutLine;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMdCutLine;
    }
}