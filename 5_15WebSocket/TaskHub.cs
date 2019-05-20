using _5_18WebSocket.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


    public class TaskHub: Hub
{
    private string _connectionString;
    private Manager _mgr;

    public TaskHub(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConStr");
        _mgr = new Manager(_connectionString);
    }

    public void AddTask(string task)
    { 
        var ToDo= new ToDo
        {
            Task = task,
            Status = ToDoStatus.Raw
        };
        _mgr.AddTask(ToDo);
        Clients.All.SendAsync("NewTask", ToDo);
    }

    public void TaskAssigned(int taskId)
    {
        string user = Context.User.Identity.Name;
        ToDo c = new ToDo
        {
            Id = taskId,
            UserNameAssigned = Context.User.Identity.Name,
            Status = ToDoStatus.InProgress,
        };
       
       ToDo udpated= _mgr.TaskAssigned(c);
        Clients.Others.SendAsync("Assigned", udpated);
        Clients.Caller.SendAsync("IAssigned", udpated);
    }

    public void AcceptTask(ToDo task)
    {
       ToDo t = _mgr.TaskAssigned(task);
       Clients.Others.SendAsync("AcceptedTask", t);
     Clients.Caller.SendAsync("IAcceptedTask", t);
    }

    public void TaskCompleted(int taskId)
    {
        _mgr.TaskCompleted(taskId, Context.User.Identity.Name);
        Clients.All.SendAsync("TaskCompleted",taskId);
        
    }

    public void NewUser()
    {
      
    }
}

