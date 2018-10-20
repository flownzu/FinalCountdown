using System;
using System.Net.Http;

namespace FinalCountdown
{
    public class SpotifyTokenHttpClient : HttpClient
    {
        public SpotifyTokenHttpClient(string[] args)
        {
            BaseAddress = new Uri("http://flownzu.com/FinalCountdown/");
            Timeout = TimeSpan.FromSeconds(15);
        }
    }
}
