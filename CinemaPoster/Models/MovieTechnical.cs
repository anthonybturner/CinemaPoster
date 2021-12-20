using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPosterApp.MovieTypes
{
    public class MovieTechnical
    {
        public string @type { get; set; }

        public string aspectRatio { get; set; }

        public string title { get; set; }

        public string duration { get; set; }

        public string year { get; set; }

        public string tagline { get; set; }

        public string contentRating { get; set; }

        public string videoResolution { get; set; }

        public string width { get; set; }

        public string bitrate { get; set; }

        public string videoFrameRate { get; set; }
        public string audienceRatingImage { get; set; }
        public string ratingImage { get; set; }
        public string rating { get; set; }
        public string audienceRating { get; set; }
        public string audioCodec { get; set; }
        public string videoCodec { get; set; }
        public string hdr { get; set; }
        public bool IsPlaying { get; set; }
    }
}
