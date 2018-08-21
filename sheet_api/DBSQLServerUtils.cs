using System.Data.SqlClient;
 
namespace sheet_api{
    static class DbsqlServerUtils{
        public static SqlConnection
            GetDbConnection(string datasource, string database, string username, string password){
            var connString = @"Data Source="+datasource+";Initial Catalog="
                                +database+";Persist Security Info=True;User ID="+username+";Password="+password;
            var conn = new SqlConnection(connString);
            return conn;
        }
    }
}
