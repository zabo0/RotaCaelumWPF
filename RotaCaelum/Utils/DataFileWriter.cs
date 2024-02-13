using System;
using System.IO;

namespace RotaCaelum.Utils
{
    internal class DataFileWriter
    {
        private Properties.Settings settings = Properties.Settings.Default;

        private string fileRootPath = Properties.Settings.Default.SaveDataFilePath + "\\";

        private string fileNameInfo;
        private string fileNamePort;

        private string filePathInfo;
        private string filePathPort;

        private int session = 0;

        private DataFileWriter() { }

        private static DataFileWriter instance = null;
        public static DataFileWriter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataFileWriter();
                }
                return instance;
            }
        }

        public void createNewDataFile() {
            session++;
            fileNameInfo = "" + DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss") +"_session_" + session + "_info.txt";
            filePathInfo = fileRootPath + fileNameInfo;

            fileNamePort = "" + DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss") + "_session_" + session + "_port.txt";
            filePathPort = fileRootPath + fileNamePort;
        }

        public void startNewSession(bool isDefaultDeploymentConfigs)
        {
            createNewDataFile();


            string configs_1;
            string configs_2;
            if (isDefaultDeploymentConfigs)
            {
                configs_1 = "first deployment configs:" +
                "\n\t\t\tpithc angle: " + settings.FirstDeployment_pitchAngle.ToString() +
                "\n\t\t\tvelocity: " + settings.FirstDeployment_velocity.ToString() +
                "\n\t\t\taltitude: " + settings.FirstDeployment_altitude.ToString() +
                "\n\t\t\tapogee: " + settings.FirstDeployment_apogee.ToString();

                configs_2 = "first deployment configs:" +
                "\n\t\t\tpithc angle: " + settings.SecondDeployment_pitchAngle.ToString() +
                "\n\t\t\tvelocity: " + settings.SecondDeployment_velocity.ToString() +
                "\n\t\t\taltitude: " + settings.SecondDeployment_altitude.ToString() +
                "\n\t\t\tdont use: " + settings.SecondDeployment_dontUse.ToString();
            }
            else
            {
                configs_1 = "first deployment configs: default";
                configs_2 = "second deployment configs: default";
            }

            string line = "new session started: session " + session + "\n\t\t" +
                "Com Port name: " + settings.COM_PORT + "\n\t\t" +
                configs_1 + "\n\t\t" +
                configs_2;

            writeDataInfo(line);
        }

        public void writeData(string[] data)
        {
            string dataLine = "\t\t-<" + data[0] + "|" + data[1] + "|" + data[2] + "|" + data[3] + "|" + data[4] + "|" +
            data[5] + "|" + data[6] + "|" + data[7] + "|" + data[8] + "|" + data[9] + "|" + data[10] + "|" +
            data[11] + "|" + data[12] + "|" + data[13] + "|" + data[14] + "|" + data[15] + "|" + data[16] + "|" + data[17] + "|" + data[18] + "|" + data[19] + "|" + data[20] + ">\n";
            File.AppendAllText(filePathInfo, dataLine );
        }

        public void writeDataInfo(string line)
        {
            File.AppendAllText(filePathInfo, "<" + DateTime.Now.ToString("HH.mm.ss") + "---" + line + "--->\n\n");
        }

        public void writePortInfo(string line)
        {
            File.AppendAllText(filePathPort, "<" + DateTime.Now.ToString("HH.mm.ss") + "---" + line + "--->\n\n");
        }

        public void writeDataPort(byte[] data)
        {
            string line = "<" + BitConverter.ToString(data) + ">\n";
            
            File.AppendAllText(filePathPort, line);
        }



        
    }
}
