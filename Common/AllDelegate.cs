using System;
using System.Collections.Generic;
using System.Text;

namespace PersonPositionServer.Common
{
    /// <summary>
    /// 硬件层收到消息事件的委托
    /// </summary>
    /// <param name="buffer"></param>
    public delegate void GotMessageEventHandler(byte[] buffer);
    /// <summary>
    /// 基站恢复正常的委托
    /// </summary>
    /// <param name="StationID"></param>
    public delegate void StationNormalEventHandler(int StationID);
}
