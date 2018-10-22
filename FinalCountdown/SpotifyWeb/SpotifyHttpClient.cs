using System;
using System.Net.Http;

namespace FinalCountdown.SpotifyWeb
{
    public class SpotifyTokenHttpClient : HttpClient
    {
        public SpotifyTokenHttpClient()
        {
            BaseAddress = new Uri("http://flownzu.com/FinalCountdown/");
            Timeout = TimeSpan.FromSeconds(15);
        }
    }
}
