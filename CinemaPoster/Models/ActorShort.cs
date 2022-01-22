using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPoster.Models
{

    [Serializable()]
    public class ActorShort
    {
        public ActorShort()
        {
        }

        public string Id { get; set; }
        public string Image { get; set; }
        public string ActorLocalImage { get; set; }
        public string Name { get; set; }
        public string AsCharacter { get; set; }
    }
}
