using Project_5;

namespace Integ_Uplink_SpaceSender
{
    [TestClass]
    public class Uplink_Integration
    {
        [TestMethod]
        public void Uplink_Provides_Data_To_Send_SpaceSender_Begins_Transmitting()
        {
            //Arrange
            String fakeAddress = "192.168.1.1";
            String fakeEndpointSpace = "https://httpbin.org/post";

            Queue<String> testQueue = Stub_SpaceSender.GetFakedTransmissionQuueue();
            Mutex bufferlock = new Mutex();
            SpaceSender space = new SpaceSender(fakeEndpointSpace, ref testQueue, ref bufferlock);
            Uplink_MadeMockable link = new Uplink_MadeMockable(fakeAddress, fakeEndpointSpace, ref space);

            //Act
            link.AddToQueue("{'path': 'https://httpbin.org/post'}");
            Thread.Sleep(5000);

            //Assert
            Assert.IsTrue(space.IsBufferEmpty());
        }
    }
}