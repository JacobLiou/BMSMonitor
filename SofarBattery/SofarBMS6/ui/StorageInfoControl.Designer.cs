namespace SofarBMS.UI
{
    partial class StorageInfoControl
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
            dgvStorageInfo = new DataGridView();
            splitContainer1 = new SplitContainer();
            btnExport = new Button();
            btn_01 = new Button();
            btn_03 = new Button();
            btn_02 = new Button();
            btn_04 = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvStorageInfo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvStorageInfo
            // 
            dgvStorageInfo.AllowUserToAddRows = false;
            dgvStorageInfo.AllowUserToDeleteRows = false;
            dgvStorageInfo.BackgroundColor = SystemColors.ControlLightLight;
            dgvStorageInfo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStorageInfo.Dock = DockStyle.Fill;
            dgvStorageInfo.GridColor = Color.FromArgb(204, 204, 204);
            dgvStorageInfo.Location = new Point(0, 0);
            dgvStorageInfo.Margin = new Padding(18, 23, 18, 23);
            dgvStorageInfo.Name = "dgvStorageInfo";
            dgvStorageInfo.ReadOnly = true;
            dgvStorageInfo.RowHeadersWidth = 51;
            dgvStorageInfo.RowTemplate.Height = 23;
            dgvStorageInfo.Size = new Size(1298, 577);
            dgvStorageInfo.TabIndex = 0;
            dgvStorageInfo.RowStateChanged += dataGridView1_RowStateChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(2, 3, 2, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = SystemColors.ControlLightLight;
            splitContainer1.Panel1.Controls.Add(btnExport);
            splitContainer1.Panel1.Controls.Add(btn_01);
            splitContainer1.Panel1.Controls.Add(btn_03);
            splitContainer1.Panel1.Controls.Add(btn_02);
            splitContainer1.Panel1.Controls.Add(btn_04);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = SystemColors.ControlLightLight;
            splitContainer1.Panel2.Controls.Add(dgvStorageInfo);
            splitContainer1.Size = new Size(1298, 700);
            splitContainer1.SplitterDistance = 119;
            splitContainer1.TabIndex = 2;
            // 
            // btnExport
            // 
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.FlatStyle = FlatStyle.System;
            btnExport.ForeColor = Color.Black;
            btnExport.Image = Resources.daochu;
            btnExport.ImageAlign = ContentAlignment.MiddleLeft;
            btnExport.Location = new Point(763, 11);
            btnExport.Margin = new Padding(4, 4, 4, 4);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(107, 34);
            btnExport.TabIndex = 12;
            btnExport.Text = "导出";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // btn_01
            // 
            btn_01.FlatAppearance.BorderSize = 0;
            btn_01.FlatStyle = FlatStyle.System;
            btn_01.ForeColor = Color.Black;
            btn_01.Image = Resources.zhongzhi;
            btn_01.ImageAlign = ContentAlignment.MiddleLeft;
            btn_01.Location = new Point(74, 11);
            btn_01.Margin = new Padding(4, 4, 4, 4);
            btn_01.Name = "btn_01";
            btn_01.Size = new Size(107, 34);
            btn_01.TabIndex = 8;
            btn_01.Text = "重置";
            btn_01.UseVisualStyleBackColor = true;
            btn_01.Click += btn_01_Click;
            // 
            // btn_03
            // 
            btn_03.FlatAppearance.BorderSize = 0;
            btn_03.FlatStyle = FlatStyle.System;
            btn_03.ForeColor = Color.Black;
            btn_03.Image = Resources.zanting;
            btn_03.ImageAlign = ContentAlignment.MiddleLeft;
            btn_03.Location = new Point(419, 11);
            btn_03.Margin = new Padding(4, 4, 4, 4);
            btn_03.Name = "btn_03";
            btn_03.Size = new Size(107, 34);
            btn_03.TabIndex = 11;
            btn_03.Text = "停止";
            btn_03.UseVisualStyleBackColor = true;
            btn_03.Click += btn_Stop_Click;
            // 
            // btn_02
            // 
            btn_02.FlatAppearance.BorderSize = 0;
            btn_02.FlatStyle = FlatStyle.System;
            btn_02.ForeColor = Color.Black;
            btn_02.Image = Resources.duqushujuku;
            btn_02.ImageAlign = ContentAlignment.MiddleLeft;
            btn_02.Location = new Point(246, 11);
            btn_02.Margin = new Padding(4, 4, 4, 4);
            btn_02.Name = "btn_02";
            btn_02.Size = new Size(107, 34);
            btn_02.TabIndex = 9;
            btn_02.Text = "读取";
            btn_02.UseVisualStyleBackColor = true;
            btn_02.Click += btn_02_Click;
            // 
            // btn_04
            // 
            btn_04.FlatAppearance.BorderSize = 0;
            btn_04.FlatStyle = FlatStyle.System;
            btn_04.ForeColor = Color.Black;
            btn_04.Image = Resources.qingkong;
            btn_04.ImageAlign = ContentAlignment.MiddleLeft;
            btn_04.Location = new Point(595, 11);
            btn_04.Margin = new Padding(4, 4, 4, 4);
            btn_04.Name = "btn_04";
            btn_04.Size = new Size(107, 34);
            btn_04.TabIndex = 10;
            btn_04.Text = "清空";
            btn_04.UseVisualStyleBackColor = true;
            btn_04.Click += btn_AA_Click;
            // 
            // StorageInfoControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Margin = new Padding(4, 4, 4, 4);
            Name = "StorageInfoControl";
            Size = new Size(1298, 700);
            Load += StorageInfoControl_Load;
            ((System.ComponentModel.ISupportInitialize)dgvStorageInfo).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStorageInfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btn_03;
        private System.Windows.Forms.Button btn_04;
        private System.Windows.Forms.Button btn_02;
        private System.Windows.Forms.Button btn_01;
        private System.Windows.Forms.Button btnExport;
    }
}
