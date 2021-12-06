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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.MovieTense = new System.Windows.Forms.Label();
            this.MovieDesc = new System.Windows.Forms.Label();
            this.RunTimeMinsBox = new System.Windows.Forms.Label();
            this.AspectRatioTextBox = new System.Windows.Forms.Label();
            this.RatingsTextBox = new System.Windows.Forms.Label();
            this.ReleaseDateBox = new System.Windows.Forms.Label();
            this.MovieTitle = new System.Windows.Forms.Label();
            this.MenuPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
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
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.MovieDesc, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.09771F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.90229F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1352, 1063);
            this.tableLayoutPanel1.TabIndex = 4;
            this.tableLayoutPanel1.UseWaitCursor = true;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 126);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1352, 836);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.UseWaitCursor = true;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.24825F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.75175F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 314F));
            this.tableLayoutPanel3.Controls.Add(this.MovieTense, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.RunTimeMinsBox, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.AspectRatioTextBox, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.RatingsTextBox, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.ReleaseDateBox, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.MovieTitle, 1, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1346, 120);
            this.tableLayoutPanel3.TabIndex = 6;
            this.tableLayoutPanel3.UseWaitCursor = true;
            // 
            // MovieTense
            // 
            this.MovieTense.AutoSize = true;
            this.MovieTense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MovieTense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MovieTense.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieTense.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MovieTense.Location = new System.Drawing.Point(408, 0);
            this.MovieTense.Name = "MovieTense";
            this.MovieTense.Size = new System.Drawing.Size(620, 56);
            this.MovieTense.TabIndex = 5;
            this.MovieTense.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MovieDesc
            // 
            this.MovieDesc.AutoSize = true;
            this.MovieDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MovieDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MovieDesc.Location = new System.Drawing.Point(3, 962);
            this.MovieDesc.Name = "MovieDesc";
            this.MovieDesc.Size = new System.Drawing.Size(1346, 93);
            this.MovieDesc.TabIndex = 7;
            this.MovieDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RunTimeMinsBox
            // 
            this.RunTimeMinsBox.AutoSize = true;
            this.RunTimeMinsBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunTimeMinsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunTimeMinsBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RunTimeMinsBox.Location = new System.Drawing.Point(3, 56);
            this.RunTimeMinsBox.Name = "RunTimeMinsBox";
            this.RunTimeMinsBox.Size = new System.Drawing.Size(399, 64);
            this.RunTimeMinsBox.TabIndex = 6;
            // 
            // AspectRatioTextBox
            // 
            this.AspectRatioTextBox.AutoSize = true;
            this.AspectRatioTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AspectRatioTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AspectRatioTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.AspectRatioTextBox.Location = new System.Drawing.Point(1034, 0);
            this.AspectRatioTextBox.Name = "AspectRatioTextBox";
            this.AspectRatioTextBox.Size = new System.Drawing.Size(309, 56);
            this.AspectRatioTextBox.TabIndex = 7;
            this.AspectRatioTextBox.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RatingsTextBox
            // 
            this.RatingsTextBox.AutoSize = true;
            this.RatingsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RatingsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RatingsTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RatingsTextBox.Location = new System.Drawing.Point(1034, 56);
            this.RatingsTextBox.Name = "RatingsTextBox";
            this.RatingsTextBox.Size = new System.Drawing.Size(309, 64);
            this.RatingsTextBox.TabIndex = 8;
            this.RatingsTextBox.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ReleaseDateBox
            // 
            this.ReleaseDateBox.AutoSize = true;
            this.ReleaseDateBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReleaseDateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReleaseDateBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ReleaseDateBox.Location = new System.Drawing.Point(3, 0);
            this.ReleaseDateBox.Name = "ReleaseDateBox";
            this.ReleaseDateBox.Size = new System.Drawing.Size(399, 56);
            this.ReleaseDateBox.TabIndex = 9;
            // 
            // MovieTitle
            // 
            this.MovieTitle.AutoSize = true;
            this.MovieTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MovieTitle.Font = new System.Drawing.Font("Microsoft Yi Baiti", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.MovieTitle.Location = new System.Drawing.Point(408, 56);
            this.MovieTitle.Name = "MovieTitle";
            this.MovieTitle.Size = new System.Drawing.Size(620, 64);
            this.MovieTitle.TabIndex = 10;
            this.MovieTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel MenuPanel;
        private System.Windows.Forms.Button SettingsButton;
        private System.Windows.Forms.Button FetchMoviesButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label MovieTense;
        private System.Windows.Forms.Label RunTimeMinsBox;
        private System.Windows.Forms.Label MovieDesc;
        private System.Windows.Forms.Label AspectRatioTextBox;
        private System.Windows.Forms.Label RatingsTextBox;
        private System.Windows.Forms.Label ReleaseDateBox;
        private System.Windows.Forms.Label MovieTitle;
    }
}

