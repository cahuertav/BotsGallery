using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Text;

namespace TranslatorBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            string content = activity.Text;

            string languageCode = await GetDesiredLanguageAsync(content);
            string response = await TranslateSentenceAsync(content, languageCode);
            context.PostAsync(response);
        }

        public async static Task<string> GetDesiredLanguageAsync(string content)
        {
            HttpClient languageClient = new HttpClient();
            languageClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "45474d57d06346679d2c66648a76f175");
            var detectLanguageResponse = await languageClient.GetStreamAsync(
                $"http://api.microsofttranslator.com/v2/http.svc/Detect?text={content}");

            return (string)new DataContractSerializer(typeof(string)).ReadObject(detectLanguageResponse);
        }

        public async static Task<string> TranslateSentenceAsync(string originalSentence, string languageCode)
        {
            Object[] body = new Object[] { new { Text = originalSentence } };
            var requestBody = JsonConvert.SerializeObject(body);
            string convertedAnswer = string.Empty;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(String.Format("https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&to=en", languageCode));
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", "45474d57d06346679d2c66648a76f175");

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                convertedAnswer = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);
            }

            string createdBody = "{ \"documents\": [ { \"result\": " + convertedAnswer + "} ] }";
            var message = JsonConvert.DeserializeObject<Rootobject>(createdBody).documents[0].result[0].translations[0].text;


            return message;
        }
    }

    public class Rootobject
    {
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public Result[] result { get; set; }
    }

    public class Result
    {
        public Detectedlanguage detectedLanguage { get; set; }
        public Translation[] translations { get; set; }
    }

    public class Detectedlanguage
    {
        public string language { get; set; }
        public float score { get; set; }
    }

    public class Translation
    {
        public string text { get; set; }
        public string to { get; set; }
    }
}