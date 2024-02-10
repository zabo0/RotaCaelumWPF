using RotaCaelum.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ScottPlot.Drawing.Colormaps;

namespace RotaCaelum.Utils
{
    internal class DataFileWriter
    {
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
            fileNameInfo = "" + DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss") +"_session_" + session + "_data.txt";
            filePathInfo = fileRootPath + fileNameInfo;

            fileNamePort = "" + DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss") + "_session_" + session + "_port.txt";
            filePathPort = fileRootPath + fileNamePort;

            writeDataInfo("file created");
            writePortInfo("file created");
        }

        public void writeData(string[] data)
        {
            string dataLine = "\t\t-<" + data[0] + "|" + data[1] + "|" + data[2] + "|" + data[3] + "|" + data[4] + "|" +
            data[5] + "|" + data[6] + "|" + data[7] + "|" + data[8] + "|" + data[9] + "|" + data[10] + "|" +
            data[11] + "|" + data[12] + "|" + data[13] + "|" + data[14] + "|" + data[15] + "|" + data[16] + "|" + data[17] + "|" + data[18] + "|" + data[19] + "|" + data[20] + ">\n";
            File.AppendAllText(filePathInfo, dataLine );
        }

        //public void writeDataInfo(string line)
        //{
        //    sw_data.WriteLine("<" + DateTime.Now.ToString("HH.mm.ss") + "---" + line + "--->");
        //    closeDataFile();
        //}

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
            string line = "-<" + BitConverter.ToString(data) + ">\n";
            //string line = "-<" ;

            //foreach (var item in data)
            //{
            //    line += item.ToString()+"-";
            //}
            
            File.AppendAllText(filePathPort, line);
        }
    }
}
