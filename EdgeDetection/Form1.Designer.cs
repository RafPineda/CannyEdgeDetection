namespace EdgeDetection
{
    partial class CanneyEdge
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
            this.load_Image = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.smoothButton = new System.Windows.Forms.Button();
            this.Edge_Find_Button = new System.Windows.Forms.Button();
            this.LowValue = new System.Windows.Forms.TextBox();
            this.HighValue = new System.Windows.Forms.TextBox();
            this.setThld = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // load_Image
            // 
            this.load_Image.Location = new System.Drawing.Point(12, 37);
            this.load_Image.Name = "load_Image";
            this.load_Image.Size = new System.Drawing.Size(75, 23);
            this.load_Image.TabIndex = 0;
            this.load_Image.Text = "Load Image";
            this.load_Image.UseVisualStyleBackColor = true;
            this.load_Image.Click += new System.EventHandler(this.load_Image_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(164, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(567, 381);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // smoothButton
            // 
            this.smoothButton.Location = new System.Drawing.Point(12, 66);
            this.smoothButton.Name = "smoothButton";
            this.smoothButton.Size = new System.Drawing.Size(75, 23);
            this.smoothButton.TabIndex = 2;
            this.smoothButton.Text = "Smooth";
            this.smoothButton.UseVisualStyleBackColor = true;
            this.smoothButton.Click += new System.EventHandler(this.smoothButton_Click);
            // 
            // Edge_Find_Button
            // 
            this.Edge_Find_Button.Location = new System.Drawing.Point(12, 95);
            this.Edge_Find_Button.Name = "Edge_Find_Button";
            this.Edge_Find_Button.Size = new System.Drawing.Size(75, 23);
            this.Edge_Find_Button.TabIndex = 3;
            this.Edge_Find_Button.Text = "DetectEdge";
            this.Edge_Find_Button.UseVisualStyleBackColor = true;
            this.Edge_Find_Button.Click += new System.EventHandler(this.Edge_Find_Button_Click);
            // 
            // LowValue
            // 
            this.LowValue.Location = new System.Drawing.Point(12, 145);
            this.LowValue.Name = "LowValue";
            this.LowValue.Size = new System.Drawing.Size(54, 20);
            this.LowValue.TabIndex = 4;
            this.LowValue.TabStop = false;
            this.LowValue.Text = "20";
            // 
            // HighValue
            // 
            this.HighValue.Location = new System.Drawing.Point(82, 145);
            this.HighValue.Name = "HighValue";
            this.HighValue.Size = new System.Drawing.Size(54, 20);
            this.HighValue.TabIndex = 5;
            this.HighValue.TabStop = false;
            this.HighValue.Text = "30";
            // 
            // setThld
            // 
            this.setThld.Location = new System.Drawing.Point(33, 171);
            this.setThld.Name = "setThld";
            this.setThld.Size = new System.Drawing.Size(75, 23);
            this.setThld.TabIndex = 6;
            this.setThld.Text = "Set Thld";
            this.setThld.UseVisualStyleBackColor = true;
            this.setThld.Click += new System.EventHandler(this.setThld_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Low Threshold";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "High Threshold";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "*Default Threshold Value:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 259);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "20 for Low";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 281);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "30 for High";
            // 
            // CanneyEdge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 590);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.setThld);
            this.Controls.Add(this.HighValue);
            this.Controls.Add(this.LowValue);
            this.Controls.Add(this.Edge_Find_Button);
            this.Controls.Add(this.smoothButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.load_Image);
            this.Name = "CanneyEdge";
            this.Text = "Canney Edge Detection";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button load_Image;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button smoothButton;
        private System.Windows.Forms.Button Edge_Find_Button;
        private System.Windows.Forms.TextBox LowValue;
        private System.Windows.Forms.TextBox HighValue;
        private System.Windows.Forms.Button setThld;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

