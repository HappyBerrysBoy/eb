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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMdCutLine = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDifferenceChePower = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtMsMdRate = new System.Windows.Forms.TextBox();
            this.txtTermLog = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAvgVolumeOverRate = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMsCutLine = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtIgnoreCheCnt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPowerHighLimit = new System.Windows.Forms.TextBox();
            this.txtPowerLowLimit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtCutOffMin = new System.Windows.Forms.TextBox();
            this.txtCutOffHour = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtContinueOrderCnt = new System.Windows.Forms.TextBox();
            this.txtPierceHoCnt = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProfitCutOff = new System.Windows.Forms.TextBox();
            this.txtCutoff = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSatisfyProfit = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtContinueSellCnt = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAllCodePageNum = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtAllCodeTtlPage = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1233, 25);
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
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(8, 18);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(167, 23);
            this.label15.TabIndex = 0;
            this.label15.Text = "현재 몇%이상이면 매도함";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMdCutLine
            // 
            this.txtMdCutLine.Location = new System.Drawing.Point(181, 18);
            this.txtMdCutLine.MaxLength = 7;
            this.txtMdCutLine.Name = "txtMdCutLine";
            this.txtMdCutLine.Size = new System.Drawing.Size(51, 21);
            this.txtMdCutLine.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(183, 25);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(389, 23);
            this.label17.TabIndex = 15;
            this.label17.Text = "(※ 참고 : 전일 거래량 기준, 1분에 0.25641% 가 이루어 져야함)";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAllCodeTtlPage);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.txtAllCodePageNum);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.txtIgnoreCheCnt);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtDays);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 366);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "일반 옵션";
            // 
            // txtDays
            // 
            this.txtDays.Location = new System.Drawing.Point(184, 19);
            this.txtDays.MaxLength = 7;
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(51, 21);
            this.txtDays.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "평균거래량을 위한 날짜수";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtContinueOrderCnt);
            this.groupBox2.Controls.Add(this.txtPierceHoCnt);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtPowerHighLimit);
            this.groupBox2.Controls.Add(this.txtPowerLowLimit);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtMsCutLine);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.txtDifferenceChePower);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.txtMsMdRate);
            this.groupBox2.Controls.Add(this.txtTermLog);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtAvgVolumeOverRate);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(269, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 365);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "매수관련 설정";
            // 
            // txtDifferenceChePower
            // 
            this.txtDifferenceChePower.Location = new System.Drawing.Point(184, 99);
            this.txtDifferenceChePower.MaxLength = 7;
            this.txtDifferenceChePower.Name = "txtDifferenceChePower";
            this.txtDifferenceChePower.Size = new System.Drawing.Size(51, 21);
            this.txtDifferenceChePower.TabIndex = 22;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(11, 100);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(167, 23);
            this.label16.TabIndex = 21;
            this.label16.Text = "구매시 체결강도 차이";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMsMdRate
            // 
            this.txtMsMdRate.Location = new System.Drawing.Point(184, 72);
            this.txtMsMdRate.MaxLength = 7;
            this.txtMsMdRate.Name = "txtMsMdRate";
            this.txtMsMdRate.Size = new System.Drawing.Size(51, 21);
            this.txtMsMdRate.TabIndex = 20;
            // 
            // txtTermLog
            // 
            this.txtTermLog.Location = new System.Drawing.Point(184, 18);
            this.txtTermLog.MaxLength = 7;
            this.txtTermLog.Name = "txtTermLog";
            this.txtTermLog.Size = new System.Drawing.Size(51, 21);
            this.txtTermLog.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(11, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(167, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "매도/매수 기준";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(11, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(167, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "로그 기간 설정(초)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAvgVolumeOverRate
            // 
            this.txtAvgVolumeOverRate.Location = new System.Drawing.Point(184, 45);
            this.txtAvgVolumeOverRate.MaxLength = 7;
            this.txtAvgVolumeOverRate.Name = "txtAvgVolumeOverRate";
            this.txtAvgVolumeOverRate.Size = new System.Drawing.Size(51, 21);
            this.txtAvgVolumeOverRate.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(11, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(167, 23);
            this.label10.TabIndex = 17;
            this.label10.Text = "평균거래량의 몇% 넘겨야";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMsCutLine
            // 
            this.txtMsCutLine.Location = new System.Drawing.Point(184, 126);
            this.txtMsCutLine.MaxLength = 7;
            this.txtMsCutLine.Name = "txtMsCutLine";
            this.txtMsCutLine.Size = new System.Drawing.Size(51, 21);
            this.txtMsCutLine.TabIndex = 24;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(11, 129);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(167, 23);
            this.label14.TabIndex = 23;
            this.label14.Text = "몇% 이상이면 매수안함";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIgnoreCheCnt
            // 
            this.txtIgnoreCheCnt.Location = new System.Drawing.Point(184, 46);
            this.txtIgnoreCheCnt.MaxLength = 7;
            this.txtIgnoreCheCnt.Name = "txtIgnoreCheCnt";
            this.txtIgnoreCheCnt.Size = new System.Drawing.Size(51, 21);
            this.txtIgnoreCheCnt.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(11, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "장시작 후 최초 몇거래 무시";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPowerHighLimit
            // 
            this.txtPowerHighLimit.Location = new System.Drawing.Point(184, 180);
            this.txtPowerHighLimit.MaxLength = 7;
            this.txtPowerHighLimit.Name = "txtPowerHighLimit";
            this.txtPowerHighLimit.Size = new System.Drawing.Size(51, 21);
            this.txtPowerHighLimit.TabIndex = 28;
            // 
            // txtPowerLowLimit
            // 
            this.txtPowerLowLimit.Location = new System.Drawing.Point(184, 153);
            this.txtPowerLowLimit.MaxLength = 7;
            this.txtPowerLowLimit.Name = "txtPowerLowLimit";
            this.txtPowerLowLimit.Size = new System.Drawing.Size(51, 21);
            this.txtPowerLowLimit.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(11, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(167, 12);
            this.label5.TabIndex = 25;
            this.label5.Text = "체경강도 최대 Limit";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "체결강도 최저 Limit";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtContinueSellCnt);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.txtSatisfyProfit);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.txtProfitCutOff);
            this.groupBox3.Controls.Add(this.txtCutoff);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtCutOffMin);
            this.groupBox3.Controls.Add(this.txtCutOffHour);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.txtMdCutLine);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Location = new System.Drawing.Point(519, 51);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(245, 366);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "매도관련 설정";
            // 
            // txtCutOffMin
            // 
            this.txtCutOffMin.Location = new System.Drawing.Point(181, 72);
            this.txtCutOffMin.MaxLength = 7;
            this.txtCutOffMin.Name = "txtCutOffMin";
            this.txtCutOffMin.Size = new System.Drawing.Size(51, 21);
            this.txtCutOffMin.TabIndex = 10;
            // 
            // txtCutOffHour
            // 
            this.txtCutOffHour.Location = new System.Drawing.Point(181, 45);
            this.txtCutOffHour.MaxLength = 7;
            this.txtCutOffHour.Name = "txtCutOffHour";
            this.txtCutOffHour.Size = new System.Drawing.Size(51, 21);
            this.txtCutOffHour.TabIndex = 9;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(8, 75);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(167, 12);
            this.label20.TabIndex = 7;
            this.label20.Text = "장마감전 무조건파는 분";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(8, 50);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(167, 12);
            this.label19.TabIndex = 8;
            this.label19.Text = "장마감전 무조건파는 시간";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContinueOrderCnt
            // 
            this.txtContinueOrderCnt.Location = new System.Drawing.Point(184, 235);
            this.txtContinueOrderCnt.MaxLength = 7;
            this.txtContinueOrderCnt.Name = "txtContinueOrderCnt";
            this.txtContinueOrderCnt.Size = new System.Drawing.Size(51, 21);
            this.txtContinueOrderCnt.TabIndex = 32;
            // 
            // txtPierceHoCnt
            // 
            this.txtPierceHoCnt.Location = new System.Drawing.Point(184, 207);
            this.txtPierceHoCnt.MaxLength = 7;
            this.txtPierceHoCnt.Name = "txtPierceHoCnt";
            this.txtPierceHoCnt.Size = new System.Drawing.Size(51, 21);
            this.txtPierceHoCnt.TabIndex = 31;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(11, 238);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(167, 12);
            this.label12.TabIndex = 29;
            this.label12.Text = "매수신호 연속 몇번";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(11, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(167, 12);
            this.label7.TabIndex = 30;
            this.label7.Text = "몇호가 뚫으면 구매 할건지";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProfitCutOff
            // 
            this.txtProfitCutOff.Location = new System.Drawing.Point(181, 125);
            this.txtProfitCutOff.MaxLength = 7;
            this.txtProfitCutOff.Name = "txtProfitCutOff";
            this.txtProfitCutOff.Size = new System.Drawing.Size(51, 21);
            this.txtProfitCutOff.TabIndex = 14;
            // 
            // txtCutoff
            // 
            this.txtCutoff.Location = new System.Drawing.Point(181, 99);
            this.txtCutoff.MaxLength = 7;
            this.txtCutoff.Name = "txtCutoff";
            this.txtCutoff.Size = new System.Drawing.Size(51, 21);
            this.txtCutoff.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "익절 %";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "손절 %";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSatisfyProfit
            // 
            this.txtSatisfyProfit.Location = new System.Drawing.Point(181, 152);
            this.txtSatisfyProfit.MaxLength = 7;
            this.txtSatisfyProfit.Name = "txtSatisfyProfit";
            this.txtSatisfyProfit.Size = new System.Drawing.Size(51, 21);
            this.txtSatisfyProfit.TabIndex = 16;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(8, 155);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(167, 12);
            this.label18.TabIndex = 15;
            this.label18.Text = "구매후 몇% 수익이면 매도";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtContinueSellCnt
            // 
            this.txtContinueSellCnt.Location = new System.Drawing.Point(181, 179);
            this.txtContinueSellCnt.MaxLength = 7;
            this.txtContinueSellCnt.Name = "txtContinueSellCnt";
            this.txtContinueSellCnt.Size = new System.Drawing.Size(51, 21);
            this.txtContinueSellCnt.TabIndex = 18;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 182);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(167, 12);
            this.label13.TabIndex = 17;
            this.label13.Text = "매도신호 연속 몇번";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAllCodePageNum
            // 
            this.txtAllCodePageNum.Location = new System.Drawing.Point(184, 72);
            this.txtAllCodePageNum.MaxLength = 7;
            this.txtAllCodePageNum.Name = "txtAllCodePageNum";
            this.txtAllCodePageNum.Size = new System.Drawing.Size(51, 21);
            this.txtAllCodePageNum.TabIndex = 11;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(11, 78);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(167, 12);
            this.label21.TabIndex = 10;
            this.label21.Text = "전체종목 가져올때 몇번째";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAllCodeTtlPage
            // 
            this.txtAllCodeTtlPage.Location = new System.Drawing.Point(184, 99);
            this.txtAllCodeTtlPage.MaxLength = 7;
            this.txtAllCodeTtlPage.Name = "txtAllCodeTtlPage";
            this.txtAllCodeTtlPage.Size = new System.Drawing.Size(51, 21);
            this.txtAllCodeTtlPage.TabIndex = 13;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(11, 106);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(167, 12);
            this.label22.TabIndex = 12;
            this.label22.Text = "전체종목 가져올때 몇등분";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 429);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label11);
            this.Name = "frmConfig";
            this.Text = "frmConfig";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMdCutLine;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtIgnoreCheCnt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDays;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtContinueOrderCnt;
        private System.Windows.Forms.TextBox txtPierceHoCnt;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPowerHighLimit;
        private System.Windows.Forms.TextBox txtPowerLowLimit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMsCutLine;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtDifferenceChePower;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtMsMdRate;
        private System.Windows.Forms.TextBox txtTermLog;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAvgVolumeOverRate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtContinueSellCnt;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtSatisfyProfit;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtProfitCutOff;
        private System.Windows.Forms.TextBox txtCutoff;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCutOffMin;
        private System.Windows.Forms.TextBox txtCutOffHour;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtAllCodeTtlPage;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtAllCodePageNum;
        private System.Windows.Forms.Label label21;
    }
}