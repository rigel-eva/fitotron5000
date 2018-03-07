using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Fitotron5000{
    public class UserContext :DbContext{
        public DbSet<User> Users {get; set;}
        public DbSet<Weight> Weights {get;set;}
         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
             optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
         }
    }
    public class User{
        public long discordID;
        public decimal goal;
        public List<Weight> weights;    
    }
    public class Weight{
        public int discordID;
        public int weightID;
        public User user;
        public decimal userWeight;
        public DateTime timeStamp;
        public Weight(int discordID, int weightID, User user, decimal userWeight){
            discordID=this.discordID;
            weightID=this.weightID;
            user=this.user;
            this.userWeight=userWeight;
            timeStamp=DateTime.Now;
        }
    }
}
