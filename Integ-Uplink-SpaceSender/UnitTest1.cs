using Project_5;

namespace Integ_Uplink_SpaceSender
{
    [TestClass]
    public class Uplink_Integration_Tests
    {
        [TestMethod]
        public void Uplink_AddToQueue_Success()
        {
            var uplink = new Uplink("address", "passThroughEndPoint", "groundStationEndPoint");

            bool result = uplink.AddToQueue("payload");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Uplink_AddToQueue_FailWhenQueueFull()
        {
            var uplink = new Uplink("address", "passThroughEndPoint", "groundStationEndPoint");

            // Fill up the queue
            for (int i = 0; i < 10; i++)
            {
                uplink.AddToQueue($"payload{i}");
            }

            bool result = uplink.AddToQueue("extraPayload");

            // Assert
            Assert.IsFalse(result);
        }
    }
}