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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnUpgrade_04 = new System.Windows.Forms.Button();
            this.lblUpgrade_05 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnImportCore = new System.Windows.Forms.Button();
            this.lblUpgrade_02 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.ckUpgrade_06 = new System.Windows.Forms.CheckBox();
            this.btnUpgrade_03 = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.lblUpgrade_01 = new System.Windows.Forms.Label();
            this.cbbChipcode = new System.Windows.Forms.ComboBox();
            this.lblUpgrade_07 = new System.Windows.Forms.Label();
            this.lblUpgradeRole = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(122, 101);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(892, 26);
            this.progressBar1.TabIndex = 3;
            // 
            // btnUpgrade_04
            // 
            this.btnUpgrade_04.BackColor = System.Drawing.SystemColors.Control;
            this.btnUpgrade_04.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUpgrade_04.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnUpgrade_04.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpgrade_04.Location = new System.Drawing.Point(1022, 58);
            this.btnUpgrade_04.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpgrade_04.Name = "btnUpgrade_04";
            this.btnUpgrade_04.Size = new System.Drawing.Size(100, 35);
            this.btnUpgrade_04.TabIndex = 5;
            this.btnUpgrade_04.Text = "启动升级";
            this.btnUpgrade_04.UseVisualStyleBackColor = false;
            this.btnUpgrade_04.Click += new System.EventHandler(this.btnUpgrade_04_Click);
            // 
            // lblUpgrade_05
            // 
            this.lblUpgrade_05.AutoSize = true;
            this.lblUpgrade_05.Location = new System.Drawing.Point(122, 131);
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
            this.listView1.Location = new System.Drawing.Point(4, 20);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1114, 496);
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
            this.groupBox1.Controls.Add(this.btnImportCore);
            this.groupBox1.Location = new System.Drawing.Point(6, 169);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1122, 520);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
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
            // lblUpgrade_02
            // 
            this.lblUpgrade_02.AutoSize = true;
            this.lblUpgrade_02.Location = new System.Drawing.Point(22, 105);
            this.lblUpgrade_02.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_02.Name = "lblUpgrade_02";
            this.lblUpgrade_02.Size = new System.Drawing.Size(56, 17);
            this.lblUpgrade_02.TabIndex = 24;
            this.lblUpgrade_02.Text = "升级进度";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Checked = false;
            this.dateTimePicker1.CustomFormat = "yy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(122, 64);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(192, 23);
            this.dateTimePicker1.TabIndex = 17;
            // 
            // ckUpgrade_06
            // 
            this.ckUpgrade_06.AutoSize = true;
            this.ckUpgrade_06.Location = new System.Drawing.Point(322, 65);
            this.ckUpgrade_06.Margin = new System.Windows.Forms.Padding(4);
            this.ckUpgrade_06.Name = "ckUpgrade_06";
            this.ckUpgrade_06.Size = new System.Drawing.Size(99, 21);
            this.ckUpgrade_06.TabIndex = 18;
            this.ckUpgrade_06.Text = "开启定时升级";
            this.ckUpgrade_06.UseVisualStyleBackColor = true;
            // 
            // btnUpgrade_03
            // 
            this.btnUpgrade_03.Location = new System.Drawing.Point(1022, 14);
            this.btnUpgrade_03.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpgrade_03.Name = "btnUpgrade_03";
            this.btnUpgrade_03.Size = new System.Drawing.Size(100, 28);
            this.btnUpgrade_03.TabIndex = 21;
            this.btnUpgrade_03.Text = "导入文件";
            this.btnUpgrade_03.UseVisualStyleBackColor = true;
            this.btnUpgrade_03.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(122, 17);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(892, 23);
            this.txtPath.TabIndex = 20;
            // 
            // lblUpgrade_01
            // 
            this.lblUpgrade_01.AutoSize = true;
            this.lblUpgrade_01.Location = new System.Drawing.Point(22, 20);
            this.lblUpgrade_01.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_01.Name = "lblUpgrade_01";
            this.lblUpgrade_01.Size = new System.Drawing.Size(56, 17);
            this.lblUpgrade_01.TabIndex = 19;
            this.lblUpgrade_01.Text = "文件路径";
            // 
            // cbbChipcode
            // 
            this.cbbChipcode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbChipcode.FormattingEnabled = true;
            this.cbbChipcode.Items.AddRange(new object[] {
            "E0",
            "S3"});
            this.cbbChipcode.Location = new System.Drawing.Point(503, 63);
            this.cbbChipcode.Margin = new System.Windows.Forms.Padding(4);
            this.cbbChipcode.Name = "cbbChipcode";
            this.cbbChipcode.Size = new System.Drawing.Size(117, 25);
            this.cbbChipcode.TabIndex = 28;
            this.cbbChipcode.SelectedIndexChanged += new System.EventHandler(this.cbbChipcode_SelectedIndexChanged);
            // 
            // lblUpgrade_07
            // 
            this.lblUpgrade_07.AutoSize = true;
            this.lblUpgrade_07.Location = new System.Drawing.Point(22, 67);
            this.lblUpgrade_07.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrade_07.Name = "lblUpgrade_07";
            this.lblUpgrade_07.Size = new System.Drawing.Size(56, 17);
            this.lblUpgrade_07.TabIndex = 25;
            this.lblUpgrade_07.Text = "升级类型";
            // 
            // lblUpgradeRole
            // 
            this.lblUpgradeRole.AutoSize = true;
            this.lblUpgradeRole.Location = new System.Drawing.Point(439, 67);
            this.lblUpgradeRole.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgradeRole.Name = "lblUpgradeRole";
            this.lblUpgradeRole.Size = new System.Drawing.Size(56, 17);
            this.lblUpgradeRole.TabIndex = 29;
            this.lblUpgradeRole.Text = "升级角色";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblUpgradeRole);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.ckUpgrade_06);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.cbbChipcode);
            this.panel1.Controls.Add(this.btnUpgrade_04);
            this.panel1.Controls.Add(this.btnUpgrade_03);
            this.panel1.Controls.Add(this.lblUpgrade_05);
            this.panel1.Controls.Add(this.txtPath);
            this.panel1.Controls.Add(this.lblUpgrade_01);
            this.panel1.Controls.Add(this.lblUpgrade_02);
            this.panel1.Controls.Add(this.lblUpgrade_07);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(6, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1132, 155);
            this.panel1.TabIndex = 23;
            // 
            // BMSUpgradeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BMSUpgradeControl";
            this.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Size = new System.Drawing.Size(1144, 700);
            this.Load += new System.EventHandler(this.BMSUpgradeControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnUpgrade_04;
        private System.Windows.Forms.Label lblUpgrade_05;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnImportCore;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.CheckBox ckUpgrade_06;
        private System.Windows.Forms.Button btnUpgrade_03;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label lblUpgrade_01;
        private System.Windows.Forms.Label lblUpgrade_02;
        private System.Windows.Forms.Label lblUpgrade_07;
        private System.Windows.Forms.ComboBox cbbChipcode;
        private Label lblUpgradeRole;
        private Panel panel1;
    }
}
