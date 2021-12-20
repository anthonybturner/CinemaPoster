using CinemaPosterApp.MovieTypes;
using CinemaPosterApp.Utilities;
using System;
using System.Xml;

namespace CinemaPosterApp
{
    public class PlexApi
    {
        public static MovieTechnical GetNowPlayingInfo(Uri appPlexURL, bool isPlaying)
        {
            MovieTechnical mtech = new MovieTechnical();
            try
            {
                XmlTextReader reader = new XmlTextReader(appPlexURL.ToString());
                if(reader.HasAttributes && isPlaying)
                {
                    mtech.IsPlaying = true;
                    return mtech;
                }
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Video":
                                while (reader.MoveToNextAttribute())
                                { // Read the attributes.
                                    switch (reader.Name)
                                    {
                                        case "title":
                                            mtech.title = reader.Value;
                                            break;

                                        case "contentRating":
                                            mtech.contentRating = reader.Value;
                                            break;
                                        case "audienceRatingImage":
                                            mtech.audienceRatingImage = reader.Value.Substring(reader.Value.LastIndexOf(".") + 1);
                                            break;
                                        case "tagline":
                                            mtech.tagline = reader.Value;
                                            break;
                                        case "rating":
                                            mtech.rating = reader.Value;
                                            break;
                                        case "audienceRating":
                                            mtech.audienceRating = reader.Value;
                                            break;
                                        case "year":
                                            mtech.year = reader.Value;
                                            break;
                                    }
                                }
                                break;
                            case "Media":
                                while (reader.MoveToNextAttribute())
                                { // Read the attributes. 
                                    switch (reader.Name)
                                    {
                                        case "duration":
                                            mtech.duration = reader.Value;
                                            break;
                                        case "aspectRatio":
                                                mtech.aspectRatio = reader.Value;
                                            break;
                                        case "bitrate":
                                            mtech.bitrate = reader.Value;
                                            break;
                                        case "videoResolution":
                                            mtech.videoResolution = reader.Value;
                                            break;
                                        case "videoFrameRate":
                                            mtech.videoFrameRate = reader.Value;
                                            break;
                                        case "width":
                                            mtech.width = reader.Value;
                                            break;
                                        case "videoCodec":
                                            mtech.videoCodec = reader.Value;
                                            break;
                                    }
                                }
                                break;
                            case "Part":
                                while (reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "Stream")
                                    {
                                        while (reader.MoveToNextAttribute())
                                        {
                                            switch (reader.Name)
                                            {
                                                case "displayTitle":
                                                var codec = reader.Value;
                                                if (codec.Contains("Unknown"))
                                                {
                                                    mtech.audioCodec = codec.Replace("Unknown (", "").Replace(")", "").Replace(" ", "_");
                                                }
                                                else if (codec.Contains("English") && !codec.Contains("(PGS)"))
                                                {
                                                    mtech.audioCodec = codec.Replace("English (", "").Replace(")", "").Replace(" ", "_");
                                                }
                                                else
                                                {
                                                    if (reader.Name == "displayTitle" && reader.Value.Contains("HDR"))
                                                    {
                                                        mtech.hdr = "hdr";
                                                    }
                                                }
                                                break;
                                            }                                           
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.WriteLog(e.Message, e.ToString());
            }
            return mtech;
        }
    }
}