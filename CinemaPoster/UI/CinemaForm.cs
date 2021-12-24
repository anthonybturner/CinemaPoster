using CinemaPosterApp.MovieTypes;
using CinemaPosterApp.PostersDB;
using CinemaPosterApp.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
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
            Logger.WriteLog("Entering CheckNowPlaying():", "");
            MovieTechnical mtech = await Task.Run(() => PlexApi.GetNowPlayingInfo(appPlexURL, NowPlaying));
            if (!mtech.IsPlaying)
            {
                if (mtech.title != null && mtech.title.Length > 0)//Plex server is playing movie
                {
                    Logger.WriteLog("CheckNowPlaying(): Movie is Playing " + mtech.title, "");

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
                        Logger.WriteLog("Stopping Now Playing", "NowShowing is set to false");
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
                       // pnlDuration.Visible = true;
                        SetPosterInfo(movie);
                    });
                    NowPlaying = true;
                    NowShowing = false;
                    Brightness.SetBrightness(100);
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog("StartNowPlaying() failed fetching movie\n" + e.Message, e.ToString());
            }
        }
        private void StopNowPlaying()
        {
            StartPosterTimer();
            BeginInvoke((Action)delegate ()
            {
                //pnlDuration.Visible = false;
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
                   // if (pnlDuration.Visible)
                   // {
                  //      pnlDuration.Visible = false;
                  //  }
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
            GetPosterTimer.Elapsed += new ElapsedEventHandler(HandleGetPoster);
            GetPosterTimer.Interval = TimeSpan.FromMinutes(PosterRefreshTime).TotalMilliseconds;
            GetPosterTimer.Enabled = true;
            GetPosterTimer.Start();

            MarqueePlotTimer = new System.Timers.Timer();
            MarqueePlotTimer.Elapsed += new ElapsedEventHandler(HandleMarqueePlot_Tick);
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
               
                PosterMovieTitle = movie.Title;
                lblMovieTitle.Text = movie.FullTitle;
                SetMovieTense(movie);
                SetPosterImage(movie);
                SetTagline(movie);
                SetPlot(movie);
                SetDurations(movie);
                SetRunTime(movie);
                SetAspectRatio(movie);
                SetContentRating(movie);
                SetReleaseDate(movie);
                SetAudienceRatingImage(movie);
                SetVideoResolution(movie);
                SetVideoFramerate(movie);
                SetAudioCodec(movie);
                SetVideoCodec(movie);
                SetHDR(movie);
                SetGenres(movie);
                SetActors(movie);
            }
        }

        private void SetMovieTense(IMDBMovie movie)
        {
            if(NowPlaying)
            {
                movie.MovieTense = "Now Playing";
            }
            else if (movie.Released != null && movie.Released.Length > 0)
            {
                DateTime d2 = DateTime.Parse(movie.Released);
                if (d2 > DateTime.Now)
                {
                    movie.MovieTense = "Coming Soon";
                }
                else
                {
                    movie.MovieTense = "Theaters Now";
                }
            }
            this.lblMovieTense.Text = movie.MovieTense;
        }

        private void SetActors(IMDBMovie movie)
        {
            
            var pboxActors = new List<PictureBox>();
            pboxActors.Add(pboxActor1);
            pboxActors.Add(pboxActor2);
            pboxActors.Add(pboxActor3);
            pboxActors.Add(pboxActor4);
            pboxActors.Add(pboxActor5);
            pboxActors.Add(pboxActor6);

            var lblActorsArr = new List<Label>();
            lblActorsArr.Add(lblActor1);
            lblActorsArr.Add(lblActor2);
            lblActorsArr.Add(lblActor3);
            lblActorsArr.Add(lblActor4);
            lblActorsArr.Add(lblActor5);
            lblActorsArr.Add(lblActor6);

            var lblCharsArr = new List<Label>();
            lblCharsArr.Add(lblCharacter1);
            lblCharsArr.Add(lblCharacter2);
            lblCharsArr.Add(lblCharacter3);
            lblCharsArr.Add(lblCharacter4);
            lblCharsArr.Add(lblCharacter5);
            lblCharsArr.Add(lblCharacter6);

            for (int i = 0; i < movie.Cast.Length; i++)
            {
                pboxActors[i].ImageLocation = movie.Cast[i].ActorLocalImage;
                lblActorsArr[i].Text = movie.Cast[i].ActorName;
                lblCharsArr[i].Text = " As " + movie.Cast[i].CharacterName;
            }
        }

        private void SetRunTime(IMDBMovie movie)
        {
            var duration = "";
            if (movie.duration != null && movie.duration.Length > 0)
            {
                duration = movie.duration;
                TimeSpan span = TimeSpan.FromMilliseconds(Int32.Parse(duration)).Duration();
                DateTime endTime = DateTime.Now.AddHours(span.Hours).AddMinutes(span.Minutes).AddSeconds(span.Seconds);
                duration = span.Hours + "h " + span.Minutes + "m " + span.Seconds + "s";

            }
            else if (movie.RuntimeStr != null && movie.RuntimeStr.Length > 0)
            {
                duration = movie.RuntimeStr;
            }

            if (duration.Length > 0)
            {
                lblRuntime.Text = duration;
            }
            else
            {
                lblRuntime.Text = "";
            }
        }

        private void SetGenres(IMDBMovie movie)
        {
            if (movie.Genres != null && movie.Genres.Length > 0)
            {
                lblGenres.Text = movie.Genres;
            }
            else
            {
                lblGenres.Text = "";
            }
        }


        private void SetHDR(IMDBMovie movie)
        {
            
           SetPosterConttrol(pboxHdrImage, movie.Hdr, "VideoCodecs", HdrImage, "HDR ");
        }

        private void SetVideoCodec(IMDBMovie movie)
        {
            SetPosterConttrol(pboxVideoCodecImage, movie.VideoCodec, "VideoCodecs", VideoCodecImage, "Video Codec ");
        }

        private void SetAudioCodec(IMDBMovie movie)
        {
           SetPosterConttrol(pboxAudioCodecImage, movie.AudioCodec, "AudioCodecs", AudioCodecImage, "Audio Codec ");
        }

        private void SetVideoFramerate(IMDBMovie movie)
        {
            //SetPosterConttrol(pboxVideoResolutionImage, movie.VideoResolution, "video", VideoResolutionImage, "Video Resolution");
        }

        private void SetVideoResolution(IMDBMovie movie)
        {
           SetPosterConttrol(pboxVideoResolutionImage, movie.VideoResolution, "VideoResolutions", VideoResolutionImage, "Video Resolution"); 
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
                SetPosterConttrol(pboxIMDBRating, "defaultEmpty", "AudienceRating", AudienceRatingImage, "imdbRating");
            }
        }

        private void SetContentRating(IMDBMovie movie)
        {
            SetPosterConttrol(pboxContentRating, movie.Rated, "ContentRatings", ContentRatingImage, "Content Rating");
        }

        private void SetDurations(IMDBMovie movie)
        {
            if (movie.duration != null && movie.duration.Length > 0){
                string duration = movie.duration;

                TimeSpan span = TimeSpan.FromMilliseconds(Int32.Parse(duration)).Duration();
                DateTime endTime = DateTime.Now.AddHours(span.Hours).AddMinutes(span.Minutes).AddSeconds(span.Seconds);
                duration = span.Hours + "h " + span.Minutes + "m " + span.Seconds + "s";

               // lblEndTime.Text = String.Format("{0}:{1}:{2}", endTime.Hour, endTime.Minute, endTime.Second);
               // lblEndTime.Text = duration;
               // pnlDuration.Visible = true;
            }
        }

        private void SetAspectRatio(IMDBMovie movie)
        {
           SetPosterConttrol(pboxAspectRatio, movie.AspectRatio, "AspectRatios", AspectRatioImage, "Aspect ratio");
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
                lblReleaseDate.Text = thisDate.ToString("MMM") + " " + day + " " + year;
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

        private void SetPosterConttrol(PictureBox pboxImage, string ctrlValue, string dir, Bitmap bImage, string errorName ="")
        {
            pboxImage.Visible = false;
            if (ctrlValue == null || ctrlValue.Length == 0)
            {
                ctrlValue = "defaultEmpty";
            }
            else
            {
                //CinemaPoster.Images.contentRatings.PG-13.png
                //string file = System.IO.Directory.GetCurrentDirectory() + String.Format(@"\images\{0}\{1}.png", dir, ctrlValue);
                Stream file = Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("CinemaPoster.Images.{0}.{1}.png", dir, ctrlValue));

                if (file != null)
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
                        Logger.WriteLog(errorName + " image not found \n " + e.Message, e.ToString());
                    }
                }
            }

        }

        private void pboxVideoResolutionImage_Click(object sender, EventArgs e)
        {

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

        private void HandleMarqueePlot_Tick(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate (){
                if (lblPlot.Text.Length > 547)
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
        private void HandleGetPoster(object source, ElapsedEventArgs e)
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
