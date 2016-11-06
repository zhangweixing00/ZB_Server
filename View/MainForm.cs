using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using PersonPositionServer.Common;
using PersonPositionServer.Model;
using PersonPositionServer.StaticService;
using System.IO;
using System.Diagnostics;
using Smart2000.Net.App;
using System.Net.Sockets;
namespace PersonPositionServer.View
{
    public partial class MainForm : Form
    {
        private NotifyIcon mainTaryIcon;
        //轮询
        private Thread LoopThread;//轮询线程

        private Thread Loop_do_Thread;
        public static bool LoopKey = false;//开关量

        public HardChannel hardChannel;//硬件通道抽象父类
        private int WaitTimes = 0;//每帧的等待次数。当收到新的一帧后就清0
        private FrmHardCommand FHC;
        private FrmRelation FR;
        private FrmDelHardwareHistory FDHH;
        private bool WhenEndIsClearTempData = false;//当结束巡检时是否清除临时数据
        private W_Set hhset;
        private testcard hhtestcard;
        private bool start_do = false;
        public Mutex MutexCheckName = new Mutex();
        private System.Timers.Timer tmserver = new System.Timers.Timer();

        private long[] keyhandles = new long[8];
        public bool loopended = false;
        public ManualResetEvent mreloopthreadexited = new ManualResetEvent(false);
        private long keyNumber = 0;
        private long Rtn = 1;
        private long requestFromKey = 0;


        #region 无意义的修改、取消按钮事件

        private void btn_OtherCancel_Click(object sender, EventArgs e)
        {
            if (panel_Other.Enabled)
            {
                InitMainSettingUI();
                btn_ConnEquip.Enabled = true;
                panel_Other.Enabled = false;
                btn_OtherOK.Text = "修改";
            }
        }

        private void btn_OtherOK_Click(object sender, EventArgs e)
        {
            //Global.Isnew = checkBox1.Checked;  
            
            try
            {
                if (btn_OtherOK.Text == "修改")
                {
                    btn_OtherOK.Text = "保存";
                    btn_ConnEquip.Enabled = false;
                    panel_Other.Enabled = true;
                }
                else
                {
                    if (radio_Net.Checked)
                    {
                        Global.IsNetConnect = true;
                    }
                    else
                    {
                        Global.IsNetConnect = false;
                    }

                    if (checkBox1 .Checked )                   
                    {
                        Global.Isnew = true;
                    }
                    else
                    {
                        Global.Isnew = false;
                    }
                    Global.LoopTime = Convert.ToInt32(com_Other_LoopTime.Text);
                    Global.AutoRunApp = check_AutoRunApp.Checked;
                    Global.AutoStart = check_AutoStart.Checked;
                    if (check_AutoRunApp.Checked)
                    {
                        CommonFun.SetAutoRunWhenStart(true, "PersonPositionServer.exe", AppDomain.CurrentDomain.BaseDirectory + "PersonPositionServer.exe");
                    }
                    else
                    {
                        CommonFun.SetAutoRunWhenStart(false, "PersonPositionServer.exe", AppDomain.CurrentDomain.BaseDirectory + "PersonPositionServer.exe");
                    }

                    btn_OtherOK.Text = "修改";
                    btn_ConnEquip.Enabled = true;
                    panel_Other.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace);
            }
        }

        #endregion

        #region 菜单栏单击事件

        private void 命令调试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FHC = new FrmHardCommand(this);
            FHC.ShowDialog(this);
            FHC = null;
        }

        private void 授权信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRegInfo ri = new FrmRegInfo();
            ri.ShowDialog(this);
        }

        private void 注册产品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReg fr = new FrmReg();
            fr.ShowDialog();
        }

        private void 调整父子基站关系ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FR = new FrmRelation(this);
            FR.ShowDialog(this);
            FR = null;
        }

        private void 产生测试数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCreateTestData frmCTD = new FrmCreateTestData();
            frmCTD.Show();
        }

        private void 清空硬件储存历史数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FDHH = new FrmDelHardwareHistory(this);
            FDHH.ShowDialog(this);
            FDHH = null;
        }

        private void 备份数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Title = "备份数据库";
            SFD.InitialDirectory = @"D:\";
            SFD.Filter = "数据库备份文件(*.DBBackup)|*.DBBackup";
            SFD.FileName = "DBBackup_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString();
            if (SFD.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    DB_Service.ExecuteSQL("backup database PersonPosition to disk = '" + SFD.FileName + "'");
                    MessageBox.Show("备份数据库成功！\n\n请妥善保存备份，以便日后恢复。", "备份数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("备份数据库失败！\n\n请不要保存在C盘。\n\n" + ex.Message, "备份数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void 还原数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "还原数据库";
            OFD.InitialDirectory = @"D:\";
            OFD.Filter = "数据库备份文件(*.DBBackup)|*.DBBackup";
            if (OFD.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    DB_Service.ExecuteSQL_Master("Restore Database PersonPosition from disk = '" + OFD.FileName + "'");
                    MessageBox.Show("  还原数据库成功！   ", "还原数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("还原数据库失败！\n\n请退出并重新启动程序，并在尚未启动服务器时重试。\n\n" + ex.Message, "还原数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void 清空数据表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDBRecover frmDB = new FrmDBRecover();
            frmDB.ShowDialog(this);
        }

        private void 还原数据库为出厂状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要还原数据库为出厂状态吗？\n\n请谨慎进行此操作。这将丢失所有用户数据！", "还原数据库为出厂设置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"\InitDB.bat";
                    p.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #endregion


       
        
        public MainForm(NotifyIcon _mainTaryIcon)
        {
            InitializeComponent();
            mainTaryIcon = _mainTaryIcon;
            mainTaryIcon.MouseClick += new MouseEventHandler(mainTaryIcon_MouseClick);
            Protocol_Service.StationNormal += new StationNormalEventHandler(Protocol_Service_StationNormal);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.Text = Application.ProductName + (Global.IsTempVersion ? "(演示版)" : "") + "  Ver:" + Application.ProductVersion;
            this.Text = Global.Product + (Global.IsTempVersion ? "(演示版)" : "") + "  Ver:" + Application.ProductVersion;
            //根据信息初始化主参数设置界面
            InitMainSettingUI();
            start_do = false;
            Socket_Service.GiveMainfrom(this);
            //启动巡检线程
            LoopThread = new Thread(new ThreadStart(SendLoop));
            LoopThread.Start();

            if (Global.AutoStart)
            {
                Thread.Sleep(500);
                btn_ConnEquip_Click(sender, e);

            }

            checkBox1.Checked = Global.Isnew;  

        }

        private void InitMainSettingUI()
        {
            if (Global.IsNetConnect)
            {
                radio_Net.Checked = true;
            }
            else
            {
                radio_Port.Checked = true;
            }
            check_AutoRunApp.Checked = Global.AutoRunApp;
            check_AutoStart.Checked = Global.AutoStart;
            com_Other_LoopTime.SelectedIndex = com_Other_LoopTime.FindStringExact(Global.LoopTime.ToString(), -1);
        }

        public void AddClientList(string LogName, string IP, int port, string Time)
        {
            this.listView1.Items.Add(new ListViewItem(new string[] { LogName, IP, port.ToString(), Time }));
        }
        /// <summary>
        /// 刷新客户端列表，对clientlist中的每个socket，若其IP地址和端口与主界面中
        /// 的IP地址和端口匹配则保留，否则主界面中移除该用户
        /// </summary>
        /// <param name="ClientList"></param>
        public void RefreshClientList(ArrayList ClientList)
        {
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                try
                {
                    string IP = this.listView1.Items[i].SubItems[1].Text;
                    int port = Convert.ToInt32(this.listView1.Items[i].SubItems[2].Text);
                    bool IsHad = false;
                    for (int j = 0; j < ClientList.Count; j++)
                    {
                        System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket)ClientList[j];
                        System.Net.IPEndPoint endPoint = (System.Net.IPEndPoint)socket.RemoteEndPoint;
                        if (IP == endPoint.Address.ToString() && port == endPoint.Port)
                        {
                            IsHad = true;
                            break;
                        }
                    }
                    if (!IsHad)
                    {
                        this.listView1.Items[i].Remove();
                    }
                }
                catch (Exception ex)
                {
                    if (Global.IsShowBug)
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器刷新客户端列表错误");
                }
            }
        }

        /// <summary>
        /// 根据ErrorStationList刷新界面上的ListView
        /// 只根据ErrorStationList刷新显示。不修改ErrorStationList
        /// </summary>
        private void RefreshErrorStationList()
        {
            //为开始更新控件做停止重绘的准备
            listView_ErrorStation.BeginUpdate();

            listView_ErrorStation.Items.Clear();
            DataRow[] StationRows = BasicData.GetStationTableRows("", true);
            for (int i = 0; i < StationRows.Length; i++)
            {
                try
                {
                    int StationID = Convert.ToInt32(StationRows[i]["ID"]);
                    string StationName = StationRows[i]["Name"].ToString();
                    string StationType = StationRows[i]["StationType"].ToString();
                    string StationFunction = StationRows[i]["StationFunction"].ToString();
                    string StationIP = StationRows[i]["IP"].ToString();
                    string StationPort = StationRows[i]["Port"].ToString();
                    string StationDuty = StationRows[i]["DutyOrder"].ToString();
                    string StationRepairRSSI = StationRows[i]["RepairRSSI"].ToString();
                    if (Protocol_Service.ErrorStationList.ContainsKey(StationID))
                    {
                        int times = Protocol_Service.ErrorStationList[StationID];
                        if (times >= Global.ErrorStationTimes)
                        {
                            listView_ErrorStation.Items.Add(new ListViewItem(new string[9] { StationID.ToString(), StationName, StationType, StationFunction, StationIP, StationPort, StationDuty, StationRepairRSSI, "故障" }, 0, Color.Red, Color.Gainsboro, null));
                        }
                        else if (times == 0)
                        {
                            listView_ErrorStation.Items.Add(new ListViewItem(new string[9] { StationID.ToString(), StationName, StationType, StationFunction, StationIP, StationPort, StationDuty, StationRepairRSSI, "未知" }, 0, Color.Orange, Color.Gainsboro, null));
                        }
                        else
                        {
                            listView_ErrorStation.Items.Add(new ListViewItem(new string[9] { StationID.ToString(), StationName, StationType, StationFunction, StationIP, StationPort, StationDuty, StationRepairRSSI, "正常" }, 0, Color.LimeGreen, Color.Gainsboro, null));
                        }
                    }
                    else
                    {
                        listView_ErrorStation.Items.Add(new ListViewItem(new string[9] { StationID.ToString(), StationName, StationType, StationFunction, StationIP, StationPort, StationDuty, StationRepairRSSI, "未知" }, 0, Color.Orange, Color.Gainsboro, null));
                    }
                }
                catch (Exception ex)
                {
                    if (Global.IsShowBug)
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器刷新故障基站列表错误");
                }
            }
            //更新完毕。绘制控件
            listView_ErrorStation.EndUpdate();
        }

        /// <summary>
        /// 判断登录用户是否已经登录并且是否超出最大登录限额
        /// 0 登录成功 1超出最大限额 2用户已登录
        /// </summary>
        /// <param name="LogName"></param>
        /// <returns></returns>
        public int CheckLogName(string LogName)
        {
            MutexCheckName.WaitOne();
            int rtn = 0;
            //这里是在客户端登录前验证。所以，人数要少1个
            if (this.listView1.Items.Count >= Global.MaxUserNum)
            {
                rtn = 1;
            }
            else
            {
                for (int i = 0; i < this.listView1.Items.Count; i++)
                {
                    try
                    {
                        if (this.listView1.Items[i].SubItems[0].Text == LogName)
                        {
                            rtn = 2;
                            break;
                        }
                    }
                    catch
                    { }
                }

            }
            MutexCheckName.ReleaseMutex();
            return rtn;
        }

        /// <summary>
        /// 建立连接按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        public long  mem_read()
        {
            int startAddr = 0;

            int lenth = 10;

            
            
            byte[] pbffer = new byte[lenth];

            //文件存储区读取操作，startAddr代表开始地址（int），pbffer代表读取出的数据(byte)，数组长度代表读取数据的长度。
            return Rtn = Smart2000App.Smart2000ReadFileStorage(keyhandles[0], startAddr, pbffer);


        }

        private int mm_check()
        {
            long  userPin1 = 0x46DA6377;
            long  userPin2 = 0xBBB006FB;
            long  userPin3 = 0x6C056AB5;
            long  userPin4 = 0x49DF0A63;
            Rtn = Smart2000App.Smart2000Open(keyhandles[0], userPin1, userPin2, userPin3, userPin4, ref requestFromKey);
            if (0 != Rtn)
            {

                MessageBox.Show("oooooooooo");
                
                //lst_MSG.Items.Insert(0, String.Format("Smart2000 Open  Failed, Errorcode： = {0}", Smart2000App.Smart2000GetLastError()));
                return 1 ;
            }
            //txt_RequestA.Text = requestFromKey.ToString();
            //txt_RequestB.Text = requestFromKey.ToString();
            //lst_MSG.Items.Insert(0, String.Format("Smart2000 Open Successfully"));
            return 0;
        }



        public void send_OpenDoor2()
        {

            //byte[] buf = new byte[6];


            //hardChannel.Send_Safe(Protocol_Service.CommandType.LightUp, CommandFactory.GetLightUpCommand(20, 4));






        }

        public void send_OpenDoor1()
        {

            //byte[] buf = new byte[6];


            //hardChannel.Send_Safe(Protocol_Service.CommandType.LightUp, CommandFactory.GetLightUpCommand(20, 8));






        }
        
        
        public void send_OpenDoor()
        {

            //byte[] buf = new byte[6];


            //hardChannel.Send_Safe(Protocol_Service.CommandType.LightUp, CommandFactory.GetLightUpCommand(20, 1));
            
            




        }
        public void send_CloseDoor()
        {

            //byte[] buf = new byte[6];
            //hardChannel.Send(buf);

           // hardChannel.Send_Safe(Protocol_Service.CommandType.LightUp, CommandFactory.GetLightUpCommand(20, 0));


        }
        
        private int  two_mm_chk()
        {
            Rtn = Smart2000App.Smart2000Verify(keyhandles[0], requestFromKey);
            if (0 != Rtn)
            {
                MessageBox.Show("tttttttttttt");
                return 1;
            }

            return 0;
        }

        
        private void btn_ConnEquip_Click(object sender, EventArgs e)
        {
           
            if (btn_ConnEquip.Text == "启动服务器")
            {
                btn_ConnEquip.Enabled = false;
                Protocol_Service .fs.Close();
                //Protocol_Service.sw.Close();
                Protocol_Service .fs   =   new FileStream("wuabcd", FileMode.Create);
                //Protocol_Service.sw.Dispose();
                Protocol_Service.sw     = new StreamWriter(Protocol_Service.fs);
                if (File.Exists(@"d:\serverlog.txt"))
                {
                    Protocol_Service.fslog = new FileStream(@"d:\serverlog.txt", FileMode.Open, FileAccess.Write);
                    Protocol_Service.swlog = new StreamWriter(Protocol_Service.fslog);
                }
                
                string appID = "zifeng";
                /*Rtn = Smart2000App.Smart2000Find(appID, out keyhandles, ref keyNumber);
                if (0 != Rtn)
                {
                    //lst_MSG.Items.Insert(0, String.Format("Smart2000 Find failed, Errorcode： = {0}", Smart2000App.Smart2000GetLastError()));
                    MessageBox.Show("未找到加密锁，请检查加密锁是否锁死");
                    return;
                }


                if (mm_check()==1 || two_mm_chk()==1)
                {
                    Socket_Service.CloseAllClientSocket();
                    
                    {
                        Loop_do_Thread.Abort();
                        LoopThread.Abort();
                    }
                
                };*/

                try
                {
                    //刷新硬件配置参数
                    if (CommonFun.RefreshHardConfig())
                    {
                        //检查并更新数据库
                        DB_Service.CheckDBAndUpdate();
                        //初始化5个常用表
                        BasicData.InitALLTable();
                        //初始化定位信息表和数据采集表中最后一条记录的时间
                        if (DB_Service.InitLastInsertHistory_Position())
                        {
                            if (Global.IsNetConnect)
                            {
                                //网口连接，则实例化硬件通道类的串口子类
                                hardChannel = new TcpSocket_Service();
                            }
                            else
                            {
                                //串口连接，则实例化硬件通道类的网口子类
                                hardChannel = new Port_Service(Global.Port_port, Convert.ToInt32(Global.Port_bote), Global.Port_check, Convert.ToInt32(Global.Port_data), Global.Port_stop);
                            }
                            //连接监听
                            hardChannel.ConnectAndListen();
                            //设置基站时间
                            //if (Global .Isnew )
                                SetStationTime();
                            //初始化故障基站列表
                            Protocol_Service.InitErrorList();
                            //启动故障基站列表刷新计时器
                            timer_ErrorStation.Enabled = true;
                            //监听本地端口
                            Socket_Service.StartListenConnectOfClient(Global.ServerPort);
                            //连接完毕,锁定UI
                            UI_OnConn();
                            //从上次的临时数据文件重载Protocol_Service.Position的数据
                            CommonFun.ReloadPositionFromFile();
                            //注册消息事件
                            hardChannel.Event_GotMessage += new GotMessageEventHandler(hardChannel_GotMessage);
                            //开始循环轮询
                            LoopKey = true;
                            start_do = false ;
                            
                            //启动服务端对客户端的心跳监听
                            beginclientticklisten();
                        }
                        else
                        {
                            MessageBox.Show(this, "无法取得定位信息历史表或采集信息历史表中最新的时间。请检查数据库连接是否正常后重试。", "连接数据库");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "启动服务器异常");
                    if (hardChannel != null)
                    {
                        hardChannel.DisConnect();
                        hardChannel = null;
                    }

                     start_do = true;
                     
                    if(Global .Isnew  )  
                        Loop_do_Thread.Abort();
                    //恢复协议类变量为初始值
                    Protocol_Service.ResumeALL();
                    //清空所有表
                    BasicData.DisableALLTable();
                    //连接关闭，解锁UI
                    UI_OffConn();
                    //关闭故障基站列表刷新计时器
                    timer_ErrorStation.Enabled = false;
                }
            }
            else
            {
                switch (MessageBox.Show(this, "您确定要停止服务并保留最近一次的数据吗？\n\n注:停止服务不会影响到历史数据，但建议您不要在人员频繁进出洞时停止服务器，以免影响考勤准确。\n\n\n是 - 停止服务并保留最近一次的数据\n\n否 - 停止服务但不保留最近一次的数据", "停止所有服务", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
                {
                    case DialogResult.Yes:
                        WhenEndIsClearTempData = false;
                        break;
                    case DialogResult.No:
                        WhenEndIsClearTempData = true;
                        break;
                    case DialogResult.Cancel:
                        return;
                }

                
                //Loop_do_Thread.Abort();
                btn_ConnEquip.Enabled = false;
                //关闭循环轮询
                LoopKey = false;
                start_do = true;
                loopended = false;
                mreloopthreadexited.Reset();
                //等待最后一次考勤定位线程发送定位消息
                if (loopended)
                {
                    Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpdatePosition, "False", "", "", "", "Stop!False", "False", "False", "False", "!");
                }
                else
                {
                    mreloopthreadexited.WaitOne();
                    //通知客户端服务停止的定位消息需等待若干秒再发送，避免由于普通定位消息延迟，比特殊定位消息晚到
                    Thread.Sleep(5000);
                    //等待返回，则表明最后一次定位考勤消息已发送
                    Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpdatePosition, "False", "", "", "", "Stop!False", "False", "False", "False", "!");
                    //复位等待事件为无信号
                    mreloopthreadexited.Reset();
                }
                if (Global.Isnew)
                    Loop_do_Thread.Abort();
               // IsJustStopLoop = false;
                Socket_Service.StopListenConnectOfClient();
                //清空临时数据
                if (WhenEndIsClearTempData)
                {
                    File.Delete(Global.ConfigTempData);
                }
                //关闭故障基站列表刷新计时器
                timer_ErrorStation.Enabled = false;
                hardChannel.DisConnect();
                hardChannel = null;

                //恢复协议类变量为初始值
                Protocol_Service.ResumeALL();
                if (Protocol_Service.fslog != null)
                {
                    Protocol_Service.fslog.Close();
                    Protocol_Service.fslog.Dispose();
                }
                
                //清空所有表
                BasicData.DisableALLTable();
                //连接关闭，解锁UI
                UI_OffConn();
                
            }
        }

        void hardChannel_GotMessage(byte[] buffer)
        {
            //只要收到完整的一帧，就将等待次数清0
             this.WaitTimes = 0;
            //送达协议类分析数据
            Protocol_Service.AnalysisAndOperate(buffer);
        }

        public void beginclientticklisten()
        {

            tmserver.Elapsed += new System.Timers.ElapsedEventHandler(tmserver_Elapsed);
            tmserver.Interval = Global.CheckClientConnInterval;
            tmserver.Enabled = true;
        }
        private void tmserver_Elapsed(object sender, EventArgs e)
        {
            try
            {
                lock (Socket_Service.htusertick.SyncRoot)
                {
                    //修改hashtable不能在foreach(object key in ht.keys)枚举时修改
                    //创建临时object数组，将hashtable中的健拷贝到临时数组
                    object[] keyarray = new Object[Socket_Service.htusertick.Keys.Count];
                    Socket_Service.htusertick.Keys.CopyTo(keyarray, 0);
                    ListBox lb = null;
                    GroupBox gbtemp = null;
                   
                    foreach (object key in keyarray)
                    {
                        Socket_Service.htusertick[key] = (int)Socket_Service.htusertick[key] + 1;
                        //Console.WriteLine(key.ToString() + "---" + Socket_Service.htusertick[key].ToString());
                        if (lb != null)
                        {

                            string clienttickip = string.Empty;
                            string clienttickport = string.Empty;
                            Socket sc = (Socket)Socket_Service.htusersocket[key];
                            if (sc.Connected)
                            {
                                System.Net.IPEndPoint ipsc = (System.Net.IPEndPoint)sc.RemoteEndPoint;
                                clienttickip = ipsc.Address.ToString();
                                clienttickport = ipsc.Port.ToString();
                            }
                            else
                            {
                                clienttickip = "异常IP";
                                clienttickport = "异常端口";
                            }
                            lb.Items.Add("账户" + key.ToString() + "(" + clienttickip + "--" + clienttickport + ")心跳值" + Socket_Service.htusertick[key].ToString() + " at" + DateTime.Now.ToString());


                        }
                        if ((int)Socket_Service.htusertick[key] >= Global.DisConnClientTime)//客户端未发送心跳已达一分钟
                        {

                            //在主界面刷新客户端列表
                            for (int i = 0; i < listView1.Items.Count; i++)
                            {
                                string listviewuser = listView1.Items[i].SubItems[0].Text;
                                string listviewip = listView1.Items[i].SubItems[1].Text;
                                if (listviewuser == key.ToString())
                                {
                                    listView1.Items.Remove(listView1.Items[i]);
                                }
                            }
                            //强制断开该连接
                            Socket_Service.DisConnectClient(key.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Global.IsShowBug)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "检测心跳包错误");
                }
            }
        }
        private void Loop_Do_DutyAndPosition()
        {

            // DateTime tempTime = DateTime.Now;
            while (true)
            {
                Thread.Sleep(Global.LoopTime);

                loopended = false;
                if (LoopKey)
                {

                    //if(mem_read ()==0)
                    if (Global.Isnew)
                    {

                        Protocol_Service.Do_DutyAndPosition(DateTime.Now);

                    }
                }
                loopended = true;
                mreloopthreadexited.Set();
            }
        }


        private void SendLoop()
        {
            //是否刚刚停止巡检。这个变量是为了控制在刚结束巡检之后发送一次服务器状态
            bool IsJustStopLoop = false;
            //进入循环体
            while (true)
            {
                while (LoopKey)
                {
                    //如果硬件调试窗体、修改基站关系窗体、清空基站硬件储存数据窗体出现，则不巡检。但是线程还是继续运行
                    if (FHC != null || FR != null || FDHH != null || hhset != null)
                        continue;
                    //开始巡检基站
                    // DateTime tempTime = DateTime.Now;
                    label_LoopStation.Text = "检查故障基站...";
                    //开始检查故障基站
                    try
                    {
                        DataRow[] rows_ErrorStation = BasicData.GetStationTableRows("", true);
                        for (int k = 0; k < rows_ErrorStation.Length; k++)
                        {

                            int StationID = Convert.ToInt32(rows_ErrorStation[k]["ID"]);
                            if (Protocol_Service.ErrorStationList.ContainsKey(StationID))
                            {
                                int ErrorTimes = Protocol_Service.ErrorStationList[StationID];
                                if (ErrorTimes < Global.ErrorStationTimes)
                                {
                                    //正常范围中
                                    Protocol_Service.ErrorStationList[StationID] = ErrorTimes + 1;
                                    ////为了适应承德的基站无卡时不发数据的BUG，临时加的
                                    //if (Protocol_Service.ErrorStationList[StationID] == Global.ErrorStationTimes - 1)
                                    //{
                                    //    if (Global.IsNetConnect && rows_ErrorStation[k]["StationType"].ToString() == "网关基站")
                                    //    {
                                    //        if (TcpSocket_Service.CmdPing(rows_ErrorStation[k]["IP"].ToString()) == "连接")
                                    //        {
                                    //            Protocol_Service.ErrorStationList[StationID] = 1;
                                    //        }
                                    //    }
                                    //}
                                }
                                else if (ErrorTimes == Global.ErrorStationTimes)
                                {
                                    //进入错误 写数据库
                                    DataTable tempTable = DB_Service.GetTable("tempTable", "select * from AlarmMachineTable where StationID = " + StationID + " and ResumeTime is null");
                                    if (tempTable.Rows.Count == 0)
                                    {
                                        //错误写入数据库
                                        DB_Service.ExecuteSQL("insert into AlarmMachineTable (StationID,IsReaded,ErrorStartTime,Reason) values (" + StationID + ",'False','" + DateTime.Now + "','基站故障')");
                                    }
                                    //如果是网关基站，则开始自动重连
                                    if (Global.IsNetConnect && rows_ErrorStation[k]["StationType"].ToString() == "网关基站")
                                    {
                                        TcpSocket_Service tcp = (TcpSocket_Service)hardChannel;
                                        tcp.ReConnectInNewThread(rows_ErrorStation[k]["IP"].ToString(), Convert.ToInt32(rows_ErrorStation[k]["Port"]));
                                    }
                                    //设置成错误处理后
                                    Protocol_Service.ErrorStationList[StationID] = Global.ErrorStationTimes + 1;
                                }
                            }
                            else
                            {
                                Protocol_Service.ErrorStationList.Add(StationID, 0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Global.IsShowBug)
                            System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器巡检中检查错误基站错误");
                        /*如果检查基站时错误，则跳出检查*/
                    }
                    DataRow[] rows;
                    if (Global.IsNetConnect)
                    {
                        //rows = BasicData.GetStationTableRows("StationType = '网关基站'",true);
                        
                        if (Global .Isnew )
                            rows = BasicData.GetStationTableRows("StationType = '网关基站' or StationType = '无线基站'", true);
                        else
                            rows = BasicData.GetStationTableRows("StationType = '网关基站'", true);
                    }
                    else
                    {
                        rows = BasicData.GetStationTableRows("StationType = 'Can基站'", true);
                    }
                    //遍历所有指定类型的基站
                    //Global.Isloopend = false ;
                    for (int i = 0; i < rows.Length; i++)
                    {

                        long dt = DateTime.Now.Ticks - Global.OpenedTick;
                        try
                        {


                            if (Global.DoorOOpened)
                            {

                                //long dt = DateTime.Now.Ticks - Global.OpenedTick;
                                
                                if(dt >(20*10000000))
                                {
                                    
                                    //Thread.Sleep(200);
                                    Global.DoorOOpened =false ;
                                     
                                    send_CloseDoor() ;
                                    //Thread.Sleep(200);
                                }
                                
                            
                            }

                            dt = DateTime.Now.Ticks - Global.AlarmTick ;

                            if (dt > (20 * 10000000))
                            {

                                //long dt = DateTime.Now.Ticks - Global.OpenedTick;

                                //if (dt > (20 * 10000000))
                                {

                                   /// Thread.Sleep(200);
                                    //Global.DoorOOpened = false;
                                    
                                    send_CloseDoor();
                                  // Thread.Sleep(200);
                                }


                            }
                            //如果这时停止巡检 则立即跳出
                            if (!LoopKey)
                                break;
                            //只有当是人员定位基站，才巡检
                            if (rows[i]["StationFunction"].ToString() == "人员定位")
                            {
                                int aimStationID = Convert.ToInt32(rows[i]["ID"]);

                                DataRow[] rows_coun = Protocol_Service.PositionTable.Select("StationID = " + rows[i]["ID"]);

                                label_LoopStation.Text = "巡检 " + aimStationID.ToString() + " 号基站";
                                //byte[] CommandBytes;
                                ////先判断产品的合法性
                                //char[] passwordList = Global.HardSN.ToCharArray();
                                //if (passwordList.Length == 6)
                                //{
                                //    CommandBytes = CommandFactory.GetLoopCommand(aimStationID, 0x55 ^ (int)passwordList[0], 0x55 ^ (int)passwordList[1], 0x55 ^ (int)passwordList[2], 0x55 ^ (int)passwordList[3], 0x55 ^ (int)passwordList[4], 0x55 ^ (int)passwordList[5]);
                                //}
                                //else
                                //{
                                //    Protocol_Service.HardLawful = 0;
                                //    break;
                                //}
                                //发送巡检前先将“巡检返回处理完毕标志”设置为假





                                Global.IsOperateComplete = false;
                                //非安全的发送巡检命令


                               
                                // hardChannel.Send(CommandFactory.GetNewLoopCommand(aimStationID, Protocol_Service.InMineList.Count));
                                //判断是否是连续包、并且每包是否已经处理完毕
                               
                                // while (Protocol_Service.IsContinue || !Global.IsOperateComplete)
                                
                                WaitTimes = 0;

                                if (Global.IsNetConnect) 
                                {

                                    if (Global.Isnew)
                                    {
                                        hardChannel.Send(CommandFactory.GetSetInMineNumCommand(aimStationID, rows_coun.Length));

                                    }
                                    else 
                                    {
                                        byte[] CommandBytes;
                                        //先判断产品的合法性
                                        char[] passwordList = Global.HardSN.ToCharArray();
                                        if (passwordList.Length == 6)
                                        {
                                            CommandBytes = CommandFactory.GetLoopCommand(aimStationID, 0x55 ^ (int)passwordList[0], 0x55 ^ (int)passwordList[1], 0x55 ^ (int)passwordList[2], 0x55 ^ (int)passwordList[3], 0x55 ^ (int)passwordList[4], 0x55 ^ (int)passwordList[5]);
                                        }
                                        else
                                        {
                                            Protocol_Service.HardLawful = 00;
                                            break;
                                        }
                                        //发送巡检前先将“巡检返回处理完毕标志”设置为假
                                        Global.IsOperateComplete = false;
                                        //非安全的发送巡检命令
                                        hardChannel.Send(CommandBytes);
                                    }    
                                        
                                    WaitTimes = 0; Protocol_Service.IsContinue = true;
                                    int times = 0;
                                    while (Protocol_Service.IsContinue || !Global.IsOperateComplete)
                                    {

                                        times++;
                                        // this.WaitTimes++;
                                        Thread.Sleep(10);
                                        if (times  >= 20)
                                        {
                                            
                                            break;//如果500毫秒内一直都在处理数据，则跳出，继续巡检下一个基站
                                        }
                                    }
                                }
                                else 
                                {
                                    if (Global.Isnew)
                                        hardChannel.Send(CommandFactory.GetSetInMineNumCommand(aimStationID, rows_coun.Length));
                                    else
                                    {

                                        byte[] CommandBytes;
                                        //先判断产品的合法性
                                        char[] passwordList = Global.HardSN.ToCharArray();
                                        if (passwordList.Length == 6)
                                        {
                                            CommandBytes = CommandFactory.GetLoopCommand(aimStationID, 0x55 ^ (int)passwordList[0], 0x55 ^ (int)passwordList[1], 0x55 ^ (int)passwordList[2], 0x55 ^ (int)passwordList[3], 0x55 ^ (int)passwordList[4], 0x55 ^ (int)passwordList[5]);
                                        }
                                        else
                                        {
                                            Protocol_Service.HardLawful = 00;
                                            break;
                                        }
                                        //发送巡检前先将“巡检返回处理完毕标志”设置为假
                                        Global.IsOperateComplete = false;
                                        //非安全的发送巡检命令
                                        hardChannel.Send(CommandBytes);
                                    }
                                    
                                    WaitTimes = 0; Protocol_Service.IsContinue = true;
                                    while (Protocol_Service.IsContinue || !Global.IsOperateComplete)
                                    {
                                        this.WaitTimes++;
                                        Thread.Sleep(20);
                                        if (this.WaitTimes >= 50)
                                            break;//如果500毫秒内一直都在处理数据，则跳出，继续巡检下一个基站
                                    }
                                }
                                //将每帧的等待次数清0
                                this.WaitTimes = 0;
                                //如果是串口连接并且有额外等待时间，则额外等待
                                if (!Global.IsNetConnect && Global.OtherLoopTime != 0)
                                    Thread.Sleep(Global.OtherLoopTime);
                            }
                            else if (rows[i]["StationFunction"].ToString() == "考勤管理")
                            {
                               /*//只有当是考勤基站，才发送人数信息
                                int aimStationID = Convert.ToInt32(rows[i]["ID"]);

                                DataRow[] rows_coun = Protocol_Service.PositionTable.Select("Area = " + rows[i]["Area"]);

                                label_LoopStation.Text = "巡检 " + aimStationID.ToString() + " 号基站";
                                //非安全的发送设置人数命令
                                hardChannel.Send(CommandFactory.GetSetInMineNumCommand(aimStationID, rows_coun.Length));
                              */

                                //只有当是考勤基站，才发送人数信息
                                int aimStationID = Convert.ToInt32(rows[i]["ID"]);
                                label_LoopStation.Text = "巡检 " + aimStationID.ToString() + " 号基站";
                                //非安全的发送设置人数命令
                                hardChannel.Send(CommandFactory.GetSetInMineNumCommand(aimStationID, Protocol_Service.InMineList.Count));
                                Thread.Sleep(1000);
                            }
                        }
                        catch (Exception ex)
                        {
                            /*如果巡检时错误，则跳出这个基站的巡检*/
                            if (Global.IsShowBug)
                                System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器巡检基站错误");
                        }
                    }
                    label_LoopStation.Text = "巡检完毕,处理数据...";
                    Thread.Sleep(Global.LoopTime);
                    
                    if (!Global.Isnew )
                    {

                        loopended = false;
                        Protocol_Service.Do_DutyAndPosition(DateTime.Now);
                        loopended = true;
                        mreloopthreadexited.Set();
                    
                    }
                  
                    //巡检完毕，先判断硬件合法性
                    //if (Protocol_Service.HardLawful == 0)
                    //{
                    //    MessageBox.Show(this,"对不起，您的产品SN有误。请输入正确的产品SN后再启动服务器。", "产品注册", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    //不合法，则直接调用按钮事件关闭巡检
                    //    btn_ConnEquip_Click(null, null);
                    //}
                    //else
                    //{
                    //合法或者未判断，开始处理数据。
                    //Object thisLock = new Object();

                    // lock (thisLock)

                    if (!start_do && Global.Isnew )
                    {

                        Loop_do_Thread = new Thread(new ThreadStart(Loop_Do_DutyAndPosition));
                        Loop_do_Thread.Start();
                        start_do = true;
                    }

                    {
                        // Protocol_Service.Do_DutyAndPosition(tempTime);
                    }    //从本次巡检信息表（BasicPositionTable）中算出坐标并发出更新消息。如果到达历史存储时间间隔，则存入数据库。同时处理特殊区域与考勤


                    //}
                    label_LoopStation.Text = "空闲...";

                    // Thread.Sleep(Global.LoopTime);
                    //设置标志量
                    //IsJustStopLoop = true;
                }
                //巡检循环之外，判断是否是刚刚巡检结束
                if (IsJustStopLoop)
                {
                    //是刚刚巡检结束，则发送一次服务器状态
                    IsJustStopLoop = false;
                    //发送服务器状态
                    Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpdatePosition, "False", "", "", "", "!0!False", "False", "False", "False", "!");
                    //停止监听
                    Socket_Service.StopListenConnectOfClient();
                    //清空临时数据
                    if (WhenEndIsClearTempData)
                    {
                        File.Delete(Global.ConfigTempData);
                    }
                    //关闭故障基站列表刷新计时器
                    timer_ErrorStation.Enabled = false;
                    hardChannel.DisConnect();
                    hardChannel = null;
                    //恢复协议类变量为初始值
                    Protocol_Service.ResumeALL();
                    //清空所有表
                    BasicData.DisableALLTable();
                    //连接关闭，解锁UI
                    UI_OffConn();
                }
                Thread.Sleep(10);
            }
        }

        private void UI_OnConn()
        {
            label_ConnDBState.Text = "已连接";
            label_ConnDBState.ForeColor = Color.LimeGreen;
            btn_ConnEquip.Enabled = true;
            btn_ConnEquip.Text = "停止所有服务";
            btn_Exit.Enabled = false;
            group_Other.Enabled = false;
            工具ToolStripMenuItem.Enabled = true;
            数据库管理ToolStripMenuItem.Enabled = false;
            注册产品ToolStripMenuItem.Enabled = false;
        }

        private void UI_OffConn()
        {
            label_ConnDBState.Text = "未连接";
            label_ConnDBState.ForeColor = Color.Red;
            btn_ConnEquip.Enabled = true;
            btn_ConnEquip.Text = "启动服务器";
            label_AreaSubject.Text = "未启动";
            listView_ErrorStation.Items.Clear();
            this.label_BasicNum_Position.Text = "0条";
            this.label_BasicNum_Collect.Text = "0条";
            this.label_Insert.Text = "0条";
            this.label_InsertCollect.Text = "0条";
            btn_Exit.Enabled = true;
            group_Other.Enabled = true;
            工具ToolStripMenuItem.Enabled = false;
            数据库管理ToolStripMenuItem.Enabled = true;
            注册产品ToolStripMenuItem.Enabled = true;
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            //关闭所有客户端连接套接字
            Socket_Service.CloseAllClientSocket();
            try
            {
                Loop_do_Thread.Abort();
                LoopThread.Abort();
            }
            catch (Exception ex)
            {
                if (Global.IsShowBug)
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器关闭客户端套接字错误");
            }
            this.mainTaryIcon.Visible = false;
            this.Dispose(true);
            //退出进程
            Process process = Process.GetCurrentProcess();
            if (!process.HasExited)
            {
                process.Kill();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            mainTaryIcon.ShowBalloonTip(2000, "窗体关闭", "服务器端程序转在后台运行。", ToolTipIcon.Info);
        }

        void mainTaryIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void timer_UpdateUI_Tick(object sender, EventArgs e)
        {
            if (LoopKey)
            {
                label_CollectState.Text = "正在巡检...";
                label_CollectState.ForeColor = Color.LimeGreen;
            }
            else
            {
                label_CollectState.Text = "停止巡检";
                label_CollectState.ForeColor = Color.Red;
            }
            if (Protocol_Service.AlarmAreaName != "未启动")
            {
                label_AreaSubject.Text = Protocol_Service.AlarmAreaName;
            }
            else
            {
                label_AreaSubject.Text = "未启动";
            }
            label_BasicNum_Position.Text = Protocol_Service.BasicNum_Position + "条";
            label_BasicNum_Collect.Text = Protocol_Service.BasicNum_Collect + "条";
            label_Analysics.Text = Protocol_Service.AnalysicsNum + "条";
            label_Insert.Text = Protocol_Service.InsertDBNum + "条";
            label_InsertCollect.Text = Protocol_Service.InsertDBNum_Collect + "条";
            label_DutyNum.Text = Protocol_Service.DutyNum + "条";
            if (hardChannel != null && hardChannel.listenThread != null && hardChannel.listenThread.IsAlive && hardChannel.ListenThreadKey)
            {
                label_ListenState.Text = "正在监听";
                label_ListenState.ForeColor = Color.LimeGreen;
                return;
            }
            label_ListenState.Text = "未监听";
            label_ListenState.ForeColor = Color.Red;
        }

        private void checkBox_ShowMore_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ShowMore.Checked)
            {
                checkBox_ShowMore.Text = " 隐藏详细运行信息<<";
                this.Width = 1040;
            }
            else
            {
                checkBox_ShowMore.Text = " 显示详细运行信息>>";
                this.Width = 456;
            }
        }

        private void timer_ErrorStation_Tick(object sender, EventArgs e)
        {
            //刷新故障基站列表
            if (this.WindowState != FormWindowState.Minimized && this.Visible == true && checkBox_ShowMore.Checked)
                RefreshErrorStationList();
        }

        private void btn_AdvSetting_Click(object sender, EventArgs e)
        {
            FrmSetting frmSet = new FrmSetting();
            frmSet.ShowDialog(this);
        }

        void Protocol_Service_StationNormal(int StationID)
        {
            if (hardChannel != null)
            {
                DataRow StationRows = BasicData.GetStationTableRows("ID = " + StationID, true)[0];
                if (StationRows["FatherStationID"] != DBNull.Value)
                {
                    int fatherStationID = Convert.ToInt32(StationRows["FatherStationID"]);
                    hardChannel.Send(CommandFactory.GetGetHistoryDataCommand(fatherStationID, StationID));
                }
                else
                {
                    hardChannel.Send(CommandFactory.GetGetHistoryDataCommand(StationID, StationID));
                }
            }
        }

        public void SetStationTime()
        {
            DataRow[] rows = BasicData.GetStationTableRows("", true);
            for (int i = 0; i < rows.Length; i++)
            {
                int stationID = Convert.ToInt32(rows[i]["ID"]);
                if (rows[i]["FatherStationID"] != DBNull.Value)
                {
                    int fatherStationID = Convert.ToInt32(rows[i]["FatherStationID"]);
                    hardChannel.Send(CommandFactory.GetSetTimeCommand(fatherStationID, stationID, DateTime.Now));
                }
                else
                {
                    hardChannel.Send(CommandFactory.GetSetTimeCommand(stationID, stationID, DateTime.Now));
                }
                Thread.Sleep(200);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //         W_Set

            hhset = new W_Set(this);
            hhset.ShowDialog(this);
            hhset = null;
        }

        private void 设置测试卡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hhtestcard = new testcard (this);
            hhtestcard.ShowDialog(this);
            hhtestcard = null;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           // Global.Isnew = checkBox1.Checked;  
        }

        private void radio_Port_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
