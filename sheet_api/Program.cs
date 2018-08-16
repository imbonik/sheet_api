using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;




namespace sheet_api
{
    class Program
    {
        private static string ClientSecret = "client_secret.json";
        private static readonly string[] ScopesSheets = {SheetsService.Scope.Spreadsheets};
        private static readonly string AppName = "nfTest";
        private static string SpreadsheetId = "1ELamFMabporvwhCmGJogy4XGh7O5QXtTGE2mo-bRwsk";
        //wabqX0zA-Tv_3njic7T4hycXOSCR4Op2i5ugknLyMY users
        //1n0DshVnxFCddC5suZgkTHG-E6lzc-5IwwkAyaVtuPbc  cities
        


        private static string[,] Data = new string[,]
        {
            {"user_name", "city_id", "id", "city"},
            {"test", "1", "1", "test_Citiжесть"},
            {"most", "2", "2", "ost"}

        };

        public static void Main(string[] args)
       
        {
           Console.WriteLine("Get credentials");
           var credential = GetSheetCredential();
            
           Console.WriteLine("Get Drive service");
           var service = GetService(credential);

           Console.WriteLine("Fill data");
           FillSpreadsheet(service, SpreadsheetId, Data);

           Console.WriteLine("Done");
           Console.ReadLine();
        }

        private static UserCredential GetSheetCredential()
        {
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

        private static SheetsService GetService(UserCredential credential)
        {
            return new SheetsService (new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName
            });
        }
        
        private static void FillSpreadsheet(SheetsService service, string spreadsheetId, string[,] data)
        {
            List<Request> requests = new List<Request>();

            //create request by an row
            for (int i = 0; i < data.GetLength(0); i++)
            {

                //get values of row
                List<CellData> values = new List<CellData>();

                for (int j = 0; j < data.GetLength(1); j++)
                {
                    values.Add(new CellData
                    {
                        UserEnteredValue = new ExtendedValue
                        {
                            StringValue = data[i, j]
                        }
                    });
                }


                //create request with
                requests.Add(
                    new Request {
                        UpdateCells = new UpdateCellsRequest
                        {
                            Start = new GridCoordinate
                            {
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
            
        BatchUpdateSpreadsheetRequest busr = new BatchUpdateSpreadsheetRequest
            {
                Requests = requests
            };
            service.Spreadsheets.BatchUpdate(busr, spreadsheetId).Execute();

        }

        private static string GetFirstCell(SheetsService service, string range, string spreadsheetId)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();

            string result = null;

            foreach (var value in response.Values)
            {
                result += " " + value[0];
            }
            
            return result;
        }
    }
    
}