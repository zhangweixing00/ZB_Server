using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;

using PersonPositionServer.Common;

namespace PersonPositionServer.StaticService
{
    public static class BasicData
    {
        //常用表

        public  static DataTable MainStationTable;
        //private static DataTable MainStationTable;
        private static DataTable MainCardTable;
        private static DataTable MainPersonTable;
        private static DataTable MainSpecalTable;
        private static DataTable MainCollectChannelTable;
        //更新数据库锁开关变量
        private static bool IsLockStationTable = false;
        private static bool IsLockCardTable = false;
        private static bool IsLockPersonTable = false;
        private static bool IsLockSpecalTable = false;
        private static bool IsLockCollectChannelTable = false;

        public static void InitALLTable()
        {
            MainStationTable = DB_Service.GetTable("MainStationTable", "select * from StationTable");
            MainCardTable = DB_Service.GetTable("MainCardTable", "select * from CardTable");
            MainPersonTable = DB_Service.GetTable("MainPersonTable", "select * from PersonTable");
            MainSpecalTable = DB_Service.GetTable("MainSpecalTable", "select * from SpecalTable");
            MainCollectChannelTable = DB_Service.GetTable("MainCollectChannelTable", "select * from CollectChannelTable");
        }

        public static void DisableALLTable()
        {
            if (MainCardTable != null)
                MainCardTable.Clear();
            if (MainPersonTable != null)
                MainPersonTable.Clear();
            if (MainStationTable != null)
                MainStationTable.Clear();
            if (MainSpecalTable != null)
                MainSpecalTable.Clear();
            if (MainCollectChannelTable != null)
                MainCollectChannelTable.Clear();
            MainCardTable = null;
            MainPersonTable = null;
            MainStationTable = null;
            MainSpecalTable = null;
            MainCollectChannelTable = null;
        }

        public static DataRow[] GetStationTableRows(string strSQL, bool IsNeedCopy)
        {
            try
            {
                while (IsLockStationTable)
                {
                    Thread.Sleep(10);
                }
                if (IsNeedCopy)
                {
                    return MainStationTable.Copy().Select(strSQL);
                }
                else
                {
                    return MainStationTable.Select(strSQL);
                }
            }
            catch
            {
                return new DataRow[0];
            }
        }

        public static DataRow[] GetCardTableRows(string strSQL, bool IsNeedCopy)
        {
            try
            {
                while (IsLockCardTable)
                {
                    Thread.Sleep(10);
                }
                if (IsNeedCopy)
                {
                    return MainCardTable.Copy().Select(strSQL);
                }
                else
                {
                    return MainCardTable.Select(strSQL);
                }
            }
            catch
            {
                return new DataRow[0];
            }
        }

        public static DataRow[] GetPersonTableRows(string strSQL, bool IsNeedCopy)
        {
            try
            {
                while (IsLockPersonTable)
                {
                    Thread.Sleep(10);
                }
                if (IsNeedCopy)
                {
                    return MainPersonTable.Copy().Select(strSQL);
                }
                else
                {
                    return MainPersonTable.Select(strSQL);
                }
            }
            catch
            {
                return new DataRow[0];
            }
        }

        public static DataRow[] GetSpecalTableRows(string strSQL)
        {
            try
            {
                while (IsLockSpecalTable)
                {
                    Thread.Sleep(10);
                }
                DataTable temp_MainSpecalTable = MainSpecalTable.Copy();
                return temp_MainSpecalTable.Select(strSQL);
            }
            catch
            {
                return new DataRow[0];
            }
        }

        public static DataRow[] GetCollectChannelTableRows(string strSQL)
        {
            try
            {
                while (IsLockCollectChannelTable)
                {
                    Thread.Sleep(10);
                }
                DataTable temp_MainCollectChannelTable = MainCollectChannelTable.Copy();
                return temp_MainCollectChannelTable.Select(strSQL);
            }
            catch
            {
                return new DataRow[0];
            }
        }

        public static void RefreshStationTable()
        {
            IsLockStationTable = true;
            try
            {
                DB_Service.RefreshTable(ref MainStationTable, "select * from StationTable");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsLockStationTable = false;
            }
        }

        public static void RefreshCardTable()
        {
            IsLockCardTable = true;
            try
            {
                DB_Service.RefreshTable(ref MainCardTable, "select * from CardTable");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsLockCardTable = false;
            }
        }

        public static void RefreshPersonTable()
        {
            IsLockPersonTable = true;
            try
            {
                DB_Service.RefreshTable(ref MainPersonTable, "select * from PersonTable");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsLockPersonTable = false;
            }
        }

        public static void RefreshSpecalTable()
        {
            IsLockSpecalTable = true;
            try
            {
                DB_Service.RefreshTable(ref MainSpecalTable, "select * from SpecalTable");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsLockSpecalTable = false;
            }
        }

        public static void RefreshCollectChannelTable()
        {
            IsLockCollectChannelTable = true;
            try
            {
                DB_Service.RefreshTable(ref MainCollectChannelTable, "select * from CollectChannelTable");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsLockCollectChannelTable = false;
            }
        }
    }
}
