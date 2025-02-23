namespace OrderTrak.Client.Services.API
{
    public partial class Client : IClient
    {
        public HttpClient HttpClient
        {
            get
            {
                return _httpClient;
            }
        }


    }
}
