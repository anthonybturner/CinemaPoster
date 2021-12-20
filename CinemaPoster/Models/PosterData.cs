using IMDbApiLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPoster.Models
{

    namespace IMDbApiLib.Models
    {
        public class PosterData
        {
            public string IMDbId { get; set; }
            public string Title { get; set; }
            public string FullTitle { get; set; }
            public string Type { set; get; }
            public string Year { set; get; }

            public List<PosterDataItem> Posters { get; set; }
            public List<PosterDataItem> Backdors { get; set; }

            public string ErrorMessage { get; set; }
        }
    }
}
