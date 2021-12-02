using MoviePoster.PostersDB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Form1()
        {
            InitializeComponent();
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


        public void SetPosterImage(String image)
        {
            if (image != null)
            {
                if (PosterImage != null)
                {
                    PosterImage.Dispose();
                }
                PosterImage = new Bitmap(image);
                pictureBox1.ClientSize = new Size(1080, 1920);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                this.Height = 1920;
                this.Width = 1080;
                panel1.Height = this.Height;
                panel1.Width = this.Width;
                pictureBox1.Image = PosterImage;
            }
        }

        private void HandleMoviesEventHandler(object source, ElapsedEventArgs e)
        {
            GetPoster();
        }

        public void GetPoster()
        {
            String image = cp.GetRandomPoster();
            if (image != null && image.Length > 0 ){
                BeginInvoke((Action)delegate ()
                {
                    SetPosterImage(image);
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
            Form settignsForm = new Settings(this);
            settignsForm.Show(this);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MenuPanel_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
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
    }
}
