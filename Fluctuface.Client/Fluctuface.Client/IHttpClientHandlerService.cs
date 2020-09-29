using System;
using System.Net.Http;

namespace Fluctuface.Client
{
    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
