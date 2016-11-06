namespace PersonPositionServer.View
{
    partial class FrmDBRecover
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_CleanHistory = new System.Windows.Forms.Button();
            this.btn_CleanAllAlarm = new System.Windows.Forms.Button();
            this.btn_CleanPerson = new System.Windows.Forms.Button();
            this.btn_CleanDuty = new System.Windows.Forms.Button();
            this.btn_CleanCard = new System.Windows.Forms.Button();
            this.btn_CleanCollectChannel = new System.Windows.Forms.Button();
            this.btn_CleanHistoryCollect = new System.Windows.Forms.Button();
            this.btn_CleanMapArea = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_CleanHistory
            // 
            this.btn_CleanHistory.Location = new System.Drawing.Point(12, 12);
            this.btn_CleanHistory.Name = "btn_CleanHistory";
            this.btn_CleanHistory.Size = new System.Drawing.Size(160, 35);
            this.btn_CleanHistory.TabIndex = 0;
            this.btn_CleanHistory.Text = "清空历史定位数据表";
            this.btn_CleanHistory.UseVisualStyleBackColor = true;
            this.btn_CleanHistory.Click += new System.EventHandler(this.btn_CleanHistory_Click);
            // 
            // btn_CleanAllAlarm
            // 
            this.btn_CleanAllAlarm.Location = new System.Drawing.Point(178, 53);
            this.btn_CleanAllAlarm.Name = "btn_CleanAllAlarm";
            this.btn_CleanAllAlarm.Size = new System.Drawing.Size(160, 35);
            this.btn_CleanAllAlarm.TabIndex = 3;
            this.btn_CleanAllAlarm.Text = "清空所有报警信息表";
            this.btn_CleanAllAlarm.UseVisualStyleBackColor = true;
            this.btn_CleanAllAlarm.Click += new System.EventHandler(this.btn_CleanAllAlarm_Click);
            // 
            // btn_CleanPerson
            // 
            this.btn_CleanPerson.Location = new System.Drawing.Point(178, 135);
            this.btn_CleanPerson.Name = "btn_CleanPerson";
            this.btn_CleanPerson.Size = new System.Drawing.Size(160, 35);
            this.btn_CleanPerson.TabIndex = 7;
            this.btn_CleanPerson.Text = "清空人员表";
            this.btn_CleanPerson.UseVisualStyleBackColor = true;
            this.btn_CleanPerson.Click += new System.EventHandler(this.btn_CleanPerson_Click);
            // 
            // btn_CleanDuty
            // 
            this.btn_CleanDuty.Location = new System.Drawing.Point(12, 53);
            this.btn_CleanDuty.Name = "btn_CleanDuty";
            this.btn_CleanDuty.Size = new System.Drawing.Size(160, 35);
            this.btn_CleanDuty.TabIndex = 2;
            this.btn_CleanDuty.Text = "清空考勤表";
            this.btn_CleanDuty.UseVisualStyleBackColor = true;
            this.btn_CleanDuty.Click += new System.EventHandler(this.btn_CleanDuty_Click);
            // 
            // btn_CleanCard
            // 
            this.btn_CleanCard.Location = new System.Drawing.Point(12, 135);
            this.btn_CleanCard.Name = "btn_CleanCard";
            this.btn_CleanCard.Size = new System.Drawing.Size(160, 35);
            this.btn_CleanCard.TabIndex = 6;
            this.btn_CleanCard.Text = "清空卡片表";
            this.btn_CleanCard.UseVisualStyleBackColor = true;
            this.btn_CleanCard.Click += new System.EventHandler(this.btn_CleanCard_Click);
            // 
            // btn_CleanCollectChannel
            // 
            this.btn_CleanCollectChannel.Location = new System.Drawing.Point(12, 94);
            this.btn_CleanCollectChannel.Name = "btn_CleanCollectChannel";
            this.btn_CleanCollectChannel.Size = new System.Drawing.Size(160, 35);
            this.btn_CleanCollectChannel.TabIndex = 4;
            this.btn_CleanCollectChannel.Text = "清空采集器通道表";
            this.btn_CleanCollectChannel.UseVisualStyleBackColor = true;
            this.btn_CleanCollectChannel.Click += new System.EventHandler(this.btn_CleanCollectChannel_Click);
            // 
            // btn_CleanHistoryCollect
            // 
            this.btn_CleanHistoryCollect.Location = new System.Drawing.Point(178, 12);
            this.btn_CleanHistoryCollect.Name = "btn_CleanHistoryCollect";
            this.btn_CleanHistoryCollect.Size = new System.Drawing.Size(160, 35);
            this.btn_CleanHistoryCollect.TabIndex = 1;
            this.btn_CleanHistoryCollect.Text = "清空历史采集信息表";
            this.btn_CleanHistoryCollect.UseVisualStyleBackColor = true;
            this.btn_CleanHistoryCollect.Click += new System.EventHandler(this.btn_CleanHistoryCollect_Click);
            // 
            // btn_CleanMapArea
            // 
            this.btn_CleanMapArea.Location = new System.Drawing.Point(178, 94);
            this.btn_CleanMapArea.Name = "btn_CleanMapArea";
            this.btn_CleanMapArea.Size = new System.Drawing.Size(160, 35);
            this.btn_CleanMapArea.TabIndex = 5;
            this.btn_CleanMapArea.Text = "清空区域表";
            this.btn_CleanMapArea.UseVisualStyleBackColor = true;
            this.btn_CleanMapArea.Click += new System.EventHandler(this.btn_CleanMapArea_Click);
            // 
            // FrmDBRecover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 182);
            this.Controls.Add(this.btn_CleanMapArea);
            this.Controls.Add(this.btn_CleanHistoryCollect);
            this.Controls.Add(this.btn_CleanCollectChannel);
            this.Controls.Add(this.btn_CleanCard);
            this.Controls.Add(this.btn_CleanDuty);
            this.Controls.Add(this.btn_CleanPerson);
            this.Controls.Add(this.btn_CleanAllAlarm);
            this.Controls.Add(this.btn_CleanHistory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDBRecover";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "清空指定表数据";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_CleanHistory;
        private System.Windows.Forms.Button btn_CleanAllAlarm;
        private System.Windows.Forms.Button btn_CleanPerson;
        private System.Windows.Forms.Button btn_CleanDuty;
        private System.Windows.Forms.Button btn_CleanCard;
        private System.Windows.Forms.Button btn_CleanCollectChannel;
        private System.Windows.Forms.Button btn_CleanHistoryCollect;
        private System.Windows.Forms.Button btn_CleanMapArea;
    }
}