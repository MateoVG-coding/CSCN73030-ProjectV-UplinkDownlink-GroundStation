using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration_Tests_SpaceSender
{
    [TestClass]
    public class Tests_SpaceSender
    {
        [TestMethod]
        public void SpaceSender_StartTransmission_Successfully_Starts_Tranmissions()
        {
            //Arrange
            String testPayload = "testPayload";
            var sender = new SpaceSender("test");

            //Act
            bool methodReturnStatus = sender.SendTransmission(testPayload);

            //Assert
            Assert.IsTrue(methodReturnStatus);
        }

        [TestMethod]
        public void SpaceSender_StartTransmission_Starts_Tranmission_Throws_OutOfMemory_Exceptions()
        {
            //Arrange
            String testPayload = "testPayload";
            var sender = new SpaceSender_Exception_OutOfMemory("test");

            //Act
            bool testReturnValue = sender.SendTransmission(testPayload);

            //Assert
            Assert.IsFalse(testReturnValue);
        }
        [TestMethod]
        public void SpaceSender_StartTransmission_Starts_Tranmission_Throws_ThreadState_Exceptions()
        {
            //Arrange
            String testPayload = "testPayload";
            var sender = new SpaceSender_Exceptions("test");

            //Act
            bool testReturnValue = sender.SendTransmission(testPayload);


            //Assert
            Assert.IsFalse(testReturnValue);
        }
    }
}