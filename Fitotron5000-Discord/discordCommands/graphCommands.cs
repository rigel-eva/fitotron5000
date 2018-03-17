using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OxyPlot.CommonGraphics;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace Fitotron5000.discordCommands
{
    class graphCommands
    {
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
                    //And exporting our PNG
                    using (var stream = new MemoryStream())
                    {
                        var pngExport = new OxyPlot.CommonGraphics.PngExporter()
                        {
                            Width = 1024,
                            Height = 768
                        };
                        pngExport.Export(weightGraph, stream);
                        stream.Position = 0;
                        await message.Channel.SendFileAsync(stream, $"Graph for {message.Author.Username}#{message.Author.Discriminator} created on {DateTime.Now} SHUT UP MOM I DO WHAT I WANT OF COURSE I'M MAKING IT LONGER.jpg.png");
                    }
                }
            }
        }
    }
}
