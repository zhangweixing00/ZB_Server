using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using PersonPositionServer.View  ;
using PersonPositionServer.Common;
using PersonPositionServer.Model;




namespace PersonPositionServer.StaticService
{
    public static class Protocol_Service
    {



       
        
        public static event StationNormalEventHandler StationNormal;
        //判断硬件合法性的标志量 -1为未判断 0为不合法 1为合法
        public static int HardLawful = -1;
        //控制一个人员在两秒钟之内只筛选一条短信的变量
        private static int LastSendMessage_CardID = -1;//上一次发送消息的卡片ID
        private static DateTime LastSendMessage_Time;//上一次发送消息的时间
        //包序号
        private static byte BagNum = 0;
        //连续包
        public static bool IsContinue = false;
        //是否需要应答
        private static bool IsNeedResponse = false;
        //保存一次巡检解析出的信息
        public static DataTable BasicPositionTable = DataTableFactory_Service.MakeBasicPositionTable("BasicPositionTable");
        //保存定位信息
        public static DataTable PositionTable = DataTableFactory_Service.MakePositionTable("PositionTable");
        public static FileStream fslog;
        public static StreamWriter swlog;
        //private static Int64  tm= DateTime.now  ;   

        public static FileStream fs = new FileStream("wuabcd", FileMode.Create);
        public static StreamWriter sw = new StreamWriter(fs);
        public static int bno = 0;
        public static int[] dbmrssi ={
                    
                                    -89, 0x01,
                                    -88, 0x02,
                                    -87, 0x05,
                                    -86, 0x09,
                                    -85, 0x0D,
                                    -84, 0x12,
                                    -83, 0x17, 
                                    -82, 0x1B, 
                                    -81, 0x20, 
                                    -80, 0x25, 
                                    -79, 0x2B, 
                                    -78, 0x30, 
                                    -77, 0x35, 
                                    -76, 0x3A, 
                                    -75, 0x3F, 
                                    -74, 0x44, 
                                    -73, 0x49, 
                                    -72, 0x4E,
                                    -71, 0x53, 
                                    -70, 0x59,
                                    -69, 0x5F,
                                    -68, 0x64,
                                    -67, 0x6B,
                                    -66, 0x6F,
                                    -65, 0x75,
                                    -64, 0x79,
                                    -63, 0x7D,
                                    -62, 0x81,
                                    -61, 0x85,
                                    -60, 0x8A,
                                    -59, 0x8F,
                                    -58, 0x94,
                                    -57, 0x99,
                                    -56 ,0x9F,
                                    -55, 0xA5,
                                    -54, 0xAA,
                                    -53, 0xB0,
                                    -52, 0xB7,
                                    -51, 0xBC,
                                    -50, 0xC1,
                                    -49, 0xC6, 
                                    -48, 0xCB, 
                                    -47, 0xCF, 
                                    -46, 0xD4,
                                    -45, 0xD8,
                                    -44, 0xDD,
                                    -43, 0xE1,
                                    -42, 0xE4,
                                    -41, 0xE9,
                                    -40, 0xEF,
                                    -39, 0xF5,
                                    -38, 0xFA,
                                    -37, 0xFD,
                                    -36, 0xFE,
                                    -35, 0xFF
                                };




        //保存洞内人数    值：基站ID-日-时-分
        public static Dictionary<int, string> InMineList = new Dictionary<int, string>();
        //显示本次巡检进洞人员字符串
        private static string JustNowInStr = "";
        //显示本次巡检出洞人员字符串
        private static string JustNowOutStr = "";
        //保存一个历史数据精度时间范围内的定位信息
        private static Dictionary<string, string> TempHistoryPositionTable = new Dictionary<string, string>();
        //当前故障基站列表，内容是故障次数
        public static Dictionary<int, int> ErrorStationList = new Dictionary<int, int>();
        //解析出的底层信息数量
        public static long BasicNum_Position = 0;//底层的卡片定位信息数量
        public static long BasicNum_Collect = 0;//底层的信息采集数量
        public static long AnalysicsNum = 0;//分析定位信息数量
        public static long DutyNum = 0;//考勤信息数量
        public static long InsertDBNum = 0;//插入定位信息数量
        public static long InsertDBNum_Collect = 0;//插入采集信息数量
        //平滑处理函数用到的表
        private static Dictionary<int, double[]> OldPoints = new Dictionary<int, double[]>();//保存每个卡片前一时刻坐标的字典
        private static Dictionary<int, int[]> OldPointsAims = new Dictionary<int, int[]>();//保存每个卡片前一时刻方向的字典
        ////////////////////////////////
        ////////特殊区域////////////////
        public static string AlarmAreaName = "未启动";//当前特殊区域方案名称（类似主键）,如果为"未启动"，则不启动特殊区域判断
        public static Dictionary<int, int> InArea = new Dictionary<int, int>();//当前特殊区域内人员，Key为CardID，内容为StationID
        public static bool IsExceedInArea = false;//当前特殊区域内的人数是否超限
        ////////////////////////////////
        ////////红外计数////////////////
        public static int HW_IncreaseNum = 0;
        public static DateTime HW_LastInTime = new DateTime(2000, 1, 1, 1, 1, 1);
        public static Mutex mutex =  new Mutex();
        //基站相关性数组
        private static string[] StationRelation;
        public static bool logexist = false;
       // private static  MainForm mainform;

       /* public P_s(MainForm _mainform)
        {
            InitializeComponent();
            this.mainform = _mainform;
        }
        */
        /// <summary>
        /// 帧类型枚举
        /// </summary>
        public enum BagType
        {
            Data,//数据帧
            Command,//命令帧
            Response,//应答帧
            Unable//无效帧
        }

        /// <summary>
        /// 发出命令枚举
        /// </summary>
        public enum CommandType
        {
            CheckHard,//校验硬件命令
            Loop,//巡检命令
            AddSon,//添加子节点命令
            DelSon,//删除子节点命令
            SetTime,//设置基站时间
            DownMessage,//发送下行短信
            LightUp,//点亮灯命令
            GetHistoryData,//得到历史数据命令
            DelHistoryData,//清空历史数据命令
            SetInMineNum,//设置考勤基站洞内人数命令
            SetInfo,//设置或取消考勤基站通知命令

            cset_s_num,
            cset_w_no,
            cset_w_num,
            cset_up_ch,
            cset_down_ch,
            cset_space,
            cset_reset

        }


        public static int Todbm(int  rssi)
        {
           //byte rssiv[] =new byte[]={ 1,2,5,9,0xd,0x12,0x17,0x1b,0x20,0x25,0x2b,0x30,0x35,0x3a,0x3f,0x44,0x49,0x4e,0x53,0x59,0x5f,0x64,0x6b,0x6f,0x75,
             //                  0x79,0x7d,0x81,0x85,0x8a,0x8f,0x94,0x99,     



            for (int i = 0; i < 55; i++)
            {
                if (rssi <= dbmrssi[i * 2 + 1])
                {

                   
                    return dbmrssi[i * 2];
                
                }
            
            
            
            }



            return -35;

        }





        /// <summary>
        /// 按协议分析一个完整帧的数据，并作出相应操作
        /// 注：这里传入的帧必须是一个已经经过检测的完好的帧，除了尚未校验
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bytes"></param>
        public static void AnalysisAndOperate(byte[] buffer)
        {
            //取出第4个字节代表的有效数据长度
            byte lengh = buffer[3];
            //校验结果临时变量
            int temp = 0;
            for (int i = 0; i < lengh + 6; i++)
            {
                temp = temp ^ buffer[i + 2];
            }
            //判断校验结果与一帧中最后一字节的校验码是否相等
            if (temp.Equals(buffer[lengh + 8]))
            {
                //////////////////
                //相等
                //////////////////

                //取出第3个字节代表的帧控制字
                byte control = buffer[2];
                //取出第7、8个字节代表的源基站ID
                int sourceStationID = (buffer[6] << 8) + buffer[7];
                //如果基站出错，则还原出正确的ID后判断基站的真实存在性
                if (sourceStationID >= 32768)
                {
                    //不正常基站,还原出基站ID 判断恢复正常基站在处理巡检返回数据里进行！
                    sourceStationID = sourceStationID - 32768;
                }
                if (!CommonFun.IsRealStation(sourceStationID))
                {
                    return;
                }
                //取出第9个字节代表的命令字
                byte command = buffer[8];

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //根据AnalysisControlByte判断分析包类型，并初始化IsNeedResponse、IsContinue、BagNum三个全局控制变量，从而做出不同操作
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (AnalysisControlByte(control, ref BagNum, ref IsNeedResponse, ref IsContinue))
                {
                    //数据帧
                    case Protocol_Service.BagType.Data:
                        //根据命令字判断是什么命令
                        switch (command)
                        {
                            //上行短消息命令字
                            case 0x55:
                                #region 上行短消息命令字
                                //取出第14字节代表的短信编号
                                byte messageSN = buffer[13];
                                string messageType = "";
                                switch (messageSN)
                                {
                                    //主洞小里程塌方
                                    case 0x00:
                                        messageType = Global.UpM0;
                                        break;
                                    //主洞大里程塌方
                                    case 0x01:
                                        messageType = Global.UpM1;
                                        break;
                                    //斜井塌方
                                    case 0x02:
                                        messageType = Global.UpM2;
                                        break;
                                    //大里程工伤
                                    case 0x03:
                                        messageType = Global.UpM3;
                                        break;
                                    //小里程工伤
                                    case 0x04:
                                        messageType = Global.UpM4;
                                        break;
                                    //大里程突水
                                    case 0x05:
                                        messageType = Global.UpM5;
                                        break;
                                    //小里程突水
                                    case 0x06:
                                        messageType = Global.UpM6;
                                        break;
                                    //大里程停水
                                    case 0x07:
                                        messageType = Global.UpM7;
                                        break;
                                    //小里程停水
                                    case 0x08:
                                        messageType = Global.UpM8;
                                        break;
                                    //请速到大里程
                                    case 0x09:
                                        messageType = Global.UpM9;
                                        break;
                                    //报警=请速到小里程
                                    case 0x0A:
                                        messageType = Global.UpM10;
                                        break;
                                    //车辆故障通道阻塞
                                    case 0x0B:
                                        messageType = Global.UpM11;
                                        break;
                                    //大里程机械故障
                                    case 0x0C:
                                        messageType = Global.UpM12;
                                        break;
                                    //小里程机械故障
                                    case 0x0D:
                                        messageType = Global.UpM13;
                                        break;
                                    //大里程水管坏抢修
                                    case 0x0E:
                                        messageType = Global.UpM14;
                                        break;
                                    //小里程水管坏抢修
                                    case 0x0F:
                                        messageType = Global.UpM15;
                                        break;
                                    //大里程风管坏抢修
                                    case 0x10:
                                        messageType = Global.UpM16;
                                        break;
                                    //小里程风管坏抢修
                                    case 0x11:
                                        messageType = Global.UpM17; 
                                        break;
                                    //大里程要求送风
                                    case 0x12:
                                        messageType = Global.UpM18;
                                        break;
                                    //小里程要求送风
                                    case 0x13:
                                        messageType = Global.UpM19;
                                        break;
                                    //大里程打眼放炮
                                    case 0x14:
                                        messageType = Global.UpM20;
                                        break;
                                    //小里程打眼放炮
                                    case 0x15:
                                        messageType = Global.UpM21;
                                        break;
                                    //大里程准备出渣
                                    case 0x16:
                                        messageType = Global.UpM22;
                                        break;
                                    //小里程准备出渣
                                    case 0x17:
                                        messageType = Global.UpM23;
                                        break;
                                    //大里程准备立架
                                    case 0x18:
                                        messageType = Global.UpM24;
                                        break;
                                    //小里程准备立架
                                    case 0x19:
                                        messageType = Global.UpM25;
                                        break;
                                    //大里程准备喷浆
                                    case 0x1A:
                                        messageType = Global.UpM26;
                                        break;
                                    //小里程准备喷浆
                                    case 0x1B:
                                        messageType = Global.UpM27;
                                        break;
                                    //缺电报警
                                    case 0xFD:
                                        messageType = "缺电报警";
                                        break;
                                    //下行短信的自动回复，表示下行短信发送成功
                                    case 0xFE:
                                        //直接跳出此包的解析方法
                                        return;
                                    default:
                                        return;
                                }
                                //取出第12、13字节代表的卡片ID（由于谢欣没有改位置，这个原本应是StationID）
                                int cardID = (buffer[11] << 8) + buffer[12];
                                //至此，这里都是正常的上行短信。则判断卡片的真实性
                                if (CommonFun.IsRealCard(cardID))
                                {
                                    //当前时间
                                    DateTime nowDateTime = DateTime.Now;
                                    //发送新短信前，先筛选去除冗余的短信。
                                    //第一次LastSendMessage_CardID=-1，所以不会执行下面的块
                                    if (cardID == LastSendMessage_CardID)
                                    {
                                        TimeSpan span = nowDateTime - LastSendMessage_Time;
                                        //如果时间间隔小于2秒则跳出
                                        if (span.Hours == 0 && span.Minutes == 0 && span.Seconds <= 2)
                                        {
                                            break;
                                        }
                                    }
                                    //根据内容区分缺电和人员发送短信
                                    if (messageType == "缺电报警")
                                    {
                                        if (Global.ShowLowPower)
                                        {
                                            //将报警写入数据库
                                            DB_Service.ExecuteSQL(DB_Service.MakeAlarmPowerSQL(cardID, nowDateTime));
                                            LastSendMessage_CardID = cardID;
                                            LastSendMessage_Time = nowDateTime;
                                            Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_LowPower, cardID.ToString(), nowDateTime.ToString(), "", "", "", "", "", "","");
                                        }
                                    }
                                    else
                                    {
                                        //判断是否是“报警”信息,如果是，则筛选出有效信息
                                        string[] tempStr= messageType.Split('=');
                                        string FinallyStr;
                                        if (tempStr.Length == 2)
                                        {
                                            if (BasicData.GetCardTableRows("CardID = " + cardID,true)[0]["CardType"].ToString() == "一般人员")
                                            {
                                                FinallyStr = tempStr[0];
                                            }
                                            else
                                            {
                                                FinallyStr = tempStr[1];
                                            }
                                        }
                                        else
                                        {
                                            FinallyStr = messageType;
                                        }
                                        //将短信写入数据库
                                        DB_Service.ExecuteSQL(DB_Service.MakeAlarmPersonSendSQL(cardID, FinallyStr, nowDateTime));
                                        LastSendMessage_CardID = cardID;
                                        LastSendMessage_Time = nowDateTime;
                                        Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpMessage, cardID.ToString(), FinallyStr, nowDateTime.ToString(), "", "", "", "", "","");
                                    }
                                }
                                #endregion
                                break;
                            //上行采集器命令字
                            case 0x75:
                                #region 上行采集器命令字
                                //更新错误次数,并将错误基站恢复为正常
                                ChangeErrorStationNormal(sourceStationID);
                                //取出第10字节代表的通道号
                                byte ChannelNum = buffer[9];
                                int Channel_ID = -1;
                                //判断指定的采集器下是否使用了指定序号的通道并且这个通道有效

                                try
                                {

                                    if (CommonFun.IsRealChannel(sourceStationID, ChannelNum, ref Channel_ID))
                                    {
                                        //有效 则取出数据
                                        double ChannelValue = (buffer[10] << 8) + buffer[11];
                                        if (ChannelValue > 32767.0)
                                        {
                                            //超出硬件最大检测值，则为0
                                            ChannelValue = 0;
                                        }
                                        //给所有客户端发送服务器采集器通道信息更新消息
                                        Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpdateCollectChannel, sourceStationID.ToString(), ChannelNum.ToString(), ChannelValue.ToString(), Channel_ID.ToString(), "", "", "", "", "");
                                        //底层的采集器通道信息数量自增
                                        Protocol_Service.BasicNum_Collect++;

                                        string KeyCollect = sourceStationID.ToString() + "-" + ChannelNum.ToString();
                                        if (DB_Service.LastInsertHistory_Collect.ContainsKey(KeyCollect))
                                        {
                                            if (((TimeSpan)DateTime.Now.Subtract(DB_Service.LastInsertHistory_Collect[KeyCollect])).TotalMinutes >= Global.HistoryPrecision_Collect)
                                            {
                                                DB_Service.LastInsertHistory_Collect[KeyCollect] = DateTime.Now;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            DB_Service.LastInsertHistory_Collect.Add(KeyCollect, DateTime.Now);
                                        }
                                        //到达历史存储时间间隔或者第一次运行，则存入数据库
                                        DateTime InsertTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                                        //到达历史存储时间间隔或者第一次运行，则存入数据库
                                        Protocol_Service.InsertDBNum_Collect += DB_Service.ExecuteSQL("insert into HistoryCollectTable (Station_ID,ChannelNum,ChannelValue,Channel_ID,Time) values (" + sourceStationID + "," + ChannelNum + "," + ChannelValue + "," + Channel_ID + ",'" + InsertTime.ToString() + "')");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (Global.IsShowBug)
                                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "采集数据数据库错误");

                                }
                                #endregion
                                break;
                            //红外信号数据
                            case 0x8C:
                                #region 红外信号数据
                                byte InNum_HongWai = 0;
                                if (Global.IsOtherDirect)
                                {
                                    InNum_HongWai = buffer[17];
                                }
                                else
                                {
                                    InNum_HongWai = buffer[20];
                                }
                                if (InNum_HongWai > 0)
                                {
                                    HW_LastInTime = DateTime.Now;
                                    HW_IncreaseNum += InNum_HongWai;
                                }
                                #endregion
                                break;
                            //考勤基站上报的考勤信息
                            case 0x25:
                                #region 考勤基站上报的考勤信息
                                int InOutCardID = (buffer[9] << 8) + buffer[10];
                                if (CommonFun.IsRealCard(InOutCardID))
                                {
                                    if (Convert.ToChar(buffer[11]) == 'I')
                                    {
                                        if (!InMineList.ContainsKey(InOutCardID))
                                        {
                                            InMineList.Add(InOutCardID, sourceStationID.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString());
                                            JustNowInStr += InOutCardID.ToString() + "?";
                                            //写数据库
                                            CheckDutyInDB(1, InOutCardID, DateTime.Now);
                                            //修改红外标志
                                            if (HW_LastInTime != new DateTime(2000, 1, 1, 1, 1, 1))
                                                HW_IncreaseNum -= 1;
                                        }
                                    }
                                    else
                                    {
                                        if (InMineList.ContainsKey(InOutCardID))
                                        {
                                            InMineList.Remove(InOutCardID);
                                            JustNowOutStr += InOutCardID.ToString() + "?";
                                            //写数据库
                                            CheckDutyInDB(2, InOutCardID, DateTime.Now);
                                            //出洞则将PositionTable中的定位信息删除
                                            DataRow[] rows = PositionTable.Select("CardID = " + InOutCardID);
                                            if (rows.Length > 0)
                                                rows[0].Delete();
                                        }
                                    }
                                    DutyNum++;
                                }
                                #endregion
                                break;
                            case 0x12:

                                #region 语音数据
                                //                                break;
                                int InOutCardID1 = (buffer[9] << 8) + buffer[10];
                                DataRow rows_Card = PositionTable.Select("CardID = " + InOutCardID1)[0];
                                int st = Convert.ToInt16(rows_Card["StationID"]), father = -1;
                                DataRow rows_st = BasicData.MainStationTable.Select("ID = " + st)[0];

                                // DataRow rows_father = BasicData.MainStationTable.Select("ID = " + rows_st["FatherStationID"])[0];
                                byte[] tempbuf = new byte[lengh - 1];
                                if (sourceStationID != st)
                                {
                                    if (Convert.ToString(rows_st["StationType"]) == "网关基站")
                                    {

                                        for (int m = 0; m < lengh - 1; m++)
                                        {
                                            tempbuf[m] = buffer[9 + m];

                                        }

                                        Socket_Service.SendData(CommandFactory.SendSoundData(st, sourceStationID, tempbuf));
                                        //(CommandFactory.SendSoundData(st, sourceStationID, tempbuf));


                                    }

                                    else if (Convert.ToString(rows_st["StationType"]) == "无线基站")
                                    {

                                        DataRow rows_father = BasicData.MainStationTable.Select("ID = " + rows_st["FatherStationID"])[0];
                                        if (Convert.ToString(rows_father["StationType"]) == "网关基站")
                                        {
                                            for (int m = 0; m < lengh; m++)
                                            {
                                                tempbuf[m] = buffer[11 + m];

                                            }
                                            father = Convert.ToInt16(rows_father["ID"]);
                                            Socket_Service.SendData(CommandFactory.SendSoundData(father, sourceStationID, tempbuf));

                                        }


                                    }
                                }

                                #endregion
                                break;

                        }
                        break;
                    //命令帧
                    case Protocol_Service.BagType.Command:
                        break;
                    //应答帧
                    case Protocol_Service.BagType.Response:
                        //根据命令字判断是什么命令
                        switch (command)
                        {

                            case 0x12:

                                #region 语音数据
//                                break;
                                int  InOutCardID = (buffer[9] << 8) + buffer[10];
                                DataRow rows_Card = PositionTable.Select("CardID = " + InOutCardID)[0];
                                int st = Convert.ToInt16(rows_Card["StationID"]), father = -1;
                                DataRow rows_st = BasicData.MainStationTable.Select("ID = " + st)[0];
                                
                               // DataRow rows_father = BasicData.MainStationTable.Select("ID = " + rows_st["FatherStationID"])[0];
                                byte[] tempbuf = new byte[lengh - 1];
                                if (sourceStationID != st)
                                {
                                    if (Convert.ToString(rows_st["StationType"]) == "网关基站")
                                    {

                                        for (int m = 0; m < lengh-1; m++)
                                        {
                                            tempbuf[m] = buffer[9 + m];

                                        }

                                        Socket_Service.SendData(CommandFactory.SendSoundData(st, sourceStationID, tempbuf));
                                        //(CommandFactory.SendSoundData(st, sourceStationID, tempbuf));


                                    }

                                   else  if (Convert.ToString(rows_st["StationType"]) == "无线基站")
                                    {

                                        DataRow rows_father = BasicData.MainStationTable.Select("ID = " + rows_st["FatherStationID"])[0];
                                        if (Convert.ToString(rows_father["StationType"]) == "网关基站")
                                        {
                                            for (int m = 0; m < lengh; m++)
                                            {
                                                tempbuf[m] = buffer[11 + m];

                                            }
                                            father = Convert.ToInt16(rows_father["ID"]);
                                            Socket_Service.SendData(CommandFactory.SendSoundData(father, sourceStationID, tempbuf));

                                        }


                                    }
                             }

                                #endregion
                                break;
                            
                            
                            
                            //巡检数据命令字
                            case 0x50:
                                #region 巡检数据命令字
                                //判断源基站ID的最高位是否为0或1
                                int IsWireError = (buffer[6] & 0x80) >> 7;
                                //如果为0，则说明与此无线基站通信正常
                                if (IsWireError == 0)
                                {
                                    //更新错误次数,并将错误基站恢复为正常
                                    ChangeErrorStationNormal(sourceStationID);
                                    //当有效数据长度为1则说明没有卡片信息，则直接跳过。
                                    if (lengh > 1)
                                    {
                                        //有巡检的正常卡片信息，则先取第10位的密码判断结果
                                        if (buffer[9] == 1 || (0x55 ^ (int)Global.HardSN.ToCharArray()[0]) == buffer[9])
                                        {
                                            if (Protocol_Service.HardLawful != 0)
                                                Protocol_Service.HardLawful = 1;//只在未判断或者之前都是正确的前提下才继续正确
                                        }
                                        else
                                        {
                                            Protocol_Service.HardLawful = 0;
                                        }
                                        //至此，已经判断完毕硬件。HardLawful已经改变过。则做完本次解析
                                        int RepairRSSI = 0;
                                        DataRow[] rows = BasicData.GetStationTableRows("ID = " + sourceStationID,true);
                                        
                                        
                                        if (rows.Length > 0)
                                        {
                                            //初始化此基站的RepairRSSI
                                            try
                                            {
                                                RepairRSSI = Convert.ToInt32(rows[0]["RepairRSSI"]);
                                            }
                                            catch
                                            {
                                                RepairRSSI = 0;
                                            }
                                        }

                                        

                                            
                                        //循环 (lengh-7)/3 次，实现将有效数据中从第16个字节开始的所有卡片信息（ID,RSSI）取出来
                                        Int16 nn=7;
                                        if(Global .Isnew)
                                            nn=9;
                                        
                                        if (RepairRSSI !=255)
                                        for (int j = 0; j < lengh - nn; j = j + 3)
                                        {

                                            //Object thisLock = new Object();



                                           // lock (thisLock)
                                            
                                                

                                                    int CardID = (buffer[j + 15] << 8) + buffer[j + 16];
                                                    //判断卡片的真实性


                                                    if (CommonFun.IsRealCard(CardID))
                                                    {
                                                        int RSSI = buffer[j + 17];
                                                        //RSSI = 0xff;
                                                        if (Global.Isnew)
                                                        {

                                                            RSSI = Todbm(RSSI);
                                                            RSSI = 255 - (~RSSI);

                                                        }

                                                        RSSI += RepairRSSI;
                                                        if (RSSI >= 255)
                                                            RSSI = 255;
                                                        
                                                        
                                                        
                                                       
                                                        if (Global.IsCheckOutRSSI && RSSI <= Global.CheckOutRSSI)
                                                        {
                                                            //如果筛选弱的RSSI并且小于标值，则直接跳过这张卡
                                                            continue;
                                                        }



                                                        try
                                                        {
                                                            mutex.WaitOne();


                                                            if (testcard.testyn)
                                                                if (CardID == testcard.cardid)
                                                                {
                                                                    string ssss = "";


                                                                    ssss = Convert.ToString(CardID) + "#####" + Convert.ToString(RSSI) + ":::::" + Convert.ToString(Todbm(RSSI)) + Environment.NewLine;
                                                                    //开始写入
                                                                    sw.Write(ssss);
                                                                    //清空缓冲区
                                                                    sw.Flush();

                                                                }
                                                           
                                                            DataRow tempRow = BasicPositionTable.NewRow();
                                                            tempRow["StationID"] = sourceStationID;
                                                            tempRow["CardID"] = CardID;
                                                            tempRow["RSSI"] = RSSI ;
                                                            tempRow["DT"] =DateTime.Now.Ticks  ; 
                                                            //添加到定位临时运算表
                                                            BasicPositionTable.Rows.Add(tempRow);
                                                            //底层的卡片定位信息数量自增
                                                            Protocol_Service.BasicNum_Position++;
                                                            bno++;
                                                           

                                                        }
                                                        catch (Exception ex)
                                                        {

                                                            if (Global.IsShowBug)
                                                                System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器解析底层数据错误");


                                                        }
                                                        finally
                                                        {
                                                            mutex.ReleaseMutex();
                                                        }

                                                    }
                                        }
                                    }
                                }
                                else
                                {
                                    //最高位为1，则说明与此无线基站通信错误
                                }
                                ///////////////////////////////////////////////////////////////////////////////
                                //巡检的返回处理完毕后，将“巡检返回处理完毕标志”设置为真
                                ///////////////////////////////////////////////////////////////////////////////
                                Global.IsOperateComplete = true;
                                #endregion
                                break;
                            //添加、删除子基站成功
                            case 0x54:
                                Global.Result_AddSon = true;
                                Global.Result_DelSon = true;
                                break;
                            //设置时间成功
                            case 0x91:
                                Global.Result_SetTime = true;
                                break;
                            //点亮灯泡成功
                            case 0x74:
                                Global.Result_LightUp = true;
                                break;
                            //返回的历史数据
                            case 0x60:
                                #region 历史数据
                                try
                                {
                                    int RepairRSSI = 0;
                                    DataRow[] rows = BasicData.GetStationTableRows("ID = " + sourceStationID, true);
                                    if (rows.Length > 0)
                                    {
                                        //初始化此基站的RepairRSSI
                                        try
                                        {
                                            RepairRSSI = Convert.ToInt32(rows[0]["RepairRSSI"]);
                                        }
                                        catch
                                        {
                                            RepairRSSI = 0;
                                        }
                                    }
                                    List<string> strSQLs = new List<string>();
                                    //循环 (lengh-7)/3 次，实现将有效数据中从第16个字节开始的所有卡片信息（ID,RSSI）取出来
                                    for (int j = 0; j < lengh - 7; j = j + 3)
                                    {
                                        int CardID = (buffer[j + 15] << 8) + buffer[j + 16];
                                        //判断卡片的真实性
                                        if (CommonFun.IsRealCard(CardID))
                                        {
                                            int RSSI = buffer[j + 17];
                                            RSSI += RepairRSSI;
                                            if (Global.IsCheckOutRSSI && RSSI <= Global.CheckOutRSSI)
                                            {
                                                //如果筛选弱的RSSI并且小于标值，则直接跳过这张卡
                                                continue;
                                            }
                                            DateTime ttt = new DateTime(2000 + buffer[9], buffer[10], buffer[11], buffer[12], buffer[13], buffer[14]);
                                            strSQLs.Add("if not exists (select * from HistoryPositionTable where cardID = " + CardID + " and MaxStationID = " + sourceStationID + " and Time = '" + ttt + "') insert into HistoryPositionTable (CardID,Geo_X,Geo_Y,MaxStationID,Time) values (" + CardID + "," + rows[0]["Geo_X"] + "," + rows[0]["Geo_Y"] + "," + sourceStationID + ",'" + ttt.ToString() + "')");
                                            //底层的卡片定位信息数量自增
                                            Protocol_Service.BasicNum_Position++;
                                        }
                                    }
                                    if (strSQLs.Count > 0)
                                    {
                                        //按一个事物全部插入数据库
                                        Protocol_Service.InsertDBNum += DB_Service.ExecuteSQLs(strSQLs);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (Global.IsShowBug)
                                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器解析底层基站历史数据错误");
                                }
                                #endregion
                                break;
                            //考勤基站上报的人数
                            case 0x24:
                                #region 考勤基站上报的人数
                                //更新错误次数,并将错误基站恢复为正常
                                ChangeErrorStationNormal(sourceStationID);
                                #endregion
                                break;


                            default:
                                return;
                        }
                        break;
                    //无效帧
                    case Protocol_Service.BagType.Unable:
                        break;
                }
            }
        }

        /// <summary>
        /// 更新错误次数,并将错误基站恢复为正常
        /// </summary>
        /// <param name="StationID"></param>
        private static void ChangeErrorStationNormal(int StationID)
        {
            if (ErrorStationList[StationID] == Global.ErrorStationTimes + 1)
            {
                DataTable tempTable = DB_Service.GetTable("tempTable", "select * from AlarmMachineTable where StationID = " + StationID + " and ResumeTime is null");
                if (tempTable.Rows.Count > 0)
                {
                    for (int sk = 0; sk < tempTable.Rows.Count; sk++)
                    {
                        DataRow temp_row = tempTable.Rows[sk];
                        int AlarmID = Convert.ToInt32(temp_row["Alarm_ID"]);
                        DateTime errorStartTime = Convert.ToDateTime(temp_row["ErrorStartTime"]);
                        TimeSpan span = DateTime.Now.Subtract(errorStartTime);
                        if (span.TotalMinutes > 1.0)
                        {
                            //数据库中错误的基站恢复
                            DB_Service.ExecuteSQL("update AlarmMachineTable set ResumeTime = '" + DateTime.Now + "' where Alarm_ID = " + AlarmID);
                        }
                        else
                        {
                            //删除这个短期的错误
                            DB_Service.ExecuteSQL("delete from AlarmMachineTable where Alarm_ID = " + AlarmID);
                        }
                    }
                }
                //抛出错误基站恢复正常事件
                StationNormal(StationID);
            }
            ErrorStationList[StationID] = 1;
        }

        /// <summary>
        /// 分析控制字
        /// </summary>
        /// <param name="controlByte"></param>
        /// <param name="BagNum"></param>
        /// <param name="IsNeedResponse"></param>
        /// <param name="IsContinue"></param>
        /// <returns></returns>
        private static Protocol_Service.BagType AnalysisControlByte(byte controlByte, ref byte BagNum, ref bool isNeedResponse, ref bool isContinue)
        {
            //取出第1～第4位代表的包序号
            BagNum = Convert.ToByte(controlByte & 0x0F);
            //取出第5位代表的是否需要应答
            isNeedResponse = Convert.ToBoolean((controlByte & 0x10) >> 4);
            //取出第6位代表的是否是连续包
            isContinue = Convert.ToBoolean((controlByte & 0x20) >> 5);
            //取出第7、8位代表的帧类型    
            switch (controlByte & 0xC0)
            {
                //数据帧
                case 0x00:
                    return Protocol_Service.BagType.Data;
                //命令帧
                case 0x40:
                    return Protocol_Service.BagType.Command;
                //应答帧
                case 0x80:
                    return Protocol_Service.BagType.Response;
                //无效帧
                case 0xC0:
                    return Protocol_Service.BagType.Unable;
                //默认无效帧
                default:
                    return Protocol_Service.BagType.Unable;
            }
        }

        /// <summary>
        /// 计算出故障基站字串
        /// 格式：StationID!StationID!StationID!
        /// </summary>
        /// <returns></returns>
        public static string GetErrorStationStr()
        {
            string temp_ErrorStationStr = "";
            try
            {
                foreach (int StationID in ErrorStationList.Keys)
                {
                    if (ErrorStationList[StationID] >= Global.ErrorStationTimes)
                    {
                        temp_ErrorStationStr += StationID.ToString() + "!";
                    }
                }
            }
            catch(Exception ex)
            {
                if (Global.IsShowBug)
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器计算故障基站错误");
            }
            //如果有内容，则去掉最后一个“!”
            if (temp_ErrorStationStr != "")
                temp_ErrorStationStr = temp_ErrorStationStr.Substring(0, temp_ErrorStationStr.Length - 1);
            return temp_ErrorStationStr;
        }

        /// <summary>
        /// 从本次巡检信息表（BasicPositionTable）、洞内人员表（InMineList）中算出综合信息并发出更新消息
        /// 如果到达历史存储时间间隔，则存入数据库
        /// </summary>
        public static void Do_DutyAndPosition(DateTime OperationTime)
        {
            
            
            
            //如果本次的定位信息的分钟数与LastInsertHistory的分钟之差数大于等于历史数据精度数。说明到达插入时间，则插入历史数据
            if (((TimeSpan)OperationTime.Subtract(DB_Service.LastInsertHistory_Position)).TotalMinutes >= Global.HistoryPrecision)
            {
                //将TempHistoryPositionTable中的一个历史筛选时间精度的数据存入数据库
                List<string> strSQLs = new List<string>();
                DateTime InsertTime = new DateTime(DB_Service.LastInsertHistory_Position.Year, DB_Service.LastInsertHistory_Position.Month, DB_Service.LastInsertHistory_Position.Day, DB_Service.LastInsertHistory_Position.Hour, DB_Service.LastInsertHistory_Position.Minute, 0);
                try
                {
                    foreach (string str in TempHistoryPositionTable.Values)
                    {
                        string[] info = str.Split('@');
                        if (info.Length == 4)
                        {
                            strSQLs.Add("insert into HistoryPositionTable (CardID,Geo_X,Geo_Y,MaxStationID,Time) values (" + info[0] + "," + info[1] + "," + info[2] + "," + info[3] + ",'" + InsertTime.ToString() + "')");
                        }
                    }
                    if (strSQLs.Count > 0)
                    {
                        //按一个事物全部插入数据库
                        Protocol_Service.InsertDBNum += DB_Service.ExecuteSQLs(strSQLs);
                    }
                }
                catch(Exception ex)
                {
                    if (Global.IsShowBug)
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器插入历史定位数据错误");
                }
                TempHistoryPositionTable.Clear();//清空TempHistoryPositionTable
                DB_Service.LastInsertHistory_Position = OperationTime;//更新历史表中最新的一批记录的时间
            }
            //如果本次巡检有结果，则更新PositionTable
            if (BasicPositionTable.Rows.Count > 0)
            {
                Object  thisLock = new Object();
                lock (thisLock)
                {
                    try
                    { 
                         mutex.WaitOne();
                        //根据本次巡检信息表更新PositionTable，并更改分析定位信息数量
                        Protocol_Service.AnalysicsNum += UpdatePositionTable();
                        //清空BasicPositionTable
                       
                         //BasicPositionTable.Rows.Clear();
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
           
            //PositionTable处理完毕，开始根据PositionTable判断特殊区域
            if (AlarmAreaName != "未启动")
            {
                //刷新InArea表
                RefreshInAreaTable();
            }
            //存放最终定位字符串 格式：...!CardID?StationID?Geo_X?Geo_Y?InNullRSSITime!...(注意：其中InNullRSSITime的格式为：日:时:分)
            StringBuilder SB_ResultPositionStr = new StringBuilder();
            //保存本次定位数据、洞内人员数据到临时数据文件
            FileStream FS = new FileStream(Global.ConfigTempData, FileMode.Create, FileAccess.Write);
            StreamWriter SW = new StreamWriter(FS);
            SW.WriteLine(OperationTime.ToString());
            //将PositionTable转化为字符串并发出进出洞消息
            for (int i = 0; i < PositionTable.Rows.Count; i++)
            {
                int _cardID = (int)PositionTable.Rows[i]["CardID"];
                int _stationID = (int)PositionTable.Rows[i]["StationID"];
                DataRow tempStationRow = BasicData.GetStationTableRows("ID = " + _stationID, true)[0];




                if (tempStationRow["DutyOrder"] != DBNull.Value)
                {
                    if (tempStationRow["DutyOrder"].ToString() == "1")
                    {
                        


                        if (InMineList.ContainsKey(_cardID))
                        {
                            InMineList.Remove(_cardID);
                            JustNowOutStr += _cardID.ToString() + "?";
                            //写数据库
                            CheckDutyInDB(2, _cardID, DateTime.Now);
                            //出洞则将PositionTable中的定位信息删除
                           
                           // goto l1;
                        }
                       
                    
                    
                    
                    }
                    else
                    {
                        //最终确保所有2,3号基站下的人员都进洞
                        if (!InMineList.ContainsKey(_cardID))
                        {
                            InMineList.Add(_cardID, _stationID.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString());
                            JustNowInStr += _cardID.ToString() + "?";
                            //写数据库
                            CheckDutyInDB(1, _cardID, OperationTime);
                            //修改红外标志
                            if (HW_LastInTime != new DateTime(2000, 1, 1, 1, 1, 1))
                                HW_IncreaseNum -= 1;
                        }
                    }
                }


                // string StrInMineTime = "";
                //if (PositionTable.Rows[i]["InMineTime"] != DBNull.Value)
                              
                



                string StrInNullRSSITime = "";
                if (PositionTable.Rows[i]["InNullRSSITime"] != DBNull.Value)
                {
                    DateTime _inNullRSSITime = Convert.ToDateTime(PositionTable.Rows[i]["InNullRSSITime"]);
                    StrInNullRSSITime = _inNullRSSITime.Day.ToString() + ":" + _inNullRSSITime.Hour.ToString() + ":" + _inNullRSSITime.Minute.ToString();
                }
                SB_ResultPositionStr.Append(_cardID.ToString() + "?" + _stationID.ToString() + "?" + PositionTable.Rows[i]["Geo_X"].ToString() + "?" + PositionTable.Rows[i]["Geo_Y"].ToString() + "?"  + StrInNullRSSITime + "!");
               
                
                SW.WriteLine(PositionTable.Rows[i]["CardID"].ToString() + "|" + PositionTable.Rows[i]["StationID"].ToString() + "|" + PositionTable.Rows[i]["Geo_X"].ToString() + "|" + PositionTable.Rows[i]["Geo_Y"].ToString() + "|" + PositionTable.Rows[i]["NullSignalTimes"].ToString() + "|" + PositionTable.Rows[i]["InMineTime"].ToString() + "|" + PositionTable.Rows[i]["InNullRSSITime"].ToString() + "|" + PositionTable.Rows[i]["IsOperated"].ToString());

                //int _cardID = Convert.ToInt32(PositionTable.Rows[i]["CardID"]);
                if (!Convert.ToBoolean(PositionTable.Rows[i]["IsOperated"]))
                {
                    PositionTable.Rows[i]["IsOperated"] = false;
                    
                    if (Convert.ToInt32(PositionTable.Rows[i]["NullSignalTimes"]) < Global.NullSignalTimes)
                    {
                        PositionTable.Rows[i]["NullSignalTimes"] = Convert.ToInt32(PositionTable.Rows[i]["NullSignalTimes"]) + 1;

                        
                    }
                    else
                    {

                        //将平滑处理用到的“旧”点删除
                        if (OldPoints.ContainsKey(_cardID))
                            OldPoints.Remove(_cardID);
                        if (OldPointsAims.ContainsKey(_cardID))
                            OldPointsAims.Remove(_cardID);
                        //判断没有信号的卡是否在洞内



                        if (InMineList.ContainsKey(_cardID))
                        {
                            //在洞内，则进入盲区
                            if (PositionTable.Rows[i]["InNullRSSITime"] == DBNull.Value)
                                PositionTable.Rows[i]["InNullRSSITime"] = OperationTime;
                        }
                        else
                        {
                            //不在洞内，则直接删除
                            PositionTable.Rows[i].Delete();
                            //break;

                        }
                    }

                }
                else 
                    PositionTable.Rows[i]["IsOperated"] = false;

                


            }
                  
            PositionTable.AcceptChanges();



            //得到当前洞内人员的字符串描述保存进入数据文件
l1:         SW.WriteLine(GetInMinePersonStr());
            //储存
            SW.Close();
            FS.Close();
            //判断洞内人员是否超时
            bool IsOverAlarmMaxHour = false;
            
            //Dictionary<string, int> copy = new Dictionary<string, int>(dictionary);



            Dictionary<int, string> InMineList_copy = new Dictionary<int, string>(InMineList);
          
            DateTime tempdate;
            DateTime _inMineTime;
            foreach (int temp_cardID in InMineList_copy.Keys)
            {
                string[] tempArray = InMineList_copy[temp_cardID].Split('-');

                
                
                    if(DateTime.Now.Day<Convert.ToInt32(tempArray[1]))
                    {
                    
                        tempdate=DateTime.Now  ;
                        tempdate =tempdate.AddDays(-1);
                                               
                        _inMineTime = new DateTime(tempdate .Year, tempdate.Month, Convert.ToInt32(tempArray[1]), Convert.ToInt32(tempArray[2]), Convert.ToInt32(tempArray[3]), 0);
                    
                    }
                    else 
                    
                   _inMineTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(tempArray[1]), Convert.ToInt32(tempArray[2]), Convert.ToInt32(tempArray[3]), 0);
                    //判断这个进入时间是否超时
                

                
                TimeSpan ts = OperationTime - _inMineTime;
                if (ts.TotalHours >= Global.AlarmMaxHour)
                {

                    IsOverAlarmMaxHour = true;
                    DB_Service.CheckAlarmMaxHourInDB(temp_cardID, (int)ts.TotalHours, Global.AlarmMaxHour, OperationTime);
                    InMineList.Remove(temp_cardID);
                    JustNowOutStr += temp_cardID.ToString() + "?";
                    //写数据库
                    CheckDutyInDB(2, temp_cardID, DateTime.Now);
                    //出洞则将PositionTable中的定位信息删除

                }
            }
            //InMineList_copy.Clear ;
            InMineList_copy = null;
            ; // <- makes all the difference Txns = null; GC.Collect();


            //当前洞内人数 判断是否洞内总人数超限
            bool IsOverAlarmMaxPerson;
            if (InMineList.Count >= Global.AlarmMaxPerson)
            {
                IsOverAlarmMaxPerson = true;
                DB_Service.CheckAlarmMaxPersonInDB(InMineList.Count, Global.AlarmMaxPerson, OperationTime);
            }
            else
            {
                IsOverAlarmMaxPerson = false;
            }
            //判断红外的进洞人数
            bool IsHW_OverNum = false;
            if (HW_LastInTime != new DateTime(2000, 1, 1, 1, 1, 1) && HW_IncreaseNum > 0)
            {
                TimeSpan ts = OperationTime - HW_LastInTime;
                if (ts.TotalSeconds > 60)
                {
                    //将报警写入数据库
                    DB_Service.ExecuteSQL(DB_Service.MakeAlarmNoCardSQL(HW_IncreaseNum, OperationTime));
                    IsHW_OverNum = true;
                    HW_IncreaseNum = 0;
                    HW_LastInTime = new DateTime(2000, 1, 1, 1, 1, 1);
                }
            }
            //给所有客户端发送服务器定位信息更新消息。如果ResultPositionStr=""，说明没有任何数据。
            string AreaStr = Protocol_Service.AlarmAreaName + "!" + InArea.Count.ToString() + "!" + Protocol_Service.IsExceedInArea.ToString();
            Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpdatePosition, "True", SB_ResultPositionStr.ToString(), GetInMinePersonStr(), GetErrorStationStr(), AreaStr, IsOverAlarmMaxPerson.ToString(), IsOverAlarmMaxHour.ToString(), IsHW_OverNum.ToString(), JustNowInStr + "!" + JustNowOutStr);
            JustNowInStr = "";
            JustNowOutStr = "";
        }

        /// <summary>
        /// 判断两个基站是否具有相关性
        /// 注：只适用于线性定位，若非线性定位或者不需要判断时，请清空基站关系文件内容。
        /// </summary>
        /// <param name="StationA"></param>
        /// <param name="StationB"></param>
        /// <returns></returns>
        private static bool IsStationRelation(string StationA, string StationB)
        {
            //若之前没有初始化过 NearRelation 数组，首先初始化之
            if (StationRelation == null)
            {
                //注：如果文件不存在或者文件内容为空，则构造"用户未设置基站关系"的字符数组
                FileStream FS = new FileStream(Global.StationRelationFile, FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader SR = new StreamReader(FS);
                string temp = SR.ReadLine();
                SR.Close();
                FS.Close();
                if (temp == null)
                {
                    StationRelation = new string[1] { "用户未设置基站关系" };
                }
                else
                {
                    StationRelation = temp.Trim().Split('-');
                }
            }

            //为空则直接返回True
            if (StationRelation[0] == "用户未设置基站关系" || StationRelation[0] == "")
            {
                return true;
            }

            //具体判断
            for (int i = 0; i < StationRelation.Length; i++)
            {
                if (StationRelation[i] == StationA)
                {
                    if ((i > 0 && StationRelation[i - 1] == StationB) || (i < StationRelation.Length - 1 && StationRelation[i + 1] == StationB))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (StationRelation[i] == StationB)
                {
                    if ((i > 0 && StationRelation[i - 1] == StationA) || (i < StationRelation.Length - 1 && StationRelation[i + 1] == StationA))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //至此，说明在文件：NearRelationFile中没有与这两个基站中任意一个有关的相邻记录，则认为这两个基站不需要做相关性判断，则返回True
            return true;
        }

        /// <summary>
        /// 根据本次巡检信息表更新PositionTable
        /// </summary>
        /// <returns>最终算出的定位记录数</returns>
        private static int UpdatePositionTable()
        {
            //最终算出的定位记录数
            int AnalysicsNum = 0;
            //先将表按CardID排序
            //Object thisLock = new Object();
            if (File.Exists(@"d:\serverlog.txt"))
            {
                if (fslog != null)
                {
                    logexist = true;
                    //swlog.WriteLine();
                    //swlog.Flush();
                }
            }
            DataTable bcopy;// = new DataTable()
            DataTable  acopy = new DataTable();
            //bcopy =BasicPositionTable ;

            DataView tempView = new DataView(BasicPositionTable);
            tempView.Sort = "CardID,StationID,RSSI ASC";

            
            bcopy  = tempView.ToTable();
            bcopy.TableName = "bcop";
            acopy=BasicPositionTable.Clone();
           // bcopy.AcceptChanges();
           // BasicPositionTable.Rows.Clear(); 
            
            /* DataRow[] rows_Card;
            while (seek < bcopy.Rows.Count)
            {
                int sid = Convert.ToInt32(bcopy.Rows[seek]["StationID"]);

                DataRow[] rows_Card = bcopy.Select("CardID = " + cardid, "RSSI DESC");
            
            }
            */

               // DataView tempView = BasicPositionTable.DefaultView;
                //tempView.Sort = "CardID ASC";
                //BasicPositionTable = tempView.ToTable();
            int sid = 0,cardid=-1 ;
            int seek = 0;
           while (seek < bcopy.Rows.Count)
            {
                try
                {
                    //指针位置的卡片ID
                    cardid = Convert.ToInt32(bcopy.Rows[seek]["CardID"]);
                    sid = Convert.ToInt32(bcopy.Rows[seek]["StationID"]); 
                    int ct=0;
                    while(cardid == Convert.ToInt32(bcopy.Rows[seek]["CardID"])&& sid == Convert.ToInt32(bcopy.Rows[seek]["StationID"]))
                    {

                        //long dt = (DateTime.Now.Ticks - Convert.ToInt64(bcopy.Rows[seek]["DT"])) / 1000000;
                        
                        //if ((dt-Global.SlickTimes*1)>=0 )
                        { //bcopy .Rows[seek].SetModified() ;
                        //  bcopy.Rows[seek].Delete();  
                        }
                      ct++;
                      seek++;

                      if (seek >= bcopy.Rows.Count)
                          break;  
                    }

                    
                    DataRow tempRow = acopy.NewRow();
                    tempRow["StationID"] = sid;
                    tempRow["CardID"] = cardid ;
                    tempRow["RSSI"] = Convert.ToInt32(bcopy.Rows[seek-ct/2-1 ]["RSSI"]);
                    tempRow["DT"] =0   ; 
                    //添加到定位临时运算表
                    
                    acopy.Rows.Add(tempRow );

                    
                }


                catch (Exception ex)
                {
                    if (Global.IsShowBug)
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器生成PositionTable错误");
                    /*如果出错。继续解析下一张卡片*/
                }
            }
             
            
           
               // DataView tempView = BasicPositionTable.DefaultView;
                //tempView.Sort = "CardID ASC";
                //BasicPositionTable = tempView.ToTable();
           
           // bcopy.AcceptChanges ();
           
           
            
            bcopy =acopy .Copy();
            seek = 0;
            while (seek < bcopy.Rows.Count)
            {
                try
                {
                    //指针位置的卡片ID
                     cardid = Convert.ToInt32(bcopy.Rows[seek]["CardID"]);

                    //bcopy表里这张卡片的记录集合，按RSSI从大到小排序
                    DataRow[] rows_Card = bcopy.Select("CardID = " + cardid, "RSSI DESC");
                    //DataRow[] rows_Card = bcopy.Select("CardID = " + cardid);
                                       
                    //将指针移动rows_CardID.Length个单位
                    seek += rows_Card.Length;
                    //最大信号强度的基站ID
                    int MaxRSSIStationID = Convert.ToInt32(rows_Card[0]["StationID"]);
                    //最大的信号强度
                    int MaxRSSI = Convert.ToInt32(rows_Card[0]["RSSI"]);
                    //最大信号的基站信息
                    DataRow FirstMaxStationRow = BasicData.GetStationTableRows("ID = " + MaxRSSIStationID, true)[0];
                    //次大信号的基站信息
                    DataRow SecondMaxStationRow = null;
                    //最终返回的坐标
                    double[] FinallyPoint = new double[2];
                    //因为只有有信号才有记录，所以，在这里不可能出现没有信号的卡片
                    //if (MaxRSSI  >= Global.ArriveRSSI)
                    if (logexist)
                    {
                        swlog.WriteLine("-------------------卡片ID" + cardid.ToString() + "定位数据如下:--------------at " + DateTime.Now.ToString());
                        for (int i = 0; i < rows_Card.Length; i++)
                        {
                            string str = "收到卡片" + cardid.ToString() + "信号的第" + (i + 1).ToString() + "大强度基站ID是 " + rows_Card[i]["StationID"].ToString() + " 信号强度为" + rows_Card[i]["RSSI"].ToString() + " at " + DateTime.Now.ToString();
                            swlog.WriteLine(str);
                        }

                    }
                    if(false )
                    {
                        //直接返回最大信号基站坐标
                        FinallyPoint = new double[2] { Convert.ToDouble(FirstMaxStationRow["Geo_X"]), Convert.ToDouble(FirstMaxStationRow["Geo_Y"]) };
                    }
                    else
                    {
                        string log = string.Empty;
                        switch (rows_Card.Length)
                        {
                            case 0:
                                //不可能没有一条记录，故这里永远不会执行
                                break;
                            case 1:
                                //只有一个信号，直接返回最大信号基站坐标
                                log = "只有一个信号最大信号基站ID" + MaxRSSIStationID.ToString() + " at " + DateTime.Now.ToString();
                                FinallyPoint = new double[2] { Convert.ToDouble(FirstMaxStationRow["Geo_X"]), Convert.ToDouble(FirstMaxStationRow["Geo_Y"]) };
                                break;
                            case 2:
                                //只有两个信号，线性算法，先判断基站相关性
                               if (IsStationRelation(MaxRSSIStationID.ToString(), rows_Card[1]["StationID"].ToString()))
                                
                               // if(true )

                                {
                                    //相关，则计算坐标
                                    SecondMaxStationRow = BasicData.GetStationTableRows("ID = " + Convert.ToInt32(rows_Card[1]["StationID"]), true)[0];
                                    FinallyPoint = ConvertRSSItoPoint(Convert.ToDouble(FirstMaxStationRow["Geo_X"]), Convert.ToDouble(FirstMaxStationRow["Geo_Y"]), Convert.ToDouble(SecondMaxStationRow["Geo_X"]), Convert.ToDouble(SecondMaxStationRow["Geo_Y"]), MaxRSSI, Convert.ToInt32(rows_Card[1]["RSSI"]));
                                    log = "只有两个信号且相关,最大信号基站是" + MaxRSSIStationID.ToString() + " 次大信号基站是" + rows_Card[1]["StationID"].ToString();
                                   break;
                                }
                                else
                                {
                                    FinallyPoint = new double[2] { Convert.ToDouble(FirstMaxStationRow["Geo_X"]), Convert.ToDouble(FirstMaxStationRow["Geo_Y"]) };
                                    log = "只有两个信号且不相关,最大信号基站是" + MaxRSSIStationID.ToString() + "次大信号基站是" + rows_Card[1]["StationID"].ToString();
                                   break;
                                    //不相关，则跳过这次计算
                                    //continue;
                                }
                            default:
                                //有两个以上的信号
                                SecondMaxStationRow = BasicData.GetStationTableRows("ID = " + Convert.ToInt32(rows_Card[1]["StationID"]), true)[0];
                                //如果采用线性定位算法，则取最大的两个做一次线性定位，
                                if (Global.IsLinePosition)
                                {
                                    //先判断基站相关性
                                    if (IsStationRelation(MaxRSSIStationID.ToString(), rows_Card[1]["StationID"].ToString()))
                                   // if(true )
                                    {
                                        //相关，则计算坐标
                                        log = "使用线性定位，头两个信号相关;最大基站ID" + MaxRSSIStationID.ToString() + "和次大信号" + rows_Card[1]["StationID"].ToString();
                                        FinallyPoint = ConvertRSSItoPoint(Convert.ToDouble(FirstMaxStationRow["Geo_X"]), Convert.ToDouble(FirstMaxStationRow["Geo_Y"]), Convert.ToDouble(SecondMaxStationRow["Geo_X"]), Convert.ToDouble(SecondMaxStationRow["Geo_Y"]), MaxRSSI, Convert.ToInt32(rows_Card[1]["RSSI"]));
                                    }
                                    else
                                    {
                                        log = "使用线性定位，头两个信号不相关;最大基站ID" + MaxRSSIStationID.ToString() + "和次大信号" + rows_Card[1]["StationID"].ToString();
                                        FinallyPoint = new double[2] { Convert.ToDouble(FirstMaxStationRow["Geo_X"]), Convert.ToDouble(FirstMaxStationRow["Geo_Y"]) };
                                        break;
                                        //不相关，则跳过这次计算
                                        //continue;
                                    }
                                }
                                else
                                {
                                    log = "有三个以上信号，不使用线性定位，最大基站ID" + MaxRSSIStationID.ToString() + "和次大信号" + rows_Card[1]["StationID"].ToString()+"第三大信号" + rows_Card[2]["StationID"].ToString();
                                    DataRow ThirdMaxStationRow = BasicData.GetStationTableRows("ID = " + Convert.ToInt32(rows_Card[2]["StationID"]), true)[0];
                                    double[] StationA = new double[2] { Convert.ToDouble(FirstMaxStationRow["Geo_X"]), Convert.ToDouble(FirstMaxStationRow["Geo_Y"]) };
                                    double[] StationB = new double[2] { Convert.ToDouble(SecondMaxStationRow["Geo_X"]), Convert.ToDouble(SecondMaxStationRow["Geo_Y"]) };
                                    double[] StationC = new double[2] { Convert.ToDouble(ThirdMaxStationRow["Geo_X"]), Convert.ToDouble(ThirdMaxStationRow["Geo_Y"]) };
                                    //构造的三角形的三个顶点
                                    double[] pointA = new double[2];
                                    double[] pointB = new double[2];
                                    double[] pointC = new double[2];
                                    //三次线性算法算出这三个顶点
                                    pointA = ConvertRSSItoPoint(StationA[0], StationA[1], StationB[0], StationB[1], MaxRSSI, Convert.ToInt32(rows_Card[1]["RSSI"]));
                                    pointB = ConvertRSSItoPoint(StationB[0], StationB[1], StationC[0], StationC[1], Convert.ToInt32(rows_Card[1]["RSSI"]), Convert.ToInt32(rows_Card[2]["RSSI"]));
                                    pointC = ConvertRSSItoPoint(StationA[0], StationA[1], StationC[0], StationC[1], MaxRSSI, Convert.ToInt32(rows_Card[2]["RSSI"]));
                                    //计算三角形的内心
                                    FinallyPoint = InCenterPoint(pointA[0], pointA[1], pointB[0], pointB[1], pointC[0], pointC[1]);
                                }
                                break;
                        }
                        if (logexist)
                        {
                            swlog.WriteLine(log);
                            swlog.Flush();
                        }
                    }
                    if (logexist)
                    {
                        swlog.WriteLine("-------------------卡片ID" + cardid.ToString() + "定位完毕--------------at " + DateTime.Now.ToString());
                        swlog.Flush();
                    }
                    //平滑处理

                    if(testcard.testyn) 
                    if (cardid == testcard .cardid )
                    {
                        string ssss = "";
                        string [] teststr1={"0 ","0 ","0"};
                        for (int ii = 0; ii < rows_Card.Length; ii++)
                        {
                            teststr1[ii] = Convert.ToString(rows_Card[ii]["RSSI"]);  
                        
                        }
                        ssss = Convert.ToString(cardid) + "---" + Convert.ToString(FinallyPoint[0]) + "(" + teststr1[0] + "==" +
                        teststr1[01] + "==" + teststr1[02] + ")" + Environment.NewLine;
                                                //开始写入
                        sw.Write(ssss);
                        //清空缓冲区
                        sw.Flush();
                    }
                    
                    
                    FinallyPoint = Slick(cardid, FinallyPoint);

                    int NearStationID;
                    
                    NearStationID = GetNearStationID_id(FinallyPoint, FirstMaxStationRow,cardid,MaxRSSI );

                    /*
                   
                    //以平滑后的坐标点判断离哪个基站近
                    int NearStationID;
                    if (SecondMaxStationRow == null)
                    {
                        //只有1个基站参与了坐标计算 则判断是否是之前的基站，如果不是，则把之前基站也参与运算最近基站
                        DataRow[] temprow = PositionTable.Select("CardID = " + cardid);
                        if (temprow.Length > 0)
                        {
                            int StationID = Convert.ToInt32(temprow[0]["StationID"]);
                            if (MaxRSSIStationID == StationID)
                            {
                                //是以前的基站 直接赋值
                                NearStationID = MaxRSSIStationID;
                            }
                            else
                            {
                                //不是以前的基站，则把之前的也加入判断最近基站
                                NearStationID = GetNearStationID(FinallyPoint, FirstMaxStationRow, BasicData.GetStationTableRows("ID = " + StationID, true)[0]);
                            }
                        }
                        else
                        {
                            //之前没有记录 说明是新进入 则直接赋值
                            NearStationID = MaxRSSIStationID;
                        }
                    }
                    else
                    {
                        //有两个或两个以上的基站参与了坐标计算，则调用计算方法
                        NearStationID = GetNearStationID(FinallyPoint, FirstMaxStationRow, SecondMaxStationRow);
                    }

                    */



                    

                    //如果《1号基站收到信号就算出井》打开 则判断
                    if (Global.OutWhen1GetRSSI)
                    {
                        if (FirstMaxStationRow["DutyOrder"] != DBNull.Value && FirstMaxStationRow["DutyOrder"].ToString() == "1")
                        {
                            NearStationID = MaxRSSIStationID;
                            FinallyPoint[0] = Convert.ToDouble(FirstMaxStationRow["Geo_X"]);
                            FinallyPoint[1] = Convert.ToDouble(FirstMaxStationRow["Geo_Y"]);
                        }
                        else if (SecondMaxStationRow != null && SecondMaxStationRow["DutyOrder"] != DBNull.Value && SecondMaxStationRow["DutyOrder"].ToString() == "1")
                        {
                            NearStationID = Convert.ToInt32(SecondMaxStationRow["ID"]);
                            FinallyPoint[0] = Convert.ToDouble(SecondMaxStationRow["Geo_X"]);
                            FinallyPoint[1] = Convert.ToDouble(SecondMaxStationRow["Geo_Y"]);
                        }
                    }
                    
                    
                    //判断此卡与基站的组合在临时的历史筛选表中是否存在，不存在则添加定位信息
                    if (!TempHistoryPositionTable.ContainsKey(cardid.ToString() + "#" + NearStationID.ToString()))
                    {
                        TempHistoryPositionTable.Add(cardid.ToString() + "#" + NearStationID.ToString(), cardid + "@" + FinallyPoint[0].ToString() + "@" + FinallyPoint[1].ToString() + "@" + NearStationID.ToString());
                    }
                    //更新PositionTable
                    DataRow[] row_PositionTable_Card = PositionTable.Select("CardID = " + cardid);
                    DataRow[] row_area = BasicData.GetStationTableRows("ID = " + NearStationID, true);
                    
                    if (row_PositionTable_Card.Length > 0)
                    {
                        //如果这张卡之前就有，首先更新StationID、Geo_X、Geo_Y，NullSignalTimes = 0、InNullRSSITime = Null、IsOperated =true
                        row_PositionTable_Card[0]["StationID"] = NearStationID;
                        row_PositionTable_Card[0]["Geo_X"] = FinallyPoint[0];
                        row_PositionTable_Card[0]["Geo_Y"] = FinallyPoint[1];
                        row_PositionTable_Card[0]["NullSignalTimes"] = 0;
                        row_PositionTable_Card[0]["InNullRSSITime"] = DBNull.Value;
                        row_PositionTable_Card[0]["IsOperated"] = true;
                        row_PositionTable_Card[0]["Area"] = row_area[0]["Area"]; 

                    }
                    else
                    {
                        //如果这张卡之前没有，则新建一条记录
                        DataRow NewPositionTableRow = PositionTable.NewRow();
                        NewPositionTableRow["CardID"] = cardid;
                        NewPositionTableRow["StationID"] = NearStationID;
                        NewPositionTableRow["Geo_X"] = FinallyPoint[0];
                        NewPositionTableRow["Geo_Y"] = FinallyPoint[1];
                        NewPositionTableRow["NullSignalTimes"] = 0;
                        NewPositionTableRow["InNullRSSITime"] = DBNull.Value;
                        NewPositionTableRow["IsOperated"] = true;
                        NewPositionTableRow["Area"] = row_area[0]["Area"]; ;
                        PositionTable.Rows.Add(NewPositionTableRow);
                    }
                    AnalysicsNum++;
                }
                catch (Exception ex)
                {
                    if (Global.IsShowBug)
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器生成PositionTable错误");
                    /*如果出错。继续解析下一张卡片*/
                }
            }

           // long dt = DateTime.Now.Ticks- Global.SlickTimes*1000000;
                        
                        //if ((dt-Global.SlickTimes*1)>=0 )
            long dt = DateTime.Now.Ticks - 10 * 10000000;
            DataRow[] drArr =BasicPositionTable. Select("DT > "+dt );//另一种模糊查询的方法 
            //bcopy = BasicPositionTable.Copy(); 
            // BasicPositionTable.Rows.Clear(); 
            DataTable dtNew= BasicPositionTable.Clone();
            
            for (int i = 0; i < drArr.Length; i++)
            {
                dtNew.ImportRow(drArr[i]);

            }


            BasicPositionTable = dtNew.Copy();  
  
            return AnalysicsNum;
        }

        /// <summary>
        /// 根据当前的设置与PositionTable，刷新当前特殊区域内人员表
        /// </summary>
        private static void RefreshInAreaTable()
        {
            //清空InArea表
            InArea.Clear();
            //恢复IsExceedInArea为未超限
            IsExceedInArea = false;
            try
            {
                DataRow[] rows = BasicData.GetSpecalTableRows("Name = '" + AlarmAreaName + "'");
                if (rows.Length > 0)
                {
                    //初始化时间控制方式
                    bool IsTimeSpan = Convert.ToBoolean(rows[0]["IsTimeSpan"]);
                    //基站ID
                    int StationID1 = -1;
                    int StationID2 = -1;
                    int StationID3 = -1;
                    int StationID4 = -1;
                    int StationID5 = -1;
                    int StationID6 = -1;
                    //时间段
                    long TimeStartTicks1 = 0;
                    long TimeEndTicks1 = 0;
                    long TimeStartTicks2 = 0;
                    long TimeEndTicks2 = 0;
                    long TimeStartTicks3 = 0;
                    long TimeEndTicks3 = 0;
                    long TimeStartTicks4 = 0;
                    long TimeEndTicks4 = 0;
                    long TimeStartTicks5 = 0;
                    long TimeEndTicks5 = 0;
                    long TimeStartTicks6 = 0;
                    long TimeEndTicks6 = 0;
                    //每个基站的人数超限值
                    int AllowMaxPeople_Station1 = 0;
                    int AllowMaxPeople_Station2 = 0;
                    int AllowMaxPeople_Station3 = 0;
                    int AllowMaxPeople_Station4 = 0;
                    int AllowMaxPeople_Station5 = 0;
                    int AllowMaxPeople_Station6 = 0;
                    //初始化每个时间段和基站ID
                    StationID1 = Convert.ToInt32(rows[0]["StationID1"]);
                    AllowMaxPeople_Station1 = Convert.ToInt32(rows[0]["AllowMaxPeople1"]);
                    if (IsTimeSpan)
                    {
                        //时间控制方式为时间段，则初始化基站ID的同时初始化每个时间段
                        TimeStartTicks1 = Convert.ToDateTime(rows[0]["TimeStart1"]).TimeOfDay.Ticks;
                        TimeEndTicks1 = Convert.ToDateTime(rows[0]["TimeEnd1"]).TimeOfDay.Ticks;
                        //判断2号以后的存在性并初始化
                        if (rows[0]["StationID2"] != DBNull.Value)
                        {
                            StationID2 = Convert.ToInt32(rows[0]["StationID2"]);
                            TimeStartTicks2 = Convert.ToDateTime(rows[0]["TimeStart2"]).TimeOfDay.Ticks;
                            TimeEndTicks2 = Convert.ToDateTime(rows[0]["TimeEnd2"]).TimeOfDay.Ticks;
                            AllowMaxPeople_Station2 = Convert.ToInt32(rows[0]["AllowMaxPeople2"]);
                        }
                        else
                        {
                            goto StationOver;
                        }
                        if (rows[0]["StationID3"] != DBNull.Value)
                        {
                            StationID3 = Convert.ToInt32(rows[0]["StationID3"]);
                            TimeStartTicks3 = Convert.ToDateTime(rows[0]["TimeStart3"]).TimeOfDay.Ticks;
                            TimeEndTicks3 = Convert.ToDateTime(rows[0]["TimeEnd3"]).TimeOfDay.Ticks;
                            AllowMaxPeople_Station3 = Convert.ToInt32(rows[0]["AllowMaxPeople3"]);
                        }
                        else
                        {
                            goto StationOver;
                        }
                        if (rows[0]["StationID4"] != DBNull.Value)
                        {
                            StationID4 = Convert.ToInt32(rows[0]["StationID4"]);
                            TimeStartTicks4 = Convert.ToDateTime(rows[0]["TimeStart4"]).TimeOfDay.Ticks;
                            TimeEndTicks4 = Convert.ToDateTime(rows[0]["TimeEnd4"]).TimeOfDay.Ticks;
                            AllowMaxPeople_Station4 = Convert.ToInt32(rows[0]["AllowMaxPeople4"]);
                        }
                        else
                        {
                            goto StationOver;
                        }
                        if (rows[0]["StationID5"] != DBNull.Value)
                        {
                            StationID5 = Convert.ToInt32(rows[0]["StationID5"]);
                            TimeStartTicks5 = Convert.ToDateTime(rows[0]["TimeStart5"]).TimeOfDay.Ticks;
                            TimeEndTicks5 = Convert.ToDateTime(rows[0]["TimeEnd5"]).TimeOfDay.Ticks;
                            AllowMaxPeople_Station5 = Convert.ToInt32(rows[0]["AllowMaxPeople5"]);
                        }
                        else
                        {
                            goto StationOver;
                        }
                        if (rows[0]["StationID6"] != DBNull.Value)
                        {
                            StationID6 = Convert.ToInt32(rows[0]["StationID6"]);
                            TimeStartTicks6 = Convert.ToDateTime(rows[0]["TimeStart6"]).TimeOfDay.Ticks;
                            TimeEndTicks6 = Convert.ToDateTime(rows[0]["TimeEnd6"]).TimeOfDay.Ticks;
                            AllowMaxPeople_Station6 = Convert.ToInt32(rows[0]["AllowMaxPeople6"]);
                        }
                    }
                    else
                    {
                        //时间控制方式为手动,则只初始化基站ID
                        //判断2号以后的存在性并初始化
                        if (rows[0]["StationID2"] != DBNull.Value)
                        {
                            StationID2 = Convert.ToInt32(rows[0]["StationID2"]);
                            AllowMaxPeople_Station2 = Convert.ToInt32(rows[0]["AllowMaxPeople2"]);
                        }
                        else
                        {
                            goto StationOver;
                        }
                        if (rows[0]["StationID3"] != DBNull.Value)
                        {
                            StationID3 = Convert.ToInt32(rows[0]["StationID3"]);
                            AllowMaxPeople_Station3 = Convert.ToInt32(rows[0]["AllowMaxPeople3"]);
                        }
                        else
                        {
                            goto StationOver;
                        }
                        if (rows[0]["StationID4"] != DBNull.Value)
                        {
                            StationID4 = Convert.ToInt32(rows[0]["StationID4"]);
                            AllowMaxPeople_Station4 = Convert.ToInt32(rows[0]["AllowMaxPeople4"]);
                        }
                        else
                        {
                            goto StationOver;
                        }
                        if (rows[0]["StationID5"] != DBNull.Value)
                        {
                            StationID5 = Convert.ToInt32(rows[0]["StationID5"]);
                            AllowMaxPeople_Station5 = Convert.ToInt32(rows[0]["AllowMaxPeople5"]);
                        }
                        else
                        {
                            goto StationOver;
                        }
                        if (rows[0]["StationID6"] != DBNull.Value)
                        {
                            StationID6 = Convert.ToInt32(rows[0]["StationID6"]);
                            AllowMaxPeople_Station6 = Convert.ToInt32(rows[0]["AllowMaxPeople6"]);
                        }
                    }
                //至此,所有的存在的基站ID和时间段已经初始化OK.
                //开始根据是否在时间执行周期内来判断是否继续执行
                StationOver:
                    //默认不在任何基站的时间内
                    bool InTimeStation1 = false;
                    bool InTimeStation2 = false;
                    bool InTimeStation3 = false;
                    bool InTimeStation4 = false;
                    bool InTimeStation5 = false;
                    bool InTimeStation6 = false;
                    //如果是自动时间段方式，则判断是否在每个基站的执行时间内
                    if (IsTimeSpan)
                    {
                        if (DateTime.Now.TimeOfDay.Ticks <= TimeEndTicks1 && DateTime.Now.TimeOfDay.Ticks >= TimeStartTicks1)
                        {
                            InTimeStation1 = true;
                        }
                        if (StationID2 != -1)
                        {
                            //说明有2的时间，则判断2
                            if (DateTime.Now.TimeOfDay.Ticks <= TimeEndTicks2 && DateTime.Now.TimeOfDay.Ticks >= TimeStartTicks2)
                            {
                                InTimeStation2 = true;
                            }
                        }
                        else
                        {
                            goto CheckIsHadInTime;
                        }
                        if (StationID3 != -1)
                        {
                            //说明有3的时间，则判断3
                            if (DateTime.Now.TimeOfDay.Ticks <= TimeEndTicks3 && DateTime.Now.TimeOfDay.Ticks >= TimeStartTicks3)
                            {
                                InTimeStation3 = true;
                            }
                        }
                        else
                        {
                            goto CheckIsHadInTime;
                        }
                        if (StationID4 != -1)
                        {
                            //说明有4的时间，则判断4
                            if (DateTime.Now.TimeOfDay.Ticks <= TimeEndTicks4 && DateTime.Now.TimeOfDay.Ticks >= TimeStartTicks4)
                            {
                                InTimeStation4 = true;
                            }
                        }
                        else
                        {
                            goto CheckIsHadInTime;
                        }
                        if (StationID5 != -1)
                        {
                            //说明有5的时间，则判断5
                            if (DateTime.Now.TimeOfDay.Ticks <= TimeEndTicks5 && DateTime.Now.TimeOfDay.Ticks >= TimeStartTicks5)
                            {
                                InTimeStation5 = true;
                            }
                        }
                        else
                        {
                            goto CheckIsHadInTime;
                        }
                        if (StationID6 != -1)
                        {
                            //说明有6的时间，则判断6
                            if (DateTime.Now.TimeOfDay.Ticks <= TimeEndTicks6 && DateTime.Now.TimeOfDay.Ticks >= TimeStartTicks6)
                            {
                                InTimeStation6 = true;
                            }
                        }
                    //总结是否有在执行时间段内的
                    CheckIsHadInTime:
                        if (!InTimeStation1 && !InTimeStation2 && !InTimeStation3 && !InTimeStation4 && !InTimeStation5 && !InTimeStation6)
                        {
                            //说明没有在任何基站的时间执行周期内
                            //跳出
                            return;
                        }
                    }
                    else
                    {
                        //如果是手动方式，则认为在执行时间内
                        InTimeStation1 = true;
                        InTimeStation2 = true;
                        InTimeStation3 = true;
                        InTimeStation4 = true;
                        InTimeStation5 = true;
                        InTimeStation6 = true;
                    }
                    //至此说明，当前时间在某个（或多个）基站的时间段内,则开始判断特殊区域
                    //注：在时间段内就说明StationID != -1 故，不重复判断
                    if (InTimeStation1)
                    {
                        DataRow[] rows_Station1 = PositionTable.Select("StationID = " + StationID1);
                        for (int s1 = 0; s1 < rows_Station1.Length; s1++)
                        {
                            InArea.Add(Convert.ToInt32(rows_Station1[s1]["CardID"]), StationID1);
                        }
                        if (rows_Station1.Length > AllowMaxPeople_Station1)
                        {
                            //超限了
                            CheckInAreaInDB(AlarmAreaName, StationID1, rows_Station1, DateTime.Now);
                            IsExceedInArea = true;
                        }
                    }
                    if (InTimeStation2)
                    {
                        DataRow[] rows_Station2 = PositionTable.Select("StationID = " + StationID2);
                        for (int s2 = 0; s2 < rows_Station2.Length; s2++)
                        {
                            if (!InArea.ContainsKey(Convert.ToInt32(rows_Station2[s2]["CardID"])))
                            {
                                InArea.Add(Convert.ToInt32(rows_Station2[s2]["CardID"]), StationID2);
                            }
                        }
                        if (rows_Station2.Length > AllowMaxPeople_Station2)
                        {
                            //超限了
                            CheckInAreaInDB(AlarmAreaName, StationID2, rows_Station2, DateTime.Now);
                            IsExceedInArea = true;
                        }
                    }
                    if (InTimeStation3)
                    {
                        DataRow[] rows_Station3 = PositionTable.Select("StationID = " + StationID3);
                        for (int s3 = 0; s3 < rows_Station3.Length; s3++)
                        {
                            if (!InArea.ContainsKey(Convert.ToInt32(rows_Station3[s3]["CardID"])))
                            {
                                InArea.Add(Convert.ToInt32(rows_Station3[s3]["CardID"]), StationID3);
                            }
                        }
                        if (rows_Station3.Length > AllowMaxPeople_Station3)
                        {
                            //超限了
                            CheckInAreaInDB(AlarmAreaName, StationID3, rows_Station3, DateTime.Now);
                            IsExceedInArea = true;
                        }
                    }
                    if (InTimeStation4)
                    {
                        DataRow[] rows_Station4 = PositionTable.Select("StationID = " + StationID4);
                        for (int s4 = 0; s4 < rows_Station4.Length; s4++)
                        {
                            if (!InArea.ContainsKey(Convert.ToInt32(rows_Station4[s4]["CardID"])))
                            {
                                InArea.Add(Convert.ToInt32(rows_Station4[s4]["CardID"]), StationID4);
                            }
                        }
                        if (rows_Station4.Length > AllowMaxPeople_Station4)
                        {
                            //超限了
                            CheckInAreaInDB(AlarmAreaName, StationID4, rows_Station4, DateTime.Now);
                            IsExceedInArea = true;
                        }
                    }
                    if (InTimeStation5)
                    {
                        DataRow[] rows_Station5 = PositionTable.Select("StationID = " + StationID5);
                        for (int s5 = 0; s5 < rows_Station5.Length; s5++)
                        {
                            if (!InArea.ContainsKey(Convert.ToInt32(rows_Station5[s5]["CardID"])))
                            {
                                InArea.Add(Convert.ToInt32(rows_Station5[s5]["CardID"]), StationID5);
                            }
                        }
                        if (rows_Station5.Length > AllowMaxPeople_Station5)
                        {
                            //超限了
                            CheckInAreaInDB(AlarmAreaName, StationID5, rows_Station5, DateTime.Now);
                            IsExceedInArea = true;
                        }
                    }
                    if (InTimeStation6)
                    {
                        DataRow[] rows_Station6 = PositionTable.Select("StationID = " + StationID6);
                        for (int s6 = 0; s6 < rows_Station6.Length; s6++)
                        {
                            if (!InArea.ContainsKey(Convert.ToInt32(rows_Station6[s6]["CardID"])))
                            {
                                InArea.Add(Convert.ToInt32(rows_Station6[s6]["CardID"]), StationID6);
                            }
                        }
                        if (rows_Station6.Length > AllowMaxPeople_Station6)
                        {
                            //超限了
                            CheckInAreaInDB(AlarmAreaName, StationID6, rows_Station6, DateTime.Now);
                            IsExceedInArea = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                if (Global.IsShowBug)
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器计算特殊区域内人员错误");
            }
        }

        /// <summary>
        /// 手动使指定的人员离开
        /// </summary>
        /// <param name="CardID"></param>
        public static bool HandCheckOut(int CardID)
        {
            try
            {
                DataRow[] row_card = PositionTable.Select("CardID = " + CardID);
                if (row_card.Length > 0)
                {
                    //将平滑处理用到的“旧”点删除
                    if (OldPoints.ContainsKey(CardID))
                        OldPoints.Remove(CardID);
                    if (OldPointsAims.ContainsKey(CardID))
                        OldPointsAims.Remove(CardID);
                    //直接删除这条记录
                    row_card[0].Delete();
                    //写数据库
                    CheckDutyInDB(2, CardID, DateTime.Now);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 得到当前洞内人员的字符串描述
        /// ...!CardID?基站ID-日-时-分!Card?基站ID-日-时-分!...
        /// </summary>
        /// <returns></returns>
        private static string GetInMinePersonStr()
        {
            string resultDutyStr = "";
            Dictionary<int, string> InMineList_copy = new Dictionary<int, string>(InMineList);
            
            foreach (int cardID in InMineList_copy.Keys)
            {
                resultDutyStr += cardID.ToString() + "?" + InMineList_copy[cardID] + "!";
            }
            InMineList_copy = null;
            return resultDutyStr;
        }

        /// <summary>
        /// 得到当前特殊区域内人员的字符串
        /// 格式：!CardID?StationID!...!...!...
        /// </summary>
        /// <returns></returns>
        public static string GetPresentInAreaStr()
        {
            string resStr = "";

            Dictionary<int, int> InArea_copy = new Dictionary<int, int>(InArea);
            
            try
            {
                foreach (int CardID in InArea_copy.Keys)
                {
                    resStr += "!" + CardID.ToString() + "?" + InArea_copy[CardID].ToString();
                }
                InArea_copy=null ;
            }
            
            catch
            { }
            return resStr;
        }

        private static double[] ConvertRSSItoPoint(double StationMax_X, double StationMax_Y, double StationMin_X, double StationMin_Y, int RSSI_Max, int RSSI_Min)
        {
            double[] resultPoint = new double[2];
            //按RSSI比例进行定位
            double Proportion = System.Math.Pow(10, Convert.ToDouble((RSSI_Min - RSSI_Max)) /25.00 );//255.0(10.0 * 3.0)
            //按照比例在三角形的三边上找到的三个点
            resultPoint[0] = (Proportion * StationMin_X + StationMax_X) / (1 + Proportion);
            resultPoint[1] = (Proportion * StationMin_Y + StationMax_Y) / (1 + Proportion);
            return resultPoint;
        }

        /// <summary>
        /// 求三角形内心
        /// </summary>
        private static double[] InCenterPoint(double Xa, double Ya, double Xb, double Yb, double Xc, double Yc)
        {
            double[] result = new double[2];
            double DistanceAB = Math.Sqrt(Math.Pow(Xa - Xb, 2) + Math.Pow(Ya - Yb, 2));
            double DistanceBC = Math.Sqrt(Math.Pow(Xb - Xc, 2) + Math.Pow(Yb - Yc, 2));
            double DistanceAC = Math.Sqrt(Math.Pow(Xa - Xc, 2) + Math.Pow(Ya - Yc, 2));
            double TotalDistance = DistanceAB + DistanceBC + DistanceAC;
            result[0] = (DistanceBC * Xa + DistanceAC * Xb + DistanceAB * Xc) / TotalDistance;
            result[1] = (DistanceBC * Ya + DistanceAC * Yb + DistanceAB * Yc) / TotalDistance;
            return result;
        }

        /// <summary>
        /// 平滑处理方法
        /// </summary>
        /// <param name="CardIDKey"></param>
        /// <param name="NewPoint"></param>
        /// <returns></returns>
        private static double[] Slick(int CardID, double[] NewPoint)
        {
            if (OldPoints.ContainsKey(CardID))
            {
                bool TempXIsDifferent = false;//NewPoint与OldPoint在X方向上是否相反，默认一致
                bool TempYIsDifferent = false;//NewPoint与OldPoint在Y方向上是否相反，默认一致

                double[] OldPoint = OldPoints[CardID];


                if (Math.Abs(NewPoint[1] - OldPoint[1]) < (0.33 * Global.MaxMoveLength) && Math.Abs(NewPoint[0] - OldPoint[0]) < (0.33 * Global.MaxMoveLength))
                {

                    return OldPoint;
                                                
                }
                
                
                int[] Aims = new int[3];
                //如果之前有方向，说明没有因为方向相反而“定点”过。则判断
                if (OldPointsAims.ContainsKey(CardID))
                {
                    
                    
                    Aims = OldPointsAims[CardID];
                    //如果本次的NewPoint与OldPoint在X上的方向与上次的不一致，则TempXIsDifferent = true
                    if (NewPoint[0] >= OldPoint[0] && Aims[0] == 0)
                    {
                        TempXIsDifferent = true;
                    }
                    else if (NewPoint[0] <= OldPoint[0] && Aims[0] == 1)
                    {
                        TempXIsDifferent = true;
                    }
                    //如果本次的NewPoint与OldPoint在Y上的方向与上次的不一致，则TempYIsDifferent = true
                    if (NewPoint[1] >= OldPoint[1] && Aims[1] == 0)
                    {
                        TempYIsDifferent = true;
                    }
                    else if (NewPoint[1] <= OldPoint[1] && Aims[1] == 1)
                    {
                        TempYIsDifferent = true;
                    }
                    //如果X与Y方向都不一致并且没有平滑过，则本次平滑不动，直接跳出。
                    if (TempXIsDifferent && TempYIsDifferent )
                    {
                        Aims[2] += 1;
                        if (Aims[2] > Global.SlickTimes)
                       // if (Aims[2] > 2)
                        {
                            OldPointsAims.Remove(CardID);
                        }
                        else
                        {
                            OldPointsAims[CardID] = Aims;
                            return OldPoint;
                        }
                    }
                }
                //至此，说明至少一个方向一致或者已经平滑过一次了，则算新的方向
                if (NewPoint[0] > OldPoint[0])
                {
                    Aims[0] = 1;
                }
                else
                {
                    Aims[0] = 0;
                }
                if (NewPoint[1] > OldPoint[1])
                {
                    Aims[1] = 1;
                }
                else
                {
                    Aims[1] = 0;
                }
                Aims[2] = 0;
                if (OldPointsAims.ContainsKey(CardID))
                {
                    OldPointsAims[CardID] = Aims;
                }
                else
                {
                    OldPointsAims.Add(CardID, Aims);
                }
                //判断并计算新的点距离后返回
                double BigLength_X = NewPoint[0] - OldPoint[0];
                double BigLength_Y = NewPoint[1] - OldPoint[1];
                double BigLength = Math.Sqrt(Math.Pow(BigLength_X, 2) + Math.Pow(BigLength_Y, 2));
                if (BigLength > Global.MaxMoveLength)
                {
                    double pow = BigLength / Global.MaxMoveLength;
                    double[] UpdatePoint = new double[2] { OldPoint[0] + BigLength_X / pow, OldPoint[1] + BigLength_Y / pow };
                    OldPoints[CardID] = UpdatePoint;
                    return UpdatePoint;
                }
                else
                {
                    OldPoints[CardID] = NewPoint;
                    return NewPoint;
                }
            }
            else
            {
                OldPoints.Add(CardID, NewPoint);
                return NewPoint;
            }
        }

        /// <summary>
        /// 计算坐标离两个基站中的哪个最近
        /// 如果这两个基站的考勤编号是1号与2号，则引入Global.DistanceForDuty1Per参与计算，否则直接按50：50的比例计算
        /// </summary>
        /// <param name="FinallyPoint"></param>
        /// <param name="FirstStation"></param>
        /// <param name="SecondStation"></param>
        /// <returns>最近的基站ID</returns>
        private static int GetNearStationID_id(double[] FinallyPoint, DataRow FirstStation, int card_id,int rssi)
        {

            byte[] buf = new byte[6];
           // HardChannel hardChannel;
            DataRow[] rows_allStation = BasicData.GetStationTableRows("", true);
            double  min=6555;
            DateTime nowDateTime = DateTime.Now;
            if(card_id ==0x22f6)
            {
                min = 6777;
            }
            int st = Convert.ToInt32(FirstStation["ID"]);
            int k,j;
            j = 0;
            for (k = 0; k < rows_allStation.Length; k++)
            {
                if (ErrorStationList[Convert.ToInt32(rows_allStation[k]["ID"])] >= Global.ErrorStationTimes - 1)
                    continue;

                double DistanceToFirst =Math .Abs ( Math.Sqrt(Math.Pow(FinallyPoint[0] - Convert.ToDouble(rows_allStation[k ]["Geo_X"]), 2) + Math.Pow(FinallyPoint[1] - Convert.ToDouble(rows_allStation[k ]["Geo_Y"]), 2)));
                if((DistanceToFirst) <min )
                {
                   
                    min =(DistanceToFirst);
                    st= Convert.ToInt32(rows_allStation[k]["ID"]);
                    j = k;
                
                }
            }
            
          
#if false           

           if ((card_id>=8949 &&card_id <=8950) && rows_allStation[j]["DutyOrder"].ToString() == "2")
           {


                   
              


               if (!Global.DoorOOpened && rssi >= Global .ArriveRSSI  )
               {

                   Global.OpenedTick = DateTime.Now.Ticks;
                   Thread.Sleep(100);
                   Global.DoorOOpened = true ;

                   PersonPositionServer.Program.frmMain.send_OpenDoor1() ;

                   Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpMessage, card_id.ToString(), "里开门", nowDateTime.ToString() , "", "", "", "", "", "");
                  Thread.Sleep(100);
               }
                   
                 

           }

           if ((card_id >= 8949 && card_id <= 8950) && rows_allStation[j]["DutyOrder"].ToString() == "1")
           {

    //           
               //if (!Global.DoorOOpened)

               if (!Global.DoorOOpened && rssi >= Global.ArriveRSSI)
               {
                   Global .OpenedTick  = DateTime.Now.Ticks;
                   Thread.Sleep(100);
                   Global.DoorOOpened = true;
                  // Global.AlarmTick = DateTime.Now.Ticks;
                   PersonPositionServer.Program.frmMain.send_OpenDoor();
                   Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpMessage, card_id.ToString(), "外开门", nowDateTime.ToString(), "", "", "", "", "", "");
                   Thread.Sleep(100);
               }

           }



           
            
            if ((card_id >= 8947 && card_id <=8948) && rows_allStation[j]["DutyOrder"].ToString() == "4")
           {

          //     Global.OpenedTick = DateTime.Now.Ticks;
               //if (!Global.DoorOOpened)

               //if (!Global.DoorOOpened)
               {
                   //Thread.Sleep(100);
                  // Global.DoorOOpened = true;
                   //Global.AlarmTick = DateTime.Now.Ticks; 
                  // PersonPositionServer.Program.frmMain.send_CloseDoor ();
                   //Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpMessage, card_id.ToString(), "归位", nowDateTime.ToString(), "", "", "", "", "", "");
                   
                   Thread.Sleep(100);
               }

           }

           if ((card_id >= 8947 && card_id <=8948) && (rows_allStation[j]["DutyOrder"].ToString() == "5"||
               rssi <190))
           {

               Global.OpenedTick = DateTime.Now.Ticks;
               //if (!Global.DoorOOpened)

               //if (!Global.DoorOOpened)
               {
                   Thread.Sleep(100);
                   //Global.DoorOOpened = true;
                   Global.AlarmTick = DateTime.Now.Ticks;
                   PersonPositionServer.Program.frmMain.send_OpenDoor2();

                   Socket_Service.BroadcastMessage(Socket_Service.Command_S2C_UpMessage, card_id.ToString(), "离位", nowDateTime.ToString(), "", "", "", "", "", "");
                   Thread.Sleep(100);
               }

           }
#endif

           return st;
        /*        
                double DistanceToFirst = Math.Sqrt(Math.Pow(FinallyPoint[0] - Convert.ToDouble(FirstStation["Geo_X"]), 2) + Math.Pow(FinallyPoint[1] - Convert.ToDouble(FirstStation["Geo_Y"]), 2));
            double DistanceToSecond = Math.Sqrt(Math.Pow(FinallyPoint[0] - Convert.ToDouble(SecondStation["Geo_X"]), 2) + Math.Pow(FinallyPoint[1] - Convert.ToDouble(SecondStation["Geo_Y"]), 2));
            if (FirstStation["DutyOrder"] != DBNull.Value && SecondStation["DutyOrder"] != DBNull.Value)
            {
                if (FirstStation["DutyOrder"].ToString() == "1" && SecondStation["DutyOrder"].ToString() == "2")
                {
                    if (DistanceToFirst * Global.DistanceForDuty1Per > DistanceToSecond)
                    {
                        return Convert.ToInt32(SecondStation["ID"]);
                    }
                    else
                    {
                        return Convert.ToInt32(FirstStation["ID"]);
                    }
                }
                else if (FirstStation["DutyOrder"].ToString() == "2" && SecondStation["DutyOrder"].ToString() == "1")
                {
                    if (DistanceToSecond * Global.DistanceForDuty1Per > DistanceToFirst)
                    {
                        return Convert.ToInt32(FirstStation["ID"]);
                    }
                    else
                    {
                        return Convert.ToInt32(SecondStation["ID"]);
                    }
                }
                else
                {
                    if (DistanceToFirst > DistanceToSecond)
                    {
                        return Convert.ToInt32(SecondStation["ID"]);
                    }
                    else
                    {
                        return Convert.ToInt32(FirstStation["ID"]);
                    }
                }
            }
            else
            {
                if (DistanceToFirst > DistanceToSecond)
                {
                    return Convert.ToInt32(SecondStation["ID"]);
                }
                else
                {
                    return Convert.ToInt32(FirstStation["ID"]);
                }
            }*/
        }

        /// <summary>
        /// 初始化故障基站列表
        /// 首次初始化把所有基站统统视为未知状态
        /// </summary>
        public static void InitErrorList()
        {
            ErrorStationList.Clear();
            DataRow[] StationRows = BasicData.GetStationTableRows("",true);
            for (int i = 0; i < StationRows.Length; i++)
            {
                try
                {
                    ErrorStationList.Add(Convert.ToInt32(StationRows[i]["ID"]), 0);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 根据相应的关系把考勤写入数据库
        /// 1进入 2离开 3未通过考勤基站进入 4超时强制离开
        /// </summary>
        /// <param name="DutyMethod"></param>
        /// <param name="CardID"></param>
        /// <param name="Time"></param>
        private static void CheckDutyInDB(int DutyMethod, int CardID, DateTime Time)
        {
            switch (DutyMethod)
            {
                case 1:
                    //进入
                    DataTable tempTable = DB_Service.GetTable("TempTable", "Select * from DutyTable where CardID = " + CardID + " and OutTime is null");
                    if (tempTable.Rows.Count > 0)
                    {
                        //尚未离开，则判断洞内时间
                        DateTime intime = Convert.ToDateTime(tempTable.Rows[0]["InTime"]);
                        TimeSpan ts = Time - intime;
                        if (ts.TotalHours > Global.AlarmMaxHour)
                        {
                            DB_Service.ExecuteSQL("update DutyTable set OutTime = '" + intime.AddHours(Global.AlarmMaxHour) + "',DataSource = '系统-异常离开' where CardID = " + CardID + " and OutTime is null");
                        }
                        else
                        {
                            return;
                        }
                    }
                    DB_Service.ExecuteSQL("insert into DutyTable (CardID,Date,InTime,DataSource) values (" + CardID + ",'" + Time.Date + "','" + Time + "','系统-正常进入')");
                    break;
                case 2:
                    //离开
                    DataTable tempTable1 = DB_Service.GetTable("TempTable", "Select * from DutyTable where CardID = " + CardID + " and OutTime is null");
                    if (tempTable1.Rows.Count > 0)
                    {
                        for (int i = 0; i < tempTable1.Rows.Count; i++)
                        {
                            try
                            {
                                TimeSpan timespan = Time.Subtract(Convert.ToDateTime(tempTable1.Rows[i]["InTime"]));
                                if (timespan.TotalMinutes >= 1)
                                {
                                    DB_Service.ExecuteSQL("update DutyTable set OutTime = '" + Time + "',DataSource = '系统-正常离开' where CardID = " + CardID + " and OutTime is null");
                                }
                                else
                                {

                                    DB_Service.ExecuteSQL("delete from DutyTable where ID= " + Convert.ToInt32(tempTable1.Rows[i]["ID"]));
                                }
                            }
                            catch (Exception ex)
                            {
                                if (Global.IsShowBug)
                                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器考勤出洞的数据库设置错误");
                            }
                        }
                    }
                    else
                    {
                        //DB_Service.ExecuteSQL("insert into DutyTable (CardID,Date,InTime,OutTime,DataSource) values (" + CardID + ",'" + Time.Date + "','" + Time + "','" + Time + "','系统-异常进入')");
                    }
                    break;
                case 3:
                    //未通过考勤基站进入
                    DataTable tempTable2 = DB_Service.GetTable("TempTable", "Select * from DutyTable where CardID = " + CardID + " and OutTime is null");
                    if (tempTable2.Rows.Count == 0)
                    {
                        DB_Service.ExecuteSQL("insert into DutyTable (CardID,Date,InTime,DataSource) values (" + CardID + ",'" + Time.Date + "','" + Time + "','系统-正常进入')");
                    }
                    break;
                case 4:
                    //超时强制离开
                    DataTable tempTable3 = DB_Service.GetTable("TempTable", "Select * from DutyTable where CardID = " + CardID + " and OutTime is null");
                    if (tempTable3.Rows.Count > 0)
                    {
                        for (int i = 0; i < tempTable3.Rows.Count; i++)
                        {
                            try
                            {
                                TimeSpan timespan = Time.Subtract(Convert.ToDateTime(tempTable3.Rows[i]["InTime"]));
                                if (timespan.TotalMinutes >= 1)
                                {
                                    DB_Service.ExecuteSQL("update DutyTable set OutTime = '" + Time + "',DataSource = '系统-超时强制离开' where CardID = " + CardID + " and OutTime is null");
                                }
                                else
                                {
                                    DB_Service.ExecuteSQL("delete from DutyTable where ID= " + Convert.ToInt32(tempTable3.Rows[i]["ID"]));
                                }
                            }
                            catch (Exception ex)
                            {
                                if (Global.IsShowBug)
                                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器考勤出洞的数据库设置错误");
                            }
                        }
                    }
                    break;
            }
        }

        private static void CheckInAreaInDB(string AreaProjectName, int StationID, DataRow[] PeopleRows, DateTime Time)
        {
            List<string> sqlList = new List<string>();
            for (int i = 0; i < PeopleRows.Length; i++)
            {
                int CardID = Convert.ToInt32(PeopleRows[i]["CardID"]);
                DataTable tempTable = DB_Service.GetTable("tempTable", "select * from AlarmInAreaTable where StationID = " + StationID + " and CardID =" + CardID);
                if (tempTable.Rows.Count > 0)
                {
                    DateTime LastInsertTime = Convert.ToDateTime(tempTable.Rows[tempTable.Rows.Count - 1]["Time"]);
                    TimeSpan ts = Time - LastInsertTime;
                    if (ts.TotalMinutes < 60)
                    {
                        continue;
                    }
                }
                //插入新的报警记录SQL语句
                sqlList.Add("insert into AlarmInAreaTable (InAreaName,StationID,CardID,Time) values ('" + AreaProjectName + "'," + StationID + "," + CardID + ",'" + Time + "')");
            }
            if (sqlList.Count > 0)
                DB_Service.ExecuteSQLs(sqlList);
        }

        public static void ResumeALL()
        {
            //硬件合法性状态
            HardLawful = -1;
            //平滑处理函数用到的表
            OldPoints.Clear();
            OldPointsAims.Clear();
            //特殊区域用到的变量
            AlarmAreaName = "未启动";
            InArea.Clear();
            IsExceedInArea = false;
            //洞内人员
            InMineList.Clear();
            //显示进出洞人员字符串
            JustNowInStr = "";
            JustNowOutStr = "";
            //保存一次巡检解析出的信息
            BasicPositionTable.Rows.Clear();
            //保存一次计算完毕的定位综合信息
            PositionTable.Rows.Clear();
            //临时历史定位信息表
            TempHistoryPositionTable.Clear();
            //当前故障的基站列表
            ErrorStationList.Clear();
            //控制一个人员在两秒钟之内只筛选一条短信的变量
            LastSendMessage_CardID = -1;//上一次发送消息的卡片ID
            //底层的卡片定位信息数量
            BasicNum_Position = 0;
            //底层的信息采集数量
            BasicNum_Collect = 0;
            //分析定位信息数量
            AnalysicsNum = 0;
            //插入定位信息数量
            InsertDBNum = 0;
            //插入采集信息数量
            InsertDBNum_Collect = 0;
            //接收考勤信息数量
            DutyNum = 0;
            //是否是连续包
            IsContinue = false;
            //包序号
            BagNum = 0;
            //是否需要应答
            IsNeedResponse = false; 
            //红外标志量
            HW_IncreaseNum = 0;
            HW_LastInTime = new DateTime(2000, 1, 1, 1, 1, 1);
            //基站相关性数组
            StationRelation = null;
        }
    }
}
