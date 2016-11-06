using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using PersonPositionServer.Common;
using PersonPositionServer.StaticService;
using PersonPositionServer.View;

namespace PersonPositionServer.Model
{
    public abstract class HardChannel
    {
        public Thread listenThread;//监听线程
        public bool ListenThreadKey = false;//监听线程开关变量
        public abstract event GotMessageEventHandler Event_GotMessage;//抽象事件
        public abstract void ConnectAndListen();
        public abstract void DisConnect();
        public abstract void Send(byte[] buffer);
        protected byte[] head = new byte[2] { 0xAA, 0x55 };//包头字节数组

        /// <summary>
        /// 安全的发送消息，发送时停止巡检，直到返回或者超时
        /// 阻塞发送线程直到收到返回的消息或者达到超时。超时：50毫秒×40=2秒
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool Send_Safe(Protocol_Service.CommandType commandType, byte[] buffer)
        {
            switch (commandType)
            {
                //添加子基站命令
                case Protocol_Service.CommandType.AddSon:
                    //暂停巡检
                    MainForm.LoopKey = false;
                    Global.Result_AddSon = false;
                    this.Send(buffer);
                    int tempNumAddSon = 0;
                    //循环等待命令的成功返回
                    while (!Global.Result_AddSon)
                    {
                        Thread.Sleep(50);
                        tempNumAddSon++;
                        if (tempNumAddSon >= 40)
                        {
                            //超时，则把开关变量继续置为False后返回失败
                            Global.Result_AddSon = false;
                            //继续巡检
                            MainForm.LoopKey = true;
                            return false;
                        }
                    }
                    //至此，说明成功收到返回，则把开关变量继续置为False后返回成功
                    Global.Result_AddSon = false;
                    //继续巡检
                    MainForm.LoopKey = true;
                    break;
                //删除子基站命令
                case Protocol_Service.CommandType.DelSon:
                    //暂停巡检
                    MainForm.LoopKey = false;
                    Global.Result_DelSon = false;
                    this.Send(buffer);
                    int tempNumDelSon = 0;
                    //循环等待命令的成功返回
                    while (!Global.Result_DelSon)
                    {
                        Thread.Sleep(50);
                        tempNumDelSon++;
                        if (tempNumDelSon >= 40)
                        {
                            //超时，则把开关变量继续置为False后返回失败
                            Global.Result_DelSon = false;
                            //继续巡检
                            MainForm.LoopKey = true;
                            return false;
                        }
                    }
                    //至此，说明成功收到返回，则把开关变量继续置为False后返回成功
                    Global.Result_DelSon = false;
                    //继续巡检
                    MainForm.LoopKey = true;
                    break;
                //设置时间命令
                case Protocol_Service.CommandType.SetTime:
                    //暂停巡检
                    MainForm.LoopKey = false;
                    Global.Result_SetTime = false;
                    this.Send(buffer);
                    int tempNumSetTime = 0;
                    //循环等待命令的成功返回
                    while (!Global.Result_SetTime)
                    {
                        Thread.Sleep(50);
                        tempNumSetTime++;
                        if (tempNumSetTime >= 40)
                        {
                            //超时，则把开关变量继续置为False后返回失败
                            Global.Result_SetTime = false;
                            //继续巡检
                            MainForm.LoopKey = true;
                            return false;
                        }
                    }
                    //至此，说明成功收到返回，则把开关变量继续置为False后返回成功
                    Global.Result_SetTime = false;
                    //继续巡检
                    MainForm.LoopKey = true;
                    break;
                //点亮灯命令
                case Protocol_Service.CommandType.LightUp:
                    //暂停巡检
                    MainForm.LoopKey = false;
                    Global.Result_LightUp = false;
                    this.Send(buffer);
                    int tempNumLightUp = 0;
                    //循环等待命令的成功返回
                    while (!Global.Result_LightUp)
                    {
                        Thread.Sleep(50);
                        tempNumLightUp++;
                        if (tempNumLightUp >= 40)
                        {
                            //超时，则把开关变量继续置为False后返回失败
                            Global.Result_LightUp = false;
                            //继续巡检
                            MainForm.LoopKey = true;
                            return false;
                        }
                    }
                    //至此，说明成功收到返回，则把开关变量继续置为False后返回成功
                    Global.Result_LightUp = false;
                    //继续巡检
                    MainForm.LoopKey = true;
                    break;
            }
            return true;
        }
    }
}
