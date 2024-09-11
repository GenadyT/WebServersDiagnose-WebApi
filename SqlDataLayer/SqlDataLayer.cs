using System.Data;
using System.Data.SqlClient;

namespace SqlDataLayer
{
    public class SqlDataLayer
    {
        private readonly string connectionString;

        private const string SQL_EXCEPTION = "Data Layer Exception!";

        public SqlDataLayer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int RunScalarSP(string storedProcedureName, List<SqlParameter> lstSqlParams)
        {
            SqlParameter[] sqlParams = lstSqlParams.ToArray<SqlParameter>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddRange(sqlParams);

                        conn.Open();
                        string strResult = cmd.ExecuteScalar().ToString();
                        conn.Close();

                        int intResult;
                        if (Int32.TryParse(strResult, out intResult))
                        {
                            return intResult;
                        }
                        else
                        {
                            throw new Exception(strResult);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }
            catch (Exception ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }
        }

        public DataTable SelectDataTable(string storedProcedureName, List<SqlParameter> lstSqlParams)
        {
            DataTable dt = new DataTable();
            SqlParameter[] sqlParams = lstSqlParams.ToArray<SqlParameter>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddRange(sqlParams);

                            conn.Open();
                            da.Fill(dt);
                            conn.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }
            catch (Exception ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }

            return dt;
        }

        public DataTableCollection SelectDataTables(string storedProcedureName, List<SqlParameter> lstSqlParams)
        {
            DataSet ds = new DataSet();
            SqlParameter[] sqlParams = lstSqlParams.ToArray<SqlParameter>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddRange(sqlParams);

                            conn.Open();
                            da.Fill(ds);
                            conn.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }
            catch (Exception ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }

            return ds.Tables;
        }

        public DataTable SelectDataTable(string cmdText)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                    {
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.Text;

                            conn.Open();
                            da.Fill(dt);
                            conn.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }
            catch (Exception ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }

            return dt;
        }

        public DataTableCollection SelectDataTableCollection(string storedProcedureName, List<SqlParameter> lstSqlParams)
        {
            DataTableCollection dtc;
            SqlParameter[] sqlParams = lstSqlParams.ToArray<SqlParameter>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddRange(sqlParams);

                            DataSet ds = new DataSet();
                            conn.Open();
                            da.Fill(ds);
                            conn.Close();

                            dtc = ds.Tables;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }
            catch (Exception ex)
            {
                //Logger.Write(ex);
                throw new Exception(SQL_EXCEPTION);
            }

            return dtc;
        }

        public DataRowCollection SelectTableRows(string cmdText)
        {
            return SelectDataTable(cmdText).Rows;
        }

        public static string IntArrayToCommaString(int?[] intArray)
        {
            int?[] arrIntArray = DictionaryNulling(intArray);
            return arrIntArray != null ? string.Join(",", arrIntArray) : null;
        }

        public static int?[] ExtractIDsArray(DataTable dataTable)
        {
            int arrayLength = dataTable.Rows.Count;
            int?[] array = new int?[arrayLength];

            for (int i = 0; i < arrayLength; i++)
            {
                array[i] = Convert.ToInt32(dataTable.Rows[i]["ID"]);
            }

            return array;
        }

        private static int?[] DictionaryNulling(int?[] array)
        {
            if ((array == null) || (array.Length == 0)) return null;
            if ((array.Length == 1) && (array[0] == 0)) return null;

            return array;
        }

        private static decimal? DecimalNulling(decimal? dVal)
        {
            if ((dVal == null) || (dVal == 0.0m))
            {
                return null;
            }
            else
            {
                return dVal;
            }

        }
    }
}
