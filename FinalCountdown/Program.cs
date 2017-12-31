using SpotifyAPI.Local;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinalCountdown
{
    class Program
    {
        private static SpotifyLocalAPI _spotifyLocalAPI = new SpotifyLocalAPI();
        static async Task Main(string[] args)
        {
            Log("Waiting until 31.12. 23:54...");
            while (true)
            {
                var time = DateTime.Now;
                if (time.Day == 31 && time.Month == 12 && time.Hour == 23 && time.Minute == 54)
                {
                    await ConnectToSpotify();
                    break;
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine("Press any key to close...");
            Console.Read();
        }

        static async Task RunSpotify()
        {
            Log("Running Spotify...");
            SpotifyLocalAPI.RunSpotify();
            Log("Giving Spotify 10 seconds to start up...");
            await Task.Delay(10000);
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                Log("Starting WebHelper...");
                SpotifyLocalAPI.RunSpotifyWebHelper();
                await Task.Delay(1000);
            }
        }

        static async Task ConnectToSpotify()
        {
            if (!SpotifyLocalAPI.IsSpotifyRunning()) await RunSpotify();
            Log("Connecting to local Spotify Client...");
            if (_spotifyLocalAPI.Connect())
            {
                Log("Connected to local Spotify Client.");
                Log("Waiting for the right time...", false);
                int i = 0;
                while (true)
                {
                    i++;
                    var time = DateTime.Now;
                    time = time.AddTicks(-(time.Ticks % TimeSpan.TicksPerSecond));
                    if (time == new DateTime(2017, 12, 31, 23, 54, 49))
                    {
                        Console.WriteLine();
                        await FinalCountdown();
                        return;
                    }
                    else
                    {
                        if (i == 10)
                        {
                            i = 0;
                            Console.Write(".");
                        }
                        Thread.Sleep(100);
                    }
                }
            }
            else Log("Could not connect to local Spotify Client.");
        }

        static async Task FinalCountdown()
        {
            Log("Executing The Final Countdown!");
            await _spotifyLocalAPI.PlayURL("spotify:track:498SE5YbgSuUgXKKe7BaA5", "spotify:user:spoing01:playlist:3EjJLY2xPFSjN1JHjl8GqL");
        }

        static void Log(string msg, bool newLine = true) => Console.Write("[" + DateTime.Now + "] " + msg + (newLine ? Environment.NewLine : ""));
    }
}