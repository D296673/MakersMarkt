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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarkt.Moderator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModeratorProductListPage : Page
    {
        public ObservableCollection<MakersMarkt.Data.User> Makers { get; set; }
        public ObservableCollection<MakersMarkt.Data.Type> Types { get; set; }
        public ObservableCollection<Product> Products { get; set; }

        private Product selectedProduct;
        public ModeratorProductListPage()
        {
            this.InitializeComponent();

            Makers = new ObservableCollection<MakersMarkt.Data.User>();
            Types = new ObservableCollection<Data.Type>();

            LoadProducts();
            LoadMakers();
            LoadTypes();
        }

        private void LoadProducts()
        {
            using (var db = new AppDbContext())
            {
                var products = db.Products.Include(u => u.Type).ToList();
                Products = new ObservableCollection<Product>(products);
                ProductListView.ItemsSource = Products;
            }
        }

        private void LoadTypes()
        {
            using (var db = new AppDbContext())
            {
                Types.Clear();

                foreach (var type in db.Types.ToList())
                {
                    Types.Add(type);
                }
            }
        }

        private void LoadMakers()
        {
            using (var db = new AppDbContext())
            {
                Makers.Clear();
                var makerRoleId = db.Roles.FirstOrDefault(r => r.Name == "Maker")?.Id;

                if (makerRoleId.HasValue)
                {
                    foreach (var user in db.Users.Where(u => u.RoleId == makerRoleId.Value))
                    {
                        Makers.Add(user);
                    }
                }
            }
        }

        private void ProductListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            selectedProduct = (Product)e.ClickedItem;

            productNameTextBox.Text = selectedProduct.Name;
            productDescriptionTextBox.Text = selectedProduct.Description;
            productPriceTextBox.Text = selectedProduct.Price.ToString();
            typeComboBox.SelectedValue = selectedProduct.TypeId;
            makersComboBox.SelectedValue = selectedProduct.MakerId;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct != null)
            {
                selectedProduct.Name = productNameTextBox.Text;
                selectedProduct.Description = productDescriptionTextBox.Text;
                selectedProduct.Price = decimal.Parse(productPriceTextBox.Text);
                selectedProduct.TypeId = (int)typeComboBox.SelectedValue;
                selectedProduct.MakerId = (int)makersComboBox.SelectedValue;

                using (var db = new AppDbContext())
                {
                    db.Products.Update(selectedProduct);
                    db.SaveChanges();
                }

                var index = Products.IndexOf(selectedProduct);
                if (index >= 0)
                {
                    Products[index] = selectedProduct;
                }

                Console.WriteLine($"Product {selectedProduct.Name} is bijgewerkt.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct != null)
            {
                using (var db = new AppDbContext())
                {
                    db.Products.Remove(selectedProduct);
                    db.SaveChanges();
                }

                Products.Remove(selectedProduct);

                productNameTextBox.Text = string.Empty;
                productDescriptionTextBox.Text = string.Empty;
                productPriceTextBox.Text = string.Empty;
                typeComboBox.SelectedIndex = -1;
                makersComboBox.SelectedIndex = -1;

                selectedProduct = null;

                Console.WriteLine("Product is verwijderd.");
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
