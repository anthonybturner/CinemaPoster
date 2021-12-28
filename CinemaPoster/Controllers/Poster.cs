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
        public delegate void SaveMovieCompleted(IMDBMovie movie, Int32 index);


        private List<IMDBMovie> movies;
        private List<ComingSoonMovie> fetchedMovies;
        private CinemaForm mainForm;
        private ImdbFetcher fetcher;

        public string LogDirectory { get; private set; }
        public string XMLDirectory { get; private set; }
        public string ImageDirecgtory { get; private set; }
        public string ActorsDirectory { get; private set; }

        public Poster()
        {
            movies = new List<IMDBMovie>();
            fetchedMovies = new List<ComingSoonMovie>();
            fetcher = new ImdbFetcher();
            CreateDirectories();
        }

        public Poster(CinemaForm form)
        {
            mainForm = form;
            movies = new List<IMDBMovie>();
            fetchedMovies = new List<ComingSoonMovie>();
            fetcher = new ImdbFetcher();
            CreateDirectories();
        }

        private void CreateDirectories()
        {
            LogDirectory = System.IO.Directory.GetCurrentDirectory() + @"\logs\";
            if (!Directory.Exists(LogDirectory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(LogDirectory);
            }
            XMLDirectory = System.IO.Directory.GetCurrentDirectory() + @"\xml\";
            if (!Directory.Exists(XMLDirectory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(XMLDirectory);
            }
            ImageDirecgtory = System.IO.Directory.GetCurrentDirectory() + @"\images\";
            if (!Directory.Exists(ImageDirecgtory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(ImageDirecgtory);
            }
            ActorsDirectory = System.IO.Directory.GetCurrentDirectory() + @"\actors\";
            if (!Directory.Exists(ActorsDirectory))
            {  // if it doesn't exist, create
                Directory.CreateDirectory(ActorsDirectory);
            }
        }

        public async Task InitPostersAsync()
        {
            string[] MovieXMLList;
            MovieXMLList = Directory.GetFiles(XMLDirectory, "*.xml", SearchOption.AllDirectories);
            if (MovieXMLList != null && MovieXMLList.Length > 0)
            {
                PopulatePosters(MovieXMLList);
            }
            else
            {
                SaveMovieCompleted callback = OnMovieSaved;
                await ImdbFetcher.FetchNewMoviesAsync(callback);
            }
        }

        public void OnMovieSaved(IMDBMovie movie, Int32 count)
        {

            movies.Add(movie);
            if (mainForm != null && count == 1)
            {
                mainForm.SetInitialPoster(movie);
            }
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
            mainForm.SetInitialPoster(GetRandomPoster());
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

       
    }
}

