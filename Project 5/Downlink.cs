﻿
using Project_5;
using System.Linq.Expressions;

public class DownLink
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
    

    public DownLink(String address, String passThroughEndPoint, String groundStationEndPoint)
    {
        payloadQueue = new Queue<String>(QUEUESIZE);
        this.passThroughAddress = address;
        this.passThroughEndPoint = passThroughEndPoint;
        this.groundStationAddress = address;
        this.groundStationEndPoint = groundStationEndPoint;
        senderGroundStation = new GroundSender(groundStationAddress + groundStationEndPoint, ref payloadQueue, ref bufferLock);
        senderPassThrough = new GroundSender(passThroughAddress + passThroughEndPoint, ref payloadQueue, ref bufferLock);
    }

    public bool ReadytoTransmit(params GroundSender[] senders)
    {
        bool status = true;

        foreach (GroundSender sender in senders)
        {
            if (!sender.transmissionStatus)
                status = false;
        }

        return status;
    }

    public bool AddToQueue(String payload)
    {
        if (payloadQueue.Count >= QUEUESIZE)
            return false;
        bufferLock.WaitOne();
        payloadQueue.Enqueue(payload);
        bufferLock.ReleaseMutex();

        if (!senderGroundStation.isRunning())
            senderGroundStation.SendTransmission();
        else if (!senderPassThrough.isRunning())
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