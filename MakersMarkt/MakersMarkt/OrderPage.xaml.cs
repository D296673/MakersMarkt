using MakersMarkt.Data;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {
        public OrderPage()
        {
            this.InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var user = Data.User.LoggedInUser;
            if (user == null) return;

            UserNameText.Text = user.Name;
            UserRoleText.Text = user.Role?.Name ?? "Unknown";
            UserCreatedAtText.Text = user.CreatedAt.ToString("MMMM dd, yyyy");

            using (var db = new AppDbContext())
            {
                var orders = db.Orders
                    .Where(o => o.SellerId == user.Id)
                    .Select(o => new
                    {
                        o.Id,
                        o.Status,
                        ProductName = o.Product.Name,
                        BuyerName = o.Buyer.Name
                    })
                    .ToList();

                UserOrdersListView.ItemsSource = orders;
            }
        }

        private void StatusChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedOrder = (Order)comboBox.DataContext;
            var newStatus = comboBox.SelectedItem as string;

            using (var db = new AppDbContext())
            {
                var order = db.Orders.FirstOrDefault(o => o.Id == selectedOrder.Id);
                if (order != null)
                {
                    order.Status = newStatus;
                    db.SaveChanges();
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Data.User.LoggedInUser = null;
            Frame.Navigate(typeof(LoginPage));
        }

    }
}
