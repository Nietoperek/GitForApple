using GitForApple.Helpers;
using GitForApple.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
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
            try
            {
                Repos.Clear();
                var repos = await DataGit.GetItemsAsync(update);
                var reposList = repos.ToList<Response>();
                await DataSQLite.SaveListAsync(reposList);
                var databaseObjects = await DataSQLite.GetItemsAsync();
                Repos.ReplaceRange(databaseObjects);
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
