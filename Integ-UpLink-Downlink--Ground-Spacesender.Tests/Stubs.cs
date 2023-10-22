using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_5
{
    public static class GroundSender_Stubs
    {
        public static HttpResponseMessage HttpRequest_Stub()
        {

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            Console.WriteLine("Setting status code for test and sleeping for 200ms to simulate a response");
            Thread.Sleep(500);
            return response;
        }

        public static void StartSendTransmission_Stub()
        {
            //Do nothing
        }

        public static void StartSendTransmission_Throws_Exception(Exception ex)
        {
            throw ex;
        }
    }

    public static class Downlink_Stubs
    {
        public static bool AddToQueue_Stub(String payload)
        {
            return true;
        }

        public static String PeekAtAddress_Stub(String payload)
        {
            return "Test_Address";
        }
    }

}
