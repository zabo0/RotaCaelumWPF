using RotaCaelum.UserControls;
using RotaCaelum.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RotaCaelum.ViewModels;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.MessageBox;
using Button = System.Windows.Controls.Button;

namespace RotaCaelum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// ofline harita indirebilmek icin -> https://egorikas.com/download-open-street-tiles-for-offline-using/
    /// </summary>
    public partial class MainWindow : Window
    {
        private Properties.Settings settings = Properties.Settings.Default;

        private ViewModelTelemetry viewModelTelemetry;

        private DataFileWriter dataFileWriter = DataFileWriter.Instance;

        private readonly BackgroundWorker comPortWorker = new BackgroundWorker();
        private List<string> ports = new List<string>();

        private SerialComminication serialComminication = SerialComminication.Instance;

        private bool isDeploymentConfigsAplied = false;

        public MainWindow()
        {
            InitializeComponent();

            viewModelTelemetry = new ViewModelTelemetry();
            DataContext = viewModelTelemetry;

            comPortWorker.DoWork += ComPortWorker_DoWork;
            comPortWorker.RunWorkerCompleted += ComPortWorker_RunWorkerCompleted;
            comPortWorker.WorkerReportsProgress = true;


            textBox_saveLocation.Text = settings.SaveDataFilePath;

        }

        private void expandChart(object sender, RoutedEventArgs e)
        {
            //WpfPlot chart = (WpfPlot)sender;

            CartesianChart chart = (CartesianChart)sender;

            switch (chart.Name) {
                case "pressureChart":
                    {
                        CallUserControl.addControlToGrid(expandedPlace, new uc_pressure());
                        break;
                    }

                case "altitudeChart":
                    {
                        CallUserControl.addControlToGrid(expandedPlace, new uc_altitude()); 
                        break;

                    }

                case "velocityChart":
                    {
                        CallUserControl.addControlToGrid(expandedPlace, new uc_velocity());
                        break;
                    }
                case "temperatureChart":
                    {
                        CallUserControl.addControlToGrid(expandedPlace, new uc_temperature()); 
                        break;
                    }
                case "gyroChart":
                    {
                        CallUserControl.addControlToGrid(expandedPlace, new uc_gyro()); 
                        break;
                    }
                case "accelChart":
                    {
                        CallUserControl.addControlToGrid(expandedPlace, new uc_acceleration()); 
                        break;
                    }

            }
            
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.Delta);
            e.Handled = true;
        }

        private void StartButtonClicked(object sender, RoutedEventArgs e)
        {
            if (button_start.Content.Equals("START"))
            {
                if(isDeploymentConfigsAplied)
                {
                    try
                    {
                        dataFileWriter.startNewSession(isDeploymentConfigsAplied);
                        dataFileWriter.writeDataInfo("start");
                        viewModelTelemetry.startReadingPort();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                    button_start.Content = "FINISH";
                    button_start.Background = Brushes.Red;

                    groupBox_saveData.IsEnabled = false;
                    groupBox_firstDeploy.IsEnabled = false;
                    groupBox_secondDeploy.IsEnabled = false;
                    groupBox_settings.IsEnabled = false;
                    border_configs.IsEnabled = false;
                }
                else
                {
                    string message = "You didn't aply all deployment configs yet. If you continue the default configs will be used.";
                    var Result = MessageBox.Show(message, "Are you sure to continue?", MessageBoxButton.YesNo);
                    if(Result == MessageBoxResult.Yes)
                    {
                        //goWithDefaultConfigs
                        try
                        {
                            dataFileWriter.startNewSession(isDeploymentConfigsAplied);
                            dataFileWriter.writeDataInfo("start");
                            viewModelTelemetry.startReadingPort();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }

                        button_start.Content = "FINISH";
                        button_start.Background = Brushes.Red;

                        groupBox_saveData.IsEnabled = false;
                        groupBox_firstDeploy.IsEnabled = false;
                        groupBox_secondDeploy.IsEnabled = false;
                        groupBox_settings.IsEnabled = false;
                        border_configs.IsEnabled = false;
                    }
                }
            }
            else if (button_start.Content.Equals("FINISH"))
            {
                try
                {
                    dataFileWriter.writeDataInfo("finish");
                    viewModelTelemetry.stopReadingPort();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                button_start.Content = "START";
                button_start.Background = Brushes.Lime;

                groupBox_saveData.IsEnabled = true;
                groupBox_firstDeploy.IsEnabled = true;
                groupBox_secondDeploy.IsEnabled = true;
                groupBox_settings.IsEnabled = true;
                border_configs.IsEnabled = true;

            }
        }
        private void ManuelDeploy_1Clicked(object sender, RoutedEventArgs e)
        {
            
        }

        private void ManuelDeploy_2Clicked(object sender, RoutedEventArgs e)
        {
             
        }


        private void saveLocation_Clicked(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select Folder";

            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                settings.SaveDataFilePath = fbd.SelectedPath;
                settings.Save();
                textBox_saveLocation.Text = fbd.SelectedPath;

                groupBox_comPortConfigs.IsEnabled = true;
                refreshComPortsConfigs();
            }
        }

        private void loadConfigsToView()
        {
            checkBox_EMERGENCY_RESCUE_MODE.IsChecked = settings.EMERGENCY_RESCUE_MODE;
            if(textBox_saveLocation.Text != "Select Location")
            {
                textBox_saveLocation.Text = settings.SaveDataFilePath;
                checkBox_dontSaveData.IsChecked = settings.DontSaveData;
            }

            if(settings.FirstDeployment_pitchAngle == -1)
            {
                checkBox_firsDeploymentPitchAngle.IsChecked = false;
            }
            else
            {
                checkBox_firsDeploymentPitchAngle.IsChecked = true;
                textBox_firsDeploymentPitchAngle.Text = settings.FirstDeployment_pitchAngle.ToString();
            }
            if (settings.FirstDeployment_velocity == -1)
            {
                checkBox_firstDeploymentVelocity.IsChecked = false;
            }
            else
            {
                checkBox_firstDeploymentVelocity.IsChecked = true;
                textBox_firstDeploymentVelocity.Text = settings.FirstDeployment_velocity.ToString();
            }
            if (settings.FirstDeployment_altitude == -1)
            {
                checkBox_firstDeploymentAltitude.IsChecked = false;
            }
            else
            {
                checkBox_firstDeploymentAltitude.IsChecked = true;
                textBox_firstDeploymentAltitude.Text = settings.FirstDeployment_altitude.ToString();
            }

            checkBox_firstDeploymentApogee.IsChecked = settings.FirstDeployment_apogee;

            if (settings.SecondDeployment_pitchAngle == -1)
            {
                checkBox_secondDeploymentPitchAngle.IsChecked = false;
            }
            else
            {
                checkBox_secondDeploymentPitchAngle.IsChecked = true;
                textBox_secondDeploymentPitchAngle.Text = settings.SecondDeployment_pitchAngle.ToString();
            }
            if (        settings.SecondDeployment_velocity == -1)
            {
                checkBox_secondDeploymentVelocity.IsChecked = false;
            }
            else
            {
                checkBox_secondDeploymentVelocity.IsChecked = true;
                textBox_secondDeploymentVelocity.Text = settings.SecondDeployment_velocity.ToString();
            }
            if (    settings.SecondDeployment_altitude == -1)
            {
                checkBox_secondDeploymentAltitude.IsChecked = false;
            }
            else
            {
                checkBox_secondDeploymentAltitude.IsChecked = true;
                textBox_secondDeploymentAltitude.Text = settings.SecondDeployment_altitude.ToString();
            }

            checkBox_dontUseSecondDeployment.IsChecked = settings.SecondDeployment_dontUse;

            refreshComPortsConfigs();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            switch (checkBox.Name)
            {
                case "checkBox_firsDeploymentPitchAngle":
                    {
                        if(checkBox.IsChecked == true)
                        {
                            textBox_firsDeploymentPitchAngle.IsEnabled = true;
                        }
                        else
                        {
                            textBox_firsDeploymentPitchAngle.IsEnabled = false;
                        }
                        break;
                    }
                case "checkBox_firstDeploymentVelocity":
                    {
                        if (checkBox.IsChecked == true)
                        {
                            textBox_firstDeploymentVelocity.IsEnabled = true;
                        }
                        else
                        {
                            textBox_firstDeploymentVelocity.IsEnabled = false;
                        }
                        break;
                    }
                case "checkBox_firstDeploymentAltitude":
                    {
                        if (checkBox.IsChecked == true)
                        {
                            textBox_firstDeploymentAltitude.IsEnabled = true;
                        }
                        else
                        {
                            textBox_firstDeploymentAltitude.IsEnabled = false;
                        }
                        break;
                    }
                case "checkBox_secondDeploymentPitchAngle":
                    {
                        if (checkBox.IsChecked == true)
                        {
                            textBox_secondDeploymentPitchAngle.IsEnabled = true;
                        }
                        else
                        {
                            textBox_secondDeploymentPitchAngle.IsEnabled = false;
                        }
                        break;
                    }
                case "checkBox_secondDeploymentVelocity":
                    {
                        if (checkBox.IsChecked == true)
                        {
                            textBox_secondDeploymentVelocity.IsEnabled = true;
                        }
                        else
                        {
                            textBox_secondDeploymentVelocity.IsEnabled = false;
                        }
                        break;
                    }
                case "checkBox_secondDeploymentAltitude":
                    {
                        if (checkBox.IsChecked == true)
                        {
                            textBox_secondDeploymentAltitude.IsEnabled = true;
                        }
                        else
                        {
                            textBox_secondDeploymentAltitude.IsEnabled = false;
                        }
                        break;
                    }
            }
        }
        
        private void button_refreshComPorts_Click(object sender, RoutedEventArgs e)
        {
            refreshComPortsConfigs();
        }

        private void refreshComPortsConfigs()
        {
            button_openComPort.IsEnabled = false;
            groupBox_firstDeploy.IsEnabled = false;
            groupBox_secondDeploy.IsEnabled = false;
            button_applyDeploymentConfig.IsEnabled = false;
            comPortWorker.RunWorkerAsync();
            progressBar_comPortSearch.Visibility = Visibility.Visible;
        }

        private void ComPortWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            comboBox_comPort.Items.Clear();
            comboBox_baudRate.Items.Clear();

            foreach (var baudrate in serialComminication.getBaudrates())
            {
                comboBox_baudRate.Items.Add(baudrate);

                if (settings.BaudRate != null && baudrate == settings.BaudRate)
                {
                    comboBox_baudRate.SelectedIndex = comboBox_baudRate.Items.IndexOf(baudrate);
                }
            }
            foreach (var port in ports)
            {
                comboBox_comPort.Items.Add(port);
                
                if (settings.COM_PORT != null && port == settings.COM_PORT)
                {
                    comboBox_comPort.SelectedIndex = comboBox_comPort.Items.IndexOf(port);
                }
                else if(settings.COM_PORT == null)
                {
                    comboBox_comPort.IsDropDownOpen = true;
                }
            }
            progressBar_comPortSearch.Visibility = Visibility.Collapsed;
            button_openComPort.IsEnabled = true;

        }

        private void ComPortWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ports = serialComminication.getComPorts();
            
        }

        private void openComportButton_clicked(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if(button.Content.Equals("Open Port"))
            {
                if (comboBox_comPort.SelectedItem.ToString() != null)
                {
                    settings.COM_PORT = comboBox_comPort.Text;
                }
                if (comboBox_baudRate.SelectedItem != null)
                {
                    settings.BaudRate = int.Parse(comboBox_baudRate.Text);
                }
                settings.Save();


                serialComminication.openComPort((Exception ex) => 
                {
                    if (ex == null)
                    {
                        button.Content = "Close Port";
                        button.Background = Brushes.Red;
                        button_start.Content = "START";
                        button_start.Background = Brushes.Lime;
                        button_start.IsEnabled = true;
                        button_applyDeploymentConfig.IsEnabled = true;
                        groupBox_firstDeploy.IsEnabled = true;
                        groupBox_secondDeploy.IsEnabled = true;
                        viewModelTelemetry.stopReadingPort();
                    }
                    else
                    {
                        MessageBox.Show(ex.ToString());
                    }
                });
            }
            else if(button.Content.Equals("Close Port"))
            {
                button.Content = "Open Port";
                button.Background = Brushes.Lime;
                serialComminication.closeComPort();
                button_start.IsEnabled = false;
                groupBox_firstDeploy.IsEnabled = false;
                groupBox_secondDeploy.IsEnabled = false;
                button_applyDeploymentConfig.IsEnabled = false;
            }            
        }
        private void applyDeploymentConfigs(object sender, RoutedEventArgs e)
        {

            groupBox_firstDeploy.IsEnabled = false;
            groupBox_secondDeploy.IsEnabled = false;
            saveDeploymentConfigs();

            viewModelTelemetry.sendDeploymentConfigs((bool result) =>
            {
                if (result)
                {
                    isDeploymentConfigsAplied = true;
                    groupBox_firstDeploy.IsEnabled = true;
                    groupBox_secondDeploy.IsEnabled = true;
                }
            });
        }

        private void saveDeploymentConfigs()
        {
            //////////////////first deployment////////////////////
            if (checkBox_firsDeploymentPitchAngle.IsChecked == true)
            {
                try
                {
                    settings.FirstDeployment_pitchAngle = short.Parse(textBox_firsDeploymentPitchAngle.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Desteklenmeyen Karakter: " + ex.Message);
                    return;
                }
            }
            else
            {
                settings.FirstDeployment_pitchAngle = -1;
            }

            if (checkBox_firstDeploymentVelocity.IsChecked == true && textBox_firstDeploymentVelocity.Text != "")
            {
                try
                {
                    settings.FirstDeployment_velocity = short.Parse(textBox_firstDeploymentVelocity.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Desteklenmeyen Karakter: " + ex.Message);
                    return;
                }
            }
            else
            {
                settings.FirstDeployment_velocity = -1;
            }

            if (checkBox_firstDeploymentAltitude.IsChecked == true)
            {
                try
                {
                    settings.FirstDeployment_altitude = short.Parse(textBox_firstDeploymentAltitude.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Desteklenmeyen Karakter: " + ex.Message);
                    return;
                }
            }
            else
            {
                settings.FirstDeployment_altitude = -1;
            }

            settings.FirstDeployment_apogee = (bool)checkBox_firstDeploymentApogee.IsChecked;

            //////////////////first deployment////////////////////

            //////////////////second deployment////////////////////

            if (checkBox_secondDeploymentPitchAngle.IsChecked == true)
            {
                try
                {
                    settings.SecondDeployment_pitchAngle = short.Parse(textBox_secondDeploymentPitchAngle.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Desteklenmeyen Karakter: " + ex.Message);
                    return;
                }
            }
            else
            {
                settings.SecondDeployment_pitchAngle = -1;
            }

            if (checkBox_secondDeploymentVelocity.IsChecked == true && textBox_secondDeploymentVelocity.Text != "")
            {
                try
                {
                    settings.SecondDeployment_velocity = short.Parse(textBox_secondDeploymentVelocity.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Desteklenmeyen Karakter: " + ex.Message);
                    return;
                }
            }
            else
            {
                settings.SecondDeployment_velocity = -1;
            }

            if (checkBox_secondDeploymentAltitude.IsChecked == true)
            {
                try
                {
                    settings.SecondDeployment_altitude = short.Parse(textBox_secondDeploymentAltitude.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Desteklenmeyen Karakter: " + ex.Message);
                    return;
                }
            }
            else
            {
                settings.SecondDeployment_altitude = -1;
            }

            settings.SecondDeployment_dontUse = (bool)checkBox_dontUseSecondDeployment.IsChecked;


            //////////////////second deployment////////////////////


            settings.Save();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            //dataFileWriter.writeDataInfo("Program Closed");
        }

        private void saveDataFilePathTextChanged(object sender, TextChangedEventArgs e)
        {


            if (textBox_saveLocation.Text != "Select location")
            {
                groupBox_comPortConfigs.IsEnabled = true;
                refreshComPortsConfigs();
            }
        }
    }


    
}
