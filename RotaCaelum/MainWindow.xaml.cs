using MapControl.Caching;
using MapControl;
using RotaCaelum.UserControls;
using RotaCaelum.Utils;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using RotaCaelum.ViewModels;
using LiveCharts.Wpf;
using LiveCharts;
using System.ComponentModel;
using RotaCaelum.Models;
using System.Diagnostics;
using System.Threading;

namespace RotaCaelum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// ofline harita indirebilmek icin -> https://egorikas.com/download-open-street-tiles-for-offline-using/
    /// </summary>
    public partial class MainWindow : Window
    {


        private ViewModelTelemetry viewModelTelemetry;
        private ModelTelemetry telemetry;

        public double[] data = new double[100_000];
        int nextDataIndex = 1;
        SignalPlot signalPlot;
        Random rand = new Random(0);


        private DispatcherTimer _updateDataTimer;
        private DispatcherTimer _renderTimer;


        private SelectedChartSingleton chart = SelectedChartSingleton.Instance;



        public MainWindow()
        {
            InitializeComponent();

            viewModelTelemetry = new ViewModelTelemetry();
            DataContext = viewModelTelemetry;
            telemetry = new ModelTelemetry();


            startCharts();


        }



        public void startCharts()
        {
            //var plt = new ScottPlot.Plot(600, 400);

            //double[] Xdata = new double[] { 1, 2, 3, 4, 5 };
            //double[] Ydata = new double[] { 1, 4, 9, 16, 25 };

            //initializeCart(pressureChart, "", "Pressure", Xdata, Ydata);
            //initializeCart(altitudeChart, "", "Altitude", Xdata, Ydata);
            //initializeCart(velocityChart, "", "Velocity", Xdata, Ydata);
            //initializeCart(temperatureChart, "", "Temp.", Xdata, Ydata);
            //initializeCart(gyroChart, "", "Gyro.", Xdata, Ydata);
            //initializeCart(accelChart, "", "Accel.", Xdata, Ydata);


            //double[] valuesX = DataGen.RandomWalk(1_000_000);
            //double[] valuesY = DataGen.RandomWalk(1_000_000);
        }


        void initializeCart(WpfPlot chart, String XLabel, String YLabel, double[] dataX, double[] dataY)
        {
            chart.Plot.AddScatter(dataX, dataY);
            //chart.Plot.XLabel(XLabel);
            //chart.Plot.YLabel(YLabel);
            chart.Plot.AxisAuto();
            chart.Plot.XAxis.Ticks(false);
            chart.Configuration.LeftClickDragPan = false;
            chart.Configuration.ScrollWheelZoom = false;
            chart.Configuration.DoubleClickBenchmark = false;
            //chart.Plot.Frameless();
            chart.Refresh();
        }


        private void expandChart(object sender, RoutedEventArgs e)
        {
            //WpfPlot chart = (WpfPlot)sender;

            CartesianChart chart = (CartesianChart)sender;

            switch (chart.Name) {
                case "pressureChart":
                    {

                        this.chart.Select = 1;
                        CallUserControl.addControlToGrid(expandedPlace, new uc_pressure());


                        break;
                    }

                case "altitudeChart":
                    {

                        this.chart.Select = 2;
                        CallUserControl.addControlToGrid(expandedPlace, new uc_altitude()); 
                        break;

                    }

                case "velocityChart":
                    {

                        this.chart.Select = 3;
                        CallUserControl.addControlToGrid(expandedPlace, new uc_velocity());
                        break;
                    }
                case "temperatureChart":
                    {
                        this.chart.Select=4;
                        CallUserControl.addControlToGrid(expandedPlace, new uc_temperature()); 
                        break;
                    }
                case "gyroChart":
                    {
                        this.chart.Select = 5;
                        CallUserControl.addControlToGrid(expandedPlace, new uc_gyro()); 
                        break;
                    }
                case "accelChart":
                    {
                        this.chart.Select = 6;
                        CallUserControl.addControlToGrid(expandedPlace, new uc_acceleration()); 
                        break;
                    }

            }
            
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;

            //if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            //{
            //    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
            //    e.Handled = true;
            //}

            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        private void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            if (button_start.Content.Equals("START"))
            {

                startMission();


                button_start.Content = "FINISH";
                button_start.Background = Brushes.Red;

                groupBox_saveData.IsEnabled = false;
                groupBox_firstDeploy.IsEnabled = false;
                groupBox_secondDeploy.IsEnabled = false;
                groupBox_settings.IsEnabled = false;

            }
            else if (button_start.Content.Equals("FINISH"))
            {

                startMission();

                button_start.Content = "START";
                button_start.Background = Brushes.Lime;

                groupBox_saveData.IsEnabled = true;
                groupBox_firstDeploy.IsEnabled = true;
                groupBox_secondDeploy.IsEnabled = true;
                groupBox_settings.IsEnabled = true;

            }
        }

        private void startMission()
        {

            for (int i = 0; i < 10; i++)
            {
                viewModelTelemetry.getTelemetry();
            }
        }


        private void ManuelDeploy_1Clicked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                viewModelTelemetry.getTelemetry();
            }
        }

        private void ManuelDeploy_2Clicked(object sender, RoutedEventArgs e)
        {
            viewModelTelemetry.openFile(); 
        }
    }


    
}
