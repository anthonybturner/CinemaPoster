using MoviePoster.MovieTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePoster
{
    public class IMDBMovie
    {
        public string Id { get; set; }

        public Boolean IsInTheaters { get; set; }

        public string Title { get; set; }

        public string FullTitle { get; set; }

        public string Year { get; set; }

        public string ReleaseState { get; set; }

        public string Image { get; set; }

        public string LocalImage { get; set; }

        public string RuntimeMins { get; set; }

        public string RuntimeStr { get; set; }

        public string Plot { get; set; }

        public string ContentRating { get; set; }

        public string ImdbRating { get; set; }

        public string ImdbRatingCount { get; set; }


        public string MetacriticRating { get; set; }

        public string Genres { get; set; }

        public string Directors { get; set; }

        public string Stars { get; set; }

        public Boolean IsLocallyLoaded { get; set; }

        public MovieTechnical TechSpecs { get; set; }

    }
}
