
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
        payloadQueue = new Queue<String>(QUEUESIZE);
        this.passThroughAddress = address;
        this.passThroughEndPoint = passThroughEndPoint;
        this.SpaceAddress = address;
        this.SpaceEndPoint = SpaceEndPoint;
        senderSpace = new SpaceSender(SpaceAddress + SpaceEndPoint, ref payloadQueue, ref bufferLock);
        senderPassThrough = new SpaceSender(passThroughAddress + passThroughEndPoint, ref payloadQueue, ref bufferLock);
    }

    private bool ReadytoTransmit(ref SpaceSender sender)
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

    public bool Clear()
    {
        bufferLock.WaitOne();
        payloadQueue.Clear();
        bufferLock.ReleaseMutex();

        return payloadQueue.Count == 0;

    }
}