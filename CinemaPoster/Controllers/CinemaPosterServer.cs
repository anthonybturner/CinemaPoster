using CinemaPoster.Utilities;
using MovieTypes;
using IMDbApiLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CinemaPosterApp.Utilities;

namespace CinemaPoster.Controllers
{
    class CinemaPosterServer
    {
        private static Serializer serializer = new Serializer();

        public static async Task<List<TitleData>> GetPostersAsync(bool IsRemoteOnly)
        {
            List<TitleData> movies = new List<IMDbApiLib.Models.TitleData>();
            string url = @"http://cinema-posters.com/Handlers/PostersHandler.ashx?method=GetPosters";
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
                    List<string> JsonList = JsonConvert.DeserializeObject<List<string>>(body);
                    //  JArray arr = JArray.FromObject(objective);
                    foreach (var JsonData in JsonList)
                    {
                        var obj = JsonConvert.DeserializeObject(JsonData);
                        TitleData movie = JObject.Parse(obj.ToString()).ToObject<TitleData>();
                        movies.Add(movie);

                        if (!IsRemoteOnly)
                        {
                            //Save json in users local directory
                            var saveloc = FileNameParser.CreateJsonDirectory(movie.Title);
                            SaveJsonLocally(movie, saveloc);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex.Message, ex.InnerException.ToString());
                }
            }
            return movies;
        }

  
        public static async Task<TitleData> GetPosterAsync(string title, bool IsRemoteOnly)
        {
            TitleData movie = null;
              string url = String.Format("http://cinema-posters.com/Handlers/PostersHandler.ashx?method=GetPoster&title={0}", title);
          //  string url = String.Format("https://localhost:44310/Handlers/PostersHandler.ashx?method=GetPoster&title={0}", title);
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),

            };
            using (var response = await client.SendAsync(request)) {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    string JsonData = JsonConvert.DeserializeObject<string>(body);
                    var obj = JsonConvert.DeserializeObject(JsonData);
                    movie = JObject.Parse(obj.ToString()).ToObject<TitleData>();
                    if (!IsRemoteOnly)
                    {
                        //Save json in users local directory
                        var saveloc = FileNameParser.CreateJsonDirectory(movie.Title);
                        SaveJsonLocally(movie, saveloc);
                        //Save image in users local directory
                    }
                }
                catch (Exception ex)
                {

                    Logger.WriteLog(ex.Message, ex.InnerException.ToString());
                }
            }
            return movie;
        }
        private static void SaveJsonLocally(TitleData movie, string saveloc)
        {
            if (!File.Exists(saveloc))
            {
                var savedJson = JsonConvert.SerializeObject(movie);
                JsonSerializer serializer = new JsonSerializer();
                var writer = new JsonTextWriter(File.CreateText(saveloc));
                serializer.Serialize(writer, savedJson);
                writer.Close();
            }
        }
    }
}
    


