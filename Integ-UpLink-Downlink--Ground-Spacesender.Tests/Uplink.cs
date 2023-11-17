using Project_5;
using System.Linq.Expressions;

public class Uplink
{
    private const int QUEUESIZE = 10;
    private Queue<String> payloadQueue;
    private SpaceSender senderPassThrough;
    private SpaceSender senderSpace;
    private String passThroughEndPoint;
    private String passThroughAddress;
    private String SpaceAddress;
    private String SpaceEndPoint;
    Mutex bufferLock = new Mutex(false);
    public Uplink(String address, String passThroughEndPoint, String SpaceEndPoint)
    {

    }

    private bool ReadytoTransmit(ref SpaceSender sender)
    {
        return Uplink_Stubs.ReadyToTransmit_Stub();
    }

    public bool AddToQueue(String payload)
    {
        return Uplink_Stubs.AddToQueue_Stub(payload);
    }

    public void Clear()
    {

    }
}