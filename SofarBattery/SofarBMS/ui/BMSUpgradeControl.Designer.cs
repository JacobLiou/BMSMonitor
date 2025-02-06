namespace SofarBMS.UI
{
    partial class BMSUpgradeControl
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
            lblUpgrade_00_1 = new Label();
            txtAppFile = new TextBox();
            progressBar1 = new ProgressBar();
            btnImportApp = new Button();
            btnUpgrade_04 = new Button();
            lblUpgrade_05 = new Label();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            groupBox1 = new GroupBox();
            groupBox3 = new GroupBox();
            ckLocal_Upgrade_Control1 = new CheckBox();
            ckLocal_Upgrade_Control0 = new CheckBox();
            lblUpgrade_02 = new Label();
            btnImportCore = new Button();
            txtCoreFile = new TextBox();
            lblUpgrade_00_2 = new Label();
            dateTimePicker1 = new DateTimePicker();
            ckUpgrade_06 = new CheckBox();
            btnUpgrade_03 = new Button();
            txtPath = new TextBox();
            lblUpgrade_01 = new Label();
            groupBox2 = new GroupBox();
            cbbChipcode = new ComboBox();
            rbUpgrade_05 = new RadioButton();
            rbBin = new RadioButton();
            lblUpgrade_07 = new Label();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // lblUpgrade_00_1
            // 
            lblUpgrade_00_1.AutoSize = true;
            lblUpgrade_00_1.Enabled = false;
            lblUpgrade_00_1.Location = new Point(69, 220);
            lblUpgrade_00_1.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_00_1.Name = "lblUpgrade_00_1";
            lblUpgrade_00_1.Size = new Size(62, 17);
            lblUpgrade_00_1.TabIndex = 0;
            lblUpgrade_00_1.Text = "BMS_APP";
            lblUpgrade_00_1.Visible = false;
            // 
            // txtAppFile
            // 
            txtAppFile.Enabled = false;
            txtAppFile.Location = new Point(170, 214);
            txtAppFile.Margin = new Padding(4, 4, 4, 4);
            txtAppFile.Name = "txtAppFile";
            txtAppFile.Size = new Size(1019, 23);
            txtAppFile.TabIndex = 2;
            txtAppFile.Visible = false;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(299, 26);
            progressBar1.Margin = new Padding(4, 4, 4, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(910, 26);
            progressBar1.TabIndex = 3;
            // 
            // btnImportApp
            // 
            btnImportApp.Enabled = false;
            btnImportApp.Location = new Point(1198, 212);
            btnImportApp.Margin = new Padding(4, 4, 4, 4);
            btnImportApp.Name = "btnImportApp";
            btnImportApp.Size = new Size(88, 33);
            btnImportApp.TabIndex = 4;
            btnImportApp.Text = "导入文件";
            btnImportApp.UseVisualStyleBackColor = true;
            btnImportApp.Visible = false;
            btnImportApp.Click += btnUpgrade_03_Click;
            // 
            // btnUpgrade_04
            // 
            btnUpgrade_04.BackColor = SystemColors.Control;
            btnUpgrade_04.FlatStyle = FlatStyle.System;
            btnUpgrade_04.Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnUpgrade_04.ForeColor = SystemColors.ActiveCaptionText;
            btnUpgrade_04.Location = new Point(1070, 57);
            btnUpgrade_04.Margin = new Padding(4, 4, 4, 4);
            btnUpgrade_04.Name = "btnUpgrade_04";
            btnUpgrade_04.Size = new Size(88, 26);
            btnUpgrade_04.TabIndex = 5;
            btnUpgrade_04.Text = "启动升级";
            btnUpgrade_04.UseVisualStyleBackColor = false;
            btnUpgrade_04.Click += btnUpgrade_04_Click;
            // 
            // lblUpgrade_05
            // 
            lblUpgrade_05.AutoSize = true;
            lblUpgrade_05.Location = new Point(170, 88);
            lblUpgrade_05.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_05.Name = "lblUpgrade_05";
            lblUpgrade_05.Size = new Size(32, 17);
            lblUpgrade_05.TabIndex = 6;
            lblUpgrade_05.Text = "Tips";
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView1.Dock = DockStyle.Fill;
            listView1.Location = new Point(4, 87);
            listView1.Margin = new Padding(4, 4, 4, 4);
            listView1.Name = "listView1";
            listView1.Size = new Size(1217, 478);
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
            // groupBox1
            // 
            groupBox1.Controls.Add(listView1);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(btnImportCore);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(6, 124);
            groupBox1.Margin = new Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 4, 4, 4);
            groupBox1.Size = new Size(1225, 569);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(ckLocal_Upgrade_Control1);
            groupBox3.Controls.Add(ckLocal_Upgrade_Control0);
            groupBox3.Controls.Add(progressBar1);
            groupBox3.Controls.Add(lblUpgrade_02);
            groupBox3.Dock = DockStyle.Top;
            groupBox3.Enabled = false;
            groupBox3.Location = new Point(4, 20);
            groupBox3.Margin = new Padding(4, 4, 4, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4, 4, 4, 4);
            groupBox3.Size = new Size(1217, 67);
            groupBox3.TabIndex = 26;
            groupBox3.TabStop = false;
            groupBox3.Text = "升级对象";
            // 
            // ckLocal_Upgrade_Control1
            // 
            ckLocal_Upgrade_Control1.AutoSize = true;
            ckLocal_Upgrade_Control1.Location = new Point(119, 27);
            ckLocal_Upgrade_Control1.Margin = new Padding(4, 4, 4, 4);
            ckLocal_Upgrade_Control1.Name = "ckLocal_Upgrade_Control1";
            ckLocal_Upgrade_Control1.Size = new Size(60, 21);
            ckLocal_Upgrade_Control1.TabIndex = 15;
            ckLocal_Upgrade_Control1.Text = "CORE";
            ckLocal_Upgrade_Control1.UseVisualStyleBackColor = true;
            // 
            // ckLocal_Upgrade_Control0
            // 
            ckLocal_Upgrade_Control0.AutoSize = true;
            ckLocal_Upgrade_Control0.Location = new Point(38, 27);
            ckLocal_Upgrade_Control0.Margin = new Padding(4, 4, 4, 4);
            ckLocal_Upgrade_Control0.Name = "ckLocal_Upgrade_Control0";
            ckLocal_Upgrade_Control0.Size = new Size(49, 21);
            ckLocal_Upgrade_Control0.TabIndex = 14;
            ckLocal_Upgrade_Control0.Text = "APP";
            ckLocal_Upgrade_Control0.UseVisualStyleBackColor = true;
            // 
            // lblUpgrade_02
            // 
            lblUpgrade_02.AutoSize = true;
            lblUpgrade_02.Location = new Point(199, 30);
            lblUpgrade_02.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_02.Name = "lblUpgrade_02";
            lblUpgrade_02.Size = new Size(56, 17);
            lblUpgrade_02.TabIndex = 24;
            lblUpgrade_02.Text = "升级进度";
            // 
            // btnImportCore
            // 
            btnImportCore.Enabled = false;
            btnImportCore.Location = new Point(1129, 156);
            btnImportCore.Margin = new Padding(4, 4, 4, 4);
            btnImportCore.Name = "btnImportCore";
            btnImportCore.Size = new Size(88, 33);
            btnImportCore.TabIndex = 12;
            btnImportCore.Text = "导入文件";
            btnImportCore.UseVisualStyleBackColor = true;
            btnImportCore.Visible = false;
            btnImportCore.Click += btnImportCore_Click;
            // 
            // txtCoreFile
            // 
            txtCoreFile.Enabled = false;
            txtCoreFile.Location = new Point(170, 268);
            txtCoreFile.Margin = new Padding(4, 4, 4, 4);
            txtCoreFile.Name = "txtCoreFile";
            txtCoreFile.Size = new Size(1019, 23);
            txtCoreFile.TabIndex = 11;
            txtCoreFile.Visible = false;
            // 
            // lblUpgrade_00_2
            // 
            lblUpgrade_00_2.AutoSize = true;
            lblUpgrade_00_2.Enabled = false;
            lblUpgrade_00_2.Location = new Point(69, 273);
            lblUpgrade_00_2.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_00_2.Name = "lblUpgrade_00_2";
            lblUpgrade_00_2.Size = new Size(73, 17);
            lblUpgrade_00_2.TabIndex = 10;
            lblUpgrade_00_2.Text = "BMS_CORE";
            lblUpgrade_00_2.Visible = false;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Checked = false;
            dateTimePicker1.CustomFormat = "yy-MM-dd HH:mm:ss";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Location = new Point(170, 61);
            dateTimePicker1.Margin = new Padding(4, 4, 4, 4);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.Size = new Size(192, 23);
            dateTimePicker1.TabIndex = 17;
            // 
            // ckUpgrade_06
            // 
            ckUpgrade_06.AutoSize = true;
            ckUpgrade_06.Location = new Point(370, 64);
            ckUpgrade_06.Margin = new Padding(4, 4, 4, 4);
            ckUpgrade_06.Name = "ckUpgrade_06";
            ckUpgrade_06.Size = new Size(99, 21);
            ckUpgrade_06.TabIndex = 18;
            ckUpgrade_06.Text = "开启定时升级";
            ckUpgrade_06.UseVisualStyleBackColor = true;
            // 
            // btnUpgrade_03
            // 
            btnUpgrade_03.Location = new Point(1070, 27);
            btnUpgrade_03.Margin = new Padding(4, 4, 4, 4);
            btnUpgrade_03.Name = "btnUpgrade_03";
            btnUpgrade_03.Size = new Size(88, 28);
            btnUpgrade_03.TabIndex = 21;
            btnUpgrade_03.Text = "导入文件";
            btnUpgrade_03.UseVisualStyleBackColor = true;
            btnUpgrade_03.Click += btnImport_Click;
            // 
            // txtPath
            // 
            txtPath.Location = new Point(170, 30);
            txtPath.Margin = new Padding(4, 4, 4, 4);
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(892, 23);
            txtPath.TabIndex = 20;
            // 
            // lblUpgrade_01
            // 
            lblUpgrade_01.AutoSize = true;
            lblUpgrade_01.Location = new Point(69, 35);
            lblUpgrade_01.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_01.Name = "lblUpgrade_01";
            lblUpgrade_01.Size = new Size(56, 17);
            lblUpgrade_01.TabIndex = 19;
            lblUpgrade_01.Text = "文件路径";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cbbChipcode);
            groupBox2.Controls.Add(btnUpgrade_03);
            groupBox2.Controls.Add(txtAppFile);
            groupBox2.Controls.Add(btnImportApp);
            groupBox2.Controls.Add(lblUpgrade_01);
            groupBox2.Controls.Add(txtPath);
            groupBox2.Controls.Add(txtCoreFile);
            groupBox2.Controls.Add(rbUpgrade_05);
            groupBox2.Controls.Add(lblUpgrade_00_2);
            groupBox2.Controls.Add(rbBin);
            groupBox2.Controls.Add(lblUpgrade_00_1);
            groupBox2.Controls.Add(lblUpgrade_05);
            groupBox2.Controls.Add(lblUpgrade_07);
            groupBox2.Controls.Add(btnUpgrade_04);
            groupBox2.Controls.Add(dateTimePicker1);
            groupBox2.Controls.Add(ckUpgrade_06);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(6, 7);
            groupBox2.Margin = new Padding(4, 4, 4, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 4, 4, 4);
            groupBox2.Size = new Size(1225, 117);
            groupBox2.TabIndex = 22;
            groupBox2.TabStop = false;
            groupBox2.Text = "升级固件";
            // 
            // cbbChipcode
            // 
            cbbChipcode.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbChipcode.FormattingEnabled = true;
            cbbChipcode.Items.AddRange(new object[] { "E0", "S3" });
            cbbChipcode.Location = new Point(364, 159);
            cbbChipcode.Margin = new Padding(4, 4, 4, 4);
            cbbChipcode.Name = "cbbChipcode";
            cbbChipcode.Size = new Size(117, 25);
            cbbChipcode.TabIndex = 28;
            cbbChipcode.SelectedIndexChanged += cbbChipcode_SelectedIndexChanged;
            // 
            // rbUpgrade_05
            // 
            rbUpgrade_05.AutoSize = true;
            rbUpgrade_05.Checked = true;
            rbUpgrade_05.Location = new Point(267, 160);
            rbUpgrade_05.Margin = new Padding(4, 4, 4, 4);
            rbUpgrade_05.Name = "rbUpgrade_05";
            rbUpgrade_05.Size = new Size(89, 21);
            rbUpgrade_05.TabIndex = 27;
            rbUpgrade_05.TabStop = true;
            rbUpgrade_05.Text = "SOFAR文件";
            rbUpgrade_05.UseVisualStyleBackColor = true;
            rbUpgrade_05.Visible = false;
            rbUpgrade_05.CheckedChanged += radioButton_CheckedChanged;
            // 
            // rbBin
            // 
            rbBin.AutoSize = true;
            rbBin.Location = new Point(170, 160);
            rbBin.Margin = new Padding(4, 4, 4, 4);
            rbBin.Name = "rbBin";
            rbBin.Size = new Size(72, 21);
            rbBin.TabIndex = 26;
            rbBin.Text = "BIN文件";
            rbBin.UseVisualStyleBackColor = true;
            rbBin.Visible = false;
            rbBin.CheckedChanged += radioButton_CheckedChanged;
            // 
            // lblUpgrade_07
            // 
            lblUpgrade_07.AutoSize = true;
            lblUpgrade_07.Location = new Point(69, 66);
            lblUpgrade_07.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_07.Name = "lblUpgrade_07";
            lblUpgrade_07.Size = new Size(56, 17);
            lblUpgrade_07.TabIndex = 25;
            lblUpgrade_07.Text = "升级类型";
            // 
            // BMSUpgradeControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Margin = new Padding(4, 4, 4, 4);
            Name = "BMSUpgradeControl";
            Padding = new Padding(6, 7, 6, 7);
            Size = new Size(1237, 700);
            Load += BMSUpgradeControl_Load;
            groupBox1.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblUpgrade_00_1;
        private System.Windows.Forms.TextBox txtAppFile;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnImportApp;
        private System.Windows.Forms.Button btnUpgrade_04;
        private System.Windows.Forms.Label lblUpgrade_05;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnImportCore;
        private System.Windows.Forms.TextBox txtCoreFile;
        private System.Windows.Forms.Label lblUpgrade_00_2;
        private System.Windows.Forms.CheckBox ckLocal_Upgrade_Control0;
        private System.Windows.Forms.CheckBox ckLocal_Upgrade_Control1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.CheckBox ckUpgrade_06;
        private System.Windows.Forms.Button btnUpgrade_03;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label lblUpgrade_01;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblUpgrade_02;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblUpgrade_07;
        private System.Windows.Forms.RadioButton rbUpgrade_05;
        private System.Windows.Forms.RadioButton rbBin;
        private System.Windows.Forms.ComboBox cbbChipcode;
    }
}
