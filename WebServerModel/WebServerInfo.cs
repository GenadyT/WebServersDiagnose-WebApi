using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServersManager;

namespace WebServerModel
{
    public class WebServerInfo
    {
        private WebServer webServer;
		public WebServer WebServer
        {
			get { return webServer; }
			//set { webServer = value; }
		}

        private List<MonitorHistory> monitorHistory;
        public List<MonitorHistory> MonitorHistory
        {
            get { return monitorHistory; }
            //set { monitorHistory = value; }
        }


        public WebServerInfo(WebServer webServer, List<MonitorHistory> monitorHistory) 
        { 
            this.webServer = webServer;
            this.monitorHistory = monitorHistory;
        }
    }
}
