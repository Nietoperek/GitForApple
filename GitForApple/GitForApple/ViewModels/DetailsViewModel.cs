using GitForApple.Models;

namespace GitForApple.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public Repo Item { get; set; }
        public DetailsViewModel(Repo item = null)
        {
            Title = item.Name;
            Item = item;
        }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

    }
}
