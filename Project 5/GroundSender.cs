using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project_5;
using System.Text;


public class GroundSender
{
    private Queue<String> transmissionQueue;
    HttpClient client;
    private Thread? transmissionManager;
    Mutex bufferLock;
    public bool transmissionStatus { get; set; }
    Uri targetURI;

    public GroundSender(String target, ref Queue<String> payloads, ref Mutex queuelock)
    {
        bufferLock = queuelock;
        transmissionStatus = false;
        transmissionQueue = payloads;
        targetURI = new Uri(target);
        client = new HttpClient();
        client.BaseAddress = new Uri(targetURI.GetLeftPart(UriPartial.Authority));
    }

    private String PeekAtAddress()
    {
        return Downlink_Stubs.PeekAtAddress_Stub();
    }

    private async void StartSendThread()
    {
        String? nextToSend = null;
        HttpContent? content = null;
        HttpResponseMessage? response = null;
        String addressOfDestination = String.Empty;

        transmissionStatus = true;

        while (transmissionQueue.Count > 0)
        {

            if (transmissionStatus)
            {

                addressOfDestination = PeekAtAddress();

                if (addressOfDestination.Equals(targetURI.GetLeftPart(UriPartial.Authority).ToString()))
                {
                    bufferLock.WaitOne();

                    try
                    {
                        nextToSend = transmissionQueue.Dequeue();
                    }
                    catch (InvalidOperationException ex)
                    {
                        nextToSend = null;
                    }

                    bufferLock.ReleaseMutex();
                }
            }

            if (nextToSend != null)
            {
                content = new StringContent(nextToSend, Encoding.UTF8, "application/json");

                try
                {
                    response = await client.PostAsync(targetURI.PathAndQuery, content);

                    //Http request sends json string that was dequeued
                    if (response.IsSuccessStatusCode)
                        transmissionStatus = true;
                    else
                        transmissionStatus = false;
                }
                catch (HttpRequestException ex)
                { return; }
            }
        }
    }

    public bool IsBufferEmpty()
    {
        return transmissionQueue.Count == 0;
    }

    public bool isRunning()
    {
        bool status = false;
        if (transmissionManager != null)
            status = transmissionManager.IsAlive;
        else
            status = false;
        return status;
    }

    public bool SendTransmission()
    {
        if (transmissionManager == null)
        {
            transmissionManager = new Thread(delegate ()
            {
                StartSendThread();
            });
        }

        if (!transmissionManager.IsAlive)
        {
            if (transmissionStatus)
            {
                try
                {
                    transmissionManager.Join();
                }
                catch (ThreadStateException) { }
                catch (ThreadInterruptedException) { }
            }

            try
            {
                transmissionManager = new Thread(delegate ()
                {
                    StartSendThread();
                });
                transmissionManager.Start();
                transmissionStatus = true;
            }
            catch (ThreadStateException)
            {

                return false;
            }
            catch (OutOfMemoryException)
            {
                return false;
            }
        }
        return true;
    }
}