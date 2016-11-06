namespace PersonPositionServer.View
{
    partial class FrmHardCommand
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
            this.btn_Send = new System.Windows.Forms.Button();
            this.text_Receive = new System.Windows.Forms.TextBox();
            this.text_Sended = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.text_Send = new System.Windows.Forms.TextBox();
            this.btn_ClearReceive = new System.Windows.Forms.LinkLabel();
            this.btn_ClearSend = new System.Windows.Forms.LinkLabel();
            this.check_IsXor = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Send
            // 
            this.btn_Send.Location = new System.Drawing.Point(436, 319);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(75, 23);
            this.btn_Send.TabIndex = 1;
            this.btn_Send.Text = "发送";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // text_Receive
            // 
            this.text_Receive.BackColor = System.Drawing.Color.WhiteSmoke;
            this.text_Receive.Location = new System.Drawing.Point(4, 22);
            this.text_Receive.Multiline = true;
            this.text_Receive.Name = "text_Receive";
            this.text_Receive.ReadOnly = true;
            this.text_Receive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.text_Receive.Size = new System.Drawing.Size(508, 164);
            this.text_Receive.TabIndex = 3;
            this.text_Receive.TextChanged += new System.EventHandler(this.text_Receive_TextChanged);
            // 
            // text_Sended
            // 
            this.text_Sended.BackColor = System.Drawing.Color.WhiteSmoke;
            this.text_Sended.Location = new System.Drawing.Point(4, 210);
            this.text_Sended.Multiline = true;
            this.text_Sended.Name = "text_Sended";
            this.text_Sended.ReadOnly = true;
            this.text_Sended.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.text_Sended.Size = new System.Drawing.Size(508, 88);
            this.text_Sended.TabIndex = 2;
            this.text_Sended.TextChanged += new System.EventHandler(this.text_Sended_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "收到的数据";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "发送的数据";
            // 
            // text_Send
            // 
            this.text_Send.Location = new System.Drawing.Point(5, 320);
            this.text_Send.Name = "text_Send";
            this.text_Send.Size = new System.Drawing.Size(427, 21);
            this.text_Send.TabIndex = 0;
            this.text_Send.TextChanged += new System.EventHandler(this.text_Send_TextChanged);
            this.text_Send.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_Send_KeyPress);
            // 
            // btn_ClearReceive
            // 
            this.btn_ClearReceive.AutoSize = true;
            this.btn_ClearReceive.Location = new System.Drawing.Point(483, 7);
            this.btn_ClearReceive.Name = "btn_ClearReceive";
            this.btn_ClearReceive.Size = new System.Drawing.Size(29, 12);
            this.btn_ClearReceive.TabIndex = 4;
            this.btn_ClearReceive.TabStop = true;
            this.btn_ClearReceive.Text = "清空";
            this.btn_ClearReceive.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btn_ClearReceive_LinkClicked);
            // 
            // btn_ClearSend
            // 
            this.btn_ClearSend.AutoSize = true;
            this.btn_ClearSend.Location = new System.Drawing.Point(482, 195);
            this.btn_ClearSend.Name = "btn_ClearSend";
            this.btn_ClearSend.Size = new System.Drawing.Size(29, 12);
            this.btn_ClearSend.TabIndex = 5;
            this.btn_ClearSend.TabStop = true;
            this.btn_ClearSend.Text = "清空";
            this.btn_ClearSend.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btn_ClearSend_LinkClicked);
            // 
            // check_IsXor
            // 
            this.check_IsXor.AutoSize = true;
            this.check_IsXor.Checked = true;
            this.check_IsXor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.check_IsXor.Location = new System.Drawing.Point(120, 304);
            this.check_IsXor.Name = "check_IsXor";
            this.check_IsXor.Size = new System.Drawing.Size(204, 16);
            this.check_IsXor.TabIndex = 6;
            this.check_IsXor.Text = "自动添加最后一个字节的校验数据";
            this.check_IsXor.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 305);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "发送十六进制数据";
            // 
            // FrmHardCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 345);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.check_IsXor);
            this.Controls.Add(this.btn_ClearSend);
            this.Controls.Add(this.btn_ClearReceive);
            this.Controls.Add(this.text_Send);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.text_Sended);
            this.Controls.Add(this.text_Receive);
            this.Controls.Add(this.btn_Send);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHardCommand";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "硬件命令调试工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox text_Receive;
        private System.Windows.Forms.TextBox text_Sended;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox text_Send;
        private System.Windows.Forms.LinkLabel btn_ClearReceive;
        private System.Windows.Forms.LinkLabel btn_ClearSend;
        private System.Windows.Forms.CheckBox check_IsXor;
        private System.Windows.Forms.Label label3;
    }
}