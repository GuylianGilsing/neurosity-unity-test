/**
 * Unity Sliders by Louis
 * Based on https://github.com/dwilches/Ardity/
 * Original author: Daniel Wilches <dwilches@gmail.com>
 *
 * His work is used under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

/**
 * This class contains methods that must be run from inside a thread and others
 * that must be invoked from Unity. Both types of methods are clearly marked in
 * the code, although you, the final user of this library, don't need to even
 * open this file unless you are introducing incompatibilities for upcoming
 * versions.
 */
public class SerialThread
{
    // Parameters passed from SerialController, used for connecting to the
    // serial device as explained in the SerialController documentation.
    private string portName;
    private int baudRate;
    private int delayBeforeReconnecting;

    // Object from the .Net framework used to communicate with serial devices.
    private SerialPort serialPort;

    // Amount of milliseconds alloted to a single read or connect. An
    // exception is thrown when such operations take more than this time
    // to complete.
    private const int readTimeout = 100;

    // Amount of milliseconds alloted to a single write. An exception is thrown
    // when such operations take more than this time to complete.
    private const int writeTimeout = 100;

    // Internal synchronized queues used to send and receive messages from the
    // serial device. They serve as the point of communication between the
    // Unity thread and the SerialComm thread.
    private string lastMessage;

    // Indicates when this thread should stop executing. When SerialController
    // invokes 'RequestStop()' this variable is set.
    private bool stopRequested = false;

    //Counting the connection attempts on a single COM-port. Stops trying to connect after 3 failed attempts.
    private int retryCount = 0;


    /**************************************************************************
     * Methods intended to be invoked from the Unity thread.
     *************************************************************************/

    // ------------------------------------------------------------------------
    // Constructs the thread object. This object is not a thread actually, but
    // its method 'RunForever' can later be used to create a real Thread.
    // ------------------------------------------------------------------------
    public SerialThread(string portName,
                                int baudRate,
                                int delayBeforeReconnecting)
    {
        this.portName = portName;
        this.baudRate = baudRate;
        this.delayBeforeReconnecting = delayBeforeReconnecting;

        lastMessage = "";
    }

    //Returns a list of all COM Ports
    public static List<string> GetPortNames()
    {
        return SerialPort.GetPortNames().ToList();
    }

    // ------------------------------------------------------------------------
    // Invoked to indicate to this thread object that it should stop.
    // ------------------------------------------------------------------------
    public void RequestStop()
    {
        lock (this)
        {
            stopRequested = true;
        }
    }

    // ------------------------------------------------------------------------
    // Polls the internal message queue returning the next available message
    // in a generic form. This can be invoked by subclasses to change the
    // type of the returned object.
    // It returns null if no message has arrived since the latest invocation.
    // ------------------------------------------------------------------------
    public object ReadMessage()
    {
        return lastMessage;
    }


    /**************************************************************************
     * Methods intended to be invoked from the SerialComm thread (the one
     * created by the SerialController).
     *************************************************************************/

    // ------------------------------------------------------------------------
    // Enters an almost infinite loop of attempting connection to the serial
    // device, reading messages and sending messages. This loop can be stopped
    // by invoking 'RequestStop'.
    // ------------------------------------------------------------------------
    public void RunForever()
    {
        while (!IsStopRequested())
        {
            if (retryCount < 3)
            {
                try
                {
                    AttemptConnection();
                    // Enter the semi-infinite loop of reading/writing to the
                    // device.
                    while (!IsStopRequested())
                    {
                        RunOnce();
                    }

                }
                catch (Exception)
                {
                    // A disconnection happened, or there was a problem
                    // reading/writing to the device. Log the detailed message
                    // to the console and notify the listener.
                    //UnityEngine.Debug.LogWarning("Exception: " + ioe.Message + " StackTrace: " + ioe.StackTrace);
                    retryCount++;
                    lastMessage = UnitySliders.SERIAL_DEVICE_DISCONNECTED;

                    // As I don't know in which stage the SerialPort threw the
                    // exception I call this method that is very safe in
                    // disregard of the port's status
                    CloseDevice();
                    // Don't attempt to reconnect just yet, wait some
                    // user-defined time. It is OK to sleep here as this is not
                    // Unity's thread, this doesn't affect frame-rate
                    // throughput.
                    Thread.Sleep(delayBeforeReconnecting);
                }
            }
            //Opening device has been attempted, but it was unsuccesfull. Try different port.
            else if (SerialPort.GetPortNames().Length > 0)
            {
                string newPortName = SerialPort.GetPortNames()[SerialPort.GetPortNames().Length - 1].ToString();
                if (portName != newPortName)
                {
                    portName = newPortName;
                    retryCount = 0;
                }
            }
            else
                retryCount = 0;
        }

        // Attempt to do a final cleanup. This method doesn't fail even if
        // the port is in an invalid status.
        CloseDevice();
    }

    // ------------------------------------------------------------------------
    // Try to connect to the serial device. May throw IO exceptions.
    // ------------------------------------------------------------------------
    private void AttemptConnection()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = readTimeout;
        serialPort.WriteTimeout = writeTimeout;
        serialPort.Open();
        lastMessage = UnitySliders.SERIAL_DEVICE_CONNECTED;
    }

    // ------------------------------------------------------------------------
    // Release any resource used, and don't fail in the attempt.
    // ------------------------------------------------------------------------
    private void CloseDevice()
    {
        if (serialPort == null)
            return;

        try
        {
            serialPort.Close();
        }
        catch (IOException)
        {
            // Nothing to do, not a big deal, don't try to cleanup any further.
        }

        serialPort = null;
    }

    // ------------------------------------------------------------------------
    // Just checks if 'RequestStop()' has already been called in this object.
    // ------------------------------------------------------------------------
    private bool IsStopRequested()
    {
        lock (this)
        {
            return stopRequested;
        }
    }

    // ------------------------------------------------------------------------
    // A single iteration of the semi-infinite loop. Attempt to read/write to
    // the serial device. If there are more lines in the queue than we may have
    // at a given time, then the newly read lines will be discarded. This is a
    // protection mechanism when the port is faster than the Unity progeram.
    // If not, we may run out of memory if the queue really fills.
    // ------------------------------------------------------------------------
    private void RunOnce()
    {
        try
        {
            // Read a message, overwriting any previously stored message as only the latest message is accurate.
            object inputMessage = serialPort.ReadLine();
            if (inputMessage != null)
            {
                lastMessage = inputMessage.ToString();
            }
        }
        catch (TimeoutException)
        {
            // This is normal, not everytime we have a report from the serial device
        }
    }
}
