using Newtonsoft.Json;
using Results;

namespace JSON
{
    //used when the client makes a request to the server
    class ServerRequest
    {
        public string requestSource;
        public string[] requestParameters;

        //Serialises various classes to JSON to be handled by the server, as C# objects can't be transferred directly over TCP

        public string Serialise(ServerRequest request)
        {
            string json = JsonConvert.SerializeObject(request, Formatting.Indented);
            return json;
        }

        public string Serialise(QuizResults results)
        {
            string json = JsonConvert.SerializeObject(results, Formatting.Indented);
            return json;
        }

        public string Serialise(Quiz.Quiz quiz)
        {
            string json = JsonConvert.SerializeObject(quiz, Formatting.Indented);
            return json;
        }
    }
}
