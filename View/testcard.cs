using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PersonPositionServer.Common;
using PersonPositionServer.StaticService;
namespace PersonPositionServer.View
{
    
    
    
    public partial class testcard : Form
    {

        private MainForm mainform;
        public static Boolean testyn = false;
        public static int cardid = 0;         
        public testcard(MainForm _mainform)
        {
            InitializeComponent();
            this.mainform = _mainform;
        }

        private void testcard_Load(object sender, EventArgs e)
        {
            cardid = Convert.ToInt16(textBox1.Text);
            testyn = checkBox1.Checked;     
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cardid = Convert.ToInt32(textBox1.Text);  
            testyn= checkBox1.Checked;    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

      
       
    }
}