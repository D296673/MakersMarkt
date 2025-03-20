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

namespace MakersMarkt.User
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserPage : Page
    {
        public UserPage()
        {
            this.InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            // Get the logged-in user
            var user = Data.User.LoggedInUser;
            if (user == null)
            {
                // Handle case where no user is logged in (optional: navigate to login)
                return;
            }
            var roleId = user.RoleId;
            // Show/hide buttons based on user 
            if (roleId == 2)
            {
                if (user.IsApproved == true)
                {
                    CreateProductButton.Visibility = Visibility.Visible;
                    OrdersButton.Visibility = Visibility.Visible;
                }
            }

            // Set user info
            UserNameText.Text = user.Name;
            using (var db = new AppDbContext())
            {
                var role = db.Roles.FirstOrDefault(r => r.Id == roleId);
                if (role != null)
                {
                    var Rolename = role.Name;
                    UserRoleText.Text = Rolename ?? "Unknown";
                }
            }
            UserCreatedAtText.Text = user.CreatedAt.ToString("MMMM dd, yyyy");

            // Load user’s products
            using (var db = new AppDbContext())
            {
                var userProducts = db.Products.Where(p => p.MakerId == user.Id).ToList();
                UserProductsListView.ItemsSource = userProducts;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Data.User.LoggedInUser = null; // Clear session
            Frame.Navigate(typeof(LoginPage)); // Redirect to login
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CreateProduct_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Maker.MakerCreateProductPage));
        }
    }

}
