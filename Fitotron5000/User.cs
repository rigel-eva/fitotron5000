using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Fitotron5000{
    public class UserContext :DbContext{
        public DbSet<User> Users {get; set;}
        public DbSet<Weight> Weights {get;set;}
        
         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
             optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=fitotron_dev;Trusted_Connection=True;");
         }
    }
    public class User{
        public int Id{get; set;}
        public ulong discordID;
        public decimal goal;
        public decimal currentWeight;
        public List<Weight> weights;    
    }
    public class Weight{
        public ulong discordID;
        public int Id{get;set;}
        public User user;
        public decimal userWeight;
        public DateTime timeStamp;
    }
}
