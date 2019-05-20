using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace _5_18WebSocket.Data
{
    public class Manager
    {
        string _connectionString { get; set; }
        public Manager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddTask(ToDo item)
        {
            using (var context = new ToDoContext(_connectionString))
            {
                context.Tasks.Add(item);
                context.SaveChanges();
            }
        }

        public ToDo TaskAssigned(ToDo td)
        {
            using (var context = new ToDoContext(_connectionString))
            {
                ToDo item = context.Tasks.FirstOrDefault(t => t.Id == td.Id);
                if (item == null)
                { return td; }
                item.Status = ToDoStatus.InProgress;
                item.UserNameAssigned = td.UserNameAssigned;
                context.SaveChanges();
                // context.Database.ExecuteSqlCommand(
                //   "UPDATE TasksWebSocket set status= @status, UserNameAssigned=@user where id = @id",
                //    new SqlParameter("@id", td.Id), new SqlParameter("@user", td.UserNameAssigned), new SqlParameter("@status", td.Status));
                 
                return item;
            }
        }
        public void TaskCompleted(int taskID)
        {
            using (var context = new ToDoContext(_connectionString))
            {
                ToDo item = context.Tasks.FirstOrDefault(t => t.Id == taskID);
                if (item == null)
                { return; }
                item.Status = ToDoStatus.Done;
                context.SaveChanges();
                
                //   context.Database.ExecuteSqlCommand(
                //       "UPDATE TasksWebSocket set status= 'InProgress', UserName'@user' where id = @taskid",
                //         new SqlParameter("@id", taskID), new SqlParameter("@user", userName));
            }
        }


        public ToDo GetTaskById(int Id)
        {
            using (var context = new ToDoContext(_connectionString))
            {
                return context.Tasks.Include(q => q.UserNameAssigned).FirstOrDefault(q => q.Id == Id);
            }
        }

        public IEnumerable<ToDo> GetAllIncompletedTasks()
        {
            using (var context = new ToDoContext(_connectionString))
            {
                return context.Tasks.Where(t => t.Status == ToDoStatus.Raw).ToList();
            }
        }

        //LogIns
        public void AddUser(User user, string password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = hash;
            using (var context = new ToDoContext(_connectionString))
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null; //incorrect email
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
            {
                return null;
            }

            return user;
        }

        public User GetByEmail(string email)
        {
            using (var context = new ToDoContext(_connectionString))
            {
                return context.Users.ToList().Where(e => e.Email == email).First();
            }
        }
    }
}

