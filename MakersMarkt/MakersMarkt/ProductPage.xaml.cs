using MakersMarkt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarkt
{
    public sealed partial class ProductPage : Page
    {
        public List<Product> Products { get; set; } = new List<Product>();

        public ProductPage()
        {
            this.InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            using (var db = new AppDbContext())
            {
                Products = await db.Products.OrderByDescending(p => p.CreatedAt).ToListAsync();
                ProductListView.UpdateLayout(); // Refresh UI
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            ProductListView.ItemsSource = Products.Where(p => p.Name.ToLower().Contains(query)).ToList();
        }

        private void CategoryDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string category = (CategoryDropdown.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (category == "All Categories")
            {
                ProductListView.ItemsSource = Products;
            }
            else
            {
                ProductListView.ItemsSource = Products.Where(p => p.Category == category).ToList();
            }
        }
    }
}
