using CinemaPoster.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CinemaPosterApp
{
    public class IMDBMovie : IMDbApiLib.Models.TitleData
    {

        public IMDBMovie()
        {
            ActorLocalNames = new List<string>();
            ActorLocalImages = new List<string>();
            VideoResolution = "1080";
        }

        public string LocalImage { get; set; }
        public string AspectRatio { get; set; }
        public string VideoResolution { get; set; }
        public string videoFrameRate { get; set; }
        public string AudioCodec { get; set; }
        public string VideoCodec { get; set; }
        public string Hdr { get; set; }
        public string Poster { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        [XmlArray("ActorLocalNames")]       
        public List<string> ActorLocalNames { get; set; }
        [XmlArray("ActorLocalImages")]
        public List<string> ActorLocalImages { get; set; }

    }
}


