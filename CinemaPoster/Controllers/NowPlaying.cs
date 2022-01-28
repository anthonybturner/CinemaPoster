using CinemaPoster.Utilities;
using CinemaPosterApp;
using IMDbApiLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace CinemaPoster.Controllers
{
    public class NowPlaying
    {
        public CinemaForm form { get; private set; }
        private Dictionary<string, string> ServerIPs { get; set; }
        private System.Timers.Timer NowPlayingTimer { get; set; }
        public String PlexToken { get; set; }
        public NowPlayingDelegate NowPlayingDel { get; set; }
        public Uri appPlexURL { get; private set; }
        private string IP { get; set; }
        public bool CurrentlyPlaying { get; private set; }
        public string currentTitle { get; private set; }
        public TitleData CurrentMovie { get; private set; }

        private const String PLEX_SHIELD = "PlexShield";
        private const String PLEX_NAS = "PlexNas";
        private const String PLEX_SHIELD_TOKEN = "GVef81rrzoTVs1GaNFn4";
        private const String PLEX_NAS_TOKEN = "pUhBki4ZxSMM1eeM_Ym6";

        public delegate void NowPlayingDelegate(TitleData n);


        public NowPlaying(CinemaForm form)
        {

            this.form = form;
            ServerIPs = new Dictionary<string, string>();
            ServerIPs.Add(PLEX_NAS, "192.168.1.249");
            ServerIPs.Add(PLEX_SHIELD, "192.168.1.171");
            IP = ServerIPs[PLEX_NAS];
            PlexToken = PLEX_NAS_TOKEN;

            InitNowPlayingTimer();
            StartTimer();
        }

        private void SetPlexServer(String PlexIP, String Token)
        {
            appPlexURL = new Uri(@String.Format("http://{0}:32400/status/sessions?X-Plex-Token={1}", PlexIP, Token));
        }

        private void InitNowPlayingTimer()
        {
            SetPlexServer(IP, PlexToken);
            NowPlayingTimer = new System.Timers.Timer();
            NowPlayingTimer.Elapsed += new System.Timers.ElapsedEventHandler(HandleNowPlayEvent);
            NowPlayingTimer.Interval = 10000;//TimeSpan.FromMinutes(1).TotalMilliseconds;
        }
        private void StartTimer()
        {
            NowPlayingTimer.Enabled = true;
            NowPlayingTimer.Start();
        }

        private void StopTimer()
        {
            NowPlayingTimer.Enabled = false;
            NowPlayingTimer.Stop();
        }

        private void HandleNowPlayEvent(object sender, ElapsedEventArgs e)
        {
            _ = CheckNowPlayingAsync();
        }

        private async Task CheckNowPlayingAsync()
        {
            XElement element = XElement.Load(appPlexURL.ToString());
            if (element == null || element.Attribute("size").Value == "0")
            {
                CurrentlyPlaying = false;
                CurrentMovie = null;
                return;
            }
            else
            {//Movie is Now Playing

                TitleData movie = CreateNowPlayingInfo(element);
                if (movie != null && NowPlayingDel != null && movie.Title != currentTitle)
                {
                    CurrentMovie = movie;
                    CurrentlyPlaying = true;
                    movie = await GetMovieInfoAsync(movie.Title);
                    NowPlayingDel.Invoke(movie);
                    currentTitle = movie.Title;
                    //     movie.StartTime = DateTime.Now.ToString("h:mm tt");
                }
            }
        }

        private async Task<TitleData> GetMovieInfoAsync(string title)
        {
            if (!CurrentlyPlaying) { return null; }

            var JsonFileLocation = FileNameParser.CreateJsonDirectory(title);
            if (File.Exists(JsonFileLocation))
            {
                using (StreamReader file = File.OpenText(JsonFileLocation))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    Newtonsoft.Json.Linq.JValue o2 = (Newtonsoft.Json.Linq.JValue)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
                    var obj = JsonConvert.DeserializeObject(o2.Value.ToString());
                    return JObject.Parse(obj.ToString()).ToObject<TitleData>();
                }
            }
            else
            {
                //Fetch remote
                return await CinemaPosterServer.GetPosterAsync(title.Replace("/", " "), form.IsUsingRemote);
            }
        }

        public XElement GetInfo()
        {
            var posterPath = appPlexURL.ToString();
            return XElement.Load(posterPath);
        }

        private void StopNowPlaying()
        {

        }

        private TitleData CreateNowPlayingInfo(XElement element)
        {
            TitleData movie = new TitleData();
            var elements = element.Descendants("Video");
            foreach (var attr in elements.Attributes())
            {
                switch (attr.Name.ToString())
                {
                    case "title":
                        movie.Title = attr.Value;
                        break;

                    case "contentRating":
                        movie.ContentRating = attr.Value;
                        break;
                    case "audienceRatingImage":
                        movie.IMDbRating = attr.Value.Substring(attr.Value.LastIndexOf(".") + 1);
                        break;
                    case "tagline":
                        movie.Tagline = attr.Value;
                        break;
                    case "rating":
                        movie.IMDbRating = attr.Value;
                        break;
                    case "audienceRating":
                        //  movie.audienceRating = attr.Value;
                        break;
                    case "year":
                        movie.Year = attr.Value;
                        break;
                    case "summary":
                        movie.Plot = attr.Value;
                        break;
                }
            }

            elements = element.Descendants("Genre");
            foreach (var el in elements)
            {
                movie.Genres += (el.Attribute("tag").Value) + " ";
            }

            try
            {
                List<XElement> els = element.Descendants("Media").ToList();
                foreach (XElement x in els)
                {
                    foreach (var attr in x.Attributes())
                    {
                        switch (attr.Name.ToString())
                        {
                            case "duration":
                                movie.RuntimeMins = attr.Value;
                                break;
                            case "aspectRatio":
                                //  movie.AspectRatio = attr.Value;
                                break;
                            case "bitrate":
                                //  movie.Bit = attr.Value;
                                break;
                            case "videoResolution":
                                //   movie.VideoResolution = attr.Value;
                                break;
                            case "videoFrameRate":
                                //movie.videoFrameRate = attr.Value;
                                break;
                            case "width":
                                //movie.he = attr.Value;
                                break;
                            case "videoCodec":
                                //  movie.VideoCodec = attr.Value;
                                break;
                        }
                    }
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



            return movie;
        }


        public Boolean IsPlaying()
        {
            return CurrentlyPlaying;
        }

    }
}