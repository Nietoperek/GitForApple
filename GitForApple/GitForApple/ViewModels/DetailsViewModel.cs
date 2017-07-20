using GitForApple.Models;

namespace GitForApple.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public DetailsViewModel(Item item = null)
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
