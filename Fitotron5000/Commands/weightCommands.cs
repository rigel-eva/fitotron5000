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
        public static async Task generateChart(SocketMessage message)
        {
            string[] messageContext = message.Content.Split(" ");
            ulong userId;
            if (message.MentionedUsers.Any())
            {
                userId = message.MentionedUsers.First().Id;                
            }
            else
            {
                userId = message.Author.Id;
            }
            using (var db = new Models.fitotron_devContext())
            {
                var user = db.Users.Where(e => e.discordID == userId);
                if (user.Any())
                {
                    Models.Users foundUser = user.First();
                    //Ordering our data points by when they were generated
                    var userWeights = db.Weights.Where(e => e.User == foundUser);
                    //Ok setting up our series in OxyPlot
                    OxyPlot.Series.LineSeries weightData = new OxyPlot.Series.LineSeries();
                    //And adding all of our datapoints in order
                    foreach (Models.Weights weight in userWeights)
                    {
                        weightData.Points.Add(new OxyPlot.DataPoint(((DateTime)weight.TimeStamp).ToOADate(), (double)weight.UserWeight));
                    }
                    //Ok and setting up our graph
                    OxyPlot.PlotModel weightGraph = new OxyPlot.PlotModel()
                    {
                        PlotType = OxyPlot.PlotType.XY,
                        Background = OxyPlot.OxyColor.FromArgb(0x80, 0x2c, 0x2f, 0x33),
                        TextColor = OxyPlot.OxyColor.FromRgb(0x99, 0xaa, 0xb5),
                    };
                    //And adding our series to our graph
                    weightGraph.Series.Add(weightData);
                    if (foundUser.Goal != null)
                    {
                        OxyPlot.Series.LineSeries goalLine = new OxyPlot.Series.LineSeries();
                        DateTime firstTime = (DateTime)userWeights.First().TimeStamp;
                        DateTime lastTime = (DateTime)userWeights.Last().TimeStamp;
                        goalLine.Points.Add(new OxyPlot.DataPoint(firstTime.ToOADate(), (double)foundUser.Goal));
                        goalLine.Points.Add(new OxyPlot.DataPoint(lastTime.ToOADate(), (double)foundUser.Goal));
                        weightGraph.Series.Add(goalLine);
                    }
                    //And exporting our SVG
                    string svgString;
                    using (var stream = File.Create("tempSVG.svg"))
                    {
                        var svgExport = new OxyPlot.SvgExporter()
                        {
                            Width = 1024,
                            Height = 768
                        };
                        svgExport.Export(weightGraph, stream);
                        svgString=svgExport.ExportToString(weightGraph);
                        stream.Close();
                    }
                    //ok now that the svg is exported, let's go ahead and convert this to a png because discord doesn't support embeded SVGs GOD DAMNIT
                    await message.Channel.SendFileAsync("tempPNG.png");//Replace with actual svg to png/jpg code plz.
                    File.Delete("tempSVG.svg");
        }
    }
}