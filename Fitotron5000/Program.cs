using Discord;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discord.WebSocket;
namespace Fitotron5000
{

    internal class Program
    {
        private const string commandPrefix = "%";
        private static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            var client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += messageHandler;
            //client.MessageReceived += CopyConstructor;
            //Note we are expecting the enviroment variable in the user profile.
            string token = System.Environment.GetEnvironmentVariable("fitotronkey"); // Remember to keep this private!
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
         private bool isCommand(SocketMessage message, string commandInvoker){
            bool returner=!message.Author.IsBot; //Checking if our bot is not talking to another bot, because that would be dumb.
            Console.WriteLine("Checking if not a bot ... : "+returner.ToString());
            Console.WriteLine("Possible Message: "+message.Content.ToLower());
            Console.WriteLine("Message Check: "+message.Content.ToLower().Substring(0, (commandPrefix + commandInvoker).Length));
            Console.WriteLine("Command looking for: "+commandPrefix + commandInvoker);
            return (!message.Author.IsBot && message.Content.ToLower().Substring(0, (commandPrefix + commandInvoker).Length) == commandPrefix + commandInvoker);
        }
        private async Task messageHandler(SocketMessage message){
            if(!message.Author.IsBot&&message.Content.Substring(0,commandPrefix.Length)==commandPrefix){
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
       
    }
}
