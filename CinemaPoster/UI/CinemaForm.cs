using CinemaPoster.Controllers;
using CinemaPosterApp.Utilities;
using IMDbApiLib.Models;
using PostersDB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CinemaPosterApp
{
    public partial class CinemaForm : Form
    {
        public static string LogDirectory = System.IO.Directory.GetCurrentDirectory() + @"\logs\";
        public static string XMLDirectory = System.IO.Directory.GetCurrentDirectory() + @"\xml\";
        public static string ImageDirectory = System.IO.Directory.GetCurrentDirectory() + @"\images\";
        public static string ActorsDirectory = System.IO.Directory.GetCurrentDirectory() + @"\actors\";
        public static string JsonDirectory = System.IO.Directory.GetCurrentDirectory()  + @"\json\";

        private System.Timers.Timer GetPosterTimer;
        private Bitmap PosterImage, AspectRatioImage, ContentRatingImage, AudienceRatingImage,
            VideoResolutionImage, VideoFramerateImage, AudioCodecImage, VideoCodecImage, HdrImage;
        public NowPlaying NowPlaying { get; set; }
        private Poster Poster { get; set; }
        public string PosterMovieTitle { get; private set; }
        public TitleData CurrentPosterMovie { get; private set; }
        public bool IsUsingRemote { get; private set; }

        FullScreen fullScreen;

        public CinemaForm()
        {
            InitializeComponent();
            pboxPoster.LoadCompleted += PictureBoxPoster_LoadCompleted;
            IsUsingRemote = true;
            fullScreen = new FullScreen(this);
             
            Poster = new Poster(this);
            Poster.PosterChangeDel = new Poster.PosterChangeDelegate(PosterChangeNotified);
            Poster.PostersCompletedDel = new Poster.PostersCompleteDelegate(PostersCompletedNotified);
            Poster.InitPostersAsync();
            Poster.StartPosters();

        }

        private void PosterChangeNotified(TitleData movie)
        {
            if (NowPlaying == null || !NowPlaying.IsPlaying())
            {
                SetPosterInfo(movie);
            }
        }

        private void PostersCompletedNotified(List<TitleData> movies)
        {
            NowPlaying = new NowPlaying(this);
            NowPlaying.NowPlayingDel = new NowPlaying.NowPlayingDelegate(NowPlayingNotifiy);
        }

        #region NowPlaying
        public void NowPlayingNotifiy(TitleData movie)
        {
            SetPosterInfo(movie);
        }
        #endregion

        #region Form UI
        public void SetPosterInfo(TitleData movie)
        {
            if (movie != null)
            {
                BeginInvoke((Action)delegate ()
                {
                    PosterMovieTitle = movie.Title;
                    SetTagline(movie);
                    SetPlot(movie);
                    SetMovieTense(movie);
                    SetPosterImage(movie);
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
                    SetStartTIme(movie); 
                });
            }
        }

        private void SetStartTIme(TitleData movie)
        {
            //if (movie.StartTime != null && movie.StartTime.Length > 0)
            //{
            //    lblStartTime.Text = movie.StartTime;
            //    lblStartTime.Visible = true;
            //}
            //else
            //{
            //    lblStartTime.Text = "";
            //    lblStartTime.Visible = false;
            //}
        }
       

        private void SetTitle(TitleData movie)
        {

            if (movie.FullTitle != null && movie.FullTitle.Length > 0)
            {
                //  lblMovieTitle.Visible = true;
                // lblMovieTitle.Text = movie.FullTitle;
            }
            else
            {
                // lblMovieTitle.Visible = false;
                // lblMovieTitle.Text = "";
            }
        }

        private void SetMovieTense(TitleData movie)
        {
            var MovieTense = "";
            if (NowPlaying != null && NowPlaying.CurrentlyPlaying)
            {
                MovieTense = "Now Playing";
            }
            else
            {
                var date = "";
                if (movie.ReleaseDate != null && movie.ReleaseDate.Length > 0)
                {
                    date = movie.ReleaseDate;
                    if (date.Length > 0)
                    {
                        DateTime dt1 = DateTime.Parse(date);
                        DateTime dt2 = DateTime.Now;
                        if (dt1 > dt2)
                        {
                            MovieTense = "Coming Soon";
                        }
                        else
                        {
                            MovieTense = "Theaters Now";
                        }
                    }
                }
                else if (movie.Year != null && movie.Year.Length > 0)
                {
                    date = movie.Year;
                    if (Int32.Parse(date) > DateTime.Now.Year)
                    {
                        MovieTense = "Coming Soon";
                    }
                    else
                    {
                        MovieTense = "Theaters Now";
                    }
                }

            }
            this.lblMovieTense.Text = MovieTense;
        }

        private void SetActors(TitleData movie)
        {

            if (movie.ActorList != null)
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
                var MaxCount = 6;

                for (int i = 0; i < movie.ActorList.Count && i < MaxCount; i++)
                {

                    CurrentPosterMovie = movie;

                    var actorName = movie.ActorList[i].Name;
                    var url = movie.ActorList[i].Image;

                    if (IsUsingRemote)
                    {
                        pboxActors[i].ImageLocation = url;
                    }
                    else
                    {
                        string ActorSaveLocation = CinemaPoster.Utilities.FileNameParser.CreateActorImageDirectory(actorName);
                        if (!File.Exists(ActorSaveLocation))//Check if Actor data exists already
                        {
                            try
                            {
                                System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                                System.Net.WebResponse response = request.GetResponse();
                                System.IO.Stream responseStream = response.GetResponseStream();
                                Bitmap bitmap2 = new Bitmap(responseStream);
                                if (pboxActors[i].Image != null)
                                {
                                    pboxActors[i].Image.Dispose();
                                }
                                pboxActors[i].Image = bitmap2;
                                pboxActors[i].Image.Save(ActorSaveLocation);
                            }
                            catch (Exception e)
                            {
                                Logger.WriteLog(e.Message, "");
                            }
                        }
                        else
                        {
                            pboxActors[i].ImageLocation = ActorSaveLocation;
                        }
                    }
                    lblActorsArr[i].Text = movie.ActorList[i].Name;
                    lblCharsArr[i].Text = " As " + movie.ActorList[i].AsCharacter;
                }
            }
        }

        private void SetRunTime(TitleData movie)
        {
            if (movie.RuntimeStr != null && movie.RuntimeStr.Length > 0)
            {

                lblRuntime.Visible = true;
                lblRuntime.Text = movie.RuntimeStr + " * ";
            }
            else
            {
                lblRuntime.Text = "";
                lblRuntime.Visible = false;
            }
        }

        private void SetGenres(TitleData movie)
        {
            if (movie.Genres != null && movie.Genres.Length > 0)
            {
                lblGenres.Visible = true;
                lblGenres.Text = movie.Genres;
            }
            else
            {
                lblGenres.Text = "";
                lblGenres.Visible = false;
            }
        }


        private void SetHDR(TitleData movie)
        {

         //   SetPosterConttrol(pboxHdrImage, movie.Hdr, "VideoCodecs", HdrImage, "HDR ");
        }

        private void SetVideoCodec(TitleData movie)
        {
           // SetPosterConttrol(pboxVideoCodecImage, movie.VideoCodec, "VideoCodecs", VideoCodecImage, "Video Codec ");
        }

        private void SetAudioCodec(TitleData movie)
        {
          //  SetPosterConttrol(pboxAudioCodecImage, movie.AudioCodec, "AudioCodecs", AudioCodecImage, "Audio Codec ");
        }

        private void SetVideoFramerate(TitleData movie)
        {
            //SetPosterConttrol(pboxVideoResolutionImage, movie.VideoResolution, "video", VideoResolutionImage, "Video Resolution");
        }

        private void SetVideoResolution(TitleData movie)
        {
            //SetPosterConttrol(pboxVideoResolutionImage, movie.VideoResolution, "VideoResolutions", VideoResolutionImage, "Video Resolution");
        }
        private void SetAudienceRatingImage(TitleData movie)
        {
            if (movie.IMDbRating == null || movie.IMDbRating.Length <= 0)
            {
                //  pboxIMDBRating.Visible = false;
                //lblIMDBRating.Visible = false;
                //  lblIMDBRating.Text = "";
                pboxIMDBRating.Visible = false;
            }
            else
            {
                pboxIMDBRating.Visible = true;
                // lblIMDBRating.Visible = true;
                //  lblIMDBRating.Text = ((Double.Parse(movie.IMDbRating) / 10.0) * 100).ToString() + "%";
                var width = Double.Parse(movie.IMDbRating) * 50 / 5;
                pboxIMDBRating.Width = (Int32)width;
                SetPosterConttrol(pboxIMDBRating, "stars", "AudienceRating", AudienceRatingImage, "imdbRating");
            }
        }

        private void SetContentRating(TitleData movie)
        {
            SetPosterConttrol(pboxContentRating, movie.ContentRating, "ContentRatings", ContentRatingImage, "Content Rating");
        }

        private void SetDurations(TitleData movie)
        {
            if (movie.RuntimeMins != null && movie.RuntimeMins.Length > 0)
            {
                string duration = movie.RuntimeMins;
                try
                {
                    TimeSpan span = TimeSpan.FromMilliseconds(Int32.Parse(duration)).Duration();
                    DateTime endTime = DateTime.Now.AddHours(span.Hours).AddMinutes(span.Minutes).AddSeconds(span.Seconds);
                    duration = span.Hours + "h " + span.Minutes + "m " + span.Seconds + "s";

                }
                catch (Exception e)
                {
                    Logger.WriteLog(e.Message, "");
                }
                // lblEndTime.Text = String.Format("{0}:{1}:{2}", endTime.Hour, endTime.Minute, endTime.Second);
                // lblEndTime.Text = duration;
                // pnlDuration.Visible = true;
            }
        }

        private void SetAspectRatio(TitleData movie)
        {
          //  SetPosterConttrol(pboxAspectRatio, movie.AspectRatio, "AspectRatios", AspectRatioImage, "Aspect ratio");
        }

        private void SetReleaseDate(TitleData movie)
        {
            
            if (movie.ReleaseDate != null && movie.ReleaseDate.Length > 0)
            {
                string[] words = movie.ReleaseDate.Split('-');
                if(words.Length == 1)
                {
                   
                    words = movie.ReleaseDate.Split(' ');
                   var year = words[2];
                   var month = words[1];
                   var day = words[0];
                    lblReleaseDate.Text = month + " " + day + " " + year + "*";
                }
                else
                {
                   var year = Int32.Parse(words[0]);
                   var month = Int32.Parse(words[1]);
                   var day = Int32.Parse(words[2]);
                    DateTime thisDate = new DateTime(year, month, day);
                    lblReleaseDate.Text = thisDate.ToString("MMM") + " " + day + " " + year + "*";

                }
            }
            else if (movie.Year != null && movie.Year.Length > 0)
            {
                lblReleaseDate.Visible = true;
                this.lblReleaseDate.Text = movie.Year + "*";
            }
            else
            {
                this.lblReleaseDate.Text = "";
                lblReleaseDate.Visible = false;
            }
        }

        private void SetPosterConttrol(PictureBox pboxImage, string ctrlValue, string dir, Bitmap bImage, string errorName = "")
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


        public void SetPlot(TitleData movie)
        {

            //if (movie.Plot != null && movie.Plot.Length > 0)
            //{
            //    xPos = lblPlot.Location.X;
            //    YPos = lblPlot.Location.Y;

            //    if (MarqueePlotTimer != null)
            //    {
            //        MarqueePlotTimer.Start();
            //    }

            //    lblPlot.Visible = true;
            //    lblPlot.Text = movie.Plot;
            //}
            //else
            //{
            //    lblPlot.Text = "";
            //    lblPlot.Visible = false;
            //}
        }

        private void HandleMarqueePlot_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    BeginInvoke((Action)delegate ()
            //    {
            //        if (lblPlot.Text.Length > 547)
            //        {
            //            if (YPos <= -this.lblPlot.Height - 50)
            //            {
            //                this.lblPlot.Location = new System.Drawing.Point(xPos, 0);
            //                YPos = 0;
            //            }
            //            else
            //            {
            //                this.lblPlot.Location = new System.Drawing.Point(xPos, YPos);
            //                YPos -= 10;
            //            }
            //        }
            //        else
            //        {
            //            this.lblPlot.Location = new System.Drawing.Point(0, 0);
            //        }
            //    });
            //}
            //catch(InvalidOperationException ex)
            //{
            //    Logger.WriteLog(ex.Message, ex.ToString());
            //}
        }

        private void SetTagline(TitleData movie)
        {
            if (movie.Tagline != null && movie.Tagline.Length > 0)
            {
                lblTagline.Visible = true;
                lblTagline.Text = movie.Tagline;
            }
            else
            {
                lblTagline.Text = "";
                lblTagline.Visible = false;
            }
        }

        private void SetPosterImage(TitleData movie)
        {
            var ImageLocation = "";
            CurrentPosterMovie = movie;
            ImageLocation = movie.Image;///Default to using remote imgae location

            if (!IsUsingRemote)
            {
                string ImageSaveLocation = CinemaPoster.Utilities.FileNameParser.CreateImageDirectory(movie.Title);
                if (File.Exists(ImageSaveLocation))//Check if image data exists already
                {//Check if poster image data exists already //TODO change to movie.local image
                    ImageLocation = ImageSaveLocation;
                }
                else
                {
                    if (movie.Image.Contains("omdb"))
                    {
                        ImageLocation = String.Format(movie.Image, OmdbApi.API_KEY);
                    }
                    pboxPoster.WaitOnLoad = false; //Goto async call back to save
                }
            }
            
            if(ImageLocation != null && ImageLocation.Length > 0)
            {
                try
                {
                    if (pboxPoster != null)
                    {
                        pboxPoster.LoadAsync(ImageLocation);
                    }
                }
                catch (Exception e)
                {
                    Logger.WriteLog(e.Message, "");
                }
            }
        }

        private void PictureBoxPoster_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //if (NowPlaying != null && NowPlaying.CurrentMovie != null && !File.Exists(NowPlaying.CurrentMovie.LocalImage))
            //{
            //    pboxPoster.Image.Save(NowPlaying.CurrentMovie.LocalImage);
            //}
            //else if(CurrentPosterMovie != null && !File.Exists(CurrentPosterMovie.Image))//TODO change to movie.local image
            //{
            //    pboxPoster.Image.Save(CurrentPosterMovie.Image);//TODO change to movie.local image
            //}
        }

        #endregion

        private void SettingButtons_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        public void ShowSettings()
        {
            Form settignsForm = new Settings(this);
            settignsForm.Show(this);
        }

    }
}
