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

namespace MakersMarkt
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var RoleId = 0;
            if (PasswordTextBox.Password != ConfirmPasswordTextBox.Password)
            {
                ErrorTextBlock.Text = "Wachtwoorden Komen niet overheen";
                return;
            }
            else if (UserNameTextBox.Text == "" || PasswordTextBox.Password == "" || ConfirmPasswordTextBox.Password == "")
            {
                ErrorTextBlock.Text = "Vul alle velden in";
                return;
            }
            using (var db = new AppDbContext())
            {
                var users = db.Users.FirstOrDefault(u => u.Name == UserNameTextBox.Text);
                var passwords = db.Users.FirstOrDefault(u => u.Password == PasswordTextBox.Password);
                if (users != null)
                {
                    ErrorTextBlock.Text = "Gebruikersnaam is al in gebruik";
                    return;
                } else if (passwords != null)
                {
                    ErrorTextBlock.Text = "Wachtwoord is al in gebruik";
                    return;
                }
                var passwordHash = LoginPage.HashPassword(PasswordTextBox.Password);
                if (IsCreatorCheckBox.IsChecked == true)
                {
                    RoleId = 2;
                }
                else
                {
                    RoleId = 3;
                }
                var user = new Data.User
                {
                    Name = UserNameTextBox.Text,
                    Password = passwordHash,
                    RoleId = RoleId,
                    CreatedAt = DateTime.Now
                };
                db.Users.Add(user);
                db.SaveChanges();
                if (RoleId == 2)
                {
                    var Flag = new ModerationFlag
                    {
                        UserId = user.Id,
                        ModeratorId = 1,
                        Category = "Maker",
                        Reason = "New Maker",
                        CreatedAt = DateTime.Now
                    };
                    db.ModerationFlags.Add(Flag);
                    db.SaveChanges();
                }
                this.Frame.Navigate(typeof(LoginPage));
            }
        }
    }
}
