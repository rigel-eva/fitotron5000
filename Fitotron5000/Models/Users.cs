using System;
using System.Collections.Generic;

namespace Fitotron5000.Models
{
    public partial class Users
    {
        public long DiscordId { get; set; }
        public decimal? Goal { get; set; }
        public decimal? CurrentWeight { get; set; }
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
    }

}
