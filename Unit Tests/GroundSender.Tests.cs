using Moq;
using Moq.AutoMock;

namespace Unit_Tests
{
    [TestClass]
    public class GroundSenderTests
    {
        String testJsonString = "This is a json test string the contents matters not";

        [TestMethod]
        public void GroundSender_Sending_Transmission_Fills_Transmission_Buffer()
        {

            //Arrange
            GroundSender sender = new GroundSender();

            //Act
            sender.SendTransmission(ref testJsonString);

            //Assert
            Assert.IsFalse(sender.IsBufferEmpty());

        }

        [TestMethod]
        public void GroundSender_Not_Added_Transmissions_Buffer_Is_Empty()
        {
            //Arrange
            GroundSender sender = new GroundSender();

            //Act
            //Assert
            Assert.IsTrue(sender.IsBufferEmpty());
        }

        [TestMethod]
        public void GroundSender_StartTransmission_Sends_Data_Tranmission_Status_Is_True()
        {
            //Arrange
            GroundSender sender = new GroundSender();

            //Act
            for (int i = 0; i < 10; i++)
                sender.SendTransmission(ref testJsonString);

            //Assert
            Assert.AreEqual(true, sender.transmissionStatus);
        }

        [TestMethod]
        public void GroundSender_StartTransmission_Sends_Data_Running_Status_Is_True()
        {
            //Arrange
            GroundSender sender = new GroundSender();

            //Act
            for (int i = 0; i < 10; i++)
                sender.SendTransmission(ref testJsonString);

            //Assert
            Assert.AreEqual(true, sender.isRunning());
        }

    }
}