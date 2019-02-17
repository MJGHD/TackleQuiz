using Newtonsoft.Json;
using Results;

namespace JSON
{
    class ServerRequest
    {
        public string requestSource;
        public string[] requestParameters;

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
