using System.Net.Http.Headers;

namespace OrderTrak.Client.Services.API
{
    public class TokenHttpClientHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public TokenHttpClientHandler(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
