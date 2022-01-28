using IMDbApiLib;
using MovieTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CinemaPosterApp;
using System.ComponentModel;
using System.Windows.Forms;
using CinemaPoster.Utilities;
using IMDbApiLib.Models;
using CinemaPosterApp.Utilities;

namespace CinemaPoster.Controllers
{
    class OmdbApi
    {

        private Form form;
        public static string API_KEY = "16dc3a0c";

        public OmdbApi()
        {
            
        }
        public OmdbApi(Form formMain)
        {
            form = formMain;
        }

        public async Task GetMovieAsync(TitleData movie, bool wantsPlot)
        {
            var url = "";
            if (wantsPlot)
            {
                url = String.Format("http://omdbapi.com?apikey={2}&t={0}&r={1}&plot=full", movie.Title, "xml", OmdbApi.API_KEY);
            }
            else
            {
                url = String.Format("http://omdbapi.com?apikey={2}&t={0}&r={1}", movie.Title, "xml", OmdbApi.API_KEY);
            }
            await GetJsonMovieAsync(url, movie);
        }


        public void GetPoster(TitleData movie)
        {
            var url = String.Format("http://img.omdbapi.com/?apikey={1}&i={0}&h=1920", movie.Id, OmdbApi.API_KEY);
            using (WebClient client = new WebClient())
            {
              //  client.DownloadFileAsync(new Uri(url), movie.LocalImage);                

            }
        }

        public async Task<TitleData> GetJsonMovieAsync(String url, TitleData movie)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),

            };
            using (var response = await client.SendAsync(request))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(body);
                    var nodes = xmlDoc.SelectNodes("root/movie");
                    foreach (XmlNode childrenNode in nodes)
                    {
                        movie.Id = childrenNode.Attributes["imdbID"].Value;
                        if(movie.Title == null || movie.Title.Length == 0)
                        {
                            movie.Title = childrenNode.Attributes["title"].Value;
                        }

                             
                        //movie.Poster =  String.Format("http://img.omdbapi.com/?apikey={1}c&i={0}&h=1920", movie.Id, OmdbApi.API_KEY);
                     //   movie.Image = movie.Poster;
                        GetPoster(movie);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex.Message, ex.InnerException.ToString());
                }
            }
            return movie;
        }

        public TitleData GetXmlMovie(String url)
        {
            TitleData movie = new TitleData();

            XmlTextReader reader = new XmlTextReader(url);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "movie")
                {
                    while (reader.MoveToNextAttribute())
                    { // Read the attributes.
                        switch (reader.Name)
                        {
                            case "imdbID":
                                movie.Id = reader.Value;
                                break;
                            case "title":
                                movie.Title = reader.Value;
                                break;
                            case "year":
                                movie.Year = reader.Value;
                                break;
                            case "rated":
                                movie.ContentRating = reader.Value;
                                break;
                            case "released":
                                movie.ReleaseDate = reader.Value;
                                break;
                            case "runtime":
                                movie.RuntimeMins = reader.Value;
                                break;
                            case "genre":
                                movie.Genres = reader.Value;
                                break;
                            case "plot":
                                movie.Plot = reader.Value;
                                break;
                            case "poster":
                                //movie.Poster = reader.Value;
                                movie.Image = reader.Value;
                                break;
                            case "metaScore":
                                movie.MetacriticRating = reader.Value;
                                break;
                            case "imdbRating":
                                movie.IMDbRating = reader.Value;
                                break;
                        }
                    }
                }
            }
            return movie;
        }



        public String GetMoviePosterUrl(String url)
        {
            var PosterUrl = "";

            XmlTextReader reader = new XmlTextReader(url);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "movie")
                {
                    while (reader.MoveToNextAttribute())
                    { // Read the attributes.
                        switch (reader.Name)
                        {
                            case "poster":
                                PosterUrl = reader.Value;
                                break;
                        }
                    }
                }
            }
            return PosterUrl;
        }
    }
}
