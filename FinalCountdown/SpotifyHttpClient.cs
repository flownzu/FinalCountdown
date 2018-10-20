using System;
using System.Net.Http;
using System.Text;

namespace FinalCountdown
{
    public class SpotifyTokenHttpClient : HttpClient
    {
        public SpotifyTokenHttpClient(string[] args)
        {
            BaseAddress = new Uri("https://accounts.spotify.com/api/token");
            DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(args[0] + ":" + args[1])));
            Timeout = TimeSpan.FromSeconds(15);
        }
    }
}
