namespace SofarBMS.UI
{
    partial class CBSUpgradeControl
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
            lblUpgrade_07 = new Label();
            groupBox2 = new GroupBox();
            txtFD = new TextBox();
            txtFC = new TextBox();
            cbbChiprole_val = new ComboBox();
            txtSlaveAddress = new TextBox();
            cbbChipcode = new ComboBox();
            progressBar1 = new ProgressBar();
            lblUpgrade_02 = new Label();
            cbbChiprole = new ComboBox();
            btnUpgrade_03 = new Button();
            lblUpgrade_01 = new Label();
            txtPath = new TextBox();
            lblUpgrade_05 = new Label();
            btnUpgrade_04 = new Button();
            dateTimePicker1 = new DateTimePicker();
            ckUpgrade_06 = new CheckBox();
            groupBox1 = new GroupBox();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblUpgrade_07
            // 
            lblUpgrade_07.AutoSize = true;
            lblUpgrade_07.Location = new Point(27, 97);
            lblUpgrade_07.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_07.Name = "lblUpgrade_07";
            lblUpgrade_07.Size = new Size(56, 17);
            lblUpgrade_07.TabIndex = 25;
            lblUpgrade_07.Text = "升级类型";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtFD);
            groupBox2.Controls.Add(txtFC);
            groupBox2.Controls.Add(cbbChiprole_val);
            groupBox2.Controls.Add(txtSlaveAddress);
            groupBox2.Controls.Add(cbbChipcode);
            groupBox2.Controls.Add(progressBar1);
            groupBox2.Controls.Add(lblUpgrade_02);
            groupBox2.Controls.Add(cbbChiprole);
            groupBox2.Controls.Add(btnUpgrade_03);
            groupBox2.Controls.Add(lblUpgrade_01);
            groupBox2.Controls.Add(txtPath);
            groupBox2.Controls.Add(lblUpgrade_05);
            groupBox2.Controls.Add(lblUpgrade_07);
            groupBox2.Controls.Add(btnUpgrade_04);
            groupBox2.Controls.Add(dateTimePicker1);
            groupBox2.Controls.Add(ckUpgrade_06);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 0);
            groupBox2.Margin = new Padding(4, 4, 4, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 4, 4, 4);
            groupBox2.Size = new Size(1237, 149);
            groupBox2.TabIndex = 24;
            groupBox2.TabStop = false;
            groupBox2.Text = "升级固件";
            // 
            // txtFD
            // 
            txtFD.Location = new Point(1024, 89);
            txtFD.Margin = new Padding(4, 4, 4, 4);
            txtFD.Name = "txtFD";
            txtFD.Size = new Size(79, 23);
            txtFD.TabIndex = 35;
            txtFD.Text = "3";
            // 
            // txtFC
            // 
            txtFC.Location = new Point(941, 91);
            txtFC.Margin = new Padding(4, 4, 4, 4);
            txtFC.Name = "txtFC";
            txtFC.Size = new Size(79, 23);
            txtFC.TabIndex = 34;
            txtFC.Text = "20";
            // 
            // cbbChiprole_val
            // 
            cbbChiprole_val.FormattingEnabled = true;
            cbbChiprole_val.Items.AddRange(new object[] { "0x24", "0x2D" });
            cbbChiprole_val.Location = new Point(574, 91);
            cbbChiprole_val.Margin = new Padding(4, 4, 4, 4);
            cbbChiprole_val.Name = "cbbChiprole_val";
            cbbChiprole_val.Size = new Size(116, 25);
            cbbChiprole_val.TabIndex = 33;
            cbbChiprole_val.SelectedIndexChanged += cbbChiprole_val_SelectedIndexChanged;
            // 
            // txtSlaveAddress
            // 
            txtSlaveAddress.Location = new Point(697, 91);
            txtSlaveAddress.Margin = new Padding(4, 4, 4, 4);
            txtSlaveAddress.Name = "txtSlaveAddress";
            txtSlaveAddress.ReadOnly = true;
            txtSlaveAddress.Size = new Size(87, 23);
            txtSlaveAddress.TabIndex = 32;
            // 
            // cbbChipcode
            // 
            cbbChipcode.FormattingEnabled = true;
            cbbChipcode.Items.AddRange(new object[] { "E0", "S3", "N2" });
            cbbChipcode.Location = new Point(792, 91);
            cbbChipcode.Margin = new Padding(4, 4, 4, 4);
            cbbChipcode.Name = "cbbChipcode";
            cbbChipcode.Size = new Size(116, 25);
            cbbChipcode.TabIndex = 30;
            cbbChipcode.SelectedIndexChanged += cbbChipcode_SelectedIndexChanged;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(128, 55);
            progressBar1.Margin = new Padding(4, 4, 4, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(975, 25);
            progressBar1.TabIndex = 3;
            // 
            // lblUpgrade_02
            // 
            lblUpgrade_02.AutoSize = true;
            lblUpgrade_02.Location = new Point(27, 63);
            lblUpgrade_02.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_02.Name = "lblUpgrade_02";
            lblUpgrade_02.Size = new Size(56, 17);
            lblUpgrade_02.TabIndex = 24;
            lblUpgrade_02.Text = "升级进度";
            // 
            // cbbChiprole
            // 
            cbbChiprole.FormattingEnabled = true;
            cbbChiprole.Items.AddRange(new object[] { "BCU", "BMU" });
            cbbChiprole.Location = new Point(425, 91);
            cbbChiprole.Margin = new Padding(4, 4, 4, 4);
            cbbChiprole.Name = "cbbChiprole";
            cbbChiprole.Size = new Size(140, 25);
            cbbChiprole.TabIndex = 29;
            cbbChiprole.SelectedIndexChanged += cbbChiprole_SelectedIndexChanged;
            // 
            // btnUpgrade_03
            // 
            btnUpgrade_03.Location = new Point(1111, 21);
            btnUpgrade_03.Margin = new Padding(4, 4, 4, 4);
            btnUpgrade_03.Name = "btnUpgrade_03";
            btnUpgrade_03.Size = new Size(88, 33);
            btnUpgrade_03.TabIndex = 21;
            btnUpgrade_03.Text = "导入文件";
            btnUpgrade_03.UseVisualStyleBackColor = true;
            btnUpgrade_03.Click += btnUpgrade_03_Click;
            // 
            // lblUpgrade_01
            // 
            lblUpgrade_01.AutoSize = true;
            lblUpgrade_01.Location = new Point(27, 29);
            lblUpgrade_01.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_01.Name = "lblUpgrade_01";
            lblUpgrade_01.Size = new Size(56, 17);
            lblUpgrade_01.TabIndex = 19;
            lblUpgrade_01.Text = "文件路径";
            // 
            // txtPath
            // 
            txtPath.Location = new Point(128, 24);
            txtPath.Margin = new Padding(4, 4, 4, 4);
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(974, 23);
            txtPath.TabIndex = 20;
            // 
            // lblUpgrade_05
            // 
            lblUpgrade_05.AutoSize = true;
            lblUpgrade_05.Location = new Point(128, 118);
            lblUpgrade_05.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_05.Name = "lblUpgrade_05";
            lblUpgrade_05.Size = new Size(32, 17);
            lblUpgrade_05.TabIndex = 6;
            lblUpgrade_05.Text = "Tips";
            // 
            // btnUpgrade_04
            // 
            btnUpgrade_04.BackColor = SystemColors.Control;
            btnUpgrade_04.FlatStyle = FlatStyle.System;
            btnUpgrade_04.Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnUpgrade_04.ForeColor = SystemColors.ActiveCaptionText;
            btnUpgrade_04.Location = new Point(1110, 51);
            btnUpgrade_04.Margin = new Padding(4, 4, 4, 4);
            btnUpgrade_04.Name = "btnUpgrade_04";
            btnUpgrade_04.Size = new Size(88, 31);
            btnUpgrade_04.TabIndex = 5;
            btnUpgrade_04.Text = "启动升级";
            btnUpgrade_04.UseVisualStyleBackColor = false;
            btnUpgrade_04.Click += btnUpgrade_04_Click;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Checked = false;
            dateTimePicker1.CustomFormat = "yy-MM-dd HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(128, 91);
            dateTimePicker1.Margin = new Padding(4, 4, 4, 4);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.Size = new Size(192, 23);
            dateTimePicker1.TabIndex = 17;
            // 
            // ckUpgrade_06
            // 
            ckUpgrade_06.AutoSize = true;
            ckUpgrade_06.Location = new Point(328, 94);
            ckUpgrade_06.Margin = new Padding(4, 4, 4, 4);
            ckUpgrade_06.Name = "ckUpgrade_06";
            ckUpgrade_06.Size = new Size(99, 21);
            ckUpgrade_06.TabIndex = 18;
            ckUpgrade_06.Text = "开启定时升级";
            ckUpgrade_06.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listView1);
            groupBox1.Location = new Point(0, 157);
            groupBox1.Margin = new Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 4, 4, 4);
            groupBox1.Size = new Size(1437, 856);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView1.Dock = DockStyle.Fill;
            listView1.Location = new Point(4, 20);
            listView1.Margin = new Padding(4, 4, 4, 4);
            listView1.Name = "listView1";
            listView1.Size = new Size(1429, 832);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
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
            // CBSUpgradeControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Margin = new Padding(4, 4, 4, 4);
            Name = "CBSUpgradeControl";
            Size = new Size(1237, 700);
            Load += CBSUpgradeControl_Load;
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Label lblUpgrade_07;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnUpgrade_03;
        private System.Windows.Forms.Label lblUpgrade_01;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label lblUpgrade_05;
        private System.Windows.Forms.Button btnUpgrade_04;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.CheckBox ckUpgrade_06;
        private System.Windows.Forms.Label lblUpgrade_02;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbbChipcode;
        private System.Windows.Forms.ComboBox cbbChiprole;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox txtSlaveAddress;
        private System.Windows.Forms.ComboBox cbbChiprole_val;
        private System.Windows.Forms.TextBox txtFC;
        private System.Windows.Forms.TextBox txtFD;
    }
}
