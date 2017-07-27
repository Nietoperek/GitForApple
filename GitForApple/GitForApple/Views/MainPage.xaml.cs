using GitForApple.Models;
using GitForApple.ViewModels;
using System;
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
            TableView.RowHeight = 80;
            MessagingCenter.Subscribe<MainViewModel>(this, "CheckConnection", async (obj) =>
            {
                if (!await DisplayAlert("No network", "Please enable WiFi or Mobile Data to continue", "CLOSE", "REFRESH"))
                    viewModel.LoadItemsCommand.Execute(null);

            });
            MessagingCenter.Subscribe<MainViewModel>(this, "SiteUnReachable", async (obj) =>
            {
                if (!await DisplayAlert("Site not reachable", "Please check your internet connection", "CLOSE", "REFRESH"))
                    viewModel.LoadItemsCommand.Execute(null);
            });

            MessagingCenter.Subscribe<MainViewModel>(this, "ItemsChanged", (obj) =>
            {
                addCells();
            });
            absLayout.RaiseChild(ProgressBar);
            absLayout.LowerChild(TableView);
        }

        async void cellTapped(object sender, EventArgs args)
        {
            //ItemsListView.IsEnabled = false; //freeze ListView
            ImageCell ic = (ImageCell)sender;
            Response selectedItem = (Response)ic.BindingContext;
            if (selectedItem == null) //item null or deselected
            {
                //ItemsListView.IsEnabled = true;
                return;
            }
            await Navigation.PushAsync(new DetailsPage(new DetailsViewModel(selectedItem)));
            // ItemsListView.SelectedItem = null;
        }

        //protected override async void OnAppearing()        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Repos.Count == 0)
            {
                viewModel.LoadItemsCommand.Execute(null);
                // while (viewModel.IsBusy) { }
            }
            //await viewModel.ExecuteLoadItemsCommandHttp(false);
            //addCells();
        }

        void addCells()
        {
            var repos = viewModel.Repos;
            //absLayout.RaiseChild(ProgressBar);
            //absLayout.LowerChild(TableView);
            foreach (Response r in repos)
            {
                ImageCell ic = new ImageCell();
                ic.SetBinding(ImageCell.ImageSourceProperty, "Owner.Avatar_url");
                ic.SetBinding(ImageCell.TextProperty, "Name");
                ic.SetBinding(ImageCell.DetailProperty, "Description");
                ic.BindingContext = r;

                ic.Tapped += cellTapped;
                TableSection.Add(ic);
            }
            //absLayout.RaiseChild(TableView);
           // absLayout.LowerChild(ProgressBar);
        }
    }
}
