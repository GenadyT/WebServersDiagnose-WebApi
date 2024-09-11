using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mime;
using WebServerModel;
using WebServersManager;
using WSM = WebServerModel;

namespace WatchesWebApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/[controller]")]
    public class WebServersManageController : ControllerBase
    {
        private readonly string connectionString;
        private const string SERVER_ERROR = "Server Error!";

        private readonly ILogger<WebServersManageController> _logger;

        public WebServersManageController(ILogger<WebServersManageController> logger)
        {
            _logger = logger;
            connectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["WebServers"];
        }

        [EnableCors("WelcomePolicy")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<WebServer> webServers;

            try
            {
                WSM.WebServerModel model = new WSM.WebServerModel(connectionString);
                webServers = model.GetAll();
            }
            catch (Exception)
            {
                return Problem(SERVER_ERROR);
            }

            return Ok(webServers);
        }

        [EnableCors("WelcomePolicy")]
        [HttpGet("GetHistory")]
        public async Task<IActionResult> GetHistory(int serverID)
        {
            List<MonitorHistory> serverHistory;

            try
            {
                WSM.WebServerModel model = new WSM.WebServerModel(connectionString);
                serverHistory = model.GetHistory(serverID);
            }
            catch (Exception)
            {
                return Problem(SERVER_ERROR);
            }

            return Ok(serverHistory);
        }

        [EnableCors("WelcomePolicy")]
        [HttpGet("GetInfo10")]
        public async Task<IActionResult> GetInfo10(int serverID)
        {
            WebServerInfo webServerInfo;

            try
            {
                WSM.WebServerModel model = new WSM.WebServerModel(connectionString);
                webServerInfo = model.GetInfo10(serverID);
            }
            catch (Exception)
            {
                return Problem(SERVER_ERROR);
            }
            
            return Ok(webServerInfo);
        }

        [EnableCors("WelcomePolicy")]
        [HttpPost]
        public async Task<IActionResult> Post(string name, string httpURL)
        {
            bool result;

            try
            {
                WSM.WebServerModel model = new WSM.WebServerModel(connectionString);
                result = model.Create(name, httpURL);
            }
            catch (Exception)
            {
                return Problem(SERVER_ERROR);
            }

            return Ok(result);
        }

        [EnableCors("WelcomePolicy")]
        [HttpPut]
        public async Task<IActionResult> Put(int id, string name, string httpURL)
        {
            bool result;

            try
            {
                WSM.WebServerModel model = new WSM.WebServerModel(connectionString);
                result = model.Update(id, name, httpURL);
            }
            catch (Exception)
            {
                return Problem(SERVER_ERROR);
            }

            return Ok(result);
        }

        [EnableCors("WelcomePolicy")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id, string name, string httpURL)
        {
            bool result;

            try
            {
                WSM.WebServerModel model = new WSM.WebServerModel(connectionString);
                result = model.Delete(id, name, httpURL);
            }
            catch (Exception)
            {
                return Problem(SERVER_ERROR);
            }

            return Ok(result);
        }
    }
}
