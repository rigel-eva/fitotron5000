using System;
using System.Collections.Generic;

namespace Fitotron5000.Models
{
    public partial class Users
    {
        public Users()
        {
            Weights = new HashSet<Weights>();
        }

        public long DiscordId { get; set; }
        public double? Goal { get; set; }
        public double? CurrentWeight { get; set; }
        public int Id { get; set; }
        public ulong discordID
        {
            get
            {
                return (ulong)DiscordId;
            }
            set
            {
                DiscordId = (long)value;
            }
        }
        public ICollection<Weights> Weights { get; set; }
    }
}
