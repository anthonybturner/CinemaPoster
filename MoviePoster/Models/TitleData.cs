using IMDbApiLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePoster.MovieTypes
{
    public class TitleData : IMDBMovie
    {
        public string OriginalTitle { get; set; }

        public string MovieTense { get; set; }
        public string ReleaseDate { set; get; }
        public string PlotLocal { set; get; }
        public bool PlotLocalIsRtl { set; get; }
        public string Awards { set; get; }
        public string Type { set; get; }
        public List<StarShort> DirectorList { get; set; }
        public string Writers { set; get; }
        public List<StarShort> WriterList { get; set; }
        public List<StarShort> StarList { get; set; }
        public List<ActorShort> ActorList { get; set; }
        public FullCastData FullCast { get; set; }
        public List<KeyValueItem> GenreList { get; set; }
        public string Companies { get; set; }
        public List<CompanyShort> CompanyList { get; set; }
        public string Countries { set; get; }
        public List<KeyValueItem> CountryList { set; get; }
        public string Languages { set; get; }
        public List<KeyValueItem> LanguageList { set; get; }
        public string IMDbRating { get; set; }
        public string IMDbRatingVotes { get; set; }
        public RatingData Ratings { set; get; }
        public WikipediaData Wikipedia { set; get; }
        public PosterData Posters { get; set; }
        public ImageData Images { get; set; }
        public TrailerData Trailer { get; set; }
        public BoxOfficeShort BoxOffice { get; set; }
        public string Tagline { get; set; }
        public string Keywords { get; set; }
        public List<string> KeywordList { get; set; }
        public List<SimilarShort> Similars { get; set; }
        public string ErrorMessage { get; set; }
        public string duration { get; set; }

        public double AspectRatio { get; set; }
        public string AudienceRating { get; set; }
        public string AudienceRatingImage { get; set; }
    }

}
