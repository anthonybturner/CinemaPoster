using MoviePoster.MovieTypes;
using MoviePoster.PostersDB;
using MoviePoster.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MoviePoster
{
    public partial class Form1 : Form
    {
        public Int32 MaxMovies = 2;
        public Int32 NewMoviesRefreshTime { get; set; }

        private System.Timers.Timer GetPosterTimer;
        private Bitmap PosterImage, AspectRatioImage, ContentRatingImage, AudienceRatingImage;
        private CinemaPoster CinemaPoster;
        public Int32 PosterRefreshTime { get; set; }
        public System.Timers.Timer CheckNowPlayingTimer { get; private set; }
        public bool NowPlaying { get; private set; }
        public bool NowShowing { get; private set; }
        public string PosterMovieTitle { get; private set; }
        public string IP { get; set; }
        public Dictionary<string, string> ServerIPs { get; set; }
        public String Token { get; set; }

        FullScreen fullScreen;

        public Form1()
        {
            InitializeComponent();
            NewMoviesRefreshTime = 60;
            PosterRefreshTime = 2;
            fullScreen = new FullScreen(this);

            NowShowing = true;
            ServerIPs = new Dictionary<string, string>();
            ServerIPs.Add("PlexNas", "192.168.1.249");
            ServerIPs.Add("PlexShield", "192.168.1.171");
            IP = ServerIPs["PlexNas"];
            Token = "GVef81rrzoTVs1GaNFn4"; //Plex shield token
            Token = "pUhBki4ZxSMM1eeM_Ym6"; //Plex nas token

            CinemaPoster = new CinemaPoster(this);
            _ = CinemaPoster.InitPostersAsync();
        }

        #region NowPlaying
        private void StartCheckNowPlayingTimer()
        {
            CheckNowPlayingTimer = new System.Timers.Timer();
            CheckNowPlayingTimer.Elapsed += new ElapsedEventHandler(HandleCheckNowPlayEvent);
            CheckNowPlayingTimer.Interval = 30000;//TimeSpan.FromMinutes(1).TotalMilliseconds;
            CheckNowPlayingTimer.Enabled = true;
            CheckNowPlayingTimer.Start();
        }
        private void StopCheckNowPlayingTimer()
        {
            CheckNowPlayingTimer.Enabled = false;
            CheckNowPlayingTimer.Stop();
            CheckNowPlayingTimer.Dispose();
        }

        private async void HandleCheckNowPlayEvent(object sender, ElapsedEventArgs e)
        {
            await CheckNowPlaying();
        }

        private async Task CheckNowPlaying()
        {
            LogWriter.WriteLog("Entering CheckNowPlaying():", "");
            MovieTechnical mtech = await Task.Run(() => PlexApi.GetNowPlayingInfo(IP, Token));
            if (mtech.title != null && mtech.title.Length > 0)//Plex server is playing movie
            {
                LogWriter.WriteLog("CheckNowPlaying(): Movie is Playing " + mtech.title, "");

                if (!NowPlaying || this.lblMovieTense.Text == "Now Playing" && mtech.title != PosterMovieTitle)
                {
                    await Task.Run(() => StartNowPlaying(mtech));
                }
            }
            else
            {//Plex server has stopped playing movie, start posters again
                NowPlaying = false;

                if (!NowShowing)
                {
                    StopNowPlaying();
                    LogWriter.WriteLog("Stopping Now Playing", "NowShowing is set to false");
                }
            }
        }
        private async Task StartNowPlaying(MovieTechnical mtech)
        {
            pnlDuration.Visible = true;
            if (!NowPlaying) { StopPosters(); }
            try
            {
                TitleData movie = await Task.Run(() => CinemaPoster.FetchMovie(mtech.title));
                if (movie != null)
                {
                    movie.AspectRatio = Double.Parse(mtech.aspectRatio);
                    movie.duration = mtech.duration;
                    movie.ContentRating = mtech.contentRating;
                    movie.AudienceRatingImage = mtech.audienceRatingImage;
                    movie.AudienceRating = mtech.audienceRating;
                    if (movie.Tagline == null || movie.Tagline.Length == 0)
                    {
                        movie.Tagline = mtech.tagline;
                    }
                    movie.MovieTense = "Now Playing";
                    BeginInvoke((Action)delegate ()
                    {
                        SetPosterInfo(movie);
                    });
                    NowPlaying = true;
                    NowShowing = false;
                    Brightness.SetBrightness(25);
                }
            }
            catch (Exception e)
            {
                LogWriter.WriteLog("StartNowPlaying() failed fetching movie\n" + e.Message, e.InnerException.ToString());
            }
        }
        private void StopNowPlaying()
        {
            StartPosterTimer();
            BeginInvoke((Action)delegate ()
            {
                pnlDuration.Visible = false;
                GetInitialPoster();
            });
            NowShowing = true;
            Brightness.SetBrightness(255);
        }
        #endregion

        #region Posters
        public void GetPoster()
        {
            TitleData movie = CinemaPoster.GetRandomPoster();
            if (movie != null)
            {
                BeginInvoke((Action)delegate ()
                {
                    SetPosterInfo(movie);
                });
            }
        }
        public void GetInitialPoster()
        {
            TitleData movie = CinemaPoster.GetRandomPoster();
            if (movie != null)
            {
                SetPosterInfo(movie);
            }
        }
        public void StartPosters()
        {
            StartPosterTimer();
            StartCheckNowPlayingTimer();
            GetInitialPoster();
        }

        public void StopPosters()
        {
            StopPosterTimer();
            // StopCheckNewMoviesTimer();
        }

        public void RestartPosters()
        {
            StopPosters();
            CinemaPoster.RemovePosters();
            CinemaPoster.InitPostersAsync();
        }

        public void RestartPosters_WithOutFetch()
        {
            StopPosters();
            StartPosters();
        }
        private void StartPosterTimer()
        {
            GetPosterTimer = new System.Timers.Timer();
            GetPosterTimer.Elapsed += new ElapsedEventHandler(HandleGetPosterEvent);
            GetPosterTimer.Interval = TimeSpan.FromMinutes(PosterRefreshTime).TotalMilliseconds;
            GetPosterTimer.Enabled = true;
            GetPosterTimer.Start();
        }
        private void StopPosterTimer()
        {
            GetPosterTimer.Enabled = false;
            GetPosterTimer.Stop();
            GetPosterTimer.Dispose();
        }
        #endregion

        #region Form UI
        public void SetPosterInfo(TitleData movie)
        {
            if (movie != null)
            {
                this.lblMovieTense.Text = movie.MovieTense;
                PosterMovieTitle = movie.Title;
                SetPosterImage(movie);
                SetTagline(movie);
                SetPlot(movie);
                SetDurations(movie);
                SetAspectRatio(movie);
                SetContentRating(movie);
                SetReleaseDate(movie);
                SetAudienceRatingImage(movie);
            }
        }

        private void SetPlot(TitleData movie)
        {
            if (movie.Plot != null && movie.Plot.Length > 0)
            {
                pnlPlot.Visible = true;
                lblPlot.Text = movie.Plot;
            }
            else
            {
                lblPlot.Text = "";
                pnlPlot.Visible = false;
            }
        }

        private void SetTagline(TitleData movie)
        {
            if (movie.Tagline != null && movie.Tagline.Length > 0)
            {
                lblTagline.Text = movie.Tagline;
            }
            else
            {
                if (movie.Title != null && movie.Title.Length > 0)
                {
                    lblTagline.Text = movie.Title;
                }
                else
                {
                    lblTagline.Text = "";
                }
            }
        }

        private void SetPosterImage(TitleData movie)
        {
            if (movie.MovieTense != "Now Playing")
            {
                if (movie.Image == null || movie.Image.Length == 0)
                {
                    string title = movie.Title.Replace(" ", "_");
                    string file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\data\{0}.jpg", title);
                    if (File.Exists(file))
                    {
                        movie.Image = file;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            if (PosterImage != null)
            {
                PosterImage.Dispose();
            }
            if (movie.Image != null && movie.Image.Length > 0 && File.Exists(movie.Image))
            {
                PosterImage = new Bitmap(movie.Image);
                if (PosterImage != null && pboxPoster != null)
                {
                    pboxPoster.Image = PosterImage;
                }
            }
        }

        private void SetAudienceRatingImage(TitleData movie)
        {
            if (movie.AudienceRating != null)
            {
                string file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\images\{0}.png", movie.AudienceRatingImage);
                if (!File.Exists(file))
                {
                    file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\images\{0}.png", "defaultcontentrating");
                }

                if (AudienceRatingImage != null)
                {
                    AudienceRatingImage.Dispose();
                }
                try
                {
                    AudienceRatingImage = new Bitmap(file);
                    pboxAudienceRating.Image = AudienceRatingImage;
                }
                catch (FileNotFoundException e)
                {
                    LogWriter.WriteLog("Audience Rating image not found \n " + e.Message, e.InnerException.ToString());
                }
            }

            if (movie.AudienceRating != null && movie.AudienceRating.Length > 0)
            {
                lblAudienceRating.Text = movie.AudienceRating;
            }
        }
        private void SetContentRating(TitleData movie)
        {
            if (movie.ContentRating != null)
            {
                string file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\images\{0}.png", movie.ContentRating);
                if (!File.Exists(file))
                {
                    file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\images\{0}.png", "defaultcontentrating");
                }

                if (ContentRatingImage != null)
                {
                    ContentRatingImage.Dispose();
                }
                try
                {
                    ContentRatingImage = new Bitmap(file);
                    pboxContentRating.Image = ContentRatingImage;
                }
                catch (FileNotFoundException e)
                {
                    LogWriter.WriteLog("Content Rating image not found \n " + e.Message, e.InnerException.ToString());
                }
            }
        }

        private void SetDurations(TitleData movie)
        {
            if (movie.duration != null && movie.duration.Length > 0)
            {
                TimeSpan span = TimeSpan.FromMilliseconds(Int32.Parse(movie.duration)).Duration();
                DateTime endTime = DateTime.Now.AddHours(span.Hours).AddMinutes(span.Minutes).AddSeconds(span.Seconds);
                pnlDuration.Visible = true;
                lblDuration.Text = span.Hours + "h " + span.Minutes + "m " + span.Seconds + "s";
                lblEndTime.Text = String.Format("{0}:{1}:{2}", endTime.Hour, endTime.Minute, endTime.Second);
            }
        }

        private void SetAspectRatio(TitleData movie)
        {

            string file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\images\{0}.png", movie.AspectRatio.ToString());
            if (!File.Exists(file))
            {
                file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\images\{0}.png", "defaultaspect");
            }
            if (AspectRatioImage != null)
            {
                AspectRatioImage.Dispose();
            }
            try
            {
                AspectRatioImage = new Bitmap(file);
                pboxAspectRatio.Image = AspectRatioImage;
            }
            catch (FileNotFoundException e)
            {
                LogWriter.WriteLog("Aspect Rating image not found \n " + e.Message, e.InnerException.ToString());
            }

        }

        private void SetReleaseDate(TitleData movie)
        {
            if (movie.ReleaseDate != null && movie.ReleaseDate.Length > 0)
            {
                string[] words = movie.ReleaseDate.Split('-');
                var year = Int32.Parse(words[0]);
                var month = Int32.Parse(words[1]);
                var day = Int32.Parse(words[2]);

                DateTime thisDate = new DateTime(year, month, day);
                lblReleaseDate.Text = thisDate.ToString("MMM") + " " + day + " " + year;
                // this.RunTimeMinsBox.Text = thisDate.ToString("MMM") + " " + day + " " + year;
            }
            else if (movie.Year != null && movie.Year.Length > 0)
            {
                this.lblReleaseDate.Text = movie.Year;
            }
            else
            {
                this.lblReleaseDate.Text = "";
            }
        }

        #endregion

        #region EventHandlers
        private void HandleGetPosterEvent(object source, ElapsedEventArgs e)
        {
            GetPoster();
        }
        private void HandleMNewoviesEventHandler(object source, ElapsedEventArgs e)
        {
            RestartPosters();
        }

        private void FetchMoviesButton_Click(object sender, EventArgs e)
        {
            RestartPosters();
        }

        private void SettingButtons_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        #endregion

        public void ShowSettings()
        {
            Form settignsForm = new Settings(this);
            settignsForm.Show(this);
        }

    }
}
