using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using PersonPositionServer.Common;

namespace PersonPositionServer.StaticService
{
    public static class DB_Service
    {
        //定位信息历史表中最新的一批记录的时间
        public static DateTime LastInsertHistory_Position;
        //采集信息历史表中最新的一批记录的时间
        public static Dictionary<string, DateTime> LastInsertHistory_Collect = new Dictionary<string, DateTime>();

        /// <summary>
        /// 检查并更新数据库
        /// </summary>
        public static void CheckDBAndUpdate()
        {
            try
            {
                List<string> tempSQLs = new List<string>();
                DataTable tempTable;
                //2010-9-19  版本：1.7.0 -> 1.7.1
                //在StationTable增加1个字段：RepairRSSI
                tempTable = DB_Service.GetTable("TempStationTable", "select * from StationTable");
                if (!tempTable.Columns.Contains("RepairRSSI"))
                {
                    tempSQLs.Clear();
                    tempSQLs.Add("Alter Table StationTable Add RepairRSSI Int");
                    tempSQLs.Add("update StationTable set RepairRSSI = 0");
                    DB_Service.ExecuteSQLs(tempSQLs);
                }
                //2010-9-26  版本：1.7.1 -> 1.7.2
                //在StationTable增加1个字段：IsLeafStation
                tempTable = DB_Service.GetTable("TempStationTable", "select * from StationTable");
                if (!tempTable.Columns.Contains("IsLeafStation"))
                {
                    tempSQLs.Clear();
                    tempSQLs.Add("Alter Table StationTable Add IsLeafStation Bit");
                    tempSQLs.Add("update StationTable set IsLeafStation = 'False'");
                    DB_Service.ExecuteSQLs(tempSQLs);
                }
                //2010-10-18  版本：1.7.2 -> 1.7.3
                //添加了1张表：CardTypeTable。添加了一行
                if (!DB_Service.IsExistTable("CardTypeTable"))
                {
                    tempSQLs.Clear();
                    tempSQLs.Add("Create Table CardTypeTable (ID Int PRIMARY KEY,CardType varchar(50) NULL)");
                    tempSQLs.Add("Insert into CardTypeTable (ID,CardType) values (1,'一般人员')");
                    tempSQLs.Add("Insert into CardTypeTable (ID,CardType) values (2,'高级人员')");
                    tempSQLs.Add("Insert into CardTypeTable (ID,CardType) values (3,'特殊人员')");
                    DB_Service.ExecuteSQLs(tempSQLs);
                }
                //2010-10-22  版本：1.7.3 -> 1.7.4
                //在LayerTable增加1个字段：IsShowInTree。添加了一行
                tempTable = DB_Service.GetTable("TempLayerTableTable", "select * from LayerTable");
                if (!tempTable.Columns.Contains("IsShowInTree"))
                {
                    tempSQLs.Clear();
                    tempSQLs.Add("Alter Table LayerTable Add IsShowInTree Bit");
                    tempSQLs.Add("update LayerTable set IsShowInTree = 'True'");
                    DB_Service.ExecuteSQLs(tempSQLs);
                    ExecuteSQL("Insert into LayerTable (TableOrShapeFile,LayerName,DataSourceType,MapName,ViewOrder,PicID,PointImage,Comment,LabelLayerColName,LabelLayerMinShow,LabelLayerMaxShow,Line_Color,Line_Width,Fill_IsSolid,Fill_Color,Fill_Image,FillLine_Enable,FillLine_Color,FillLine_Width,IsShowInTree) values ('MapTextTable','地图文字信息',1,'" + DB_Service.GetTable("temp", "select * from MapTable").Rows[0]["MapName"].ToString() + "',9,'MapNULLIco','MapNULLIco','','Name',0,9999999,'255,0,0,0',1,'True','255,0,0,0','','True','255,0,0,0',1,'False')");
                }
                //2010-11-22  版本：1.7.5 -> 1.7.6
                //在SpecalTable删除1个字段：KeyRSSI。
                tempTable = DB_Service.GetTable("TempSpecalTable", "select * from SpecalTable");
                if (tempTable.Columns.Contains("KeyRSSI"))
                {
                    ExecuteSQL("Alter Table SpecalTable Drop Column KeyRSSI");
                }
                //2011-1-5  版本：1.7.9 -> 1.8.0
                //在WorkTypeTable增加1个字段：NeedWorkHour
                //添加2张表：AlarmMaxPersonTable AlarmMaxHourTable
                tempTable = DB_Service.GetTable("TempWorkTypeTable", "select * from WorkTypeTable");
                if (!tempTable.Columns.Contains("NeedWorkHour"))
                {
                    tempSQLs.Clear();
                    tempSQLs.Add("Alter Table WorkTypeTable Add NeedWorkHour Int");
                    tempSQLs.Add("update WorkTypeTable set NeedWorkHour = 8");
                    tempSQLs.Add("Create Table AlarmMaxPersonTable (Alarm_ID Int IDENTITY(1,1) PRIMARY KEY,RealPersonNum Int NULL,AllowPersonNum Int NULL,Time DateTime NULL)");
                    tempSQLs.Add("Create Table AlarmMaxHourTable (Alarm_ID Int IDENTITY(1,1) PRIMARY KEY,CardID Int NULL,InMineHour Int NULL,AllowInMineHour Int NULL,Time DateTime NULL)");
                    DB_Service.ExecuteSQLs(tempSQLs);
                }
                //2011-2-12  版本：1.8.1 -> 1.8.2
                //修改PersonTable的PID主键为varchar(50)类型，删除Class字段
                //修改CardTable、DepartmentTable中的PID外键为varchar(50)类型
                tempTable = DB_Service.GetTable("TempDepartmentTable", "select * from DepartmentTable");
                if (tempTable.Columns["ChiefPID"].DataType == typeof(Int32))
                {
                    tempSQLs.Clear();
                    //建立临时表
                    tempSQLs.Add("Create Table tmp_PersonTable ([PID] varchar(50) NOT NULL, [Name] varchar(50) NULL,[Sex] varchar(50) NULL,[Age] [tinyint] NULL,[Blood] [varchar](50) NULL,[WorkType] [varchar](50) NULL,[Department] [varchar](50) NULL,[Tele] [varchar](50) NULL,[Mobile] [varchar](50) NULL,[PersonKey] [varchar](50) NULL,[BirthDay] [varchar](50) NULL,[Email] [varchar](50) NULL,[FamilyAdd] [varchar](50) NULL,[Photo] [image] NULL)");
                    //把原表数据导入临时表 
                    tempSQLs.Add("Insert into tmp_PersonTable(PID, Name,Sex,Age,Blood,WorkType,Department,Tele,Mobile,PersonKey,BirthDay,Email,FamilyAdd,Photo) SELECT CONVERT(varchar,PID), Name,Sex,Age,Blood,WorkType,Department,Tele,Mobile,PersonKey,BirthDay,Email,FamilyAdd,Photo From PersonTable");
                    //删除原表 
                    tempSQLs.Add("Drop Table PersonTable ");
                    //修改临时表名为原表名 
                    tempSQLs.Add("EXEC sp_rename 'tmp_PersonTable', 'PersonTable'");
                    //为表添加主键 
                    tempSQLs.Add("Alter Table PersonTable Add Constraint PersonTable_PID Primary Key(PID)");
                    //修改CardTable中的PID外键类型
                    tempSQLs.Add("Alter table CardTable alter column PID varchar(50) null");
                    //修改DepartmentTable中的PID外键类型
                    tempSQLs.Add("Alter table DepartmentTable alter column ChiefPID varchar(50) null");
                    
                    DB_Service.ExecuteSQLs(tempSQLs);
                }
                //2011-3-2  版本：1.8.5 -> 1.8.6
                //新增1个表：AlarmNoCardTable
                if (!DB_Service.IsExistTable("AlarmNoCardTable"))
                {
                    tempSQLs.Clear();
                    tempSQLs.Add("Create Table AlarmNoCardTable (Alarm_ID Int IDENTITY(1,1),NoCardNum Int NULL,IsReaded Bit NULL,Time DateTime NULL)");
                    tempSQLs.Add("Alter Table AlarmNoCardTable Add Constraint AlarmNoCardTable_Alarm_ID Primary Key(Alarm_ID)");
                    DB_Service.ExecuteSQLs(tempSQLs);
                }
                //2011-3-22  版本：1.8.6 -> 1.8.7
                //新增1个表：AlarmInAreaTable
                if (!DB_Service.IsExistTable("AlarmInAreaTable"))
                {
                    tempSQLs.Clear();
                    tempSQLs.Add("Create Table AlarmInAreaTable (Alarm_ID Int IDENTITY(1,1),InAreaName Varchar(50) NULL,StationID Int NULL,CardID Int NULL,Time DateTime NULL)");
                    tempSQLs.Add("Alter Table AlarmInAreaTable Add Constraint AlarmInAreaTable_Alarm_ID Primary Key(Alarm_ID)");
                    DB_Service.ExecuteSQLs(tempSQLs);
                }
                tempSQLs.Clear();
                tempSQLs = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化定位信息表中最后一条记录的时间
        /// </summary>
        /// <returns></returns>
        public static bool InitLastInsertHistory_Position()
        {
            try
            {
                using (SqlConnection conn_Time = new SqlConnection(Global.DBConnStr))
                {
                    conn_Time.Open();
                    using (SqlCommand comm_Time = new SqlCommand("select top 1 Time from HistoryPositionTable order by ID DESC", conn_Time))
                    {
                        using (SqlDataReader reader_Time = comm_Time.ExecuteReader())
                        {
                            if (reader_Time.Read())
                            {
                                try
                                {
                                    if (reader_Time.IsDBNull(0))
                                    {
                                        DB_Service.LastInsertHistory_Position = new DateTime(2000, 1, 1, 0, 0, 0);
                                    }
                                    else
                                    {
                                        DB_Service.LastInsertHistory_Position = reader_Time.GetDateTime(0);
                                    }
                                    return true;
                                }
                                catch
                                {
                                    return false;
                                }
                                finally
                                {
                                    reader_Time.Close();
                                }
                            }
                            else
                            {
                                DB_Service.LastInsertHistory_Position = new DateTime(2000, 1, 1, 0, 0, 0);
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSQLs"></param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSQL(string strSQL)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Global.DBConnStr))
                {
                    using (SqlCommand command = new SqlCommand(strSQL, conn))
                    {
                        conn.Open();
                        command.CommandText = strSQL;
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 以一个事务执行SQL语句组
        /// </summary>
        /// <param name="strSQLs"></param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSQLs(List<string> strSQLs)
        {
            int result = 0;
            SqlTransaction tran = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(Global.DBConnStr))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        conn.Open();
                        tran = conn.BeginTransaction();
                        command.Connection = conn;
                        command.Transaction = tran;
                        for (int i = 0; i < strSQLs.Count; i++)
                        {
                            command.CommandText = strSQLs[i].ToString();
                            result += command.ExecuteNonQuery();
                        }
                        tran.Commit();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 以Master表的身份连接数据库并执行SQL语句
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteSQL_Master(string strSQL)
        {
            try
            {
                string TempConn = Global.DBConnStr.Replace("PersonPosition", "Master");
                using (SqlConnection conn = new SqlConnection(TempConn))
                {
                    using (SqlCommand command = new SqlCommand(strSQL, conn))
                    {
                        conn.Open();
                        command.CommandText = strSQL;
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 得到一个以指定表名命名的用户表
        /// </summary>
        /// <param name="NewTableName">新表名</param>
        /// <param name="strSQL">SQL语句</param>
        /// <returns></returns>
        public static DataTable GetTable(string NewTableName, string strSQL)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Global.DBConnStr))
                {
                    using (SqlCommand command = new SqlCommand(strSQL, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            using (DataTable table = new DataTable(NewTableName))
                            {
                                adapter.Fill(table);
                                return table;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断制定表名的表是否存在
        /// </summary>
        public static bool IsExistTable(string TableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Global.DBConnStr))
                {
                    using (SqlCommand command = new SqlCommand("select top 1 * from " + TableName, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            using (DataTable table = new DataTable(TableName))
                            {
                                adapter.Fill(table);
                                return true;
                            }
                        }
                    }
                }

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 刷新指定表的记录
        /// </summary>
        /// <param name="Table">要刷新纪录的表</param>
        /// <param name="strSQL">刷新SQL语句</param>
        public static void RefreshTable(ref DataTable Table, string strSQL)
        {
            try
            {
                Table.Rows.Clear();
                using (SqlConnection conn = new SqlConnection(Global.DBConnStr))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(strSQL, conn))
                    {
                        adapter.Fill(Table);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        /// <summary>
        /// 根据情况，返回插入AlarmPersonSendTable的SQL语句
        /// </summary>
        /// <param name="sourceStationID"></param>
        /// <param name="cardID"></param>
        /// <param name="RSSI"></param>
        /// <param name="nowTime"></param>
        /// <param name="lastTime"></param>
        /// <returns></returns>
        public static string MakeAlarmPersonSendSQL(int cardID, string MessageType,DateTime Time)
        {
            return "insert into AlarmPersonSendTable (CardID,IsReaded,SendTime,AlarmType) values (" + cardID + ",'False','" + Time + "','" + MessageType + "')";
        }

        /// <summary>
        /// 根据情况，返回插入AlarmPowerTable的SQL语句
        /// </summary>
        /// <param name="sourceStationID"></param>
        /// <param name="cardID"></param>
        /// <param name="RSSI"></param>
        /// <param name="nowTime"></param>
        /// <param name="lastTime"></param>
        /// <returns></returns>
        public static string MakeAlarmPowerSQL(int cardID, DateTime Time)
        {
            return "insert into AlarmPowerTable (CardID,IsReaded,ErrorStartTime) values (" + cardID + ",'False','" + Time + "')";
        }

        /// <summary>
        /// 根据情况，返回插入AlarmNoCardTable的SQL语句
        /// </summary>
        /// <param name="noCardNum"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static string MakeAlarmNoCardSQL(int noCardNum, DateTime Time)
        {
            return "insert into AlarmNoCardTable (NoCardNum,IsReaded,Time) values (" + noCardNum + ",'False','" + Time + "')";
        }

        public static void CheckAlarmMaxPersonInDB(int InMinePersonNum, int AllowMaxPersonNum, DateTime Time)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Global.DBConnStr))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select top 1 Time from AlarmMaxPersonTable order by Alarm_ID DESC", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                try
                                {
                                    if (reader.IsDBNull(0))
                                    {
                                        DB_Service.ExecuteSQL("insert into AlarmMaxPersonTable (RealPersonNum,AllowPersonNum,Time) values (" + InMinePersonNum + "," + AllowMaxPersonNum + ",'" + Time + "')");
                                    }
                                    else
                                    {
                                        DateTime lastTime = reader.GetDateTime(0);
                                        TimeSpan ts = Time - lastTime;
                                        if (ts.TotalMinutes > 5.0)
                                            DB_Service.ExecuteSQL("insert into AlarmMaxPersonTable (RealPersonNum,AllowPersonNum,Time) values (" + InMinePersonNum + "," + AllowMaxPersonNum + ",'" + Time + "')");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                finally
                                {
                                    reader.Close();
                                }
                            }
                            else
                            {
                                DB_Service.ExecuteSQL("insert into AlarmMaxPersonTable (RealPersonNum,AllowPersonNum,Time) values (" + InMinePersonNum + "," + AllowMaxPersonNum + ",'" + Time + "')");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CheckAlarmMaxHourInDB(int CardID, int InMineHour, int AllowInMineHour, DateTime Time)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Global.DBConnStr))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand("select Alarm_ID,Time from AlarmMaxHourTable where CardID = " + CardID + " order by Alarm_ID DESC", conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                try
                                {
                                    if (reader.IsDBNull(1))
                                    {
                                        DB_Service.ExecuteSQL("insert into AlarmMaxHourTable (CardID,InMineHour,AllowInMineHour,Time) values (" + CardID + "," + InMineHour + "," + AllowInMineHour + ",'" + Time + "')");
                                    }
                                    else
                                    {
                                        DateTime lastTime = reader.GetDateTime(1);
                                        TimeSpan ts = Time - lastTime;
                                        if (ts.TotalMinutes > 30.0)
                                            DB_Service.ExecuteSQL("insert into AlarmMaxHourTable (CardID,InMineHour,AllowInMineHour,Time) values (" + CardID + "," + InMineHour + "," + AllowInMineHour + ",'" + Time + "')");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                finally
                                {
                                    reader.Close();
                                }
                            }
                            else
                            {
                                DB_Service.ExecuteSQL("insert into AlarmMaxHourTable (CardID,InMineHour,AllowInMineHour,Time) values (" + CardID + "," + InMineHour + "," + AllowInMineHour + ",'" + Time + "')");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
