namespace MoviePoster
{
    partial class Form1
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
            this.MenuPanel = new System.Windows.Forms.Panel();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.FetchMoviesButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.MovieDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.MenuPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuPanel
            // 
            this.MenuPanel.BackColor = System.Drawing.Color.Transparent;
            this.MenuPanel.Controls.Add(this.SettingsButton);
            this.MenuPanel.Controls.Add(this.FetchMoviesButton);
            this.MenuPanel.Location = new System.Drawing.Point(15, 1026);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(177, 30);
            this.MenuPanel.TabIndex = 3;
            this.MenuPanel.UseWaitCursor = true;
            this.MenuPanel.Visible = false;
            this.MenuPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            this.MenuPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MenuPanel_MouseDown);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Location = new System.Drawing.Point(98, 3);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(75, 23);
            this.SettingsButton.TabIndex = 2;
            this.SettingsButton.Text = "Settings";
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.UseWaitCursor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingButtons_Click);
            // 
            // FetchMoviesButton
            // 
            this.FetchMoviesButton.Location = new System.Drawing.Point(3, 3);
            this.FetchMoviesButton.Name = "FetchMoviesButton";
            this.FetchMoviesButton.Size = new System.Drawing.Size(89, 23);
            this.FetchMoviesButton.TabIndex = 1;
            this.FetchMoviesButton.Text = "Fetch Movies";
            this.FetchMoviesButton.UseVisualStyleBackColor = true;
            this.FetchMoviesButton.UseWaitCursor = true;
            this.FetchMoviesButton.Click += new System.EventHandler(this.FetchMoviesButton_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.MenuPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1352, 1032);
            this.panel1.TabIndex = 1;
            this.panel1.UseWaitCursor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(278, 276);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(779, 501);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.UseWaitCursor = true;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Uighur", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1335, 50);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Coming Soon";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.MovieDescriptionTextBox, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.024155F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.97585F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1335, 1056);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // textBox2
            // 
            this.MovieDescriptionTextBox.BackColor = System.Drawing.Color.Black;
            this.MovieDescriptionTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MovieDescriptionTextBox.Font = new System.Drawing.Font("Microsoft Uighur", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieDescriptionTextBox.ForeColor = System.Drawing.SystemColors.Info;
            this.MovieDescriptionTextBox.Location = new System.Drawing.Point(3, 1006);
            this.MovieDescriptionTextBox.Name = "textBox2";
            this.MovieDescriptionTextBox.Size = new System.Drawing.Size(1329, 47);
            this.MovieDescriptionTextBox.TabIndex = 3;
            this.MovieDescriptionTextBox.Text = "Movie Description here";
            this.MovieDescriptionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MovieDescriptionTextBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1352, 1032);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Cinema Poster";
            this.UseWaitCursor = true;
            this.MenuPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel MenuPanel;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.Button FetchMoviesButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox MovieDescriptionTextBox;
    }
}

