using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PersonPositionServer.Common;
using PersonPositionServer.StaticService;

namespace PersonPositionServer.View
{
    public partial class FrmDelHardwareHistory : Form
    {
        private MainForm mainform;

        public FrmDelHardwareHistory(MainForm _mainform)
        {
            InitializeComponent();
            this.mainform = _mainform;
        }

        private void FrmDelHardwareHistory_Load(object sender, EventArgs e)
        {
            foreach (DataRow row in BasicData.GetStationTableRows("",true))
            {
                com_Station.Items.Add(row["ID"]);
            }

            if (com_Station.Items.Count > 0)
                com_Station.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int stationID = Convert.ToInt32(com_Station.Text);
            DataRow[] rows = BasicData.GetStationTableRows("ID = " + stationID,true);
            if (rows.Length > 0)
            {
                if (rows[0]["FatherStationID"] != DBNull.Value)
                {
                    int fatherStationID = Convert.ToInt32(rows[0]["FatherStationID"]);
                    this.mainform.hardChannel.Send(CommandFactory.GetDelHistoryDataCommand(fatherStationID, stationID));
                }
                else
                {
                    this.mainform.hardChannel.Send(CommandFactory.GetDelHistoryDataCommand(stationID, stationID));
                }
            }
            MessageBox.Show("清空基站储存历史数据成功！", "清空历史数据");
        }
    }
}