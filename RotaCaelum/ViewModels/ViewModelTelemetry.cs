using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using RotaCaelum.Models;
using RotaCaelum.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace RotaCaelum.ViewModels
{
    internal class ViewModelTelemetry : BaseViewModel
    {
        private SelectedChartSingleton chart = SelectedChartSingleton.Instance;

        ChartValues<float> chartValuesPressure = new ChartValues<float>();
        ChartValues<float> chartValuesAltitude = new ChartValues<float>();
        ChartValues<float> chartValuesVelocity = new ChartValues<float>();
        ChartValues<float> chartValuesTemperature = new ChartValues<float>();

        ChartValues<float> chartValuesGyro_X = new ChartValues<float>();
        ChartValues<float> chartValuesGyro_Y = new ChartValues<float>();
        ChartValues<float> chartValuesGyro_Z = new ChartValues<float>();

        ChartValues <float> chartValuesAccel_X = new ChartValues<float>();
        ChartValues <float> chartValuesAccel_Y = new ChartValues<float>();
        ChartValues <float> chartValuesAccel_Z = new ChartValues<float>();

        StreamWriter sw;
        int index = 0;

        float[] dataPressure = new float[1000];
        float[] dataAltitude = new float[1000];
        float[] dataVelocity = new float[1000];
        float[] dataTemperature = new float[1000];
        float[] dataGyro_X = new float[1000];
        float[] dataGyro_Y = new float[1000];
        float[] dataGyro_Z = new float[1000];
        float[] dataAccel_X = new float[1000];
        float[] dataAccel_Y = new float[1000];
        float[] dataAccel_Z = new float[1000];


        private ModelTelemetry telemetry;
        private ModelChartSeriesCollections collections;

        private readonly BackgroundWorker worker = new BackgroundWorker();

        Random rand = new Random(0);

        public ViewModelTelemetry()
        {
            InitializeLineSeries();
            telemetry = new ModelTelemetry();
            collections = new ModelChartSeriesCollections();
            

            srCollectionPressure = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pressure",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesPressure
                }
            };
            srCollectionPressure_mini = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pressure",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesPressure
                }
            };
            srCollectionAltitude = new SeriesCollection
            {
                 new LineSeries
                {
                    Title = "Altitude",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesAltitude
                }
            };
            srCollectionAltitude_mini = new SeriesCollection
            {
                 new LineSeries
                {
                    Title = "Altitude",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesAltitude
                }
            };
            srCollectionVelocity = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Velocity",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesVelocity
                }
            };
            srCollectionVelocity_mini = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Velocity",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesVelocity
                }
            };
            srCollectionTemperature = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Temperature",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesTemperature
                }
            };
            srCollectionTemperature_mini = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Temperature",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesTemperature
                }
            };
            srCollectionGyro = new SeriesCollection
            {
                new LineSeries
                {
                    Title ="X Gyro",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesGyro_X
                },
                new LineSeries
                {
                    Title ="Y Gyro",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesGyro_Y
                },
                new LineSeries
                {
                    Title ="Z Gyro",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesGyro_Z
                }
            };
            srCollectionGyro_mini = new SeriesCollection
            {
                new LineSeries
                {
                    Title ="X Gyro",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesGyro_X
                },
                new LineSeries
                {
                    Title ="Y Gyro",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesGyro_Y
                },
                new LineSeries
                {
                    Title ="Z Gyro",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesGyro_Z
                }
            };
            srCollectionAccel = new SeriesCollection{
                new LineSeries
                {
                    Title = "X Accel",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesAccel_X
                },
                new LineSeries
                {
                    Title = "Y Accel",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesAccel_Y
                },
                new LineSeries
                {
                    Title = "Z Accel",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesAccel_Z
                }
            };
            srCollectionAccel_mini = new SeriesCollection{
                new LineSeries
                {
                    Title = "X Accel",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesAccel_X
                },
                new LineSeries
                {
                    Title = "Y Accel",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesAccel_Y
                },
                new LineSeries
                {
                    Title = "Z Accel",
                    PointGeometry = null,
                    Fill = null,
                    Values = chartValuesAccel_Z
                }
            };



        }

        public byte serialNo
        {
            get => telemetry.serialNo;
            set { telemetry.serialNo = value; OnPropertyChanged(nameof(serialNo)); }
        }
        public byte packageNo
        {
            get => telemetry.packageNo;
            set { telemetry.packageNo = value; OnPropertyChanged(nameof(packageNo)); }
        }
        public long time
        {
            get => telemetry.time;
            set { telemetry.time = value; OnPropertyChanged(nameof(time)); }
        }
        public byte status
        {
            get => telemetry.status;
            set { telemetry.status = value; OnPropertyChanged(nameof(status)); }
        }
        public byte error
        {
            get => telemetry.error;
            set { telemetry.error = value; OnPropertyChanged(nameof(error)); }
        }
        public float pressure
        {
            get => telemetry.pressure;
            set { telemetry.pressure = value; OnPropertyChanged(nameof(pressure)); }
        }
        public float altitude
        {
            get => telemetry.altitude;
            set { telemetry.altitude = value; OnPropertyChanged(nameof(altitude)); }
        }
        public float temperature
        {
            get => telemetry.temperature;
            set { telemetry.temperature = value; OnPropertyChanged(nameof(temperature)); }
        }
        public float bataryVolt
        {
            get => telemetry.bataryVolt;
            set { telemetry.bataryVolt = value; OnPropertyChanged(nameof(bataryVolt)); }
        }
        public float velocity
        {
            get => telemetry.velocity;
            set { telemetry.velocity = value; OnPropertyChanged(nameof(velocity)); }
        }
        public float x_accel
        {
            get => telemetry.x_accel;
            set { telemetry.x_accel = value; OnPropertyChanged(nameof(x_accel)); }
        }
        public float y_accel
        {
            get => telemetry.y_accel;
            set { telemetry.y_accel = value; OnPropertyChanged(nameof(y_accel)); }
        }
        public float z_accel
        {
            get => telemetry.z_accel;
            set { telemetry.z_accel = value; OnPropertyChanged(nameof(z_accel)); }
        }
        public float x_gyro
        {
            get => telemetry.x_gyro;
            set { telemetry.x_gyro = value; OnPropertyChanged(nameof(x_gyro)); }
        }
        public float y_gyro
        {
            get => telemetry.y_gyro;
            set { telemetry.y_gyro = value; OnPropertyChanged(nameof(y_gyro)); }
        }
        public float z_gyro
        {
            get => telemetry.z_gyro;
            set { telemetry.z_gyro = value; OnPropertyChanged(nameof(z_gyro)); }
        }
        public float alt_gps
        {
            get => telemetry.alt_gps;
            set { telemetry.alt_gps = value; OnPropertyChanged(nameof(alt_gps)); }
        }
        public float lat_gps
        {
            get => telemetry.lat_gps;
            set { telemetry.lat_gps = value; OnPropertyChanged(nameof(lat_gps)); }
        }
        public float lon_gps
        {
            get => telemetry.lon_gps;
            set { telemetry.lon_gps = value; OnPropertyChanged(nameof(lon_gps)); }
        }
        public byte checkSum
        {
            get => telemetry.checkSum;
            set { telemetry.checkSum = value; OnPropertyChanged(nameof(checkSum)); }
        }

        public SeriesCollection srCollectionPressure
        {
            get => collections.srCollectionPressure;
            set { collections.srCollectionPressure = value; OnPropertyChanged(nameof(srCollectionPressure)); }
        }
        public SeriesCollection srCollectionPressure_mini
        {
            get => collections.srCollectionPressure_mini;
            set { collections.srCollectionPressure_mini = value; OnPropertyChanged(nameof(srCollectionPressure_mini)); }
        }
        public SeriesCollection srCollectionAltitude
        {
            get => collections.srCollectionAltitude;
            set { collections.srCollectionAltitude = value; OnPropertyChanged(nameof(srCollectionAltitude)); }
        }
        public SeriesCollection srCollectionAltitude_mini
        {
            get => collections.srCollectionAltitude_mini;
            set { collections.srCollectionAltitude_mini = value; OnPropertyChanged(nameof(srCollectionAltitude_mini)); }
        }
        public SeriesCollection srCollectionVelocity
        {
            get => collections.srCollectionVelocity;
            set { collections.srCollectionVelocity = value; OnPropertyChanged(nameof(srCollectionVelocity)); }
        }
        public SeriesCollection srCollectionVelocity_mini
        {
            get => collections.srCollectionVelocity_mini;
            set { collections.srCollectionVelocity_mini = value; OnPropertyChanged(nameof(srCollectionVelocity_mini)); }
        }
        public SeriesCollection srCollectionTemperature
        {
            get => collections.srCollectionTemperature;
            set { collections.srCollectionTemperature = value; OnPropertyChanged(nameof(srCollectionTemperature)); }
        }
        public SeriesCollection srCollectionTemperature_mini
        {
            get => collections.srCollectionTemperature_mini;
            set { collections.srCollectionTemperature_mini = value; OnPropertyChanged(nameof(srCollectionTemperature_mini)); }
        }
        public SeriesCollection srCollectionGyro
        {
            get => collections.srCollectionGyro;
            set { collections.srCollectionGyro = value; OnPropertyChanged(nameof(srCollectionGyro)); }
        }
        public SeriesCollection srCollectionGyro_mini
        {
            get => collections.srCollectionGyro_mini;
            set { collections.srCollectionGyro_mini = value; OnPropertyChanged(nameof(srCollectionGyro_mini)); }
        }
        public SeriesCollection srCollectionAccel
        {
            get => collections.srCollectionAccel;
            set { collections.srCollectionAccel = value; OnPropertyChanged(nameof(srCollectionAccel)); }
        }
        public SeriesCollection srCollectionAccel_mini
        {
            get => collections.srCollectionAccel_mini;
            set { collections.srCollectionAccel_mini = value; OnPropertyChanged(nameof(srCollectionAccel_mini)); }
        }
        


        public void getTelemetry()
        {
            index++;

            serialNo = 001;
            packageNo = 1;
            time = 14;
            status = 3;
            error = 4;
            pressure = NextFloat(rand);
            altitude = NextFloat(rand);
            temperature = NextFloat(rand);
            bataryVolt = NextFloat(rand);
            velocity = NextFloat(rand);
            x_accel = NextFloat(rand);
            y_accel = NextFloat(rand);
            z_accel = NextFloat(rand);
            x_gyro = NextFloat(rand);
            y_gyro = NextFloat(rand);
            z_gyro = NextFloat(rand);
            alt_gps = NextFloat(rand);
            lat_gps = NextFloat(rand);
            lon_gps = NextFloat(rand);
            checkSum = 32;


            if(sw != null)
            {
                sw.WriteLine("<" + index + "|" + serialNo + "|" + packageNo + "|" + time + "|" + status + "|" +
                     error + "|" + pressure + "|" + altitude + "|" + temperature + "|" + bataryVolt + "|" + velocity + "|" +
                     x_accel + "|" + y_accel + "|" + z_accel + "|" + x_gyro + "|" + y_gyro + "|" + z_gyro + "|" + alt_gps + "|" + lat_gps + "|" + lon_gps + "|" + checkSum + ">");
            }
            else
            {
                MessageBox.Show("Exception: NullPointerExeption" );
            }


            if (index >= 1000)
            {
                chartValuesPressure.RemoveAt(0);
                chartValuesAltitude.RemoveAt(0);
                chartValuesVelocity.RemoveAt(0);
                chartValuesTemperature.RemoveAt(0);
                chartValuesAccel_X.RemoveAt(0);
                chartValuesAccel_Y.RemoveAt(0);
                chartValuesAccel_Z.RemoveAt(0);
                chartValuesGyro_X.RemoveAt(0);
                chartValuesGyro_Y.RemoveAt(0);
                chartValuesGyro_Z.RemoveAt(0);
            }
            


            chartValuesPressure.Add(chartValuesPressure.LastOrDefault() + telemetry.pressure);
            chartValuesAltitude.Add(chartValuesAltitude.LastOrDefault() + telemetry.altitude);
            chartValuesVelocity.Add(chartValuesVelocity.LastOrDefault() + telemetry.velocity);
            chartValuesTemperature.Add(chartValuesTemperature.LastOrDefault() + telemetry.temperature);

            chartValuesAccel_X.Add(chartValuesAccel_X.LastOrDefault() + telemetry.x_accel);
            chartValuesAccel_Y.Add(chartValuesAccel_Y.LastOrDefault() + telemetry.y_accel);
            chartValuesAccel_Z.Add(chartValuesAccel_Z.LastOrDefault() + telemetry.z_accel);

            chartValuesGyro_X.Add(chartValuesGyro_X.LastOrDefault() + telemetry.x_gyro);
            chartValuesGyro_Y.Add(chartValuesGyro_Y.LastOrDefault() + telemetry.y_gyro);
            chartValuesGyro_Z.Add(chartValuesGyro_Z.LastOrDefault() + telemetry.z_gyro);


            switch (chart.Selected)
            {
                case 1:
                    {
                        //SrCollection.Clear();
                        //SrCollection = new SeriesCollection
                        //{
                        //    new LineSeries
                        //    {
                        //        Title = "Pressure",
                        //        PointGeometry = null,
                        //        Values = chartValuesPressure
                        //    }
                        //};

                        break;
                    }
                case 2:
                    {
                        //SrCollectionAltitude.Clear();
                        //SrCollectionAltitude = new SeriesCollection
                        //{
                        //    new LineSeries
                        //    {
                        //        Title = "Altitude",
                        //        PointGeometry = null,
                        //        Values = chartValuesAltitude
                        //    }
                        //};

                        break;
                    }
                case 3:
                    {

                        break;
                    }
                case 4:
                    {
                        break;
                    }
                case 5:
                    {
                        break;
                    }
                case 6:
                    {
                        break;
                    }
            }
        }

        public void openFile()
        {
            try
            {
                //C:\Users\scice\Desktop
                string fileName = "" + DateTime.Now.ToString("yyyy.MM.dd_hh.mm.ss") + ".txt";
                string filePath = "C:\\Users\\scice\\Desktop\\telemetry\\" + fileName;
                sw = new StreamWriter(filePath, true);
                sw.WriteLine("<file opened>");

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                MessageBox.Show("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        public void closeFile()
        {
            sw.Close();
        }

        static float NextFloat(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            // choose -149 instead of -126 to also generate subnormal floats (*)
            double exponent = Math.Pow(2.0, random.Next(-10, 10));
            return (float)(mantissa * exponent);
        }


        private void InitializeLineSeries()
        {
            chartValuesPressure = new ChartValues<float>();
            chartValuesAltitude = new ChartValues<float>();
            chartValuesVelocity = new ChartValues<float>();
            chartValuesTemperature = new ChartValues<float>();
            chartValuesGyro_X = new ChartValues<float>();
            chartValuesGyro_Y = new ChartValues<float>();
            chartValuesGyro_Z = new ChartValues<float>();
            chartValuesAccel_X = new ChartValues<float>();
            chartValuesAccel_Y = new ChartValues<float>();
            chartValuesAccel_Z = new ChartValues<float>();
        }
    }
}
