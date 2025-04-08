namespace SofarBMS.UI
{
    partial class CBSFileTransmit_BCU
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSlaveAddress = new System.Windows.Forms.TextBox();
            this.cbbFileNumber = new System.Windows.Forms.ComboBox();
            this.cbbModeName = new System.Windows.Forms.ComboBox();
            this.btnFileTransmit = new System.Windows.Forms.Button();
            this.ckReadAll = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtStartLocal = new System.Windows.Forms.TextBox();
            this.txtReadCount = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSubDeviceAddress = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvPrintBlock = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "BCU设备地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 88);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "文件编号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 130);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "访问模块";
            // 
            // txtSlaveAddress
            // 
            this.txtSlaveAddress.Location = new System.Drawing.Point(108, 37);
            this.txtSlaveAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSlaveAddress.Name = "txtSlaveAddress";
            this.txtSlaveAddress.Size = new System.Drawing.Size(140, 23);
            this.txtSlaveAddress.TabIndex = 3;
            this.txtSlaveAddress.TextChanged += new System.EventHandler(this.txtSlaveAddress_TextChanged);
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
            this.cbbFileNumber.Location = new System.Drawing.Point(108, 82);
            this.cbbFileNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbFileNumber.Name = "cbbFileNumber";
            this.cbbFileNumber.Size = new System.Drawing.Size(140, 25);
            this.cbbFileNumber.TabIndex = 4;
            // 
            // cbbModeName
            // 
            this.cbbModeName.FormattingEnabled = true;
            this.cbbModeName.Items.AddRange(new object[] {
            "BCU",
            "BMU"});
            this.cbbModeName.Location = new System.Drawing.Point(108, 125);
            this.cbbModeName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbModeName.Name = "cbbModeName";
            this.cbbModeName.Size = new System.Drawing.Size(140, 25);
            this.cbbModeName.TabIndex = 5;
            // 
            // btnFileTransmit
            // 
            this.btnFileTransmit.Location = new System.Drawing.Point(727, 75);
            this.btnFileTransmit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFileTransmit.Name = "btnFileTransmit";
            this.btnFileTransmit.Size = new System.Drawing.Size(88, 72);
            this.btnFileTransmit.TabIndex = 6;
            this.btnFileTransmit.Text = "启动";
            this.btnFileTransmit.UseVisualStyleBackColor = true;
            this.btnFileTransmit.Click += new System.EventHandler(this.btnFileTransmit_Click);
            // 
            // ckReadAll
            // 
            this.ckReadAll.AutoSize = true;
            this.ckReadAll.Location = new System.Drawing.Point(265, 85);
            this.ckReadAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckReadAll.Name = "ckReadAll";
            this.ckReadAll.Size = new System.Drawing.Size(99, 21);
            this.ckReadAll.TabIndex = 7;
            this.ckReadAll.Text = "是否读取全部";
            this.ckReadAll.UseVisualStyleBackColor = true;
            this.ckReadAll.CheckedChanged += new System.EventHandler(this.ckReadAll_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(380, 130);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "起始位置";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(383, 88);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "读取条数";
            // 
            // txtStartLocal
            // 
            this.txtStartLocal.Location = new System.Drawing.Point(449, 125);
            this.txtStartLocal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtStartLocal.Name = "txtStartLocal";
            this.txtStartLocal.Size = new System.Drawing.Size(116, 23);
            this.txtStartLocal.TabIndex = 10;
            this.txtStartLocal.Visible = false;
            // 
            // txtReadCount
            // 
            this.txtReadCount.Location = new System.Drawing.Point(449, 82);
            this.txtReadCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtReadCount.Name = "txtReadCount";
            this.txtReadCount.Size = new System.Drawing.Size(116, 23);
            this.txtReadCount.TabIndex = 11;
            this.txtReadCount.TextChanged += new System.EventHandler(this.txtReadCount_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtSubDeviceAddress);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtReadCount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtStartLocal);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSlaveAddress);
            this.groupBox1.Controls.Add(this.cbbFileNumber);
            this.groupBox1.Controls.Add(this.ckReadAll);
            this.groupBox1.Controls.Add(this.cbbModeName);
            this.groupBox1.Controls.Add(this.btnFileTransmit);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1477, 184);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(262, 45);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "子设备BMU地址";
            // 
            // txtSubDeviceAddress
            // 
            this.txtSubDeviceAddress.Location = new System.Drawing.Point(365, 37);
            this.txtSubDeviceAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSubDeviceAddress.Name = "txtSubDeviceAddress";
            this.txtSubDeviceAddress.Size = new System.Drawing.Size(140, 23);
            this.txtSubDeviceAddress.TabIndex = 13;
            this.txtSubDeviceAddress.TextChanged += new System.EventHandler(this.txtSubDeviceAddress_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvPrintBlock);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 184);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(1477, 833);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            // 
            // lvPrintBlock
            // 
            this.lvPrintBlock.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvPrintBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPrintBlock.Location = new System.Drawing.Point(4, 20);
            this.lvPrintBlock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lvPrintBlock.Name = "lvPrintBlock";
            this.lvPrintBlock.Size = new System.Drawing.Size(1469, 809);
            this.lvPrintBlock.TabIndex = 0;
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
            // CBSFileTransmit_BCU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CBSFileTransmit_BCU";
            this.Size = new System.Drawing.Size(1477, 1017);
            this.Load += new System.EventHandler(this.CBSFileTransmit_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSlaveAddress;
        private System.Windows.Forms.ComboBox cbbFileNumber;
        private System.Windows.Forms.ComboBox cbbModeName;
        private System.Windows.Forms.Button btnFileTransmit;
        private System.Windows.Forms.CheckBox ckReadAll;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtStartLocal;
        private System.Windows.Forms.TextBox txtReadCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvPrintBlock;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSubDeviceAddress;
    }
}
