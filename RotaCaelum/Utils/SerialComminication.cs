using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RotaCaelum.Utils
{
    public class SerialComminication
    {


        private SerialPort _serialComPort = new SerialPort();

        public SerialPort serialComPort
        {
            get { return _serialComPort; }
        }

        private SerialComminication() {}

        private static SerialComminication instance = null;
        public static SerialComminication Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SerialComminication();
                }
                return instance;
            }
        }



        public List<string> getComPorts()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                var portList = portnames.Select(n => n + " - " + ports.FirstOrDefault(s => s.Contains(n))).ToList();

                return portList;
            }
        }



        public List<int> getBaudrates()
        {
            return new List<int> { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };
        }


        public void openComPort(Action<Exception> result)
        {

            try
            {
                serialComPort.PortName = Properties.Settings.Default.COM_PORT.Split('(')[1].TrimEnd(')');
                //serialComPort.PortName = "COM11";
                serialComPort.BaudRate = Properties.Settings.Default.BaudRate;
                //serialComPort.BaudRate = 9600;
                serialComPort.Parity = Parity.None;
                serialComPort.StopBits = StopBits.One;
                serialComPort.DataBits = 8;
                serialComPort.Open();
                result(null);

            }catch(Exception ex)
            {
                result(ex);
            }
        }

        public void closeComPort()
        {
            if (serialComPort.IsOpen)
            {   
                serialComPort.Close();
            }
        }


    }
}



//public async Task getComPorts(Action<List<string>> callback) {

//    List<string> tList;
//    var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort");
//    string[] portnames = SerialPort.GetPortNames();


//    await Task.Run(() =>
//    {
//        var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
//        tList = (from n in portnames
//                 join p in ports on n equals p["DeviceID"].ToString()
//                 select n + " - " + p["Caption"]).ToList();

//        callback(tList);
//    });
//}

//public void getComPorts(Action<List<string>> callback)
//{
//    List<string> tList;
//    var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort");
//    string[] portnames = SerialPort.GetPortNames();
//    var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
//    tList = (from n in portnames
//             join p in ports on n equals p["DeviceID"].ToString()
//             select n + " - " + p["Caption"]).ToList();

//    callback(tList);
//}