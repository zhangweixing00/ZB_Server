using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;

using PersonPositionServer.Common;
using PersonPositionServer.StaticService;

namespace PersonPositionServer.Model
{
    public class Port_Service:HardChannel
    {
        private SerialPort serialPort;
        private string port;
        private int bote;
        private string check;
        private int databit;
        private string stop;
        public override event GotMessageEventHandler Event_GotMessage;//重写父类的抽象事件

        public Port_Service(string _port, int _bote, string _check, int _databit, string _stop)
        {
            this.port = _port;
            this.bote = _bote;
            this.check = _check;
            this.databit = _databit;
            this.stop = _stop;

            listenThread = new Thread(new ThreadStart(Listen));
        }

        /// <summary>
        /// 连接并监听
        /// </summary>
        public override void ConnectAndListen()
        {
            Parity parity;
            StopBits stopbits;
            try
            {
                switch (check)
                {
                    case "None":
                        parity = Parity.None;
                        break;
                    case "Even":
                        parity = Parity.Even;
                        break;
                    case "Mark":
                        parity = Parity.Mark;
                        break;
                    case "Odd":
                        parity = Parity.Odd;
                        break;
                    case "Space":
                        parity = Parity.Space;
                        break;
                    default:
                        parity = Parity.None;
                        break;
                }

                switch (stop)
                {
                    case "1":
                        stopbits = StopBits.One;
                        break;
                    case "1.5":
                        stopbits = StopBits.OnePointFive;
                        break;
                    case "2":
                        stopbits = StopBits.Two;
                        break;
                    default:
                        stopbits = StopBits.One;
                        break;
                }

                serialPort = new SerialPort(port, bote, parity, databit, stopbits);
                serialPort.Open();
                ListenThreadKey = true;
                listenThread.Start();
            }
            catch(Exception ex)
            {
                serialPort.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public override void DisConnect()
        {
            ListenThreadKey = false;
            Thread.Sleep(200);
            try
            {
                serialPort.Close();
            }
            catch
            {  }
        }

        /// <summary>
        /// 监听线程方法
        /// </summary>
        private void Listen()
        {
            while (ListenThreadKey)
            {
                try
                {
                    if (serialPort.IsOpen)
                    {
                        if (serialPort.BytesToRead >= Global.MinBagLength)
                        {
                            if (serialPort.ReadByte() == base.head[0])
                            {
                                if (serialPort.ReadByte() == base.head[1])
                                {
                                    //第3个字节的帧控制字
                                    int control = serialPort.ReadByte();
                                    //第4个字节的有效数据长度
                                    int lengh = serialPort.ReadByte();
                                    int times = 0;
                                    //当缓冲区内的数据不及lenth + 5时，循环等待总计500毫秒的时间等数据来。
                                    while (serialPort.BytesToRead < lengh + 5)
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
                                        serialPort.Read(temp, 4, lengh + 5);
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
                catch(Exception ex)
                {
                    if (Global.IsShowBug)
                        System.Windows.Forms.MessageBox.Show(ex.Message + "\n" + ex.TargetSite + "\n" + ex.StackTrace, "服务器底层串口类解析错误");
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="buffer"></param>
        public override void Send(byte[] buffer)
        {
            serialPort.Write(buffer, 0, buffer.Length);
        }
    }

}
