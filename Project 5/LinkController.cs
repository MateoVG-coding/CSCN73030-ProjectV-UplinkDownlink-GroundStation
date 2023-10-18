using System.Net;
using System.Net.Sockets;

namespace link
{
    public class LinkController
    {
        public void CreateEndpoints()
        {
            string[] endpoints = { "http://localhost:2400/", "http://localhost:2400/status/" };

            HttpListener listener = new HttpListener();

            foreach (string endpoint in endpoints)
            {
                listener.Prefixes.Add(endpoint);
            }
            listener.Start();
            Console.WriteLine(listener.ToString());

            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest req = context.Request;

            Console.WriteLine(req.RawUrl);
            switch(req.RawUrl)
            {
                case "/status/":
                    Console.WriteLine("client requested status");
                    break;
            }
            HttpListenerResponse res = context.Response;

            string responseString = "Bruh";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            res.ContentLength64 = buffer.Length;
            System.IO.Stream output = res.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();
            listener.Stop();
        }
    }

}
