using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
 
namespace sheet_api{
    class QueryManager{
        public static List<UserCityView> GetUserCityViewList()
        {
            var userCityViews = new List<UserCityView>();
            var conn = DbUtils.GetDbConnection();
            conn.Open();
            try
            {
                userCityViews = QueryPlotTable(conn);
            }
            catch (Exception e){
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally{
                conn.Close();
                conn.Dispose();
            }

            return userCityViews;            
        }
 
        private static List<UserCityView> QueryPlotTable(SqlConnection conn ){
            const string sql = "Select user_name, city_id, city from plot_table";
            var cmd = new SqlCommand{
                Connection = conn,
                CommandText = sql
            };
            
            List<UserCityView> userCityViews = new List<UserCityView>();

            using (DbDataReader reader = cmd.ExecuteReader()){
                if (reader.HasRows){
                    while (reader.Read()){
                        var userNameIndex = reader.GetOrdinal("user_name");// 0
                        var userName =  Convert.ToString(reader.GetValue(userNameIndex));
                        var cityIdIndex = reader.GetOrdinal("city_id");// 1
                        var cityId = Convert.ToInt32(reader.GetValue(cityIdIndex));
                        var user = new User(userName, cityId);
                        var cityIndex = reader.GetOrdinal("city");// 3
                        var cityName =  Convert.ToString(reader.GetValue(cityIndex));
                        var city = new City(cityId, cityName);

                        var userCityView = new UserCityView(user, city);
                        userCityViews.Add(userCityView);
                    }
                }
            }
            return userCityViews;
        }
    }
}
