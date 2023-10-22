

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

    public bool GetLinkStatus()
    {
       return Stub_SpaceSender.GetLinkStatus_Stub();
    }

    public void Clear()
    {

    }



}