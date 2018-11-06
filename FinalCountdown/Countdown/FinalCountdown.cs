using FinalCountdown.SpotifyWeb;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FinalCountdown.Countdown
{
    public class FinalCountdown
    {
        private static CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private static SpotifyWebHandler _webHandler;
        private static bool _isRunning { get; set; }
#if DEBUG
        public static int Day;
        public static int Hour;
        public static int Minute;
        public static int Second;
#else
        private static readonly int Day = 365;
        private static readonly int Hour = 23;
        private static readonly int Minute = 54;
        private static readonly int Second = 49;
#endif

        public event EventHandler CountdownStarted;
        public event EventHandler CountdownStopped;
        public event EventHandler TheFinalCountdown;

        public bool IsRunning => _isRunning;
        public string ContextUri { get; set; } = "";
        public int? Offset { get; set; }

        public FinalCountdown(SpotifyWebHandler spotifyWebHandler)
        {
            _webHandler = spotifyWebHandler;
        }

        public void SetContext(string uri, int offset)
        {
            ContextUri = uri;
            Offset = offset;
        }

        public void ClearContext()
        {
            ContextUri = "";
            Offset = null;
        }

        public async void Start()
        {
            if (_isRunning) return;
            else
            {
                _isRunning = true;
                OnCountdownStarted();
                // in debug mode the song should be played 10secs from when you start the countdown (basically for testing purposes)
#if DEBUG
                var time = DateTime.Now.Add(TimeSpan.FromSeconds(10));
                Day = time.DayOfYear;
                Hour = time.Hour;
                Minute = time.Minute;
                Second = time.Second;
#endif
                while (!_tokenSource.Token.IsCancellationRequested)
                {
                    var currentTime = DateTime.Now;
                    if (currentTime.DayOfYear >= Day && currentTime.Hour == Hour && currentTime.Minute == Minute && currentTime.Second == Second)
                    {
                        OnTheFinalCountdown();
                        // If the user chose a playlist to play the song, the song is played from the playlist
                        if (!string.IsNullOrEmpty(ContextUri))
                        {
                            await _webHandler.PlayTrack(contextUri: ContextUri, offset: Offset);
                        }
                        // otherwise the song is played normally (not recommended due to it repeating after it is done!)
                        else await _webHandler.PlayTrack(trackUris: new List<string>() { "spotify:track:498SE5YbgSuUgXKKe7BaA5" });
                        break;
                    }
                    Thread.Sleep(200);
                }
            }
            _isRunning = false;
            OnCountdownStopped();
        }

        public async Task Cancel()
        {
            _tokenSource.Cancel();
            await Task.Delay(300);
            _tokenSource = new CancellationTokenSource();
        }

        protected virtual void OnCountdownStarted()
        {
            CountdownStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCountdownStopped()
        {
            CountdownStopped?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnTheFinalCountdown()
        {
            TheFinalCountdown?.Invoke(this, EventArgs.Empty);
        }
    }
}
