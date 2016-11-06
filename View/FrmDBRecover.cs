using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PersonPositionServer.StaticService;

namespace PersonPositionServer.View
{
    public partial class FrmDBRecover : Form
    {
        public FrmDBRecover()
        {
            InitializeComponent();
        }

        private void btn_CleanHistory_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("注意！您正要清空历史定位信息表。您确定要这样做吗？", "清空指定表数据", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                List<string> strSQLs = new List<string>();
                strSQLs.Add("select * into HistoryPositionTable_Temp from HistoryPositionTable where 1=2");
                strSQLs.Add("Drop Table HistoryPositionTable");
                strSQLs.Add("EXEC sp_rename 'HistoryPositionTable_Temp', 'HistoryPositionTable'");
                strSQLs.Add("Alter Table HistoryPositionTable Add Constraint HistoryPositionTable_ID Primary Key(ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs);
                    MessageBox.Show(" 清空数据表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }
            }
        }

        private void btn_CleanAllAlarm_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("注意！您正要清空人员发送报警、故障基站报警、缺电报警、超员报警、超时报警、无卡人员进入报警、特殊区域报警共7张表。您确定要这样做吗？", "清空指定表数据", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                List<string> strSQLs = new List<string>();
                strSQLs.Add("select * into AlarmPersonSendTable_Temp from AlarmPersonSendTable where 1=2");
                strSQLs.Add("Drop Table AlarmPersonSendTable");
                strSQLs.Add("EXEC sp_rename 'AlarmPersonSendTable_Temp', 'AlarmPersonSendTable'");
                strSQLs.Add("Alter Table AlarmPersonSendTable Add Constraint AlarmPersonSendTable_Alarm_ID Primary Key(Alarm_ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs);
                    MessageBox.Show(" 清空人员发送报警信息表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }

                List<string> strSQLs1 = new List<string>();
                strSQLs1.Add("select * into AlarmMachineTable_Temp from AlarmMachineTable where 1=2");
                strSQLs1.Add("Drop Table AlarmMachineTable");
                strSQLs1.Add("EXEC sp_rename 'AlarmMachineTable_Temp', 'AlarmMachineTable'");
                strSQLs1.Add("Alter Table AlarmMachineTable Add Constraint AlarmMachineTable_Alarm_ID Primary Key(Alarm_ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs1);
                    MessageBox.Show(" 清空故障基站表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }

                List<string> strSQLs2 = new List<string>();
                strSQLs2.Add("select * into AlarmPowerTable_Temp from AlarmPowerTable where 1=2");
                strSQLs2.Add("Drop Table AlarmPowerTable");
                strSQLs2.Add("EXEC sp_rename 'AlarmPowerTable_Temp', 'AlarmPowerTable'");
                strSQLs2.Add("Alter Table AlarmPowerTable Add Constraint AlarmPowerTable_Alarm_ID Primary Key(Alarm_ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs2);
                    MessageBox.Show(" 清空缺电报警表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }

                List<string> strSQLs3 = new List<string>();
                strSQLs3.Add("select * into AlarmMaxPersonTable_Temp from AlarmMaxPersonTable where 1=2");
                strSQLs3.Add("Drop Table AlarmMaxPersonTable");
                strSQLs3.Add("EXEC sp_rename 'AlarmMaxPersonTable_Temp', 'AlarmMaxPersonTable'");
                strSQLs3.Add("Alter Table AlarmMaxPersonTable Add Constraint AlarmMaxPersonTable_Alarm_ID Primary Key(Alarm_ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs3);
                    MessageBox.Show(" 清空超员报警表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }

                List<string> strSQLs4 = new List<string>();
                strSQLs4.Add("select * into AlarmMaxHourTable_Temp from AlarmMaxHourTable where 1=2");
                strSQLs4.Add("Drop Table AlarmMaxHourTable");
                strSQLs4.Add("EXEC sp_rename 'AlarmMaxHourTable_Temp', 'AlarmMaxHourTable'");
                strSQLs4.Add("Alter Table AlarmMaxHourTable Add Constraint AlarmMaxHourTable_Alarm_ID Primary Key(Alarm_ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs4);
                    MessageBox.Show(" 清空超时报警表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }

                List<string> strSQLs5 = new List<string>();
                strSQLs5.Add("select * into AlarmNoCardTable_Temp from AlarmNoCardTable where 1=2");
                strSQLs5.Add("Drop Table AlarmNoCardTable");
                strSQLs5.Add("EXEC sp_rename 'AlarmNoCardTable_Temp', 'AlarmNoCardTable'");
                strSQLs5.Add("Alter Table AlarmNoCardTable Add Constraint AlarmNoCardTable_Alarm_ID Primary Key(Alarm_ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs5);
                    MessageBox.Show(" 清空无卡人员进入报警表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }

                List<string> strSQLs6 = new List<string>();
                strSQLs6.Add("select * into AlarmInAreaTable_Temp from AlarmInAreaTable where 1=2");
                strSQLs6.Add("Drop Table AlarmInAreaTable");
                strSQLs6.Add("EXEC sp_rename 'AlarmInAreaTable_Temp', 'AlarmInAreaTable'");
                strSQLs6.Add("Alter Table AlarmInAreaTable Add Constraint AlarmInAreaTable_Alarm_ID Primary Key(Alarm_ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs6);
                    MessageBox.Show(" 清空特殊区域报警表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }
            }
        }

        private void btn_CleanPerson_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("注意！您正要清空人员表。您确定要这样做吗？", "清空指定表数据", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                List<string> strSQLs = new List<string>();
                strSQLs.Add("select * into PersonTable_Temp from PersonTable where 1=2");
                strSQLs.Add("Drop Table PersonTable");
                strSQLs.Add("EXEC sp_rename 'PersonTable_Temp', 'PersonTable'");
                strSQLs.Add("Alter Table PersonTable Add Constraint PersonTable_PID Primary Key(PID)");
                strSQLs.Add("update CardTable set PID = null");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs);
                    MessageBox.Show(" 清空数据表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }
            }
        }

        private void btn_CleanDuty_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("注意！您正要清空考勤表。您确定要这样做吗？", "清空指定表数据", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                List<string> strSQLs = new List<string>();
                strSQLs.Add("select * into DutyTable_Temp from DutyTable where 1=2");
                strSQLs.Add("Drop Table DutyTable");
                strSQLs.Add("EXEC sp_rename 'DutyTable_Temp', 'DutyTable'");
                strSQLs.Add("Alter Table DutyTable Add Constraint DutyTable_ID Primary Key(ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs);
                    MessageBox.Show(" 清空数据表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }
            }
        }

        private void btn_CleanCard_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("注意！您正要清空卡片表。您确定要这样做吗？", "清空指定表数据", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                List<string> strSQLs = new List<string>();
                strSQLs.Add("select * into CardTable_Temp from CardTable where 1=2");
                strSQLs.Add("Drop Table CardTable");
                strSQLs.Add("EXEC sp_rename 'CardTable_Temp', 'CardTable'");
                strSQLs.Add("Alter Table CardTable Add Constraint CardTable_CardID Primary Key(CardID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs);
                    MessageBox.Show(" 清空数据表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }
            }
        }

        private void btn_CleanCollectChannel_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("注意！您正要清空采集器通道信息表。您确定要这样做吗？\n\n清空表同时将自动清空采集器基站里的通道信息。", "清空指定表数据", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                List<string> strSQLs = new List<string>();
                strSQLs.Add("select * into CollectChannelTable_Temp from CollectChannelTable where 1=2");
                strSQLs.Add("Drop Table CollectChannelTable");
                strSQLs.Add("EXEC sp_rename 'CollectChannelTable_Temp', 'CollectChannelTable'");
                strSQLs.Add("Alter Table CollectChannelTable Add Constraint CollectChannelTable_Channel_ID Primary Key(Channel_ID)");
                strSQLs.Add("update StationTable set CollectChannelIDStr = null");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs);
                    MessageBox.Show(" 清空数据表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }
            }
        }

        private void btn_CleanHistoryCollect_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("注意！您正要清空历史采集信息表。您确定要这样做吗？", "清空指定表数据", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                List<string> strSQLs = new List<string>();
                strSQLs.Add("select * into HistoryCollectTable_Temp from HistoryCollectTable where 1=2");
                strSQLs.Add("Drop Table HistoryCollectTable");
                strSQLs.Add("EXEC sp_rename 'HistoryCollectTable_Temp', 'HistoryCollectTable'");
                strSQLs.Add("Alter Table HistoryCollectTable Add Constraint HistoryCollectTable_ID Primary Key(ID)");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs);
                    MessageBox.Show(" 清空数据表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }
            }
        }

        private void btn_CleanMapArea_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("注意！您正要清空区域表，同时也会将基站中的区域信息清空。您确定要这样做吗？", "清空指定表数据", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                List<string> strSQLs = new List<string>();
                strSQLs.Add("select * into MapAreaTable_Temp from MapAreaTable where 1=2");
                strSQLs.Add("Drop Table MapAreaTable");
                strSQLs.Add("EXEC sp_rename 'MapAreaTable_Temp', 'MapAreaTable'");
                strSQLs.Add("Alter Table MapAreaTable Add Constraint MapAreaTable_MapAreaName Primary Key(MapAreaName)");
                strSQLs.Add("update StationTable set Area = null");
                try
                {
                    DB_Service.ExecuteSQLs(strSQLs);
                    MessageBox.Show(" 清空数据表成功！   ", "清空指定表数据", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "清空指定表数据");
                }
            }
        }
    }
}