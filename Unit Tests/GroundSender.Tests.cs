using Moq;
using Moq.AutoMock;
using Project_5;

namespace Unit_Tests
{
    [TestClass]
    public class GroundSenderTests
    {
        String testJsonString = "This is a json test string the contents matters not";
        String testURL = "http://192.168.1.10/SendData";
        Queue<String> queue = GroundSender_Stubs.GetFakedTransmissionQuueue();
        Mutex bufferlock = new Mutex();

        [TestMethod]
        public void GroundSender_Sending_Transmission_Fills_Transmission_Buffer()
        {

            //Arrange
            GroundSender sender = new GroundSender(testURL, ref queue, ref bufferlock);

            //Act
            sender.SendTransmission(ref testJsonString);

            //Assert
            Assert.IsFalse(sender.IsBufferEmpty());

        }

        [TestMethod]
        public void GroundSender_Added_Transmissions_Buffer_Is_Not_Empty()
        {
            //Arrange
            GroundSender sender = new GroundSender(testURL, ref queue, ref bufferlock);

            //Act
            //Assert
            Assert.IsFalse(sender.IsBufferEmpty());
        }

        [TestMethod]
        public void GroundSender_StartTransmission_Sends_Data_Tranmission_Status_Is_True()
        {
            //Arrange
            GroundSender sender = new GroundSender(testURL, ref queue, ref bufferlock);

            //Act
            for (int i = 0; i < 10; i++)
                sender.SendTransmission(ref testJsonString);
            Thread.Sleep(200);

            //Assert
            Assert.AreEqual(true, sender.transmissionStatus);
        }

        [TestMethod]
        public void GroundSender_StartTransmission_Sends_Data_Running_Status_Is_True()
        {
            //Arrange
            GroundSender sender = new GroundSender(testURL, ref queue, ref bufferlock);

            //Act
            for (int i = 0; i < 10; i++)
                sender.SendTransmission(ref testJsonString);

            //Assert
            Assert.AreEqual(true, sender.isRunning());
        }

        [TestMethod]
        public void GroundSender_SendTransmission_Returns_True_When_Transmissions_Are_Being_Sent()
        {
            //Arrange
            GroundSender sender = new GroundSender(testURL, ref queue, ref bufferlock);

            //Act
            
            bool testResult = sender.SendTransmission(ref testJsonString);

            //Assert
            Assert.IsTrue(testResult);
        }

    }
}