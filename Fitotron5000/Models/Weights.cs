using System;
using System.Collections.Generic;

namespace Fitotron5000.Models
{
    public partial class Weights
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public double? UserWeight { get; set; }
        public DateTime? TimeStamp { get; set; }

        public Users User { get; set; }
    }
}
