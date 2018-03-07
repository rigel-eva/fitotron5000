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
         protected override void OnModelCreating(ModelBuilder modelBuilder){
             modelBuilder.Entity<User>(entity=>{
                 entity.Property(e=>e.discordID).IsRequired();
             });
             modelBuilder.Entity<Weight>(entity=>{
                 entity.HasOne(u=>u.user).WithMany(w=>w.weights).HasForeignKey(u=>u.Id);
             });
         }
    }
    public class User{
        public int Id{get; set;}
        public ulong discordID{get; set;}
        public decimal goal{get; set;}
        public decimal currentWeight{get; set;}
        public List<Weight> weights{get; set;}
    }
    public class Weight{
        public int userID{get; set;}
        public int Id{get;set;}
        public User user{get; set;}
        public decimal userWeight{get; set;}
        public DateTime timeStamp{get; set;}
    }
}
