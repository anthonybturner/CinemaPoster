using IMDbApiLib;
using CinemaPosterApp.MovieTypes;
using CinemaPosterApp.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IMDbApiLib.Models;
using CinemaPoster.Utilities;
using CinemaPoster.Controllers;

namespace CinemaPosterApp.Controllers
{
    class ImdbFetcher
    {

        public static async Task<IMDBMovie> DownloadMovieAsync(string title)
        {
            IMDBMovie movie = new IMDBMovie();
            var apiLib = new ApiLib("k_u215r302");
            IMDbApiLib.Models.SearchData data = await apiLib.SearchMovieAsync(title.Replace("/", " "));
            if (data != null && data.Results != null)
            {
                foreach (var resultTitle in data.Results)
                {
                    
                    IMDbApiLib.Models.TitleData x = await apiLib.TitleAsync(resultTitle.Id);
                    if (x.Title == null || x.FullTitle == null)  continue;
                    movie.Title = x.Title;
                    movie.FullTitle = x.FullTitle;
                    movie.Year = x.Year;
                    movie.Image = "";
                    movie.Runtime = x.RuntimeMins;
                    movie.RuntimeStr = x.RuntimeStr;
                    movie.Plot = x.Plot;
                    movie.Rated = x.ContentRating;
                    movie.imdbRating = x.IMDbRating;
                    movie.MetacriticRating = x.MetacriticRating;
                    movie.Genres = x.Genres;
                    movie.Director = x.Directors;
                    movie.Actors = x.Stars;
                    movie.Id = x.Id;
                    break;
                }
            }
            return movie;
        }

        public static async Task<List<IMDBMovie>> DownloadComingSoonMoviesAsync()
        {
            List<IMDBMovie> movies = new List<IMDBMovie>();
            var apiLib = new ApiLib("k_u215r302");
            var data = await apiLib.ComingSoonAsync();
            IMDBMovie movie;

            foreach (IMDbApiLib.Models.NewMovieDataDetail x in data.Items)
            {
                movie = new IMDBMovie();
                movie.MovieTense = "COMING SOON";
                movie.Id = x.Id;
                movie.Title = x.Title;
                movie.FullTitle = x.FullTitle;
                movie.Year = x.Year;
                movie.ReleaseState = x.ReleaseState;
                movie.Image = "";
                movie.Runtime = x.RuntimeMins;
                movie.RuntimeStr = x.RuntimeStr;
                movie.Plot = x.Plot;
                movie.Rated = x.ContentRating;
                movie.imdbRating = x.IMDbRating;
                movie.AudienceRating = x.IMDbRating;
                movie.ImdbRatingCount = x.IMDbRatingCount;
                movie.MetacriticRating = x.MetacriticRating;
                movie.Genres = x.Genres;
                movie.Director = x.Directors;
                movie.Actors = x.Stars;
                movies.Add(movie);
            }
            return movies;
        }

        public static async Task<List<IMDBMovie>> DownloadMostPopularMoviesAsync()
        {
            List<IMDBMovie> movies = new List<IMDBMovie>();
            var apiLib = new ApiLib("k_u215r302");
            var data = await apiLib.MostPopularMoviesAsync();
            OmdbApi OmdbApi = new OmdbApi();
            IMDBMovie movie;
            foreach (IMDbApiLib.Models.MostPopularDataDetail x in data.Items)
            {
                
                var IMDBMovie = await apiLib.TitleAsync(x.Id, Language.en, false, false, false, true, false, false, true);
                movie = new IMDBMovie();
                movie.MovieTense = "Theaters Now";
                movie.Id = x.Id;
                movie.Title = x.Title;
                movie.Plot = IMDBMovie.Plot;
                movie.FullTitle = x.FullTitle;
                movie.Year = x.Year;
                movie.Image = "";
                movie.Runtime = IMDBMovie.RuntimeMins;
                movie.RuntimeStr = IMDBMovie.RuntimeStr;
                movie.Released = IMDBMovie.ReleaseDate;
                movie.Tagline = IMDBMovie.Tagline;
                movie.Rated = IMDBMovie.ContentRating;
                movie.AudienceRating = x.IMDbRating;
                movie.imdbRating = x.IMDbRating;
                movie.ImdbRatingCount = x.IMDbRatingCount;
                movie.MetacriticRating = IMDBMovie.MetacriticRating;
                movie.Genres = IMDBMovie.Genres;
                movie.Director = IMDBMovie.Directors;
                movie.Actors = IMDBMovie.Stars;

                IMDBMovie m = await Task.Run(() => OmdbApi.GetMovieAsync(movie.Title, false, "xml"));
                movie.Rated = m.Rated;
                //var datam = await FetchTechSpecs(movie);

                movies.Add(movie);
            }
            return movies;
        }

        public static async Task<List<IMDBMovie>> FetchTechSpecs(IMDBMovie movie)
        {
            List<IMDBMovie> movies = new List<IMDBMovie>();
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
                    Logger.WriteLog(ex.Message, ex.InnerException.ToString());
                }
                movies.Add(movie);
            }
            return movies;
        }

        public static async Task SaveMovie(IMDBMovie movie, Serializer ser, ApiLib apiLib)
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";
            string filename = movie.Title.Replace(": ", "_").Replace("/", "_");
            filename = filename.Replace(" ", "_") + ".jpg";

            string saveLocation = directory + filename;
            movie.LocalImage = saveLocation;
            movie.Image = saveLocation;

            if (!File.Exists(saveLocation))
            {

                var data = await apiLib.PostersAsync(movie.Id);
                PosterDataItem wantedPoster = null;
                foreach (var x in data.Posters)
                {
                    if (wantedPoster == null)
                    {
                        wantedPoster = x;
                        continue;
                    }
                    if (x.Height > wantedPoster.Height && x.Width >= 1440)
                    {
                        wantedPoster = x;
                    }
                }

                if (wantedPoster != null)
                {
                    var url = wantedPoster.Link;
                    byte[] imageBytes;
                    HttpWebRequest imageRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
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
                            ser.SerializeObject<IMDBMovie>(movie, saveLocation);

                        }
                        catch (Exception e)
                        {
                            Logger.WriteLog(e.Message, e.ToString());
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

        public static async Task SaveMovies(List<IMDBMovie> movies)
        {
            Serializer ser = new Serializer();

            var apiLib = new ApiLib("k_u215r302");
            foreach (IMDBMovie movie in movies)
            {
                await SaveMovie(movie, ser, apiLib);
            }
        }
    }
}