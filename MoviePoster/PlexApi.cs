using MoviePoster.MovieTypes;
using MoviePoster.Utilities;
using System;
using System.Xml;

namespace MoviePoster
{
    public class PlexApi
    {
        public static MovieTechnical GetNowPlayingInfo(string ip, string token)
        {
            MovieTechnical mtech = new MovieTechnical();

            Uri appPlexURL = new Uri(@String.Format("http://{0}:32400/status/sessions?X-Plex-Token={1}", ip, token));
            try
            {
                XmlTextReader reader = new XmlTextReader(appPlexURL.ToString());
                while (reader.Read())
                {

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Video":
                                while (reader.MoveToNextAttribute())
                                { // Read the attributes.
                                    if (reader.Name == "title")
                                    {
                                        mtech.title = reader.Value;
                                        break;
                                    }
                                    else if (reader.Name == "contentRating")
                                    {
                                        mtech.contentRating = reader.Value;
                                    }
                                    else if (reader.Name == "audienceRatingImage")
                                    {
                                        mtech.audienceRatingImage = reader.Value.Substring(reader.Value.LastIndexOf(".") + 1);
                                    }
                                    else if (reader.Name == "tagline")
                                    {
                                        mtech.tagline = reader.Value;
                                    }
                                    else if (reader.Name == "rating")
                                    {
                                        mtech.rating = reader.Value;
                                    }
                                    else if (reader.Name == "audienceRating")
                                    {
                                        mtech.audienceRating = reader.Value;
                                    }
                                    else if (reader.Name == "year")
                                    {
                                        mtech.year = reader.Value;
                                    }
                                }
                                break;
                            case "Media":
                                while (reader.MoveToNextAttribute())
                                { // Read the attributes.
                                    if (reader.Name == "duration")
                                    {
                                        mtech.duration = reader.Value;
                                    }
                                    else if (reader.Name == "aspectRatio")
                                    {
                                        mtech.aspectRatio = reader.Value;
                                    }
                                    else if (reader.Name == "bitrate")
                                    {
                                        mtech.bitrate = reader.Value;
                                    }
                                    else if (reader.Name == "videoResolution")
                                    {
                                        mtech.videoResolution = reader.Value;
                                    }
                                    else if (reader.Name == "videoFrameRate")
                                    {
                                        mtech.videoFrameRate = reader.Value;
                                    }
                                    else if (reader.Name == "width")
                                    {
                                        mtech.width = reader.Value;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.WriteLog(e.Message, e.InnerException.ToString());
            }
            return mtech;
        }
    }
}