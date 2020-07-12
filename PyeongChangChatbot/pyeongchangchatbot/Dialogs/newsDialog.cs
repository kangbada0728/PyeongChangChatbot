namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using System.Collections;
    using HtmlAgilityPack;

    [Serializable]
    public class newsDialog : IDialog<string>
    {
        ArrayList titles = new ArrayList();
        ArrayList descriptions = new ArrayList();
        ArrayList datetimes = new ArrayList();
        ArrayList addresses = new ArrayList();

        string url = "https://www.pyeongchang2018.com/en/bbs/press/image/list";

       


        public int limit = 3;

        public async Task StartAsync(IDialogContext context)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='photo-tit']"))
            {
                titles.Add(node.InnerHtml.Trim());
            }
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='photo-desc']"))
            {
                String[] s1 = node.InnerHtml.Trim().Split('\n');
                String[] s2 = s1[s1.Length - 1].Trim().Split(' ');
                String description = "";
                for (int i = 4; i < s2.Length; i++)
                {
                    description = description + " " + s2[i];
                }
                descriptions.Add(description);
            }

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='photo-date']"))
            {
                datetimes.Add(node.InnerHtml.Trim());
            }
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='photo-list']//a[@href='#view']"))
            {
                String[] s1 = node.OuterHtml.Trim().Split('\'');
                String address = "https://www.pyeongchang2018.com/en/bbs/press/image/view?menuId=255&bbsId=28&searchOpt=&searchTxt=&pageNo=1&sortSeCd=3&cnId=" + s1[1] + "&rows=" + s1[3] + "&langSeCd=en";
                addresses.Add(address);
            }

            Random rnd = new Random();
            int index = rnd.Next(15);

            await context.PostAsync("title: " + titles[index].ToString());
            await context.PostAsync("descriptions: " + descriptions[index].ToString());
            await context.PostAsync("datetimes: " + datetimes[index].ToString());
            await context.PostAsync("addresses: " + addresses[index].ToString());

            limit--;

            if (limit > 0)
            {
                PromptDialog.Choice(
               context,
               this.AfterChoiceSelected,
               new[] { "Other News", "Backwards" },
               "If you want other news, please press the 'Other News' button or the 'Backwards' button if it is not.",
               "If you want other news, please press the 'Other News' button or the 'Backwards' button if it is not.",
               attempts: 3);
            }
            else
            {
                await context.PostAsync("More News can be found at:");
                await context.PostAsync(url);
                context.Done(url);

            }

        }

        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selection = await result;

                switch (selection)
                {
                    case "Other News":
                        await this.StartAsync(context);
                        break;
                    case "Backwards":
                        context.Done(selection);
                        break;
                }
            }
            catch (TooManyAttemptsException)
            {
                await this.StartAsync(context);
            }
        }

    }


}