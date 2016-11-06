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
    public partial class W_Set : Form
    {

        private MainForm mainform;

        public W_Set(MainForm _mainform)
        {
            InitializeComponent();
            this.mainform = _mainform;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            int dId=Convert.ToInt16(numericUpDown8.Value);  
            int sID=Convert.ToInt16(numericUpDown6.Value);  
            this.mainform.hardChannel.Send(CommandFactory.set_s_num(dId,sID));
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int dId = Convert.ToInt16(numericUpDown6.Value);
            int num1 = Convert.ToInt16(numericUpDown5.Value);  
            this.mainform.hardChannel.Send(CommandFactory.set_w_no(dId , num1 ));    
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int dId = Convert.ToInt16(numericUpDown6.Value);
            int num1 = Convert.ToInt16(numericUpDown7.Value);
            this.mainform.hardChannel.Send(CommandFactory.set_w_num(dId, num1));    
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int dId = Convert.ToInt16(numericUpDown6.Value);
            int num1 = Convert.ToInt16(numericUpDown1.Value);
            this.mainform.hardChannel.Send(CommandFactory.set_up_ch(dId, num1));          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int dId = Convert.ToInt16(numericUpDown6.Value);
            int num1 = Convert.ToInt16(numericUpDown2.Value);
            this.mainform.hardChannel.Send(CommandFactory.set_down_ch(dId, num1));      
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int dId = Convert.ToInt16(numericUpDown6.Value);
            int num1 = Convert.ToInt16(numericUpDown3.Value);
            int num2 = Convert.ToInt16(numericUpDown4.Value);
            this.mainform.hardChannel.Send(CommandFactory.set_space(dId, num1,num2));  
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int dId = Convert.ToInt16(numericUpDown6.Value);
            this.mainform.hardChannel.Send(CommandFactory.set_reset(dId ));   
        }

       

        
    }
}