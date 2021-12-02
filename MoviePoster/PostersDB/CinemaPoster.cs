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

namespace MoviePoster
{
    public class CinemaPoster
    {
        private List<Movie> movies;
        private List<ComingSoonMovie> fetchedMovies;
        private Form1 mainForm;
        public CinemaPoster(Form1 form1)
        {
            mainForm = form1;
            movies = new List<Movie>();
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
                PopulatePosters(files);
            }
            else
            {
                PopulatePosters(files);
            }
        }

        private void PopulatePosters(String[] files)
        {
            foreach (string filename in files)
            {
                if (Regex.IsMatch(filename, @"\.jpg$|\.png$|\.gif$"))
                {
                    Movie m = new Movie();
                    m.image = new image();
                    m.image.localUrl = filename;
                    movies.Add(m);
                }
            }
        }
        private void OnGetMoviesCompleted()
        {

        }

        private async void FetchNewMovies()
        {
            var client = new System.Net.Http.HttpClient();
            var request = new System.Net.Http.HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://imdb8.p.rapidapi.com/title/get-coming-soon-movies?homeCountry=US&purchaseCountry=US&currentCountry=US"),
                Headers =
            {
                { "x-rapidapi-host", "imdb8.p.rapidapi.com" },
                { "x-rapidapi-key", "GxMkqGPyb5mshlyXjpsAb93WKWM3p1ep7nAjsnvcYn5Bq2lZYD" },
            },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                JToken data = JToken.Parse(body);
                foreach (JToken d in data)
                {
                    Movie movie = new Movie();
                    ComingSoonMovie comingSoon = d.ToObject<ComingSoonMovie>();
                    comingSoon.id = comingSoon.id.Replace("/title/", "");
                    comingSoon.id = comingSoon.id.Replace("/", "");

                    fetchedMovies.Add(comingSoon);
                }
                GetMovies();
            }
        }

        private async void GetMovies()
        {
            var count = 0;
            foreach (ComingSoonMovie c in fetchedMovies)
            {
                if (count >= mainForm.MaxMovies)
                {
                    break;
                }
                var client = new System.Net.Http.HttpClient();
                var request = new System.Net.Http.HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://imdb8.p.rapidapi.com/title/get-details?tconst=" + c.id),
                    Headers =
            {
                { "x-rapidapi-host", "imdb8.p.rapidapi.com" },
                { "x-rapidapi-key", "GxMkqGPyb5mshlyXjpsAb93WKWM3p1ep7nAjsnvcYn5Bq2lZYD" },
            },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(body);
                    Movie movie = data.ToObject<Movie>();
                    movies.Add(movie);
                    StoreMovieLocally(movie);
                }
                count++;
            }
        }
        private void StoreMovieLocally(Movie movie)
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\pics\";
            string saveLocation = directory + movie.title.Trim().Replace(":", "") + ".jpg";

            if (!File.Exists(saveLocation) && movie.image != null && movie.image.url.Length > 0)
            {

                byte[] imageBytes;
                HttpWebRequest imageRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(movie.image.url);
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
                movie.image.localUrl = saveLocation;
            }
        }

       

        public Movie GetRandomPoster()
        {
            Random r = new Random();
            int next = r.Next(movies.Count);
            return movies[next];
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
            movies = new List<Movie>();
        }


    }
}
