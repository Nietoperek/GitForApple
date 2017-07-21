using GitForApple.Helpers;
using GitForApple.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitForApple.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        
        public ObservableRangeCollection<Response> Repos { get; set; }
        public Command LoadItemsCommand { get; set; }

        public MainViewModel()
        {
            Title = "Repo viewer";         
            Repos = new ObservableRangeCollection<Response>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommandHttp());            
        }
        async Task ExecuteLoadItemsCommandHttp()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Repos.Clear();
                var repos = await DataGit.GetItemsAsync(true);
                Repos.ReplaceRange(repos);
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
