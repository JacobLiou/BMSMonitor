namespace SofarBMS.UI
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtSlaveAddress = new TextBox();
            cbbFileNumber = new ComboBox();
            cbbModeName = new ComboBox();
            btnFileTransmit = new Button();
            ckReadAll = new CheckBox();
            label4 = new Label();
            label5 = new Label();
            txtStartLocal = new TextBox();
            txtReadCount = new TextBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            lvPrintBlock = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 24);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "设备地址";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 57);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 1;
            label2.Text = "文件编号";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 87);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 2;
            label3.Text = "访问模块";
            // 
            // txtSlaveAddress
            // 
            txtSlaveAddress.Location = new Point(108, 20);
            txtSlaveAddress.Margin = new Padding(4, 4, 4, 4);
            txtSlaveAddress.Name = "txtSlaveAddress";
            txtSlaveAddress.Size = new Size(140, 23);
            txtSlaveAddress.TabIndex = 3;
            txtSlaveAddress.TextChanged += txtSlaveAddress_TextChanged;
            // 
            // cbbFileNumber
            // 
            cbbFileNumber.FormattingEnabled = true;
            cbbFileNumber.Items.AddRange(new object[] { "0：故障录波文件1\t", "1：故障录波文件2\t", "2：故障录波文件3\t", "3：5min特性数据文件\t", "4：运行日志文件\t    ", "5：历史事件文件" });
            cbbFileNumber.Location = new Point(109, 51);
            cbbFileNumber.Margin = new Padding(4, 4, 4, 4);
            cbbFileNumber.Name = "cbbFileNumber";
            cbbFileNumber.Size = new Size(140, 25);
            cbbFileNumber.TabIndex = 4;
            // 
            // cbbModeName
            // 
            cbbModeName.FormattingEnabled = true;
            cbbModeName.Items.AddRange(new object[] { "BCU", "BMU" });
            cbbModeName.Location = new Point(108, 81);
            cbbModeName.Margin = new Padding(4, 4, 4, 4);
            cbbModeName.Name = "cbbModeName";
            cbbModeName.Size = new Size(140, 25);
            cbbModeName.TabIndex = 5;
            // 
            // btnFileTransmit
            // 
            btnFileTransmit.Location = new Point(631, 51);
            btnFileTransmit.Margin = new Padding(4, 4, 4, 4);
            btnFileTransmit.Name = "btnFileTransmit";
            btnFileTransmit.Size = new Size(88, 53);
            btnFileTransmit.TabIndex = 6;
            btnFileTransmit.Text = "启动";
            btnFileTransmit.UseVisualStyleBackColor = true;
            btnFileTransmit.Click += btnFileTransmit_Click;
            // 
            // ckReadAll
            // 
            ckReadAll.AutoSize = true;
            ckReadAll.Location = new Point(257, 24);
            ckReadAll.Margin = new Padding(4, 4, 4, 4);
            ckReadAll.Name = "ckReadAll";
            ckReadAll.Size = new Size(99, 21);
            ckReadAll.TabIndex = 7;
            ckReadAll.Text = "是否读取全部";
            ckReadAll.UseVisualStyleBackColor = true;
            ckReadAll.CheckedChanged += ckReadAll_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(384, 56);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(56, 17);
            label4.TabIndex = 8;
            label4.Text = "起始位置";
            label4.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(384, 86);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 9;
            label5.Text = "读取条数";
            label5.Visible = false;
            // 
            // txtStartLocal
            // 
            txtStartLocal.Location = new Point(474, 51);
            txtStartLocal.Margin = new Padding(4, 4, 4, 4);
            txtStartLocal.Name = "txtStartLocal";
            txtStartLocal.Size = new Size(116, 23);
            txtStartLocal.TabIndex = 10;
            txtStartLocal.Visible = false;
            // 
            // txtReadCount
            // 
            txtReadCount.Location = new Point(474, 81);
            txtReadCount.Margin = new Padding(4, 4, 4, 4);
            txtReadCount.Name = "txtReadCount";
            txtReadCount.Size = new Size(116, 23);
            txtReadCount.TabIndex = 11;
            txtReadCount.Visible = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtReadCount);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtStartLocal);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(txtSlaveAddress);
            groupBox1.Controls.Add(cbbFileNumber);
            groupBox1.Controls.Add(ckReadAll);
            groupBox1.Controls.Add(cbbModeName);
            groupBox1.Controls.Add(btnFileTransmit);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Margin = new Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 4, 4, 4);
            groupBox1.Size = new Size(1277, 118);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lvPrintBlock);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(0, 118);
            groupBox2.Margin = new Padding(4, 4, 4, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 4, 4, 4);
            groupBox2.Size = new Size(1277, 582);
            groupBox2.TabIndex = 24;
            groupBox2.TabStop = false;
            // 
            // lvPrintBlock
            // 
            lvPrintBlock.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvPrintBlock.Dock = DockStyle.Fill;
            lvPrintBlock.Location = new Point(4, 20);
            lvPrintBlock.Margin = new Padding(4, 4, 4, 4);
            lvPrintBlock.Name = "lvPrintBlock";
            lvPrintBlock.Size = new Size(1269, 558);
            lvPrintBlock.TabIndex = 0;
            lvPrintBlock.UseCompatibleStateImageBehavior = false;
            lvPrintBlock.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "datetime";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "ID";
            columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Data";
            columnHeader3.Width = 500;
            // 
            // CBSFileTransmit
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Margin = new Padding(4, 4, 4, 4);
            Name = "CBSFileTransmit";
            Size = new Size(1277, 700);
            Load += CBSFileTransmit_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
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
    }
}
