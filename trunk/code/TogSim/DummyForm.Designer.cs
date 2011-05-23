namespace Noea.TogSim.Gui.GDI
{
	partial class DummyForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.track22 = new System.Windows.Forms.Label();
            this.track21 = new System.Windows.Forms.Label();
            this.track12 = new System.Windows.Forms.Label();
            this.track11 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.train24 = new System.Windows.Forms.Label();
            this.train23 = new System.Windows.Forms.Label();
            this.train22 = new System.Windows.Forms.Label();
            this.train21 = new System.Windows.Forms.Label();
            this.train14 = new System.Windows.Forms.Label();
            this.train13 = new System.Windows.Forms.Label();
            this.train12 = new System.Windows.Forms.Label();
            this.train11 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.track22);
            this.panel1.Controls.Add(this.track21);
            this.panel1.Controls.Add(this.track12);
            this.panel1.Controls.Add(this.track11);
            this.panel1.Location = new System.Drawing.Point(383, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(308, 348);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // track22
            // 
            this.track22.AutoSize = true;
            this.track22.Location = new System.Drawing.Point(114, 49);
            this.track22.Name = "track22";
            this.track22.Size = new System.Drawing.Size(105, 13);
            this.track22.TabIndex = 3;
            this.track22.Text = "Blocked by train# 01";
            // 
            // track21
            // 
            this.track21.AutoSize = true;
            this.track21.Location = new System.Drawing.Point(27, 49);
            this.track21.Name = "track21";
            this.track21.Size = new System.Drawing.Size(63, 13);
            this.track21.TabIndex = 2;
            this.track21.Text = "Track #001";
            // 
            // track12
            // 
            this.track12.AutoSize = true;
            this.track12.Location = new System.Drawing.Point(114, 36);
            this.track12.Name = "track12";
            this.track12.Size = new System.Drawing.Size(105, 13);
            this.track12.TabIndex = 1;
            this.track12.Text = "Blocked by train# 01";
            // 
            // track11
            // 
            this.track11.AutoSize = true;
            this.track11.Location = new System.Drawing.Point(27, 36);
            this.track11.Name = "track11";
            this.track11.Size = new System.Drawing.Size(63, 13);
            this.track11.TabIndex = 0;
            this.track11.Text = "Track #001";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Lavender;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.train24);
            this.panel2.Controls.Add(this.train23);
            this.panel2.Controls.Add(this.train22);
            this.panel2.Controls.Add(this.train21);
            this.panel2.Controls.Add(this.train14);
            this.panel2.Controls.Add(this.train13);
            this.panel2.Controls.Add(this.train12);
            this.panel2.Controls.Add(this.train11);
            this.panel2.Location = new System.Drawing.Point(27, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(350, 71);
            this.panel2.TabIndex = 1;
            // 
            // train24
            // 
            this.train24.AutoSize = true;
            this.train24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.train24.Location = new System.Drawing.Point(197, 26);
            this.train24.Name = "train24";
            this.train24.Size = new System.Drawing.Size(47, 15);
            this.train24.TabIndex = 7;
            this.train24.Text = "Forward";
            // 
            // train23
            // 
            this.train23.AutoSize = true;
            this.train23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.train23.Location = new System.Drawing.Point(146, 26);
            this.train23.Name = "train23";
            this.train23.Size = new System.Drawing.Size(49, 15);
            this.train23.TabIndex = 6;
            this.train23.Text = "Stopped";
            // 
            // train22
            // 
            this.train22.AutoSize = true;
            this.train22.Location = new System.Drawing.Point(72, 26);
            this.train22.Name = "train22";
            this.train22.Size = new System.Drawing.Size(72, 13);
            this.train22.TabIndex = 5;
            this.train22.Text = "at Track#001";
            // 
            // train21
            // 
            this.train21.AutoSize = true;
            this.train21.Location = new System.Drawing.Point(12, 26);
            this.train21.Name = "train21";
            this.train21.Size = new System.Drawing.Size(53, 13);
            this.train21.TabIndex = 4;
            this.train21.Text = "Train #01";
            // 
            // train14
            // 
            this.train14.AutoSize = true;
            this.train14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.train14.Location = new System.Drawing.Point(197, 10);
            this.train14.Name = "train14";
            this.train14.Size = new System.Drawing.Size(47, 15);
            this.train14.TabIndex = 3;
            this.train14.Text = "Forward";
            // 
            // train13
            // 
            this.train13.AutoSize = true;
            this.train13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.train13.Location = new System.Drawing.Point(146, 10);
            this.train13.Name = "train13";
            this.train13.Size = new System.Drawing.Size(49, 15);
            this.train13.TabIndex = 2;
            this.train13.Text = "Stopped";
            // 
            // train12
            // 
            this.train12.AutoSize = true;
            this.train12.Location = new System.Drawing.Point(72, 10);
            this.train12.Name = "train12";
            this.train12.Size = new System.Drawing.Size(72, 13);
            this.train12.TabIndex = 1;
            this.train12.Text = "at Track#001";
            // 
            // train11
            // 
            this.train11.AutoSize = true;
            this.train11.Location = new System.Drawing.Point(12, 10);
            this.train11.Name = "train11";
            this.train11.Size = new System.Drawing.Size(53, 13);
            this.train11.TabIndex = 0;
            this.train11.Text = "Train #01";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.PapayaWhip;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Location = new System.Drawing.Point(27, 92);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(350, 95);
            this.panel3.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(73, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "Status: Green";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Signal#001";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Snow;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Location = new System.Drawing.Point(27, 193);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(350, 85);
            this.panel4.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Location = new System.Drawing.Point(73, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 15);
            this.label10.TabIndex = 1;
            this.label10.Text = "Value: false";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Sensor#001";
            // 
            // DummyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 371);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "DummyForm";
            this.Text = "Dummy";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label track11;
		private System.Windows.Forms.Label track12;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label train14;
		private System.Windows.Forms.Label train13;
		private System.Windows.Forms.Label train12;
		private System.Windows.Forms.Label train11;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label track22;
		private System.Windows.Forms.Label track21;
		private System.Windows.Forms.Label train24;
		private System.Windows.Forms.Label train23;
		private System.Windows.Forms.Label train22;
		private System.Windows.Forms.Label train21;
	}
}

