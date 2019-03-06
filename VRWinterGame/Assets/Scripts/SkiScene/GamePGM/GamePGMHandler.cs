using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class GamePGMHandler : MonoBehaviour
{
    // Create a serial object
    private SerialPort myDataPGM;

    // Actual SerialPort
    public SerialPortConnection portNamePGM = SerialPortConnection.COM5;
    private string _portName;

    // Baud rate
    public BaudRateValue baudRatePGM = BaudRateValue._115200;
    private int _baudRate;

    void Start()
    {
        OpenPort();
    }

    string ChoosePort(SerialPortConnection port)
    {

        if (port == SerialPortConnection.COM3)
        {
            _portName = "COM3";
        }
        else if (port == SerialPortConnection.COM4)
        {
            _portName = "COM4";
        }
        else if (port == SerialPortConnection.COM5)
        {
            _portName = "COM5";
        }
        else if (port == SerialPortConnection.COM6)
        {
            _portName = "COM6";
        }
        else if (port == SerialPortConnection.COM7)
        {
            _portName = "COM7";
        }
        else if (port == SerialPortConnection.COM8)
        {
            _portName = "COM8";
        }
        else if (port == SerialPortConnection.COM9)
        {
            _portName = "COM9";
        }
        else if (port == SerialPortConnection.COM10)
        {
            _portName = "\\\\.\\COM10";
        }

        return _portName;

    }

    int ChooseBaudRate(BaudRateValue baudRate)
    {

        if (baudRate == BaudRateValue._9600)
        {
            _baudRate = 9600;
        }
        else if (baudRate == BaudRateValue._38400)
        {
            _baudRate = 38400;
        }
        else if (baudRate == BaudRateValue._115200)
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
        myDataPGM.ReadTimeout = 500; // Probando 
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
