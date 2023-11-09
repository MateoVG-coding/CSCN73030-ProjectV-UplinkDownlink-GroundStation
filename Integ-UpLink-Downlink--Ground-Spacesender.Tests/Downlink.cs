
using Project_5;
using System.Linq.Expressions;

class DownLink
{
    private const int QUEUESIZE = 10;
    private Queue<String> payloadQueue;
    private GroundSender senderPassThrough;
    private GroundSender senderGroundStation;
    private String passThroughEndPoint;
    private String passThroughAddress;
    private String groundStationAddress;
    private String groundStationEndPoint;
    Mutex bufferLock = new Mutex(false);
    DownLink(String address, String passThroughEndPoint, String groundStationEndPoint)
    {
            }

    private bool ReadytoTransmit(ref GroundSender sender)
    {
        
    }

    public bool AddToQueue(String payload)
    {
        
    }

    public void Clear()
    {
        
    }
}