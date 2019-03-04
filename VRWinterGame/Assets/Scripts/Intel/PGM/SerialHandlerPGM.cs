using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public enum SerialPortConnectionPGM
{
    COM3,
    COM4,
    COM5,
    COM6,
    COM7,
    COM8,
    COM9,
    COM10
}

public enum BaudRateValuePGM
{
    _9600,
    _38400,
    _115200
}

public class SerialHandlerPGM : MonoBehaviour
{

    // Create a serial object
    private SerialPort myDataPGM;

    // Actual SerialPort
    public SerialPortConnectionPGM portNamePGM = SerialPortConnectionPGM.COM5;
    private string _portName;

    // Baud rate
    public BaudRateValuePGM baudRatePGM = BaudRateValuePGM._115200;
    private int _baudRate;

    void Start()
    {
        OpenPort();
    }

    string ChoosePort(SerialPortConnectionPGM port)
    {

        if (port == SerialPortConnectionPGM.COM3)
        {
            _portName = "COM3";
        }
        else if (port == SerialPortConnectionPGM.COM4)
        {
            _portName = "COM4";
        }
        else if (port == SerialPortConnectionPGM.COM5)
        {
            _portName = "COM5";
        }
        else if (port == SerialPortConnectionPGM.COM6)
        {
            _portName = "COM6";
        }
        else if (port == SerialPortConnectionPGM.COM7)
        {
            _portName = "COM7";
        }
        else if (port == SerialPortConnectionPGM.COM8)
        {
            _portName = "COM8";
        }
        else if (port == SerialPortConnectionPGM.COM9)
        {
            _portName = "COM9";
        }
        else if (port == SerialPortConnectionPGM.COM10)
        {
            _portName = "\\\\.\\COM10";
        }

        return _portName;

    }

    int ChooseBaudRate(BaudRateValuePGM baudRate)
    {

        if (baudRate == BaudRateValuePGM._9600)
        {
            _baudRate = 9600;
        }
        else if (baudRate == BaudRateValuePGM._38400)
        {
            _baudRate = 38400;
        }
        else if (baudRate == BaudRateValuePGM._115200)
        {
            _baudRate = 115200;
        }

        return _baudRate;

    }

    // Use this for initialization
    void OpenPort()
    {
        // This will just print the devices connected in the port 
        //foreach (string str in SerialPort.GetPortNames())
        //{
        //    Debug.Log(string.Format("port : {0}", str));
        //}

        myDataPGM = new SerialPort(ChoosePort(portNamePGM), ChooseBaudRate(baudRatePGM), Parity.None, 8, StopBits.One);

        myDataPGM.Open();
        myDataPGM.ReadTimeout = 500; 
    }

    void OnDestroy()
    {
        for (int i = 0; i < 5; i++) // To reset the microcontroller once the program is closed 
        {
            Debug.Log("5");
            myDataPGM.Write("5");
        }
        myDataPGM.Close();
        myDataPGM.Dispose();
    }

    private void Close()
    {

        if (myDataPGM != null && myDataPGM.IsOpen)
        {
            myDataPGM.Close();
            myDataPGM.Dispose();
        }
    }

    public void Write(string message)
    {
        try
        {
            myDataPGM.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

}
