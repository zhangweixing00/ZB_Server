using System;
using System.IO;
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

namespace PersonPositionServer.View
{
    public partial class FrmTemp : Form
    {
        private NotifyIcon mainTaryIcon;
        private Thread LoopThread;//轮询文件线程
        private bool LoopKey = false;//轮询文件开关量
        private MainForm mainform;

        public FrmTemp(NotifyIcon _mainTaryIcon)
        {
            InitializeComponent();
            mainTaryIcon = _mainTaryIcon;
            mainTaryIcon.MouseClick += new MouseEventHandler(mainTaryIcon_MouseClick);
            this.Text = Application.ProductName + (Global.IsTempVersion ? "(演示版)" : "") + "  Ver:" + Application.ProductVersion;

            mainform = new MainForm(_mainTaryIcon);
            MainForm.LoopKey = true;
        }

        private void FrmTemp_Load(object sender, EventArgs e)
        {
            Socket_Service.GiveMainfrom(mainform);
            //开始循环轮询
            LoopThread = new Thread(new ThreadStart(SendLoop));
            LoopThread.Start();
        }

        void mainTaryIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void FrmTemp_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            mainTaryIcon.ShowBalloonTip(2000, "窗体关闭", "服务器端数据值守进程转在后台运行。", ToolTipIcon.Info);
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                LoopThread.Abort();
            }
            catch
            { }
            this.mainTaryIcon.Visible = false;
            this.Dispose(true);
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (btn_Start.Text == "启动服务器")
            {
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
                            //监听本地端口
                            Socket_Service.StartListenConnectOfClient(Global.ServerPort);
                            //连接完毕,锁定UI
                            UI_OnConn();
                            //开始轮询前删除底层数据区文件,不存在不引发异常
                            File.Delete(@"X:\TAG.TXT");
                            //从上次的临时数据文件重载Protocol_Service.Position的数据
                            CommonFun.ReloadPositionFromFile();
                            //开始循环轮询
                            LoopKey = true;
                            //发送服务器状态
                            Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpdatePosition, "True", "", "", "", Protocol_Service.AlarmAreaName + "!0!False", "False", "False", "False", "!");
                        }
                        else
                        {
                            MessageBox.Show("无法取得定位信息历史表或采集信息历史表中最新的时间。请检查数据库连接是否正常后重试。", "连接数据库");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "启动服务器异常");
                    //关闭循环轮询
                    LoopKey = false;
                    //恢复协议类变量为初始值
                    Protocol_Service.ResumeALL();
                    //清空所有表
                    BasicData.DisableALLTable();
                    //连接关闭，解锁UI
                    UI_OffConn();
                }
            }
            else
            {
                //关闭循环轮询
                LoopKey = false;
                //发送服务器状态
                Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpdatePosition, "False", "", "", "", Protocol_Service.AlarmAreaName + "!0!False", "False", "False", "False", "!");
                //停止监听
                Socket_Service.StopListenConnectOfClient();
                //恢复协议类变量为初始值
                Protocol_Service.ResumeALL();
                //清空所有表
                BasicData.DisableALLTable();
                //连接关闭，解锁UI
                UI_OffConn();
            }
        }

        private void SendLoop()
        {
            while (true)
            {
                if (LoopKey)
                {
                    if (File.Exists(@"X:\TAG.TXT"))
                    {
                        label_LoopStation.Text = "收到数据,开始处理...";
                        DateTime tempTime = DateTime.Now;
                        StreamReader SR = new StreamReader(@"X:\TAG.TXT");
                        //读第一行
                        string TimeStr = SR.ReadLine();
                        while (!SR.EndOfStream)
                        {
                            //读下一行
                            string[] DataList = SR.ReadLine().Split(';');
                            if (DataList.Length > 0)
                            {
                                string[] StationInfo = DataList[0].Split(':');
                                int StationID = Convert.ToInt32(StationInfo[0]);
                                int StationIndex = Convert.ToInt32(StationInfo[1]);
                                for (int i = 1; i < DataList.Length; i++)
                                {
                                    string[] TagInfo = DataList[i].Split(':');
                                    DataRow tempRow = Protocol_Service.BasicPositionTable.NewRow();
                                    tempRow["StationID"] = StationID;
                                    tempRow["CardID"] = Convert.ToInt32(TagInfo[0]);
                                    tempRow["RSSI"] = Convert.ToInt32(TagInfo[1]);
                                    //添加到定位临时运算表
                                    Protocol_Service.BasicPositionTable.Rows.Add(tempRow);
                                    //底层的卡片定位信息数量自增
                                    Protocol_Service.BasicNum_Position++;
                                }
                            }
                            else
                            {
                                MessageBox.Show("格式错误");
                            }
                        }
                        //读完后就立即释放资源
                        SR.Close();
                        //开始处理数据。
                        try
                        {
                            //从本次巡检信息表（BasicPositionTable）中算出坐标并发出更新消息。如果到达历史存储时间间隔，则存入数据库。同时处理特殊区域与考勤
                            Protocol_Service.Do_DutyAndPosition(tempTime);
                            ////如果进出洞字符串有人员，则发送消息
                            //if (Protocol_Service.InOutStr != "")
                            //{
                            //    //防止上面发送定位信息或者采集器时抢占通道的等待时间
                            //    Thread.Sleep(200);
                            //    Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_InOutMine, Protocol_Service.InOutStr.Substring(0, Protocol_Service.InOutStr.Length - 1) + "。", tempTime.TimeOfDay.ToString(), "", "", "", "", "", "","");
                            //    Protocol_Service.InOutStr = "";
                            //}
                        }
                        catch(Exception ex)
                        {
                            if (Global.IsShowBug)
                                System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器巡检中处理错误时的错误");
                            /*如果处理数据时错误，则继续巡检*/
                        }
                        //删除文件
                        File.Delete(@"X:\TAG.TXT");
                    }
                    label_LoopStation.Text = "空闲...";
                }
                Thread.Sleep(Global.LoopTime);
            }
        }

        private void UI_OnConn()
        {
            label_ConnDBState.Text = "已连接";
            label_ConnDBState.ForeColor = Color.LimeGreen;
            label_InMine.ForeColor = Color.LimeGreen;
            btn_Start.Text = "停止所有服务";
            btn_Exit.Enabled = false;
        }

        private void UI_OffConn()
        {
            label_ConnDBState.Text = "未连接";
            label_ConnDBState.ForeColor = Color.Red;
            btn_Start.Text = "启动服务器";
            label_InMine.Text = "未知";
            label_InMine.ForeColor = Color.Orange;
            label_InArea.Text = "未知";
            label_InArea.ForeColor = Color.Black;
            label_AreaSubject.Text = "未启动";
            this.label_BasicNum_Position.Text = "0条";
            this.label_BasicNum_Collect.Text = "0条";
            this.label_Insert.Text = "0条";
            this.label_InsertCollect.Text = "0条";
            btn_Exit.Enabled = true;
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
            label_ListenState.Text = "未监听";
            label_ListenState.ForeColor = Color.Red;
        }

        private void btn_AdvSetting_Click(object sender, EventArgs e)
        {
            FrmSetting frmSet = new FrmSetting();
            frmSet.ShowDialog(this);
        }
    }
}