using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Data;
using System.Diagnostics;

using PersonPositionServer.Common;
using PersonPositionServer.StaticService;

namespace PersonPositionServer.Model
{
    public class TcpSocket_Service:HardChannel
    {
        private Hashtable tcpScoketList;//由于要装Socket对象，所以用哈希表
        private Hashtable netStreamList;//由于要装Netstream对象，所以用哈希表
        public override event GotMessageEventHandler Event_GotMessage;//重写父类的抽象事件
        private Hashtable ReConnecttingList;//当前正在重连基站的线程列表 由于要装Thread对象，所以用哈希表

        public TcpSocket_Service()
        {
            tcpScoketList = new Hashtable();
            netStreamList = new Hashtable();
            ReConnecttingList = new Hashtable();
            listenThread = new Thread(new ThreadStart(Listen));
        }

        public void ReConnectInNewThread(string IP,int Port)
        {
            //当前正在重连列表中没有时，才继续
            if (!ReConnecttingList.ContainsKey(IP))
            {
                Thread newThread = new Thread(new ParameterizedThreadStart(ReConnect));
                ReConnecttingList.Add(IP, newThread);
                newThread.Start(IP + ":" + Port.ToString());
            }
        }

        private void ReConnect(object _iPAndPort)
        {
            string[] templist = _iPAndPort.ToString().Split(':');
            string IP = templist[0];
            int Port = Convert.ToInt32(templist[1]);

            if (tcpScoketList.ContainsKey(IP))
            {
                TcpClient tempClient = (TcpClient)tcpScoketList[IP];
                tcpScoketList.Remove(IP);
                try
                {
                    tempClient.Close();
                    tempClient = null;
                }
                catch
                { }
            }
            if (netStreamList.ContainsKey(IP))
            {
                NetworkStream tempStream = (NetworkStream)netStreamList[IP];
                netStreamList.Remove(IP);
                try
                {
                    tempStream.Close();
                    tempStream = null;
                }
                catch
                { }
            }
            bool key = true;
            while (key)
            {
                Thread.Sleep(30000);//30秒后自动重连
                if (CmdPing(IP,3000))
                {
                    try
                    {
                        TcpClient tcpClient = new TcpClient();
                        tcpClient.Connect(IP, Port);
                        NetworkStream netStream = tcpClient.GetStream();
                        tcpScoketList.Add(IP, tcpClient);
                        netStreamList.Add(IP, netStream);
                        key = false;
                    }
                    catch
                    {   }
                }
            }
            ReConnecttingList.Remove(IP);
        }

        public override void ConnectAndListen()
        {
            DataRow[] rows = BasicData.GetStationTableRows("StationType = '网关基站'",true);
            Dictionary<string, string> IPList = new Dictionary<string, string>();//控制不同基站若IP相同，则只建立其中一个IP的连接的字典
            for (int i = 0; i < rows.Length; i++)
            {
                try
                {
                    string IP = rows[i]["IP"].ToString();
                    //若不同基站用同一IP，则只建立其中一个IP的连接
                    if (!IPList.ContainsKey(IP))
                    {
                        if (CmdPing(IP,3000))
                        {
                            TcpClient tcp = new TcpClient();
                            tcp.Connect(rows[i]["IP"].ToString(), Convert.ToInt32(rows[i]["Port"]));
                            NetworkStream stream = tcp.GetStream();
                            tcpScoketList.Add(IP, tcp);
                            netStreamList.Add(IP, stream);
                            IPList.Add(IP, IP);
                        }
                    }
                }
                catch(Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("df"+ex.Message);
                }
            }
            //启动监听线程
            ListenThreadKey = true;
            listenThread.Start();
        }

         

        public static string CmdPing(string IP)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine("ping -n 1 " + IP);
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            string pingrst;
            if ((strRst.IndexOf("Destination host unreachable") == -1) && (strRst.IndexOf("目标主机无法访问") == -1))
            {

                pingrst = "连接";

            }

            else
                pingrst = "无法到达目的主机";
            p.Close();
            //System.Windows.Forms.MessageBox.Show(pingrst);
            return pingrst;
        }
        public static bool CmdPing(string IP, int timeout)
        {
            pingclass.pingtimeout = timeout;
            return pingclass.PingHost(IP);
        }
        public override void DisConnect()
        {
            try
            {
                foreach (object o in tcpScoketList.Values)
                {
                    TcpClient tcp = (TcpClient)o;
                    tcp.Close();
                }
            }
            catch
            { }
            tcpScoketList.Clear();
            netStreamList.Clear();
            try
            {
                ListenThreadKey = false;
            }
            catch
            { }
            try
            {
                foreach (object o in ReConnecttingList.Values)
                {
                    Thread thread = (Thread)o;
                    thread.Abort();
                }
            }
            catch
            { }
            ReConnecttingList.Clear();
        }

        private void Listen()
        {
            while (ListenThreadKey)
            {
                try
                {
                    
                    
                    foreach (object o in tcpScoketList.Keys)
                    {
                        string IP = o.ToString();
                        TcpClient tcpScoket = (TcpClient)tcpScoketList[IP];
                        NetworkStream netStream = (NetworkStream)netStreamList[IP];
                        if (tcpScoket != null && netStream != null)
                        {
                            if (netStream.CanRead & netStream.DataAvailable)
                            {
                                if (tcpScoket.Available >= Global.MinBagLength)
                                {
                                    if (netStream.ReadByte() == base.head[0])
                                    {
                                        if (netStream.ReadByte() == base.head[1])
                                        {
                                            //第3个字节的帧控制字
                                            int control = netStream.ReadByte();
                                            //第4个字节的有效数据长度
                                            int lengh = netStream.ReadByte();
                                            int times = 0;
                                            //当缓冲区内的数据不及lenth + 5时，循环等待总计500毫秒的时间等数据来。
                                            while (tcpScoket.Available < lengh + 5)
                                            {
                                                Thread.Sleep(25);
                                                times++;
                                                if (times == 20)
                                                    break;
                                            }
                                            if (times < 20)
                                            {
                                                //500毫秒之内来了，则处理数据
                                                byte[] temp = new byte[lengh + Global.MinBagLength];
                                                temp[0] = base.head[0];
                                                temp[1] = base.head[1];
                                                temp[2] = Convert.ToByte(control);
                                                temp[3] = Convert.ToByte(lengh);
                                                netStream.Read(temp, 4, lengh + 5);
                                                //使用多播委托异步抛出收到消息的事件
                                                if (Event_GotMessage != null)
                                                {
                                                    Delegate[] delegateList = Event_GotMessage.GetInvocationList();
                                                    foreach (GotMessageEventHandler GMEH in delegateList)
                                                    {
                                                        GMEH.BeginInvoke(temp, null, null);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    if (Global.IsShowBug)
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器底层网口类监听错误");
                }
                Thread.Sleep(1);
            }
        }

        public override void Send(byte[] buffer)
        {
            int AimStationID;
            if(Global.Isnew )
                AimStationID = (buffer[4] << 8) + buffer[5];
            else
                AimStationID = (buffer[2] << 8) + buffer[3];
            try
            {
                DataRow row = BasicData.GetStationTableRows("ID = " + AimStationID, true)[0];
                NetworkStream netStream = (NetworkStream)netStreamList[row["IP"].ToString()];
                if (Convert.ToString(row["StationType"]) == "无线基站")
                {
                    Int16 father = Convert.ToInt16(row["FatherStationID"]);
                    row = BasicData.GetStationTableRows("ID = " + father, true)[0];
                    netStream = (NetworkStream)netStreamList[row["IP"].ToString()];

                }
                // NetworkStream netStream = (NetworkStream)netStreamList[row["IP"].ToString()];
                if (netStream != null)
                {
                    netStream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                if (Global.IsShowBug)
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器底层网口发送数据错误");
            }
        }
    }
}
