using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServersManager;
using SDL = SqlDataLayer;

namespace WebServerModel
{
    public class WebServerModel
    {
        //private string connectionString;
        private SDL.SqlDataLayer sqlDataLayer;

        public WebServerModel(string connectionString)
        {
            //this.connectionString = connectionString;
            this.sqlDataLayer = new SDL.SqlDataLayer(connectionString);
        }

        public List<WebServer> GetAll()
        {
            List<WebServer> webServers = new List<WebServer>();

            string cmdText = "SELECT * FROM tblWebServers";
            DataRowCollection dataRows = sqlDataLayer.SelectTableRows(cmdText);
            if (dataRows.Count > 0)
            {
                foreach (DataRow row in dataRows)
                {
                    webServers.Add(new WebServer(row));
                }
            }
            
            return webServers;
        }

        public List<MonitorHistory> GetHistory(int serverID)
        {
            List<MonitorHistory> webServers = new List<MonitorHistory>();

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            SqlParameter sqlParameter = new SqlParameter("@ServerID", serverID);
            sqlParameters.Add(sqlParameter);

            DataTable dt = sqlDataLayer.SelectDataTable("spGetServerHistory", sqlParameters);
            DataRowCollection dataRows = dt.Rows;
            if (dataRows.Count > 0)
            {
                foreach (DataRow row in dataRows)
                {
                    webServers.Add(new MonitorHistory(row));
                }
            }

            return webServers;
        }        

        public WebServerInfo GetInfo10(int serverID)
        {
            WebServer webServer;
            List<MonitorHistory> monitorHistory = new List<MonitorHistory>();
            WebServerInfo webServerInfo;            

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            SqlParameter sqlParameter = new SqlParameter("@ServerID", serverID);
            sqlParameters.Add(sqlParameter);
            
            DataTableCollection dataTables = sqlDataLayer.SelectDataTables("spGetServer", sqlParameters);

            DataTable tableWebServers = dataTables[0];
            DataTable tableMonitorHistory = dataTables[1];

            if (tableWebServers.Rows.Count > 0)
            {
                DataRow dataRow = tableWebServers.Rows[0];
                webServer = new WebServer(dataRow);
            }
            else
            {
                return null;
            }

            foreach (DataRow row in tableMonitorHistory.Rows)
            {
                monitorHistory.Add(new MonitorHistory(row));
            }

            webServerInfo = new WebServerInfo(webServer, monitorHistory);
            return webServerInfo;
        }

        public bool Create(string name, string httpURL)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@Name", name));
            sqlParameters.Add(new SqlParameter("@HttpURL", httpURL));

            int result = sqlDataLayer.RunScalarSP("spInsertServer", sqlParameters);
            return result > 0;
        }

        public bool Update(int id, string name, string httpURL)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@ID", id));
            sqlParameters.Add(new SqlParameter("@Name", name));
            sqlParameters.Add(new SqlParameter("@HttpURL", httpURL));

            int result = sqlDataLayer.RunScalarSP("spUpdateServer", sqlParameters);
            return result > 0;
        }

        public bool Delete(int id, string name, string httpURL)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@ID", id));
            sqlParameters.Add(new SqlParameter("@Name", name));
            sqlParameters.Add(new SqlParameter("@HttpURL", httpURL));

            int result = sqlDataLayer.RunScalarSP("spDeleteServer", sqlParameters);
            return result > 0;
        }
        
    }
}
