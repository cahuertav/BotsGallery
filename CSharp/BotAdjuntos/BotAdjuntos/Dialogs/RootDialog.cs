using BotAdjuntos.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BotAdjuntos.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        const string subscriptionKey = "tu llave de reconocimiento de texto";
        const string uriBase = "https://southcentralus.api.cognitive.microsoft.com/vision/v1.0/ocr";
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;


            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            string requestParameters = "language=en&detectOrientation=true";
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            var attachmentUrl = activity.Attachments[0].ContentUrl;
            var httpClient = new HttpClient();
            var attachmentData = await httpClient.GetByteArrayAsync(attachmentUrl);

            using (ByteArrayContent content = new ByteArrayContent(attachmentData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                string contentString = await response.Content.ReadAsStringAsync();


                await context.PostAsync(GetFinalWord(contentString));
            }
            context.Wait(MessageReceivedAsync);
        }

        static string GetFinalWord(string json)
        {
            var serializedEntity = JsonConvert.DeserializeObject<Models.Rootobject>(json);

            List<Word> wordsList = (serializedEntity.regions[0].lines[0].words).ToList<Word>();

            string finalWord = string.Empty;

            foreach (var item in wordsList)
            {
                finalWord = finalWord + item.text;
            }

            return finalWord;
        }
    }
}