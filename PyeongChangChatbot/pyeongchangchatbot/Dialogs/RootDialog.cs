namespace pyeongchangchatbot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

#pragma warning disable 1998

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string searchDate = "View the Sports schedule by date";
        private const string searchTime = "View the Sports schedule by time";
        private const string searchEvent = "View the Sports schedule by name";
        private const string news = "Get the latest news";
        private const string socialMedia = "Get the official PyeongChang SNS";
        private const string tour = "Get PyeongChang attractions";




        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }
        
        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            await this.SendWelcomeMessageAsync(context);
        }
        
        
        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            //Select
            PromptDialog.Choice(
               context,
               this.AfterChoiceSelected,
               new[] { searchDate, searchTime, searchEvent, news, socialMedia, tour },
               "Please select the item you want.The answer may take up to 5 seconds.",
               "Please select the item you want.The answer may take up to 5 seconds...",
               attempts: 3);
        }
        

        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var selection = await result;

                switch (selection)
                {
                    case searchDate:
                        context.Call(new searchDateDialog(), this.playingAfter);
                        break;

                    case searchTime:
                        context.Call(new searchTimeDialog(), this.playingAfter);
                        break;

                    case searchEvent:
                        context.Call(new searchEventDialog(), this.playingAfter);
                        break;

                    case news:
                        context.Call(new newsDialog(), this.playingAfter);
                        break;

                    case socialMedia:
                        context.Call(new socialMediaDialog(), this.playingAfter);
                        break;

                    case tour:
                        context.Call(new tourDialog(), this.playingAfter);
                        break;

                }
            }
            catch (TooManyAttemptsException)
            {
                await this.StartAsync(context);
            }
        }


        private async Task playingAfter(IDialogContext context, IAwaitable<string> result)
        {  
            await this.SendWelcomeMessageAsync(context);
        }
    }
}