namespace PersonPositionServer.View
{
    partial class FrmSetting
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
            this.textArriveRSSI = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.textServerPort = new System.Windows.Forms.TextBox();
            this.com_PressMinute = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox_IsLinePosition = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.com_Port_Stop = new System.Windows.Forms.ComboBox();
            this.com_Port_port = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.com_Port_Data = new System.Windows.Forms.ComboBox();
            this.com_Port_Bote = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.com_Port_Check = new System.Windows.Forms.ComboBox();
            this.textSlick = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.textDBConnStr = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.com_ErrorStationTimes = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.com_SlickTimes = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.com_NullRSSITimes = new System.Windows.Forms.ComboBox();
            this.checkBox_CheckOutRSSI = new System.Windows.Forms.CheckBox();
            this.text_CheckOutRSSI = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.com_PressMinuteCollect = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.com_OtherLoopTime = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.com_DistanceForDuty1Per = new System.Windows.Forms.ComboBox();
            this.checkBox_ShowLowPower = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.tex_AlarmMaxPerson = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tex_AlarmMaxHour = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.check_OutWhen1GetRSSI = new System.Windows.Forms.CheckBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.check_IsShowBug = new System.Windows.Forms.CheckBox();
            this.label39 = new System.Windows.Forms.Label();
            this.check_SwitchHW = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_IsUseHongWai = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textArriveRSSI
            // 
            this.textArriveRSSI.Location = new System.Drawing.Point(148, 206);
            this.textArriveRSSI.Name = "textArriveRSSI";
            this.textArriveRSSI.Size = new System.Drawing.Size(34, 21);
            this.textArriveRSSI.TabIndex = 88;
            this.textArriveRSSI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textArriveRSSI_KeyPress);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(13, 210);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(131, 12);
            this.label13.TabIndex = 87;
            this.label13.Text = "到达基站信号强度标值:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(15, 112);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(167, 12);
            this.label19.TabIndex = 86;
            this.label19.Text = "历史定位数据存储精度(分钟):";
            // 
            // textServerPort
            // 
            this.textServerPort.Location = new System.Drawing.Point(579, 25);
            this.textServerPort.Name = "textServerPort";
            this.textServerPort.Size = new System.Drawing.Size(45, 21);
            this.textServerPort.TabIndex = 83;
            this.textServerPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textServerPort_KeyPress);
            // 
            // com_PressMinute
            // 
            this.com_PressMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_PressMinute.FormattingEnabled = true;
            this.com_PressMinute.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50",
            "60"});
            this.com_PressMinute.Location = new System.Drawing.Point(184, 108);
            this.com_PressMinute.Name = "com_PressMinute";
            this.com_PressMinute.Size = new System.Drawing.Size(37, 20);
            this.com_PressMinute.TabIndex = 85;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(503, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(71, 12);
            this.label14.TabIndex = 82;
            this.label14.Text = "服务器端口:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(598, 292);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 93;
            this.button1.Text = "保存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox_IsLinePosition
            // 
            this.checkBox_IsLinePosition.AutoSize = true;
            this.checkBox_IsLinePosition.Checked = true;
            this.checkBox_IsLinePosition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IsLinePosition.Location = new System.Drawing.Point(137, 185);
            this.checkBox_IsLinePosition.Name = "checkBox_IsLinePosition";
            this.checkBox_IsLinePosition.Size = new System.Drawing.Size(15, 14);
            this.checkBox_IsLinePosition.TabIndex = 108;
            this.checkBox_IsLinePosition.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 186);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 12);
            this.label8.TabIndex = 107;
            this.label8.Text = "仅使用线性定位算法:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(247, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 109;
            this.label9.Text = "端  口:";
            // 
            // com_Port_Stop
            // 
            this.com_Port_Stop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_Port_Stop.FormattingEnabled = true;
            this.com_Port_Stop.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.com_Port_Stop.Location = new System.Drawing.Point(423, 49);
            this.com_Port_Stop.Name = "com_Port_Stop";
            this.com_Port_Stop.Size = new System.Drawing.Size(59, 20);
            this.com_Port_Stop.TabIndex = 118;
            // 
            // com_Port_port
            // 
            this.com_Port_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_Port_port.FormattingEnabled = true;
            this.com_Port_port.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16",
            "COM17",
            "COM18",
            "COM19",
            "COM20"});
            this.com_Port_port.Location = new System.Drawing.Point(299, 24);
            this.com_Port_port.Name = "com_Port_port";
            this.com_Port_port.Size = new System.Drawing.Size(63, 20);
            this.com_Port_port.TabIndex = 114;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(247, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 110;
            this.label10.Text = "波特率:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(371, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 113;
            this.label11.Text = "数据位:";
            // 
            // com_Port_Data
            // 
            this.com_Port_Data.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_Port_Data.FormattingEnabled = true;
            this.com_Port_Data.Items.AddRange(new object[] {
            "8",
            "7",
            "6"});
            this.com_Port_Data.Location = new System.Drawing.Point(423, 24);
            this.com_Port_Data.Name = "com_Port_Data";
            this.com_Port_Data.Size = new System.Drawing.Size(59, 20);
            this.com_Port_Data.TabIndex = 117;
            // 
            // com_Port_Bote
            // 
            this.com_Port_Bote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_Port_Bote.FormattingEnabled = true;
            this.com_Port_Bote.Items.AddRange(new object[] {
            "38400",
            "115200"});
            this.com_Port_Bote.Location = new System.Drawing.Point(299, 49);
            this.com_Port_Bote.Name = "com_Port_Bote";
            this.com_Port_Bote.Size = new System.Drawing.Size(63, 20);
            this.com_Port_Bote.TabIndex = 115;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(247, 76);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 12);
            this.label12.TabIndex = 111;
            this.label12.Text = "校验位:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(371, 52);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 12);
            this.label15.TabIndex = 112;
            this.label15.Text = "停止位:";
            // 
            // com_Port_Check
            // 
            this.com_Port_Check.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_Port_Check.FormattingEnabled = true;
            this.com_Port_Check.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.com_Port_Check.Location = new System.Drawing.Point(299, 73);
            this.com_Port_Check.Name = "com_Port_Check";
            this.com_Port_Check.Size = new System.Drawing.Size(63, 20);
            this.com_Port_Check.TabIndex = 116;
            // 
            // textSlick
            // 
            this.textSlick.Location = new System.Drawing.Point(134, 231);
            this.textSlick.Name = "textSlick";
            this.textSlick.Size = new System.Drawing.Size(34, 21);
            this.textSlick.TabIndex = 120;
            this.textSlick.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textSlick_KeyPress);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(13, 235);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(119, 12);
            this.label26.TabIndex = 119;
            this.label26.Text = "平滑处理距离起始值:";
            // 
            // textDBConnStr
            // 
            this.textDBConnStr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textDBConnStr.Location = new System.Drawing.Point(503, 165);
            this.textDBConnStr.Multiline = true;
            this.textDBConnStr.Name = "textDBConnStr";
            this.textDBConnStr.Size = new System.Drawing.Size(244, 106);
            this.textDBConnStr.TabIndex = 123;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(503, 151);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(95, 12);
            this.label16.TabIndex = 122;
            this.label16.Text = "数据库连接字串:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(677, 292);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 28);
            this.button2.TabIndex = 125;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(503, 105);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(203, 12);
            this.label17.TabIndex = 127;
            this.label17.Text = "判断基站故障阀值(无返回巡检次数):";
            // 
            // com_ErrorStationTimes
            // 
            this.com_ErrorStationTimes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_ErrorStationTimes.FormattingEnabled = true;
            this.com_ErrorStationTimes.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "15",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
            this.com_ErrorStationTimes.Location = new System.Drawing.Point(710, 101);
            this.com_ErrorStationTimes.Name = "com_ErrorStationTimes";
            this.com_ErrorStationTimes.Size = new System.Drawing.Size(37, 20);
            this.com_ErrorStationTimes.TabIndex = 126;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(13, 259);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(83, 12);
            this.label20.TabIndex = 131;
            this.label20.Text = "平滑处理次数:";
            // 
            // com_SlickTimes
            // 
            this.com_SlickTimes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_SlickTimes.FormattingEnabled = true;
            this.com_SlickTimes.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7 ",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "20",
            "30"});
            this.com_SlickTimes.Location = new System.Drawing.Point(100, 256);
            this.com_SlickTimes.Name = "com_SlickTimes";
            this.com_SlickTimes.Size = new System.Drawing.Size(38, 20);
            this.com_SlickTimes.TabIndex = 130;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.SteelBlue;
            this.label21.Location = new System.Drawing.Point(239, 9);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 12);
            this.label21.TabIndex = 137;
            this.label21.Text = "串口参数";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.SteelBlue;
            this.label23.Location = new System.Drawing.Point(7, 167);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(53, 12);
            this.label23.TabIndex = 139;
            this.label23.Text = "定位参数";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.SteelBlue;
            this.label24.Location = new System.Drawing.Point(239, 106);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(53, 12);
            this.label24.TabIndex = 140;
            this.label24.Text = "考勤参数";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.Color.SteelBlue;
            this.label25.Location = new System.Drawing.Point(7, 9);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(53, 12);
            this.label25.TabIndex = 141;
            this.label25.Text = "巡检参数";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.Color.SteelBlue;
            this.label27.Location = new System.Drawing.Point(497, 9);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(53, 12);
            this.label27.TabIndex = 142;
            this.label27.Text = "其他参数";
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label28.Location = new System.Drawing.Point(3, 275);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(753, 14);
            this.label28.TabIndex = 143;
            this.label28.Text = "                                                                                 " +
                "                                                                    ";
            this.label28.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(246, 125);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(155, 12);
            this.label29.TabIndex = 145;
            this.label29.Text = "出洞或进入盲区无信号次数:";
            // 
            // com_NullRSSITimes
            // 
            this.com_NullRSSITimes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_NullRSSITimes.FormattingEnabled = true;
            this.com_NullRSSITimes.Items.AddRange(new object[] {
            "5",
            "6",
            "8",
            "10",
            "15",
            "20"});
            this.com_NullRSSITimes.Location = new System.Drawing.Point(405, 121);
            this.com_NullRSSITimes.Name = "com_NullRSSITimes";
            this.com_NullRSSITimes.Size = new System.Drawing.Size(48, 20);
            this.com_NullRSSITimes.TabIndex = 144;
            // 
            // checkBox_CheckOutRSSI
            // 
            this.checkBox_CheckOutRSSI.AutoSize = true;
            this.checkBox_CheckOutRSSI.Location = new System.Drawing.Point(16, 28);
            this.checkBox_CheckOutRSSI.Name = "checkBox_CheckOutRSSI";
            this.checkBox_CheckOutRSSI.Size = new System.Drawing.Size(180, 16);
            this.checkBox_CheckOutRSSI.TabIndex = 156;
            this.checkBox_CheckOutRSSI.Text = "筛选去除微弱的信号强度数据";
            this.checkBox_CheckOutRSSI.UseVisualStyleBackColor = true;
            this.checkBox_CheckOutRSSI.CheckedChanged += new System.EventHandler(this.checkBox_CheckOutRSSI_CheckedChanged);
            // 
            // text_CheckOutRSSI
            // 
            this.text_CheckOutRSSI.Enabled = false;
            this.text_CheckOutRSSI.Location = new System.Drawing.Point(126, 49);
            this.text_CheckOutRSSI.Name = "text_CheckOutRSSI";
            this.text_CheckOutRSSI.Size = new System.Drawing.Size(34, 21);
            this.text_CheckOutRSSI.TabIndex = 155;
            this.text_CheckOutRSSI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_CheckOutRSSI_KeyPress);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Enabled = false;
            this.label34.Location = new System.Drawing.Point(15, 53);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(107, 12);
            this.label34.TabIndex = 154;
            this.label34.Text = "微弱信号强度阀值:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(167, 12);
            this.label6.TabIndex = 158;
            this.label6.Text = "历史采集信息存储精度(分钟):";
            // 
            // com_PressMinuteCollect
            // 
            this.com_PressMinuteCollect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_PressMinuteCollect.FormattingEnabled = true;
            this.com_PressMinuteCollect.Items.AddRange(new object[] {
            "5"});
            this.com_PressMinuteCollect.Location = new System.Drawing.Point(184, 133);
            this.com_PressMinuteCollect.Name = "com_PressMinuteCollect";
            this.com_PressMinuteCollect.Size = new System.Drawing.Size(37, 20);
            this.com_PressMinuteCollect.TabIndex = 157;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 77);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(137, 24);
            this.label18.TabIndex = 160;
            this.label18.Text = "串口连接时额外等待间隔\r\n单位:毫秒 (0为不等待):";
            // 
            // com_OtherLoopTime
            // 
            this.com_OtherLoopTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_OtherLoopTime.FormattingEnabled = true;
            this.com_OtherLoopTime.Items.AddRange(new object[] {
            "0",
            "50",
            "100",
            "150",
            "200",
            "250",
            "300",
            "350",
            "400",
            "450",
            "500"});
            this.com_OtherLoopTime.Location = new System.Drawing.Point(157, 79);
            this.com_OtherLoopTime.Name = "com_OtherLoopTime";
            this.com_OtherLoopTime.Size = new System.Drawing.Size(50, 20);
            this.com_OtherLoopTime.TabIndex = 159;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(246, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 12);
            this.label4.TabIndex = 162;
            this.label4.Text = "判定考勤1号基站长度系数:";
            // 
            // com_DistanceForDuty1Per
            // 
            this.com_DistanceForDuty1Per.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.com_DistanceForDuty1Per.FormattingEnabled = true;
            this.com_DistanceForDuty1Per.Items.AddRange(new object[] {
            "0.2",
            "0.3",
            "0.4",
            "0.5",
            "0.6",
            "0.7",
            "0.8",
            "0.9",
            "1"});
            this.com_DistanceForDuty1Per.Location = new System.Drawing.Point(405, 144);
            this.com_DistanceForDuty1Per.Name = "com_DistanceForDuty1Per";
            this.com_DistanceForDuty1Per.Size = new System.Drawing.Size(48, 20);
            this.com_DistanceForDuty1Per.TabIndex = 161;
            // 
            // checkBox_ShowLowPower
            // 
            this.checkBox_ShowLowPower.AutoSize = true;
            this.checkBox_ShowLowPower.Checked = true;
            this.checkBox_ShowLowPower.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ShowLowPower.Location = new System.Drawing.Point(638, 80);
            this.checkBox_ShowLowPower.Name = "checkBox_ShowLowPower";
            this.checkBox_ShowLowPower.Size = new System.Drawing.Size(15, 14);
            this.checkBox_ShowLowPower.TabIndex = 164;
            this.checkBox_ShowLowPower.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(503, 81);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(131, 12);
            this.label35.TabIndex = 163;
            this.label35.Text = "当卡片缺电时发出报警:";
            // 
            // tex_AlarmMaxPerson
            // 
            this.tex_AlarmMaxPerson.Location = new System.Drawing.Point(589, 51);
            this.tex_AlarmMaxPerson.Name = "tex_AlarmMaxPerson";
            this.tex_AlarmMaxPerson.Size = new System.Drawing.Size(30, 21);
            this.tex_AlarmMaxPerson.TabIndex = 166;
            this.tex_AlarmMaxPerson.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tex_AlarmMaxPerson_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(503, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 165;
            this.label5.Text = "超员报警人数:";
            // 
            // tex_AlarmMaxHour
            // 
            this.tex_AlarmMaxHour.Location = new System.Drawing.Point(722, 50);
            this.tex_AlarmMaxHour.Name = "tex_AlarmMaxHour";
            this.tex_AlarmMaxHour.Size = new System.Drawing.Size(24, 21);
            this.tex_AlarmMaxHour.TabIndex = 168;
            this.tex_AlarmMaxHour.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tex_AlarmMaxHour_KeyPress);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(625, 55);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(95, 12);
            this.label36.TabIndex = 167;
            this.label36.Text = "超时报警小时数:";
            // 
            // check_OutWhen1GetRSSI
            // 
            this.check_OutWhen1GetRSSI.AutoSize = true;
            this.check_OutWhen1GetRSSI.Location = new System.Drawing.Point(458, 170);
            this.check_OutWhen1GetRSSI.Name = "check_OutWhen1GetRSSI";
            this.check_OutWhen1GetRSSI.Size = new System.Drawing.Size(15, 14);
            this.check_OutWhen1GetRSSI.TabIndex = 170;
            this.check_OutWhen1GetRSSI.UseVisualStyleBackColor = true;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(246, 171);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(209, 12);
            this.label37.TabIndex = 169;
            this.label37.Text = "只要考勤1号基站收到信号就出井(洞):";
            // 
            // label38
            // 
            this.label38.ForeColor = System.Drawing.Color.Red;
            this.label38.Location = new System.Drawing.Point(246, 185);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(234, 25);
            this.label38.TabIndex = 171;
            this.label38.Text = "仅当考勤1号基站远离其他基站(之间存在信号盲区)时才使用。";
            // 
            // check_IsShowBug
            // 
            this.check_IsShowBug.AutoSize = true;
            this.check_IsShowBug.Location = new System.Drawing.Point(614, 128);
            this.check_IsShowBug.Name = "check_IsShowBug";
            this.check_IsShowBug.Size = new System.Drawing.Size(15, 14);
            this.check_IsShowBug.TabIndex = 173;
            this.check_IsShowBug.UseVisualStyleBackColor = true;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(503, 129);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(107, 12);
            this.label39.TabIndex = 172;
            this.label39.Text = "显示调试错误信息:";
            // 
            // check_SwitchHW
            // 
            this.check_SwitchHW.AutoSize = true;
            this.check_SwitchHW.Enabled = false;
            this.check_SwitchHW.Location = new System.Drawing.Point(359, 259);
            this.check_SwitchHW.Name = "check_SwitchHW";
            this.check_SwitchHW.Size = new System.Drawing.Size(15, 14);
            this.check_SwitchHW.TabIndex = 175;
            this.check_SwitchHW.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(248, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 174;
            this.label1.Text = "翻转红外计数方向:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.SteelBlue;
            this.label2.Location = new System.Drawing.Point(239, 221);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 176;
            this.label2.Text = "红外参数";
            // 
            // checkBox_IsUseHongWai
            // 
            this.checkBox_IsUseHongWai.AutoSize = true;
            this.checkBox_IsUseHongWai.Location = new System.Drawing.Point(250, 238);
            this.checkBox_IsUseHongWai.Name = "checkBox_IsUseHongWai";
            this.checkBox_IsUseHongWai.Size = new System.Drawing.Size(192, 16);
            this.checkBox_IsUseHongWai.TabIndex = 177;
            this.checkBox_IsUseHongWai.Text = "是否连接并开启红外设备与功能";
            this.checkBox_IsUseHongWai.UseVisualStyleBackColor = true;
            this.checkBox_IsUseHongWai.CheckedChanged += new System.EventHandler(this.checkBox_IsUseHongWai_CheckedChanged);
            // 
            // FrmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(757, 324);
            this.Controls.Add(this.checkBox_IsUseHongWai);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.check_SwitchHW);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.check_IsShowBug);
            this.Controls.Add(this.label39);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.check_OutWhen1GetRSSI);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.tex_AlarmMaxHour);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.tex_AlarmMaxPerson);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox_ShowLowPower);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.com_DistanceForDuty1Per);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.com_OtherLoopTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.com_PressMinuteCollect);
            this.Controls.Add(this.checkBox_CheckOutRSSI);
            this.Controls.Add(this.text_CheckOutRSSI);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.com_NullRSSITimes);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.com_Port_Check);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.com_Port_Bote);
            this.Controls.Add(this.com_Port_Data);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.com_Port_Stop);
            this.Controls.Add(this.com_SlickTimes);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.com_Port_port);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.com_ErrorStationTimes);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textDBConnStr);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.textSlick);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.checkBox_IsLinePosition);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textArriveRSSI);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.textServerPort);
            this.Controls.Add(this.com_PressMinute);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label28);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "高级参数设置";
            this.Load += new System.EventHandler(this.FrmSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textArriveRSSI;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textServerPort;
        private System.Windows.Forms.ComboBox com_PressMinute;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_IsLinePosition;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox com_Port_Stop;
        private System.Windows.Forms.ComboBox com_Port_port;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox com_Port_Data;
        private System.Windows.Forms.ComboBox com_Port_Bote;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox com_Port_Check;
        private System.Windows.Forms.TextBox textSlick;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox textDBConnStr;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox com_ErrorStationTimes;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox com_SlickTimes;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox com_NullRSSITimes;
        private System.Windows.Forms.CheckBox checkBox_CheckOutRSSI;
        private System.Windows.Forms.TextBox text_CheckOutRSSI;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox com_PressMinuteCollect;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox com_OtherLoopTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox com_DistanceForDuty1Per;
        private System.Windows.Forms.CheckBox checkBox_ShowLowPower;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox tex_AlarmMaxPerson;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tex_AlarmMaxHour;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.CheckBox check_OutWhen1GetRSSI;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.CheckBox check_IsShowBug;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.CheckBox check_SwitchHW;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_IsUseHongWai;
    }
}