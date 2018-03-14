using Discord.WebSocket;
using System.Linq;
using System;
namespace Fitotron5000
{
    class userRegistration
    {
        static public void getUser(SocketMessage message)
        {
            string[] messageContext = message.Content.Split(" ");
            try
            {
                using (var db = new Models.fitotron_devContext())
                {
                    var user = db.Users.Where(e => e.discordID == message.Author.Id);
                    if (user == null)
                    {
                        message.Channel.SendMessageAsync("Could not find user!");
                    }
                    else
                    {
                        Models.Users foundUser = user.First<Models.Users>();
                        string returnString = "User Found!";
                        if (foundUser.CurrentWeight != null)
                        {
                            returnString+=$"\n\tCurrent Weight: {Math.Round((double)foundUser.CurrentWeight, 1)} Lbs";
                        }
                        if (foundUser.Goal != null)
                        {
                            returnString += $"\n\tWeight Goal: {Math.Round((double)foundUser.Goal, 1)} Lbs";
                        }
                        message.Channel.SendMessageAsync(returnString);
                    }
                }
            } catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static public void registerUser(SocketMessage message)
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
                db.Users.Add(user);
                var weight = new Models.Weights
                {
                    UserId = user.Id,
                    UserWeight = userWeight,
                    TimeStamp = DateTime.Now
                };
                db.Weights.Add(weight);
                db.SaveChanges();
            }
            message.Channel.SendMessageAsync("Successuflly added user!");
        }
    }
}
