namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class tourDialog : IDialog<string>
    {
        public string[] a = new string[3] { "Sangwonsa Temple", "http://english.visitkorea.or.kr/enu/ATR/SI_EN_3_1_1_1.jsp?cid=264220", "1215-89, Odaesan-ro, , Pyeongchang-gun, Gangwon-do" };
        public string[] b = new string[3] { "Woljeongsa Temple", "http://english.visitkorea.or.kr/enu/ATR/SI_EN_3_1_1_1.jsp?cid=264189", "374-8, Odaesan-ro, Jinbu-myeon, Pyeongchang-gun, Gangwon-do" };
        public string[] c = new string[3] { "Baengnyong Cave", "http://english.visitkorea.or.kr/enu/ATR/SI_EN_3_1_1_1.jsp?cid=1367673", "63, Munhui-gil, Mitan-myeon, Pyeongchang-gun, Gangwon-do" };
        public string[] d = new string[3] { "Birthplace of Lee Hyo-seok", "http://english.visitkorea.or.kr/enu/ATR/SI_EN_3_1_1_1.jsp?cid=2363831", "33-11, Ihyoseok-gil, Pyeongchang-gun, Gangwon-do" };
        public string[] e = new string[3] { "Pyeongchang Mooee Arts Center", "http://english.visitkorea.or.kr/enu/ATR/SI_EN_3_1_1_1.jsp?cid=1794973", "233, Saripyeong-gil, Bongpyeong-myeon, Pyeongchang-gun, Gangwon-do" };
        public string[] f = new string[3] { "Daegwallyeong Skyranch", "http://english.visitkorea.or.kr/enu/ATR/SI_EN_3_1_1_1.jsp?cid=2037224", "458-23, Kkotbadyangji-gil, Pyeongchang-gun, Gangwon-do" };
        public string z = "http://english.visitkorea.or.kr/common_intl/mapInformation.kto?md=enu&func_name=main";

        public int limit = 3;

        public async Task StartAsync(IDialogContext context)
        {
            Random r = new Random();

            int num = r.Next(1, 6);

            if (num == 1)
            {
                await context.PostAsync(a[0]);
                await context.PostAsync(a[1]);
                await context.PostAsync(a[2]);
            }
            else if (num == 2)
            {
                await context.PostAsync(b[0]);
                await context.PostAsync(b[1]);
                await context.PostAsync(b[2]);
            }
            else if (num == 3)
            {
                await context.PostAsync(c[0]);
                await context.PostAsync(c[1]);
                await context.PostAsync(c[2]);
            }
            else if (num == 4)
            {
                await context.PostAsync(d[0]);
                await context.PostAsync(d[1]);
                await context.PostAsync(d[2]);
            }
            else if (num == 5)
            {
                await context.PostAsync(e[0]);
                await context.PostAsync(e[1]);
                await context.PostAsync(e[2]);
            }
            else
            {
                await context.PostAsync(f[0]);
                await context.PostAsync(f[1]);
                await context.PostAsync(f[2]);
            }

            limit--;

            if (limit > 0)
            {
                PromptDialog.Choice(
               context,
               this.AfterChoiceSelected,
               new[] { "Recommend Again", "Backwards" },
               "If you want another recommendation, please click the 'Recommend Again' button or the 'Backwards' button if it is not.",
               "If you want another recommendation, please click the 'Recommend Again' button or the 'Backwards' button if it is not.",
               attempts: 3);
            }
            else
            {
                await context.PostAsync("More attraction can be found at:");
                await context.PostAsync(z);
                context.Done(z);

            }
        }

        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selection = await result;

                switch (selection)
                {
                    case "Recommend Again":
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