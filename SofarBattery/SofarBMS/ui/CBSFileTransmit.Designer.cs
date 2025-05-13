namespace SofarBMS.ui
{
    partial class CBSFileTransmit
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBMUAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBCUAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReadCount = new System.Windows.Forms.TextBox();
            this.txtStartLocal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ckReadAll = new System.Windows.Forms.CheckBox();
            this.btnFileTransmit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbFileNumber = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbModeName = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvPrintBlock = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtBMUAddress);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtBCUAddress);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtReadCount);
            this.panel1.Controls.Add(this.txtStartLocal);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.ckReadAll);
            this.panel1.Controls.Add(this.btnFileTransmit);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbbFileNumber);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbbModeName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1209, 125);
            this.panel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 89);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "BMU设备地址";
            // 
            // txtBMUAddress
            // 
            this.txtBMUAddress.Location = new System.Drawing.Point(116, 86);
            this.txtBMUAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtBMUAddress.Name = "txtBMUAddress";
            this.txtBMUAddress.Size = new System.Drawing.Size(140, 23);
            this.txtBMUAddress.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "BCU设备地址";
            // 
            // txtBCUAddress
            // 
            this.txtBCUAddress.Location = new System.Drawing.Point(116, 53);
            this.txtBCUAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtBCUAddress.Name = "txtBCUAddress";
            this.txtBCUAddress.Size = new System.Drawing.Size(140, 23);
            this.txtBCUAddress.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(286, 89);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "起始位置";
            // 
            // txtReadCount
            // 
            this.txtReadCount.Location = new System.Drawing.Point(355, 53);
            this.txtReadCount.Margin = new System.Windows.Forms.Padding(4);
            this.txtReadCount.Name = "txtReadCount";
            this.txtReadCount.Size = new System.Drawing.Size(140, 23);
            this.txtReadCount.TabIndex = 15;
            // 
            // txtStartLocal
            // 
            this.txtStartLocal.Location = new System.Drawing.Point(355, 86);
            this.txtStartLocal.Margin = new System.Windows.Forms.Padding(4);
            this.txtStartLocal.Name = "txtStartLocal";
            this.txtStartLocal.Size = new System.Drawing.Size(140, 23);
            this.txtStartLocal.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(286, 56);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "读取条数";
            // 
            // ckReadAll
            // 
            this.ckReadAll.AutoSize = true;
            this.ckReadAll.Location = new System.Drawing.Point(503, 20);
            this.ckReadAll.Margin = new System.Windows.Forms.Padding(4);
            this.ckReadAll.Name = "ckReadAll";
            this.ckReadAll.Size = new System.Drawing.Size(99, 21);
            this.ckReadAll.TabIndex = 11;
            this.ckReadAll.Text = "是否读取全部";
            this.ckReadAll.UseVisualStyleBackColor = true;
            this.ckReadAll.CheckedChanged += new System.EventHandler(this.ckReadAll_CheckedChanged);
            // 
            // btnFileTransmit
            // 
            this.btnFileTransmit.Location = new System.Drawing.Point(609, 17);
            this.btnFileTransmit.Name = "btnFileTransmit";
            this.btnFileTransmit.Size = new System.Drawing.Size(75, 27);
            this.btnFileTransmit.TabIndex = 10;
            this.btnFileTransmit.Text = "启动";
            this.btnFileTransmit.UseVisualStyleBackColor = true;
            this.btnFileTransmit.Click += new System.EventHandler(this.btnFileTransmit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "文件编号";
            // 
            // cbbFileNumber
            // 
            this.cbbFileNumber.FormattingEnabled = true;
            this.cbbFileNumber.Items.AddRange(new object[] {
            "0：BCU通讯板故障录波",
            "1：BCU功率板故障录波",
            "2：保留\t",
            "3：5min特性数据文件\t",
            "4：运行日志文件\t    ",
            "5：历史事件文件"});
            this.cbbFileNumber.Location = new System.Drawing.Point(355, 18);
            this.cbbFileNumber.Margin = new System.Windows.Forms.Padding(4);
            this.cbbFileNumber.Name = "cbbFileNumber";
            this.cbbFileNumber.Size = new System.Drawing.Size(140, 25);
            this.cbbFileNumber.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "访问模块";
            // 
            // cbbModeName
            // 
            this.cbbModeName.FormattingEnabled = true;
            this.cbbModeName.Items.AddRange(new object[] {
            "BCU",
            "BMU"});
            this.cbbModeName.Location = new System.Drawing.Point(116, 18);
            this.cbbModeName.Margin = new System.Windows.Forms.Padding(4);
            this.cbbModeName.Name = "cbbModeName";
            this.cbbModeName.Size = new System.Drawing.Size(140, 25);
            this.cbbModeName.TabIndex = 7;
            this.cbbModeName.SelectedIndexChanged += new System.EventHandler(this.cbbModeName_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lvPrintBlock);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 125);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1209, 643);
            this.panel2.TabIndex = 1;
            // 
            // lvPrintBlock
            // 
            this.lvPrintBlock.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvPrintBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPrintBlock.Location = new System.Drawing.Point(0, 0);
            this.lvPrintBlock.Margin = new System.Windows.Forms.Padding(4);
            this.lvPrintBlock.Name = "lvPrintBlock";
            this.lvPrintBlock.Size = new System.Drawing.Size(1209, 643);
            this.lvPrintBlock.TabIndex = 1;
            this.lvPrintBlock.UseCompatibleStateImageBehavior = false;
            this.lvPrintBlock.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "datetime";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ID";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Data";
            this.columnHeader3.Width = 500;
            // 
            // CBSFileTransmit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "CBSFileTransmit";
            this.Size = new System.Drawing.Size(1209, 768);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label label3;
        private ComboBox cbbModeName;
        private Label label2;
        private ComboBox cbbFileNumber;
        private Button btnFileTransmit;
        private CheckBox ckReadAll;
        private Label label4;
        private TextBox txtReadCount;
        private TextBox txtStartLocal;
        private Label label5;
        private Label label6;
        private TextBox txtBMUAddress;
        private Label label1;
        private TextBox txtBCUAddress;
        private Panel panel2;
        private ListView lvPrintBlock;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
    }
}
