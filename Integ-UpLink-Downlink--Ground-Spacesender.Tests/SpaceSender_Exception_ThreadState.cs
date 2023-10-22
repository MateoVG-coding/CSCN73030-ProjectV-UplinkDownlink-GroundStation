#define TEST
using Project_5;
using System.Collections;
using System.Linq.Expressions;
using System.Text;

public class SpaceSender_Exceptions
{
    private Queue<string> transmissionQueue = new Queue<string>();
    private HttpClient client = new HttpClient();
    private Thread? transmissionManager;
    Mutex bufferLock;
    public bool TransmissionStatus { get; private set; }
    private string targetURL;
    private Thread? transmissionManager_Ping;

    public SpaceSender_Exceptions(string targetURL)
    {
        this.targetURL = targetURL;
        transmissionQueue = new Queue<string>();
        client = new HttpClient();
        transmissionManager = new Thread(StartSendThread);
        transmissionManager_Ping = new Thread(StartPingThread);
        bufferLock = new Mutex();
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
        bool status = false;
        if (transmissionManager != null)
            status = transmissionManager.IsAlive;
        else
            status = false;
        return status;
    }

    public void SendTransmission(string jsonData)
    {
        lock (bufferLock)
        {
            transmissionQueue.Enqueue(jsonData);
        }

        if (!transmissionManager.IsAlive)
        {
            if (TransmissionStatus)
                transmissionManager.Join();
            try
            {
                transmissionManager = new Thread(delegate ()
                {
                    StartSendThread();
                });
                transmissionManager.Start();
            }
            catch (ThreadStateException)
            {
                return;
            }
            catch (OutOfMemoryException)
            {
                return;
            }
        }
    }

    private async void StartSendThread()
    {
        TransmissionStatus = true;
        String? nextToSend = null;
        client = new HttpClient();
        HttpContent? content = null;
        HttpResponseMessage? response = null;

        while (transmissionQueue.Count > 0)
        {
#if !DEBUG
            if (TransmissionStatus)
            {

                bufferLock.WaitOne();
                nextToSend = transmissionQueue.Dequeue();
                bufferLock.ReleaseMutex();
            }
#endif       
            if (nextToSend != null)
            {
                content = new StringContent(nextToSend, Encoding.UTF8, "application/json");

                try
                {
#if DEBUG
                    response = Stub_SpaceSender.HttpRequest_Stub();
#else
                    response = await client.PostAsync(targetURL, content);
#endif
                    //Http request sends json string that was dequeued
                    if (response.IsSuccessStatusCode)
                        TransmissionStatus = true;
                    else
                        TransmissionStatus = false;
                }
                catch (HttpRequestException ex)
                { return; }
            }
        }
    }

    public bool IsRunning_Ping()
    {
        bool status = false;
        if (transmissionManager_Ping != null)
            status = transmissionManager_Ping.IsAlive;
        else
            status = false;
        return status;
    }

    private async void StartPingThread()
    {
        while (true)
        {
            try
            {
#if DEBUG
                HttpResponseMessage response = Stub_SpaceSender.HttpRequest_Stub();
#else
                HttpResponseMessage response = await client.GetAsync(targetURL);
#endif


                if (response.IsSuccessStatusCode)
                {
                    TransmissionStatus = true;
                    Thread.Sleep(1000);
                }
                else
                {
                    TransmissionStatus = false;
                    Thread.Sleep(1000);
                }
            }
            catch (HttpRequestException e)
            {
                return;
            }
        }
    }

    public void SendPing()
    {
        if (!transmissionManager_Ping.IsAlive)
        {
            if (TransmissionStatus)
                transmissionManager_Ping.Join();
            try
            {
                transmissionManager_Ping = new Thread(delegate ()
                {
                    StartSendThread();
                });
                transmissionManager_Ping.Start();
            }
            catch (ThreadStateException)
            {
                return;
            }
            catch (OutOfMemoryException)
            {
                return;
            }
        }
    }
}