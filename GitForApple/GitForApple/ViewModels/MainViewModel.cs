﻿using Android.Widget;
using GitForApple.Helpers;
using GitForApple.Models;
using GitForApple.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace GitForApple.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        public ObservableRangeCollection<Repo> Repos { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command UpdateItemsCommand { get; set; }
        public Command NetworkCommand { get; set; }
        public Command NetworkConnectionCommand { get; set; }

        public MainViewModel()
        {
            Title = "Repo viewer";
            Repos = new ObservableRangeCollection<Repo>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandHttp(false));
            UpdateItemsCommand = new Command(async () => await ExecuteLoadItemsCommandHttp(true)); 
        }
       async Task<bool> NetworkAvailable()
        {
            var network = DependencyService.Get<Helpers.INetworkState>().getNetworkStatus();
            if (network.Equals(NetworkStatus.NotConnected))
            {
                MessagingCenter.Send(this, "CheckConnection");
                return false;
            }
            else if (!await DataGit.isSiteReachable("https://api.github.com/"))
            {
                MessagingCenter.Send(this, "SiteUnReachable");
                return false;
            }
            return true;
        }
        async Task ExecuteLoadItemsCommandHttp(bool update)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            if (Repos.Count == 0)
            {
                var databaseObjects = await DataSQLite.GetItemsAsync();
                if (databaseObjects != null && databaseObjects.Any())
                {
                    Repos.ReplaceRange(databaseObjects);
                }
            }
            if (!await NetworkAvailable())
            {
                IsBusy = false;
                return;
            }
            try
            {
                var repos = await DataGit.GetItemsAsync(Repos.Count > 0);
                if (repos != null && repos.Any())
                {
                    await DataSQLite.SaveListAsync(repos);
                    Repos.ReplaceRange(repos);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
