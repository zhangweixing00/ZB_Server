namespace PersonPositionServer.View
{
    partial class FrmReg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.text_Company = new System.Windows.Forms.TextBox();
            this.text_SN = new System.Windows.Forms.TextBox();
            this.btn_Reg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "公司名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "产品SN";
            // 
            // text_Company
            // 
            this.text_Company.Location = new System.Drawing.Point(59, 11);
            this.text_Company.Name = "text_Company";
            this.text_Company.Size = new System.Drawing.Size(221, 21);
            this.text_Company.TabIndex = 1;
            // 
            // text_SN
            // 
            this.text_SN.Location = new System.Drawing.Point(59, 43);
            this.text_SN.Name = "text_SN";
            this.text_SN.Size = new System.Drawing.Size(221, 21);
            this.text_SN.TabIndex = 2;
            // 
            // btn_Reg
            // 
            this.btn_Reg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Reg.Location = new System.Drawing.Point(204, 77);
            this.btn_Reg.Name = "btn_Reg";
            this.btn_Reg.Size = new System.Drawing.Size(76, 31);
            this.btn_Reg.TabIndex = 3;
            this.btn_Reg.Text = "注册";
            this.btn_Reg.UseVisualStyleBackColor = true;
            this.btn_Reg.Click += new System.EventHandler(this.btn_Reg_Click);
            // 
            // FrmReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(295, 121);
            this.Controls.Add(this.btn_Reg);
            this.Controls.Add(this.text_SN);
            this.Controls.Add(this.text_Company);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmReg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "注册产品";
            this.Load += new System.EventHandler(this.FrmReg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox text_Company;
        private System.Windows.Forms.TextBox text_SN;
        private System.Windows.Forms.Button btn_Reg;
    }
}