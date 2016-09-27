using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.Data.POCOs
{
    public class AlbumLength
    {
        public string Title { get; set; }
        public int numTracks { get; set; }
        public double? priceOfAlbum { get; set; }
        public double averageTrackLengthSec { get; set; }
        public double averageTrackLengthSecNoD { get; set; }
    }
}
