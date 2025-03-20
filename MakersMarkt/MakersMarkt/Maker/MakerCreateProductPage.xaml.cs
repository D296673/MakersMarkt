using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MakersMarkt.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarkt.Maker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MakerCreateProductPage : Page
    {
        public MakerCreateProductPage()
        {
            this.InitializeComponent();
            //combobox vullen met de types database
            using (var db = new AppDbContext())
            {
                var types = db.Types.Select(t => t.Name).ToList();
                ProductTypeComboBox.ItemsSource = types;
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void CreateProductButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var product = new Product
                {
                    Name = ProductNameTextBox.Text,
                    Description = ProductDescriptionTextBox.Text,
                    Price = Convert.ToDecimal(ProductPriceTextBox.Text),
                    TypeId = db.Types.FirstOrDefault(t => t.Name == ProductTypeComboBox.SelectedItem.ToString()).Id,
                    Complexity = ProductComplexityTextBox.Text,
                    UniqueFeatures = ProductUniqueFeaturesTextBox.Text,
                    MakerId = Data.User.LoggedInUser.Id,
                    CreatedAt = DateTime.Now
                };
                db.Products.Add(product);
                db.SaveChanges();

            }
        }
    }
}
