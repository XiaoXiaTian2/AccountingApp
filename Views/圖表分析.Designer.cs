namespace AccountingApp.Views
{
    partial class 圖表分析
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
            this.end_time = new System.Windows.Forms.DateTimePicker();
            this.start_time = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.period = new System.Windows.Forms.ComboBox();
            this.chartType = new System.Windows.Forms.ComboBox();
            this.navbar1 = new AccountingApp.Components.Navbar();
            this.SuspendLayout();
            // 
            // end_time
            // 
            this.end_time.Location = new System.Drawing.Point(386, 20);
            this.end_time.Name = "end_time";
            this.end_time.Size = new System.Drawing.Size(200, 25);
            this.end_time.TabIndex = 11;
            // 
            // start_time
            // 
            this.start_time.Location = new System.Drawing.Point(91, 20);
            this.start_time.Name = "start_time";
            this.start_time.Size = new System.Drawing.Size(200, 25);
            this.start_time.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(965, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 41);
            this.button1.TabIndex = 9;
            this.button1.Text = "查詢";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SendRequest);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 233);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "篩選條件";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "群組條件";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Location = new System.Drawing.Point(34, 251);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(651, 338);
            this.flowLayoutPanel2.TabIndex = 12;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(37, 82);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(648, 138);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Location = new System.Drawing.Point(697, 82);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(448, 507);
            this.flowLayoutPanel3.TabIndex = 16;
            // 
            // period
            // 
            this.period.FormattingEnabled = true;
            this.period.Location = new System.Drawing.Point(638, 20);
            this.period.Name = "period";
            this.period.Size = new System.Drawing.Size(132, 23);
            this.period.TabIndex = 17;
            // 
            // chartType
            // 
            this.chartType.FormattingEnabled = true;
            this.chartType.Location = new System.Drawing.Point(804, 20);
            this.chartType.Name = "chartType";
            this.chartType.Size = new System.Drawing.Size(132, 23);
            this.chartType.TabIndex = 17;
            this.chartType.SelectedValueChanged += new System.EventHandler(this.ChangePeriod);
            // 
            // navbar1
            // 
            this.navbar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.navbar1.Location = new System.Drawing.Point(0, 607);
            this.navbar1.Name = "navbar1";
            this.navbar1.Size = new System.Drawing.Size(1157, 94);
            this.navbar1.TabIndex = 1;
            // 
            // 圖表分析
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 701);
            this.Controls.Add(this.chartType);
            this.Controls.Add(this.period);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.end_time);
            this.Controls.Add(this.start_time);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.navbar1);
            this.Name = "圖表分析";
            this.Text = "圖表分析";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Components.Navbar navbar1;
        private System.Windows.Forms.DateTimePicker end_time;
        private System.Windows.Forms.DateTimePicker start_time;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.ComboBox period;
        private System.Windows.Forms.ComboBox chartType;
    }
}