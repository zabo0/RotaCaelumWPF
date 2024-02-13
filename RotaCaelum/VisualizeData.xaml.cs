using LiveCharts;
using RotaCaelum.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;
using RotaCaelum.Models;
using LiveCharts.Wpf;
using Button = System.Windows.Controls.Button;

namespace RotaCaelum
{
    /// <summary>
    /// Interaction logic for VisualizeData.xaml
    /// </summary>
    public partial class VisualizeData : Window
    {

        string filePath;

        ChartValues<float> chartValuesPressure = new ChartValues<float>();
        ChartValues<float> chartValuesAltitude = new ChartValues<float>();
        ChartValues<float> chartValuesVelocity = new ChartValues<float>();
        ChartValues<float> chartValuesTemperature = new ChartValues<float>();

        ChartValues<float> chartValuesGyro_X = new ChartValues<float>();
        ChartValues<float> chartValuesGyro_Y = new ChartValues<float>();
        ChartValues<float> chartValuesGyro_Z = new ChartValues<float>();

        ChartValues<float> chartValuesAccel_X = new ChartValues<float>();
        ChartValues<float> chartValuesAccel_Y = new ChartValues<float>();
        ChartValues<float> chartValuesAccel_Z = new ChartValues<float>();

        private ModelChartSeriesCollections collections;



        public SeriesCollection srCollectionPressure { get; set; }
        public SeriesCollection srCollectionAltitude { get; set; }
        public SeriesCollection srCollectionVelocity { get; set; }
        public SeriesCollection srCollectionTemperature { get; set; }
        public SeriesCollection srCollectionGyro { get; set; }
        public SeriesCollection srCollectionAccel { get; set; }


        public VisualizeData()
        {
            InitializeComponent();


            
        }

        private void browseFile(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filePath = dialog.FileName;
            }
        }

        private void button_initialize_Click(object sender, RoutedEventArgs e)
        {

            clearChart();

            progressBar.Visibility = Visibility.Visible;
            progressBar.Value = 0;

            var allLines = File.ReadAllLines(filePath);

            foreach(var line in allLines)
            {
                

                var stringDatas = line.TrimStart('<').TrimEnd('>').Split('-');
                byte[] byteDatas = new byte[stringDatas.Length];


                for(int i = 0; i<stringDatas.Length; i++)
                {
                    byteDatas[i] = byte.Parse(stringDatas[i], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    progressBar.Value += 1;
                }
                

                chartValuesPressure.Add(getFloat(byteDatas, 9));
                chartValuesAltitude.Add(getFloat(byteDatas, 13));
                chartValuesTemperature.Add(getFloat(byteDatas, 17));
                chartValuesVelocity.Add(getFloat(byteDatas, 25));

                chartValuesAccel_X.Add(getFloat(byteDatas, 29));
                chartValuesAccel_Y.Add(getFloat(byteDatas, 33));
                chartValuesAccel_Z.Add(getFloat(byteDatas, 37));

                chartValuesGyro_X.Add(getFloat(byteDatas, 41));
                chartValuesGyro_Y.Add(getFloat(byteDatas, 45));
                chartValuesGyro_Z.Add(getFloat(byteDatas, 59));

                progressBar.Value += 100 / allLines.Length;

            }

            initializeChart();

            

            progressBar.Visibility = Visibility.Hidden;

            button_Pressure.IsEnabled = true;
            button_Altitude.IsEnabled = true;
            button_Velocity.IsEnabled = true;
            button_Temperature.IsEnabled = true;
            button_Gyro.IsEnabled = true;
            button_Accel.IsEnabled = true;

        }

        private float getFloat(byte[] data, int startIndex)
        {
            return BitConverter.ToSingle(data, startIndex);
        }


      


        private void initializeChart()
        {
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
        }

        private void getChart(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            switch (button.Name)
            {
                case "button_Pressure":
                    {
                        Chart.Series = srCollectionPressure;
                        break;
                    }
                case "button_Altitude":
                    {
                        Chart.Series = srCollectionAltitude;
                        break;
                    }
                case "button_Velocity":
                    {
                        Chart.Series = srCollectionVelocity;
                        break;
                    }
                case "button_Temperature":
                    {
                        Chart.Series = srCollectionTemperature;
                        break;
                    }
                case "button_Gyro":
                    {
                        Chart.Series = srCollectionGyro;
                        break;
                    }
                case "button_Accel":
                    {
                        Chart.Series = srCollectionAccel;
                        break;
                    }
            }
        }

        private void clearChart()
        {
            chartValuesPressure.Clear();
            chartValuesAltitude.Clear();
            chartValuesVelocity.Clear();
            chartValuesTemperature.Clear();

            chartValuesGyro_X.Clear();
            chartValuesGyro_Y.Clear();
            chartValuesGyro_Z.Clear();

            chartValuesAccel_X.Clear();
            chartValuesAccel_Y.Clear();
            chartValuesAccel_Z.Clear();

            Chart.Series.Clear();
        }
    }
}
