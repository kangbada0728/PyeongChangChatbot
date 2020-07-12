namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using System.Text;
    using System.Data.SqlClient;

    [Serializable]
    public class searchEventDialog : IDialog<string>
    {
        private const string event1 = "Opening Ceremony";
        private const string event2 = "Alpine Skiing";
        private const string event3 = "Bobsleigh";
        private const string event4 = "Biathlon";
        private const string event5 = "Cross-Country Skiing";
        private const string event6 = "Curling";
        private const string event7 = "Freestyle Skiing";
        private const string event8 = "Figure Skating";
        private const string event9 = "Ice Hockey";
        private const string event10 = "Luge";
        private const string event11 = "Nordic Combined";
        private const string event12 = "Snowboard";
        private const string event13 = "Ski Jumping";
        private const string event14 = "Skeleton";
        private const string event15 = "Speed Skating";
        private const string event16 = "Short Track Speed Skating";
        private const string event17 = "Closing Ceremony";



        public async Task StartAsync(IDialogContext context)
        {
            PromptDialog.Choice(
                context,
                this.AfterChoiceSelected,
                new[] { event1, event2, event3, event4, event5, event6, event7, event8, event9, event10,
                    event11, event12, event13, event14, event15, event16, event17},
                "Please select sports you want",
                "Please select sports you want",
                attempts: 3);
        }

        private async Task AfterChoiceSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string selection = await result;

                await context.PostAsync("You chose " + selection + ". Please wait");

                try
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = "pyeongchang.database.windows.net";
                    builder.UserID = "hjs0579";
                    builder.Password = "(wnstjs)3578";
                    builder.InitialCatalog = "PyeongChangDatabase";

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        Console.WriteLine("\nQuery data example:");
                        Console.WriteLine("=========================================\n");

                        connection.Open();
                        StringBuilder sb = new StringBuilder();
                        sb.Append("SELECT * ");
                        sb.Append("FROM [dbo].[SportsInformation] ");
                        sb.Append("WHERE [Sports] = '\"" + selection + "\"'");
                        String sql = sb.ToString();

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    await context.PostAsync("====================");
                                    await context.PostAsync("Sports: " + reader.GetString(0));
                                    await context.PostAsync("\nDescription: " + reader.GetString(1));
                                    await context.PostAsync("\nDate: " + reader.GetString(3) + "." + reader.GetString(4) + "." + reader.GetString(5));
                                    await context.PostAsync("\nDay of Week: " + reader.GetString(6));
                                    await context.PostAsync("\nStart Time: " + reader.GetString(7) + ":" + reader.GetString(8));
                                    await context.PostAsync("\nEnd Time: " + reader.GetString(9) + ":" + reader.GetString(10));
                                    await context.PostAsync("\nVenue: " + reader.GetString(11));
                                }
                                await context.PostAsync("====================");
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.Source);
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.ToString());
                }

                context.Done(selection);

            }
            catch (TooManyAttemptsException)
            {
                await this.StartAsync(context);
            }
        }
    }
}