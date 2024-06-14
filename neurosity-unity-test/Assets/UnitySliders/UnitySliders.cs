/**
 * Unity Sliders by Louis
 * Based on https://github.com/dwilches/Ardity/
 * Original author: Daniel Wilches <dwilches@gmail.com>
 *
 * His work is used under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using UnityEditor;

public class UnitySliders : MonoBehaviour
{
    //[SerializeField] public Material planeMaterial;
    //[SerializeField] public Material cubeMaterial;
    //[SerializeField] public Material orbMaterial;
    //[SerializeField] public Material cylinderMaterial;
    
    static private UnitySliders activeSliderScript;
    //Port name with which the SerialPort object will be created. Leave empty for auto detection.
    string portName = "";

    //Baud rate that the serial device is using to transmit data.
    int baudRate = 19200;
    
    //After an error in the serial communication, or an unsuccessful connect, how many milliseconds we should wait.
    private int reconnectionDelay = 2000;

    [SerializeField]
    private bool connected = false;
    
    [Header("Sliders:")]
    [SerializeField][Range(0.0f, 255f)] byte Red;
    [SerializeField][Range(0.0f, 255f)] byte Orange;
    [SerializeField][Range(0.0f, 255f)] byte Yellow;
    [SerializeField][Range(0.0f, 255f)] byte Green;
    [SerializeField][Range(0.0f, 255f)] byte Blue;

    // Constants used to mark the start and end of a connection. There is no
    // way you can generate clashing messages from your serial device, as I
    // compare the references of these strings, no their contents. So if you
    // send these same strings from the serial device, upon reconstruction they
    // will have different reference ids.
    public const string SERIAL_DEVICE_CONNECTED = "__Connected__";
    public const string SERIAL_DEVICE_DISCONNECTED = "__Disconnected__";

    // Internal reference to the Thread and the object that runs in it.
    protected Thread thread;
    protected SerialThread serialThread;

    
    // ------------------------------------------------------------------------
    // Invoked whenever the SerialController gameobject is activated.
    // It creates a new thread that tries to connect to the serial device
    // and start reading from it.
    // ------------------------------------------------------------------------
    void OnEnable()
    {
        if(GameObject.FindObjectsOfType<UnitySliders>().Length > 1)
        {
            if (activeSliderScript == null)
            {
                activeSliderScript = this;
            }
            else
            {
                Debug.LogWarning("Multiple Unity Slider Scripts found in Scene. Removed automatically, but you should fix this manually.");
                Destroy(this);
                return;
            }
        }
        if(portName == "")
        {
            List<string> Ports = SerialThread.GetPortNames();
            if(Ports.Count > 0)
            {
                portName = Ports[Ports.Count - 1];
            }
        }
        serialThread = new SerialThread(portName, 
                                             baudRate, 
                                             reconnectionDelay);
        thread = new Thread(new ThreadStart(serialThread.RunForever));
        thread.Start();
        connected = false;
    }

    // ------------------------------------------------------------------------
    // Invoked whenever the SerialController gameobject is deactivated.
    // It stops and destroys the thread that was reading from the serial device.
    // ------------------------------------------------------------------------
    void OnDisable()
    {
        // The serialThread reference should never be null at this point,
        // unless an Exception happened in the OnEnable(), in which case I've
        // no idea what face Unity will make.
        if (serialThread != null)
        {
            serialThread.RequestStop();
            serialThread = null;
        }

        // This reference shouldn't be null at this point anyway.
        if (thread != null)
        {
            thread.Join();
            thread = null;
        }
    }

    // ------------------------------------------------------------------------
    // Polls messages from the queue that the SerialThread object keeps. Once a
    // message has been polled it is removed from the queue. There are some
    // special messages that mark the start/end of the communication with the
    // device.
    // ------------------------------------------------------------------------
    void Update()
    {
        
        // Read the next message from the queue
        string message = (string)serialThread.ReadMessage();
        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SERIAL_DEVICE_CONNECTED))
            connected = true;
        else if (ReferenceEquals(message, SERIAL_DEVICE_DISCONNECTED))
            connected = false;
        else
        {
            if (message.Length <= 0) return;
            if (message[0] != '#')
            {
                //Wrong device is connected. Close the connection.
                serialThread.RequestStop();
                return;
            }
            message = message.Substring(1);
            string[] values = message.Split(':');
            if (values.Length == 5)
            {
                byte.TryParse(values[0], System.Globalization.NumberStyles.HexNumber, null, out Red);
                byte.TryParse(values[1], System.Globalization.NumberStyles.HexNumber, null, out Orange);
                byte.TryParse(values[2], System.Globalization.NumberStyles.HexNumber, null, out Yellow);
                byte.TryParse(values[3], System.Globalization.NumberStyles.HexNumber, null, out Green);
                byte.TryParse(values[4], System.Globalization.NumberStyles.HexNumber, null, out Blue);
            }
        }


        planeMaterial.color = new Color(GetSliderValue01(SliderColors.Green), GetSliderValue01(SliderColors.Green), GetSliderValue01(SliderColors.Green));
        cubeMaterial.color = new Color(GetSliderValue01(SliderColors.Red), GetSliderValue01(SliderColors.Blue), 0);
        orbMaterial.color = new Color(0, GetSliderValue01(SliderColors.Orange), GetSliderValue01(SliderColors.Blue));
        cylinderMaterial.color = new Color(GetSliderValue01(SliderColors.Blue), 0, GetSliderValue01(SliderColors.Yellow));
    }

    public float GetSliderValue01(SliderColors color)
    {
        switch (color)
        {
            case SliderColors.Red:
                return Red / 255f;
            case SliderColors.Orange:
                return Orange / 255f;
            case SliderColors.Yellow:
                return Yellow / 255f;
            case SliderColors.Green:
                return Green / 255f;
            case SliderColors.Blue:
                return Blue / 255f;
            default:
                return 0;
        }
    }
    public float GetSliderValueRaw(SliderColors color)
    {
        switch (color)
        {
            case SliderColors.Red:
                return Red;
            case SliderColors.Orange:
                return Orange;
            case SliderColors.Yellow:
                return Yellow;
            case SliderColors.Green:
                return Green;
            case SliderColors.Blue:
                return Blue;
            default:
                return 0;
        }
    }

    public bool IsConnected()
    {
        return connected;
    }


    public enum SliderColors
    {
        Red,
        Orange,
        Yellow,
        Green,
        Blue
    }

}
