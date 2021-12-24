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
using CinemaPosterApp.MovieTypes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CinemaPosterApp.Controllers;
using CinemaPosterApp.Utilities;
using CinemaPoster.Controllers;

namespace CinemaPosterApp.PostersDB
{
    public class Poster
    {
        private List<IMDBMovie> movies;
        private List<ComingSoonMovie> fetchedMovies;
        private CinemaForm mainForm;
        private ImdbFetcher fetcher;
        private OmdbApi OmdbApi;
        public Poster(CinemaForm form1)
        {
            mainForm = form1;
            OmdbApi = new OmdbApi(mainForm);
            movies = new List<IMDBMovie>();
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
            directory = System.IO.Directory.GetCurrentDirectory() + @"\xml\";
            if (!Directory.Exists(directory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(directory);
            }
            directory = System.IO.Directory.GetCurrentDirectory() + @"\images\";
            if (!Directory.Exists(directory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(directory);
            }
            directory = System.IO.Directory.GetCurrentDirectory() + @"\actors\";
            if (!Directory.Exists(directory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(directory);
            }
        }

        public async Task InitPostersAsync()
        {
            Boolean hasMovies = false;
            string[] newMovies = new string[0];
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\xml\";

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
                    IMDBMovie m = sr.DeSerializeObject<IMDBMovie>(String.Format(filename));
                    movies.Add(m);
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

        public void RemovePosters()
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\xml\";
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
                        Logger.WriteLog(e.Message, e.ToString());
                        Thread.Sleep(DelayOnRetry);
                    }
                }
            }
            movies = new List<IMDBMovie>();
        }

        public async Task<IMDBMovie> FetchMovieAsync(string mtitle, MovieTechnical mtech)
        {

            //IMDBMovie movie = await Task.Run(() => OmdbApi.GetMovieAsync(mtitle, false));

            IMDBMovie movie = await Task.Run(() => ImdbFetcher.DownloadMovieAsync(mtitle));
            movie.AspectRatio = mtech.aspectRatio;
            movie.Tagline = movie.Tagline;
            movie.duration = mtech.duration;
            movie.Rated = mtech.contentRating;
            movie.AudienceRatingImage = mtech.audienceRatingImage;
            movie.AudienceRating = mtech.audienceRating;
            movie.VideoResolution = mtech.videoResolution;
            movie.videoFrameRate = mtech.videoFrameRate;
            movie.AudioCodec = mtech.audioCodec;
            movie.VideoCodec = mtech.videoCodec;
            movie.Hdr = mtech.hdr;

            //await Task.Run(() => OmdbApi.DownloadPosterAsync(movie));

            await Task.Run(() => ImdbFetcher.SaveMovie(movie, new Serializer(), new ApiLib("k_u215r302")));


            return movie;
        }

        private async Task FetchNewMoviesAsync()
        {

            List<IMDBMovie> pmMovies = await Task.Run( () => ImdbFetcher.DownloadMostPopularMoviesAsync());
            List<IMDBMovie> csMovies = await Task.Run(() => ImdbFetcher.DownloadComingSoonMoviesAsync());
            movies.AddRange(pmMovies);
            movies.AddRange(csMovies);
            await Task.Run(() => ImdbFetcher.SaveMovies(movies));

        }
    }
}

