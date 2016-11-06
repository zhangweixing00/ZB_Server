using System;
using System.Collections.Generic;
using System.Text;

namespace PersonPositionServer.Common
{
    public static class pingclass
    {
        const int SOCKET_ERROR = -1;
        const int ICMP_ECHO = 8;
        public static int pingtimeout = 5000;
        public static string errorstring = string.Empty;
        public class IcmpPacket
        {
            public Byte Type;// type of message
            public Byte SubCode;// type of sub code
            public UInt16 CheckSum;// ones complement checksum of struct
            public UInt16 Identifier; // identifier
            public UInt16 SequenceNumber; // sequence number
            public Byte[] Data;
        }

        public static bool PingHost(string host)
        {
            //Declare the IPHostEntry

            System.Net.IPHostEntry serverHE, fromHE;
            int nBytes = 0;
            int dwStart = 0, dwStop = 0;
            //Initilize a Socket of the Type ICMP
            System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork,
            System.Net.Sockets.SocketType.Raw, System.Net.Sockets.ProtocolType.Icmp);
            // Get the server endpoint
            try
            {
                //serverHE = System.Net.Dns.GetHostByName(host);
                serverHE = System.Net.Dns.GetHostEntry(host);
            }
            catch (Exception)
            {
                errorstring = "未找到主机";
                //Console.WriteLine("Host not found"); // fail
                return false;
            }
            // Convert the server IP_EndPoint to an 
            //     EndPoint
            System.Net.IPAddress ipbind = null;
            System.Net.IPEndPoint ipepServer = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(host), 0);
            System.Net.EndPoint epServer = (ipepServer);
            // Set the receiving endpoint to the cli
            //     ent machine
            //fromHE = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName());
            fromHE = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (System.Net.IPAddress ip in fromHE.AddressList)
            {
                if (System.Net.Sockets.AddressFamily.InterNetwork.Equals(ip.AddressFamily))
                {
                    ipbind = ip;
                    break;
                }
            }
            System.Net.IPEndPoint ipEndPointFrom = new System.Net.IPEndPoint(ipbind, 0);
            System.Net.EndPoint EndPointFrom = (ipEndPointFrom);
            int PacketSize = 0;
            IcmpPacket packet = new IcmpPacket();

            // Construct the packet to send
            packet.Type = ICMP_ECHO; //8
            packet.SubCode = 0;
            packet.CheckSum = UInt16.Parse("0");
            packet.Identifier = UInt16.Parse("45");
            packet.SequenceNumber = UInt16.Parse("0");
            int PingData = 32; // sizeof(IcmpPacket) - 8;
            packet.Data = new Byte[PingData];
            //Initilize the Packet.Data
            for (int i = 0; i < PingData; i++)
            {
                packet.Data[i] = (byte)'#';
            }
            //Variable to hold the total Packet size
            //     
            PacketSize = PingData + 8;
            Byte[] icmp_pkt_buffer = new Byte[PacketSize];
            Int32 Index = 0;
            //Call a Methos Serialize which counts
            //The total number of Bytes in the Packe
            //     t
            Index = Serialize(
            packet,
            icmp_pkt_buffer,
            PacketSize,
            PingData);
            //Error in Packet Size
            if (Index == -1)
            {
                Console.WriteLine("Error in Making Packet");
                errorstring = "创建封包错误";
                return false;
            }
            // now get this critter into a UInt16 ar
            //     ray
            //Get the Half size of the Packet
            Double double_length = Convert.ToDouble(Index);
            Double dtemp = System.Math.Ceiling(double_length / 2);
            int cksum_buffer_length = Convert.ToInt32(dtemp);
            //Create a Byte Array
            UInt16[] cksum_buffer = new UInt16[cksum_buffer_length];
            //Code to initilize the Uint16 array
            int icmp_header_buffer_index = 0;
            for (int i = 0; i < cksum_buffer_length; i++)
            {
                cksum_buffer[i] =
                BitConverter.ToUInt16(icmp_pkt_buffer, icmp_header_buffer_index);
                icmp_header_buffer_index += 2;
            }
            //Call a method which will return a chec
            //     ksum
            UInt16 u_cksum = checksum(cksum_buffer, cksum_buffer_length);
            //Save the checksum to the Packet
            packet.CheckSum = u_cksum;
            // Now that we have the checksum, serial
            //     ize the packet again
            Byte[] sendbuf = new Byte[PacketSize];
            //again check the packet size
            Index = Serialize(
            packet,
            sendbuf,
            PacketSize,
            PingData);
            //if there is a error report it
            if (Index == -1)
            {
                Console.WriteLine("Error in Making Packet");
                errorstring = "添加校验码后的封包错误";
                return false;
            }
            dwStart = System.Environment.TickCount; // Start timing
            Console.WriteLine(dwStart.ToString());
            //send the Pack over the socket

            if ((nBytes = socket.SendTo(sendbuf, PacketSize, 0, epServer)) == SOCKET_ERROR)
            {
                Console.WriteLine("Socket Error cannot Send Packet");
                errorstring = "SendTo方法失败";
                return false;
            }


            // Initialize the buffers. The receive b
            //     uffer is the size of the
            // ICMP header plus the IP header (20 by
            //     tes)
            Byte[] ReceiveBuffer = new Byte[256];
            nBytes = 0;
            //Receive the bytes
            bool recd = false;
            int timeout = 0;
            //loop for checking the time of the serv
            //     er responding
            while (!recd)
            {

                socket.ReceiveTimeout = pingtimeout;

                try
                {
                    nBytes = socket.ReceiveFrom(ReceiveBuffer, 256, 0, ref EndPointFrom);
                    Console.WriteLine(nBytes.ToString());
                    if (nBytes == SOCKET_ERROR)
                    {
                        //Console.WriteLine("Host not Responding");
                        errorstring = "主机没有响应";
                        recd = true;
                        return false;

                    }
                    else if (nBytes > 0)
                    {
                        dwStop = System.Environment.TickCount - dwStart; // stop timing
                        Console.WriteLine("Reply from " + epServer.ToString() + " in " + dwStop + "ms => Bytes Received: " + nBytes);
                        recd = true;
                        return true;
                    }
                    timeout = System.Environment.TickCount - dwStart;
                    Console.WriteLine(timeout.ToString());
                    //if (timeout > 1000)
                    //{
                    //    Console.WriteLine("Time Out");
                    //    recd = true;
                    //    return false;
                    //}
                }
                catch (Exception ex)
                {
                    if (Global.IsShowBug)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                    errorstring = ex.Message;
                    return false;
                }
            }
            //close the socket
            socket.Close();
            return false;
        }

        public static Int32 Serialize(IcmpPacket packet, Byte[] Buffer, Int32 PacketSize, Int32 PingData)
        {
            Int32 cbReturn = 0;
            // serialize the struct into the array
            int Index = 0;
            Byte[] b_type = new Byte[1];
            b_type[0] = (packet.Type);
            Byte[] b_code = new Byte[1];
            b_code[0] = (packet.SubCode);
            Byte[] b_cksum = BitConverter.GetBytes(packet.CheckSum);
            Byte[] b_id = BitConverter.GetBytes(packet.Identifier);
            Byte[] b_seq = BitConverter.GetBytes(packet.SequenceNumber);
            // Console.WriteLine("Serialize type ");
            //     
            Array.Copy(b_type, 0, Buffer, Index, b_type.Length);
            Index += b_type.Length;
            // Console.WriteLine("Serialize code ");
            //     
            Array.Copy(b_code, 0, Buffer, Index, b_code.Length);
            Index += b_code.Length;
            // Console.WriteLine("Serialize cksum ")
            //     ;
            Array.Copy(b_cksum, 0, Buffer, Index, b_cksum.Length);
            Index += b_cksum.Length;
            // Console.WriteLine("Serialize id ");
            Array.Copy(b_id, 0, Buffer, Index, b_id.Length);
            Index += b_id.Length;
            Array.Copy(b_seq, 0, Buffer, Index, b_seq.Length);
            Index += b_seq.Length;
            // copy the data
            Array.Copy(packet.Data, 0, Buffer, Index, PingData);
            Index += PingData;
            if (Index != PacketSize/* sizeof(IcmpPacket) */)
            {
                cbReturn = -1;
                return cbReturn;
            }
            cbReturn = Index;
            return cbReturn;
        }

        public static UInt16 checksum(UInt16[] buffer, int size)
        {
            Int32 cksum = 0;
            int counter;
            counter = 0;
            while (size > 0)
            {
                UInt16 val = buffer[counter];
                cksum += Convert.ToInt32(buffer[counter]);
                counter += 1;
                size -= 1;
            }
            cksum = (cksum >> 16) + (cksum & 0xffff);
            cksum += (cksum >> 16);
            return (UInt16)(~cksum);
        }
    }
}
