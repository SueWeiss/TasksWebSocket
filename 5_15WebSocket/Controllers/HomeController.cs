using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _5_15WebSocket.Models;
using Microsoft.AspNetCore.Authorization;
using _5_18WebSocket.Data;
using Microsoft.Extensions.Configuration;

namespace _5_15WebSocket.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private Manager _mgr;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _mgr = new Manager(_connectionString);
        }

        [Authorize]
        public IActionResult Index()
        {
            
            return View(_mgr.GetAllIncompletedTasks());
        }
        


    }
}
