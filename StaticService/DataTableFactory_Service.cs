using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PersonPositionServer.StaticService
{
    public static class DataTableFactory_Service
    {
        /// <summary>
        /// 一次巡检信息表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable MakeBasicPositionTable(string tableName)
        {
            //创建定位信息表
            DataTable table = new DataTable(tableName);
            //创建StationID列
            DataColumn StationID = new DataColumn("StationID");
            StationID.DataType = System.Type.GetType("System.Int32");
            table.Columns.Add(StationID);
            //创建CardID列
            DataColumn CardID = new DataColumn("CardID");
            CardID.DataType = System.Type.GetType("System.Int32");
            table.Columns.Add(CardID);
            //创建RSSI列
            DataColumn RSSI = new DataColumn("RSSI");
            RSSI.DataType = System.Type.GetType("System.Int32");
            table.Columns.Add(RSSI);
            
            DataColumn DT = new DataColumn("DT");
            StationID.DataType = System.Type.GetType("System.Int64");
            table.Columns.Add(DT);

           /*DataColumn[] Keys = new DataColumn[2];
            Keys[0] = StationID;
            Keys[1] = CardID;
            table.PrimaryKey = Keys;
            */
            


            return table; 
        }

        /// <summary>
        /// 综合定位信息表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable MakePositionTable(string tableName)
        {
            //创建定位信息表
            DataTable table = new DataTable(tableName);
            //创建CardID列
            DataColumn CardID = new DataColumn("CardID");
            CardID.DataType = System.Type.GetType("System.Int32");
            table.Columns.Add(CardID);
            
            //创建StationID列
            
            DataColumn StationID = new DataColumn("StationID");
            StationID.DataType = System.Type.GetType("System.Int32");
            table.Columns.Add(StationID);




            //创建Geo_X列
            DataColumn Geo_X = new DataColumn("Geo_X");
            Geo_X.DataType = System.Type.GetType("System.Double");
            table.Columns.Add(Geo_X);
            //创建Geo_Y列
            DataColumn Geo_Y = new DataColumn("Geo_Y");
            Geo_Y.DataType = System.Type.GetType("System.Double");
            table.Columns.Add(Geo_Y);
            //创建NullSignalTimes列
            DataColumn NullSignalTimes = new DataColumn("NullSignalTimes");
            NullSignalTimes.DataType = System.Type.GetType("System.Int32");
            table.Columns.Add(NullSignalTimes);
            //创建InMineTime列
            DataColumn InMineTime = new DataColumn("InMineTime");
            InMineTime.DataType = System.Type.GetType("System.DateTime");
            table.Columns.Add(InMineTime);
            //创建InNullRSSITime列
            DataColumn InNullRSSITime = new DataColumn("InNullRSSITime");
            InNullRSSITime.DataType = System.Type.GetType("System.DateTime");
            table.Columns.Add(InNullRSSITime);
            //创建IsOperated列
            DataColumn IsOperated = new DataColumn("IsOperated");
            IsOperated.DataType = System.Type.GetType("System.Boolean");
            table.Columns.Add(IsOperated);


            DataColumn Area = new DataColumn("Area");
            Area.DataType = System.Type.GetType("System.String");
            table.Columns.Add(Area);

            //添加主键
            DataColumn[] Keys = new DataColumn[1];
            Keys[0] = CardID;
            table.PrimaryKey = Keys;

            return table;
        }
    }
}
