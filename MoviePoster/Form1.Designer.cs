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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MovieDesc = new System.Windows.Forms.TextBox();
            this.AspectRatioTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.RatingsTextBox = new System.Windows.Forms.TextBox();
            this.MenuPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(1352, 1063);
            this.panel1.TabIndex = 1;
            this.panel1.UseWaitCursor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.MovieDesc, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.919662F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.08034F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1352, 1063);
            this.tableLayoutPanel1.TabIndex = 4;
            this.tableLayoutPanel1.UseWaitCursor = true;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
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
            this.textBox1.Size = new System.Drawing.Size(1352, 50);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Coming Soon";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.UseWaitCursor = true;
            // 
            // MovieDesc
            // 
            this.MovieDesc.BackColor = System.Drawing.Color.Black;
            this.MovieDesc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MovieDesc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MovieDesc.Enabled = false;
            this.MovieDesc.Font = new System.Drawing.Font("Microsoft Uighur", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieDesc.ForeColor = System.Drawing.SystemColors.Info;
            this.MovieDesc.Location = new System.Drawing.Point(3, 954);
            this.MovieDesc.Name = "MovieDesc";
            this.MovieDesc.Size = new System.Drawing.Size(1346, 40);
            this.MovieDesc.TabIndex = 4;
            this.MovieDesc.Text = "Movie Description here";
            this.MovieDesc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MovieDesc.UseWaitCursor = true;
            // 
            // AspectRatioTextBox
            // 
            this.AspectRatioTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AspectRatioTextBox.BackColor = System.Drawing.Color.Black;
            this.AspectRatioTextBox.Enabled = false;
            this.AspectRatioTextBox.Font = new System.Drawing.Font("Microsoft Uighur", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AspectRatioTextBox.ForeColor = System.Drawing.SystemColors.Info;
            this.AspectRatioTextBox.Location = new System.Drawing.Point(3, 3);
            this.AspectRatioTextBox.Multiline = true;
            this.AspectRatioTextBox.Name = "AspectRatioTextBox";
            this.AspectRatioTextBox.Size = new System.Drawing.Size(729, 47);
            this.AspectRatioTextBox.TabIndex = 3;
            this.AspectRatioTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.AspectRatioTextBox.UseWaitCursor = true;
            this.AspectRatioTextBox.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Location = new System.Drawing.Point(0, 55);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1352, 871);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.UseWaitCursor = true;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.44283F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.55717F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 325F));
            this.tableLayoutPanel2.Controls.Add(this.AspectRatioTextBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.RatingsTextBox, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 1007);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1346, 53);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // RatingsTextBox
            // 
            this.RatingsTextBox.BackColor = System.Drawing.Color.Black;
            this.RatingsTextBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.RatingsTextBox.Enabled = false;
            this.RatingsTextBox.Font = new System.Drawing.Font("Microsoft Uighur", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RatingsTextBox.ForeColor = System.Drawing.SystemColors.Info;
            this.RatingsTextBox.Location = new System.Drawing.Point(1023, 3);
            this.RatingsTextBox.Multiline = true;
            this.RatingsTextBox.Name = "RatingsTextBox";
            this.RatingsTextBox.Size = new System.Drawing.Size(320, 47);
            this.RatingsTextBox.TabIndex = 4;
            this.RatingsTextBox.Text = "Ratings";
            this.RatingsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RatingsTextBox.UseWaitCursor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1352, 1063);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Cinema Poster";
            this.UseWaitCursor = true;
            this.MenuPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
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
        private System.Windows.Forms.TextBox MovieDesc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox RatingsTextBox;
        private System.Windows.Forms.TextBox AspectRatioTextBox;
    }
}

