#define TEST
using System.Collections;
using System.Linq.Expressions;
using System.Text;

public class SpaceSender
{
    private Queue<string> transmissionQueue = new Queue<string>();
    private HttpClient client = new HttpClient();
    private Thread transmissionManager;
    private object bufferLock = new object();
    public bool TransmissionStatus { get; private set; }
    private string targetURL;

    public SpaceSender(string targetURL)
    {
        this.targetURL = targetURL;
        transmissionQueue = new Queue<string>();
        client = new HttpClient();
        transmissionManager = new Thread(StartSendThread);
        bufferLock = new object();
    }

    public bool IsBufferEmpty()
    {
        lock (bufferLock)
        {
            return transmissionQueue.Count == 0;
        }
    }

    public bool IsRunning()
    {
        return transmissionManager.IsAlive;
    }

    public void SendTransmission(string jsonData)
    {
        lock (bufferLock)
        {
            transmissionQueue.Enqueue(jsonData);
        }

        if (!transmissionManager.IsAlive)
        {
            try
            {
                transmissionManager = new Thread(StartSendThread);
                transmissionManager.Start();
            }
            catch (ThreadStateException)
            {
                TransmissionStatus = false;
            }
            catch (OutOfMemoryException)
            {
                TransmissionStatus = false;
            }
        }
    }

    private async void StartSendThread()
    {
        TransmissionStatus = true;

        while (true)
        {
            string nextToSend = null;

            lock (bufferLock)
            {
                if (transmissionQueue.Count > 0)
                {
                    nextToSend = transmissionQueue.Dequeue();
                }
            }

            if (nextToSend != null)
            {
                HttpContent content = new StringContent(nextToSend, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(targetURL, content);

                if (response.IsSuccessStatusCode)
                {
                    TransmissionStatus = true;
                }
                else
                {
                    TransmissionStatus = false;
                }
            }
            else
            {
                Thread.Sleep(100);
            }
        }
    }
}