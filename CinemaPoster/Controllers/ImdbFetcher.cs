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
        private static void CreateTitle(TitleData IMDBMovie, IMDBMovie movie)
        {
            movie.Title = IMDBMovie.Title;
            movie.FullTitle = IMDBMovie.FullTitle;
            movie.Year = IMDBMovie.Year;
            movie.Image = "";
            movie.Runtime = IMDBMovie.RuntimeMins;
            movie.RuntimeStr = IMDBMovie.RuntimeStr;
            movie.Plot = IMDBMovie.Plot;
            movie.Rated = IMDBMovie.ContentRating;
            movie.imdbRating = IMDBMovie.IMDbRating;
            movie.MetacriticRating = IMDBMovie.MetacriticRating;
            movie.Genres = IMDBMovie.Genres;
            movie.Director = IMDBMovie.Directors;
            movie.Actors = IMDBMovie.Stars;
            movie.Id = IMDBMovie.Id;
            var MaxCasts = 5;
            if (IMDBMovie.ActorList != null)
            {
                for (var i = 0; i < IMDBMovie.ActorList.Count; i++)
                {
                    if (i > MaxCasts) break;
                    var actor = IMDBMovie.ActorList[i];
                    movie.Cast[i] = new IMDBMovie.TopCast();
                    movie.Cast[i].CharacterName = actor.AsCharacter;
                    movie.Cast[i].ActorName = actor.Name;
                    movie.Cast[i].ActorImage = actor.Image;
                }
            }
        }

        public static async Task<IMDBMovie> FetchMovieAsync(string mtitle, MovieTechnical mtech)
        {
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

            await Task.Run(() => ImdbFetcher.SaveMovie(movie, new Serializer(), new ApiLib("k_u215r302")));
            if (movie.Title == null)
            {
                movie.Title = mtitle;
            }
            return movie;
        }

        public static async Task FetchNewMoviesAsync(PostersDB.Poster.SaveMovieCompleted callback)
        {
            List<IMDBMovie> TempMovies = new List<IMDBMovie>();
            List<IMDBMovie> pmMovies = await Task.Run(() => ImdbFetcher.DownloadMostPopularMoviesAsync());
            List<IMDBMovie> csMovies = await Task.Run(() => ImdbFetcher.DownloadComingSoonMoviesAsync());
            TempMovies.AddRange(pmMovies);
            TempMovies.AddRange(csMovies);
            await Task.Run(() => ImdbFetcher.SaveMovies(TempMovies, callback));
        }

        public static async Task<IMDBMovie> DownloadMovieAsync(string title)
        {
            IMDBMovie movie = new IMDBMovie();
            var apiLib = new ApiLib("k_u215r302");
            IMDbApiLib.Models.SearchData data = await apiLib.SearchMovieAsync(title.Replace("/", " "));
            if (data != null && data.Results != null)
            {
                foreach (var x in data.Results)
                {
                    var IMDBMovie = await apiLib.TitleAsync(x.Id, Language.en, true, false, false, true, false, false, true);
                    if (IMDBMovie.Title == null || IMDBMovie.FullTitle == null) continue;
                    CreateTitle(IMDBMovie, movie);
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
            foreach (IMDbApiLib.Models.NewMovieDataDetail x in data.Items)
            {
                IMDBMovie movie = new IMDBMovie();
                var IMDBMovie = await apiLib.TitleAsync(x.Id, Language.en, true, false, false, true, false, false, true);
                CreateTitle(IMDBMovie, movie);
                movie.MovieTense = "COMING SOON";
                movie.ReleaseState = x.ReleaseState;
                movie.ImdbRatingCount = x.IMDbRatingCount;
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
                
                var IMDBMovie = await apiLib.TitleAsync(x.Id, Language.en, true, false, false, true, false, false, true);
                movie = new IMDBMovie();
                CreateTitle(IMDBMovie, movie);
                movie.MovieTense = "Theaters Now";
                movie.Id = x.Id;
                movie.Title = x.Title;
                movie.FullTitle = x.FullTitle;
                movie.Year = x.Year;
                movie.Image = "";
                movie.AudienceRating = x.IMDbRating;
                movie.imdbRating = x.IMDbRating;
                movie.ImdbRatingCount = x.IMDbRatingCount;
               
                IMDBMovie m = await Task.Run(() => OmdbApi.GetMovieAsync(movie.Title, false, "xml"));
                movie.Rated = m.Rated;
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
           
            await DownloadPosterAsync(movie, apiLib);
            await DownloadActorsAsync(movie, apiLib);
            ser.SerializeObject<IMDBMovie>(movie, movie.Title);
        }

        private static async Task DownloadPosterAsync(IMDBMovie movie, ApiLib apiLib)
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\images\";
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
                    if (x.Height > wantedPoster.Height && x.Width >= 1440  && x.Height <= 2100 && x.Height >= 1080)
                    {
                        wantedPoster = x;
                    }
                }

                if (wantedPoster != null)
                {
                    var url = wantedPoster.Link;
                    byte[] imageBytes;
                    FileStream fs = null;
                    BinaryWriter bw = null;
                    try
                    {
                        HttpWebRequest imageRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                        WebResponse imageResponse = imageRequest.GetResponse();
                        Stream responseStream = imageResponse.GetResponseStream();
                        using (BinaryReader br = new BinaryReader(responseStream))
                        {
                            imageBytes = br.ReadBytes(500000);
                            br.Close();
                            responseStream.Close();
                            imageResponse.Close();
                            fs = new FileStream(saveLocation, FileMode.Create);
                            bw = new BinaryWriter(fs);
                            bw.Write(imageBytes);
                        }
                    }
                    catch(Exception e)
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
        private static async Task DownloadActorsAsync(IMDBMovie movie, ApiLib apiLib)
        {
            string dir = System.IO.Directory.GetCurrentDirectory() + @"\actors\";
            var casts = (from x in movie.Cast  where x != null select x).ToList();
           // Downloader dl = new Downloader();
            foreach (var cast in casts)
            {
                var filename = cast.ActorName.Replace(": ", "_").Replace("/", "_");
                filename = filename.Replace(" ", "_") + ".jpg";
                string saveLocation = dir + filename;
                cast.ActorLocalImage = saveLocation;
              
                //if (!File.Exists(saveLocation))//Check if Actor data exists already
                //{
                //    var url = cast.ActorImage;
                //     Task.Run(async () => await dl.Download(url, saveLocation)).Wait();
                //}-
            } 
        }

        public static async Task SaveMovies(List<IMDBMovie> movies, PostersDB.Poster.SaveMovieCompleted callback)
        {
            Serializer ser = new Serializer();

            var apiLib = new ApiLib("k_u215r302");
            for (var i = 0; i < movies.Count; i++)
            {
                IMDBMovie movie = movies[i];
                await SaveMovie(movie, ser, apiLib);
                callback(movie, i+1);
            }
        }
    }
}