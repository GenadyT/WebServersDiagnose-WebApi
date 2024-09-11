using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServerModel
{
    public class MonitorHistory
    {
        private int serverID;
        public int ServerID
        {
            get { return serverID; }
            //set { serverID = value; }
        }

        private DateTime monitorTime;
        public DateTime MonitorTime
        {
            get { return monitorTime; }
            //set { monitorTime = value; }
        }

        private int httpResponseCode;
        public int HttpResponseCode
        {
            get { return httpResponseCode; }
            //set { httpResponseCode = value; }
        }

        private int httpResponseLatency;
        public int HttpResponseLatency
        {
            get { return httpResponseLatency; }
            //set { httpResponseLatency = value; }
        }


        public MonitorHistory(
            int serverID,
            DateTime monitorTime,
            int httpResponseCode,
            int httpResponseLatency
            ) 
        { 
            this.serverID = serverID;
            this.monitorTime = monitorTime;
            this.httpResponseCode = httpResponseCode;
            this.httpResponseLatency = httpResponseLatency;
        }

        public MonitorHistory(DataRow dataRow)
        {
            this.serverID = Convert.ToInt32(dataRow["serverID"]);
            this.monitorTime = Convert.ToDateTime(dataRow["MonitorTime"]);
            this.httpResponseCode = Convert.ToInt32(dataRow["HttpResponseCode"]);
            this.httpResponseLatency = Convert.ToInt32(dataRow["HttpResponseLatency"]);
        }
    }
}
