using System;
using System.Data.SqlClient;
using System.Data.Common;
 
namespace sheet_api

{
    class QueryManager
    {
        private static void madeSQL()
        {
            string[] result = new string[]{};
            SqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            try
            {
                result = QueryPlotTable(conn);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }       
            Console.Read();
        }
 
        private static string[] QueryPlotTable(SqlConnection conn )
        {
            string[] result = new string[]{};
            
            string sql = "Select user_name, city_id, id, city from plot_table"; 
 
            SqlCommand cmd = new SqlCommand();
 
            cmd.Connection = conn;
            cmd.CommandText = sql; 
 
             
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                     
                    while (reader.Read())
                    {
                        int i = 0;
                        int userNameIndex = reader.GetOrdinal("user_name");
                        string userName =  Convert.ToString(reader.GetValue(0));
                         
                        int cityIdIndex = reader.GetOrdinal("city_id");// 2
                        int cityId = Convert.ToInt32(reader.GetValue(cityIdIndex));
                        
                        
                        int IdIndex = reader.GetOrdinal("id");// 2
                        int IdInt = Convert.ToInt32(reader.GetValue(IdIndex));
 
                        
                        int cityIndex = reader.GetOrdinal("city");
                        string city =  Convert.ToString(reader.GetValue(cityIndex));
                        
                        
                        result[i] = new string []{{"userName" , "cityId", "IdInt", "city"}}
                        ;


                    }
                }
            }

            return result;

        }
    }
 
}
