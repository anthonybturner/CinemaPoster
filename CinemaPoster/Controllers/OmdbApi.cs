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
                    LogWriter.WriteLog(ex.Message, ex.InnerException.ToString());
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

        public async Task DownloadPosterAsync(IMDBMovie movie)
        {
            var url = String.Format("http://img.omdbapi.com/?i={0}&h={1}&apikey=16dc3a0c", movie.imdbID, "3000");
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";
            string filename = movie.Title.Replace(": ", "_");

            filename = filename.Replace(" ", "_") + ".jpg";

            string saveLocation = directory + filename;
            movie.LocalImage = saveLocation;
            movie.Image = saveLocation;

            if (!File.Exists(saveLocation))
            {
                byte[] imageBytes;
                WebResponse imageResponse = null;
                Stream responseStream = null;
                try
                {
                    HttpWebRequest imageRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                    imageResponse = imageRequest.GetResponse();
                    responseStream = imageResponse.GetResponseStream();
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
                        }
                        catch (Exception e)
                        {
                            LogWriter.WriteLog(e.Message, e.ToString());
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
                catch (Exception ex)
                {
                    LogWriter.WriteLog(ex.Message, ex.ToString());

                }
                finally
                {
                    if (imageResponse != null)
                    {
                        imageResponse.Close();
                    }
                    if (responseStream != null)
                    {
                        responseStream.Close();
                    }
                }

            }
        }

        public async Task Download(string saveLocation, string url)
        {


        }

        public void SaveMovie(IMDBMovie movie)
        {
            Serializer ser = new Serializer();
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";
            string filename = movie.Title.Replace(": ", "_");
            filename = filename.Replace(" ", "_") + ".xml";
            string saveLocation = directory + filename;

            if (!File.Exists(saveLocation))
            {
                ser.SerializeObject<IMDBMovie>(movie, saveLocation);
            }
        }

        private bool downloadComplete = false;

        private async Task startDownload(Uri toDownload, string saveLocation)
        {
            string outputFile = Path.Combine(saveLocation, Path.GetFileName(toDownload.AbsolutePath));

            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(toDownload, outputFile);

            while (!downloadComplete)
            {
                Application.DoEvents();
            }

            downloadComplete = false;
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // No changes in this method...
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (form != null)
            {
                form.BeginInvoke((MethodInvoker)delegate
                {
                //textBoxLog.AppendText("OK");
                downloadComplete = true;
                });
            }
        }

        public async Task SaveMoviesAsync(List<IMDBMovie> movies)
        {
            Serializer ser = new Serializer();
            var apiLib = new ApiLib("k_u215r302");
            string directory = System.IO.Directory.GetCurrentDirectory() + @"\data\";

            foreach (IMDBMovie movie in movies)
            {
                string filename = movie.Title.Replace(": ", "_");
                filename = filename.Replace(" ", "_") + ".jpg";
                string saveLocation = directory + filename;
                movie.LocalImage = saveLocation;
                movie.Image = saveLocation;

                if (!File.Exists(saveLocation))
                {
                    IMDbApiLib.Models.SearchData data = await apiLib.SearchMovieAsync(movie.FullTitle);
                    if (data != null)
                    {
                        var res = (from x in data.Results select x).FirstOrDefault();
                        if (res != null)
                        {
                            var data2 = await apiLib.PostersAsync(res.Id);
                            PosterDataItem wantedPoster = null;
                            foreach(var x in data2.Posters)
                            {
                                if(wantedPoster == null)
                                {
                                    wantedPoster = x;
                                    continue;
                                }
                                if( x.Height > wantedPoster.Height && x.Width >=1440)
                                {
                                    wantedPoster = x;
                                }
                            }

                            if (wantedPoster != null)
                            {
                                var url = wantedPoster.Link;
                                Task.Run(() => new Downloader().Download(url, saveLocation)).Wait();
                                saveLocation = saveLocation.Replace(".jpg", ".xml");
                                ser.SerializeObject<IMDBMovie>(movie, saveLocation);
                            }
                        }
                    }
                   
                    
                }
                // OmdbApi.SaveMovie(movie);
            }
        }
    }
}
