namespace sheet_api
{
    public class City
    {
        private int cityId;
        private string cityName;


        public City(int cityId, string cityName)
        {
            this.cityId = cityId;
            this.cityName = cityName;
        }

        public int CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }

        public string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }
    }
}