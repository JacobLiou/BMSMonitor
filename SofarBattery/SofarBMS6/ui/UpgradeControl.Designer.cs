namespace SofarBMS.UI
{
    partial class UpgradeControl
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
            lblUpgrade_01 = new Label();
            lblUpgrade_02 = new Label();
            txtUpgradeFile = new TextBox();
            progressBar1 = new ProgressBar();
            btnUpgrade_03 = new Button();
            btnUpgrade_04 = new Button();
            lblUpgrade_05 = new Label();
            groupBox1 = new GroupBox();
            listView1 = new ListViewBuff();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            cbbChip_role = new ComboBox();
            txtChip_code = new TextBox();
            lblUpgradeRole = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblUpgrade_01
            // 
            lblUpgrade_01.AutoSize = true;
            lblUpgrade_01.Location = new Point(15, 14);
            lblUpgrade_01.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_01.Name = "lblUpgrade_01";
            lblUpgrade_01.Size = new Size(56, 17);
            lblUpgrade_01.TabIndex = 0;
            lblUpgrade_01.Text = "文件路径";
            // 
            // lblUpgrade_02
            // 
            lblUpgrade_02.AutoSize = true;
            lblUpgrade_02.Location = new Point(15, 47);
            lblUpgrade_02.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_02.Name = "lblUpgrade_02";
            lblUpgrade_02.Size = new Size(56, 17);
            lblUpgrade_02.TabIndex = 1;
            lblUpgrade_02.Text = "下载进度";
            // 
            // txtUpgradeFile
            // 
            txtUpgradeFile.Location = new Point(98, 11);
            txtUpgradeFile.Margin = new Padding(4, 4, 4, 4);
            txtUpgradeFile.Name = "txtUpgradeFile";
            txtUpgradeFile.Size = new Size(819, 23);
            txtUpgradeFile.TabIndex = 2;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(98, 42);
            progressBar1.Margin = new Padding(4, 4, 4, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(820, 33);
            progressBar1.TabIndex = 3;
            // 
            // btnUpgrade_03
            // 
            btnUpgrade_03.AutoSize = true;
            btnUpgrade_03.Location = new Point(939, 5);
            btnUpgrade_03.Margin = new Padding(4, 4, 4, 4);
            btnUpgrade_03.Name = "btnUpgrade_03";
            btnUpgrade_03.Size = new Size(88, 35);
            btnUpgrade_03.TabIndex = 4;
            btnUpgrade_03.Text = "导入文件";
            btnUpgrade_03.UseVisualStyleBackColor = true;
            btnUpgrade_03.Click += btnUpgrade_03_Click;
            // 
            // btnUpgrade_04
            // 
            btnUpgrade_04.AutoSize = true;
            btnUpgrade_04.Location = new Point(939, 42);
            btnUpgrade_04.Margin = new Padding(4, 4, 4, 4);
            btnUpgrade_04.Name = "btnUpgrade_04";
            btnUpgrade_04.Size = new Size(88, 35);
            btnUpgrade_04.TabIndex = 5;
            btnUpgrade_04.Text = "启动升级";
            btnUpgrade_04.UseVisualStyleBackColor = true;
            btnUpgrade_04.Click += btnUpgrade_04_Click;
            // 
            // lblUpgrade_05
            // 
            lblUpgrade_05.AutoSize = true;
            lblUpgrade_05.Location = new Point(98, 119);
            lblUpgrade_05.Margin = new Padding(4, 0, 4, 0);
            lblUpgrade_05.Name = "lblUpgrade_05";
            lblUpgrade_05.Size = new Size(32, 17);
            lblUpgrade_05.TabIndex = 6;
            lblUpgrade_05.Text = "Tips";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listView1);
            groupBox1.Location = new Point(21, 140);
            groupBox1.Margin = new Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 4, 4, 4);
            groupBox1.Size = new Size(1006, 487);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "记录日志";
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView1.Dock = DockStyle.Fill;
            listView1.Location = new Point(4, 20);
            listView1.Margin = new Padding(4, 4, 4, 4);
            listView1.Name = "listView1";
            listView1.Size = new Size(998, 463);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "DateTime";
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
            // cbbChip_role
            // 
            cbbChip_role.FormattingEnabled = true;
            cbbChip_role.Items.AddRange(new object[] { "ARM", "PCU", "DSP", "BMS", "BDU" });
            cbbChip_role.Location = new Point(98, 83);
            cbbChip_role.Margin = new Padding(4, 4, 4, 4);
            cbbChip_role.Name = "cbbChip_role";
            cbbChip_role.Size = new Size(140, 25);
            cbbChip_role.TabIndex = 8;
            cbbChip_role.SelectedIndexChanged += cbbChip_role_SelectedIndexChanged;
            // 
            // txtChip_code
            // 
            txtChip_code.Location = new Point(246, 86);
            txtChip_code.Margin = new Padding(4, 4, 4, 4);
            txtChip_code.Name = "txtChip_code";
            txtChip_code.ReadOnly = true;
            txtChip_code.Size = new Size(116, 23);
            txtChip_code.TabIndex = 9;
            // 
            // lblUpgradeRole
            // 
            lblUpgradeRole.AutoSize = true;
            lblUpgradeRole.Location = new Point(21, 86);
            lblUpgradeRole.Margin = new Padding(4, 0, 4, 0);
            lblUpgradeRole.Name = "lblUpgradeRole";
            lblUpgradeRole.Size = new Size(56, 17);
            lblUpgradeRole.TabIndex = 10;
            lblUpgradeRole.Text = "升级角色";
            // 
            // UpgradeControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblUpgradeRole);
            Controls.Add(txtChip_code);
            Controls.Add(cbbChip_role);
            Controls.Add(groupBox1);
            Controls.Add(lblUpgrade_05);
            Controls.Add(btnUpgrade_04);
            Controls.Add(btnUpgrade_03);
            Controls.Add(progressBar1);
            Controls.Add(txtUpgradeFile);
            Controls.Add(lblUpgrade_02);
            Controls.Add(lblUpgrade_01);
            Margin = new Padding(4, 4, 4, 4);
            Name = "UpgradeControl";
            Size = new Size(1205, 692);
            Load += UpgradeControl_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblUpgrade_01;
        private System.Windows.Forms.Label lblUpgrade_02;
        private System.Windows.Forms.TextBox txtUpgradeFile;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnUpgrade_03;
        private System.Windows.Forms.Button btnUpgrade_04;
        private System.Windows.Forms.Label lblUpgrade_05;
        //private System.Windows.Forms.ListView listView1;
        private ListViewBuff listView1;//增加listview双缓存处理
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbbChip_role;
        private System.Windows.Forms.TextBox txtChip_code;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label lblUpgradeRole;
    }
}
