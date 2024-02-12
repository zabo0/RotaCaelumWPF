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
using System.IO.Ports;
using System.Collections;
using BruTile.Wms;

namespace RotaCaelum.ViewModels
{
    internal class ViewModelTelemetry : BaseViewModel
    {
        private Properties.Settings settings = Properties.Settings.Default;



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

        private DataFileWriter dataFileWriter = DataFileWriter.Instance;
        int index = 0;

        bool isReadingPort = false;

        private ModelTelemetry telemetry;
        private ModelChartSeriesCollections collections;
        private ModelStatus modelStatus;

        private readonly BackgroundWorker comPortWorker = new BackgroundWorker();
        private SerialComminication serialComminication = SerialComminication.Instance;



        Random rand = new Random(0);

        public ViewModelTelemetry()
        {
            InitializeLineSeries();
            telemetry = new ModelTelemetry();
            collections = new ModelChartSeriesCollections();
            modelStatus = new ModelStatus();
            serialComminication.serialComPort.DataReceived += SerialComPort_DataReceived;

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
        public int packageNo
        {
            get => telemetry.packageNo;
            set { telemetry.packageNo = value; OnPropertyChanged(nameof(packageNo)); }
        }
        public float time
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


        public byte ready
        {
            get => modelStatus.ready;
            set { modelStatus.ready = value; OnPropertyChanged(nameof(ready)); }
        }
        public byte takeOff
        {
            get => modelStatus.takeOff;
            set { modelStatus.takeOff = value; OnPropertyChanged(nameof(takeOff)); }
        }
        public byte ascent
        {
            get => modelStatus.ascent;
            set { modelStatus.ascent = value; OnPropertyChanged(nameof(ascent)); }
        }
        public byte firstDeploy
        {
            get => modelStatus.firstDeploy;
            set { modelStatus.firstDeploy = value; OnPropertyChanged(nameof(firstDeploy)); }
        }
        public byte drag
        {
            get => modelStatus.drag;
            set { modelStatus.drag = value; OnPropertyChanged(nameof(drag)); }
        }
        public byte secondDeploy
        {
            get => modelStatus.secondDeploy;
            set { modelStatus.secondDeploy = value; OnPropertyChanged(nameof(secondDeploy)); }
        }
        public byte descent
        {
            get => modelStatus.descent;
            set { modelStatus.descent = value; OnPropertyChanged(nameof(descent)); }
        }
        public byte landed
        {
            get => modelStatus.landed;
            set { modelStatus.landed = value; OnPropertyChanged(nameof(landed)); }
        }

        private string _statusString;
        public string statusString 
        {
            get { return _statusString; }
            set { _statusString = value; OnPropertyChanged(nameof(statusString)); }
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


        private void SerialComPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (serialComminication.serialComPort.IsOpen && isReadingPort)
                {
                    byte[] headerBuffer = new byte[2];
                    serialComminication.serialComPort.Read(headerBuffer, 0, headerBuffer.Length);


                    if (headerBuffer[0] != 0xE0 || headerBuffer[1] != 0xE1)
                    {
                        Console.WriteLine("Throwing invalid header: " + BitConverter.ToString(headerBuffer));
                        serialComminication.serialComPort.DiscardInBuffer();
                        return;
                    }

                    byte[] dataBuffer = new byte[66];

                    for (int i = 0; i < 66; i++)
                    {
                        serialComminication.serialComPort.ReadTimeout = 500;
                        dataBuffer[i] = (byte)serialComminication.serialComPort.ReadByte();
                    }

                    dataFileWriter.writeDataPort(dataBuffer);

                    //gelen byte datasini ayristir.

                    index++;


                    serialNo = dataBuffer[0];
                    packageNo = BitConverter.ToUInt16(dataBuffer, 1);
                    time = getFloat(dataBuffer, 3);
                    status = dataBuffer[7];
                    statusString = Convert.ToString(status, 2).PadLeft(8, '0'); 
                    error = dataBuffer[8];
                    pressure = getFloat(dataBuffer, 9);
                    altitude = getFloat(dataBuffer, 13);
                    temperature = getFloat(dataBuffer, 17);
                    bataryVolt = getFloat(dataBuffer, 21);
                    velocity = getFloat(dataBuffer, 25);
                    x_accel = getFloat(dataBuffer, 29);
                    y_accel = getFloat(dataBuffer, 33);
                    z_accel = getFloat(dataBuffer, 37);
                    x_gyro = getFloat(dataBuffer, 41);
                    y_gyro = getFloat(dataBuffer, 45);
                    z_gyro = getFloat(dataBuffer, 49);
                    alt_gps = getFloat(dataBuffer, 54);
                    lat_gps = getFloat(dataBuffer, 57);
                    lon_gps = getFloat(dataBuffer, 61);
                    checkSum = dataBuffer[65];

                    

                    //ready = statusArray[0];

                    //string[] dataPackageForDataFile = new string[21];
                    //dataPackageForDataFile[0] = index.ToString();
                    //dataPackageForDataFile[1] = serialNo.ToString();
                    //dataPackageForDataFile[2] = packageNo.ToString();
                    //dataPackageForDataFile[3] = time.ToString();
                    //dataPackageForDataFile[4] = status.ToString();
                    //dataPackageForDataFile[5] = error.ToString();
                    //dataPackageForDataFile[6] = pressure.ToString();
                    //dataPackageForDataFile[7] = altitude.ToString();
                    //dataPackageForDataFile[8] = temperature.ToString();
                    //dataPackageForDataFile[9] = bataryVolt.ToString();
                    //dataPackageForDataFile[10] = velocity.ToString();
                    //dataPackageForDataFile[11] = x_accel.ToString();
                    //dataPackageForDataFile[12] = y_accel.ToString();
                    //dataPackageForDataFile[13] = z_accel.ToString();
                    //dataPackageForDataFile[14] = x_gyro.ToString();
                    //dataPackageForDataFile[15] = y_gyro.ToString();
                    //dataPackageForDataFile[16] = z_gyro.ToString();
                    //dataPackageForDataFile[17] = alt_gps.ToString();
                    //dataPackageForDataFile[18] = lat_gps.ToString();
                    //dataPackageForDataFile[19] = lon_gps.ToString();
                    //dataPackageForDataFile[20] = checkSum.ToString();
                    
                    //dataFileWriter.writeData(dataPackageForDataFile);

                    if (index >= 500)
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

                    chartValuesPressure.Add(telemetry.pressure);
                    chartValuesAltitude.Add(telemetry.altitude);
                    chartValuesVelocity.Add(telemetry.velocity);
                    chartValuesTemperature.Add(telemetry.temperature);

                    chartValuesAccel_X.Add(telemetry.x_accel);
                    chartValuesAccel_Y.Add(telemetry.y_accel);
                    chartValuesAccel_Z.Add(telemetry.z_accel);

                    chartValuesGyro_X.Add(telemetry.x_gyro);
                    chartValuesGyro_Y.Add(telemetry.y_gyro);
                    chartValuesGyro_Z.Add(telemetry.z_gyro);
                }
            }
            catch(System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private float getFloat(byte[] data, int startIndex)
        {
            return BitConverter.ToSingle(data, startIndex);
        }

        public void startReadingPort()
        {
            isReadingPort = true;
        }

        public void stopReadingPort()
        {
            isReadingPort = false;
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


        public void sendDeploymentConfigs(Action<bool> result)
        {
            var configsPackage = createPackageDeploymentConfigs();
            serialComminication.serialComPort.WriteTimeout = 200;
            for(byte i = 0; i < 10; i++)
            {
                serialComminication.serialComPort.Write(configsPackage, 0, configsPackage.Length);
            }
            result(true);
        }

        private byte[] createPackageDeploymentConfigs()
        {
            byte[] package = new byte[16];

            //packageType: 37 = deployment configs
            package[0] = getBytes(37)[0];
            //first_pitchAngle
            package[1] = getBytes(settings.FirstDeployment_pitchAngle)[0];
            package[2] = getBytes(settings.FirstDeployment_pitchAngle)[1];
            //first_velocity
            package[3] = getBytes(settings.FirstDeployment_velocity)[0];
            package[4] = getBytes(settings.FirstDeployment_velocity)[1];
            //first_altitude
            package[5] = getBytes(settings.FirstDeployment_altitude)[0];
            package[6] = getBytes(settings.FirstDeployment_altitude)[1];
            //first_apogee true,false
            package[7] = getBytes(Convert.ToByte(settings.FirstDeployment_apogee))[0];
            //second_pitchAngle
            package[8] = getBytes(settings.SecondDeployment_pitchAngle)[0];
            package[9] = getBytes(settings.FirstDeployment_pitchAngle)[1];
            //second_velocity
            package[10] = getBytes(settings.SecondDeployment_velocity)[0];
            package[11] = getBytes(settings.SecondDeployment_velocity)[1];
            //second_altitude
            package[12] = getBytes(settings.SecondDeployment_altitude)[0];
            package[13] = getBytes(settings.SecondDeployment_altitude)[1];
            //second_dontUseSecond true, false
            package[14] = getBytes(Convert.ToByte(settings.SecondDeployment_dontUse))[0];
            //checkSum
            package[15] = getBytes(calculateCRC(package))[0];


            return package;
        }

        private byte[] getBytes(Int16 value)
        {
            var buffer = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                return buffer;
            }
            //return new[] { buffer[0], buffer[1], buffer[2], buffer[3] };
            return new[] { buffer[0], buffer[1] };
        }

        byte calculateCRC(byte[] package)
        {
            int check_sum = 0;
            for (int i = 1; i < 13; i++)
            {
                check_sum += package[i];
            }
            return Convert.ToByte(check_sum % 256);
        }

        public void clearCharts()
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
        }
    }

}
