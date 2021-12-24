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
using System.Xml;
using CinemaPosterApp;
using System.ComponentModel;
using System.Windows.Forms;
using CinemaPoster.Utilities;
using IMDbApiLib.Models;

namespace CinemaPoster.Controllers
{
    class OmdbApi
    {

        private Form form;

        public OmdbApi()
        {
            
        }
        public OmdbApi(Form formMain)
        {
            form = formMain;
        }

        public async Task<IMDBMovie> GetMovieAsync(string title, bool wantsPlot, string datatype = "json")
        {
            IMDBMovie movie = new IMDBMovie();
            var url = "";
            if (wantsPlot)
            {
                url = String.Format("http://omdbapi.com?apikey=16dc3a0c&t={0}&r={1}&plot=full", title, datatype);
            }
            else
            {
                url = String.Format("http://omdbapi.com?apikey=16dc3a0c&t={0}&r={1}", title, datatype);
            }

            if (datatype == "xml")
            {
                movie = GetXmlMovie(url);
            }
            else
            {
                movie = await GetJsonMovieAsync(url);
            }

            return movie;
        }

        public async Task<IMDBMovie> GetJsonMovieAsync(String url)
        {
            IMDBMovie movie = new IMDBMovie();

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
                    Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JToken.Parse(body);
                    movie = token.ToObject<IMDBMovie>();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex.Message, ex.InnerException.ToString());
                }
            }
            return movie;
        }

        public  IMDBMovie GetXmlMovie(String url)
        {
            IMDBMovie movie = new IMDBMovie();

            XmlTextReader reader = new XmlTextReader(url);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "movie")
                {
                    while (reader.MoveToNextAttribute())
                    { // Read the attributes.
                        switch (reader.Name)
                        {
                            case "title":
                                movie.Title = reader.Value;
                                break;
                            case "year":
                                movie.Year = reader.Value;
                                break;
                            case "rated":
                                movie.Rated = reader.Value;
                                break;
                            case "released":
                                movie.Released = reader.Value;
                                break;
                            case "tuntime":
                                movie.Runtime = reader.Value;
                                break;
                            case "henre":
                                movie.Genre = reader.Value;
                                break;
                            case "director":
                                movie.Director = reader.Value;
                                break;
                            case "plot":
                                movie.Plot = reader.Value;
                                break;
                            case "poster":
                                movie.Poster = reader.Value;
                                break;
                            case "metaScore":
                                movie.MetaScore = reader.Value;
                                break;
                            case "imdbRating":
                                movie.imdbRating = reader.Value;
                                break;
                            case "imdbID":
                                movie.imdbID = reader.Value;
                                break;
                        }
                    }
                }
            }
            return movie;
        }
    }
}
