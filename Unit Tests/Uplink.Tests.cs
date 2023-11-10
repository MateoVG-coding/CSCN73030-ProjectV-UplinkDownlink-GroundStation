using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Project_5;


namespace Unit_Tests
{
    [TestClass]
    public class UplinkTests
    {
        [TestMethod]
        public void AddToQueue_WhenQueueNotFull_ShouldReturnTrue()
        {
            var uplink = new Uplink("Address", "PassThroughEndPoint", "GroundStationEndPoint");
            var payload = "TestPayload";

            // Act
            var result = uplink.AddToQueue(payload);   

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddToQueue_WhenQueueFull_ShouldReturnFalse()
        {
            // Arrange
            var uplink = new Uplink("Address", "PassThroughEndPoint", "GroundStationEndPoint");
            // Fill up the queue
            for (int i = 0; i < 10; i++)
            {
                uplink.AddToQueue($"Payload{i}");
            }
            var payload = "TestPayload";

            // Act
            var result = uplink.AddToQueue(payload);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
