using MoviePoster.PostersDB;
using MoviePoster.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;


namespace MoviePoster
{
    public partial class Form1 : Form
    {
       public Int32 MaxMovies = 2;
       private System.Timers.Timer aTimer;
       private Bitmap PosterImage;
       private IMDBPoster cp;
       public UInt16 PosterRefreshTime = 5000;
       FullScreen fullScreen;

        public Form1()
        {
            InitializeComponent();
            fullScreen = new FullScreen(this);

            cp = new IMDBPoster(this);
            cp.InitPosters();
        }


        private void InitTimer()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(HandleMoviesEventHandler);
            aTimer.Interval = PosterRefreshTime;
        }

        public void StartPosters()
        {
            InitTimer();
            aTimer.Enabled = true;
            aTimer.Start();
        }

        public void StopPosters()
        {
            aTimer.Enabled = false;
            aTimer.Stop();
        }

        public void RestartPosters()
        {
            StopPosters();
            cp.RemovePosters();
            cp.InitPosters();
            aTimer.Dispose();

            StartPosters();
        }


        public void SetPosterData(IMDBMovie movie)
        {
            if (movie != null && movie.LocalImage != null && movie.LocalImage.Length > 0 && File.Exists(movie.LocalImage))
            {
                if (PosterImage != null)
                {
                    PosterImage.Dispose();
                }
                PosterImage = new Bitmap(movie.LocalImage);
                pictureBox1.ClientSize = new Size(1080, 1920);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                this.Height = 1920;
                this.Width = 1080;
                panel1.Height = this.Height;
                panel1.Width = this.Width;
                pictureBox1.Image = PosterImage;
                MovieDesc.Text =  movie.Plot;
                if (movie.TechSpecs.aspectRatios != null)
                {
                    AspectRatioTextBox.Visible = true;

                    foreach (var ratio in movie.TechSpecs.aspectRatios)
                    {
                        
                        AspectRatioTextBox.Text += ratio + " \t | ";
                    }
                }
                else
                {
                    AspectRatioTextBox.Text = "";
                    AspectRatioTextBox.Visible = false;

                }

                if (movie.ImdbRating.Length > 0)
                {
                    RatingsTextBox.Visible = true;
                    RatingsTextBox.Text = movie.ImdbRating;

                }
                else
                {
                    RatingsTextBox.Visible = false;
                }
            }
        }

        private void HandleMoviesEventHandler(object source, ElapsedEventArgs e)
        {
            GetPoster();
        }

        public void GetPoster()
        {
            IMDBMovie movie = cp.GetRandomPoster();
            if (movie != null){
                BeginInvoke((Action)delegate ()
                {
                    SetPosterData(movie);
                });
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FetchMoviesButton_Click(object sender, EventArgs e)
        {
            RestartPosters();
        }

        private void SettingButtons_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MenuPanel_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ToggleMenu();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void ShowSettings()
        {
            Form settignsForm = new Settings(this);
            settignsForm.Show(this);
        }

        public void ToggleMenu()
        {
            if (MenuPanel.Visible)
            {
                MenuPanel.Visible = false;
            }
            else
            {
                MenuPanel.Visible = true;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
