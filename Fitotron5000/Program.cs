using Discord;
using System;
using System.Threading.Tasks;
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
            client.MessageReceived += MessageReceived;
            client.MessageReceived += Ping;
            //client.MessageReceived += CopyConstructor;
            //Note we are expecting the enviroment variable in the user profile.
            string token = System.Environment.GetEnvironmentVariable("fitotronkey", EnvironmentVariableTarget.User); // Remember to keep this private!
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        private async Task MessageReceived(SocketMessage message)
        {
            if (isCommand(message,"hard"))
            {
                await message.Channel.SendMessageAsync("AS NAILS!");
            }
        }
        private async Task Ping(SocketMessage message)
        {
            if (isCommand(message,"ping"))
            {
                await message.Channel.SendMessageAsync("Pong!");
            }
            else if (isCommand(message, "pong"))
            {
                await message.Channel.SendMessageAsync("Ping!");
            }
        }
        //private async Task CopyConstructor(SocketMessage message)
        //{
        //    if (isCommand(message,"copyconstructor"))
        //    {
        //        await message.Channel.SendMessageAsync(message.Content);
        //    }
        //}
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private bool isCommand(SocketMessage message, string commandInvoker){
            return (message.Content.ToLower().Substring(0, (commandPrefix + commandInvoker).Length) == commandPrefix + commandInvoker);
        }
    }
}
