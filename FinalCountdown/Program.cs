using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FinalCountdown
{
    class Program
    {
        private static SpotifyWebAPI _spotifyWebAPI;
        private static AuthorizationCode _auth;
        private static Token _token;
        private static string _refreshToken;

        static async Task Main(string[] args)
        {
            AuthorizationCodeAuth authorizationCodeAuth = new AuthorizationCodeAuth("http://flownzu.com/FinalCountdown/auth", "http://localhost:8800", Scope.Streaming, "123");
            authorizationCodeAuth.Start();
            authorizationCodeAuth.AuthReceived += AuthorizationCodeAuth_AuthReceived;
            authorizationCodeAuth.OpenBrowser();
            Log("Waiting for auth to finish...");
            while (_auth == null)
            {
                Thread.Sleep(1000);
            }
            authorizationCodeAuth.Stop();
            if (_auth.Error != null || _auth.Code == null)
            {
                Log("Authorization failed!");
                Log(_auth.Error);
            }
            else
            {
                Log("Authorization granted!");
                _token = await GetToken(args);
                if (_token.Error != null)
                {
                    Log("Failed to get access token!");
                    Log(_token.Error);
                }
                else
                {
                    _refreshToken = _token.RefreshToken;
                    Log("Access granted!");
                    var refreshTokenTimer = new Timer(new TimerCallback(RefreshToken), args, (int)_token.ExpiresIn * 1000, (int)_token.ExpiresIn * 1000);
                    Log("Waiting until 31.12. 23:54:49...");
                    _spotifyWebAPI = new SpotifyWebAPI
                    {
                        AccessToken = _token.AccessToken,
                        TokenType = _token.TokenType,
                    };
                    while (true)
                    {
                        var time = DateTime.Now;
                        if (time.Day == 31 && time.Month == 12 && time.Hour == 23 && time.Minute == 54 && time.Second == 49)
                        {
                            Log("Executing the Final Countdown!");
                            _spotifyWebAPI.ResumePlayback(contextUri: "spotify:user:spoing01:playlist:3EjJLY2xPFSjN1JHjl8GqL", offset: 2);
                            break;
                        }
                        Thread.Sleep(500);
                    }
                    Log("Press any key to close...");
                    Console.Read();
                }
            }
        }

        private static async Task<Token> GetToken(string[] args)
        {
            Log("Getting access and refresh token...");
            using (var httpClient = new SpotifyTokenHttpClient(args))
            {
                var response = await httpClient.PostAsync("", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "code", _auth.Code }
                }));
                return JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
            }
        }

        private static async void RefreshToken(object state)
        {
            Log("Refreshing token...");
            using (var httpClient = new SpotifyTokenHttpClient(state as string[]))
            {
                var response = await httpClient.PostAsync("", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", _refreshToken }
                }));
                _token = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
                _spotifyWebAPI.AccessToken = _token.AccessToken;
                _spotifyWebAPI.TokenType = _token.TokenType;
            }
            Log("Token refreshed!");
        }

        private static void AuthorizationCodeAuth_AuthReceived(object sender, AuthorizationCode payload)
        {
            _auth = payload;
        }

        static void Log(string msg, bool newLine = true) => Console.Write("[" + DateTime.Now + "] " + msg + (newLine ? Environment.NewLine : ""));
    }
}