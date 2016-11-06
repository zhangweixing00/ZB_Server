using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using PersonPositionServer.Common;
using PersonPositionServer.StaticService;

namespace PersonPositionServer.View
{
    public partial class FrmRelation : Form
    {
        private MainForm mainform;

        public FrmRelation(MainForm father)
        {
            InitializeComponent();
            this.mainform = father;
        }

        private void btn_AddRelation_Click(object sender, EventArgs e)
        {
            try
            {
                int father = Convert.ToInt32(textBox1.Text);
                int son = Convert.ToInt32(textBox2.Text);
                //安全的发送设置子基站命令
                if (this.mainform.hardChannel.Send_Safe(Protocol_Service.CommandType.AddSon, CommandFactory.GetAddSonCommand(father, son)))
                {
                    MessageBox.Show("添加父子关系成功！");
                }
                else
                {
                    MessageBox.Show("添加父子关系失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace);
            }
        }

        private void btn_DelRelation_Click(object sender, EventArgs e)
        {
            try
            {
                int fatherDelSon = Convert.ToInt32(textBox3.Text);
                int sonDelSon = Convert.ToInt32(textBox4.Text);
                //安全的发送删除子基站命令
                if (this.mainform.hardChannel.Send_Safe(Protocol_Service.CommandType.DelSon, CommandFactory.GetDelSonCommand(fatherDelSon, sonDelSon)))
                {
                    MessageBox.Show("删除父子关系成功！");
                }
                else
                {
                    MessageBox.Show("删除父子关系失败！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace);
            }
        }
    }
}