using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PersonPositionServer.StaticService;

namespace PersonPositionServer.View
{
    public partial class FrmCreateTestData : Form
    {
        DataTable CollectStationTable;
        int CollectResult = 0;

        public FrmCreateTestData()
        {
            InitializeComponent();

            CollectStationTable = DB_Service.GetTable("CollectStationTable", "select * from StationTable where StationFunction = '信息采集'");

            for (int i = 0; i < CollectStationTable.Rows.Count; i++)
            {
                com_CollectStationID.Items.Add(CollectStationTable.Rows[i]["ID"]);
            }

            com_CollectLoopTime.SelectedIndex = 0;
        }

        private void btn_MakeCollectLoop_Click(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt32(com_CollectLoopTime.Text) * 1000;
            if (timer1.Enabled)
            {
                btn_MakeCollectLoop.Text = "开始循环产生";
                timer1.Enabled = false;
                btn_MakeCollectOnce.Enabled = true;
            }
            else
            {
                btn_MakeCollectLoop.Text = "结束循环产生";
                timer1.Enabled = true;
                btn_MakeCollectOnce.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btn_MakeCollectOnce_Click(sender, e);
        }

        private void btn_MakeCollectOnce_Click(object sender, EventArgs e)
        {
            if (com_CollectStationID.Text != "" && com_CollectChannelNum.Text != "" && text_CollectValueMin.Text != "" && text_CollectValueMax.Text != "" && text_CollectChannel_ID.Text != "")
            {
                try
                {
                    Random ran = new Random(DateTime.Now.Second);
                    int value = ran.Next(Convert.ToInt32(text_CollectValueMin.Text), Convert.ToInt32(text_CollectValueMax.Text));
                    //给所有客户端发送服务器采集器通道信息更新消息
                    Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpdateCollectChannel, com_CollectStationID.Text, com_CollectChannelNum.Text, value.ToString(), text_CollectChannel_ID.Text, "", "", "", "","");
                    label_CollectResult.Text = Convert.ToString(++CollectResult) + " 条";
                }
                catch
                {
                    if (timer1.Enabled)
                        btn_MakeCollectLoop_Click(sender, e);
                }
            }
            else
            {
                if (timer1.Enabled)
                    btn_MakeCollectLoop_Click(sender, e);
                MessageBox.Show("请输入完整信息.");
            }
        }

        private void com_CollectStationID_SelectedIndexChanged(object sender, EventArgs e)
        {
            com_CollectChannelNum.Items.Clear();
            text_CollectChannel_ID.Text = "";
            string[] ChannelList = CollectStationTable.Select("ID = " + com_CollectStationID.Text)[0]["CollectChannelIDStr"].ToString().Split('-');
            for (int i = 0; i < ChannelList.Length; i++)
            {
                if (ChannelList[i] != "")
                    com_CollectChannelNum.Items.Add(ChannelList[i].Split(':')[0]);
            }
        }

        private void com_CollectChannelNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            text_CollectChannel_ID.Text = "";
            DataRow StationRow = CollectStationTable.Select("ID = " + com_CollectStationID.Text)[0];
            string[] ChannelList = StationRow["CollectChannelIDStr"].ToString().Split('-');
            for (int i = 0; i < ChannelList.Length; i++)
            {
                if (ChannelList[i] != "")
                {
                    string ChannelNum = ChannelList[i].Split(':')[0];
                    string Channel_ID = ChannelList[i].Split(':')[1];
                    if (ChannelNum == com_CollectChannelNum.Text)
                    {
                        text_CollectChannel_ID.Text = Channel_ID;
                    }
                }
            }
            
        }

        private void text_CollectValueMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void text_CollectValueMax_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}