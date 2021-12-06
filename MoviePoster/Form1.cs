using MoviePoster.MovieTypes;
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
        public Int32 NewMoviesRefreshTime { get; set; }

        private System.Timers.Timer aTimer, aUpdateNewMoviesTimer;
        private Bitmap PosterImage;
        private IMDBPoster cp;
        public Int32 PosterRefreshTime { get; set; }
        FullScreen fullScreen;

        public Form1()
        {
            InitializeComponent();
            NewMoviesRefreshTime = 60;
            PosterRefreshTime = 1;
            fullScreen = new FullScreen(this);

            cp = new IMDBPoster(this);
            cp.InitPosters();
        }


        private void StartTimer()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(HandleMoviesEventHandler);
            aTimer.Interval = TimeSpan.FromMinutes(PosterRefreshTime).TotalMilliseconds;
        }

        private void StartCheckNewMoviesTimer()
        {
            if (NewMoviesRefreshTime < 1) return;

            aUpdateNewMoviesTimer = new System.Timers.Timer();
            aUpdateNewMoviesTimer.Elapsed += new ElapsedEventHandler(HandleMNewoviesEventHandler);
            //            aUpdateNewMoviesTimer.Interval = TimeSpan.FromMinutes(NewMoviesRefreshTime).TotalMilliseconds;
            aUpdateNewMoviesTimer.Interval = 5000;

            aUpdateNewMoviesTimer.Start();

        }

        private void StopCheckNewMoviesTimer()
        {
            aUpdateNewMoviesTimer.Stop();
            aUpdateNewMoviesTimer.Enabled = false;
            aUpdateNewMoviesTimer.Dispose();
        }

        private void StopTimer()
        {
            aTimer.Enabled = false;
            aTimer.Stop();
            aTimer.Dispose();
        }

        public void StartPosters()
        {
            StartTimer();
            //  StartCheckNewMoviesTimer();
            aTimer.Enabled = true;
            aTimer.Start();
        }

        public void StopPosters()
        {
            StopTimer();
            // StopCheckNewMoviesTimer();
        }

        public void RestartPosters()
        {
            StopPosters();
            cp.RemovePosters();
            cp.InitPosters();
            StartPosters();
        }

        public void RestartPosters_WithOutFetch()
        {
            StopPosters();
            StartPosters();
        }

        public void SetPosterData(TitleData movie)
        {
            if (movie != null && movie.LocalImage != null && movie.LocalImage.Length > 0 && File.Exists(movie.LocalImage))
            {
                if (PosterImage != null)
                {
                    PosterImage.Dispose();
                }
                PosterImage = new Bitmap(movie.LocalImage);
                //pictureBox1.ClientSize = new Size(PosterImage.Width, PosterImage.Height);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                this.Height = 1920;
                this.Width = 1080;
              //  panel1.Height = this.Height;
              //  panel1.Width = this.Width;
                pictureBox1.Image = PosterImage;
                if( movie.Tagline.Length > 0)
                {
                    TaglineLabel.Text = movie.Tagline;

                }
                else
                {
                    TaglineLabel.Text = movie.Title;

                }

                if (movie.TechSpecs != null && movie.TechSpecs.aspectRatios != null)
                {
                    //AspectsRatioBox.Visible = true;

                    foreach (var ratio in movie.TechSpecs.aspectRatios)
                    {

                        if (ratio == "2.35:1")
                        {
                            pictureBox235.Visible = true;
                        }
                        //AR2351.Text += ratio;
                    }
                }
                else
                {
                    pictureBox235.Visible = true;

                }
                if (movie.ImdbRating.Length > 0)
                {
                   RatingsTextBox.Text = movie.ImdbRating + "/10";
                }
                else
                {
                    RatingsTextBox.Text = "0/10??? ";
                }

                this.MovieTense.Text = movie.MovieTense;
                SetReleaseDate(movie);
                SetRuntime(movie);
            }
        }

        private void SetRuntime(TitleData movie)
        {
            if (movie.RuntimeMins != null && movie.RuntimeMins.Length > 0)
            {
               // this.RunTimeMinsBox.Visible = true;
                this.RunTimeMinsBox.Text = "Runtime: " + movie.RuntimeMins + " mins";
            }
            else
            {
                this.RunTimeMinsBox.Text = "Runtime: 90 mins ???";
            }
        }

        private void SetReleaseDate(TitleData movie)
        {
            if (movie.ReleaseDate != null && movie.ReleaseDate.Length > 0)
            {
                //this.AspectsRatioBox.Visible = true;
                string[] words = movie.ReleaseDate.Split('-');

                var year = Int32.Parse(words[0]);
                var month = Int32.Parse(words[1]);
                var day = Int32.Parse(words[2]);

                DateTime thisDate = new DateTime(year, month, day);
                this.ReleaseDateBoxd.Text =  thisDate.ToString("MMM") + " " + day + " " + year;
            }
            else if (movie.Year != null && movie.Year.Length > 0)
            {
                this.ReleaseDateBoxd.Text =  movie.Year;
                // this.AspectsRatioBox.Visible = false;
            }
            else
            {
                this.ReleaseDateBoxd.Text = "January 1 2021 ";

            }
        }

        private void HandleMoviesEventHandler(object source, ElapsedEventArgs e)
        {
            GetPoster();
        }
        private void HandleMNewoviesEventHandler(object source, ElapsedEventArgs e)
        {
            RestartPosters();
        }

        public void GetPoster()
        {
            TitleData movie = cp.GetRandomPoster();
            if (movie != null)
            {
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


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void ShowSettings()
        {
            Form settignsForm = new Settings(this);
            settignsForm.Show(this);
        }

       
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TaglineLabel_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
