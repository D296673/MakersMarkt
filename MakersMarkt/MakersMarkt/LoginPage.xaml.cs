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
    public sealed partial class LoginPage : Page
    {
        private int _userId { get; set; }
        public LoginPage()
        {
            this.InitializeComponent();
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                string name = NameTextBox.Text;
                string password = PasswordTextBox.Password;
                string hashedPassword = HashPassword(password); // Hash the entered password

                // Compare the hashed password with the stored hashed password
                var user = db.Users.FirstOrDefault(u => u.Name == name && u.Password == hashedPassword);

                if (user != null)
                {
                    int RoleId = user.RoleId;
                    int userId = user.Id;
                    _userId = userId;
                    Data.User.LoggedInUser = user;

                    switch (RoleId)
                    {
                        case 1:
                            this.Frame.Navigate(typeof(Moderator.ModeratorPage));
                            break;

                        case 2:
                            this.Frame.Navigate(typeof(Maker.MakerPage));
                            break;

                        case 3:
                            this.Frame.Navigate(typeof(User.UserPage));
                            break;
                    }
                }
                else
                {
                    ErrorTextBlock.Text = "Naam of wachtwoord is onjuist";
                }
            }
        }
    }
}
