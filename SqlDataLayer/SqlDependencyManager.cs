using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataLayer
{
    public class SqlDependencyManager
    {
        private string tableName;
        private string connectionString;
        private Action<DataRowCollection> notifyHandler;

        public SqlDependencyManager(string tableName, string connectionString, Action<DataRowCollection> notifyHandler) {
            this.tableName = tableName;
            this.connectionString = connectionString;
            this.notifyHandler = notifyHandler;

            dependencyInit();
        }

        private void dependencyInit()
        {
            // Assume connection is an open SqlConnection.

            // Create a new SqlCommand object.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(
                    $"SELECT * FROM dbo.{this.tableName}", connection))
                {
                    // Create a dependency and associate it with the SqlCommand.
                    SqlDependency dependency = new SqlDependency(command);
                    // Maintain the reference in a class member.

                    // Subscribe to the SqlDependency event.
                    dependency.OnChange += new
                       OnChangeEventHandler(onDependencyChange);

                    // Execute the command.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Process the DataReader.
                    }
                }
            }
            
        }

        // Handler method
        private void onDependencyChange(object sender,
           SqlNotificationEventArgs e)
        {
            // Handle the event (for example, invalidate this cache entry).
            notifyHandler(null);
        }

        public void Start()
        {
            // Create a dependency connection.
            SqlDependency.Start(connectionString);
        }

        public void Stop()
        {
            // Release the dependency.
            SqlDependency.Stop(connectionString);
        }
    }
}
