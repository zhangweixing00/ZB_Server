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
    public partial class FrmRegInfo : Form
    {
        public FrmRegInfo()
        {
            InitializeComponent();
        }

        private void FrmRegInfo_Load(object sender, EventArgs e)
        {
            label_ProductName.Text = Application.ProductName + (Global.IsTempVersion ? "(演示版)" : "");
            label_ProductCompany.Text = Application.CompanyName;
            label_ProductVerson.Text = "软件版本：" + Application.ProductVersion;
            label_RegCompanyName.Text = "授权给：" + Global.CompanyName;
            label_RegHardSN.Text = "产品SN：" + Global.HardSN;
        }
    }
}