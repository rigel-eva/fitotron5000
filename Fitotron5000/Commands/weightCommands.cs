using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using System.Linq;
namespace Fitotron5000.Commands
{
    class weightCommands
    {
        public static void addWeight(SocketMessage message)
        {
            string[] args = message.Content.Split(" ");
            //Ok, we are expecting the format of `%addweight 888` where 888 is the weight
            using(var db=new Models.fitotron_devContext())
            {
                var user = db.Users.Where(e => e.discordID == message.Author.Id);
                if (user.Any())
                {
                    Models.Users foundUser = user.First();
                    double newWeight;
                    if (double.TryParse(args[1], out newWeight )){
                        Models.Weights weight = new Models.Weights
                        {
                            User = foundUser,
                            UserWeight = newWeight,
                            TimeStamp = DateTime.Now
                        };
                        db.Weights.Add(weight);
                        foundUser.CurrentWeight = newWeight;
                        db.Users.Update(foundUser);
                        db.SaveChanges();
                        message.Channel.SendMessageAsync("Added New Weight!");
                    }
                    else
                    {
                        message.Channel.SendMessageAsync($"Error!: Was expecting a numeric number. instead got {args[1]}");
                    }
                }
                else
                {
                    message.Channel.SendMessageAsync($"Error!: You need to register first with {Program.commandPrefix+Program.registerCommand}");
                }
            }
        }
    }
}
