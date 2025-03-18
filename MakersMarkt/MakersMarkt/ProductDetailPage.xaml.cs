using MakersMarkt.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

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
            this.Frame.Navigate(typeof(ProductPage));
        }

        private async void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct == null)
                return;

            using (var db = new AppDbContext())
            {
                var product = await db.Products.FindAsync(SelectedProduct.Id);
                if (product != null)
                {
                    product.Status = "Order Processing";
                    await db.SaveChangesAsync(); // Save changes to the DB
                }
            }
            // Display confirmation dialog
            var dialog = new ContentDialog
            {
                Title = "Order Placed",
                Content = "Your order is now being processed. You will be notified once it's shipped.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot // Required for WinUI
            };

            await dialog.ShowAsync(); // Show message async
        }

    }
}
