using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePoster
{
    public class Movie
    {
        public string @type { get; set; }

        public string id { get; set; }
        public string title { get; set; }

        public image image { get; set; }

        public string titleType { get; set; }

        public string year { get; set; }


    }
}
