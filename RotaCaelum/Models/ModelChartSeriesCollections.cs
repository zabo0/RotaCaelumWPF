using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaCaelum.Models
{
    internal class ModelChartSeriesCollections
    {

        public SeriesCollection srCollectionPressure { get; set; }
        public SeriesCollection srCollectionPressure_mini { get; set; }

        public SeriesCollection srCollectionAltitude { get; set; }
        public SeriesCollection srCollectionAltitude_mini { get; set; }

        public SeriesCollection srCollectionVelocity { get; set; }
        public SeriesCollection srCollectionVelocity_mini { get; set; }

        public SeriesCollection srCollectionTemperature { get; set; }
        public SeriesCollection srCollectionTemperature_mini { get; set; }

        public SeriesCollection srCollectionGyro { get; set; }
        public SeriesCollection srCollectionGyro_mini { get; set; }

        public SeriesCollection srCollectionAccel { get; set; }
        public SeriesCollection srCollectionAccel_mini { get; set; }

    }
}
