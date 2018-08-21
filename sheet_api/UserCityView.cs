namespace sheet_api
{
    public class UserCityView
    {
        private string userName;
        private int cityId;
        private string cityName;

        public UserCityView(User user, City city)
        {
            userName = user.UserName;
            cityId = user.CityId;
            cityName = city.CityName;
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
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