using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaCaelum.Utils
{
    internal class ObservableValue : INotifyPropertyChanged
    {

        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }


        #region INotifyPropertyChangedImplementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            //Raise PropertyChanged event
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
