using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JSON;

namespace Networking
{
    class ServerConnection
    {
        public string ServerRequest(string source, string[] parameters)
        {
            IPAddress ServerIP = IPAddress.Parse("10.25.131.115");
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
            if(serialisation.requestSource == "SIGNUP" || serialisation.requestSource == "LOGIN")
            {
                byte[] readBuffer = new byte[64];

                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                LogInResponse response = JsonConvert.DeserializeObject<LogInResponse>(messageFromServer);

                if (response.requestSuccess)
                {
                    if(response.isTeacher == true)
                    {
                        return "TEACHER";
                    }
                    else
                    {
                        return "STUDENT";
                    }
                }
                else
                {
                    return "FAILED";
                }
            }
            else if(serialisation.requestSource == "JOINCLASS")
            {
                byte[] readBuffer = new byte[64];

                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                return messageFromServer;
            }
            else if(serialisation.requestSource == "QUIZMARKINGVIEW")
            {
                byte[] readBuffer = new byte[2000];
                Console.WriteLine("read");
                stream.Read(readBuffer, 0, readBuffer.Length);
                Console.WriteLine("a");
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                return messageFromServer;
            }
            else
            {
                return "placeholder";
            }
        }
    }

    class LogInResponse
    {
        public bool requestSuccess;
        public bool isTeacher;
    }
}
