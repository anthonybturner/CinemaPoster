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
using Newtonsoft.Json.Linq;
using IMDbApiLib;

namespace MoviePoster.PostersDB
{
    public class IMDBPoster
    {
        private List<IMDBMovie> movies;
        private List<ComingSoonMovie> fetchedMovies;
        private Form1 mainForm;
        public IMDBPoster(Form1 form1)
        {
            mainForm = form1;
            movies = new List<IMDBMovie>();
            fetchedMovies = new List<ComingSoonMovie>();
        }


        public async void InitPosters()
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\pics\";
            if (!Directory.Exists(directory))  // if it doesn't exist, create
                Directory.CreateDirectory(directory);

            var files = Directory.GetFiles(directory, "*.jpg*", SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                Task t = new Task(FetchNewMovies);
                t.Start();
                await t;
            }

            PopulatePosters(files);
            mainForm.StartPosters();
            
        }

        private void PopulatePosters(String[] files)
        {
            foreach (string filename in files)
            {
                if (Regex.IsMatch(filename, @"\.jpg$|\.png$|\.gif$"))
                {
                    IMDBMovie m = new IMDBMovie();
                    m.LocalImage = filename;
                    movies.Add(m);
                }
            }
        }
        private void OnGetMoviesCompleted()
        {

        }

        private async void FetchNewMovies()
        {

            var apiLib = new ApiLib("k_u215r302");
            var data = await apiLib.ComingSoonAsync();
            IMDBMovie movie;

            foreach (IMDbApiLib.Models.NewMovieDataDetail x in data.Items)
            {
                movie = new IMDBMovie();
                movie.Id = x.Id;
                movie.Title = x.Title;
                movie.FullTitle = x.FullTitle;
                movie.Year = x.Year;
                movie.ReleaseState = x.ReleaseState;
                movie.Image = x.Image;
                movie.RuntimeMins = x.RuntimeMins;
                movie.RuntimeStr  = x.RuntimeStr;
                movie.Plot = x.Plot;
                movie.ContentRating = x.ContentRating;
                movie.ImdbRating = x.IMDbRating;
                movie.ImdbRatingCount = x.IMDbRatingCount;
                movie.MetacriticRating = x.MetacriticRating; 
                movie.Genres = x.Genres;
                movie.Directors = x.Directors;
                movie.Stars = x.Stars;
                movies.Add(movie);
            }

            Task t = new Task(SaveMovies);
            t.Start();
            await t;

        }
      
        private void SaveMovie( IMDBMovie movie)
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\pics\";
            movie.Title = movie.Title.Replace(": ", "_");
            string saveLocation = directory + movie.Title.Replace(" ", "_") + ".jpg";
            movie.LocalImage = saveLocation;

            if (!File.Exists(saveLocation))
            {

                byte[] imageBytes;
                HttpWebRequest imageRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(movie.Image);
                WebResponse imageResponse = imageRequest.GetResponse();
                Stream responseStream = imageResponse.GetResponseStream();

                using (BinaryReader br = new BinaryReader(responseStream))
                {
                    imageBytes = br.ReadBytes(500000);
                    br.Close();
                }
                responseStream.Close();
                imageResponse.Close();
                FileStream fs = new FileStream(saveLocation, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                try
                {
                    bw.Write(imageBytes);
                }
                finally
                {
                    fs.Close();
                    bw.Close();
                }
            }
        }

        private async void SaveMovies()
        {
            var apiLib = new ApiLib("k_u215r302");
            foreach (IMDBMovie movie in movies)
            {
                string directory = System.IO.Directory.GetCurrentDirectory() + @"\pics\";
                movie.Title = movie.Title.Replace(": ", "_");
                string saveLocation = directory + movie.Title.Replace(" ", "_") + ".jpg";
                movie.LocalImage = saveLocation;

                if (!File.Exists(saveLocation))
                {
                 
                    var data = await apiLib.PostersAsync(movie.Id);
                    var y = (from x in data.Posters select x).FirstOrDefault();
                    
                    if(y != null)
                    {
                        byte[] imageBytes;
                        HttpWebRequest imageRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(y.Link);
                        WebResponse imageResponse = imageRequest.GetResponse();
                        Stream responseStream = imageResponse.GetResponseStream();

                        using (BinaryReader br = new BinaryReader(responseStream))
                        {
                            imageBytes = br.ReadBytes(500000);
                            br.Close();
                        }
                        responseStream.Close();
                        imageResponse.Close();
                        FileStream fs = new FileStream(saveLocation, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        try
                        {
                            bw.Write(imageBytes);
                        }
                        finally
                        {
                            fs.Close();
                            bw.Close();
                        }
                    }
                }
            }
        }

        public String GetRandomPoster()
        {
            if (movies.Count == 0) { return ""; }
            Random r = new Random();
            int next = r.Next(movies.Count);
            return movies[next].LocalImage;
        }

        public void RemovePosters()
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\pics\";
            System.IO.DirectoryInfo di = new DirectoryInfo(directory);
            int DelayOnRetry = 1000;
            int NumberOfRetries = 3;

            foreach (FileInfo file in di.GetFiles())
            {
                for (int i = 1; i <= NumberOfRetries; ++i)
                {
                    try
                    {
                        file.Delete();
                        break; // When done we can break loop
                    }
                    catch (IOException e) when (i <= NumberOfRetries)
                    {
                        Thread.Sleep(DelayOnRetry);
                    }
                }
            }
            movies = new List<IMDBMovie>();
        }


    }
}
