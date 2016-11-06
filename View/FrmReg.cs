using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PersonPositionServer.Common;

namespace PersonPositionServer.View
{
    public partial class FrmReg : Form
    {
        public FrmReg()
        {
            InitializeComponent();
        }

        private void FrmReg_Load(object sender, EventArgs e)
        {
            text_Company.Text = Global.CompanyName;
            text_SN.Text = Global.HardSN;
        }

        private void btn_Reg_Click(object sender, EventArgs e)
        {
            if (text_Company.Text.Trim() != "" && text_SN.Text.Trim() != "")
            {
                if (text_SN.Text.Trim().Length == 6)
                {
                    Global.CompanyName = text_Company.Text.Trim();
                    Global.HardSN = text_SN.Text.Trim();
                    MessageBox.Show("注册信息添加完毕。", "产品注册", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("您输入的产品SN不正确！", "产品注册", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("请输入完整注册信息！", "产品注册", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}