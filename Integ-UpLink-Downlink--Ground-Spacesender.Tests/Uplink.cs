
using Project_5;
using System.Linq.Expressions;

class Uplink
{
    private const int QUEUESIZE = 10;
    private Queue<String> payloadQueue;
    private GroundSender senderPassThrough;
    private GroundSender senderSpace;
    private String passThroughEndPoint;
    private String passThroughAddress;
    private String spaceAddress;
    private String spaceEndPoint;
    Mutex bufferLock = new Mutex(false);
    Uplink(String address, String passThroughEndPoint, String groundStationEndPoint)
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