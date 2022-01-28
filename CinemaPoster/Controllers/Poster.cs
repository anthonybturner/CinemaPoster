using MovieTypes;
using IMDbApiLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using CinemaPosterApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PostersDB
{
    public class Poster
    {
        private System.Timers.Timer PosterTimer { get; set; }
        public PosterChangeDelegate PosterChangeDel { get; set; }
        public List<TitleData> movies { get; set; }



        public delegate void PosterChangeDelegate(TitleData n);

        public delegate void PostersCompleteDelegate(List<TitleData> MoviesList);
        public PostersCompleteDelegate PostersCompletedDel { get; internal set; }
        public CinemaForm form { get; private set; }
        public bool IsUsingRemoteOnly { get;  set; }

        public Poster(CinemaForm form)
        {
            this.form = form;
            IsUsingRemoteOnly = true;
            movies = new List<TitleData>();
            CreateDirectories();            
            InitPosterTimer();
        }

        private void CreateDirectories()
        {
            if (!Directory.Exists(CinemaForm.LogDirectory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(CinemaForm.LogDirectory);
            }
           
            if (!Directory.Exists(CinemaForm.XMLDirectory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(CinemaForm.XMLDirectory);
            }
           
            if (!Directory.Exists(CinemaForm.ImageDirectory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(CinemaForm.ImageDirectory);
            }
           
            if (!Directory.Exists(CinemaForm.ActorsDirectory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(CinemaForm.ActorsDirectory);
            }
            if (!Directory.Exists(CinemaForm.JsonDirectory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(CinemaForm.JsonDirectory);
            }
        }

        public async Task InitPostersAsync()
        {
            if (form.IsUsingRemote)//Only fetch remote files do not save
            {
                movies = await CinemaPoster.Controllers.CinemaPosterServer.GetPostersAsync(true);
            }
            else 
            {//Fetch movies from remote api
                string[] JsonMovieList;
                JsonMovieList = Directory.GetFiles(CinemaForm.JsonDirectory, "*.json", SearchOption.AllDirectories);
                if (JsonMovieList == null || JsonMovieList.Length == 0)
                {
                    movies = await CinemaPoster.Controllers.CinemaPosterServer.GetPostersAsync(false);
                }
                else
                {
                    PopulateLocalPosters(JsonMovieList);
                }
            }
          
            if (PostersCompletedDel != null)
            {
                PostersCompletedDel.Invoke(movies);
            }
        }

        private void InitPosterTimer()
        {
            PosterTimer = new System.Timers.Timer();
            PosterTimer.Elapsed += new System.Timers.ElapsedEventHandler(HandlePosterChangevent);
            PosterTimer.Interval = 30000;//TimeSpan.FromMinutes(1).TotalMilliseconds;
        }
        private void StartTimer()
        {
            PosterTimer.Enabled = true;
            PosterTimer.Start();
        }

        private void StopTimer()
        {
            PosterTimer.Enabled = false;
            PosterTimer.Stop();
        }
        private void PopulateLocalPosters(String[] files)
        {
            Serializer sr = new Serializer();
            foreach (string json in files)
            {
                if (Regex.IsMatch(json, @"\.json$"))
                {
                    using (StreamReader file = File.OpenText(json))
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        Newtonsoft.Json.Linq.JValue obj = (Newtonsoft.Json.Linq.JValue)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
                        TitleData movie = JObject.Parse(obj.ToString()).ToObject<TitleData>();
                        movies.Add(movie);
                    }
                }
            }
            }

        private void HandlePosterChangevent(object sender, ElapsedEventArgs e)
        {
            if (PosterChangeDel != null)
            {
                TitleData movie = GetRandomPoster();
                if (movie != null)
                {
                    PosterChangeDel.Invoke(movie);
                }
            }
        }

        public TitleData GetRandomPoster()
        {
            if (movies.Count == 0) { return null; }
            Random r = new Random();
            int next = r.Next(movies.Count);
            return movies[next];
        }
        public void StartPosters()
        {
              StartTimer();
        }
        public void StopPosters()
        {
            StopTimer();
           
        }
    }
}

