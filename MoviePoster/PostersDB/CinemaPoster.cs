using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using IMDbApiLib;
using System.Text.Json;
using MoviePoster.MovieTypes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MoviePoster.Controllers;
using MoviePoster.Utilities;

namespace MoviePoster.PostersDB
{
    public class CinemaPoster
    {
        private List<TitleData> movies;
        private List<ComingSoonMovie> fetchedMovies;
        private Form1 mainForm;
        private ImdbFetcher fetcher;
        public CinemaPoster(Form1 form1)
        {
            mainForm = form1;
            movies = new List<TitleData>();
            fetchedMovies = new List<ComingSoonMovie>();
            fetcher = new ImdbFetcher();
            CreateDirectories();
           
        }

        private void CreateDirectories()
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\logs\";
            if (!Directory.Exists(directory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(directory);
            }
            directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";
            if (!Directory.Exists(directory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(directory);
            }
        }

        public async Task InitPostersAsync()
        {
            Boolean hasMovies = false;
            string[] newMovies = new string[0];
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";

            newMovies = Directory.GetFiles(directory, "*.xml", SearchOption.AllDirectories);
            if (newMovies != null && newMovies.Length > 0)
            {
                hasMovies = true;
            }
            
            if (hasMovies)
            {//movies exists on drive
                PopulatePosters(newMovies);
            }
            else
            {
                await FetchNewMoviesAsync();
            }
           mainForm.StartPosters();
        }

        private void PopulatePosters(String[] files)
        {
            Serializer sr = new Serializer();
            foreach (string filename in files)
            {
                if (Regex.IsMatch(filename, @"\.xml$"))
                {
                    TitleData m = sr.DeSerializeObject<TitleData>(String.Format(filename));
                    movies.Add(m);
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

        public void RemovePosters()
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";
            System.IO.DirectoryInfo di = new DirectoryInfo(directory);
            int DelayOnRetry = 1000;
            int NumberOfRetries = 3;

            foreach (FileInfo fileinfo in di.GetFiles())
            {
                for (int i = 1; i <= NumberOfRetries; ++i)
                {
                    try
                    {
                        fileinfo.Delete();
                        break; // When done we can break loop
                    }
                    catch (IOException e) when (i <= NumberOfRetries)
                    {
                        LogWriter.WriteLog(e.Message, e.InnerException.ToString());
                        Thread.Sleep(DelayOnRetry);
                    }
                }
            }
            movies = new List<TitleData>();
        }

        public async Task<TitleData> FetchMovie(string mtitle)
        {

            TitleData movie = await Task.Run(() => ImdbFetcher.DownloadMovieAsync(mtitle));
                              await Task.Run(() => ImdbFetcher.SaveMovie(movie));


            return movie;
        }

        private async Task FetchNewMoviesAsync()
        {
            List<TitleData> pmMovies = await Task.Run( () => ImdbFetcher.DownloadMostPopularMoviesAsync());
            List<TitleData> csMovies = await Task.Run(() => ImdbFetcher.DownloadComingSoonMoviesAsync());
            movies.AddRange(pmMovies);
            movies.AddRange(csMovies);
            await Task.Run(() => ImdbFetcher.SaveMovies(movies));

        }
    }
}

