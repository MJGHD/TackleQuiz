using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JSONSerialise;

namespace Networking
{
    //TODO: Make all of this networking stuff async if needed
    class ServerConnection
    {
        public bool ServerRequest(string source, string[] parameters)
        {
            IPAddress ServerIP = IPAddress.Parse("192.168.1.133");
            TcpClient client = new TcpClient();

            //Connects the client to the server and creates a TCP communication stream
            client.Connect(ServerIP, 8888);
            NetworkStream stream = client.GetStream();

            //Gets the serialised JSON and converts it to a byte array to be sent to the server
            var serialisation = new ServerRequest();
            serialisation.requestSource = source;
            serialisation.requestParameters = parameters;
            string json = serialisation.Serialise(serialisation);

            byte[] messageToServer = Encoding.ASCII.GetBytes(json);
            stream.Write(messageToServer, 0, messageToServer.Length);

            //Handling the response from the server based on the page that the request came from
            if(serialisation.requestSource == "SIGNUP")
            {
                byte[] readBuffer = new byte[128];
                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.ASCII.GetString(readBuffer);
                Debug.Write(messageFromServer);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
