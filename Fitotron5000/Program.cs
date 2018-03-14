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
        public const string registerCommand = "register";
        public const string commandPrefix = "%";
        private static void Main(string[] args) => new Program().MainAsync(args).GetAwaiter().GetResult();
        public async Task MainAsync(string[] args)
        {
            string token = "";//Where our token will be eventually stored
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].Split(" ")[0])
                {
                    case "--discordKey":
                        Console.WriteLine($"{DateTime.Now.ToString("h:mm:ss")}Info\tUsing Key from Command Line");
                        token = args[i].Split(" ")[1];//ok, we want to grab the next argument in the set and skip past everything else
                        break;
                }
            }
            var client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += messageHandler;
            //client.MessageReceived += CopyConstructor;
            //Note we are expecting the enviroment variable in the user profile.
            if (token == "")
            {
                token = System.Environment.GetEnvironmentVariable("fitotronkey");
            }
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private async Task messageHandler(SocketMessage message)
        {
            if (!message.Author.IsBot && !message.Author.IsWebhook && message.Content.Substring(0, commandPrefix.Length) == commandPrefix)
            {
                switch (message.Content.Split(" ")[0].ToLower())
                {
                    case commandPrefix + "hard":
                        await message.Channel.SendMessageAsync("AS NAILS!");
                        break;
                    case commandPrefix + "ping":
                        await message.Channel.SendMessageAsync("Pong!");
                        break;
                    case commandPrefix + "pong":
                        await message.Channel.SendMessageAsync("Ping!");
                        break;
                    case commandPrefix + "copyconstructor":
                        CopyConstructor(message);
                        break;
                    case commandPrefix + "isthisthekrustykrab":
                        await message.Channel.SendMessageAsync($"NO THIS IS FITOTRON5000");
                        break;
                    case commandPrefix + registerCommand:
                        await Commands.userRegistration.registerUser(message);
                        break;
                    case commandPrefix + "userinfo":
                        await Commands.userRegistration.getUser(message);
                        break;
                    case commandPrefix + "addweight":
                        Commands.weightCommands.addWeight(message);
                        break;
                    case commandPrefix + "exit":
                        if (message.Author.Id == 176537265336614912)
                        {
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
        private async Task WriteError(ISocketMessageChannel channel, Exception e)
        {
            await channel.SendMessageAsync($"Something went wrong on the server ... the admin should check the logs\nMore info:\t**{e.GetType().ToString()}**\n\t\t{e.Message}\n\t\t{e.HelpLink}");
            Console.WriteLine($"Error Thrown in registerUser! {e.GetType().ToString()}\n\t\t{e.Message}\n\t\t{e.StackTrace}\n\t\t{e.HelpLink}");
        }
    }
}
