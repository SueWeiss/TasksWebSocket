using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace _5_18WebSocket.Data
{
    public class ToDo
    {
        public string UserNameAssigned { get; set; }
        public string Task { get; set; }
        public ToDoStatus Status { get; set; }
        public int Id { get; set; }
    }
    public class User
    {
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
    }
    public enum ToDoStatus
    {
        Done,
        InProgress,
        Raw
    }
    public class ToDoContext : DbContext
    {
        private string _connectionString;
        public ToDoContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer(_connectionString);
        }
    }
    public class ToDoContextFactory : IDesignTimeDbContextFactory<ToDoContext>
    {
        public ToDoContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}5_15WebSocket"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new ToDoContext(config.GetConnectionString("ConStr"));
        }
    }
}
