namespace eb
{
    partial class frmSimulation
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.spdInterest = new FarPoint.Win.Spread.FpSpread();
            this.spsInterest = new FarPoint.Win.Spread.SheetView();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnSimulation = new System.Windows.Forms.ToolStripButton();
            this.chkAllFiles = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spdInterest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spsInterest)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSimulation});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1031, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
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
            this.spdInterest.Size = new System.Drawing.Size(1031, 429);
            this.spdInterest.TabIndex = 1;
            // 
            // spsInterest
            // 
            this.spsInterest.Reset();
            this.spsInterest.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.spsInterest.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.spsInterest.ColumnCount = 10;
            this.spsInterest.RowCount = 0;
            this.spsInterest.ActiveRowIndex = -1;
            this.spsInterest.ColumnHeader.Cells.Get(0, 0).Value = "날짜";
            this.spsInterest.ColumnHeader.Cells.Get(0, 1).Value = "시간";
            this.spsInterest.ColumnHeader.Cells.Get(0, 2).Value = "매수여부";
            this.spsInterest.ColumnHeader.Cells.Get(0, 3).Value = "금액";
            this.spsInterest.ColumnHeader.Cells.Get(0, 4).Value = "비율";
            this.spsInterest.ColumnHeader.Cells.Get(0, 5).Value = "체결강도";
            this.spsInterest.ColumnHeader.Cells.Get(0, 6).Value = "매도%-매수%";
            this.spsInterest.ColumnHeader.Cells.Get(0, 7).Value = "매도액-매수액";
            this.spsInterest.ColumnHeader.Cells.Get(0, 8).Value = "이익/손해금액";
            this.spsInterest.ColumnHeader.Cells.Get(0, 9).Value = "누적";
            this.spsInterest.Columns.Get(0).Label = "날짜";
            this.spsInterest.Columns.Get(0).Width = 69F;
            this.spsInterest.Columns.Get(2).Label = "매수여부";
            this.spsInterest.Columns.Get(2).Width = 61F;
            this.spsInterest.Columns.Get(3).Label = "금액";
            this.spsInterest.Columns.Get(3).Width = 70F;
            this.spsInterest.Columns.Get(4).Label = "비율";
            this.spsInterest.Columns.Get(4).Width = 44F;
            this.spsInterest.Columns.Get(5).CellType = textCellType1;
            this.spsInterest.Columns.Get(5).Label = "체결강도";
            this.spsInterest.Columns.Get(5).Locked = true;
            this.spsInterest.Columns.Get(5).Width = 55F;
            this.spsInterest.Columns.Get(6).Label = "매도%-매수%";
            this.spsInterest.Columns.Get(6).Width = 86F;
            this.spsInterest.Columns.Get(7).Label = "매도액-매수액";
            this.spsInterest.Columns.Get(7).Width = 88F;
            this.spsInterest.Columns.Get(8).Label = "이익/손해금액";
            this.spsInterest.Columns.Get(8).Width = 97F;
            this.spsInterest.Columns.Get(9).Label = "누적";
            this.spsInterest.Columns.Get(9).Width = 91F;
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
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(130, 2);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(81, 21);
            this.dtpTo.TabIndex = 12;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(43, 2);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(81, 21);
            this.dtpFrom.TabIndex = 11;
            // 
            // btnSimulation
            // 
            this.btnSimulation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSimulation.Image = global::eb.Properties.Resources.ShipPlan_04;
            this.btnSimulation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSimulation.Name = "btnSimulation";
            this.btnSimulation.Size = new System.Drawing.Size(23, 22);
            this.btnSimulation.Text = "Do Simulation";
            this.btnSimulation.Click += new System.EventHandler(this.btnSimulation_Click);
            // 
            // chkAllFiles
            // 
            this.chkAllFiles.AutoSize = true;
            this.chkAllFiles.Location = new System.Drawing.Point(217, 3);
            this.chkAllFiles.Name = "chkAllFiles";
            this.chkAllFiles.Size = new System.Drawing.Size(69, 16);
            this.chkAllFiles.TabIndex = 13;
            this.chkAllFiles.Text = "All Files";
            this.chkAllFiles.UseVisualStyleBackColor = true;
            // 
            // frmSimulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 454);
            this.Controls.Add(this.chkAllFiles);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.spdInterest);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmSimulation";
            this.Text = "frmSimulation";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spdInterest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spsInterest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private FarPoint.Win.Spread.FpSpread spdInterest;
        private FarPoint.Win.Spread.SheetView spsInterest;
        private System.Windows.Forms.ToolStripButton btnSimulation;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.CheckBox chkAllFiles;
    }
}