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
using CinemaPoster.Models;
using MovieTypes;

namespace CinemaPosterApp.Controllers
{
    class ImdbFetcher
    {
       private ApiLib apiLib { get; set; }

        public ImdbFetcher()
        {
        }

        public async Task<TitleData> FetchMovieAsync(string mtitle, MovieTechnical mtech)
        {
            TitleData movie = await Task.Run(() => DownloadMovieAsync(mtitle));

            if(movie == null)
            {
                return null;
            }

           // movie.AspectRatio = mtech.aspectRatio;
            movie.Tagline = movie.Tagline;
           /// movie.VideoResolution = mtech.videoResolution;
            //movie.videoFrameRate = mtech.videoFrameRate;
///movie.AudioCodec = mtech.audioCodec;
         //   movie.VideoCodec = mtech.videoCodec;
         //   movie.Hdr = mtech.hdr;
            if(movie.Title == null || movie.Title.Length == 0)
            {
                movie.Title = mtitle;
            }
            if (movie.Tagline == null || movie.Tagline.Length == 0)
            {
                movie.Tagline = mtech.tagline;
            }
            await Task.Run(() => SaveMovie(movie, new Serializer(), apiLib));
          
            return movie;
        }

        public async Task<List<TitleData>> FetchNewMoviesAsync()
        {
            List<TitleData> TempMovies = new List<TitleData>();
            List<TitleData> pmMovies = await Task.Run(() => DownloadMostPopularMoviesAsync());
            List<TitleData> csMovies = await Task.Run(() => DownloadComingSoonMoviesAsync());
           
            TempMovies.AddRange(pmMovies);
            TempMovies.AddRange(csMovies);
            Serializer ser = new Serializer();
            for (var i = 0; i < TempMovies.Count; i++)
            {
                TitleData movie = TempMovies[i];
                ser.SerializeObject(movie, movie.Title);
               // callback(movie, i + 1);
            }
            return TempMovies;
        }

        public async Task<TitleData> DownloadMovieAsync(string title)
        {
            TitleData movie = new TitleData();
            IMDbApiLib.Models.SearchData data = await apiLib.SearchMovieAsync(title.Replace("/", " "));
            if (data != null && data.Results != null)
            {
                foreach (var x in data.Results)
                {
                    var IMDBMovie = await apiLib.TitleAsync(x.Id, Language.en, true, false, false, true, false, false, true);
                    if (IMDBMovie.Title == null || IMDBMovie.FullTitle == null) continue;

                    movie = await CreateMovie(x.Id, "");
                    break;
                }
            }
            else{//Try getting basic poster info from another api
                OmdbApi api = new OmdbApi();
               // movie = await api.GetMovieAsync(title, false, "xml");
               // movie = await api.GetPosterAsync(movie.Id, false, "xml");
                await CreateMovie(movie.Id, title);
            }
            return movie;
        }
        private async Task<TitleData> CreateMovie(String Id, string title)
        {
            TitleData Movie = new TitleData();
            //var directory = @"..\actors\";
            //var filename = "";
            //var saveLocation = "";

            //if (Id != null && Id.Length > 0)
            //{
            //    TitleData mMovie = await apiLib.TitleAsync(Id, Language.en, true, false, false, true, false, false, true);
            //    for (var i = 0; i < mMovie.ActorList.Count; i++)
            //    {
            //        var x = mMovie.ActorList[i];
            //        filename = x.Name.Replace(" ", "_").Trim().ToLower() + ".jpg";
            //        saveLocation = directory + filename;
            //        Movie.ActorLocalImages.Add(saveLocation);
            //    }

            //    Movie.ContentRating = mMovie.ContentRating;
            //    Movie.FullTitle = mMovie.FullTitle;
            //    Movie.Genres = mMovie.Genres;
            //    Movie.Image = mMovie.Image;
            //    Movie.Images = mMovie.Images;
            //    Movie.IMDbRating = mMovie.IMDbRating;
            //    Movie.IMDbRatingVotes = mMovie.IMDbRatingVotes;
            //    Movie.MetacriticRating = mMovie.MetacriticRating;
            //    Movie.OriginalTitle = mMovie.OriginalTitle;
            //    Movie.Plot = mMovie.Plot;
            //    Movie.PlotLocal = mMovie.PlotLocal;
            //    Movie.Posters = mMovie.Posters;
            //    Movie.Ratings = mMovie.Ratings;
            //    Movie.ReleaseDate = mMovie.ReleaseDate;
            //    Movie.RuntimeMins = mMovie.RuntimeMins;
            //    Movie.RuntimeStr = mMovie.RuntimeStr;
            //    Movie.Stars = mMovie.Stars;
            //    Movie.Tagline = mMovie.Tagline;
            //    Movie.Title = mMovie.Title;
            //    Movie.Year = mMovie.Year;
            //    Movie.Awards = mMovie.Awards;
            //    Movie.BoxOffice = mMovie.BoxOffice;
            // //   Movie.ActorList = mMovie.ActorList;

            //}else
            //{
            //    Movie.Title = title;
            //}
           
            //directory = System.IO.Directory.GetCurrentDirectory() + @"\images\";
            //filename = Movie.Title.Replace(": ", "_").Replace("/", "_");
            //filename = filename.Replace(" ", "_") + ".jpg";
            //saveLocation = directory + filename;
            //Movie.LocalImage = saveLocation;
           // CreatePosterImageLinks(Movie);
            return Movie;
        }

        public async Task<List<TitleData>> DownloadComingSoonMoviesAsync()
        {
            List<TitleData> movies = new List<TitleData>();
            var data = await apiLib.ComingSoonAsync();
            foreach (IMDbApiLib.Models.NewMovieDataDetail x in data.Items)
            {
                var Movie = await CreateMovie(x.Id, "");
                movies.Add(Movie);
            }
            return movies;
        }

        public async Task<List<TitleData>> DownloadMostPopularMoviesAsync()
        {
            List<TitleData> movies = new List<TitleData>();
            var data = await apiLib.MostPopularMoviesAsync();
            foreach (IMDbApiLib.Models.MostPopularDataDetail x in data.Items)
            {
                var Movie = await CreateMovie(x.Id, "");
                movies.Add(Movie);
            }
            return movies;
        }

        private static void CreatePosterImageLinks(TitleData movie)
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\images\";
            string filename = movie.Title.Replace(": ", "_").Replace("/", "_");
            filename = filename.Replace(" ", "_") + ".jpg";

            PosterDataItem wantedPoster = null;
            //foreach (var poster in movie.Posters.Posters)
            //{
            //    if (wantedPoster == null)
            //    {
            //        wantedPoster = poster;
            //        continue;
            //    }
            //    if (poster.Height > wantedPoster.Height && poster.Width >= 1440 && poster.Height <= 2100 && poster.Height >= 1080)
            //    {
            //        wantedPoster = poster;
            //    }
            //}

            //if (wantedPoster != null)
            //{
            //    var url = wantedPoster.Link;
            //    movie.Image = url;
            //}
        }

        public  async Task<List<TitleData>> FetchTechSpecs(TitleData movie)
        {
            List<TitleData> movies = new List<TitleData>();
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(String.Format("https://imdb8.p.rapidapi.com/title/get-technical?tconst={0}", movie.Id)),
                Headers =
                {
                    { "x-rapidapi-key", "apikeyhere" },
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
                  //  movie.TechSpecs = token.ToObject<MovieTechnical>();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex.Message, ex.InnerException.ToString());
                }
                movies.Add(movie);
            }
            return movies;
        }

        public void SaveMovie(TitleData movie, Serializer ser, ApiLib apiLib)
        {
            ser.SerializeObject<TitleData>(movie, movie.Title);
        }

        private void DownloadPoster(TitleData movie)
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\images\";
            string filename = movie.Title.Replace(": ", "_").Replace("/", "_");
            filename = filename.Replace(" ", "_") + ".jpg";

            string saveLocation = directory + filename;
          //  movie.LocalImage = saveLocation; TODO put back
            movie.Image = saveLocation;
            if (!File.Exists(saveLocation))
            {
                PosterDataItem wantedPoster = null;
            //    foreach (var x in movie.Posters.Posters)
            //    {
            //        if (wantedPoster == null)
            //        {
            //            wantedPoster = x;
            //            continue;
            //        }
            //        if (x.Height > wantedPoster.Height && x.Width >= 1440  && x.Height <= 2100 && x.Height >= 1080)
            //        {
            //            wantedPoster = x;
            //        }
            //    }

            //    if (wantedPoster != null)
            //    {
            //        var url = wantedPoster.Link;
            //        byte[] imageBytes;
            //        FileStream fs = null;
            //        BinaryWriter bw = null;
            //        try
            //        {
            //            HttpWebRequest imageRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            //            WebResponse imageResponse = imageRequest.GetResponse();
            //            Stream responseStream = imageResponse.GetResponseStream();
            //            using (BinaryReader br = new BinaryReader(responseStream))
            //            {
            //                imageBytes = br.ReadBytes(500000);
            //                br.Close();
            //                responseStream.Close();
            //                imageResponse.Close();
            //                fs = new FileStream(saveLocation, FileMode.Create);
            //                bw = new BinaryWriter(fs);
            //                bw.Write(imageBytes);
            //            }
            //        }
            //        catch(Exception e)
            //        {
            //            Logger.WriteLog(e.Message, e.ToString());
            //        }
            //        finally
            //        {
            //            if (fs != null)
            //            {
            //                fs.Close();
            //            }
            //            if (bw != null)
            //            {
            //                bw.Close();
            //            }
            //        }
            //    }
            }            
        }
        private  async Task DownloadActorsAsync(TitleData movie, ApiLib apiLib)
        {
           
        }
    }
}
