namespace pyeongchangchatbot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;
    using System.Text;
    using System.Data.SqlClient;

    [Serializable]
    public class searchTimeDialog : IDialog<string>
    {
        private int attempts = 5;
        public int real_date = 0;
        public int real_time1 = 0;
        public int real_time2 = 0;


        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("First, please enter the desired date. You can enter from 8 to 25 days.");
            await context.PostAsync("Please enter in the following format. e.g. 08 or 25");

            context.Wait(this.MessageReceivedAsync);
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string dates = message.Text.Trim();

            if ((message.Text != null) && (dates.Length == 2) && (dates[0] >= 48 && dates[0] <= 57) && (dates[1] >= 48 && dates[1] <= 57))
            {
                int exactDate = int.Parse(dates);

                if (exactDate >= 8 && exactDate <= 25)
                {
                    real_date = exactDate;
                    await context.PostAsync("You chose " +real_date + "th.");
                    await context.PostAsync("Please enter your desired time range.");
                    await context.PostAsync("You can only select in hours. Please type according to the format. E.g. 01-23");
                    context.Wait(this.MessageReceivedAsync2);
                }
                else
                {
                    --attempts;
                    if (attempts > 0)
                    {
                        await context.PostAsync("Available dates range from 8 to 25 days.");
                        await context.PostAsync("Please re-enter the desired date according to the format.e.g. 08 or 25");

                        context.Wait(this.MessageReceivedAsync);
                    }
                    else
                    {
                        context.Fail(new TooManyAttemptsException("You have entered too many incorrect messages."));
                    }
                }

            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I did not understand");
                    await context.PostAsync("Please re-enter the desired date according to the format.e.g. 08 or 25");

                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("You have entered too many incorrect messages."));
                }
            }
        }

        private async Task MessageReceivedAsync2(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string times = message.Text.Trim();

            if ((message.Text != null) && (times.Length == 5) && (times[0] >= 48 && times[0] <= 50) && (times[1] >= 48 && times[1] <= 57) && (times[3] >= 48 && times[3] <= 50) && (times[4] >= 48 && times[4] <= 57) && times[2] == '-')
            {

                int exactDate1 = 10 * (times[0] - 48) + (times[1] - 48);
                int exactDate2 = 10 * (times[3] - 48) + (times[4] - 48);

                if ((exactDate1 >= 0 && exactDate1 <= 24) && (exactDate2 >= 0 && exactDate2 <= 24))
                {
                    if (exactDate1 > exactDate2)
                    {
                        int i = exactDate1;
                        exactDate1 = exactDate2;
                        exactDate2 = i;
                    }

                    real_time1 = exactDate1;
                    real_time2 = exactDate2;
                    
                    await context.PostAsync("You chose from " + real_time1 + ":00 to" + real_time2 + ":00. Please wait.");

                    // 데이터베이스 입력
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
                            sb.Append("WHERE [Day] = " + real_date + "and [StartTime_H] >= " + real_time1 + "and [EndTime_H] < " + real_time2);
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

                    context.Done(times);

                }
                else
                {
                    --attempts;
                    if (attempts > 0)
                    {
                        await context.PostAsync("Available dates range from 00:00 to 24:00");
                        await context.PostAsync("Please re-enter the desired time.");
                        await context.PostAsync("You can only select in hours. Please type according to the format. E.g. 01 - 23");
                        context.Wait(this.MessageReceivedAsync2);
                    }
                    else
                    {
                        context.Fail(new TooManyAttemptsException("You have entered too many incorrect messages."));
                    }
                }
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I didn't understand");
                    await context.PostAsync("Please re-enter the desired time.");
                    await context.PostAsync("You can only select in hours. Please type according to the format. E.g. 01 - 23");
                    context.Wait(this.MessageReceivedAsync2);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("You have entered too many incorrect messages."));
                }
            }



        }




    }
}