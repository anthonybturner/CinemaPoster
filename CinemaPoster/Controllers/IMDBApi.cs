using CinemaPosterApp;
using IMDbApiLib;
using IMDbApiLib.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using CinemaPosterApp.MovieTypes;
using System.IO;
using CinemaPoster.Utilities;

namespace CinemaPoster.Controllers
{
    class IMDBApi
    {

        private static ApiLib api = new ApiLib("k_u215r302");
        private static OmdbApi omdbApi = new OmdbApi();
        private static Serializer serializer = new Serializer();

        public static async Task GetMovieInfoAsync(IMDBMovie movie, Boolean IsNowPlaying = false)
        {
            await UpdateMovieInfo(movie, IsNowPlaying);            
        }

        private static async Task<IMDBMovie> UpdateMovieInfo(IMDBMovie movie, Boolean IsNowPlaying = false)
        {
            if (IsNowPlaying){//NowPlaying requires searching for  movie by title
                movie.LocalImage = FileNameParser.CreateImageDirectory(movie.Title);
                var XmlFileLocation = FileNameParser.CreateXmlDirectory(movie.Title);
                if (File.Exists(XmlFileLocation))
                {
                    IMDBMovie movie2 = serializer.DeSerializeObject<IMDBMovie>(XmlFileLocation);//If movie exists on disk, use that first.
                    if (movie2 != null)
                    {
                        movie.Poster = movie2.Image;
                        movie.Image = movie2.Image;
                        movie.ActorList = movie2.ActorList;
                        movie.ActorLocalImages = movie2.ActorLocalImages;
                        return movie;
                    }
                } else{
                    //Create local image path
                    //Fetch remote
                    IMDbApiLib.Models.SearchData data = await api.SearchMovieAsync(movie.Title.Replace("/", " "));
                    if (data != null && data.Results != null)
                    {
                        var results = (from x in data.Results select x).FirstOrDefault();
                        if (results != null)
                        {
                            movie.Id = results.Id;
                            movie.Title = results.Title;
                        }
                    }          
                }
            }
            await GetFullMovieInfoAsync(movie);
            serializer.SerializeObject<IMDBMovie>(movie, movie.Title);
            return movie;
        }

        private static async Task GetFullMovieInfoAsync(IMDBMovie movie)
        {
            if (movie.Id != null && movie.Id.Length > 0)
            {
                var directory = @"..\actors\";
                TitleData mMovie = await api.TitleAsync(movie.Id, Language.en, true, false, false, true, false, false, true);
                if (mMovie != null)
                {
                    if (mMovie.ActorList != null)
                    {
                        for (var i = 0; i < mMovie.ActorList.Count; i++)
                        {
                            var x = mMovie.ActorList[i];
                            var filename = x.Name.Replace(" ", "_").Trim().ToLower() + ".jpg";
                            var saveLocation = directory + filename;
                            movie.ActorLocalImages.Add(saveLocation);
                        }
                    }
                    movie.ContentRating = mMovie.ContentRating;
                    movie.FullTitle = mMovie.FullTitle;
                    movie.Genres = mMovie.Genres;
                    movie.Image = mMovie.Image;
                    //movie.Image = String.Format("http://img.omdbapi.com/?apikey={1}&i={0}&h=1920", movie.Id, "{0}");
                    movie.Images = mMovie.Images;
                    movie.IMDbRating = mMovie.IMDbRating;
                    movie.IMDbRatingVotes = mMovie.IMDbRatingVotes;
                    movie.MetacriticRating = mMovie.MetacriticRating;
                    movie.OriginalTitle = mMovie.OriginalTitle;
                    movie.Plot = mMovie.Plot;
                    movie.PlotLocal = mMovie.PlotLocal;
                    movie.Posters = mMovie.Posters;
                    movie.Ratings = mMovie.Ratings;
                    movie.ReleaseDate = mMovie.ReleaseDate;
                    movie.RuntimeMins = mMovie.RuntimeMins;
                    movie.RuntimeStr = mMovie.RuntimeStr;
                    movie.StarList = mMovie.StarList;
                    movie.Stars = mMovie.Stars;
                    movie.Tagline = mMovie.Tagline;
                    movie.Title = mMovie.Title;
                    movie.Year = mMovie.Year;
                    movie.Awards = mMovie.Awards;
                    movie.BoxOffice = mMovie.BoxOffice;
                    movie.ActorList = mMovie.ActorList;
                }
            }
            else{
                //We may be local or API sub not paid. Try getting basic poster info from another api
                await omdbApi.GetMovieAsync(movie, false);
                //TODO find way to fill actors list

            }
        }

        public static async Task<List<IMDBMovie>> GetMoviesInfoAsync()
        {
            List<IMDBMovie> movies = new List<IMDBMovie>();

            var data = await api.MostPopularMoviesAsync();
            foreach (IMDbApiLib.Models.MostPopularDataDetail x in data.Items)
            {
                IMDBMovie movie = new IMDBMovie();
                movie.Id = x.Id;
                movie.Title = x.Title;
                await GetMovieInfoAsync(movie);
                movies.Add(movie);
            }

            var data2 = await api.ComingSoonAsync();
            foreach (IMDbApiLib.Models.NewMovieDataDetail x in data2.Items)
            {
                IMDBMovie movie = new IMDBMovie();
                movie.Id = x.Id;
                movie.Title = x.Title;
                await GetMovieInfoAsync(movie);
                movies.Add(movie);
            }
            return movies;
        }
    }
}
