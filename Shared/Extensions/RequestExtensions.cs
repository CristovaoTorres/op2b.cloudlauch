using System;
using Microsoft.AspNetCore.Http;

namespace Platform.Shared.Extensions
{
    public static class RequestExtensions
    {
        /// <summary>
        /// Idenpendente de proxy-reverso ou qualquer outra modificacao feita no request por CDN's como a do Cloudflare, 
        /// retorna o IpAddress do usuario visitante/chamador.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetIpAddress(this Microsoft.AspNetCore.Http.ConnectionInfo request)
        {
            string ipAddress = request.RemoteIpAddress.ToString();

            switch (request.RemoteIpAddress.AddressFamily)
            {
                case System.Net.Sockets.AddressFamily.InterNetwork:
                    if (!ipAddress.Equals("::1", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //usa a classe Uri para remover a porta, do ip address, em algumas situacoes a porta do provedor vem junto.
                        Uri uri = new Uri("http://" + ipAddress);

                        Console.WriteLine("http://" + ipAddress);
                        return uri.Host.ToString();
                    }
                    return "127.0.0.1";
                case System.Net.Sockets.AddressFamily.InterNetworkV6:
                    return ipAddress;
                default:
                    return "None";
            }
        }
    }
}
