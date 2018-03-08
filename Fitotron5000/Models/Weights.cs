using System;
using System.Collections.Generic;

namespace Fitotron5000.Models
{
    public partial class Weights
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal? UserWeight { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
