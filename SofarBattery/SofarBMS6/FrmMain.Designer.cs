
namespace SofarBMS
{
    partial class FrmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            panel1 = new Panel();
            Menu = new MenuStrip();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            splitContainer1 = new SplitContainer();
            panel2 = new Panel();
            btnAlarmInfo = new Button();
            btnStratListen = new Button();
            btnClearInit = new Button();
            cbbIDP = new ComboBox();
            btnResetCAN = new Button();
            cbbBaud = new ComboBox();
            cbbID = new ComboBox();
            lblSp_01 = new Label();
            lblSp_03 = new Label();
            btnConnectionCAN = new Button();
            lblSp_02 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLightLight;
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(2, 1, 2, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1391, 672);
            panel1.TabIndex = 27;
            // 
            // Menu
            // 
            Menu.AutoSize = false;
            Menu.ImageScalingSize = new Size(20, 20);
            Menu.Location = new Point(0, 0);
            Menu.Name = "Menu";
            Menu.Padding = new Padding(5, 1, 0, 1);
            Menu.Size = new Size(1391, 27);
            Menu.TabIndex = 0;
            Menu.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 739);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(1391, 22);
            statusStrip1.TabIndex = 28;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(80, 17);
            toolStripStatusLabel1.Text = "错误码反馈值";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 27);
            splitContainer1.Margin = new Padding(4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Size = new Size(1391, 712);
            splitContainer1.SplitterDistance = 34;
            splitContainer1.SplitterWidth = 6;
            splitContainer1.TabIndex = 29;
            // 
            // panel2
            // 
            panel2.BackColor = Color.DarkSeaGreen;
            panel2.Controls.Add(btnAlarmInfo);
            panel2.Controls.Add(btnStratListen);
            panel2.Controls.Add(btnClearInit);
            panel2.Controls.Add(cbbIDP);
            panel2.Controls.Add(btnResetCAN);
            panel2.Controls.Add(cbbBaud);
            panel2.Controls.Add(cbbID);
            panel2.Controls.Add(lblSp_01);
            panel2.Controls.Add(lblSp_03);
            panel2.Controls.Add(btnConnectionCAN);
            panel2.Controls.Add(lblSp_02);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Margin = new Padding(2, 3, 2, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(1391, 34);
            panel2.TabIndex = 1;
            // 
            // btnAlarmInfo
            // 
            btnAlarmInfo.AutoSize = true;
            btnAlarmInfo.BackColor = Color.Blue;
            btnAlarmInfo.FlatAppearance.BorderSize = 0;
            btnAlarmInfo.FlatStyle = FlatStyle.Flat;
            btnAlarmInfo.ForeColor = SystemColors.Control;
            btnAlarmInfo.Location = new Point(1237, 4);
            btnAlarmInfo.Margin = new Padding(4);
            btnAlarmInfo.Name = "btnAlarmInfo";
            btnAlarmInfo.Size = new Size(90, 28);
            btnAlarmInfo.TabIndex = 56;
            btnAlarmInfo.Text = "报警记录";
            btnAlarmInfo.UseVisualStyleBackColor = false;
            btnAlarmInfo.Click += btnAlarmInfo_Click;
            // 
            // btnStratListen
            // 
            btnStratListen.AutoSize = true;
            btnStratListen.BackColor = Color.Green;
            btnStratListen.FlatAppearance.BorderSize = 0;
            btnStratListen.FlatStyle = FlatStyle.Flat;
            btnStratListen.ForeColor = SystemColors.Control;
            btnStratListen.Location = new Point(1140, 4);
            btnStratListen.Margin = new Padding(4);
            btnStratListen.Name = "btnStratListen";
            btnStratListen.Size = new Size(90, 28);
            btnStratListen.TabIndex = 55;
            btnStratListen.Text = "总线监听";
            btnStratListen.UseVisualStyleBackColor = false;
            btnStratListen.Click += btnStratListen_Click;
            // 
            // btnClearInit
            // 
            btnClearInit.AutoSize = true;
            btnClearInit.Location = new Point(1008, 4);
            btnClearInit.Margin = new Padding(4);
            btnClearInit.Name = "btnClearInit";
            btnClearInit.Size = new Size(125, 28);
            btnClearInit.TabIndex = 54;
            btnClearInit.Text = "清除出厂设置";
            btnClearInit.UseVisualStyleBackColor = true;
            btnClearInit.Visible = false;
            btnClearInit.Click += btnClearInit_Click;
            // 
            // cbbIDP
            // 
            cbbIDP.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbIDP.Font = new Font("宋体", 11.5F, FontStyle.Regular, GraphicsUnit.Point);
            cbbIDP.FormattingEnabled = true;
            cbbIDP.Items.AddRange(new object[] { "1", "2" });
            cbbIDP.Location = new Point(66, 7);
            cbbIDP.Margin = new Padding(2, 1, 2, 1);
            cbbIDP.Name = "cbbIDP";
            cbbIDP.Size = new Size(75, 23);
            cbbIDP.TabIndex = 53;
            cbbIDP.SelectedIndexChanged += cbbIDP_SelectedIndexChanged;
            // 
            // btnResetCAN
            // 
            btnResetCAN.AutoSize = true;
            btnResetCAN.Location = new Point(487, 4);
            btnResetCAN.Margin = new Padding(4);
            btnResetCAN.Name = "btnResetCAN";
            btnResetCAN.Size = new Size(75, 28);
            btnResetCAN.TabIndex = 50;
            btnResetCAN.Text = "重连";
            btnResetCAN.UseVisualStyleBackColor = true;
            btnResetCAN.Visible = false;
            btnResetCAN.Click += btnResetCAN_Click;
            // 
            // cbbBaud
            // 
            cbbBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbBaud.FlatStyle = FlatStyle.System;
            cbbBaud.Font = new Font("宋体", 11.5F, FontStyle.Regular, GraphicsUnit.Point);
            cbbBaud.FormattingEnabled = true;
            cbbBaud.Items.AddRange(new object[] { "100Kbps", "125Kbps", "200Kbps", "250Kbps", "400Kbps", "500Kbps", "666Kbps", "800Kbps", "1000Kbps" });
            cbbBaud.Location = new Point(303, 7);
            cbbBaud.Margin = new Padding(4);
            cbbBaud.Name = "cbbBaud";
            cbbBaud.Size = new Size(93, 23);
            cbbBaud.TabIndex = 42;
            // 
            // cbbID
            // 
            cbbID.DropDownStyle = ComboBoxStyle.DropDownList;
            cbbID.Font = new Font("宋体", 11.5F, FontStyle.Regular, GraphicsUnit.Point);
            cbbID.FormattingEnabled = true;
            cbbID.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
            cbbID.Location = new Point(170, 7);
            cbbID.Margin = new Padding(2, 1, 2, 1);
            cbbID.Name = "cbbID";
            cbbID.Size = new Size(75, 23);
            cbbID.TabIndex = 31;
            cbbID.SelectedIndexChanged += cbbID_SelectedIndexChanged;
            // 
            // lblSp_01
            // 
            lblSp_01.AutoSize = true;
            lblSp_01.ForeColor = Color.Black;
            lblSp_01.Location = new Point(247, 10);
            lblSp_01.Margin = new Padding(4, 0, 4, 0);
            lblSp_01.Name = "lblSp_01";
            lblSp_01.Size = new Size(44, 17);
            lblSp_01.TabIndex = 41;
            lblSp_01.Text = "波特率";
            // 
            // lblSp_03
            // 
            lblSp_03.AutoSize = true;
            lblSp_03.ForeColor = Color.Black;
            lblSp_03.Location = new Point(144, 10);
            lblSp_03.Margin = new Padding(4, 0, 4, 0);
            lblSp_03.Name = "lblSp_03";
            lblSp_03.Size = new Size(21, 17);
            lblSp_03.TabIndex = 47;
            lblSp_03.Text = "ID";
            // 
            // btnConnectionCAN
            // 
            btnConnectionCAN.AutoSize = true;
            btnConnectionCAN.Location = new Point(404, 4);
            btnConnectionCAN.Margin = new Padding(4);
            btnConnectionCAN.Name = "btnConnectionCAN";
            btnConnectionCAN.Size = new Size(75, 28);
            btnConnectionCAN.TabIndex = 43;
            btnConnectionCAN.Text = "连接";
            btnConnectionCAN.UseVisualStyleBackColor = true;
            btnConnectionCAN.Click += btnConnectionCAN_Click;
            // 
            // lblSp_02
            // 
            lblSp_02.AutoSize = true;
            lblSp_02.ForeColor = Color.Black;
            lblSp_02.Location = new Point(10, 10);
            lblSp_02.Margin = new Padding(2, 0, 2, 0);
            lblSp_02.Name = "lblSp_02";
            lblSp_02.Size = new Size(44, 17);
            lblSp_02.TabIndex = 30;
            lblSp_02.Text = "电池簇";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1391, 761);
            Controls.Add(splitContainer1);
            Controls.Add(Menu);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = Menu;
            Margin = new Padding(2, 1, 2, 1);
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BMS电池上位机V1.0.2.3.20240722";
            FormClosing += FrmMain_FormClosing;
            Load += FrmMain_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Panel panel1;
#pragma warning disable CS0108 // “FrmMain.Menu”隐藏继承的成员“Form.Menu”。如果是有意隐藏，请使用关键字 new。
        private System.Windows.Forms.MenuStrip Menu;
#pragma warning restore CS0108 // “FrmMain.Menu”隐藏继承的成员“Form.Menu”。如果是有意隐藏，请使用关键字 new。
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbbIDP;
        private System.Windows.Forms.Button btnResetCAN;
        private System.Windows.Forms.ComboBox cbbBaud;
        private System.Windows.Forms.ComboBox cbbID;
        private System.Windows.Forms.Label lblSp_01;
        private System.Windows.Forms.Label lblSp_03;
        private System.Windows.Forms.Button btnConnectionCAN;
        private System.Windows.Forms.Label lblSp_02;
        private System.Windows.Forms.Button btnClearInit;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStratListen;
        private System.Windows.Forms.Button btnAlarmInfo;
    }
}

