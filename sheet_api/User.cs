namespace sheet_api
{
    public class User
    {
        private string userName;
        private int cityId;

        public User(string userName, int cityId)
        {
            this.userName = userName;
            this.cityId = cityId;
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
    }
}