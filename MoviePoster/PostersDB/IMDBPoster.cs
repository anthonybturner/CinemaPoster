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

namespace MoviePoster.PostersDB
{
    public class IMDBPoster
    {
        private List<TitleData> movies;
        private List<ComingSoonMovie> fetchedMovies;
        private Form1 mainForm;
        public IMDBPoster(Form1 form1)
        {
            mainForm = form1;
            movies = new List<TitleData>();
            fetchedMovies = new List<ComingSoonMovie>();
        }


        public async void InitPosters()
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";
            if (!Directory.Exists(directory))  // if it doesn't exist, create
                Directory.CreateDirectory(directory);

            var files = Directory.GetFiles(directory, "*.xml", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
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
            movies = new List<TitleData>();
        }
        private async void FetchNewMovies()
        {
            await PopulateMovies();
            Task t = new Task(SaveMovies);
            t.Start();
            await t;
        }

        private async Task PopulateComingSoonMovies()
        {
            var apiLib = new ApiLib("k_u215r302");
            var data = await apiLib.ComingSoonAsync();
            TitleData movie;

            foreach (IMDbApiLib.Models.NewMovieDataDetail x in data.Items)
            {
                movie = new TitleData();
                movie.MovieTense = "COMING SOON";
                movie.Id = x.Id;
                movie.Title = x.Title;
                movie.FullTitle = x.FullTitle;
                movie.Year = x.Year;
                movie.ReleaseState = x.ReleaseState;
                movie.Image = x.Image;
                movie.RuntimeMins = x.RuntimeMins;
                movie.RuntimeStr = x.RuntimeStr;
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
        }

        private async Task PopulateMovies()
        {
            await PopulateMostPopularMovies();
            await PopulateComingSoonMovies();
        }


        private async Task PopulateMostPopularMovies()
        {
            var apiLib = new ApiLib("k_u215r302");
            var data = await apiLib.MostPopularMoviesAsync();

            TitleData movie;
            foreach (IMDbApiLib.Models.MostPopularDataDetail x in data.Items)
            {
                var titleData = await apiLib.TitleAsync(x.Id);

                movie = new TitleData();
                movie.MovieTense = "NOW SHOWING";
                movie.Id = x.Id;
                movie.Title = x.Title;
                movie.FullTitle = x.FullTitle;
                movie.Year = x.Year;
                movie.Image = x.Image;
                movie.RuntimeMins = titleData.RuntimeMins;
                movie.RuntimeStr = titleData.RuntimeStr;
                movie.ReleaseDate = titleData.ReleaseDate;
                movie.Tagline = titleData.Tagline;
                movie.ContentRating = titleData.ContentRating;
                movie.ImdbRating = x.IMDbRating;
                movie.ImdbRatingCount = x.IMDbRatingCount;
                movie.MetacriticRating = titleData.MetacriticRating;
                movie.Genres = titleData.Genres;
                movie.Directors = titleData.Directors;
                movie.Stars = titleData.Stars;
                movies.Add(movie);

            }
        }

        private async void FetchTechSpecs(TitleData movie)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(String.Format("https://imdb8.p.rapidapi.com/title/get-technical?tconst={0}", movie.Id)),
                Headers =
                {
                    { "x-rapidapi-key", "00e3b6603emsh590a064a3e8c494p175df9jsn6aaf9b2c458b" },
                    { "x-rapidapi-host", "imdb8.p.rapidapi.com" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    JToken token = JToken.Parse(body);
                    movie.TechSpecs = token.ToObject<MovieTechnical>();
                }
                catch (Exception ex)
                {
                }
                movies.Add(movie);
            }
        }

        private async void SaveMovies()
        {
            Serializer ser = new Serializer();

            var apiLib = new ApiLib("k_u215r302");
            foreach (TitleData movie in movies)
            {
                string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";
                string filename = movie.Title.Replace(": ", "_");
                filename = filename.Replace(" ", "_") + ".jpg";

                string saveLocation = directory + filename;
                movie.LocalImage = saveLocation;

                if (!File.Exists(saveLocation))
                {

                    var data = await apiLib.PostersAsync(movie.Id);
                    var y = (from x in data.Posters select x).FirstOrDefault();

                    if (y != null)
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
                            saveLocation = saveLocation.Replace(".jpg", ".xml");
                            ser.SerializeObject<TitleData>(movie, saveLocation);
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

    }
}

