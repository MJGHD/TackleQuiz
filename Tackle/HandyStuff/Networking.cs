using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using JSON;

namespace Networking
{
    class ServerConnection
    {
        public string ServerRequest(string source, string[] parameters)
        {
            IPAddress ServerIP = IPAddress.Parse("192.168.1.140");
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

            //Handling the response from the server based on the page that the request came from or what buffer size is needed
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
            else if(serialisation.requestSource == "JOINCLASS" || serialisation.requestSource == "REMOVECLASSMEMBER" || serialisation.requestSource == "ACCEPTREQUEST" || serialisation.requestSource == "FINISHMARKING")
            {
                byte[] readBuffer = new byte[64];

                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                return messageFromServer;
            }
            else if(serialisation.requestSource == "DELETECLASS")
            {
                byte[] readBuffer = new byte[64];

                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                return messageFromServer;
            }
            else if(serialisation.requestSource == "CREATECLASS" || serialisation.requestSource == "CREATEDRAFT" || serialisation.requestSource == "SENDTOCLASS" || serialisation.requestSource == "DELETEDRAFT")
            {
                byte[] readBuffer = new byte[64];

                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                return messageFromServer;
            }
            else if (serialisation.requestSource == "CLASSLIST")
            {
                byte[] readBuffer = new byte[1000];

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
            else if(serialisation.requestSource == "CREATEQUIZ")
            {
                byte[] readBuffer = new byte[64];

                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                return messageFromServer;
            }
            //any sort of list that may need a big buffer
            else if (serialisation.requestSource == "QUIZLIST" || serialisation.requestSource == "DRAFTLIST" || serialisation.requestSource == "HOMEWORKLIST" || serialisation.requestSource == "REQUESTLIST" || serialisation.requestSource == "TEACHERQUIZHISTORY" || serialisation.requestSource == "QUIZMARKINGLIST" || serialisation.requestSource == "GETLEADERBOARD")
            {
                byte[] readBuffer = new byte[3000];

                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                return messageFromServer;
            }
            else if (serialisation.requestSource == "OPENQUIZ")
            {
                byte[] readBuffer = new byte[2000];

                stream.Read(readBuffer, 0, readBuffer.Length);
                string messageFromServer = Encoding.Default.GetString(readBuffer);

                return messageFromServer;
            }
            else if (serialisation.requestSource == "CLASSMEMBERLIST")
            {
                byte[] readBuffer = new byte[1000];

                stream.Read(readBuffer, 0, readBuffer.Length);
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
