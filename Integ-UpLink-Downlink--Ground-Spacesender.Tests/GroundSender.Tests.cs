using link;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Integration_Tests
{
    [TestClass]
    public class Tests
    { 
        [TestMethod]
        public void GroundSender_StartTransmission_Successfully_Starts_Tranmissions()
        {
            //Arrange
            String testPayload = "testPayload";
            var sender = new GroundSender("test");

            //Act
            bool methodReturnStatus = sender.SendTransmission(ref testPayload);

            //Assert
            Assert.IsTrue(methodReturnStatus);
        }
        [TestMethod]
        public void GroundSender_StartTransmission_Starts_Tranmission_Throws_OutOfMemory_Exceptions()
        {
            //Arrange
            String testPayload = "testPayload";
            var sender = new GroundSender_Exception_OutOfMemory("test");

            //Act
            bool testReturnValue = sender.SendTransmission(ref testPayload);

            //Assert
            Assert.IsFalse(testReturnValue);
        }
        [TestMethod]
        public void GroundSender_StartTransmission_Starts_Tranmission_Throws_ThreadState_Exceptions()
        {
            //Arrange
            String testPayload = "testPayload";
            var sender = new GroundSender_Exception_ThreadStateException("test");

            //Act
            bool testReturnValue = sender.SendTransmission(ref testPayload);
            

            //Assert
            Assert.IsFalse(testReturnValue);
        }

        [TestMethod]
        public void GroundSender_StartSendThread_Changes_Transmission_State()
        {
            //Arrange
            String testPayload = "testPayload";
            var sender = new GroundSender_Exception_HttpRequestException("test");

            //Act
            sender.StartSendThread();
            bool testTransmissionStatus = sender.transmissionStatus;

            //Assert
            Assert.IsTrue(testTransmissionStatus);
        }
    }
}