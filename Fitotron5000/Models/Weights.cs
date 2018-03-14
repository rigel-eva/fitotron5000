using System;
using System.Collections.Generic;
using System.Linq;
namespace Fitotron5000.Models
{
    public partial class Weights
    {
        private int? _userId;

        public int Id { get; set; }
        public int? UserId { get => _userId; set => _userId = value; }
        public double? UserWeight { get; set; }
        public DateTime? TimeStamp { get; set; }

        public Users User { get; set; }
    }
}
