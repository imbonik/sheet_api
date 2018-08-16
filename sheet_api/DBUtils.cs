using System.Data.SqlClient;
 
 
namespace sheet_api
{
    class DBUtils
    {
        public static SqlConnection GetDBConnection()
        {
            string datasource = @"localhost";
            
            string database = "google_docs";
            string username = "seesharp";
            string password = "root";
 
            return DBSQLServerUtils.GetDBConnection(datasource,  database, username, password);
        }
    }
 
}