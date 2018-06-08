using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace LanguageDetection.Dialogs
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
            context.PostAsync(languageCode);
        }

        public async static Task<string> GetDesiredLanguageAsync(string content)
        {
            HttpClient languageClient = new HttpClient();
            languageClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "45474d57d06346679d2c66648a76f175");
            var detectLanguageResponse = await languageClient.GetStreamAsync(
                $"http://api.microsofttranslator.com/v2/http.svc/Detect?text={content}");

            return (string)new DataContractSerializer(typeof(string)).ReadObject(detectLanguageResponse);
        }
    }
}