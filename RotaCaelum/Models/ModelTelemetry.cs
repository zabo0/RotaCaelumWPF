using RotaCaelum.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotaCaelum.Models
{
    internal class ModelTelemetry
    {

        public byte serialNo { get; set; }
        public byte packageNo { get; set; }
        public long time { get; set; }
        public byte status { get; set; }
        public byte error { get; set; }
        public float pressure { get; set; }
        public float altitude { get; set; }
        public float temperature { get; set; }
        public float bataryVolt { get; set; }
        public float velocity { get; set; }
        public float x_accel { get; set; }
        public float y_accel { get; set; }
        public float z_accel { get; set; }
        public float x_gyro { get; set; }
        public float y_gyro { get; set; }
        public float z_gyro { get; set; }
        public float alt_gps { get; set; }
        public float lat_gps { get; set; }
        public float lon_gps { get; set; }
        public byte checkSum { get; set; }



        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}


        //byte serialNo;
        //byte packageNo;
        //long time;
        //byte status;
        //byte error;

        //float pressure;
        //float altitude;
        //float temperature;
        //float bataryVolt;
        //float velocity;

        //float x_accel;
        //float y_accel;
        //float z_accel;

        //float x_gyro;
        //float y_gyro;
        //float z_gyro;

        //float alt_gps;
        //float lat_gps;
        //float lon_gps;

        //byte checkSum;

        //public byte SrNo
        //{
        //    get { return serialNo; }
        //    set { serialNo = value; OnPropertyChanged("SerialNo"); }
        //}

        //public byte PkgNo
        //{
        //    get { return packageNo; }
        //    set { packageNo = value; OnPropertyChanged("PackageNo"); }
        //}

        //public long Time
        //{
        //    get { return time; }
        //    set { time = value; OnPropertyChanged("Time"); }
        //}

        //public byte Status
        //{
        //    get { return status; }
        //    set { status = value; OnPropertyChanged("Status"); }
        //}

        //public byte Error
        //{
        //    get { return error; }
        //    set { error = value; OnPropertyChanged("Error"); }
        //}



        //public float Pressure
        //{
        //    get { return pressure; }
        //    set { pressure = value; OnPropertyChanged("Pressure"); }
        //}



        //public float Altitude
        //{
        //    get { return altitude; }
        //    set { altitude = value; OnPropertyChanged("Altitude"); }
        //}



        //public float Temperature
        //{
        //    get { return temperature; }
        //    set { temperature = value; OnPropertyChanged("Temperature"); }
        //}


        //public float BataryVolt
        //{
        //    get { return bataryVolt; }
        //    set { bataryVolt = value; OnPropertyChanged("BataryVolt"); }
        //}


        //public float Velocity
        //{
        //    get { return velocity; }
        //    set { velocity = value; OnPropertyChanged("Velocity"); }
        //}


        //public float X_accel
        //{
        //    get { return x_accel; }
        //    set { x_accel = value; OnPropertyChanged("X_accel"); }
        //}


        //public float Y_accel
        //{
        //    get { return y_accel; }
        //    set { y_accel = value; OnPropertyChanged("Y_accel"); }
        //}


        //public float Z_accel
        //{
        //    get { return z_accel; }
        //    set { z_accel = value; OnPropertyChanged("Z_accel"); }
        //}


        //public float X_gyro
        //{
        //    get { return x_gyro; }
        //    set { x_gyro = value; OnPropertyChanged("X_gyro"); }
        //}

        //public float Y_gyro
        //{
        //    get { return y_gyro; }
        //    set { y_gyro = value; OnPropertyChanged("Y_gyro"); }
        //}


        //public float Z_gyro
        //{
        //    get { return z_gyro; }
        //    set { z_gyro = value; OnPropertyChanged("Z_gyro"); }
        //}

        //public float Alt_gps
        //{
        //    get { return alt_gps; }
        //    set { alt_gps = value; OnPropertyChanged("Alt_gps"); }
        //}


        //public float Lat_gps
        //{
        //    get { return lat_gps; }
        //    set { lat_gps = value; OnPropertyChanged("Lat_gps"); }
        //}


        //public float Lon_gps
        //{
        //    get { return lon_gps; }
        //    set { lon_gps = value; OnPropertyChanged("Lon_gps"); }
        //}


        //public byte CheckSum
        //{
        //    get { return checkSum; }
        //    set { checkSum = value; OnPropertyChanged("CheckSum"); }
        //}

    }
}
