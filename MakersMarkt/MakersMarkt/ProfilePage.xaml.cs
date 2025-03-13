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
using UserModel = MakersMarkt.Data.User;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MakersMarkt
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        private List<UserModel> Users;
        private List<Product> Products;
        public ProfilePage()
        {
            this.InitializeComponent();
            LoadProfile();
        }

        private void LoadProfile()
        {
            using (var db = new AppDbContext())
            {
                var UserId = Data.User.LoggedInUser.Id;
                var user = db.Users
                      .Where(i => i.Id == UserId)
                      .FirstOrDefault();
                Products = db.Products.Where(i => i.MakerId == UserId).ToList();

                UserNameTextBox.Text = user.Name;
                CreatedAtTextBox.Text = user.CreatedAt.ToString();
                ProductListview.ItemsSource = Products;
            }
        }
        

    }
}
