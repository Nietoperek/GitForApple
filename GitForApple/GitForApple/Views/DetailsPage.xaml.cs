using GitForApple.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GitForApple.Views
{
    public partial class DetailsPage : ContentPage
    {
        private DetailsViewModel viewModel;

        public DetailsPage()
        {
            InitializeComponent();
        }

        public DetailsPage(DetailsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
    }
}