using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace Fitotron5000{
    public class UserContext :DbContext{
        public DbSet<User> Users {get; set;}
        public DbSet<Weight> Weights {get;set;}
        private string connectionString;
        public UserContext(string connectionString):base(){
            this.connectionString=connectionString;
        }
         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
             optionsBuilder.UseSqlServer(connectionString);
         }
         protected override void OnModelCreating(ModelBuilder modelBuilder){
             modelBuilder.Entity<User>(entity=>{
                 entity.Property(e=>e._discordID).IsRequired();
                 entity.Ignore(e=>e.discordID);
             });
             modelBuilder.Entity<Weight>(entity=>{
                 entity.HasOne(u=>u.user).WithMany(w=>w.weights).HasForeignKey(u=>u.Id);
             });
         }
    }
    public class User{
        public int Id{get; set;}
        public long _discordID{get; set;}
        public ulong discordID{
            get{
                return unchecked((ulong)(_discordID-long.MinValue));
            } 
            set{
                _discordID=unchecked((long)value+long.MinValue);
            }}
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
