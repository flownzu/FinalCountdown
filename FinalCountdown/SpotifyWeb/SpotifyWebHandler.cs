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

namespace FinalCountdown.SpotifyWeb
{
    public class SpotifyWebHandler
    {
        private static SpotifyWebAPI _spotifyWebAPI;
        private static AuthorizationCode _auth;
        private static Token _token;
        private static string _refreshToken;
        private static Timer _refreshTokenTimer;

        public event EventHandler AuthorizationCompleted;
        public event EventHandler AuthenticationCompleted;
        public event EventHandler SpotifyWebAPIReady;

        public async Task GetAuthorization()
        {
            // Start new AuthorizationCode Auth with my web-server as redirect-uri
            AuthorizationCodeAuth authorizationCodeAuth = new AuthorizationCodeAuth("http://flownzu.com/FinalCountdown/auth", "http://localhost:8800", Scope.Streaming, "123");
            authorizationCodeAuth.Start();
            authorizationCodeAuth.AuthReceived += AuthorizationCodeAuth_AuthReceived;
            authorizationCodeAuth.OpenBrowser();
            // Wait for auth to finish
            while (_auth == null)
            {
                await Task.Delay(1000);
            }
            authorizationCodeAuth.Stop();
        }

        private void AuthorizationCodeAuth_AuthReceived(object sender, AuthorizationCode payload)
        {
            _auth = payload;
            OnAuthorizationCompleted();
        }

        public async Task GetToken()
        {
            using (var httpClient = new SpotifyTokenHttpClient())
            {
                var response = await httpClient.PostAsync("", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "code", _auth.Code }
                }));
                // Get the token and refresh token from response and initialize the refreshTokenTimer that automatically refreshes the token
                _token = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
                _refreshToken = _token.RefreshToken;
                if (_refreshTokenTimer != null)
                {
                    _refreshTokenTimer.Change((int)_token.ExpiresIn * 1000, (int)_token.ExpiresIn * 1000);
                }
                else _refreshTokenTimer = new Timer(new TimerCallback(RefreshToken), null, (int)_token.ExpiresIn * 1000, (int)_token.ExpiresIn * 1000);
                OnAuthenticationCompleted();
            }
        }

        public async void RefreshToken(object state)
        {
            using (var httpClient = new SpotifyTokenHttpClient())
            {
                var response = await httpClient.PostAsync("", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", _refreshToken }
                }));
                // Get the token from the refresh token response
                // The refresh token does not need to be updated as it can be used again
                _token = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
                _spotifyWebAPI.AccessToken = _token.AccessToken;
                _spotifyWebAPI.TokenType = _token.TokenType;
            }
        }

        public void BuildWebAPI()
        {
            if (!string.IsNullOrEmpty(_token.AccessToken) && !string.IsNullOrEmpty(_token.TokenType))
            {
                _spotifyWebAPI = new SpotifyWebAPI
                {
                    AccessToken = _token.AccessToken,
                    TokenType = _token.TokenType
                };
                OnSpotifyWebAPIReady();
            }
        }

        public async Task<PrivateProfile> GetUserProfile()
        {
            // get the users private profile (display username etc)
            _spotifyWebAPI.WebClient.EmptyHeaders();
            return await _spotifyWebAPI.GetPrivateProfileAsync();
        }

        public async Task<List<SimplePlaylist>> GetPlaylists(string userId)
        {
            _spotifyWebAPI.WebClient.EmptyHeaders();
            var playlistPages = await _spotifyWebAPI.GetUserPlaylistsAsync(userId);
            // if the response has 0 items initialize an empty collection otherwise initialize the collection with the items from the first response
            List<SimplePlaylist> playlistList = playlistPages.Items != null ? new List<SimplePlaylist>(playlistPages.Items) : new List<SimplePlaylist>();
            while (playlistPages.HasNextPage())
            {
                _spotifyWebAPI.WebClient.EmptyHeaders();
                // get next page and add the items to collection
                playlistPages = await _spotifyWebAPI.GetNextPageAsync(playlistPages);
                playlistList.AddRange(playlistPages.Items);
            }
            return playlistList;
        }

        public async Task<List<PlaylistTrack>> GetTracksFromPlaylist(string userId, string playlistId)
        {
            _spotifyWebAPI.WebClient.EmptyHeaders();
            var trackPages = await _spotifyWebAPI.GetPlaylistTracksAsync(userId, playlistId);
            // basically the same as GetPlaylists code except with tracks
            List<PlaylistTrack> playlistTracks = trackPages.Items != null ? new List<PlaylistTrack>(trackPages.Items) : new List<PlaylistTrack>();
            while (trackPages.HasNextPage())
            {
                _spotifyWebAPI.WebClient.EmptyHeaders();
                trackPages = await _spotifyWebAPI.GetNextPageAsync(trackPages);
                playlistTracks.AddRange(trackPages.Items);
            }
            return playlistTracks;
        }

        public async Task<ErrorResponse> PlayTrack(string contextUri = "", List<string> trackUris = null, int? offset = null)
        {
            _spotifyWebAPI.WebClient.EmptyHeaders();
            return await _spotifyWebAPI.ResumePlaybackAsync(contextUri: contextUri, uris: trackUris, offset: offset);
        }

        protected virtual void OnAuthorizationCompleted()
        {
            AuthorizationCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnAuthenticationCompleted()
        {
            AuthenticationCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSpotifyWebAPIReady()
        {
            SpotifyWebAPIReady?.Invoke(this, EventArgs.Empty);
        }
    }
}
