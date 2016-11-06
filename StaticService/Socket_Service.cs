using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using PersonPositionServer.Common;
using PersonPositionServer.View;

namespace PersonPositionServer.StaticService
{
    public static class Socket_Service
    {
        //服务器监听主套接字
        private static TcpClient serverSocket;
        //监听线程
        private static Thread Thread_Listenner;
        //客户端列表
        private static ArrayList ClientList = new ArrayList();
        //命令头长度
        public const int CommandHeadLength = 6;
        //Socket缓冲区大小
        private const int BufferSize = 32768;
        //记录用户对应的心跳 健为用户名，值为心跳值 超过一分钟（60）即认为客户端与服务端连接已断开
        public static Hashtable htusertick = new Hashtable();
        //记录用户对应的socket
        public static Hashtable htusersocket = new Hashtable();
        //服务器命令
        public const string Command_S2C_ShutDown = "S2C_SD";
        public const string Command_S2C_LowPower = "S2C_LP";
        public const string Command_S2C_UpMessage = "S2C_UM";
        public const string Command_S2C_DownMesType = "S2C_MT";
        public const string Command_S2C_InArea = "S2C_IA";
        public const string Command_S2C_UpdatePosition = "S2C_UP";
        public const string Command_S2C_UpdateDB = "S2C_UD";
        public const string Command_S2C_UpdateCollectChannel = "S2C_UC";
        //客户端命令
        public const string Command_C2S_Reg = "C2S_RE";
        public const string Command_C2S_UnReg = "C2S_UR";
        public const string Command_C2S_AddRelation = "C2S_AR";
        public const string Command_C2S_DelRelation = "C2S_DR";
        public const string Command_C2S_DownMessage = "C2S_DM";
        public const string Command_C2S_RequestDownMesType = "C2S_MT";
        public const string Command_C2S_RequestInArea = "C2S_IA";
        public const string Command_C2S_LightUp = "C2S_LU";
        public const string Command_C2S_UpdateDB = "C2S_UD";
        public const string Command_C2S_AreaSubject = "C2S_AS";
        public const string Command_C2S_HandCheckOut = "C2S_HC";
        public const string Command_C2S_SetInfo = "C2S_SI";
        public const string Command_C2S_ConnTick = "C2S_CT";
        //客户端命令的返回
        public const string RES_S2C_Reg = "RES_RE";
        public const string RES_S2C_LightUp = "RES_LU";
        public const string RES_S2C_AreaSubject = "RES_AS";
        public const string RES_S2C_HandCheckOut = "RES_HC";

        private static MainForm mainform;

        public static void GiveMainfrom(MainForm _mainform)
        {
            Socket_Service.mainform = _mainform;
        }

        /// <summary>
        /// 开始监听客户端新连接请求线程
        /// </summary>
        /// <param name="port"></param>
        public static void StartListenConnectOfClient(int port)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            serverSocket = new TcpClient(endPoint);
            Thread_Listenner = new Thread(new ThreadStart(ListenThread));
            Thread_Listenner.Start();
        }
        /// <summary>
        /// 断开指定用户的连接
        /// </summary>
        /// <param name="users"></param>
        public static void DisConnectClient(string username)
        {
            try
            {
                Socket clientsock = (Socket)htusersocket[username];
                System.Net.IPEndPoint endPointclient = (System.Net.IPEndPoint)clientsock.RemoteEndPoint;
                string ip = endPointclient.Address.ToString();
                int port = endPointclient.Port;
                for (int j = 0; j < ClientList.Count; j++)
                {
                    System.Net.Sockets.Socket socket = (System.Net.Sockets.Socket)ClientList[j];
                    if (socket != null)
                    {
                        System.Net.IPEndPoint endPoint = (System.Net.IPEndPoint)socket.RemoteEndPoint;
                        //在clientlist中多个socket的IP地址可能是一样的，即一台电脑开两个客户端，因此需要比较IP地址和端口
                        if (ip == endPoint.Address.ToString() && endPoint.Port == port)
                        {
                            ClientList.Remove(socket);
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();
                            break;
                        }
                    }
                }
                //断开该连接
                clientsock.Close();
                //在hashusersocket和hashclienttick中清除该用户
                lock (htusersocket.SyncRoot)
                {
                    if (htusersocket.ContainsKey(username))
                    {
                        htusersocket.Remove(username);
                    }
                }
                lock (htusertick.SyncRoot)
                {
                    if (htusertick.Contains(username))
                    {
                        htusertick.Remove(username);
                    }
                }

            }
            catch (Exception ex)
            {
                if (Global.IsShowBug)
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "强制断开客户端连接错误");
            }

        }
        /// <summary>
        /// 停止监听客户端新连接请求线程
        /// 注：只是停止线程和服务器监听套接字，但是所有客户端套接字还保留
        /// </summary>
        public static void StopListenConnectOfClient()
        {
            try
            {
                serverSocket.Client.Close();
                serverSocket.Close();
            }
            catch
            { }
            try
            {
                Thread_Listenner.Abort();
            }
            catch
            { }
        }

        /// <summary>
        /// 关闭所有客户端套接字
        /// </summary>
        public static void CloseAllClientSocket()
        {
            for (int i = 0; i < ClientList.Count; i++)
            {
                try
                {
                    Socket ClientSocket = (Socket)ClientList[i];
                    ClientSocket.Close();
                }
                catch
                { }
            }
            ClientList.Clear();
        }

        private static void ListenThread()
        {
            serverSocket.Client.Listen(50);
            while (true)
            {
                try
                {
                    Socket TempClient = serverSocket.Client.Accept();
                    TempClient.SendBufferSize = BufferSize;
                    lock (ClientList.SyncRoot)
                    {
                        ClientList.Add(TempClient);
                    }
                    Thread AnalysisDataThread = new Thread(new ParameterizedThreadStart(AnalysisData));
                    AnalysisDataThread.Start(TempClient);
                }
                catch
                { }
            }
        }

        public  static void SendData(Byte [] buf)
        {

            Socket_Service.mainform.hardChannel.Send(buf) ;
        
        
        }


        /// <summary>
        /// 客户端消息分析
        /// </summary>
        /// <param name="socket"></param>
        private static void AnalysisData(object socket)
        {
            try
            {
                Socket clientSocket = (Socket)socket;
                bool key = true;
                while (key)
                {
                    byte[] buffer = new byte[BufferSize];
                    int bytes = clientSocket.Receive(buffer);
                    if (bytes == 0)
                    {
                        continue;
                    }
                    try
                    {
                        string[] TempStrs = Encoding.Unicode.GetString(buffer, 0, bytes).Split('|');
                        string Command = "";
                        string Parameter1 = "";
                        string Parameter2 = "";
                        string Parameter3 = "";
                        string Parameter4 = "";
                        string Parameter5 = "";
                        string Parameter6 = "";
                        string Parameter7 = "";
                        string Parameter8 = "";
                        string Parameter9 = "";

                        //解析出命令字、命令参数
                        switch (TempStrs.Length)
                        {
                            case 1:
                                Command = TempStrs[0];
                                break;
                            case 2:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                break;
                            case 3:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                Parameter2 = TempStrs[2];
                                break;
                            case 4:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                Parameter2 = TempStrs[2];
                                Parameter3 = TempStrs[3];
                                break;
                            case 5:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                Parameter2 = TempStrs[2];
                                Parameter3 = TempStrs[3];
                                Parameter4 = TempStrs[4];
                                break;
                            case 6:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                Parameter2 = TempStrs[2];
                                Parameter3 = TempStrs[3];
                                Parameter4 = TempStrs[4];
                                Parameter5 = TempStrs[5];
                                break;
                            case 7:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                Parameter2 = TempStrs[2];
                                Parameter3 = TempStrs[3];
                                Parameter4 = TempStrs[4];
                                Parameter5 = TempStrs[5];
                                Parameter6 = TempStrs[6];
                                break;
                            case 8:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                Parameter2 = TempStrs[2];
                                Parameter3 = TempStrs[3];
                                Parameter4 = TempStrs[4];
                                Parameter5 = TempStrs[5];
                                Parameter6 = TempStrs[6];
                                Parameter7 = TempStrs[7];
                                break;
                            case 9:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                Parameter2 = TempStrs[2];
                                Parameter3 = TempStrs[3];
                                Parameter4 = TempStrs[4];
                                Parameter5 = TempStrs[5];
                                Parameter6 = TempStrs[6];
                                Parameter7 = TempStrs[7];
                                Parameter8 = TempStrs[8];
                                break;
                            case 10:
                                Command = TempStrs[0];
                                Parameter1 = TempStrs[1];
                                Parameter2 = TempStrs[2];
                                Parameter3 = TempStrs[3];
                                Parameter4 = TempStrs[4];
                                Parameter5 = TempStrs[5];
                                Parameter6 = TempStrs[6];
                                Parameter7 = TempStrs[7];
                                Parameter8 = TempStrs[8];
                                Parameter9 = TempStrs[9];
                                break;
                        }

                        switch (Command)
                        {
                            case Command_C2S_Reg:
                                switch (mainform.CheckLogName(Parameter1))
                                {
                                    case 0:
                                        mainform.AddClientList(Parameter1, ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString(), ((IPEndPoint)clientSocket.RemoteEndPoint).Port, DateTime.Now.ToString());
                                        //返回登录成功
                                        clientSocket.Send(Encoding.Unicode.GetBytes(RES_S2C_Reg + "|0|" + DateTime.Now.ToString() + "|" + Global.IsUseHongWai.ToString() + "|" + Global.IsTempVersion.ToString()));
                                        try
                                        {
                                            htusertick.Add(Parameter1, 0);
                                            //记录用户对应的socket
                                            htusersocket.Add(Parameter1, clientSocket);
                                        }
                                        catch (Exception ex)
                                        {
                                            System.Windows.Forms.MessageBox.Show(ex.Message);
                                        }
                                        break;
                                    case 1:
                                        //返回登录失败：人数达到最大限额
                                        clientSocket.Send(Encoding.Unicode.GetBytes(RES_S2C_Reg + "|1"));
                                        break;
                                    case 2:
                                        //返回登录失败：用户已经登录
                                        object[] keyarray = new Object[htusersocket.Keys.Count];
                                        Socket_Service.htusertick.Keys.CopyTo(keyarray, 0);
                                        string clientip = string.Empty;
                                        foreach (object obj in keyarray)
                                        {
                                            if (obj.ToString() == Parameter1)
                                            {
                                                Socket sc = (Socket)htusersocket[obj];
                                                if (sc.Connected)//该账户的客户端连接正常
                                                {
                                                    IPEndPoint endpoint = (IPEndPoint)sc.RemoteEndPoint;
                                                    clientip = endpoint.Address.ToString();
                                                }
                                                else
                                                {
                                                    clientip = "该账户连接正在断开请稍后";
                                                }
                                            }
                                        }
                                        clientSocket.Send(Encoding.Unicode.GetBytes(RES_S2C_Reg + "|2" + "|" + clientip));
                                        break;
                                }
                                break;
                            case Command_C2S_ConnTick:
                                //收到心跳消息则复位该用户心跳值为0
                                if (htusertick.Contains(Parameter1))
                                {
                                    htusertick[Parameter1] = 0;
                                    System.Net.Sockets.Socket tempsocket = (System.Net.Sockets.Socket)socket;
                                    System.Net.IPEndPoint tempendpoint = (System.Net.IPEndPoint)tempsocket.RemoteEndPoint;
                                    Console.WriteLine(tempendpoint.Address.ToString() + "--" + tempendpoint.Port.ToString() + "清0");
                                }
                                Console.WriteLine("清0");
                                break;
                            case Command_C2S_UnReg:
                                key = false;
                                mainform.RefreshClientList(ClientList);
                                lock (htusersocket)
                                {
                                    if (htusersocket.Contains(Parameter1))
                                    {
                                        htusersocket.Remove(Parameter1);
                                    }
                                }
                                lock (htusertick)
                                {
                                    if (htusertick.Contains(Parameter1))
                                    {
                                        htusertick.Remove(Parameter1);
                                    }
                                }
                                break;
                            case Command_C2S_AddRelation:
                                int father = Convert.ToInt32(Parameter1);
                                int son = Convert.ToInt32(Parameter2);
                                //发送设置子基站命令
                                Socket_Service.mainform.hardChannel.Send(CommandFactory.GetAddSonCommand(father, son));
                                break;
                            case Command_C2S_DelRelation:
                                int fatherDelSon = Convert.ToInt32(Parameter1);
                                int sonDelSon = Convert.ToInt32(Parameter2);
                                //发送删除子基站命令
                                Socket_Service.mainform.hardChannel.Send(CommandFactory.GetDelSonCommand(fatherDelSon, sonDelSon));
                                break;
                            case Command_C2S_DownMessage:
                                int cardID = Convert.ToInt32(Parameter1);
                                int messageType = Convert.ToInt32(Parameter2);
                                //根据连接方式发送短信
                                if (mainform.hardChannel.GetType() == Type.GetType("PersonPositionServer.Model.TcpSocket_Service"))
                                {
                                    //网口：给所有网关基站一个一个的发
                                    System.Data.DataRow[] rows = BasicData.GetStationTableRows("StationType = '网关基站'",true);
                                    for (int i = 0; i < rows.Length; i++)
                                    {
                                        try
                                        {
                                            int stationID = Convert.ToInt32(rows[i]["ID"]);
                                            Socket_Service.mainform.hardChannel.Send(CommandFactory.GetDownMessageCommand(stationID, cardID, messageType));
                                        }
                                        catch(Exception ex)
                                        {
                                            if (Global.IsShowBug)
                                                System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器下发短信错误");
                                        }
                                    }
                                }
                                else
                                {
                                    //串口：给Can总线群发
                                    Socket_Service.mainform.hardChannel.Send(CommandFactory.GetDownMessageCommand(1023, cardID, messageType));//这里的1023是0x:03FF 是Can总线中的广播地址
                                }
                                break;
                            case Command_C2S_RequestDownMesType:
                                //给客户端发送下行短信类型
                                clientSocket.Send(Encoding.Unicode.GetBytes(Command_S2C_DownMesType + "|" + Global.DownM0 + "=" + Global.DownM1 + "=" + Global.DownM2 + "=" + Global.DownM3 + "=" + Global.DownM4 + "=" + Global.DownM5 + "=" + Global.DownM6 + "=" + Global.DownM7 + "=" + Global.DownM8 + "=" + Global.DownM9 + "=" + Global.DownM10 + "=" + Global.DownM11));
                                break;
                            case Command_C2S_RequestInArea:
                                //给客户端发送特殊区域内人员
                                clientSocket.Send(Encoding.Unicode.GetBytes(Command_S2C_InArea + "|" + Protocol_Service.InArea.Count.ToString() + "|" + Protocol_Service.GetPresentInAreaStr()));
                                break;
                            case Command_C2S_LightUp:
                                //发送点亮灯命令
                                if (Socket_Service.mainform.hardChannel.Send_Safe(Protocol_Service.CommandType.LightUp, CommandFactory.GetLightUpCommand(Convert.ToInt32(Parameter1), Convert.ToInt32(Parameter2))))
                                {
                                    //返回点亮灯成功
                                    clientSocket.Send(Encoding.Unicode.GetBytes(RES_S2C_LightUp));
                                }
                                break;
                            case Command_C2S_HandCheckOut:
                                //强制离开指定的员工
                                if (Protocol_Service.HandCheckOut(Convert.ToInt32(Parameter1)))
                                {
                                    //返回强制离开成功
                                    clientSocket.Send(Encoding.Unicode.GetBytes(RES_S2C_HandCheckOut)); 
                                }
                                break;
                            case Command_C2S_UpdateDB:
                                Dictionary<string, string> ParameterList = new Dictionary<string, string>();
                                ParameterList.Add(Parameter1, Parameter1);
                                if (Parameter2 != "")
                                {
                                    ParameterList.Add(Parameter2, Parameter2);
                                    if (Parameter3 != "")
                                    {
                                        ParameterList.Add(Parameter3, Parameter3);
                                        if (Parameter4 != "")
                                        {
                                            ParameterList.Add(Parameter4, Parameter4);
                                            if (Parameter5 != "")
                                            {
                                                ParameterList.Add(Parameter5, Parameter5);
                                                if (Parameter6 != "")
                                                {
                                                    ParameterList.Add(Parameter6, Parameter6);
                                                    if (Parameter7 != "")
                                                    {
                                                        ParameterList.Add(Parameter7, Parameter7);
                                                        if (Parameter8 != "")
                                                        {
                                                            ParameterList.Add(Parameter8, Parameter8);
                                                            if (Parameter9 != "")
                                                            {
                                                                ParameterList.Add(Parameter9, Parameter9);

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (ParameterList.ContainsKey("CardTable"))
                                    BasicData.RefreshCardTable();
                                if (ParameterList.ContainsKey("CollectChannelTable"))
                                    BasicData.RefreshCollectChannelTable();
                                if (ParameterList.ContainsKey("PersonTable"))
                                    BasicData.RefreshPersonTable();
                                if (ParameterList.ContainsKey("SpecalTable"))
                                    BasicData.RefreshSpecalTable();
                                if (ParameterList.ContainsKey("StationTable"))
                                    BasicData.RefreshStationTable();
                                //给除这个客户端以外的所有客户端发送数据库更新消息
                                BroadcastMessageWithOutOne(clientSocket,Socket_Service.Command_S2C_UpdateDB, Parameter1, Parameter2, Parameter3, Parameter4, Parameter5, Parameter6, Parameter7, Parameter8, Parameter9);
                                break;
                            case Command_C2S_AreaSubject:
                                //返回命令成功
                                clientSocket.Send(Encoding.Unicode.GetBytes(RES_S2C_AreaSubject));
                                switch (Parameter1)
                                {
                                    case "ON":
                                        Protocol_Service.AlarmAreaName = Parameter2;
                                        break;
                                    case "OFF":
                                        Protocol_Service.AlarmAreaName = "未启动";
                                        Protocol_Service.InArea.Clear();
                                        Protocol_Service.IsExceedInArea = false;
                                        break;
                                    case "CHANGE":
                                        Protocol_Service.AlarmAreaName = Parameter2;
                                        break;
                                }
                                break;
                            case Command_C2S_SetInfo:
                                Socket_Service.mainform.hardChannel.Send(CommandFactory.GetSetInfoCommand(Convert.ToInt32(Parameter1), Convert.ToByte(Parameter2), Convert.ToByte(Parameter3), Parameter4));
                                break;
                        }
                    }
                    catch(Exception ex)
                    {
                        if (Global.IsShowBug)
                            System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器解析客户端命令错误");
                        //在解析命令时如果有错，则忽略
                    }
                }
                ClientList.Remove(socket);
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                mainform.RefreshClientList(ClientList);
            }
            catch
            {
                ClientList.Remove(socket);

                //在htlcientsock和htusertick中清除该socket
                object[] keyarray = new Object[htusersocket.Keys.Count];
                Socket_Service.htusertick.Keys.CopyTo(keyarray, 0);

                System.Net.Sockets.Socket csocket = (System.Net.Sockets.Socket)socket;
                if (csocket.Connected && csocket != null)//远程主机未强制关闭
                {
                    System.Net.IPEndPoint cendpoint = (System.Net.IPEndPoint)csocket.RemoteEndPoint;
                    foreach (object obj in keyarray)
                    {
                        System.Net.Sockets.Socket htsocket = (System.Net.Sockets.Socket)htusersocket[obj];
                        System.Net.IPEndPoint htendPoint = (System.Net.IPEndPoint)htsocket.RemoteEndPoint;

                        if ((htendPoint.Address.ToString() == cendpoint.Address.ToString()) && (htendPoint.Port == cendpoint.Port))
                        {
                            //先关闭连接再移除
                            htsocket.Shutdown(SocketShutdown.Both);
                            htsocket.Close();
                            htusersocket.Remove(obj);
                            htusertick.Remove(obj);
                        }
                    }
                }
                else//远程主机已强制关闭
                {
                    foreach (object obj in keyarray)
                    {
                        System.Net.Sockets.Socket htsocket = (System.Net.Sockets.Socket)htusersocket[obj];
                        if (!htsocket.Connected && htsocket != null)
                        {
                            htsocket.Shutdown(SocketShutdown.Both);
                            htsocket.Close();
                            htusersocket.Remove(obj);
                            htusertick.Remove(obj);
                        }
                    }
                }
                mainform.RefreshClientList(ClientList);
            }
        }

        /// <summary>
        /// 给当前已经连接的所有客户端发送消息
        /// </summary>
        public static void BroadcastMessage(string Command, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9)
        {
            try
            {
                foreach (Socket soc in ClientList)
                {
                    soc.Send(Encoding.Unicode.GetBytes(Command + "|" + Parameter1 + "|" + Parameter2 + "|" + Parameter3 + "|" + Parameter4 + "|" + Parameter5 + "|" + Parameter6 + "|" + Parameter7 + "|" + Parameter8 + "|" + Parameter9));
                }
            }
            catch(Exception ex)
            {
                if (Global.IsShowBug)
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器套接字类 BroadcastMessage 方法错误");
            }
        }

        /// <summary>
        /// 给除指定客户端以外的所有客户端发送消息
        /// </summary>
        public static void BroadcastMessageWithOutOne(Socket WithOutOne, string Command, string Parameter1, string Parameter2, string Parameter3, string Parameter4, string Parameter5, string Parameter6, string Parameter7, string Parameter8, string Parameter9)
        {
            try
            {
                foreach (Socket soc in ClientList)
                {
                    if (soc != WithOutOne)
                        soc.Send(Encoding.Unicode.GetBytes(Command + "|" + Parameter1 + "|" + Parameter2 + "|" + Parameter3 + "|" + Parameter4 + "|" + Parameter5 + "|" + Parameter6 + "|" + Parameter7 + "|" + Parameter8 + "|" + Parameter9));
                }
            }
            catch (Exception ex)
            {
                if (Global.IsShowBug)
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器套接字类 BroadcastMessageWithOutOne 方法错误");
            }
        }

    }
}
