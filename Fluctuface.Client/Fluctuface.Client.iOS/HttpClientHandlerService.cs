using System.Net.Http;
using Xamarin.Forms;

[assembly: Dependency(typeof(Haiku.Fluctuface.Client.iOS.HttpClientHandlerService))]
namespace Haiku.Fluctuface.Client.iOS
{
    public class HttpClientHandlerService : IHttpClientHandlerService
    {
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}
