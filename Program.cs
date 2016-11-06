using System;
using System.Collections.Generic;
using System.Windows.Forms;

using PersonPositionServer.View;
using PersonPositionServer.StaticService;
using PersonPositionServer.Common;

namespace PersonPositionServer
{
    static class Program
    {
       static public MainForm frmMain;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!CommonFun.PrevInstance())
            {
               
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //不对控件的多进程访问安全进行控制
                Control.CheckForIllegalCrossThreadCalls = false;
                //初始化托盘图标
                NotifyIcon MainTrayIcon = new NotifyIcon();
                MainTrayIcon.Icon = global::PersonPositionServer.Properties.Resources.MianICO;
                MainTrayIcon.Text = Application.ProductName + (Global.IsTempVersion ? "" : "");
                MainTrayIcon.Visible = true    ;
                frmMain = new MainForm(MainTrayIcon);
                Application.Run(frmMain);
               //Application.Run(new MainForm(MainTrayIcon));
                //Application.Run(new FrmTemp(MainTrayIcon));
            }
            else
            {
                MessageBox.Show("服务器端数据值守程序已经在运行了！", Application.ProductName + (Global.IsTempVersion ? "(演示版)" : ""), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}