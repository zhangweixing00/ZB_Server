namespace PersonPositionServer.View
{
    partial class FrmCreateTestData
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.text_CollectChannel_ID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.com_CollectLoopTime = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.text_CollectValueMax = new System.Windows.Forms.TextBox();
            this.com_CollectChannelNum = new System.Windows.Forms.ComboBox();
            this.com_CollectStationID = new System.Windows.Forms.ComboBox();
            this.text_CollectValueMin = new System.Windows.Forms.TextBox();
            this.btn_MakeCollectOnce = new System.Windows.Forms.Button();
            this.btn_MakeCollectLoop = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label_CollectResult = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label_CollectResult);
            this.groupBox2.Controls.Add(this.text_CollectChannel_ID);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.com_CollectLoopTime);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.text_CollectValueMax);
            this.groupBox2.Controls.Add(this.com_CollectChannelNum);
            this.groupBox2.Controls.Add(this.com_CollectStationID);
            this.groupBox2.Controls.Add(this.text_CollectValueMin);
            this.groupBox2.Controls.Add(this.btn_MakeCollectOnce);
            this.groupBox2.Controls.Add(this.btn_MakeCollectLoop);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(7, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(534, 123);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "采集数据";
            // 
            // text_CollectChannel_ID
            // 
            this.text_CollectChannel_ID.Enabled = false;
            this.text_CollectChannel_ID.Location = new System.Drawing.Point(403, 16);
            this.text_CollectChannel_ID.Name = "text_CollectChannel_ID";
            this.text_CollectChannel_ID.Size = new System.Drawing.Size(34, 21);
            this.text_CollectChannel_ID.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(313, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "对应的通道ID：";
            // 
            // com_CollectLoopTime
            // 
            this.com_CollectLoopTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_CollectLoopTime.FormattingEnabled = true;
            this.com_CollectLoopTime.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.com_CollectLoopTime.Location = new System.Drawing.Point(387, 44);
            this.com_CollectLoopTime.Name = "com_CollectLoopTime";
            this.com_CollectLoopTime.Size = new System.Drawing.Size(53, 20);
            this.com_CollectLoopTime.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(141, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "～";
            // 
            // text_CollectValueMax
            // 
            this.text_CollectValueMax.Location = new System.Drawing.Point(160, 44);
            this.text_CollectValueMax.Name = "text_CollectValueMax";
            this.text_CollectValueMax.Size = new System.Drawing.Size(46, 21);
            this.text_CollectValueMax.TabIndex = 10;
            this.text_CollectValueMax.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_CollectValueMax_KeyPress);
            // 
            // com_CollectChannelNum
            // 
            this.com_CollectChannelNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_CollectChannelNum.FormattingEnabled = true;
            this.com_CollectChannelNum.Location = new System.Drawing.Point(250, 16);
            this.com_CollectChannelNum.Name = "com_CollectChannelNum";
            this.com_CollectChannelNum.Size = new System.Drawing.Size(44, 20);
            this.com_CollectChannelNum.TabIndex = 7;
            this.com_CollectChannelNum.SelectedIndexChanged += new System.EventHandler(this.com_CollectChannelNum_SelectedIndexChanged);
            // 
            // com_CollectStationID
            // 
            this.com_CollectStationID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_CollectStationID.FormattingEnabled = true;
            this.com_CollectStationID.Location = new System.Drawing.Point(105, 17);
            this.com_CollectStationID.Name = "com_CollectStationID";
            this.com_CollectStationID.Size = new System.Drawing.Size(61, 20);
            this.com_CollectStationID.TabIndex = 6;
            this.com_CollectStationID.SelectedIndexChanged += new System.EventHandler(this.com_CollectStationID_SelectedIndexChanged);
            // 
            // text_CollectValueMin
            // 
            this.text_CollectValueMin.Location = new System.Drawing.Point(92, 44);
            this.text_CollectValueMin.Name = "text_CollectValueMin";
            this.text_CollectValueMin.Size = new System.Drawing.Size(46, 21);
            this.text_CollectValueMin.TabIndex = 3;
            this.text_CollectValueMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_CollectValueMin_KeyPress);
            // 
            // btn_MakeCollectOnce
            // 
            this.btn_MakeCollectOnce.Location = new System.Drawing.Point(331, 88);
            this.btn_MakeCollectOnce.Name = "btn_MakeCollectOnce";
            this.btn_MakeCollectOnce.Size = new System.Drawing.Size(95, 28);
            this.btn_MakeCollectOnce.TabIndex = 5;
            this.btn_MakeCollectOnce.Text = "产生一次";
            this.btn_MakeCollectOnce.UseVisualStyleBackColor = true;
            this.btn_MakeCollectOnce.Click += new System.EventHandler(this.btn_MakeCollectOnce_Click);
            // 
            // btn_MakeCollectLoop
            // 
            this.btn_MakeCollectLoop.Location = new System.Drawing.Point(432, 88);
            this.btn_MakeCollectLoop.Name = "btn_MakeCollectLoop";
            this.btn_MakeCollectLoop.Size = new System.Drawing.Size(95, 28);
            this.btn_MakeCollectLoop.TabIndex = 1;
            this.btn_MakeCollectLoop.Text = "开始循环产生";
            this.btn_MakeCollectLoop.UseVisualStyleBackColor = true;
            this.btn_MakeCollectLoop.Click += new System.EventHandler(this.btn_MakeCollectLoop_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(248, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "循环产生时间间隔(秒)：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "产生值范围：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "采集器基站ID：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "通道序号：";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label_CollectResult
            // 
            this.label_CollectResult.AutoSize = true;
            this.label_CollectResult.ForeColor = System.Drawing.Color.Green;
            this.label_CollectResult.Location = new System.Drawing.Point(91, 96);
            this.label_CollectResult.Name = "label_CollectResult";
            this.label_CollectResult.Size = new System.Drawing.Size(29, 12);
            this.label_CollectResult.TabIndex = 16;
            this.label_CollectResult.Text = "0 条";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Green;
            this.label3.Location = new System.Drawing.Point(14, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "已产生数据：";
            // 
            // FrmCreateTestData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 138);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmCreateTestData";
            this.Text = "产生测试数据";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_MakeCollectLoop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox com_CollectStationID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox text_CollectValueMin;
        private System.Windows.Forms.Button btn_MakeCollectOnce;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox text_CollectValueMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox com_CollectChannelNum;
        private System.Windows.Forms.ComboBox com_CollectLoopTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox text_CollectChannel_ID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_CollectResult;
        private System.Windows.Forms.Label label3;
    }
}