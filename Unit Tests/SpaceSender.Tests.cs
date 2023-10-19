using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Unit_Tests
{
    [TestClass]
    public class SpaceSenderTests
    {
        [TestMethod]
        public void SpaceSender_SendTransmission_AddsToQueue()
        {
            // Arrange
            var sender = new SpaceSender("http://example.com");
            var testData = "test data";

            // Act
            sender.SendTransmission(testData);

            // Assert
            Assert.IsFalse(sender.IsBufferEmpty());
        }

        [TestMethod]
        public async Task SpaceSender_SendTransmission_StartsThreadIfNotRunning()
        {
            // Arrange
            var sender = new SpaceSender("http://example.com");
            var testData = "Test Data";

            // Act
            sender.SendTransmission(testData);

            await Task.Delay(5000); // Wait for up to 1 second

            // Assert
            Assert.IsTrue(sender.TransmissionStatus); // Check the TransmissionStatus
        }


        [TestMethod]
        public async Task SpaceSender_SendTransmission_DoesNotStartThreadIfAlreadyRunning()
        {
            // Arrange
            var sender = new SpaceSender("http://example.com");
            var testData1 = "test data 1";
            var testData2 = "test data 2";

            // Act
            sender.SendTransmission(testData1);
            await Task.Delay(5000); // Give the thread some time to start.
            sender.SendTransmission(testData2);

            // Assert
            Assert.IsTrue(sender.IsRunning()); // The thread should only start once.
        }

        [TestMethod]
        public void SpaceSender_IsBufferEmpty_ReturnsTrueWhenEmpty()
        {
            // Arrange
            var sender = new SpaceSender("http://example.com");

            // Act
            var isEmpty = sender.IsBufferEmpty();

            // Assert
            Assert.IsTrue(isEmpty);
        }

        [TestMethod]
        public void SpaceSender_SendPing_StartsPingThread()
        {
            SpaceSender sender = new SpaceSender("http://example.com");
            bool isRunning = sender.IsRunning();
            Assert.IsFalse(isRunning); 

            sender.SendPing(); 
            isRunning = sender.IsRunning_Ping();
            Assert.IsTrue(isRunning); 
        }


        [TestMethod]
        public async Task SpaceSender_SendPing_SetStatusToTrueWhenPingResponseIsReceived()
        {
            SpaceSender sender = new SpaceSender("http://example.com");
            Assert.IsFalse(sender.TransmissionStatus); 

            sender.SendPing();
            await Task.Delay(500);
            Assert.IsTrue(sender.TransmissionStatus);
        }

        
    }
}