using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoManager : MonoBehaviour
{
    [SerializeField] public string PortPath;

    public float ReadValue { get; set; }
    public GameObject Audio1;
    public GameObject Audio2;
    //SerialPort arduino = new SerialPort("COM3", 115200); windows
    // /dev/cu.usbserial-140; mac
    private SerialPort _arduino;

    void Start()
    {
        _arduino = new SerialPort(PortPath, 9600);
        _arduino.Open();
        Audio1.SetActive(false);
        Audio2.SetActive(false);
        //InvokeRepeating("ReadData", 1f, 0.01f);
    }
    
    private void ReadData()
    {
        ReadValue = float.Parse(_arduino.ReadLine()) / 100;

    }

    private void OnApplicationQuit()
    {
        _arduino.Close();
    }
    private void Update()
    {
        //Debug.Log(_arduino.ReadLine());
        string msg = _arduino.ReadLine();
        string[] tokens = msg.Split(',');
        int firstSensor = (int)float.Parse(tokens[0]);
        int secondSensor = (int)float.Parse(tokens[1]);
        //Debug.Log(firstSensor+"-"+secondSensor);
        if(firstSensor == 14 || firstSensor == 15)
        {
            Audio1.SetActive(true);
            Audio2.SetActive(false);
        }else if(secondSensor == 14 || secondSensor == 15)
        {
            Audio2.SetActive(true);
            Audio1.SetActive(false);
        }

        Debug.Log("First: " + firstSensor);
        Debug.Log("Second: " + secondSensor);
    }
}