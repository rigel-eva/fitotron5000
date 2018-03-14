using Discord.WebSocket;
using System.Linq;
using System;
using System.Threading.Tasks;
namespace Fitotron5000.Commands
{
    class userCommands
    {
        static public async Task getUser(SocketMessage message)
        {
            string[] messageContext = message.Content.Split(" ");
            try
            {
                using (var db = new Models.fitotron_devContext())
                {
                    var user = db.Users.Where(e => e.discordID == message.Author.Id);
                    if (user.Any())
                    {
                        Models.Users foundUser = user.First();
                        string returnString = "User Found!";
                        if (foundUser.CurrentWeight != null)
                        {
                            returnString += $"\n\tCurrent Weight: {Math.Round((double)foundUser.CurrentWeight, 1)} Lbs";
                        }
                        if (foundUser.Goal != null)
                        {
                            returnString += $"\n\tWeight Goal: {Math.Round((double)foundUser.Goal, 1)} Lbs";
                        }
                        await message.Channel.SendMessageAsync(returnString);
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("Could not find user!");
                    }
                }
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static public async Task registerUser(SocketMessage message)
        {
            //Expected input:
            //%register - Register the user with nil weight
            //%register 250 - Register the user with weight 250
            //%register 250 200 - Register the user with weight 250 and goal of 200
            string[] messageContext = message.Content.Split(" ");
            float userWeight = 0;
            float userGoal = 0;
            switch (messageContext.Length)
            {
                case 3:
                    userGoal = float.Parse(messageContext[2]);
                    goto case 2;
                case 2:
                    userWeight = float.Parse(messageContext[1]);
                    break;
                default:
                    break;
            }
            //ok if we made it past this point our little kitten should be good and ready to be petted and placed into the database nyaaaa~
            using (var db = new Models.fitotron_devContext())
            {
                var user = new Models.Users
                {
                    discordID = message.Author.Id,
                    CurrentWeight = userWeight,
                    Goal = userGoal
                };
                await db.Users.AddAsync(user);
                var weight = new Models.Weights
                {
                    User = user,
                    UserWeight = userWeight,
                    TimeStamp = DateTime.Now
                };
                await db.Weights.AddAsync (weight);
                await db.SaveChangesAsync();
            }
            await message.Channel.SendMessageAsync("Successuflly added user!");
        }
    }
}
