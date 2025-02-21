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
            this.lblUpgrade_00_1 = new System.Windows.Forms.Label();
            this.txtAppFile = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnImportApp = new System.Windows.Forms.Button();
            this.btnUpgrade_04 = new System.Windows.Forms.Button();
            this.lblUpgrade_05 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ckLocal_Upgrade_Control1 = new System.Windows.Forms.CheckBox();
            this.ckLocal_Upgrade_Control0 = new System.Windows.Forms.CheckBox();
            this.lblUpgrade_02 = new System.Windows.Forms.Label();
            this.btnImportCore = new System.Windows.Forms.Button();
            this.txtCoreFile = new System.Windows.Forms.TextBox();
            this.lblUpgrade_00_2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.ckUpgrade_06 = new System.Windows.Forms.CheckBox();
            this.btnUpgrade_03 = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.lblUpgrade_01 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbbChipcode = new System.Windows.Forms.ComboBox();
            this.rbUpgrade_05 = new System.Windows.Forms.RadioButton();
            this.rbBin = new System.Windows.Forms.RadioButton();
            this.lblUpgrade_07 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUpgrade_00_1
            // 
            this.lblUpgrade_00_1.AutoSize = true;
            this.lblUpgrade_00_1.Enabled = false;
            this.lblUpgrade_00_1.Location = new System.Drawing.Point(69, 220);
            this.lblUpgrade_00_1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_00_1.Name = "lblUpgrade_00_1";
            this.lblUpgrade_00_1.Size = new System.Drawing.Size(62, 17);
            this.lblUpgrade_00_1.TabIndex = 0;
            this.lblUpgrade_00_1.Text = "BMS_APP";
            this.lblUpgrade_00_1.Visible = false;
            // 
            // txtAppFile
            // 
            this.txtAppFile.Enabled = false;
            this.txtAppFile.Location = new System.Drawing.Point(170, 214);
            this.txtAppFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtAppFile.Name = "txtAppFile";
            this.txtAppFile.Size = new System.Drawing.Size(1019, 23);
            this.txtAppFile.TabIndex = 2;
            this.txtAppFile.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(299, 26);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(910, 26);
            this.progressBar1.TabIndex = 3;
            // 
            // btnImportApp
            // 
            this.btnImportApp.Enabled = false;
            this.btnImportApp.Location = new System.Drawing.Point(1198, 212);
            this.btnImportApp.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportApp.Name = "btnImportApp";
            this.btnImportApp.Size = new System.Drawing.Size(88, 33);
            this.btnImportApp.TabIndex = 4;
            this.btnImportApp.Text = "导入文件";
            this.btnImportApp.UseVisualStyleBackColor = true;
            this.btnImportApp.Visible = false;
            // 
            // btnUpgrade_04
            // 
            this.btnUpgrade_04.BackColor = System.Drawing.SystemColors.Control;
            this.btnUpgrade_04.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUpgrade_04.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnUpgrade_04.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpgrade_04.Location = new System.Drawing.Point(1069, 58);
            this.btnUpgrade_04.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpgrade_04.Name = "btnUpgrade_04";
            this.btnUpgrade_04.Size = new System.Drawing.Size(88, 26);
            this.btnUpgrade_04.TabIndex = 5;
            this.btnUpgrade_04.Text = "启动升级";
            this.btnUpgrade_04.UseVisualStyleBackColor = false;
            // 
            // lblUpgrade_05
            // 
            this.lblUpgrade_05.AutoSize = true;
            this.lblUpgrade_05.Location = new System.Drawing.Point(169, 85);
            this.lblUpgrade_05.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_05.Name = "lblUpgrade_05";
            this.lblUpgrade_05.Size = new System.Drawing.Size(32, 17);
            this.lblUpgrade_05.TabIndex = 6;
            this.lblUpgrade_05.Text = "Tips";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(4, 87);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1217, 478);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnImportCore);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(6, 124);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1225, 569);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ckLocal_Upgrade_Control1);
            this.groupBox3.Controls.Add(this.ckLocal_Upgrade_Control0);
            this.groupBox3.Controls.Add(this.progressBar1);
            this.groupBox3.Controls.Add(this.lblUpgrade_02);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(4, 20);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(1217, 67);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "升级对象";
            // 
            // ckLocal_Upgrade_Control1
            // 
            this.ckLocal_Upgrade_Control1.AutoSize = true;
            this.ckLocal_Upgrade_Control1.Location = new System.Drawing.Point(119, 27);
            this.ckLocal_Upgrade_Control1.Margin = new System.Windows.Forms.Padding(4);
            this.ckLocal_Upgrade_Control1.Name = "ckLocal_Upgrade_Control1";
            this.ckLocal_Upgrade_Control1.Size = new System.Drawing.Size(60, 21);
            this.ckLocal_Upgrade_Control1.TabIndex = 15;
            this.ckLocal_Upgrade_Control1.Text = "CORE";
            this.ckLocal_Upgrade_Control1.UseVisualStyleBackColor = true;
            // 
            // ckLocal_Upgrade_Control0
            // 
            this.ckLocal_Upgrade_Control0.AutoSize = true;
            this.ckLocal_Upgrade_Control0.Location = new System.Drawing.Point(38, 27);
            this.ckLocal_Upgrade_Control0.Margin = new System.Windows.Forms.Padding(4);
            this.ckLocal_Upgrade_Control0.Name = "ckLocal_Upgrade_Control0";
            this.ckLocal_Upgrade_Control0.Size = new System.Drawing.Size(49, 21);
            this.ckLocal_Upgrade_Control0.TabIndex = 14;
            this.ckLocal_Upgrade_Control0.Text = "APP";
            this.ckLocal_Upgrade_Control0.UseVisualStyleBackColor = true;
            // 
            // lblUpgrade_02
            // 
            this.lblUpgrade_02.AutoSize = true;
            this.lblUpgrade_02.Location = new System.Drawing.Point(199, 30);
            this.lblUpgrade_02.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_02.Name = "lblUpgrade_02";
            this.lblUpgrade_02.Size = new System.Drawing.Size(56, 17);
            this.lblUpgrade_02.TabIndex = 24;
            this.lblUpgrade_02.Text = "升级进度";
            // 
            // btnImportCore
            // 
            this.btnImportCore.Enabled = false;
            this.btnImportCore.Location = new System.Drawing.Point(1129, 156);
            this.btnImportCore.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportCore.Name = "btnImportCore";
            this.btnImportCore.Size = new System.Drawing.Size(88, 33);
            this.btnImportCore.TabIndex = 12;
            this.btnImportCore.Text = "导入文件";
            this.btnImportCore.UseVisualStyleBackColor = true;
            this.btnImportCore.Visible = false;
            // 
            // txtCoreFile
            // 
            this.txtCoreFile.Enabled = false;
            this.txtCoreFile.Location = new System.Drawing.Point(170, 268);
            this.txtCoreFile.Margin = new System.Windows.Forms.Padding(4);
            this.txtCoreFile.Name = "txtCoreFile";
            this.txtCoreFile.Size = new System.Drawing.Size(1019, 23);
            this.txtCoreFile.TabIndex = 11;
            this.txtCoreFile.Visible = false;
            // 
            // lblUpgrade_00_2
            // 
            this.lblUpgrade_00_2.AutoSize = true;
            this.lblUpgrade_00_2.Enabled = false;
            this.lblUpgrade_00_2.Location = new System.Drawing.Point(69, 273);
            this.lblUpgrade_00_2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_00_2.Name = "lblUpgrade_00_2";
            this.lblUpgrade_00_2.Size = new System.Drawing.Size(73, 17);
            this.lblUpgrade_00_2.TabIndex = 10;
            this.lblUpgrade_00_2.Text = "BMS_CORE";
            this.lblUpgrade_00_2.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Checked = false;
            this.dateTimePicker1.CustomFormat = "yy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(169, 60);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(192, 23);
            this.dateTimePicker1.TabIndex = 17;
            // 
            // ckUpgrade_06
            // 
            this.ckUpgrade_06.AutoSize = true;
            this.ckUpgrade_06.Location = new System.Drawing.Point(369, 61);
            this.ckUpgrade_06.Margin = new System.Windows.Forms.Padding(4);
            this.ckUpgrade_06.Name = "ckUpgrade_06";
            this.ckUpgrade_06.Size = new System.Drawing.Size(99, 21);
            this.ckUpgrade_06.TabIndex = 18;
            this.ckUpgrade_06.Text = "开启定时升级";
            this.ckUpgrade_06.UseVisualStyleBackColor = true;
            // 
            // btnUpgrade_03
            // 
            this.btnUpgrade_03.Location = new System.Drawing.Point(1069, 24);
            this.btnUpgrade_03.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpgrade_03.Name = "btnUpgrade_03";
            this.btnUpgrade_03.Size = new System.Drawing.Size(88, 28);
            this.btnUpgrade_03.TabIndex = 21;
            this.btnUpgrade_03.Text = "导入文件";
            this.btnUpgrade_03.UseVisualStyleBackColor = true;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(169, 27);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(892, 23);
            this.txtPath.TabIndex = 20;
            // 
            // lblUpgrade_01
            // 
            this.lblUpgrade_01.AutoSize = true;
            this.lblUpgrade_01.Location = new System.Drawing.Point(68, 30);
            this.lblUpgrade_01.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_01.Name = "lblUpgrade_01";
            this.lblUpgrade_01.Size = new System.Drawing.Size(56, 17);
            this.lblUpgrade_01.TabIndex = 19;
            this.lblUpgrade_01.Text = "文件路径";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbbChipcode);
            this.groupBox2.Controls.Add(this.btnUpgrade_03);
            this.groupBox2.Controls.Add(this.txtAppFile);
            this.groupBox2.Controls.Add(this.btnImportApp);
            this.groupBox2.Controls.Add(this.lblUpgrade_01);
            this.groupBox2.Controls.Add(this.txtPath);
            this.groupBox2.Controls.Add(this.txtCoreFile);
            this.groupBox2.Controls.Add(this.rbUpgrade_05);
            this.groupBox2.Controls.Add(this.lblUpgrade_00_2);
            this.groupBox2.Controls.Add(this.rbBin);
            this.groupBox2.Controls.Add(this.lblUpgrade_00_1);
            this.groupBox2.Controls.Add(this.lblUpgrade_05);
            this.groupBox2.Controls.Add(this.lblUpgrade_07);
            this.groupBox2.Controls.Add(this.btnUpgrade_04);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.ckUpgrade_06);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(6, 7);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(1225, 117);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "升级固件";
            // 
            // cbbChipcode
            // 
            this.cbbChipcode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbChipcode.FormattingEnabled = true;
            this.cbbChipcode.Items.AddRange(new object[] {
            "E0",
            "S3"});
            this.cbbChipcode.Location = new System.Drawing.Point(364, 159);
            this.cbbChipcode.Margin = new System.Windows.Forms.Padding(4);
            this.cbbChipcode.Name = "cbbChipcode";
            this.cbbChipcode.Size = new System.Drawing.Size(117, 25);
            this.cbbChipcode.TabIndex = 28;
            // 
            // rbUpgrade_05
            // 
            this.rbUpgrade_05.AutoSize = true;
            this.rbUpgrade_05.Checked = true;
            this.rbUpgrade_05.Location = new System.Drawing.Point(267, 160);
            this.rbUpgrade_05.Margin = new System.Windows.Forms.Padding(4);
            this.rbUpgrade_05.Name = "rbUpgrade_05";
            this.rbUpgrade_05.Size = new System.Drawing.Size(89, 21);
            this.rbUpgrade_05.TabIndex = 27;
            this.rbUpgrade_05.TabStop = true;
            this.rbUpgrade_05.Text = "SOFAR文件";
            this.rbUpgrade_05.UseVisualStyleBackColor = true;
            this.rbUpgrade_05.Visible = false;
            // 
            // rbBin
            // 
            this.rbBin.AutoSize = true;
            this.rbBin.Location = new System.Drawing.Point(170, 160);
            this.rbBin.Margin = new System.Windows.Forms.Padding(4);
            this.rbBin.Name = "rbBin";
            this.rbBin.Size = new System.Drawing.Size(72, 21);
            this.rbBin.TabIndex = 26;
            this.rbBin.Text = "BIN文件";
            this.rbBin.UseVisualStyleBackColor = true;
            this.rbBin.Visible = false;
            // 
            // lblUpgrade_07
            // 
            this.lblUpgrade_07.AutoSize = true;
            this.lblUpgrade_07.Location = new System.Drawing.Point(68, 63);
            this.lblUpgrade_07.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_07.Name = "lblUpgrade_07";
            this.lblUpgrade_07.Size = new System.Drawing.Size(56, 17);
            this.lblUpgrade_07.TabIndex = 25;
            this.lblUpgrade_07.Text = "升级类型";
            // 
            // BMSUpgradeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BMSUpgradeControl";
            this.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Size = new System.Drawing.Size(1237, 700);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

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
