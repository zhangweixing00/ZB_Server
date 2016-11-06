using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PersonPositionServer.StaticService;
using PersonPositionServer.Common;

namespace PersonPositionServer.View
{
    public partial class FrmSetting : Form
    {
        public FrmSetting()
        {
            InitializeComponent();
        }

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            textServerPort.Text = Global.ServerPort.ToString();
            textArriveRSSI.Text = Global.ArriveRSSI.ToString();
            com_OtherLoopTime.SelectedIndex = com_OtherLoopTime.FindStringExact(Global.OtherLoopTime.ToString(), -1);
            com_PressMinute.SelectedIndex = com_PressMinute.FindStringExact(Global.HistoryPrecision.ToString(), -1);
            com_PressMinuteCollect.SelectedIndex = com_PressMinuteCollect.FindStringExact(Global.HistoryPrecision_Collect.ToString(), -1);
            com_NullRSSITimes.SelectedIndex = com_NullRSSITimes.FindStringExact(Global.NullSignalTimes.ToString(), -1);
            com_DistanceForDuty1Per.SelectedIndex = com_DistanceForDuty1Per.FindStringExact(Global.DistanceForDuty1Per.ToString(), -1);
            checkBox_IsLinePosition.Checked = Global.IsLinePosition;
            checkBox_ShowLowPower.Checked = Global.ShowLowPower;
            textDBConnStr.Text = Global.DBConnStr;
            checkBox_CheckOutRSSI.Checked = Global.IsCheckOutRSSI;
            check_OutWhen1GetRSSI.Checked = Global.OutWhen1GetRSSI;
            text_CheckOutRSSI.Text = Global.CheckOutRSSI.ToString();
            tex_AlarmMaxHour.Text = Global.AlarmMaxHour.ToString();
            tex_AlarmMaxPerson.Text = Global.AlarmMaxPerson.ToString();
            textSlick.Text = Global.MaxMoveLength.ToString();
            check_IsShowBug.Checked = Global.IsShowBug;
            check_SwitchHW.Checked = Global.IsOtherDirect;
            checkBox_IsUseHongWai.Checked = Global.IsUseHongWai;

            com_Port_port.SelectedIndex = com_Port_port.FindStringExact(Global.Port_port, -1);
            com_Port_Bote.SelectedIndex = com_Port_Bote.FindStringExact(Global.Port_bote, -1);
            com_Port_Check.SelectedIndex = com_Port_Check.FindStringExact(Global.Port_check, -1);
            com_Port_Data.SelectedIndex = com_Port_Data.FindStringExact(Global.Port_data, -1);
            com_Port_Stop.SelectedIndex = com_Port_Stop.FindStringExact(Global.Port_stop, -1);
            com_ErrorStationTimes.SelectedIndex = com_ErrorStationTimes.FindStringExact(Global.ErrorStationTimes.ToString(), -1);
            com_SlickTimes.SelectedIndex = com_SlickTimes.FindStringExact(Global.SlickTimes.ToString(), -1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ServerPort = Convert.ToInt32(textServerPort.Text);
                Global.ArriveRSSI = Convert.ToInt32(textArriveRSSI.Text);
                Global.OtherLoopTime = Convert.ToInt32(com_OtherLoopTime.Text);
                Global.NullSignalTimes = Convert.ToInt32(com_NullRSSITimes.Text);
                Global.DistanceForDuty1Per = Convert.ToDouble(com_DistanceForDuty1Per.Text);
                Global.HistoryPrecision = Convert.ToInt32(com_PressMinute.Text);
                Global.HistoryPrecision_Collect = Convert.ToInt32(com_PressMinuteCollect.Text);
                Global.IsCheckOutRSSI = checkBox_CheckOutRSSI.Checked;
                Global.ShowLowPower = checkBox_ShowLowPower.Checked;
                Global.CheckOutRSSI = Convert.ToByte(text_CheckOutRSSI.Text);
                Global.OutWhen1GetRSSI = check_OutWhen1GetRSSI.Checked;
                Global.IsLinePosition = checkBox_IsLinePosition.Checked;
                Global.DBConnStr = textDBConnStr.Text;
                Global.MaxMoveLength = Convert.ToDouble(textSlick.Text);
                Global.AlarmMaxPerson = Convert.ToInt32(tex_AlarmMaxPerson.Text);
                Global.AlarmMaxHour = Convert.ToInt32(tex_AlarmMaxHour.Text);
                Global.IsShowBug = check_IsShowBug.Checked;
                Global.IsOtherDirect = check_SwitchHW.Checked;
                Global.IsUseHongWai = checkBox_IsUseHongWai.Checked;

                Global.Port_port = com_Port_port.Text;
                Global.Port_bote = com_Port_Bote.Text;
                Global.Port_check = com_Port_Check.Text;
                Global.Port_data = com_Port_Data.Text;
                Global.Port_stop = com_Port_Stop.Text;
                Global.ErrorStationTimes = Convert.ToInt32(com_ErrorStationTimes.Text);
                Global.SlickTimes = Convert.ToInt32(com_SlickTimes.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace);
            }
            this.Close();
        }

        private void checkBox_CheckOutRSSI_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_CheckOutRSSI.Checked)
            {
                label34.Enabled = true;
                text_CheckOutRSSI.Enabled = true;
            }
            else
            {
                label34.Enabled = false;
                text_CheckOutRSSI.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_MapBackgroundPicGISZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字和句点
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void textBox_MapBackgroundPicGISCenterX_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字和句点
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void textBox_MapBackgroundPicGISCenterY_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字和句点
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void textServerPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void textArriveRSSI_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void textSlick_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字和句点
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void textBox_MapDistanceKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字和句点
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void text_CheckOutRSSI_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tex_AlarmMaxPerson_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void tex_AlarmMaxHour_KeyPress(object sender, KeyPressEventArgs e)
        {
            //控制只能输入数字
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void checkBox_IsUseHongWai_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_IsUseHongWai.Checked)
            {
                label1.Enabled = true;
                check_SwitchHW.Enabled = true;
            }
            else
            {
                label1.Enabled = false;
                check_SwitchHW.Enabled = false;
            }
        }
    }
}