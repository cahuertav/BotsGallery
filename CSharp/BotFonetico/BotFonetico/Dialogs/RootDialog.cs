using System;
using System.Threading.Tasks;
using BotFonetico.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotFonetico.Dialogs
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
            AlphabetHelper lettersHelper = new AlphabetHelper();

            char[] messageArray = activity.Text.ToCharArray();

            foreach (char item in messageArray)
            {
                string finalCharacter = item.ToString().ToLower();
                await context.PostAsync(lettersHelper.GetLetterName(finalCharacter));
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}