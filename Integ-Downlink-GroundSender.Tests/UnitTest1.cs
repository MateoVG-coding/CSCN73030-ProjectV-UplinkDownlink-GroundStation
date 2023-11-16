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
            String fakeEndpointGround = "/rceiveData";
            String fakeEndPointPassthrough = "/reflectData";
            
            Queue<String> testQueue = GroundSender_Stubs.GetFakedTransmissionQuueue();
            Mutex bufferlock = new Mutex();
            GroundSender ground = new GroundSender(fakeAddress, ref testQueue, ref bufferlock);
            GroundSender passthrough = new GroundSender(fakeAddress, ref testQueue, ref bufferlock);
            DownLink_MadeMockable link = new DownLink_MadeMockable(fakeAddress, fakeEndpointGround, fakeEndPointPassthrough, ref ground, ref passthrough);
            String testPayload = "Test payload";

            //Act
            link.AddToQueue(testPayload);
            ground.SendTransmission();
            passthrough.SendTransmission();


            //Assert
            Assert.IsTrue(ground.isRunning());
            Assert.IsTrue(passthrough.isRunning());
        }
    }
}