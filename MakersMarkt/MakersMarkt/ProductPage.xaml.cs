using MakersMarkt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Type = MakersMarkt.Data.Type;

namespace MakersMarkt
{
    public sealed partial class ProductPage : Page
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Type> Types { get; set; } = new List<Type>(); // Store types
        private readonly DispatcherQueue dispatcherQueue;

        public ProductPage()
        {
            this.InitializeComponent();
            dispatcherQueue = DispatcherQueue.GetForCurrentThread(); // Get DispatcherQueue instance
            _ = LoadDataAsync(); // Start loading data
        }

        private async Task LoadDataAsync()
        {
            using (var db = new AppDbContext())
            {
                Products = await db.Products.OrderByDescending(p => p.CreatedAt).ToListAsync();
                Types = await db.Types.ToListAsync(); // Load types

                // Ensure UI updates after data is loaded
                dispatcherQueue.TryEnqueue(() =>
                {
                    ProductListView.ItemsSource = Products;
                    CategoryDropdown.Items.Clear();

                    // Populate dropdown dynamically
                    CategoryDropdown.Items.Add(new ComboBoxItem { Content = "All Types", IsSelected = true });

                    foreach (var type in Types)
                    {
                        CategoryDropdown.Items.Add(new ComboBoxItem { Content = type.Name, Tag = type.Id });
                    }
                });
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Products == null || Products.Count == 0) return;

            string query = SearchBox.Text.ToLower();
            ProductListView.ItemsSource = Products.Where(p => p.Name.ToLower().Contains(query)).ToList();
        }
        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListView.SelectedItem is Product selectedProduct)
            {
                Frame.Navigate(typeof(ProductDetailPage), selectedProduct);
            }
        }

        private void CategoryDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Products == null || Products.Count == 0) return; // Ensure products are loaded

            var selectedItem = CategoryDropdown.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;

            if (selectedItem.Content.ToString() == "All Types")
            {
                ProductListView.ItemsSource = Products;
            }
            else
            {
                int selectedTypeId = (int)selectedItem.Tag;
                ProductListView.ItemsSource = Products.Where(p => p.TypeId == selectedTypeId).ToList();
            }
        }
    }
}
