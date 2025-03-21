using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class PanicBtn : MonoBehaviour
{
    [Header("Network Settings")]
    public string ipAddress = "0.0.0.0"; // Listen on all available interfaces
    public int port = 8889;
    public bool debugMode = true;

    [Header("Movement Settings")]
    public float sensitivity = 5.0f;
    public bool useSmoothing = true;
    public float smoothingFactor = 0.1f;

    private UdpClient udpClient;
    private Thread receiveThread;
    private bool threadRunning = false;

    private float x, y, z;
    private Vector3 targetPosition;
    private Vector3 initialPosition;
    private object lockObject = new object();

    // State variable to control data processing
    private bool isProcessingData = false;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;

        // Start UDP listener in a separate thread
        StartUDPListener();
    }

    void StartUDPListener()
    {
        try
        {
            // Create UDP client
            udpClient = new UdpClient(port);

            // Start receive thread
            threadRunning = true;
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log($"UDP listener started on port {port}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to start UDP listener: {e.Message}");
        }
    }

    void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (threadRunning)
        {
            try
            {
                // Receive bytes
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.ASCII.GetString(data);

                // Process the data
                if (debugMode)
                {
                    Debug.Log($"Received: {message} from {remoteEndPoint}");
                }

                ParseData(message);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"UDP receive error: {e.Message}");
            }
        }
    }

    void ParseData(string data)
    {
        try
        {
            string[] parts = data.Split(',');

            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part))
                    continue;

                int colonIndex = part.IndexOf(':');
                if (colonIndex <= 0 || colonIndex >= part.Length - 1)
                    continue;

                string key = part.Substring(0, colonIndex).Trim();
                string value = part.Substring(colonIndex + 1).Trim();

                float parsedValue;
                if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedValue))
                {
                    lock (lockObject)
                    {
                        if (key == "X")
                            x = parsedValue;
                        else if (key == "Y")
                            y = parsedValue;
                        else if (key == "Z")
                            z = parsedValue;
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing data: {e.Message}");
        }
    }

    void Update()
    {
        // Check for button presses to start/stop data processing
        if (OVRInput.GetDown(OVRInput.Button.One)) // "A" button
        {
            isProcessingData = true;
            Debug.Log("Data processing started");
        }
        if (OVRInput.GetDown(OVRInput.Button.Two)) // "B" button
        {
            isProcessingData = false;
            Debug.Log("Data processing stopped");
        }

        // Update position only if data processing is active
        if (isProcessingData)
        {
            // Calculate target position based on accelerometer data
            lock (lockObject)
            {
                // Note: adjusting axes to match Unity's coordinate system
                targetPosition = initialPosition + new Vector3(x, z, y) * sensitivity;
            }

            // Apply smoothing if enabled
            if (useSmoothing)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, smoothingFactor);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }

    void OnApplicationQuit()
    {
        CloseConnection();
    }

    void OnDestroy()
    {
        CloseConnection();
    }

    void CloseConnection()
    {
        threadRunning = false;

        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
            receiveThread = null;
        }

        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }

        Debug.Log("UDP connection closed");
    }
}