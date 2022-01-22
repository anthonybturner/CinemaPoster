using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

using CinemaPosterApp.MovieTypes;

using CinemaPosterApp.Controllers;
using CinemaPosterApp.Utilities;
using System.Timers;

namespace CinemaPosterApp.PostersDB
{
    public class Poster
    {
        private System.Timers.Timer PosterTimer { get; set; }
        public PosterChangeDelegate PosterChangeDel { get; set; }
        public List<IMDBMovie> movies { get; set; }

        private List<ComingSoonMovie> fetchedMovies { get;  set; }

        private ImdbFetcher fetcher { get; set; }

        public delegate void PosterChangeDelegate(IMDBMovie n);

        public delegate void PostersCompleteDelegate(List<IMDBMovie> MoviesList);
        public PostersCompleteDelegate PostersCompletedDel { get; internal set; }

        public Poster()
        {
          
            movies = new List<IMDBMovie>();
            fetchedMovies = new List<ComingSoonMovie>();
            fetcher = new ImdbFetcher();
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
        }

        public async Task InitPostersAsync()
        {
            string[] MovieXMLList;
            MovieXMLList = Directory.GetFiles(CinemaForm.XMLDirectory, "*.xml", SearchOption.AllDirectories);
            if (MovieXMLList == null || MovieXMLList.Length == 0)
            {//Fetch movies from remote api
              movies = await CinemaPoster.Controllers.IMDBApi.GetMoviesInfoAsync();
            }
            else
            {
              PopulatePosters(MovieXMLList);
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
        private void PopulatePosters(String[] files)
        {
            Serializer sr = new Serializer();
            foreach (string filename in files)
            {
                if (Regex.IsMatch(filename, @"\.xml$"))
                {
                    IMDBMovie m = (IMDBMovie)sr.DeSerializeObject<IMDBMovie>(String.Format(filename));
                    movies.Add(m);
                }
            }
            if(PostersCompletedDel != null)
            {
                PostersCompletedDel.Invoke(movies);
            }
        }

        private void HandlePosterChangevent(object sender, ElapsedEventArgs e)
        {
            if (PosterChangeDel != null)
            {
                IMDBMovie movie = GetRandomPoster();
                if (movie != null)
                {
                    PosterChangeDel.Invoke(movie);
                }
            }
        }

        public IMDBMovie GetRandomPoster()
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

