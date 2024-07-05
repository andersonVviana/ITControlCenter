namespace ITControlCenter
{
    partial class MenuFrm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuFrm));
            pictureBox1 = new PictureBox();
            btnAD = new Button();
            label1 = new Label();
            btnHI = new Button();
            button1 = new Button();
            button2 = new Button();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(104, 31);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(145, 142);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnAD
            // 
            btnAD.BackColor = Color.FromArgb(245, 243, 240);
            btnAD.FlatAppearance.BorderColor = Color.FromArgb(245, 243, 240);
            btnAD.FlatAppearance.MouseDownBackColor = Color.FromArgb(82, 37, 131);
            btnAD.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 243, 240);
            btnAD.FlatStyle = FlatStyle.Flat;
            btnAD.Image = (Image)resources.GetObject("btnAD.Image");
            btnAD.ImageAlign = ContentAlignment.MiddleRight;
            btnAD.Location = new Point(90, 206);
            btnAD.Name = "btnAD";
            btnAD.Size = new Size(175, 42);
            btnAD.TabIndex = 1;
            btnAD.Text = "Active Directory";
            btnAD.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAD.UseVisualStyleBackColor = false;
            btnAD.Click += btnAD_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(10, 529);
            label1.Name = "label1";
            label1.Size = new Size(159, 15);
            label1.TabIndex = 6;
            label1.Text = "IT Arcade Beauty Brazil | 2024";
            // 
            // btnHI
            // 
            btnHI.BackColor = Color.FromArgb(245, 243, 240);
            btnHI.FlatAppearance.BorderColor = Color.FromArgb(245, 243, 240);
            btnHI.FlatAppearance.MouseDownBackColor = Color.FromArgb(82, 37, 131);
            btnHI.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 243, 240);
            btnHI.FlatStyle = FlatStyle.Flat;
            btnHI.Image = (Image)resources.GetObject("btnHI.Image");
            btnHI.ImageAlign = ContentAlignment.MiddleRight;
            btnHI.Location = new Point(90, 261);
            btnHI.Name = "btnHI";
            btnHI.Size = new Size(175, 42);
            btnHI.TabIndex = 7;
            btnHI.Text = "Hardware Inventory";
            btnHI.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHI.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.FlatAppearance.BorderColor = Color.FromArgb(82, 37, 131);
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(82, 37, 131);
            button1.FlatStyle = FlatStyle.Flat;
            button1.ForeColor = Color.FromArgb(82, 37, 131);
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.Location = new Point(323, 9);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(31, 29);
            button1.TabIndex = 8;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(245, 243, 240);
            button2.FlatAppearance.BorderColor = Color.FromArgb(245, 243, 240);
            button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(82, 37, 131);
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(245, 243, 240);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.ImageAlign = ContentAlignment.MiddleRight;
            button2.Location = new Point(90, 317);
            button2.Name = "button2";
            button2.Size = new Size(175, 42);
            button2.TabIndex = 9;
            button2.Text = "IT Contracts";
            button2.TextImageRelation = TextImageRelation.ImageBeforeText;
            button2.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(285, 529);
            label2.Name = "label2";
            label2.Size = new Size(61, 15);
            label2.TabIndex = 10;
            label2.Text = "V. 1.0 beta";
            // 
            // MenuFrm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(82, 37, 131);
            ClientSize = new Size(364, 550);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(btnHI);
            Controls.Add(label1);
            Controls.Add(btnAD);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MenuFrm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button btnAD;
        private Label label1;
        private Button btnHI;
        private Button button1;
        private Button button2;
        private Label label2;
    }
}
