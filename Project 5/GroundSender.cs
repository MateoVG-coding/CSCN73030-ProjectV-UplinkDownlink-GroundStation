
#define TEST 
using Project_5_S;
using System.Collections;
using System.Linq.Expressions;
using System.Text;

public class GroundSender
{
    private Queue<String> transmissionQueue;
    HttpClient client;
    private Thread transmissionManager;
    Mutex bufferLock;
    public bool transmissionStatus { get; set; }
    String targetURL;
    

    public GroundSender(String target)
    {
        bufferLock = new Mutex();
        transmissionStatus = false;
        client = new HttpClient();
        transmissionQueue = new Queue<String>();
        targetURL = target;
        transmissionManager = new Thread(delegate ()
        {
            StartSendThread();
        });
        transmissionManager.IsBackground = true;
    }

    private async void StartSendThread()
    {
        String? nextToSend = null;
        client = new HttpClient();
        HttpContent? content = null;
        HttpResponseMessage? response = null;

        transmissionStatus = true;

        while (transmissionQueue.Count > 0)
        {
            if (transmissionStatus)
            {
                bufferLock.WaitOne();
                nextToSend = transmissionQueue.Dequeue();
                bufferLock.ReleaseMutex();
            }
                
            if(nextToSend != null)
                content = new StringContent(nextToSend, Encoding.UTF8, "application/json");
#if DEBUG
            response = GroundSender_Stubs.HttpRequest_Stub();
#else
            response = await client.PostAsync(targetURL, content);
#endif


            //Http request sends json string that was dequeued
            if (response.IsSuccessStatusCode)
                transmissionStatus = true;
            else
                transmissionStatus = false;
        }
    }

    public bool IsBufferEmpty()
    {
        return transmissionQueue.Count == 0;
    }

    public bool isRunning()
    {
        return transmissionManager.IsAlive;
    }

    public bool SendTransmission(ref String jsonData) 
    {
        try
        {
            bufferLock.WaitOne();
            transmissionQueue.Enqueue(jsonData);
            bufferLock.ReleaseMutex();
        }catch(ApplicationException)
        {
            return false;
        }catch(ObjectDisposedException)
        {
            return false;
        }
        
        if(!transmissionManager.IsAlive)
        {
            transmissionManager = new Thread(delegate ()
            {
                StartSendThread();
            });
            transmissionManager.IsBackground = true;
            try
            {
                transmissionManager.Start();

            }catch(ThreadStateException)
            {
                return false;
            }
            catch(OutOfMemoryException)
            {
                return false;
            }
            
        }
        return true;
    }
}