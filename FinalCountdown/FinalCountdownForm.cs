﻿using FinalCountdown.SpotifyWeb;
using SpotifyAPI.Web.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalCountdown
{
    public partial class FinalCountdownForm : Form
    {
        private static SpotifyWebHandler _spotifyWebHandler = new SpotifyWebHandler();
        private static Countdown.FinalCountdown _finalCountdown;
        private static PrivateProfile _userProfile;

        public FinalCountdownForm()
        {
            // register event
            _spotifyWebHandler.AuthorizationCompleted += _spotifyWebHandler_AuthorizationCompleted;
            _spotifyWebHandler.AuthenticationCompleted += _spotifyWebHandler_AuthenticationCompleted;
            _spotifyWebHandler.SpotifyWebAPIReady += _spotifyWebHandler_SpotifyWebAPIReady;
            InitializeComponent();
            Task.Run(async () => await BuildWebAPI());
        }

        private void _spotifyWebHandler_AuthenticationCompleted(object sender, EventArgs e)
        {
            UpdateStatus("Authentication completed.");
        }

        private void _spotifyWebHandler_AuthorizationCompleted(object sender, EventArgs e)
        {
            UpdateStatus("Authorization completed.");
        }

        private async void _spotifyWebHandler_SpotifyWebAPIReady(object sender, EventArgs e)
        {
            _finalCountdown = new Countdown.FinalCountdown(_spotifyWebHandler);
            _finalCountdown.CountdownStarted += _finalCountdown_CountdownStarted;
            _finalCountdown.CountdownStopped += _finalCountdown_CountdownStopped;
            _finalCountdown.TheFinalCountdown += _finalCountdown_TheFinalCountdown;
            _userProfile = await _spotifyWebHandler.GetUserProfile();
            Invoke(new Action(() =>
            {
                richTextBoxAccountInfo.AppendText("Connected as: " + _userProfile.DisplayName + Environment.NewLine);
                richTextBoxAccountInfo.AppendText("Account Status: " + new string(_userProfile.Product.Select((c, index) => index == 0 ? char.ToUpper(c) : c).ToArray()));
                checkBoxUsePlaylist.Enabled = true;
                buttonToggle.Enabled = true;
            }));
        }

        private void _finalCountdown_TheFinalCountdown(object sender, EventArgs e)
        {
            UpdateStatus("Executing 'The Final Countdown'");
        }

        private void _finalCountdown_CountdownStopped(object sender, EventArgs e)
        {
            Invoke(new Action(() => buttonToggle.Text = "Start!"));
            UpdateStatus("Final Countdown stopped.");
        }

        private void _finalCountdown_CountdownStarted(object sender, EventArgs e)
        {
            Invoke(new Action(() => buttonToggle.Text = "Stop!"));
            UpdateStatus("Final Countdown started.");
        }

        private async Task BuildWebAPI()
        {
            UpdateStatus("Waiting for user to grant authorization...");
            await _spotifyWebHandler.GetAuthorization();
            await _spotifyWebHandler.GetToken();
            _spotifyWebHandler.BuildWebAPI();
        }

        private void UpdateStatus(string status)
        {
            // invoke since the method gets called from a different thread
            Invoke(new Action(() =>
            {
                toolStripStatusLabel.Text = status;
            }));
        }

        private async void CheckBoxUsePlaylist_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUsePlaylist.Checked)
            {
                // clear playlists
                Invoke(new Action(() => listBoxPlaylists.Items.Clear()));
                var playlists = await _spotifyWebHandler.GetPlaylists(_userProfile.Id);
                // add new item to listbox for every playlist
                foreach (SimplePlaylist playlist in playlists)
                {
                    Invoke(new Action(() =>
                    {
                        listBoxPlaylists.Items.Add(playlist);
                    }));
                }
            }
        }

        private void ListBoxPlaylists_Format(object sender, ListControlConvertEventArgs e)
        {
            // display in listbox should just be the name of the playlist
            e.Value = (e.ListItem as SimplePlaylist).Name;
        }

        private async void ListBoxPlaylists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPlaylists.SelectedIndex >= 0)
            {
                // clear all items in the listboxtracks
                listBoxTracks.Items.Clear();
                var playlist = listBoxPlaylists.SelectedItem as SimplePlaylist;
                // get all tracks from playlist and add the tracks to the listboxtracks
                var tracks = await _spotifyWebHandler.GetTracksFromPlaylist(_userProfile.Id, playlist.Id);
                foreach (FullTrack track in tracks.Select(x => x.Track))
                {
                    Invoke(new Action(() =>
                    {
                        listBoxTracks.Items.Add(track);
                    }));
                }
            }
        }

        private void ListBoxTracks_Format(object sender, ListControlConvertEventArgs e)
        {
            // display in listbox should be artist names concatenated with a comma and the track name
            var item = (e.ListItem as FullTrack);
            e.Value = string.Join(", ", item.Artists.Select(x => x.Name)) + "   -   " + item.Name;
        }

        private async void ButtonToggle_Click(object sender, EventArgs e)
        {
            // when final countdown is already running cancel the countdown
            if (_finalCountdown.IsRunning)
            {
                await _finalCountdown.Cancel();
            }
            else
            {
                // otherwise check if the user wants to play the song within a playlist
                if (checkBoxUsePlaylist.Checked && listBoxPlaylists.SelectedIndex >= 0)
                {
                    // get tracks and search for The Final Countdown
                    var tracks = listBoxTracks.Items.OfType<FullTrack>();
                    int? trackIndex = tracks.Select((x, index) => new { Track = x, Index = index }).Where(x => x.Track.Name == "The Final Countdown" && x.Track.Artists.Any(a => a.Name == "Europe")).FirstOrDefault()?.Index;
                    // if the track was found set the context of _finalCountdown
                    if (tracks.Count() > 0 && trackIndex.HasValue)
                    {
                        _finalCountdown.SetContext((listBoxPlaylists.SelectedItem as SimplePlaylist).Uri, trackIndex.Value);
                    }
                    // otherwise clear it since we don't know where we should play the song from
                    else _finalCountdown.ClearContext();
                }
                else _finalCountdown.ClearContext();
#pragma warning disable 4014
                Task.Run(() => _finalCountdown.Start());
#pragma warning restore 4014
            }
        }
    }
}
