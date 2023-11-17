

using Project_5;

namespace Integ_Downlink_GroundSender.Tests
{
    [TestClass]
    public class Downlink_Integration
    {
        [TestMethod]
        public void Downlink_Provides_Data_To_Send_GroundSender_Begins_Transmitting()
        {
            //Arrange
            String fakeAddress = "192.168.1.1";
            String fakeEndpointGround = "https://httpbin.org/post";
            String fakeEndPointPassthrough = "https://httpbin.org/post";

            Queue<String> testQueue = GroundSender_Stubs.GetFakedTransmissionQuueue();
            Mutex bufferlock = new Mutex();
            GroundSender ground = new GroundSender(fakeEndpointGround, ref testQueue, ref bufferlock);
            GroundSender passthrough = new GroundSender(fakeEndPointPassthrough, ref testQueue, ref bufferlock);
            DownLink_MadeMockable link = new DownLink_MadeMockable(fakeAddress, fakeEndpointGround, fakeEndPointPassthrough, ref ground, ref passthrough);

            //Act
            link.AddToQueue("{'path': 'https://httpbin.org/post'}");
            Thread.Sleep(5000);

            //Assert
            Assert.IsTrue(ground.IsBufferEmpty());
        }
    }
}