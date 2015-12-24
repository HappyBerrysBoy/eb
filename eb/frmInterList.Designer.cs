namespace eb
{
    partial class frmInterList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInterList));
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnDoLog = new System.Windows.Forms.ToolStripButton();
            this.btnConfig = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.btnSelectQuery = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.spdInterest = new FarPoint.Win.Spread.FpSpread();
            this.spsInterest = new FarPoint.Win.Spread.SheetView();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.btnLoadInterestList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDel = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.spdLog = new FarPoint.Win.Spread.FpSpread();
            this.spsLog = new FarPoint.Win.Spread.SheetView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnShowLog = new System.Windows.Forms.ToolStripButton();
            this.btnFindPoint = new System.Windows.Forms.ToolStripButton();
            this.btnSimulation = new System.Windows.Forms.ToolStripButton();
            this.stsBar = new System.Windows.Forms.StatusStrip();
            this.lblSTS = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSum = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmbQueryKind = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spdInterest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spsInterest)).BeginInit();
            this.toolStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spdLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spsLog)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.stsBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDoLog,
            this.btnConfig,
            this.toolStripLabel1,
            this.btnSelectQuery});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1259, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnDoLog
            // 
            this.btnDoLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDoLog.Enabled = false;
            this.btnDoLog.Image = ((System.Drawing.Image)(resources.GetObject("btnDoLog.Image")));
            this.btnDoLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDoLog.Name = "btnDoLog";
            this.btnDoLog.Size = new System.Drawing.Size(23, 22);
            this.btnDoLog.Text = "Do";
            this.btnDoLog.Click += new System.EventHandler(this.btnDoLog_Click);
            // 
            // btnConfig
            // 
            this.btnConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnConfig.Image")));
            this.btnConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(23, 22);
            this.btnConfig.Text = "Setting Config";
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(195, 22);
            this.toolStripLabel1.Text = "                                               ";
            // 
            // btnSelectQuery
            // 
            this.btnSelectQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelectQuery.Enabled = false;
            this.btnSelectQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectQuery.Image")));
            this.btnSelectQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectQuery.Name = "btnSelectQuery";
            this.btnSelectQuery.Size = new System.Drawing.Size(23, 22);
            this.btnSelectQuery.Text = "Select Query";
            this.btnSelectQuery.Click += new System.EventHandler(this.btnSelectQuery_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.spdInterest);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.spdLog);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(1259, 514);
            this.splitContainer1.SplitterDistance = 384;
            this.splitContainer1.TabIndex = 7;
            // 
            // spdInterest
            // 
            this.spdInterest.AccessibleDescription = "";
            this.spdInterest.BackColor = System.Drawing.SystemColors.Control;
            this.spdInterest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spdInterest.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.spdInterest.Location = new System.Drawing.Point(0, 25);
            this.spdInterest.Name = "spdInterest";
            this.spdInterest.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.spdInterest.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.spsInterest});
            this.spdInterest.Size = new System.Drawing.Size(384, 489);
            this.spdInterest.TabIndex = 0;
            // 
            // spsInterest
            // 
            this.spsInterest.Reset();
            this.spsInterest.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.spsInterest.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.spsInterest.ColumnCount = 6;
            this.spsInterest.RowCount = 0;
            this.spsInterest.ActiveRowIndex = -1;
            this.spsInterest.ColumnHeader.Cells.Get(0, 0).Value = "구분";
            this.spsInterest.ColumnHeader.Cells.Get(0, 1).Value = "코드";
            this.spsInterest.ColumnHeader.Cells.Get(0, 2).Value = "이름";
            this.spsInterest.ColumnHeader.Cells.Get(0, 3).Value = "사용";
            this.spsInterest.ColumnHeader.Cells.Get(0, 4).Value = "비율";
            this.spsInterest.ColumnHeader.Cells.Get(0, 5).Value = "평균거래량";
            this.spsInterest.Columns.Get(0).Label = "구분";
            this.spsInterest.Columns.Get(0).Width = 39F;
            this.spsInterest.Columns.Get(2).Label = "이름";
            this.spsInterest.Columns.Get(2).Width = 85F;
            this.spsInterest.Columns.Get(3).Label = "사용";
            this.spsInterest.Columns.Get(3).Width = 33F;
            this.spsInterest.Columns.Get(4).Label = "비율";
            this.spsInterest.Columns.Get(4).Width = 34F;
            this.spsInterest.Columns.Get(5).CellType = textCellType1;
            this.spsInterest.Columns.Get(5).Label = "평균거래량";
            this.spsInterest.Columns.Get(5).Locked = true;
            this.spsInterest.Columns.Get(5).Width = 69F;
            this.spsInterest.DefaultStyle.CellType = textCellType2;
            this.spsInterest.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.spsInterest.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.spsInterest.DefaultStyle.Parent = "DataAreaDefault";
            this.spsInterest.DefaultStyle.Renderer = textCellType2;
            this.spsInterest.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.spsInterest.RowHeader.Columns.Default.Resizable = false;
            this.spsInterest.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            this.spdInterest.SetActiveViewport(0, -1, 0);
            // 
            // toolStrip3
            // 
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoadInterestList,
            this.toolStripSeparator1,
            this.btnAdd,
            this.btnDel,
            this.btnSave});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(384, 25);
            this.toolStrip3.TabIndex = 1;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // btnLoadInterestList
            // 
            this.btnLoadInterestList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLoadInterestList.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadInterestList.Image")));
            this.btnLoadInterestList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadInterestList.Name = "btnLoadInterestList";
            this.btnLoadInterestList.Size = new System.Drawing.Size(23, 22);
            this.btnLoadInterestList.Text = "Load Interest List";
            this.btnLoadInterestList.Click += new System.EventHandler(this.btnLoadInterestList_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 22);
            this.btnAdd.Text = "Add Interest";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDel.Image = ((System.Drawing.Image)(resources.GetObject("btnDel.Image")));
            this.btnDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(23, 22);
            this.btnDel.Text = "Delete Interest";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "Save Interest List";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // spdLog
            // 
            this.spdLog.AccessibleDescription = "";
            this.spdLog.BackColor = System.Drawing.SystemColors.Control;
            this.spdLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spdLog.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.spdLog.Location = new System.Drawing.Point(0, 25);
            this.spdLog.Name = "spdLog";
            this.spdLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.spdLog.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.spsLog});
            this.spdLog.Size = new System.Drawing.Size(871, 489);
            this.spdLog.TabIndex = 7;
            this.spdLog.MouseUp += new System.Windows.Forms.MouseEventHandler(this.spdLog_MouseUp);
            // 
            // spsLog
            // 
            this.spsLog.Reset();
            this.spsLog.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.spsLog.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.spsLog.ColumnCount = 26;
            this.spsLog.RowCount = 0;
            this.spsLog.ActiveRowIndex = -1;
            this.spsLog.ColumnHeader.Cells.Get(0, 0).Value = "Code";
            this.spsLog.ColumnHeader.Cells.Get(0, 1).Value = "체결시간";
            this.spsLog.ColumnHeader.Cells.Get(0, 2).Value = "전일대";
            this.spsLog.ColumnHeader.Cells.Get(0, 3).Value = "변동금액";
            this.spsLog.ColumnHeader.Cells.Get(0, 4).Value = "비율";
            this.spsLog.ColumnHeader.Cells.Get(0, 5).Value = "현재가격";
            this.spsLog.ColumnHeader.Cells.Get(0, 6).Value = "시가";
            this.spsLog.ColumnHeader.Cells.Get(0, 7).Value = "고가";
            this.spsLog.ColumnHeader.Cells.Get(0, 8).Value = "저가";
            this.spsLog.ColumnHeader.Cells.Get(0, 9).Value = "매수/도";
            this.spsLog.ColumnHeader.Cells.Get(0, 10).Value = "체결";
            this.spsLog.ColumnHeader.Cells.Get(0, 11).Value = "누적거래량";
            this.spsLog.ColumnHeader.Cells.Get(0, 12).Value = "누적거래대금";
            this.spsLog.ColumnHeader.Cells.Get(0, 13).Value = "매도누적체결량";
            this.spsLog.ColumnHeader.Cells.Get(0, 14).Value = "매도누적체결건수";
            this.spsLog.ColumnHeader.Cells.Get(0, 15).Value = "매수누적체결량";
            this.spsLog.ColumnHeader.Cells.Get(0, 16).Value = "매수누적체결건수";
            this.spsLog.ColumnHeader.Cells.Get(0, 17).Value = "체결강도";
            this.spsLog.ColumnHeader.Cells.Get(0, 18).Value = "매도호가";
            this.spsLog.ColumnHeader.Cells.Get(0, 19).Value = "매수호가";
            this.spsLog.ColumnHeader.Cells.Get(0, 20).Value = "장구분";
            this.spsLog.ColumnHeader.Cells.Get(0, 21).Value = "전일동시간대거래량";
            this.spsLog.ColumnHeader.Cells.Get(0, 22).Value = "매수/도주문";
            this.spsLog.ColumnHeader.Cells.Get(0, 23).Value = "수량";
            this.spsLog.ColumnHeader.Cells.Get(0, 24).Value = "가격";
            this.spsLog.ColumnHeader.Cells.Get(0, 25).Value = "세금";
            this.spsLog.Columns.Get(0).Label = "Code";
            this.spsLog.Columns.Get(0).Locked = true;
            this.spsLog.Columns.Get(1).Label = "체결시간";
            this.spsLog.Columns.Get(1).Locked = true;
            this.spsLog.Columns.Get(2).Label = "전일대";
            this.spsLog.Columns.Get(2).Locked = true;
            this.spsLog.Columns.Get(3).Label = "변동금액";
            this.spsLog.Columns.Get(3).Locked = true;
            this.spsLog.Columns.Get(4).Label = "비율";
            this.spsLog.Columns.Get(4).Locked = true;
            this.spsLog.Columns.Get(5).Label = "현재가격";
            this.spsLog.Columns.Get(5).Locked = true;
            this.spsLog.Columns.Get(6).Label = "시가";
            this.spsLog.Columns.Get(6).Locked = true;
            this.spsLog.Columns.Get(7).Label = "고가";
            this.spsLog.Columns.Get(7).Locked = true;
            this.spsLog.Columns.Get(8).Label = "저가";
            this.spsLog.Columns.Get(8).Locked = true;
            this.spsLog.Columns.Get(9).Label = "매수/도";
            this.spsLog.Columns.Get(9).Locked = true;
            this.spsLog.Columns.Get(10).Label = "체결";
            this.spsLog.Columns.Get(10).Locked = true;
            this.spsLog.Columns.Get(11).Label = "누적거래량";
            this.spsLog.Columns.Get(11).Locked = true;
            this.spsLog.Columns.Get(12).Label = "누적거래대금";
            this.spsLog.Columns.Get(12).Locked = true;
            this.spsLog.Columns.Get(13).Label = "매도누적체결량";
            this.spsLog.Columns.Get(13).Locked = true;
            this.spsLog.Columns.Get(14).Label = "매도누적체결건수";
            this.spsLog.Columns.Get(14).Locked = true;
            this.spsLog.Columns.Get(15).Label = "매수누적체결량";
            this.spsLog.Columns.Get(15).Locked = true;
            this.spsLog.Columns.Get(16).Label = "매수누적체결건수";
            this.spsLog.Columns.Get(16).Locked = true;
            this.spsLog.Columns.Get(17).Label = "체결강도";
            this.spsLog.Columns.Get(17).Locked = true;
            this.spsLog.Columns.Get(18).Label = "매도호가";
            this.spsLog.Columns.Get(18).Locked = true;
            this.spsLog.Columns.Get(19).Label = "매수호가";
            this.spsLog.Columns.Get(19).Locked = true;
            this.spsLog.Columns.Get(20).Label = "장구분";
            this.spsLog.Columns.Get(20).Locked = true;
            this.spsLog.Columns.Get(21).Label = "전일동시간대거래량";
            this.spsLog.Columns.Get(21).Locked = true;
            this.spsLog.Columns.Get(22).Label = "매수/도주문";
            this.spsLog.Columns.Get(22).Locked = true;
            this.spsLog.Columns.Get(23).Label = "수량";
            this.spsLog.Columns.Get(23).Locked = true;
            this.spsLog.Columns.Get(24).Label = "가격";
            this.spsLog.Columns.Get(24).Locked = true;
            this.spsLog.Columns.Get(25).Label = "세금";
            this.spsLog.Columns.Get(25).Locked = true;
            this.spsLog.DefaultStyle.CellType = textCellType3;
            this.spsLog.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.spsLog.DefaultStyle.Parent = "DataAreaDefault";
            this.spsLog.DefaultStyle.Renderer = textCellType3;
            this.spsLog.RowHeader.Columns.Default.Resizable = false;
            this.spsLog.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            this.spdLog.SetViewportLeftColumn(0, 0, 7);
            this.spdLog.SetActiveViewport(0, -1, 0);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnShowLog,
            this.btnFindPoint,
            this.btnSimulation});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(871, 25);
            this.toolStrip2.TabIndex = 8;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnShowLog
            // 
            this.btnShowLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowLog.Image = ((System.Drawing.Image)(resources.GetObject("btnShowLog.Image")));
            this.btnShowLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(23, 22);
            this.btnShowLog.Text = "Inquiry Logs";
            this.btnShowLog.Click += new System.EventHandler(this.btnShowLog_Click);
            // 
            // btnFindPoint
            // 
            this.btnFindPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFindPoint.Image = ((System.Drawing.Image)(resources.GetObject("btnFindPoint.Image")));
            this.btnFindPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFindPoint.Name = "btnFindPoint";
            this.btnFindPoint.Size = new System.Drawing.Size(23, 22);
            this.btnFindPoint.Text = "Find Point";
            this.btnFindPoint.Click += new System.EventHandler(this.btnFindPoint_Click);
            // 
            // btnSimulation
            // 
            this.btnSimulation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSimulation.Image = ((System.Drawing.Image)(resources.GetObject("btnSimulation.Image")));
            this.btnSimulation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSimulation.Name = "btnSimulation";
            this.btnSimulation.Size = new System.Drawing.Size(23, 22);
            this.btnSimulation.Text = "Simulation";
            this.btnSimulation.Click += new System.EventHandler(this.btnSimulation_Click);
            // 
            // stsBar
            // 
            this.stsBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSTS,
            this.toolStripStatusLabel1,
            this.lblSum});
            this.stsBar.Location = new System.Drawing.Point(0, 542);
            this.stsBar.Name = "stsBar";
            this.stsBar.Size = new System.Drawing.Size(1259, 22);
            this.stsBar.TabIndex = 8;
            this.stsBar.Text = "statusStrip1";
            // 
            // lblSTS
            // 
            this.lblSTS.Name = "lblSTS";
            this.lblSTS.Size = new System.Drawing.Size(89, 17);
            this.lblSTS.Text = "합계/평균/기타";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel1.Text = "|||";
            // 
            // lblSum
            // 
            this.lblSum.Name = "lblSum";
            this.lblSum.Size = new System.Drawing.Size(35, 17);
            this.lblSum.Text = "value";
            // 
            // cmbQueryKind
            // 
            this.cmbQueryKind.FormattingEnabled = true;
            this.cmbQueryKind.Location = new System.Drawing.Point(177, 3);
            this.cmbQueryKind.Name = "cmbQueryKind";
            this.cmbQueryKind.Size = new System.Drawing.Size(118, 20);
            this.cmbQueryKind.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "조회종류";
            // 
            // frmInterList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 564);
            this.Controls.Add(this.cmbQueryKind);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stsBar);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmInterList";
            this.Text = "eb Manager";
            this.Load += new System.EventHandler(this.frmInterList_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spdInterest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spsInterest)).EndInit();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spdLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spsLog)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.stsBar.ResumeLayout(false);
            this.stsBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnDoLog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private FarPoint.Win.Spread.FpSpread spdInterest;
        private FarPoint.Win.Spread.SheetView spsInterest;
        private FarPoint.Win.Spread.FpSpread spdLog;
        private FarPoint.Win.Spread.SheetView spsLog;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton btnSelectQuery;
        private System.Windows.Forms.ToolStripButton btnConfig;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnFindPoint;
        private System.Windows.Forms.StatusStrip stsBar;
        private System.Windows.Forms.ToolStripStatusLabel lblSTS;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDel;
        private System.Windows.Forms.ToolStripButton btnLoadInterestList;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnShowLog;
        private System.Windows.Forms.ComboBox cmbQueryKind;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblSum;
        private System.Windows.Forms.ToolStripButton btnSimulation;
    }
}