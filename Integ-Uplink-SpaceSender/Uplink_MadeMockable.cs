using Project_5;
using System.Linq.Expressions;

class Uplink_MadeMockable
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
    public Uplink_MadeMockable(String address, String passThroughEndPoint, String SpaceEndPoint, ref SpaceSender passthrough, ref SpaceSender space)
    {
        payloadQueue = new Queue<String>(QUEUESIZE);
        this.passThroughAddress = address;
        this.passThroughEndPoint = passThroughEndPoint;
        this.SpaceAddress = address;
        this.SpaceEndPoint = SpaceEndPoint;
        senderSpace = space;
        senderPassThrough = passthrough;
    }

    private bool ReadytoTransmit(ref GroundSender sender)
    {
        return Uplink_Stubs.ReadyToTransmit_Stub();
    }

    public bool AddToQueue(String payload)
    {
        if (payloadQueue.Count >= QUEUESIZE)
            return false;
        bufferLock.WaitOne();
        payloadQueue.Enqueue(payload);
        bufferLock.ReleaseMutex();

        if (!senderSpace.IsRunning())
            senderSpace.SendTransmission();
        else if (!senderPassThrough.IsRunning())
            senderPassThrough.SendTransmission();
        else
            return false;

        return true;
    }

    public void Clear()
    {

    }
}
