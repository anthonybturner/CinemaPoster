using CinemaPosterApp.MovieTypes;
using CinemaPosterApp.PostersDB;
using CinemaPosterApp.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CinemaPosterApp
{
    public partial class CinemaForm : Form
    {
        private const String PLEX_SHIELD = "PlexShield";
        private const String PLEX_NAS = "PlexNas";
        private const String PLEX_SHIELD_TOKEN = "GVef81rrzoTVs1GaNFn4";
        private const String PLEX_NAS_TOKEN = "pUhBki4ZxSMM1eeM_Ym6";
        public const Int32 MAX_MOVIES = 2;

        public Int32 NewMoviesRefreshTime { get; set; }

        private System.Timers.Timer GetPosterTimer;
        private Bitmap PosterImage, AspectRatioImage, ContentRatingImage, AudienceRatingImage, 
            VideoResolutionImage, VideoFramerateImage, AudioCodecImage, VideoCodecImage, HdrImage;
        private Poster CinemaPoster;
        public Int32 PosterRefreshTime { get; set; }
        public System.Timers.Timer CheckNowPlayingTimer { get; private set; }
        public bool NowPlaying { get; private set; }
        public bool NowShowing { get; private set; }
        public string PosterMovieTitle { get; private set; }
        public string IP { get; set; }
        public Dictionary<string, string> ServerIPs { get; set; }
      

        public String PlexToken { get; set; }
        public Uri appPlexURL { get; private set; }

        private int xPos = 0, YPos = 0;
        private System.Timers.Timer MarqueePlotTimer;
        FullScreen fullScreen;

        public CinemaForm()
        {
            InitializeComponent();
            NewMoviesRefreshTime = 60;
            PosterRefreshTime = 1;
            fullScreen = new FullScreen(this);

            NowShowing = true;

            ServerIPs = new Dictionary<string, string>();
            ServerIPs.Add(PLEX_NAS, "192.168.1.249");
            ServerIPs.Add(PLEX_SHIELD, "192.168.1.171");

            IP = ServerIPs[PLEX_NAS];
            PlexToken = PLEX_NAS_TOKEN;
            CinemaPoster = new Poster(this);
            _ = CinemaPoster.InitPostersAsync();
        }

        #region NowPlaying
        private void StartCheckNowPlayingTimer()
        {
            SetPlexServer(IP, PlexToken);
            CheckNowPlayingTimer = new System.Timers.Timer();
            CheckNowPlayingTimer.Elapsed += new ElapsedEventHandler(HandleCheckNowPlayEvent);
            CheckNowPlayingTimer.Interval = 15000;//TimeSpan.FromMinutes(1).TotalMilliseconds;
            CheckNowPlayingTimer.Enabled = true;
            CheckNowPlayingTimer.Start();
        }

        private void SetPlexServer(String PlexIP, String Token)
        {
            appPlexURL = new Uri(@String.Format("http://{0}:32400/status/sessions?X-Plex-Token={1}", PlexIP, Token));
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
            MovieTechnical mtech = await Task.Run(() => PlexApi.GetNowPlayingInfo(appPlexURL, NowPlaying));
            if (!mtech.IsPlaying)
            {
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
            
        }
        private async Task StartNowPlaying(MovieTechnical mtech)
        {
            if (!NowPlaying) { StopPosters(); }
            try
            {
                var title = mtech.title + " " + (mtech.year != null ? mtech.year : "");
                IMDBMovie movie = await Task.Run(() => CinemaPoster.FetchMovieAsync(title, mtech));
                if (movie != null)
                {
                    if (movie.Tagline == null || movie.Tagline.Length == 0)
                    {
                        movie.Tagline = mtech.tagline;
                    }
                    movie.MovieTense = "Now Playing";
                    BeginInvoke((Action)delegate ()
                    {
                        pnlDuration.Visible = true;
                        SetPosterInfo(movie);
                    });
                    NowPlaying = true;
                    NowShowing = false;
                    Brightness.SetBrightness(25);
                }
            }
            catch (Exception e)
            {
                LogWriter.WriteLog("StartNowPlaying() failed fetching movie\n" + e.Message, e.ToString());
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
            IMDBMovie movie = CinemaPoster.GetRandomPoster();
            if (movie != null)
            {
                BeginInvoke((Action)delegate ()
                {
                    if (pnlDuration.Visible)
                    {
                        pnlDuration.Visible = false;
                    }
                    SetPosterInfo(movie);
                });
            }
        }
        public void GetInitialPoster()
        {
            IMDBMovie movie = CinemaPoster.GetRandomPoster();
            if (movie != null)
            {
              // SetPosterInfo(movie);
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

            MarqueePlotTimer = new System.Timers.Timer();
            MarqueePlotTimer.Elapsed += new ElapsedEventHandler(MarqueePlot_Tick);
            MarqueePlotTimer.Interval = 800; ;
            MarqueePlotTimer.Enabled = true;
        }
        private void StopPosterTimer()
        {
            GetPosterTimer.Enabled = false;
            GetPosterTimer.Stop();
            GetPosterTimer.Dispose();

            MarqueePlotTimer.Enabled = false;
            MarqueePlotTimer.Stop();
            //MarqueePlotTimer.Dispose();
        }
        #endregion

        #region Form UI
        public void SetPosterInfo(IMDBMovie movie)
        {
            if (movie != null)
            {
                if(movie.MovieTense == null || movie.MovieTense.Length == 0)
                {
                    movie.MovieTense = "Theaters Now";
                }
                this.lblMovieTense.Text = movie.MovieTense;
                PosterMovieTitle = movie.Title;
                lblMovieTitle.Text = movie.FullTitle;
                SetPosterImage(movie);
                SetTagline(movie);
                SetPlot(movie);
                SetDurations(movie);
                SetAspectRatio(movie);
                SetContentRating(movie);
                SetReleaseDate(movie);
                SetAudienceRatingImage(movie);
                SetVideoResolution(movie);
                SetVideoFramerate(movie);
                SetAudioCodec(movie);
                SetVideoCodec(movie);
                SetHDR(movie);

            }
        }

        private void SetHDR(IMDBMovie movie)
        {
            SetPosterConttrol(pboxVideoCodecImage, movie.Hdr, "videoCodecs", HdrImage, "HDR ");
        }

        private void SetVideoCodec(IMDBMovie movie)
        {
            SetPosterConttrol(pboxHdrImage, movie.VideoCodec, "videoCodecs", VideoCodecImage, "Video Codec ");
        }

        private void SetAudioCodec(IMDBMovie movie)
        {
            SetPosterConttrol(pboxAudioCodecImage, movie.AudioCodec, "audio", AudioCodecImage, "Audio Codec ");
        }

        private void SetVideoFramerate(IMDBMovie movie)
        {
            //SetPosterConttrol(pboxVideoResolutionImage, movie.VideoResolution, "video", VideoResolutionImage, "Video Resolution");
        }

        private void SetVideoResolution(IMDBMovie movie)
        {
            SetPosterConttrol(pboxVideoResolutionImage, movie.VideoResolution, "video", VideoResolutionImage, "Video Resolution"); 
        }
        private void SetAudienceRatingImage(IMDBMovie movie)
        {
            if (movie.imdbRating == null || movie.imdbRating.Length <= 0)
            {
                lblIMDBRating.Text = "";
                pboxIMDBRating.Visible = false;
            }
            else {
                lblIMDBRating.Text = ((Double.Parse(movie.imdbRating) / 10.0) * 100).ToString() + "%";
                SetPosterConttrol(pboxIMDBRating, "defaultEmpty", "audienceRating", AudienceRatingImage, "imdbRating");
            }
        }

        private void SetContentRating(IMDBMovie movie)
        {
            SetPosterConttrol(pboxContentRating, movie.Rated, "contentRating", ContentRatingImage, "Content Rating");
        }

        private void SetDurations(IMDBMovie movie)
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

        private void SetAspectRatio(IMDBMovie movie)
        {
            SetPosterConttrol(pboxAspectRatio, movie.AspectRatio, "aspects", AspectRatioImage, "Aspect ratio");
        }

        private void SetReleaseDate(IMDBMovie movie)
        {
            if (movie.Released != null && movie.Released.Length > 0)
            {
                string[] words = movie.Released.Split('-');
                var year = Int32.Parse(words[0]);
                var month = Int32.Parse(words[1]);
                var day = Int32.Parse(words[2]);

                DateTime thisDate = new DateTime(year, month, day);
                // lblMovieTitle.Text = thisDate.ToString("MMM") + " " + day + " " + year;
                // this.RunTimeMinsBox.Text = thisDate.ToString("MMM") + " " + day + " " + year;
            }
            else if (movie.Year != null && movie.Year.Length > 0)
            {
                //this.lblMovieTitle.Text = movie.Year;
            }
            else
            {
                // this.lblMovieTitle.Text = "";
            }
        }

        private void SetPosterConttrol(PictureBox pboxImage, string ctrlValue, string dir, Bitmap bImage, string errorName ="")
        {
            pboxImage.Visible = false;
            if (ctrlValue == null || ctrlValue.Length == 0)
            {
                ctrlValue = "defaultEmpty";
            }
            else
            {
                string file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\images\{0}\{1}.png", dir, ctrlValue);
                if (File.Exists(file))
                {
                    if (bImage != null)
                    {
                        bImage.Dispose();
                    }
                    try
                    {
                        bImage = new Bitmap(file);
                        pboxImage.Image = bImage;
                        pboxImage.Visible = true;
                    }
                    catch (FileNotFoundException e)
                    {
                        LogWriter.WriteLog(errorName + " image not found \n " + e.Message, e.ToString());
                    }
                }
            }

        }

        private void SetPlot(IMDBMovie movie)
        {
            if (movie.Plot != null && movie.Plot.Length > 0)
            {
                xPos = lblPlot.Location.X;
                YPos = lblPlot.Location.Y;
               
                MarqueePlotTimer.Start();
               
                pnlPlot.Visible = true;
                lblPlot.Text = movie.Plot;
            }
            else
            {
                lblPlot.Text = "";
                pnlPlot.Visible = false;
            }
        }

        private void MarqueePlot_Tick(object sender, EventArgs e)
        {
            
                BeginInvoke((Action)delegate (){
                    if (lblPlot.Text.Length > 197)
                    {
                        if (YPos <= -this.pnlPlot.Height -50)
                        {
                            this.lblPlot.Location = new System.Drawing.Point(xPos, 0);
                            YPos = 0;
                        }
                        else{
                            this.lblPlot.Location = new System.Drawing.Point(xPos, YPos);
                            YPos -= 10; 

                        }
                    }
                    else
                    {
                        this.lblPlot.Location = new System.Drawing.Point(0, 0);
                    }
                });
                
            
        }

        private void SetTagline(IMDBMovie movie)
        {
            if (movie.Tagline != null && movie.Tagline.Length > 0)
            {
                lblTagline.Text = movie.Tagline;
            }
            else
            {
                lblTagline.Text = "";
            }
        }

        private void SetPosterImage(IMDBMovie movie)
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
