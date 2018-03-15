using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Linq;
using System.IO;
namespace Fitotron5000.Commands
{
    class weightCommands
    {
        public static async Task addWeight(SocketMessage message)
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
                        await db.Weights.AddAsync(weight);
                        foundUser.CurrentWeight = newWeight;
                        db.Users.Update(foundUser);
                        await db.SaveChangesAsync();
                        await message.Channel.SendMessageAsync("Added New Weight!");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync($"Error!: Was expecting a numeric number. instead got {args[1]}");
                    }
                }
                else
                {
                    await message.Channel.SendMessageAsync($"Error!: You need to register first with {Program.commandPrefix+Program.registerCommand}");
                }
            }
        }
    }
}