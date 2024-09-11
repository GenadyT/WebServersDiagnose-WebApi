using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServersManager
{
    public class WebServer
    {
		private int id;
		public int ID
		{
			get { return id; }
			//set { myVar = value; }
		}

        private string name;
        public string Name
        {
            get { return name; }
            //set { name = value; }
        }

        private string httpURL;
        public string HttpURL
        {
            get { return httpURL; }
            //set { httpURL = value; }
        }

        private int healthID;
        public int HealthID
        {
            get { return healthID; }
            //set { healthID = value; }
        }

        public WebServer(string name, string httpURL)
        {
            this.id = 0;
            this.name = name;
            this.httpURL = httpURL;
            this.healthID = 0;
        }

        public WebServer(int id, string name, string httpURL) : this(name, httpURL)
        { 
            this.id = id;
        }

        public WebServer(int id, string name, string httpURL, int healthID) : this(id, name, httpURL)
        {
            this.healthID = healthID;
        }

        public WebServer(DataRow dataRow)
        {
            this.id = Convert.ToInt32(dataRow["ID"]);
            this.name = dataRow["Name"].ToString();
            this.httpURL = dataRow["HttpURL"].ToString();
            this.healthID = Convert.ToInt32(dataRow["HealthID"]);
        }

        public void HealthUpdate(int healthID)
        {
            this.healthID = healthID;
        }
    }
}
