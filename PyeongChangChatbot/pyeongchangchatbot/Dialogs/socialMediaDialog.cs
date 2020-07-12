namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class socialMediaDialog : IDialog<string>
    {
        string a = "test";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("The official social media of PyeongChang Olympics are as follows.");
            await context.PostAsync("In turn, Facebook, Twitter, Instagram, Youtube, and Flickr.");
            await context.PostAsync("https://www.facebook.com/PyeongChang2018");
            await context.PostAsync("https://twitter.com/PyeongChang2018");
            await context.PostAsync("https://www.instagram.com/pyeongchang2018/");
            await context.PostAsync("https://www.youtube.com/user/PyeongChang2018");
            await context.PostAsync("https://www.flickr.com/photos/pyeongchang2018_kr");

            context.Done(a);
        }
    }
}