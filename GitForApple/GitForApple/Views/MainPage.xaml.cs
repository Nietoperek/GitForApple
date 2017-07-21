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
        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MainViewModel();
                        
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Response;
            if (item == null)
                return;

            await Navigation.PushAsync(new DetailsPage(new DetailsViewModel(item)));

            ItemsListView.SelectedItem = null;
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Repos.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }                
    }
}
