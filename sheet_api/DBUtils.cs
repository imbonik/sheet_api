using System.Data.SqlClient;
 
 
namespace sheet_api{
    static class DbUtils{
        public static SqlConnection GetDbConnection(){
            const string datasource = @"localhost";
            const string database = "google_docs";
            const string username = "seesharp";
            const string password = "root";
 
            return DbsqlServerUtils.GetDbConnection(datasource,  database, username, password);
        }
    }
}