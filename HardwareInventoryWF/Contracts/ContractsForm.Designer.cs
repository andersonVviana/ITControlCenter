
namespace HardwareInventoryWF.Contracts
{
    partial class ContractsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractsForm));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.materialButton1 = new FontAwesome.Sharp.Material.MaterialButton();
            this.materialButton2 = new FontAwesome.Sharp.Material.MaterialButton();
            this.materialButton3 = new FontAwesome.Sharp.Material.MaterialButton();
            this.materialButton4 = new FontAwesome.Sharp.Material.MaterialButton();
            this.materialButton5 = new FontAwesome.Sharp.Material.MaterialButton();
            this.materialButton6 = new FontAwesome.Sharp.Material.MaterialButton();
            this.materialButton7 = new FontAwesome.Sharp.Material.MaterialButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1142, 316);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(43, 468);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(1142, 372);
            this.dataGridView2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(31, 124);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1165, 732);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 73);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lato Black", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(100, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 33);
            this.label1.TabIndex = 5;
            this.label1.Text = "Licenses and Contracts";
            // 
            // materialButton1
            // 
            this.materialButton1.FlatAppearance.BorderSize = 0;
            this.materialButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton1.IconChar = FontAwesome.Sharp.MaterialIcons.Plus;
            this.materialButton1.IconColor = System.Drawing.Color.White;
            this.materialButton1.IconSize = 50;
            this.materialButton1.Location = new System.Drawing.Point(1202, 124);
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.Size = new System.Drawing.Size(66, 67);
            this.materialButton1.TabIndex = 6;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // materialButton2
            // 
            this.materialButton2.FlatAppearance.BorderSize = 0;
            this.materialButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton2.IconChar = FontAwesome.Sharp.MaterialIcons.Plus;
            this.materialButton2.IconColor = System.Drawing.Color.White;
            this.materialButton2.IconSize = 50;
            this.materialButton2.Location = new System.Drawing.Point(1202, 468);
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.Size = new System.Drawing.Size(66, 67);
            this.materialButton2.TabIndex = 7;
            this.materialButton2.UseVisualStyleBackColor = true;
            this.materialButton2.Click += new System.EventHandler(this.materialButton2_Click);
            // 
            // materialButton3
            // 
            this.materialButton3.FlatAppearance.BorderSize = 0;
            this.materialButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton3.IconChar = FontAwesome.Sharp.MaterialIcons.Delete;
            this.materialButton3.IconColor = System.Drawing.Color.White;
            this.materialButton3.IconSize = 50;
            this.materialButton3.Location = new System.Drawing.Point(1202, 270);
            this.materialButton3.Name = "materialButton3";
            this.materialButton3.Size = new System.Drawing.Size(66, 67);
            this.materialButton3.TabIndex = 8;
            this.materialButton3.UseVisualStyleBackColor = true;
            this.materialButton3.Click += new System.EventHandler(this.materialButton3_Click);
            // 
            // materialButton4
            // 
            this.materialButton4.FlatAppearance.BorderSize = 0;
            this.materialButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton4.IconChar = FontAwesome.Sharp.MaterialIcons.Pencil;
            this.materialButton4.IconColor = System.Drawing.Color.White;
            this.materialButton4.IconSize = 50;
            this.materialButton4.Location = new System.Drawing.Point(1202, 197);
            this.materialButton4.Name = "materialButton4";
            this.materialButton4.Size = new System.Drawing.Size(66, 67);
            this.materialButton4.TabIndex = 9;
            this.materialButton4.UseVisualStyleBackColor = true;
            this.materialButton4.Click += new System.EventHandler(this.materialButton4_Click);
            // 
            // materialButton5
            // 
            this.materialButton5.FlatAppearance.BorderSize = 0;
            this.materialButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton5.IconChar = FontAwesome.Sharp.MaterialIcons.Pencil;
            this.materialButton5.IconColor = System.Drawing.Color.White;
            this.materialButton5.IconSize = 50;
            this.materialButton5.Location = new System.Drawing.Point(1202, 541);
            this.materialButton5.Name = "materialButton5";
            this.materialButton5.Size = new System.Drawing.Size(66, 67);
            this.materialButton5.TabIndex = 10;
            this.materialButton5.UseVisualStyleBackColor = true;
            // 
            // materialButton6
            // 
            this.materialButton6.FlatAppearance.BorderSize = 0;
            this.materialButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton6.IconChar = FontAwesome.Sharp.MaterialIcons.Delete;
            this.materialButton6.IconColor = System.Drawing.Color.White;
            this.materialButton6.IconSize = 50;
            this.materialButton6.Location = new System.Drawing.Point(1202, 614);
            this.materialButton6.Name = "materialButton6";
            this.materialButton6.Size = new System.Drawing.Size(66, 67);
            this.materialButton6.TabIndex = 11;
            this.materialButton6.UseVisualStyleBackColor = true;
            this.materialButton6.Click += new System.EventHandler(this.materialButton6_Click);
            // 
            // materialButton7
            // 
            this.materialButton7.BackColor = System.Drawing.Color.Red;
            this.materialButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.materialButton7.IconChar = FontAwesome.Sharp.MaterialIcons.Close;
            this.materialButton7.IconColor = System.Drawing.Color.White;
            this.materialButton7.IconSize = 40;
            this.materialButton7.Location = new System.Drawing.Point(1241, 12);
            this.materialButton7.Name = "materialButton7";
            this.materialButton7.Size = new System.Drawing.Size(35, 33);
            this.materialButton7.TabIndex = 12;
            this.materialButton7.UseVisualStyleBackColor = false;
            this.materialButton7.Click += new System.EventHandler(this.materialButton7_Click);
            // 
            // ContractsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(37)))), ((int)(((byte)(131)))));
            this.ClientSize = new System.Drawing.Size(1288, 868);
            this.Controls.Add(this.materialButton7);
            this.Controls.Add(this.materialButton6);
            this.Controls.Add(this.materialButton5);
            this.Controls.Add(this.materialButton4);
            this.Controls.Add(this.materialButton3);
            this.Controls.Add(this.materialButton2);
            this.Controls.Add(this.materialButton1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ContractsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ContractsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.Material.MaterialButton materialButton1;
        private FontAwesome.Sharp.Material.MaterialButton materialButton2;
        private FontAwesome.Sharp.Material.MaterialButton materialButton3;
        private FontAwesome.Sharp.Material.MaterialButton materialButton4;
        private FontAwesome.Sharp.Material.MaterialButton materialButton5;
        private FontAwesome.Sharp.Material.MaterialButton materialButton6;
        private FontAwesome.Sharp.Material.MaterialButton materialButton7;
    }
}