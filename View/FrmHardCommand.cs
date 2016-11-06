using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PersonPositionServer.View
{
    public partial class FrmHardCommand : Form
    {
        private MainForm mainform;
        private bool isUerInput = true;

        public FrmHardCommand(MainForm father)
        {
            InitializeComponent();

            this.mainform = father;
            this.mainform.hardChannel.Event_GotMessage += new PersonPositionServer.Common.GotMessageEventHandler(hardChannel_Event_GotMessage);
            
        }

        void hardChannel_Event_GotMessage(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                text_Receive.AppendText(buffer[i].ToString());
            }
            text_Receive.AppendText("\r\n");
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            char[] tempSendList = text_Send.Text.Replace(" ", "").ToCharArray();
            if (tempSendList.Length % 2 == 0)
            {
                int BufferSize;
                if (check_IsXor.Checked)
                {
                    BufferSize = tempSendList.Length / 2 + 1;
                }
                else
                {
                    BufferSize = tempSendList.Length / 2;
                }
                byte[] sendBuffer = new byte[BufferSize];
                for (int i = 0; i < tempSendList.Length; i += 2)
                {
                    char char_High = tempSendList[i];
                    byte num_High = 0;
                    switch (char_High)
                    {
                        case '0':
                            num_High = 0;
                            break;
                        case '1':
                            num_High = 1;
                            break;
                        case '2':
                            num_High = 2;
                            break;
                        case '3':
                            num_High = 3;
                            break;
                        case '4':
                            num_High = 4;
                            break;
                        case '5':
                            num_High = 5;
                            break;
                        case '6':
                            num_High = 6;
                            break;
                        case '7':
                            num_High = 7;
                            break;
                        case '8':
                            num_High = 8;
                            break;
                        case '9':
                            num_High = 9;
                            break;
                        case 'A':
                            num_High = 10;
                            break;
                        case 'B':
                            num_High = 11;
                            break;
                        case 'C':
                            num_High = 12;
                            break;
                        case 'D':
                            num_High = 13;
                            break;
                        case 'E':
                            num_High = 14;
                            break;
                        case 'F':
                            num_High = 15;
                            break;
                    }
                    char char_Low = tempSendList[i+1];
                    byte num_Low = 0;
                    switch (char_Low)
                    {
                        case '0':
                            num_Low = 0;
                            break;
                        case '1':
                            num_Low = 1;
                            break;
                        case '2':
                            num_Low = 2;
                            break;
                        case '3':
                            num_Low = 3;
                            break;
                        case '4':
                            num_Low = 4;
                            break;
                        case '5':
                            num_Low = 5;
                            break;
                        case '6':
                            num_Low = 6;
                            break;
                        case '7':
                            num_Low = 7;
                            break;
                        case '8':
                            num_Low = 8;
                            break;
                        case '9':
                            num_Low = 9;
                            break;
                        case 'A':
                            num_Low = 10;
                            break;
                        case 'B':
                            num_Low = 11;
                            break;
                        case 'C':
                            num_Low = 12;
                            break;
                        case 'D':
                            num_Low = 13;
                            break;
                        case 'E':
                            num_Low = 14;
                            break;
                        case 'F':
                            num_Low = 15;
                            break;
                    }
                    sendBuffer[i / 2] = Convert.ToByte(num_High * 16 + num_Low);
                }
                string XorStr = "";
                if (check_IsXor.Checked)
                {
                    //校验结果临时变量
                    int temp = 0;
                    for (int j = 0; j < sendBuffer.Length - 1; j++)
                    {
                        temp = temp ^ sendBuffer[j];
                    }
                    sendBuffer[BufferSize - 1] = Convert.ToByte(temp);
                    
                    XorStr = temp.ToString();
                }
                try
                {
                    //发送命令
                    this.mainform.hardChannel.Send(sendBuffer);
                    if (text_Sended.Text == "")
                    {
                        text_Sended.AppendText(text_Send.Text + " " + XorStr);
                    }
                    else
                    {
                        text_Sended.AppendText("\r\n" + text_Send.Text + " " + XorStr);
                    }
                    text_Send.Text = "";
                    text_Sended.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("命令发送失败。请注意命令格式。基站号等均为16进制表示。\n\n" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("对不起，您发送的命令不正确。", "硬件命令调试工具", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void text_Send_TextChanged(object sender, EventArgs e)
        {
            if (isUerInput)
            {
                char[] aaa = text_Send.Text.Replace(" ","").ToCharArray();
                string finallyStr;
                if (aaa.Length < 3)
                {
                    finallyStr = text_Send.Text;
                }
                else
                {
                    finallyStr = aaa[0].ToString() + aaa[1].ToString();
                    for (int i = 2; i < aaa.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            finallyStr += " " + aaa[i].ToString();
                        }
                        else
                        {
                            finallyStr += aaa[i].ToString();
                        }
                    }
                }
                isUerInput = false;
                text_Send.Text = finallyStr.ToUpper();
                text_Send.Select(text_Send.Text.Length, 0);
                isUerInput = true;
            }
        }

        private void btn_ClearSend_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            text_Sended.Text = "";
        }

        private void btn_ClearReceive_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            text_Receive.Text = "";
        }

        private void text_Send_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btn_Send_Click(null, null);
            }
            else
            {
                //控制只能输入十六进制数字
                if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back && (e.KeyChar < 'A' || e.KeyChar > 'F') && (e.KeyChar < 'a' || e.KeyChar > 'f'))
                {
                    e.Handled = true;
                }
            }
        }

        private void text_Receive_TextChanged(object sender, EventArgs e)
        {

        }

        private void text_Sended_TextChanged(object sender, EventArgs e)
        {

        }
    }
}