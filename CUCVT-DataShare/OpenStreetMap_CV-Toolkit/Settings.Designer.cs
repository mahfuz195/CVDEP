namespace OpenStreetMap_CV_Toolkit
{
    partial class FromSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_server = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_set = new System.Windows.Forms.Button();
            this.bt_cancle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Ip";
            // 
            // textBox_server
            // 
            this.textBox_server.Location = new System.Drawing.Point(70, 11);
            this.textBox_server.Name = "textBox_server";
            this.textBox_server.Size = new System.Drawing.Size(218, 20);
            this.textBox_server.TabIndex = 1;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(70, 37);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(97, 20);
            this.textBox_port.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // bt_set
            // 
            this.bt_set.Location = new System.Drawing.Point(70, 72);
            this.bt_set.Name = "bt_set";
            this.bt_set.Size = new System.Drawing.Size(75, 23);
            this.bt_set.TabIndex = 9;
            this.bt_set.Text = "Set";
            this.bt_set.UseVisualStyleBackColor = true;
            this.bt_set.Click += new System.EventHandler(this.bt_set_Click);
            // 
            // bt_cancle
            // 
            this.bt_cancle.Location = new System.Drawing.Point(151, 72);
            this.bt_cancle.Name = "bt_cancle";
            this.bt_cancle.Size = new System.Drawing.Size(75, 23);
            this.bt_cancle.TabIndex = 10;
            this.bt_cancle.Text = "Cancel";
            this.bt_cancle.UseVisualStyleBackColor = true;
            this.bt_cancle.Click += new System.EventHandler(this.bt_cancle_Click);
            // 
            // FromSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 118);
            this.Controls.Add(this.bt_cancle);
            this.Controls.Add(this.bt_set);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_server);
            this.Controls.Add(this.label1);
            this.Name = "FromSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_server;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_set;
        private System.Windows.Forms.Button bt_cancle;
    }
}