using SpotifyAPI.Web;
using System.Net.Http;
using System.Reflection;

namespace FinalCountdown.SpotifyWeb
{
    public static class WebClientExtensions
    {
        // just a hack until https://github.com/JohnnyCrazy/SpotifyAPI-NET/pull/302 hits release
        public static void EmptyHeaders(this IClient spotifyWebClient)
        {
            // retrieves the internal HttpClient from the SpotifyWebClient
            var field = spotifyWebClient.GetType().GetField("_client", BindingFlags.Instance | BindingFlags.NonPublic);
            HttpClient httpClient = (HttpClient)field.GetValue(spotifyWebClient);
            // Clear default request headers :)
            httpClient.DefaultRequestHeaders.Clear();
        }
    }
}
