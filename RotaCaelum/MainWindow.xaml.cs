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

namespace RotaCaelum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public double[] data = new double[100_000];
        int nextDataIndex = 1;
        SignalPlot signalPlot;
        Random rand = new Random(0);


        private DispatcherTimer _updateDataTimer;
        private DispatcherTimer _renderTimer;

        public MainWindow()
        {
            InitializeComponent();


            startCharts();


            // plot the data array only once
            //signalPlot = pressure.Plot.AddSignal(data);
            //pressure.Plot.YLabel("Value");
            //pressure.Plot.XLabel("Sample Number");
            //pressure.Refresh();

            // create a timer to modify the data
            _updateDataTimer = new DispatcherTimer();
            _updateDataTimer.Interval = TimeSpan.FromMilliseconds(1);
            _updateDataTimer.Tick += UpdateData;
            _updateDataTimer.Start();

            // create a timer to update the GUI
            _renderTimer = new DispatcherTimer();
            _renderTimer.Interval = TimeSpan.FromMilliseconds(20);
            _renderTimer.Tick += Render;
            _renderTimer.Start();

            Closed += (sender, args) =>
            {
                _updateDataTimer?.Stop();
                _renderTimer?.Stop();
            };

        }

        void UpdateData(object sender, EventArgs e)
        {
            if (nextDataIndex >= data.Length)
            {
                throw new OverflowException("data array isn't long enough to accomodate new data");
                // in this situation the solution would be:
                //   1. clear the plot
                //   2. create a new larger array
                //   3. copy the old data into the start of the larger array
                //   4. plot the new (larger) array
                //   5. continue to update the new array
            }

            //double randomValue = Math.Round(rand.NextDouble() - .5, 3);
            //double latestValue = data[nextDataIndex - 1] + randomValue;
            //data[nextDataIndex] = latestValue;
            //signalPlot.MaxRenderIndex = nextDataIndex;
            ////ReadingsTextbox.Text = $"{nextDataIndex + 1}";
            ////LatestValueTextbox.Text = $"{latestValue:0.000}";
            //nextDataIndex += 1;
        }

        void Render(object sender, EventArgs e)
        {
            //if(checkBox_EMERGENCY_RESCUE_MODE.IsChecked == true)
            //{
            //    pressure.Plot.AxisAuto();
            //}
            //pressure.Refresh();
        }

        private void DisableAutoAxis(object sender, RoutedEventArgs e)
        {
            //pressure.Plot.AxisAuto(verticalMargin: .5);
            //var oldLimits = pressure.Plot.GetAxisLimits();
            //pressure.Plot.SetAxisLimits(xMax: oldLimits.XMax + 1000);
        }




        public void startCharts()
        {
            var plt = new ScottPlot.Plot(600, 400);

            double[] Xdata = new double[] { 1, 2, 3, 4, 5 };
            double[] Ydata = new double[] { 1, 4, 9, 16, 25 };

            initializeCart(pressureChart, "", "Pressure", Xdata, Ydata);
            initializeCart(altitudeChart, "", "Altitude", Xdata, Ydata);
            initializeCart(velocityChart, "", "Velocity", Xdata, Ydata);
            initializeCart(temperatureChart, "", "Temp.", Xdata, Ydata);
            initializeCart(gyroChart, "", "Gyro.", Xdata, Ydata);
            initializeCart(accelChart, "", "Accel.", Xdata, Ydata);


            double[] valuesX = DataGen.RandomWalk(1_000_000);
            double[] valuesY = DataGen.RandomWalk(1_000_000);
        }


        void initializeCart(WpfPlot chart, String XLabel, String YLabel, double[] dataX, double[] dataY)
        {
            chart.Plot.AddScatter(dataX, dataY);
            chart.Plot.XLabel(XLabel);
            chart.Plot.YLabel(YLabel);
            chart.Plot.AxisAuto();
            chart.Plot.XAxis.Ticks(false);
            chart.Configuration.LeftClickDragPan = false;
            chart.Configuration.ScrollWheelZoom = false;
            chart.Configuration.DoubleClickBenchmark = false;
            //chart.Plot.Frameless();
            chart.Refresh();
        }

        private void pressureChartClicked(object sender, RoutedEventArgs e)
        {
            CallUserControl.addControlToGrid(expandedPlace, new uc_pressure());
        }
    }


    
}
