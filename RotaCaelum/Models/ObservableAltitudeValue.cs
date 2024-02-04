using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaCaelum.Models
{
    internal class ObservableAltitudeValue : INotifyPropertyChanged
    {
        private double _value;

        public double Altitude
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Altitude"); // Called OnPropertyChanged()
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
