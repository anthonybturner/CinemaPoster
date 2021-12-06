using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePoster.MovieTypes
{
    public class MovieTechnical
    {
        public string @type { get; set; }

        public string[] aspectRatios { get; set; }

        public string title { get; set; }

        public string titleType { get; set; }

        public Int32 year { get; set; }

        public string[] cameras { get; set; }

        public string[] colorations { get; set; }

        public string[] negativeFormats { get; set; }

        public string[] printedFormats { get; set; }

        public string[] processes { get; set; }

        public string[] soundMixes { get; set; }

        public static implicit operator Task<object>(MovieTechnical v)
        {
            throw new NotImplementedException();
        }
    }
}
