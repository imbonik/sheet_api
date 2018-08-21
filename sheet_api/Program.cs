using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace sheet_api{
    static class Program{
        private static string ClientSecret = "client_secret.json";
        private static readonly string[] ScopesSheets = {SheetsService.Scope.Spreadsheets};
        private static readonly string AppName = "nfTest";
        private static string SummaryViewTableId = "1ELamFMabporvwhCmGJogy4XGh7O5QXtTGE2mo-bRwsk";
        private static string UserTableId = "wabqX0zA-Tv_3njic7T4hycXOSCR4Op2i5ugknLyMY";
        private static string CityTableId = "1n0DshVnxFCddC5suZgkTHG-E6lzc-5IwwkAyaVtuPbc";
        
        private static string[,] _data = new string[,]{
            {"user_name", "city_id", "id", "city"},
            {"test", "1", "1", "test_Citiжесть"},
            {"most", "2", "2", "ost"}

        };

        public static void Main(string[] args){
           Console.WriteLine("Get credentials");
           var credential = GetSheetCredential();
            
           Console.WriteLine("Get Drive service");
           var service = GetService(credential);
        
           
           
            
           Console.WriteLine("Get UserCityView");
           var userCityViewList = QueryManager.GetUserCityViewList();
           
           Console.WriteLine("Fill data");
           FillSpreadsheet(service, SummaryViewTableId, userCityViewList);

           Console.WriteLine("Done");
           Console.ReadLine();
        }

        private static UserCredential GetSheetCredential(){
            using (var stream = new FileStream(ClientSecret, FileMode.Open, FileAccess.Read))
            {
                var credPath = Path.Combine(Directory.GetCurrentDirectory(), "sheetCreds.json");

                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    ScopesSheets,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
        }

        private static SheetsService GetService(UserCredential credential){
            return new SheetsService (new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName
            });
        }
        
        private static void FillSpreadsheet(SheetsService service, string spreadsheetId, List<UserCityView> userCityViews){
            var requests = new List<Request>();
            for (var i = 1; i < userCityViews.Count; i++){
                List<CellData> values = new List<CellData>();
                for (var j = 0; j < userCityViews.Count+1; j++)
                {
                    var userCityView = userCityViews[i];
                    values.Add(new CellData{
                        UserEnteredValue = new ExtendedValue{
                            StringValue = userCityView.UserName+","+ userCityView.CityId+","+ userCityView.CityName
                        }
                    });
                }
                requests.Add(new Request {UpdateCells = new UpdateCellsRequest{
                            Start = new GridCoordinate{
                                SheetId = 0,
                                RowIndex = i,
                                ColumnIndex = 0
                            },
                            Rows = new List<RowData> { new RowData {Values = values}},
                            Fields = "userEnteredValue"
                        }
                    }
                );
            }
            
        BatchUpdateSpreadsheetRequest busr = new BatchUpdateSpreadsheetRequest{
                Requests = requests
            };
            service.Spreadsheets.BatchUpdate(busr, spreadsheetId).Execute();
        }

        private static string GetFirstCell(SheetsService service, string range, string spreadsheetId){
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();
            string result = null;
            foreach (var value in response.Values){
                result += " " + value[0];
            }
            return result;
        }
    }
}
