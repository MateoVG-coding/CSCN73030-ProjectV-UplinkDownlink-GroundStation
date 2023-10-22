

using Project_5;

class UpLink
{
    private SpaceSender sender;
    private string enpoint;
    private string ipAddress;
    bool linkStatus;

    UpLink(String ip, String enpoint)
    {

    }

    public void InitSender()
    {

    }

    public bool AddToQueue(String payload)
    {
        return Downlink_Stubs.AddToQueue_Stub(payload);

    }

    public bool GetTranmissionStatus()
    {
       return Stub_SpaceSender.GetTranmissionStatus_Stub();
    }

    public void Clear()
    {

    }



}