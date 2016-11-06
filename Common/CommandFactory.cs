using System;
using System.Collections.Generic;
using System.Text;

using PersonPositionServer.StaticService;

namespace PersonPositionServer.Common
{
    public class CommandFactory
    {
        public static byte BagNum = 0;

        //新的巡检命令　带密码
        public static byte[] GetLoopCommand(int AimStationID, int Password1, int Password2, int Password3, int Password4, int Password5, int Password6)
        {
            byte[] ParameterArray = new byte[6];
            ParameterArray[0] = Convert.ToByte(Password1);
            ParameterArray[1] = Convert.ToByte(Password2);
            ParameterArray[2] = Convert.ToByte(Password3);
            ParameterArray[3] = Convert.ToByte(Password4);
            ParameterArray[4] = Convert.ToByte(Password5);
            ParameterArray[5] = Convert.ToByte(Password6);

            return MakeCommand(Protocol_Service.CommandType.Loop, 0,AimStationID, ParameterArray);
        }

        public static byte[] GetAddSonCommand(int FatherID, int SonID)
        {
            byte[] son = new byte[2];
            son[0] = Convert.ToByte((SonID >> 8) & 0xFF);//高八位
            son[1] = Convert.ToByte(SonID & 0xFF);//低八位
            //根据协议，子基站ID的最高位为控制位（1表示添加，0表示删除）
            //在这里则“或”上10000000
            son[0] = Convert.ToByte(son[0] | 0x80);

            return MakeCommand(Protocol_Service.CommandType.AddSon, 0,FatherID, son);
        }

        public static byte[] GetDelSonCommand(int FatherID, int DelSonID)
        {
            byte[] son = new byte[2];
            son[0] = Convert.ToByte((DelSonID >> 8) & 0xFF);//高八位
            son[1] = Convert.ToByte(DelSonID & 0xFF);//低八位
            //根据协议，子基站ID的最高位为控制位（1表示添加，0表示删除）
            //在这里则“与”上01111111
            son[0] = Convert.ToByte(son[0] & 0x7F);

            return MakeCommand(Protocol_Service.CommandType.DelSon, 0,FatherID, son);
        }

        public static byte[] GetDownMessageCommand(int AimStationID,int CardID, int MessageType)
        {
            byte[] ParameterArray = new byte[4];
            ParameterArray[0] = Convert.ToByte((CardID >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(CardID & 0xFF);//低八位
            ParameterArray[2] = 1;
            ParameterArray[3] = Convert.ToByte(MessageType);

            return MakeCommand(Protocol_Service.CommandType.DownMessage, 0,AimStationID, ParameterArray);
        }

        public static byte[] GetSetTimeCommand(int FatherStationID, int AimStationID, DateTime Time)
        {
            byte[] ParameterArray = new byte[9];
            ParameterArray[0] = Convert.ToByte((AimStationID >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(AimStationID & 0xFF);//低八位
            ParameterArray[2] = Convert.ToByte(Time.Year.ToString().Substring(2, 2));//年
            ParameterArray[3] = Convert.ToByte(Time.Month);//月
            if (Convert.ToInt32(Time.DayOfWeek) == 0)
            {
                ParameterArray[4] = Convert.ToByte(7);//星期几
            }
            else
            {
                ParameterArray[4] = Convert.ToByte(Convert.ToInt32(Time.DayOfWeek));//星期几
            }
            ParameterArray[5] = Convert.ToByte(Time.Day);//日
            ParameterArray[6] = Convert.ToByte(Time.Hour);//时
            ParameterArray[7] = Convert.ToByte(Time.Minute);//分
            ParameterArray[8] = Convert.ToByte(Time.Second);//秒

            return MakeCommand(Protocol_Service.CommandType.SetTime, 0, FatherStationID, ParameterArray);
        }

        public static byte[] GetLightUpCommand(int AimStationID,int LightNum)
        {
            byte[] ParameterArray = new byte[1];
            ParameterArray[0] = Convert.ToByte(LightNum);

            return MakeCommand(Protocol_Service.CommandType.LightUp, 0,AimStationID, ParameterArray);
        }

        public static byte[] GetGetHistoryDataCommand(int FatherStationID, int AimStationID)
        {
            byte[] ParameterArray = new byte[2];
            ParameterArray[0] = Convert.ToByte((AimStationID >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(AimStationID & 0xFF);//低八位
            return MakeCommand(Protocol_Service.CommandType.GetHistoryData, 0, FatherStationID, ParameterArray);
        }

      
        
        
        public static byte[] GetDelHistoryDataCommand(int FatherStationID, int AimStationID)
        {
            byte[] ParameterArray = new byte[2];
            ParameterArray[0] = Convert.ToByte((AimStationID >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(AimStationID & 0xFF);//低八位
            return MakeCommand(Protocol_Service.CommandType.DelHistoryData, 0, FatherStationID, ParameterArray);
        }

        public static byte[] GetSetInMineNumCommand(int AimStationID, int InMineNum)
        {
            byte[] ParameterArray = new byte[2];
            ParameterArray[0] = Convert.ToByte((InMineNum >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(InMineNum & 0xFF);//低八位
            return MakeCommand(Protocol_Service.CommandType.SetInMineNum, 0, AimStationID, ParameterArray);
        }

        public static byte[] GetSetInfoCommand(int AimStationID, byte CommandStyle, byte ShowIndex, string Info)
        {
            byte[] info_list = Encoding.Unicode.GetBytes(Info);
            byte[] ParameterArray = new byte[info_list.Length + 2];
            ParameterArray[0] = CommandStyle;
            ParameterArray[1] = ShowIndex;
            for (int i = 0; i < info_list.Length; i++)
            {
                ParameterArray[2 + i] = info_list[i];
            }
            return MakeCommand(Protocol_Service.CommandType.SetInfo, 0, AimStationID, ParameterArray);
        }

        //最新的巡检命令　带人数
        public static byte[] GetNewLoopCommand(int AimStationID, int InMineNum)
        {
            byte[] ParameterArray = new byte[6];
            ParameterArray[0] = Convert.ToByte((InMineNum >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(InMineNum & 0xFF);//低八位
            ParameterArray[2] = 0;
            ParameterArray[3] = 0;
            ParameterArray[4] = 0;
            ParameterArray[5] = 0;

            return MakeCommand(Protocol_Service.CommandType.Loop, 0, AimStationID, ParameterArray);
        }





        public static byte[] set_s_num(int dID, int sID)
        {
            byte[] ParameterArray = new byte[2];
            ParameterArray[0] = Convert.ToByte((dID >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(dID & 0xFF);//低八位
           

            return MakeCommand(Protocol_Service.CommandType.cset_s_num, 0, sID, ParameterArray);

        }
        
        
        public static byte[] set_w_no(int dID, int num)
        {
            byte[] ParameterArray = new byte[2];
            ParameterArray[0] = Convert.ToByte((num >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(num & 0xFF);//低八位
           

            return MakeCommand(Protocol_Service.CommandType.cset_w_no, 0, dID, ParameterArray);

        }
        
        
        public static byte[] set_w_num(int dID, int num)
        {
            byte[] ParameterArray = new byte[2];
            ParameterArray[0] = Convert.ToByte((num >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(num & 0xFF);//低八位
           

            return MakeCommand(Protocol_Service.CommandType.cset_w_num, 0, dID, ParameterArray);

        }
        
        public static byte[] set_up_ch(int dID, int num)
        {
            byte[] ParameterArray = new byte[1];
            ParameterArray[0] = Convert.ToByte(num);//高八位
            //ParameterArray[1] = Convert.ToByte(num & 0xFF);//低八位
           

            return MakeCommand(Protocol_Service.CommandType.cset_up_ch, 0, dID, ParameterArray);

        }
        
         public static byte[] set_down_ch(int dID, int num)
        {
            byte[] ParameterArray = new byte[1];
            ParameterArray[0] = Convert.ToByte(num);//高八位
            //ParameterArray[1] = Convert.ToByte(num & 0xFF);//低八位
           

            return MakeCommand(Protocol_Service.CommandType.cset_down_ch, 0, dID, ParameterArray);

        }
        
        public static byte[] set_space(int dID, int num1,int num2)
        {
            byte[] ParameterArray = new byte[4];
            ParameterArray[0] = Convert.ToByte((num1 >> 8) & 0xFF);//高八位
            ParameterArray[1] = Convert.ToByte(num1 & 0xFF);//低八位
           
            ParameterArray[2] = Convert.ToByte((num2 >> 8) & 0xFF);//高八位
            ParameterArray[3] = Convert.ToByte(num2 & 0xFF);//低八位
               
            return MakeCommand(Protocol_Service.CommandType.cset_space, 0, dID, ParameterArray);

        }

        public static byte[] set_reset(int dID)
        {
            
            return MakeCommand(Protocol_Service.CommandType.cset_reset, 0, dID ,null);

        }
        
        //语音数据处理


       
        public static byte[] SendSoundData(int AimStationID, int souceId, byte[] Para)
        {
            int len=Para.Length ;
            byte[] ResultCommand = new byte[9 + len+1];
            byte check;
            check = 0;
            ResultCommand[0] = 0xAA;
            ResultCommand[1] = 0x55;
            ResultCommand[2] = Convert.ToByte(0x00 + (BagNum & 0x0F));
            ResultCommand[3] = Convert.ToByte (len+1);
            ResultCommand[4] = Convert.ToByte((AimStationID >> 8) & 0xFF);//高八位
            ResultCommand[5] = Convert.ToByte(AimStationID & 0xFF);//低八位
            ResultCommand[6] = Convert.ToByte((souceId >> 8) & 0xFF);//高八位
            ResultCommand[7] = Convert.ToByte(souceId & 0xFF);//低八位
            ResultCommand[8] = 0x12;
            
           
            for (int j = 0; j < Para.Length; j++)
            {
                ResultCommand[j + 9] = Para[j];   
            
            }

            //计算校验位
            for (int j = 2; j < ResultCommand.Length - 1; j++)
            {
                check =Convert.ToByte  ( check ^ ResultCommand[j]);
            }
            //将校验码放入最后一位
            ResultCommand[9 + len] = Convert.ToByte(check);

            return ResultCommand;
        }


        private static byte[] MakeCommand(Protocol_Service.CommandType CommandType,int SourceStationID, int AimStationID, byte[] ParameterArray)
        {
            //最终返回的命令
            byte[] ResultCommand = null;
            //帧控制字
            byte control = Convert.ToByte(0x40 + (BagNum & 0x0F));
            //自增包序号
            BagNum++;
            //有效数据长度，为：命令字的长度1 + 参数数组的长度

            byte length = 1;
            if(ParameterArray!=null )
                length = Convert.ToByte(1 + ParameterArray.Length);
            //原基站ID
            byte[] source = new byte[2];
            source[0] = Convert.ToByte((SourceStationID >> 8) & 0xFF);//高八位
            source[1] = Convert.ToByte(SourceStationID & 0xFF);//低八位
            //目标基站ID
            byte[] aim = new byte[2];
            aim[0] = Convert.ToByte((AimStationID >> 8) & 0xFF);//高八位
            aim[1] = Convert.ToByte(AimStationID & 0xFF);//低八位
            //帧命令字
            byte command = 0;
            //校验字
            int check = 0;

            switch (CommandType)
            {
                //巡检命令
                case Protocol_Service.CommandType.Loop:
                    command = 0x50;
                    break;
                //添加子节点命令
                case Protocol_Service.CommandType.AddSon:
                    command = 0x54;
                    break;
                //删除子节点命令
                case Protocol_Service.CommandType.DelSon:
                    command = 0x54;
                    break;
                //设置基站时间
                case Protocol_Service.CommandType.SetTime:
                    command = 0x91;
                    break;
                //发送下行短信
                case Protocol_Service.CommandType.DownMessage:
                    command = 0x51;
                    break;
                //点亮灯命令
                case Protocol_Service.CommandType.LightUp:
                    command = 0x74;
                    break;
                //得到历史数据命令
                case Protocol_Service.CommandType.GetHistoryData:
                    command = 0x60;
                    break;
                //清空历史数据命令
                case Protocol_Service.CommandType.DelHistoryData:
                    command = 0x5F;
                    break;
                //设置考勤基站人数命令
                case Protocol_Service.CommandType.SetInMineNum:
                    command = 0x24;
                    break;
                //设置显示信息命令
                case Protocol_Service.CommandType.SetInfo:
                    command = 0x20;
                    break;

                case Protocol_Service.CommandType.cset_s_num :
                    command = 0x22;
                    break;    
                case Protocol_Service.CommandType.cset_w_num :
                    command = 0x27;
                    break;   
                case Protocol_Service.CommandType.cset_w_no :
                    command = 0x21;
                    break;   
                case Protocol_Service.CommandType.cset_space :
                    command = 0x23;
                    break;   
                case Protocol_Service.CommandType.cset_up_ch :
                    command = 0x1e;
                    break;   
                case Protocol_Service.CommandType.cset_down_ch :
                    command = 0x1f;
                    break;
                case Protocol_Service.CommandType.cset_reset:
                    command = 0x26;
                    break;   
            }

            ResultCommand = new byte[9 + length];
            ResultCommand[0] = 0xAA;
            ResultCommand[1] = 0x55;
            ResultCommand[2] = control;
            ResultCommand[3] = length;
            ResultCommand[4] = aim[0];
            ResultCommand[5] = aim[1];
            ResultCommand[6] = source[0];
            ResultCommand[7] = source[1];
            ResultCommand[8] = command;
            if (ParameterArray != null)
            for (int i = 0; i < ParameterArray.Length; i++)
            {
                ResultCommand[9 + i] = ParameterArray[i];
            }
            //计算校验位
            for (int j = 2; j < ResultCommand.Length - 1; j++)
            {
                check = check ^ ResultCommand[j];
            }
            //将校验码放入最后一位
            ResultCommand[9 + length - 1] = Convert.ToByte(check);
            if (Global.Isnew)
                return ResultCommand;
            else
            {
                byte[]  temparr=null ;
                temparr = new byte[7 + length];
                for (Int16 i=0;i<7+length ;i++)
                {
                    temparr[i]=ResultCommand[i+2];
                }
                return temparr;
            }

        }

    }
}
