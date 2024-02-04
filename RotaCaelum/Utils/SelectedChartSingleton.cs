using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaCaelum.Utils
{
    public sealed class SelectedChartSingleton
    {

        private byte select;

        public byte Select
        {
            set { select = value; }
        }

        private byte selected;

        public byte Selected
        {
            get { return select; }
        }



        private SelectedChartSingleton() { }
        private static SelectedChartSingleton instance = null;
        public static SelectedChartSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SelectedChartSingleton();
                }
                return instance;
            }
        }

    }
}
