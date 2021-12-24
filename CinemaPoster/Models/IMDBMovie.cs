using CinemaPosterApp.MovieTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPosterApp
{
    public class IMDBMovie
    {

        public IMDBMovie()
        {
            Cast = new TopCast[6];
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string FullTitle { get; set; }

        public string Year { get; set; }
        public string Genre { get; set; }

        public string ReleaseState { get; set; }

        public string Image { get; set; }

        public string LocalImage { get; set; }

        public string Runtime { get; set; }

        public string RuntimeStr { get; set; }

        public string Plot { get; set; }

        public string Rated { get; set; }
        public Rating[] Ratings { get; set; }
        public string MetaScore { get; set; }

        public string imdbRating { get; set; }
        public string imdbVotes { get; set; }
        public string imdbID { get; set; }
        public string Type { get; set; }
        public string DVD { get; set; }
        public string BoxOffice { get; set; }
        public string Production { get; set; }
        public string Website { get; set; }
        public string Response { get; set; }

        public string ImdbRatingCount { get; set; }

        public string MetacriticRating { get; set; }

        public string Genres { get; set; }

        public string Director { get; set; }
        
        public string Writer { get; set; }

        public string Actors { get; set; }

        public MovieTechnical TechSpecs { get; set; }

        public string MovieTense { get; set; }
        public string Released { set; get; }

        public string Tagline { get; set; }
        public string duration { get; set; }
        public string AspectRatio { get; set; }
        public string AudienceRating { get; set; }
        public string AudienceRatingImage { get; set; }
        public string VideoResolution { get; set; }
        public string videoFrameRate { get; set; }
        public string AudioCodec { get; set; }
        public string VideoCodec { get; set; }
        public string Hdr { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Awards { get; set; }
        public string Poster { get; set; }
        public TopCast[] Cast { get; set; }

        public class Rating
        {
            public string Source { get; set; }
            public string Value { get; set; }

        }
        public class TopCast
        {
            public string ActorName { get; set; }
            public string CharacterName { get; set; }
            public string ActorImage { get; set; }
            public string ActorLocalImage { get; set; }

        }

    }
}
