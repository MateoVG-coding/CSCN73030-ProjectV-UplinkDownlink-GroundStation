

using Project_5_S;

class DownLink
{
    private GroundSender sender;
    private string enpoint;
    private string ipAddress;

    DownLink(String ip,  String enpoint)
    {

    }

    public void InitSender()
    {

    }

    public bool AddToQueue(String payload)
    {
        return Downlink_Stubs.AddToQueue_Stub(payload);

    }

    public void Clear()
    {

    }



}