
#define TEST 
using System.Collections;
using System.Linq.Expressions;
using System.Text;

public class GroundSender
{
    private Queue<String> transmissionQueue;
    HttpClient client;
    private Thread transmissionManager;
    Mutex bufferLock;
    public bool isRunning { get; set; }
    private bool transmissionStatus { get; set; }

    public GroundSender()
    {
        bufferLock = new Mutex();
        isRunning = false;
        transmissionStatus = false;
        client = new HttpClient();
        transmissionQueue = new Queue<String>();
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

        isRunning = true;
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
            response = await client.PostAsync("a url", content);

            //Http request sends json string that was dequeued
            if (response.IsSuccessStatusCode)
                transmissionStatus = true;
            else
                transmissionStatus = false;
        }
        isRunning = false;
    }

    public bool IsBufferEmpty()
    {
        return transmissionQueue.Count > 0;
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
#if DEBUG
                Console.WriteLine("jumping start of thread");
#else
                transmissionManager.Start();

#endif
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