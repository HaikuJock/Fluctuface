using System;
using System.Net.Http;

namespace Haiku.Fluctuface.Client
{
    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
