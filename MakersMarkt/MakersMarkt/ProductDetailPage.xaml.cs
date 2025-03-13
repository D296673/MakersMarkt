using MakersMarkt.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace MakersMarkt
{
    public sealed partial class ProductDetailPage : Page
    {
        private Product SelectedProduct;

        public ProductDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Product product)
            {
                SelectedProduct = product;
                ProductName.Text = product.Name;
                ProductPrice.Text = $"${product.Price}";
                ProductDescription.Text = product.Description;
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            // Empty for now
        }
    }
}
