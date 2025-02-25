using System.Net.Http.Headers;

namespace OrderTrak.Client.Services.API
{
    public class TokenHttpClientHandler(ITokenProvider tokenProvider) : DelegatingHandler
    {
        private readonly ITokenProvider TokenProvider = tokenProvider;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await TokenProvider.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
