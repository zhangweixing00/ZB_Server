using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

using PersonPositionServer.Common;


namespace PersonPositionServer.StaticService
{
    public static class CommonFun
    {
        /// <summary>
        /// 刷新硬件配置参数
        /// </summary>
        /// <returns></returns>
        public static bool RefreshHardConfig()
        {
            try
            {
                string temp;

                Global.CurrentlyMes = null;
                temp = Global.CurrentlyMes;

                Global.DownM0 = null;
                temp = Global.DownM0;
                Global.DownM1 = null;
                temp = Global.DownM1;
                Global.DownM2 = null;
                temp = Global.DownM2;
                Global.DownM3 = null;
                temp = Global.DownM3;
                Global.DownM4 = null;
                temp = Global.DownM4;
                Global.DownM5 = null;
                temp = Global.DownM5;
                Global.DownM6 = null;
                temp = Global.DownM6;

                Global.UpM0 = null;
                temp = Global.UpM0;
                Global.UpM1 = null;
                temp = Global.UpM1;
                Global.UpM2 = null;
                temp = Global.UpM2;
                Global.UpM3 = null;
                temp = Global.UpM3;
                Global.UpM4 = null;
                temp = Global.UpM4;
                Global.UpM5 = null;
                temp = Global.UpM5;
                Global.UpM6 = null;
                temp = Global.UpM6;
                Global.UpM7 = null;
                temp = Global.UpM7;
                Global.UpM8 = null;
                temp = Global.UpM8;
                Global.UpM9 = null;
                temp = Global.UpM9;
                Global.UpM10 = null;
                temp = Global.UpM10;
                Global.UpM11 = null;
                temp = Global.UpM11;
                Global.UpM12 = null;
                temp = Global.UpM12;
                Global.UpM13 = null;
                temp = Global.UpM13;
                Global.UpM14 = null;
                temp = Global.UpM14;
                Global.UpM15 = null;
                temp = Global.UpM15;
                Global.UpM16 = null;
                temp = Global.UpM16;
                Global.UpM17 = null;
                temp = Global.UpM17;
                Global.UpM18 = null;
                temp = Global.UpM18;
                Global.UpM19 = null;
                temp = Global.UpM19;
                Global.UpM20 = null;
                temp = Global.UpM20;
                Global.UpM21 = null;
                temp = Global.UpM21;
                Global.UpM22 = null;
                temp = Global.UpM22;
                Global.UpM23 = null;
                temp = Global.UpM23;
                Global.UpM24 = null;
                temp = Global.UpM24;
                Global.UpM25 = null;
                temp = Global.UpM25;
                Global.UpM26 = null;
                temp = Global.UpM26;
                Global.UpM27 = null;
                temp = Global.UpM27;
                Global.UpM28 = null;
                temp = Global.UpM28;
                Global.UpM29 = null;
                temp = Global.UpM29;

                return true;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "硬件配置参数初始化失败");
                return false;
            }
        }

        /// <summary>
        /// 设置开机自动启动
        /// </summary>
        /// <param name="started"></param>
        /// <param name="exeName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SetAutoRunWhenStart(bool started, string exeName, string path)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);//打开注册表子项
            if (key == null)//如果该项不存在的话，则创建该子项
            {
                key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }
            if (started == true)
            {
                try
                {
                    key.SetValue(exeName, path);//设置为开机启动
                    key.Close();
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    key.DeleteValue(exeName);//取消开机启动
                    key.Close();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断当前进程是否已经有一个运行实例
        /// </summary>
        /// <returns></returns>
        public static bool PrevInstance()
        {
            string procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if ((System.Diagnostics.Process.GetProcessesByName(procName)).GetUpperBound(0) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 从上次的临时数据文件重载Protocol_Service.Position的数据
        /// </summary>
        public static void ReloadPositionFromFile()
        {
            if (File.Exists(Global.ConfigTempData))
            {
                FileStream FS = new FileStream(Global.ConfigTempData, FileMode.Open, FileAccess.Read);
                StreamReader SR = new StreamReader(FS);
                try
                {
                    DateTime LastWriyteTime = Convert.ToDateTime(SR.ReadLine());
                    TimeSpan ts = DateTime.Now - LastWriyteTime;
                    if (ts.TotalMinutes < 5)
                    {
                        while (SR.Peek() >= 0)
                        {
                            string[] info_List = SR.ReadLine().Split('|');
                            switch (info_List.Length)
                            {
                                case 1:
                                    //考勤信息
                                    string[] DutyList = info_List[0].Split('!');
                                    for (int i = 0; i < DutyList.Length; i++)
                                    {
                                        if (DutyList[i] != "")
                                        {
                                            string[] DutyInfo = DutyList[i].Split('?');
                                            int cardID = Convert.ToInt32(DutyInfo[0]);
                                            if(IsRealCard(cardID))
                                                Protocol_Service.InMineList.Add(cardID, DutyInfo[1]);
                                        }
                                    }
                                        break;
                                case 7:
                                    //定位信息
                                    int CardID = Convert.ToInt32(info_List[0]);
                                    int StationID = Convert.ToInt32(info_List[1]);
                                    if (IsRealCard(CardID) && IsRealStation(StationID))
                                    {
                                        System.Data.DataRow row_Position = Protocol_Service.PositionTable.NewRow();
                                        row_Position["CardID"] = CardID;
                                        row_Position["StationID"] = StationID;
                                        row_Position["Geo_X"] = Convert.ToDouble(info_List[2]);
                                        row_Position["Geo_Y"] = Convert.ToDouble(info_List[3]);
                                        row_Position["NullSignalTimes"] = Convert.ToInt32(info_List[4]);
                                        if (info_List[5] != "")
                                            row_Position["InNullRSSITime"] = Convert.ToDateTime(info_List[5]);
                                        row_Position["IsOperated"] = Convert.ToBoolean(info_List[6]);
                                        Protocol_Service.PositionTable.Rows.Add(row_Position);
                                    }
                                    break;
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    if(Global.IsShowBug)
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "从上次的临时数据文件重载Protocol_Service.Position数据错误");
                }
                finally
                {
                    SR.Close();
                    FS.Close();
                }
            }
        }

        /// <summary>
        /// 判断指定的基站是否登记
        /// </summary>
        public static bool IsRealStation(int StationID)
        {
            if (BasicData.GetStationTableRows("ID = " + StationID,false).Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断指定的卡片是否登记并绑定
        /// </summary>
        public static bool IsRealCard(int CardID)
        {
            DataRow[] rows = BasicData.GetCardTableRows("CardID = " + CardID,false);
            if (rows.Length > 0)
            {
                //有卡
                if (rows[0]["PID"] != DBNull.Value)
                {
                    if (BasicData.GetPersonTableRows("PID = '" + rows[0]["PID"].ToString() + "'",false).Length > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定的采集器下是否使用了指定序号的通道并且这个通道有效
        /// 如果有效 则将Channel_ID更新
        /// </summary>
        public static bool IsRealChannel(int StationID, byte ChannelNum, ref int Channel_ID)
        {
            DataRow StationRow = BasicData.GetStationTableRows("ID = " + StationID,true)[0];
            string[] ChannelList = StationRow["CollectChannelIDStr"].ToString().Split('-');
            if (ChannelList.Length < 1)
            {
                return false;
            }
            else
            {
                for (int k = 0; k < ChannelList.Length; k++)
                {
                    if (ChannelList[k] != "")
                    {
                        string[] List_ChannelNum = ChannelList[k].Split(':');
                        if (List_ChannelNum.Length == 2)
                        {
                            //判断采集器通道字串里是否有这个通道
                            if (ChannelNum == Convert.ToInt32(List_ChannelNum[0]))
                            {
                                //有通道，则判断这个通道在通道表里是否存在
                                if (BasicData.GetCollectChannelTableRows("Channel_ID = " + List_ChannelNum[1]).Length > 0)
                                {
                                    Channel_ID = Convert.ToInt32(List_ChannelNum[1]);
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }
    }
}
