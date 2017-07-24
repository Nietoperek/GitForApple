using Android.Widget;
using GitForApple.Helpers;
using GitForApple.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitForApple.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        
        public ObservableRangeCollection<Response> Repos { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command UpdateItemsCommand { get; set; }

        public MainViewModel()
        {
            Title = "Repo viewer";         
            Repos = new ObservableRangeCollection<Response>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandHttp(false));
            UpdateItemsCommand = new Command(async () => await ExecuteLoadItemsCommandHttp(true));
        }
        async Task ExecuteLoadItemsCommandHttp(bool update)
        {
            if (IsBusy)
                return;
            IsBusy = true;
            var network = DependencyService.Get<Helpers.INetworkState>().getNetworkStatus();
            if (network.Equals(NetworkStatus.NotReachable))
            {
                IsBusy = false;
                return;
            }
            try
            {
                var repos = await DataGit.GetItemsAsync(update);
                if (repos != null && repos.Any())
                {
                   // Repos.Clear();
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
