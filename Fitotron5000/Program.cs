using Discord;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord.WebSocket;
namespace Fitotron5000
{

    internal class Program
    {
        private const string commandPrefix = "%";
        private static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();
        public async Task MainAsync(string[] args)
        {
            string token="";//Where our token will be eventually stored
            for(int i =0; i<args.Length; i++){
                switch(args[i].Split(" ")[0]){
                    case "--discordKey":
                        Console.WriteLine($"{DateTime.Now.ToString("h:mm:ss")}Info\tUsing Key from Command Line");
                        token=args[i].Split(" ")[1];//ok, we want to grab the next argument in the set and skip past everything else
                        break;
                }
            }
            var client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += messageHandler;
            //client.MessageReceived += CopyConstructor;
            //Note we are expecting the enviroment variable in the user profile.
            if(token==""){
                token = System.Environment.GetEnvironmentVariable("fitotronkey");
            }
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private async Task messageHandler(SocketMessage message){
            if(!message.Author.IsBot&&!message.Author.IsWebhook&&message.Content.Substring(0,commandPrefix.Length)==commandPrefix){
                switch(message.Content.Split(" ")[0].ToLower()){
                    case commandPrefix+"hard":
                        await message.Channel.SendMessageAsync("AS NAILS!");
                        break;
                    case commandPrefix+"ping":
                        await message.Channel.SendMessageAsync("Pong!");
                        break;
                    case commandPrefix+"pong":
                        await message.Channel.SendMessageAsync("Ping!");
                        break;
                    case commandPrefix+"copyconstructor":
                        CopyConstructor(message);
                        break;
                    case commandPrefix+"register":
                        await registerUser(message);
                        break;
                    case commandPrefix + "userinfo":
                        await getUser(message);
                        break;
                    case commandPrefix+"exit":
                        if(message.Author.Id==176537265336614912){
                            await message.Channel.SendMessageAsync($"Ok, Exiting {message.Author.Username}.");
                            System.Environment.Exit(1);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void CopyConstructor(SocketMessage message)
        {
               message.Channel.SendMessageAsync(message.Content);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private async Task WriteError(ISocketMessageChannel channel,Exception e)
        {
            await channel.SendMessageAsync($"Something went wrong on the server ... the admin should check the logs\nMore info:\t**{e.GetType().ToString()}**\n\t\t{e.Message}\n\t\t{e.HelpLink}");
            Console.WriteLine($"Error Thrown in registerUser! {e.GetType().ToString()}\n\t\t{e.Message}\n\t\t{e.StackTrace}\n\t\t{e.HelpLink}");
        }
        private async Task registerUser(SocketMessage message){
           //Expected input:
           //%register - Register the user with nil weight
           //%register 250 - Register the user with weight 250
           //%register 250 200 - Register the user with weight 250 and goal of 200
           string[] messageContext=message.Content.Split(" ");
            try
            {
                decimal userWeight = 0;
                decimal userGoal = 0;
                switch (messageContext.Length)
                {
                    case 3:
                        userGoal = Decimal.Parse(messageContext[2]);
                        goto case 2;
                    case 2:
                        userWeight = Decimal.Parse(messageContext[1]);
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
                    await db.SaveChangesAsync();
                }
                await message.Channel.SendMessageAsync("Successuflly added user!");

            }
            catch (Exception e)
            {
                await WriteError(message.Channel,e);
            }
        }
        private async Task getUser(SocketMessage message)
        {
            string[] messageContext = message.Content.Split(" ");
            try
            {
                using(var db=new Models.fitotron_devContext())
                {
                    var user = db.Users.Where(e => e.discordID == message.Author.Id);
                    if (user == null)
                    {
                        await message.Channel.SendMessageAsync("Could not find user!");
                    }
                    else
                    {
                        Models.Users foundUser = user.First<Models.Users>();
                        await message.Channel.SendMessageAsync($"User Found!\n\tCurrent Weight: {foundUser.CurrentWeight} Lbs\n\tWeight Goal: {foundUser.Goal}");
                    }
                }
            }
            catch(Exception e)
            {
                await WriteError(message.Channel, e);
            }
        }
        
    }
}
