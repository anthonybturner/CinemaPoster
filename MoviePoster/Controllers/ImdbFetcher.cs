using IMDbApiLib;
using MoviePoster.MovieTypes;
using MoviePoster.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MoviePoster.Controllers
{
    class ImdbFetcher
    {

        public static async Task<TitleData> DownloadMovieAsync(string title)
        {
            TitleData movie = new TitleData();
            var apiLib = new ApiLib("k_u215r302");
            IMDbApiLib.Models.SearchData data = await apiLib.SearchMovieAsync(title);
            if (data != null)
            {
                var res = (from x in data.Results select x).FirstOrDefault();
                if (res != null)
                {
                    IMDbApiLib.Models.TitleData x = await apiLib.TitleAsync(res.Id);
                    movie.Id = x.Id;
                    movie.Title = x.Title;
                    movie.FullTitle = x.FullTitle;
                    movie.Year = x.Year;
                    movie.Image = "";
                    movie.RuntimeMins = x.RuntimeMins;
                    movie.RuntimeStr = x.RuntimeStr;
                    movie.Plot = x.Plot;
                    movie.ContentRating = x.ContentRating;
                    movie.ImdbRating = x.IMDbRating;
                    movie.MetacriticRating = x.MetacriticRating;
                    movie.Genres = x.Genres;
                    movie.Directors = x.Directors;
                    movie.Stars = x.Stars;
                }
            }
            return movie;
        }

        public static async Task<List<TitleData>> DownloadComingSoonMoviesAsync()
        {
            List<TitleData> movies = new List<TitleData>();
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
                movie.Image = "";
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
            return movies;
        }

        public static async Task<List<TitleData>> DownloadMostPopularMoviesAsync()
        {
            List<TitleData> movies = new List<TitleData>();
            var apiLib = new ApiLib("k_u215r302");
            var data = await apiLib.MostPopularMoviesAsync();

            TitleData movie;
            foreach (IMDbApiLib.Models.MostPopularDataDetail x in data.Items)
            {
                var titleData = await apiLib.TitleAsync(x.Id);

                movie = new TitleData();
                movie.MovieTense = "Theaters Now";
                movie.Id = x.Id;
                movie.Title = x.Title;
                movie.Plot = titleData.Plot;
                movie.FullTitle = x.FullTitle;
                movie.Year = x.Year;
                movie.Image = "";
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
            return movies;
        }

        public static async Task<List<TitleData>> FetchTechSpecs(TitleData movie)
        {
            List<TitleData> movies = new List<TitleData>();
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
                    Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JToken.Parse(body);
                    movie.TechSpecs = token.ToObject<MovieTechnical>();
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLog(ex.Message, ex.InnerException.ToString());
                }
                movies.Add(movie);
            }
            return movies;
        }

        public static async Task SaveMovie(TitleData movie)
        {
            Serializer ser = new Serializer();
            var apiLib = new ApiLib("k_u215r302");
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";
            string filename = movie.Title.Replace(": ", "_");
            filename = filename.Replace(" ", "_") + ".jpg";

            string saveLocation = directory + filename;
            movie.LocalImage = saveLocation;
            movie.Image = saveLocation;

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
                    FileStream fs = null;
                    BinaryWriter bw = null;
                    using (BinaryReader br = new BinaryReader(responseStream))
                    {
                        try
                        {
                            imageBytes = br.ReadBytes(500000);
                            br.Close();
                            responseStream.Close();
                            imageResponse.Close();
                            fs = new FileStream(saveLocation, FileMode.Create);
                            bw = new BinaryWriter(fs);

                            bw.Write(imageBytes);
                            saveLocation = saveLocation.Replace(".jpg", ".xml");
                            ser.SerializeObject<TitleData>(movie, saveLocation);
                        }
                        catch (Exception e)
                        {
                            LogWriter.WriteLog(e.Message, e.InnerException.ToString());
                        }
                        finally
                        {
                            if (fs != null)
                            {
                                fs.Close();
                            }
                            if (bw != null)
                            {
                                bw.Close();
                            }
                        }
                    }

                }
            }

        }

        public static async Task SaveMovies(List<TitleData> movies)
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
                movie.Image = saveLocation;

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
                        FileStream fs = null;
                        BinaryWriter bw = null;
                        using (BinaryReader br = new BinaryReader(responseStream))
                        {
                            try
                            {
                                imageBytes = br.ReadBytes(500000);
                                br.Close();
                                responseStream.Close();
                                imageResponse.Close();
                                fs = new FileStream(saveLocation, FileMode.Create);
                                bw = new BinaryWriter(fs);

                                bw.Write(imageBytes);
                                saveLocation = saveLocation.Replace(".jpg", ".xml");
                                ser.SerializeObject<TitleData>(movie, saveLocation);

                            }
                            catch (Exception e)
                            {
                                LogWriter.WriteLog(e.Message, e.InnerException.ToString());
                                continue;
                            }
                            finally
                            {
                                if (fs != null)
                                {
                                    fs.Close();
                                }
                                if (bw != null)
                                {
                                    bw.Close();
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
