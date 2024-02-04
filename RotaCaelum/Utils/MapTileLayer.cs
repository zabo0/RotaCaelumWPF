using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaCaelum.Utils
{
    internal class MapTileLayer : Microsoft.Maps.MapControl.WPF.MapTileLayer
    {
        public MapTileLayer()
        {
            TileSource = new MapTileSource();
        }

        public string UriFormat
        {
            get { return TileSource.UriFormat; }
            set { TileSource.UriFormat = value; }
        }
    }
}
