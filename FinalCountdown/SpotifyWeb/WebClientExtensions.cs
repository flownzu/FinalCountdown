using SpotifyAPI.Web;
using System.Net.Http;
using System.Reflection;

namespace FinalCountdown.SpotifyWeb
{
    public static class WebClientExtensions
    {
        public static void EmptyHeaders(this IClient spotifyWebClient)
        {
            var field = spotifyWebClient.GetType().GetField("_client", BindingFlags.Instance | BindingFlags.NonPublic);
            HttpClient httpClient = (HttpClient)field.GetValue(spotifyWebClient);
            httpClient.DefaultRequestHeaders.Clear();
        }
    }
}
