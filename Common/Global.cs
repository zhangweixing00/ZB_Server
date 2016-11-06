using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using PersonPositionServer.Model;

namespace PersonPositionServer.Common
{
    public abstract class Global
    {
        //串口参数
        private static string _port_port;
        private static string _port_bote;
        private static string _port_check;
        private static string _port_data;
        private static string _port_stop;
        //其他参数
        private static string _isNetConnect;
        private static string _serverPort;
        private static string _loopTime;
        private static string _otherLoopTime;
        private static string _historyPrecision;
        private static string _historyPrecision_Collect;
        private static string _isCheckOutRSSI;
        private static string _checkOutRSSI;
        private static string _maxMoveLength;//每个卡片移动的最大值（大于这个值，则平滑处理）
        private static string _arriveRSSI;//认为到达基站旁边的RSSI值，用于定位
        private static string _companyName;
        private static string _hardSN;
        private static string _dBConnStr;
        private static string _autoRunApp;
        private static string _autoStart;
        private static string _isLinePosition;//是否只使用线性定位算法
        private static string _errorStationTimes;
        private static string _slickTimes;
        private static string _nullSignalTimes;//无信号触发出动或进入盲区开关量（默认8）
        private static string _distanceForDuty1Per;//判定距离考勤1号基站的长度的缩小系数（默认0.7）
        private static string _showLowPower;
        private static string _alarmMaxPerson;
        private static string _alarmMaxHour;
        private static string _outWhen1GetRSSI;
        private static string _isShowBug;
        private static string _isOtherDirect;
        private static string _isUseHongWai;
        private static string  _Isnew ;
        private static string _product;


        //硬件参数
        private static string _currentlyMes;
        private static string _upM0;
        private static string _upM1;
        private static string _upM2;
        private static string _upM3;
        private static string _upM4;
        private static string _upM5;
        private static string _upM6;
        private static string _upM7;
        private static string _upM8;
        private static string _upM9;
        private static string _upM10;
        private static string _upM11;
        private static string _upM12;
        private static string _upM13;
        private static string _upM14;
        private static string _upM15;
        private static string _upM16;
        private static string _upM17;
        private static string _upM18;
        private static string _upM19;
        private static string _upM20;
        private static string _upM21;
        private static string _upM22;
        private static string _upM23;
        private static string _upM24;
        private static string _upM25;
        private static string _upM26;
        private static string _upM27;
        private static string _upM28;
        private static string _upM29;
        private static string _downM0;
        private static string _downM1;
        private static string _downM2;
        private static string _downM3;
        private static string _downM4;
        private static string _downM5;
        private static string _downM6;
        private static string _downM7;
        private static string _downM8;
        private static string _downM9;
        private static string _downM10;
        private static string _downM11;

        public static long AlarmTick = 0; 
        public static long OpenedTick=0; 
        public static bool DoorOOpened=false ;
        private static string _DisConnClientTime;
        //服务端巡检客户端心跳定时间隔
        private static string _CheckClientConnInterval;
        public static bool Isloopend=false ;   
        
        //是否是演示版
        public static bool IsTempVersion = false   ;
        //是否是限制版
        public static bool IsLimitVersion = false   ;
        //巡检返回处理完毕标志
        public static bool IsOperateComplete = false;
        //允许一次巡检中没有任何数据的最大次数
        public const int MaxNothingTimes = 5;
        //服务器端接收的最小数据帧的长度,就是一个不包含有效数据的空帧的最小长度
        public const int MinBagLength = 10;
        private static string ConfigFile = AppDomain.CurrentDomain.BaseDirectory + "Config.config";
        private static string ConfigFile_Hard = AppDomain.CurrentDomain.BaseDirectory + "HardConfig.config";
        public static string ConfigTempData = AppDomain.CurrentDomain.BaseDirectory + "TempData.txt";
        public static string StationRelationFile = AppDomain.CurrentDomain.BaseDirectory + "StationRelation.txt";

        //////////////////////////////
        /// <summary>
        /// 最大客户端连接数量限制！！！！！
        /// </summary>
        public static int MaxUserNum
        {
            get 
            {
                if (IsTempVersion)
                    return 1;
                else
                    return 99;
            }
        }

        //////////////////////////////////////////////
        //命令返回开关变量
        //////////////////////////////////////////////
        //设置子基站命令的返回的状态
        public static bool Result_AddSon = false;
        //设置时间命令的返回的状态
        public static bool Result_SetTime = false;
        //删除子基站命令的返回的状态
        public static bool Result_DelSon = false;
        //点亮灯命令的返回的状态
        public static bool Result_LightUp = false;
        //取得历史数据命令的返回的状态
        public static bool Result_GetHistoryData = false;

        #region 属性_其他参数
        public static int CheckClientConnInterval
        {
            get
            {
                if (_CheckClientConnInterval == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/CheckClientConnInterval");
                    if (node != null)
                    {
                        _CheckClientConnInterval = node.InnerText;
                    }
                    else
                    {
                        _CheckClientConnInterval = "1000";
                        AddNodeNoAttrib(ConfigFile, @"/config/CheckClientConnInterval", _CheckClientConnInterval.ToString());
                    }
                }
                return Convert.ToInt32(_CheckClientConnInterval);
            }
            set
            {
                _CheckClientConnInterval = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/CheckClientConnInterval");
                if (node != null)
                {
                    node.InnerText = _CheckClientConnInterval;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/CheckClientConnInterval", _CheckClientConnInterval);
                }
            }
        }
        public static bool IsNetConnect
        {
            get
            {
                if (_isNetConnect == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/IsNetConnect");
                    if (node != null)
                    {
                        _isNetConnect = node.InnerText;
                    }
                    else
                    {
                        _isNetConnect = "True";
                        AddNodeNoAttrib(ConfigFile, @"/config/IsNetConnect", _isNetConnect);
                    }
                }
                return Convert.ToBoolean(_isNetConnect);
            }
            set
            {
                _isNetConnect = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/IsNetConnect");
                if (node != null)
                {
                    node.InnerText = _isNetConnect;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/IsNetConnect", _isNetConnect);
                }
            }
        }

        public static int ServerPort
        {
            get 
            {
                if (_serverPort == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/ServerPort");
                    if (node != null)
                    {
                        _serverPort = node.InnerText;
                    }
                    else
                    {
                        _serverPort = "7898";
                        AddNodeNoAttrib(ConfigFile, @"/config/ServerPort", _serverPort);
                    }
                }
                return Convert.ToInt32(_serverPort);
            }
            set 
            { 
                _serverPort = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/ServerPort");
                if (node != null)
                {
                    node.InnerText = _serverPort;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/ServerPort", _serverPort);
                }
            }
        }

        public static int LoopTime
        {
            get
            {
                if (_loopTime == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/LoopTime");
                    if (node != null)
                    {
                        _loopTime = node.InnerText;
                    }
                    else
                    {
                        _loopTime = "2000";
                        AddNodeNoAttrib(ConfigFile, @"/config/LoopTime", _loopTime);
                    }
                }
                return Convert.ToInt32(_loopTime);
            }
            set
            {
                _loopTime = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/LoopTime");
                if (node != null)
                {
                    node.InnerText = _loopTime;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/LoopTime", _loopTime);
                }
            }
        }

        public static int OtherLoopTime
        {
            get
            {
                if (_otherLoopTime == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/OtherLoopTime");
                    if (node != null)
                    {
                        _otherLoopTime = node.InnerText;
                    }
                    else
                    {
                        _otherLoopTime = "100";
                        AddNodeNoAttrib(ConfigFile, @"/config/OtherLoopTime", _otherLoopTime);
                    }
                }
                return Convert.ToInt32(_otherLoopTime);
            }
            set
            {
                _otherLoopTime = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/OtherLoopTime");
                if (node != null)
                {
                    node.InnerText = _otherLoopTime;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/OtherLoopTime", _otherLoopTime);
                }
            }
        }

        public static int HistoryPrecision
        {
            get
            {
                if (_historyPrecision == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/HistoryPrecision");
                    if (node != null)
                    {
                        _historyPrecision = node.InnerText;
                    }
                    else
                    {
                        _historyPrecision = "1";
                        AddNodeNoAttrib(ConfigFile, @"/config/HistoryPrecision", _historyPrecision);
                    }
                }
                return Convert.ToInt32(_historyPrecision);
            }
            set
            {
                _historyPrecision = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/HistoryPrecision");
                if (node != null)
                {
                    node.InnerText = _historyPrecision;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/HistoryPrecision", _historyPrecision);
                }
            }
        }

        public static int HistoryPrecision_Collect
        {
            get
            {
                if (_historyPrecision_Collect == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/HistoryPrecision_Collect");
                    if (node != null)
                    {
                        _historyPrecision_Collect = node.InnerText;
                    }
                    else
                    {
                        _historyPrecision_Collect = "5";
                        AddNodeNoAttrib(ConfigFile, @"/config/HistoryPrecision_Collect", _historyPrecision_Collect);
                    }
                }
                return Convert.ToInt32(_historyPrecision_Collect);
            }
            set
            {
                _historyPrecision_Collect = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/HistoryPrecision_Collect");
                if (node != null)
                {
                    node.InnerText = _historyPrecision_Collect;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/HistoryPrecision_Collect", _historyPrecision_Collect);
                }
            }
        }

        public static bool IsCheckOutRSSI
        {
            get
            {
                if (_isCheckOutRSSI == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/IsCheckOutRSSI");
                    if (node != null)
                    {
                        _isCheckOutRSSI = node.InnerText;
                    }
                    else
                    {
                        _isCheckOutRSSI = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/IsCheckOutRSSI", _isCheckOutRSSI);
                    }
                }
                return Convert.ToBoolean(_isCheckOutRSSI);
            }
            set
            {
                _isCheckOutRSSI = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/IsCheckOutRSSI");
                if (node != null)
                {
                    node.InnerText = _isCheckOutRSSI;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/IsCheckOutRSSI", _isCheckOutRSSI);
                }
            }
        }


        public static byte CheckOutRSSI
        {
            get
            {
                if (_checkOutRSSI == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/CheckOutRSSI");
                    if (node != null)
                    {
                        _checkOutRSSI = node.InnerText;
                    }
                    else
                    {
                        _checkOutRSSI = "175";
                        AddNodeNoAttrib(ConfigFile, @"/config/CheckOutRSSI", _checkOutRSSI);
                    }
                }
                return Convert.ToByte(_checkOutRSSI);
            }
            set
            {
                _checkOutRSSI = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/CheckOutRSSI");
                if (node != null)
                {
                    node.InnerText = _checkOutRSSI;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/CheckOutRSSI", _checkOutRSSI);
                }
            }
        }

        public static double MaxMoveLength
        {
            get
            {
                if (_maxMoveLength == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/MaxMoveLength");
                    if (node != null)
                    {
                        _maxMoveLength = node.InnerText;
                    }
                    else
                    {
                        _maxMoveLength = "1.5";
                        AddNodeNoAttrib(ConfigFile, @"/config/MaxMoveLength", _maxMoveLength);
                    }
                }
                return Convert.ToDouble(_maxMoveLength);
            }
            set
            {
                _maxMoveLength = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/MaxMoveLength");
                if (node != null)
                {
                    node.InnerText = _maxMoveLength;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/MaxMoveLength", _maxMoveLength);
                }
            }
        }

        public static int ArriveRSSI
        {
            get
            {
                if (_arriveRSSI == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/ArriveRSSI");
                    if (node != null)
                    {
                        _arriveRSSI = node.InnerText;
                    }
                    else
                    {
                        _arriveRSSI = "216";
                        AddNodeNoAttrib(ConfigFile, @"/config/ArriveRSSI", _arriveRSSI);
                    }
                }
                return Convert.ToInt32(_arriveRSSI);
            }
            set
            {
                _arriveRSSI = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/ArriveRSSI");
                if (node != null)
                {
                    node.InnerText = _arriveRSSI;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/ArriveRSSI", _arriveRSSI);
                }
            }
        }

        public static int AlarmMaxPerson
        {
            get
            {
                if (_alarmMaxPerson == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/AlarmMaxPerson");
                    if (node != null)
                    {
                        _alarmMaxPerson = node.InnerText;
                    }
                    else
                    {
                        _alarmMaxPerson = "100";
                        AddNodeNoAttrib(ConfigFile, @"/config/AlarmMaxPerson", _alarmMaxPerson);
                    }
                }
                return Convert.ToInt32(_alarmMaxPerson);
            }
            set
            {
                _alarmMaxPerson = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/AlarmMaxPerson");
                if (node != null)
                {
                    node.InnerText = _alarmMaxPerson;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/AlarmMaxPerson", _alarmMaxPerson);
                }
            }
        }

        public static int AlarmMaxHour
        {
            get
            {
                if (_alarmMaxHour == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/AlarmMaxHour");
                    if (node != null)
                    {
                        _alarmMaxHour = node.InnerText;
                    }
                    else
                    {
                        _alarmMaxHour = "9";
                        AddNodeNoAttrib(ConfigFile, @"/config/AlarmMaxHour", _alarmMaxHour);
                    }
                }
                return Convert.ToInt32(_alarmMaxHour);
            }
            set
            {
                _alarmMaxHour = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/AlarmMaxHour");
                if (node != null)
                {
                    node.InnerText = _alarmMaxHour;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/AlarmMaxHour", _alarmMaxHour);
                }
            }
        }

        public static int ErrorStationTimes
        {
            get
            {
                if (_errorStationTimes == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/ErrorStationTimes");
                    if (node != null)
                    {
                        _errorStationTimes = node.InnerText;
                    }
                    else
                    {
                        _errorStationTimes = "15";
                        AddNodeNoAttrib(ConfigFile, @"/config/ErrorStationTimes", _errorStationTimes);
                    }
                }
                return Convert.ToInt32(_errorStationTimes);
            }
            set
            {
                _errorStationTimes = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/ErrorStationTimes");
                if (node != null)
                {
                    node.InnerText = _errorStationTimes;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/ErrorStationTimes", _errorStationTimes);
                }
            }
        }

        public static int SlickTimes
        {
            get
            {
                if (_slickTimes == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/SlickTimes");
                    if (node != null)
                    {
                        _slickTimes = node.InnerText;
                    }
                    else
                    {
                        _slickTimes = "2";
                        AddNodeNoAttrib(ConfigFile, @"/config/SlickTimes", _slickTimes);
                    }
                }
                return Convert.ToInt32(_slickTimes);
            }
            set
            {
                _slickTimes = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/SlickTimes");
                if (node != null)
                {
                    node.InnerText = _slickTimes;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/SlickTimes", _slickTimes);
                }
            }
        }

        public static bool OutWhen1GetRSSI
        {
            get
            {
                if (_outWhen1GetRSSI == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/OutWhen1GetRSSI");
                    if (node != null)
                    {
                        _outWhen1GetRSSI = node.InnerText;
                    }
                    else
                    {
                        _outWhen1GetRSSI = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/OutWhen1GetRSSI", _outWhen1GetRSSI);
                    }
                }
                return Convert.ToBoolean(_outWhen1GetRSSI);
            }
            set
            {
                _outWhen1GetRSSI = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/OutWhen1GetRSSI");
                if (node != null)
                {
                    node.InnerText = _outWhen1GetRSSI;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/OutWhen1GetRSSI", _outWhen1GetRSSI);
                }
            }
        }

        public static int NullSignalTimes
        {
            get
            {
                if (_nullSignalTimes == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/NullSignalTimes");
                    if (node != null)
                    {
                        _nullSignalTimes = node.InnerText;
                    }
                    else
                    {
                        _nullSignalTimes = "8";
                        AddNodeNoAttrib(ConfigFile, @"/config/NullSignalTimes", _nullSignalTimes);
                    }
                }
                return Convert.ToInt32(_nullSignalTimes);
            }
            set
            {
                _nullSignalTimes = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/NullSignalTimes");
                if (node != null)
                {
                    node.InnerText = _nullSignalTimes;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/NullSignalTimes", _nullSignalTimes);
                }
            }
        }

        public static double DistanceForDuty1Per
        {
            get
            {
                if (_distanceForDuty1Per == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/DistanceForDuty1Per");
                    if (node != null)
                    {
                        _distanceForDuty1Per = node.InnerText;
                    }
                    else
                    {
                        _distanceForDuty1Per = "0.7";
                        AddNodeNoAttrib(ConfigFile, @"/config/DistanceForDuty1Per", _distanceForDuty1Per);
                    }
                }
                return Convert.ToDouble(_distanceForDuty1Per);
            }
            set
            {
                _distanceForDuty1Per = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/DistanceForDuty1Per");
                if (node != null)
                {
                    node.InnerText = _distanceForDuty1Per;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/DistanceForDuty1Per", _distanceForDuty1Per);
                }
            }
        }

        public static string CompanyName
        {
            get
            {
                if (_companyName == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/reg/companyname");
                    if (node != null)
                    {
                        _companyName = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少CompanyName项，请确保配置文件的正确、完整性。");
                        System.Environment.Exit(0);
                    }
                }
                return _companyName;
            }
            set
            {
                _companyName = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/reg/companyname");
                if (node != null)
                {
                    node.InnerText = _companyName;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少CompanyName项，请确保配置文件的正确、完整性。");
                    System.Environment.Exit(0);
                }
            }
        }

        public static string HardSN
        {
            get
            {
                if (_hardSN == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/reg/hardsn");
                    if (node != null)
                    {
                        _hardSN = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少HardSN项，请确保配置文件的正确、完整性。");
                        System.Environment.Exit(0);
                    }
                }
                return _hardSN;
            }
            set
            {
                _hardSN = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/reg/hardsn");
                if (node != null)
                {
                    node.InnerText = _hardSN;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少HardSN项，请确保配置文件的正确、完整性。");
                    System.Environment.Exit(0);
                }
            }
        }

        public static string DBConnStr
        {
            get
            {
                if (_dBConnStr == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/DBConnStr");
                    if (node != null)
                    {
                        _dBConnStr = node.InnerText;
                    }
                    else
                    {
                        _dBConnStr = @"server=localhost\SQLEXPRESS;Database=PersonPosition;User ID=sa;Password=123456;";
                        AddNodeNoAttrib(ConfigFile, @"/config/DBConnStr", _dBConnStr);
                    }
                }
                return _dBConnStr;
            }
            set
            {
                _dBConnStr = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/DBConnStr");
                if (node != null)
                {
                    node.InnerText = _dBConnStr;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/DBConnStr", _dBConnStr);
                }
            }
        }

        public static bool AutoRunApp
        {
            get
            {
                if (_autoRunApp == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/AutoRunApp");
                    if (node != null)
                    {
                        _autoRunApp = node.InnerText;
                    }
                    else
                    {
                        _autoRunApp = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/AutoRunApp", _autoRunApp);
                    }
                }
                return Convert.ToBoolean(_autoRunApp);
            }
            set
            {
                _autoRunApp = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/AutoRunApp");
                if (node != null)
                {
                    node.InnerText = _autoRunApp;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/AutoRunApp", _autoRunApp);
                }
            }
        }

        public static bool AutoStart
        {
            get
            {
                if (_autoStart == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/AutoStart");
                    if (node != null)
                    {
                        _autoStart = node.InnerText;
                    }
                    else
                    {
                        _autoStart = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/AutoStart", _autoStart);
                    }
                }
                return Convert.ToBoolean(_autoStart);
            }
            set
            {
                _autoStart = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/AutoStart");
                if (node != null)
                {
                    node.InnerText = _autoStart;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/AutoStart", _autoStart);
                }
            }
        }

        public static bool IsLinePosition
        {
            get
            {
                if (_isLinePosition == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/IsLinePosition");
                    if (node != null)
                    {
                        _isLinePosition = node.InnerText;
                    }
                    else
                    {
                        _isLinePosition = "True";
                        AddNodeNoAttrib(ConfigFile, @"/config/IsLinePosition", _isLinePosition);
                    }
                }
                return Convert.ToBoolean(_isLinePosition);
            }
            set
            {
                _isLinePosition = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/IsLinePosition");
                if (node != null)
                {
                    node.InnerText = _isLinePosition;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/IsLinePosition", _isLinePosition);
                }
            }
        }

        public static bool ShowLowPower
        {
            get
            {
                if (_showLowPower == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/ShowLowPower");
                    if (node != null)
                    {
                        _showLowPower = node.InnerText;
                    }
                    else
                    {
                        _showLowPower = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/ShowLowPower", _showLowPower);
                    }
                }
                return Convert.ToBoolean(_showLowPower);
            }
            set
            {
                _showLowPower = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/ShowLowPower");
                if (node != null)
                {
                    node.InnerText = _showLowPower;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/ShowLowPower", _showLowPower);
                }
            }
        }

        public static bool IsShowBug
        {
            get
            {
                if (_isShowBug == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/IsShowBug");
                    if (node != null)
                    {
                        _isShowBug = node.InnerText;
                    }
                    else
                    {
                        _isShowBug = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/IsShowBug", _isShowBug);
                    }
                }
                return Convert.ToBoolean(_isShowBug);
            }
            set
            {
                _isShowBug = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/IsShowBug");
                if (node != null)
                {
                    node.InnerText = _isShowBug;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/IsShowBug", _isShowBug);
                }
            }
        }



        public static bool Isnew
        {
            get
            {
                if (_Isnew == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/Isnew");
                    if (node != null)
                    {
                        _Isnew = node.InnerText;
                    }
                    else
                    {
                        _Isnew = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/Isnew", _Isnew);
                    }
                }
                
                return Convert.ToBoolean(_Isnew);
            }
            set
            {
                _Isnew = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/Isnew");
                if (node != null)
                {
                    node.InnerText = _Isnew;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/Isnew", _Isnew);
                }
            }
        }
        public static int DisConnClientTime
        {
            get
            {
                if (_DisConnClientTime == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/ClientDisconnectTime");
                    if (node != null)
                    {
                        _DisConnClientTime = node.InnerText;
                    }
                    else
                    {
                        _DisConnClientTime = "30";
                        AddNodeNoAttrib(ConfigFile, @"/config/ClientDisconnectTime", _DisConnClientTime.ToString());
                    }
                }
                return Convert.ToInt32(_DisConnClientTime);
            }
            set
            {
                _DisConnClientTime = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/ClientDisconnectTime");
                if (node != null)
                {
                    node.InnerText = _DisConnClientTime;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/ClientDisconnectTime", _DisConnClientTime);
                }
            }
        }
        public static string Product
        {
            get
            {
                if (_product == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/Product");
                    if (node != null)
                    {
                        _product = node.InnerText;
                    }
                    else
                    {
                        _product = "";
                        AddNodeNoAttrib(ConfigFile, @"/config/Product", _product);
                    }
                }
                return _product;
            }
            set
            {
                _product = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/Product");
                if (node != null)
                {
                    node.InnerText = _product;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/Product", _product);
                }
            }
        } 
        
        public static bool IsUseHongWai
        {
            get
            {
                if (_isUseHongWai == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/IsUseHongWai");
                    if (node != null)
                    {
                        _isUseHongWai = node.InnerText;
                    }
                    else
                    {
                        _isUseHongWai = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/IsUseHongWai", _isUseHongWai);
                    }
                }
                return Convert.ToBoolean(_isUseHongWai);
            }
            set
            {
                _isUseHongWai = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/IsUseHongWai");
                if (node != null)
                {
                    node.InnerText = _isUseHongWai;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/IsUseHongWai", _isUseHongWai);
                }
            }
        }

        public static bool IsOtherDirect
        {
            get
            {
                if (_isOtherDirect == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/IsOtherDirect");
                    if (node != null)
                    {
                        _isOtherDirect = node.InnerText;
                    }
                    else
                    {
                        _isOtherDirect = "False";
                        AddNodeNoAttrib(ConfigFile, @"/config/IsOtherDirect", _isOtherDirect);
                    }
                }
                return Convert.ToBoolean(_isOtherDirect);
            }
            set
            {
                _isOtherDirect = value.ToString();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/IsOtherDirect");
                if (node != null)
                {
                    node.InnerText = _isOtherDirect;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/IsOtherDirect", _isOtherDirect);
                }
            }
        }

        #endregion

        #region 属性_串口参数

        public static string Port_port
        {
            get
            {
                if (_port_port == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_port");
                    if (node != null)
                    {
                        _port_port = node.InnerText;
                    }
                    else
                    {
                        _port_port = "COM1";
                        AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_port", _port_port);
                    }
                }
                return _port_port;
            }
            set
            {
                _port_port = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_port");
                if (node != null)
                {
                    node.InnerText = _port_port;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_port", _port_port);
                }
            }
        }


        public static string Port_bote
        {
            get
            {
                if (_port_bote == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_bote");
                    if (node != null)
                    {
                        _port_bote = node.InnerText;
                    }
                    else
                    {
                        _port_bote = "115200";
                        AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_bote", _port_bote);
                    }
                }
                return _port_bote;
            }
            set
            {
                _port_bote = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_bote");
                if (node != null)
                {
                    node.InnerText = _port_bote;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_bote", _port_bote);
                }
            }
        }


        public static string Port_check
        {
            get
            {
                if (_port_check == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_check");
                    if (node != null)
                    {
                        _port_check = node.InnerText;
                    }
                    else
                    {
                        _port_check = "None";
                        AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_check", _port_check);
                    }
                }
                return _port_check;
            }
            set
            {
                _port_check = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_check");
                if (node != null)
                {
                    node.InnerText = _port_check;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_check", _port_check);
                }
            }
        }


        public static string Port_data
        {
            get
            {
                if (_port_data == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_data");
                    if (node != null)
                    {
                        _port_data = node.InnerText;
                    }
                    else
                    {
                        _port_data = "8";
                        AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_data", _port_data);
                    }
                }
                return _port_data;
            }
            set
            {
                _port_data = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_data");
                if (node != null)
                {
                    node.InnerText = _port_data;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_data", _port_data);
                }
            }
        }


        public static string Port_stop
        {
            get
            {
                if (_port_stop == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_stop");
                    if (node != null)
                    {
                        _port_stop = node.InnerText;
                    }
                    else
                    {
                        _port_stop = "1";
                        AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_stop", _port_stop);
                    }
                }
                return _port_stop;
            }
            set
            {
                _port_stop = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                XmlNode node = xmlDoc.SelectSingleNode("/config/Port/Port_stop");
                if (node != null)
                {
                    node.InnerText = _port_stop;
                    xmlDoc.Save(ConfigFile);
                }
                else
                {
                    AddNodeNoAttrib(ConfigFile, @"/config/Port/Port_stop", _port_stop);
                }
            }
        }

        #endregion

        #region 属性_硬件参数

        public static string CurrentlyMes
        {
            get
            {
                if (_currentlyMes == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/CurrentlyMes");
                    if (node != null)
                    {
                        _currentlyMes = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少CurrentlyMes项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _currentlyMes;
            }
            set
            {
                _currentlyMes = value;
            }
        }

        public static string UpM0
        {
            get
            {
                if (_upM0 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M0");
                    if (node != null)
                    {
                        _upM0 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM0;
            }
            set
            {
                _upM0 = value;
            }
        }

        public static string UpM1
        {
            get
            {
                if (_upM1 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M1");
                    if (node != null)
                    {
                        _upM1 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM1;
            }
            set
            {
                _upM1 = value;
            }
        }

        public static string UpM2
        {
            get
            {
                if (_upM2 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M2");
                    if (node != null)
                    {
                        _upM2 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM2;
            }
            set
            {
                _upM2 = value;
            }
        }

        public static string UpM3
        {
            get
            {
                if (_upM3 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M3");
                    if (node != null)
                    {
                        _upM3 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM3;
            }
            set
            {
                _upM3 = value;
            }
        }

        public static string UpM4
        {
            get
            {
                if (_upM4 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M4");
                    if (node != null)
                    {
                        _upM4 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM4;
            }
            set
            {
                _upM4 = value;
            }
        }

        public static string UpM5
        {
            get
            {
                if (_upM5 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M5");
                    if (node != null)
                    {
                        _upM5 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM5;
            }
            set
            {
                _upM5 = value;
            }
        }

        public static string UpM6
        {
            get
            {
                if (_upM6 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M6");
                    if (node != null)
                    {
                        _upM6 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM6;
            }
            set
            {
                _upM6 = value;
            }
        }

        public static string UpM7
        {
            get
            {
                if (_upM7 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M7");
                    if (node != null)
                    {
                        _upM7 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM7;
            }
            set 
            { 
                _upM7 = value; 
            }
        }

        public static string UpM8
        {
            get
            {
                if (_upM8 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M8");
                    if (node != null)
                    {
                        _upM8 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM8;
            }
            set
            {
                _upM8 = value;
            }
        }

        public static string UpM9
        {
            get
            {
                if (_upM9 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M9");
                    if (node != null)
                    {
                        _upM9 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM9;
            }
            set
            {
                _upM9 = value;
            }
        }

        public static string UpM10
        {
            get
            {
                if (_upM10 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M10");
                    if (node != null)
                    {
                        _upM10 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM10;
            }
            set
            {
                _upM10 = value;
            }
        }

        public static string UpM11
        {
            get
            {
                if (_upM11 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M11");
                    if (node != null)
                    {
                        _upM11 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM11;
            }
            set
            {
                _upM11 = value;
            }
        }

        public static string UpM12
        {
            get
            {
                if (_upM12 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M12");
                    if (node != null)
                    {
                        _upM12 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM12;
            }
            set
            {
                _upM12 = value;
            }
        }

        public static string UpM13
        {
            get
            {
                if (_upM13 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M13");
                    if (node != null)
                    {
                        _upM13 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM13;
            }
            set
            {
                _upM13 = value;
            }
        }

        public static string UpM14
        {
            get
            {
                if (_upM14 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M14");
                    if (node != null)
                    {
                        _upM14 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM14;
            }
            set
            {
                _upM14 = value;
            }
        }

        public static string UpM15
        {
            get
            {
                if (_upM15 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M15");
                    if (node != null)
                    {
                        _upM15 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM15;
            }
            set
            {
                _upM15 = value;
            }
        }

        public static string UpM16
        {
            get
            {
                if (_upM16 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M16");
                    if (node != null)
                    {
                        _upM16 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM16;
            }
            set
            {
                _upM16 = value;
            }
        }

        public static string UpM17
        {
            get
            {
                if (_upM17 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M17");
                    if (node != null)
                    {
                        _upM17 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM17;
            }
            set
            {
                _upM17 = value;
            }
        }

        public static string UpM18
        {
            get
            {
                if (_upM18 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M18");
                    if (node != null)
                    {
                        _upM18 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM18;
            }
            set
            {
                _upM18 = value;
            }
        }

        public static string UpM19
        {
            get
            {
                if (_upM19 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M19");
                    if (node != null)
                    {
                        _upM19 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM19;
            }
            set
            {
                _upM19 = value;
            }
        }

        public static string UpM20
        {
            get
            {
                if (_upM20 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M20");
                    if (node != null)
                    {
                        _upM20 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM20;
            }
            set
            {
                _upM20 = value;
            }
        }

        public static string UpM21
        {
            get
            {
                if (_upM21 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M21");
                    if (node != null)
                    {
                        _upM21 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM21;
            }
            set
            {
                _upM21 = value;
            }
        }

        public static string UpM22
        {
            get
            {
                if (_upM22 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M22");
                    if (node != null)
                    {
                        _upM22 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM22;
            }
            set
            {
                _upM22 = value;
            }
        }

        public static string UpM23
        {
            get
            {
                if (_upM23 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M23");
                    if (node != null)
                    {
                        _upM23 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM23;
            }
            set
            {
                _upM23 = value;
            }
        }

        public static string UpM24
        {
            get
            {
                if (_upM24 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M24");
                    if (node != null)
                    {
                        _upM24 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM24;
            }
            set
            {
                _upM24 = value;
            }
        }

        public static string UpM25
        {
            get
            {
                if (_upM25 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M25");
                    if (node != null)
                    {
                        _upM25 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM25;
            }
            set
            {
                _upM25 = value;
            }
        }

        public static string UpM26
        {
            get
            {
                if (_upM26 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M26");
                    if (node != null)
                    {
                        _upM26 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM26;
            }
            set
            {
                _upM26 = value;
            }
        }

        public static string UpM27
        {
            get
            {
                if (_upM27 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M27");
                    if (node != null)
                    {
                        _upM27 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM27;
            }
            set
            {
                _upM27 = value;
            }
        }

        public static string UpM28
        {
            get
            {
                if (_upM28 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M28");
                    if (node != null)
                    {
                        _upM28 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM28;
            }
            set
            {
                _upM28 = value;
            }
        }

        public static string UpM29
        {
            get
            {
                if (_upM29 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/UpMes/M29");
                    if (node != null)
                    {
                        _upM29 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少MesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _upM29;
            }
            set
            {
                _upM29 = value;
            }
        }

        public static string DownM0
        {
            get
            {
                if (_downM0 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M0");
                    if (node != null)
                    {
                        _downM0 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM0;
            }
            set
            {
                _downM0 = value;
            }
        }

        public static string DownM1
        {
            get
            {
                if (_downM1 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M1");
                    if (node != null)
                    {
                        _downM1 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM1;
            }
            set
            {
                _downM1 = value;
            }
        }

        public static string DownM2
        {
            get
            {
                if (_downM2 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M2");
                    if (node != null)
                    {
                        _downM2 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM2;
            }
            set
            {
                _downM2 = value;
            }
        }

        public static string DownM3
        {
            get
            {
                if (_downM3 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M3");
                    if (node != null)
                    {
                        _downM3 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM3;
            }
            set
            {
                _downM3 = value;
            }
        }

        public static string DownM4
        {
            get
            {
                if (_downM4 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M4");
                    if (node != null)
                    {
                        _downM4 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM4;
            }
            set
            {
                _downM4 = value;
            }
        }

        public static string DownM5
        {
            get
            {
                if (_downM5 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M5");
                    if (node != null)
                    {
                        _downM5 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM5;
            }
            set
            {
                _downM5 = value;
            }
        }

        public static string DownM6
        {
            get
            {
                if (_downM6 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M6");
                    if (node != null)
                    {
                        _downM6 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM6;
            }
            set
            {
                _downM6 = value;
            }
        }

        public static string DownM7
        {
            get
            {
                if (_downM7 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M7");
                    if (node != null)
                    {
                        _downM7 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM7;
            }
            set
            {
                _downM7 = value;
            }
        }
        public static string DownM8
        {
            get
            {
                if (_downM8 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M8");
                    if (node != null)
                    {
                        _downM8 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM8;
            }
            set
            {
                _downM8 = value;
            }
        }
        public static string DownM9
        {
            get
            {
                if (_downM9 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M9");
                    if (node != null)
                    {
                        _downM9 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM9;
            }
            set
            {
                _downM9 = value;
            }
        }
        public static string DownM10
        {
            get
            {
                if (_downM10 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M10");
                    if (node != null)
                    {
                        _downM10 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM10;
            }
            set
            {
                _downM10 = value;
            }
        }
        public static string DownM11
        {
            get
            {
                if (_downM11 == null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigFile_Hard);
                    XmlNode node = xmlDoc.SelectSingleNode("/config/" + Global.CurrentlyMes + "/DownMes/M11");
                    if (node != null)
                    {
                        _downM11 = node.InnerText;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("对不起，配置文件缺少DownMesMesType项，请确保配置文件的正确、完整性。", "硬件配置参数初始化失败");
                        System.Environment.Exit(0);
                    }
                }
                return _downM11;
            }
            set
            {
                _downM11 = value;
            }
        }

        #endregion

        /// <summary>
        /// 递归添加指定的节点
        /// 注：节点无属性，非叶子节点本身没有值
        /// </summary>
        /// <param name="_ConfigFileName"></param>
        /// <param name="_FatherNodePath"></param>
        /// <param name="_NodeName"></param>
        /// <param name="_NodeValue"></param>
        private static void AddNodeNoAttrib(string _ConfigFileName, string _NodeName, string _NodeValue)
        {
            if (File.Exists(_ConfigFileName))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_ConfigFileName);
                string[] tempList = _NodeName.Split('/');
                string _NewNodeName = tempList[tempList.Length - 1];

                string FatherPath = "";
                for (int i = 0; i < tempList.Length - 1; i++)
                {
                    if (tempList[i] != "")
                    {
                        FatherPath += "/" + tempList[i];
                    }
                }
                XmlNode FatherNode = xmlDoc.SelectSingleNode(FatherPath);
                if (FatherNode == null)
                {
                    //递归添加节点
                    AddNodeNoAttrib(_ConfigFileName, FatherPath, "");
                    xmlDoc.Load(_ConfigFileName);
                    FatherNode = xmlDoc.SelectSingleNode(FatherPath);
                }
                XmlElement NewElement = xmlDoc.CreateElement(_NewNodeName);
                NewElement.InnerText = _NodeValue;
                FatherNode.AppendChild(NewElement);
                xmlDoc.Save(_ConfigFileName);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("配置文件：" + _ConfigFileName + "缺失，无法启动程序。请确保完整的配置文件或者重新安装程序");
                System.Environment.Exit(0);
            }
        }

    }
}
