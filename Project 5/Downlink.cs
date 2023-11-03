

using Project_5;

class DownLink
{
    private GroundSender sender;
    private String passThroughEndpoint;
    private String passThroughAddress;
    private String groundStationAddress;
    private String groundStationEndPoint;

    DownLink(String passThroughAddress,  String passThroughEndPoint, String groundStationAddress, String groundStationEndPoint)
    {

    }

    private String PeekAtAddress(String payload)
    {
        //returns "Test_Address"
        return Downlink_Stubs.PeekAtAddress_Stub();
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