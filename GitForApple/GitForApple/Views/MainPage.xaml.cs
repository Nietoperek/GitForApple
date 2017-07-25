using GitForApple.Models;
using GitForApple.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GitForApple.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        MainViewModel viewModel;
        bool isBusy;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MainViewModel();
            bool alertResult = true;
            MessagingCenter.Subscribe<MainViewModel>(this, "CheckConnection", async (obj) =>
            {
                alertResult = await DisplayAlert("No network", "Please enable WiFi or Mobile Data to continue", "CLOSE", "REFRESH");
                if (!alertResult)
                {
                    viewModel.LoadItemsCommand.Execute(null);
                    alertResult = true;
                }
            });
            MessagingCenter.Subscribe<MainViewModel>(this, "SiteUnReachable", async (obj) =>
            {
                alertResult = await DisplayAlert("Site not reachable", "Please check your internet connection", "CLOSE", "REFRESH");
                if (!alertResult)
                {
                    viewModel.LoadItemsCommand.Execute(null);
                    alertResult = true;
                }
            });
        }

        void subscribeToMC()
        {

        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Response;
            if (item == null)
                return;
            if (!isBusy)
            {
                isBusy = true;
                await Navigation.PushAsync(new DetailsPage(new DetailsViewModel(item)));
                isBusy = false;
            }
            ItemsListView.SelectedItem = null;
        }

        //protected override async void OnAppearing()        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Repos.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
