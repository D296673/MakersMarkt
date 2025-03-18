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
    public sealed partial class ModeratorUserListPage : Page
    {
        private MakersMarkt.Data.User selectedUser;
        public ModeratorUserListPage()
        {
            this.InitializeComponent();

            LoadUsers(); 
        }

        private void LoadUsers()
        {
            using (var db = new AppDbContext())
            {
                var users = db.Users.Include(u => u.Role).ToList();
                UserListView.ItemsSource = users;
            }
        }

        private void UserListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = (MakersMarkt.Data.User)UserListView.SelectedItem;

            if (selectedUser != null)
            {
                ApprovedRadio.IsChecked = selectedUser.IsApproved;
                RejectedRadio.IsChecked = selectedUser.IsRejected;
                DeleteUserButton.IsEnabled = selectedUser.IsRejected;
            }
        }

        private void ApprovalStatusChanged(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                selectedUser.IsApproved = ApprovedRadio.IsChecked == true;
                selectedUser.IsRejected = RejectedRadio.IsChecked == true;

                DeleteUserButton.IsEnabled = selectedUser.IsRejected;

                SaveUserApprovalStatus(selectedUser);
            }
        }

        private void SaveUserApprovalStatus(MakersMarkt.Data.User user)
        {
            using (var db = new AppDbContext())
            {
                var dbUser = db.Users.FirstOrDefault(u => u.Id == user.Id);
                if (dbUser != null)
                {
                    dbUser.IsApproved = user.IsApproved;
                    dbUser.IsRejected = user.IsRejected;
                    db.SaveChanges();
                }
            }

            Console.WriteLine($"Gebruiker {user.Name} goedkeuringsstatus opgeslagen: " +
                              $"Goedgekeurd = {user.IsApproved}, Afgekeurd = {user.IsRejected}");
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null && selectedUser.IsRejected)
            {
                var users = (ObservableCollection<MakersMarkt.Data.User>)UserListView.ItemsSource;
                users.Remove(selectedUser);

                Console.WriteLine($"Gebruiker {selectedUser.Name} is verwijderd.");

                selectedUser = null;
                DeleteUserButton.IsEnabled = false;
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
