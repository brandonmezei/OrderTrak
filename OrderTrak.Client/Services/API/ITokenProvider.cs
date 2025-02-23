namespace OrderTrak.Client.Services.API
{
    public interface ITokenProvider
    {
        Task<string> GetTokenAsync();
    }
}
