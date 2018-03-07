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
                        registerUser(message);
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
       private void registerUser(SocketMessage message){
           //Expected input:
           //%register - Register the user with nil weight
           //%register 250 - Register the user with weight 250
           //%register 250 200 - Register the user with weight 250 and goal of 200
           string[] messageContext=message.Content.Split(" ");
           //try{
               decimal userWeight=0;
               decimal userGoal=0;
               switch(messageContext.Length){
                   case 3:
                    userGoal=Decimal.Parse(messageContext[2]);
                    goto case 2;
                   case 2:
                    userWeight=Decimal.Parse(messageContext[1]);
                    break;
                   default:
                   break;
               }
               //ok if we made it past this point our little kitten should be good and ready to be petted and placed into the database nyaaaa~
               using(var db=new UserContext()){
                   var user=new User{
                       discordID=message.Author.Id,
                       currentWeight=userWeight,
                       goal=userGoal
                   };
                   db.Users.Add(user);
                   db.SaveChanges();
               }

        //    }catch(Exception e){
        //        await message.Channel.SendMessageAsync($"Wrong syntax for command, was expecting decimals.\n{e.GetType().ToString()}\n{e.StackTrace}");
        //    }
       }
    }
}
