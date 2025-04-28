using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;
using System.Text.Json;
using Biblioteka.Models;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IImageBackgroundChanger backgroundChanger;
        public MainWindow()
        {
            InitializeComponent();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            backgroundChanger = new ImageBackgroundChanger(new string[]
            {
                System.IO.Path.Combine(basePath, "background_photos", "background.jpg"),
                System.IO.Path.Combine(basePath, "background_photos", "background1.jpg"),
                System.IO.Path.Combine(basePath, "background_photos", "background2.jpg")
            });

            backgroundChanger.SetInterval(5);

            backgroundChanger.StartChanging(Background);
        }

        private void add_button_click(object sender, RoutedEventArgs e)
        {
            ChoiceWindow add = new ChoiceWindow();
            add.ShowDialog();
        }

        private void borrow_button_click(object sender, RoutedEventArgs e)
        {
            BorrowWindow add = new BorrowWindow();
            add.ShowDialog();
        }

        private void return_button_click(object sender, RoutedEventArgs e)
        {
            ReturnWindow add = new ReturnWindow();
            add.ShowDialog();
        }

        private void ShowRegisterForm_Click(object sender, RoutedEventArgs e)
        {
            LoginGrid.Visibility = Visibility.Collapsed;
            RegisterGrid.Visibility = Visibility.Visible;
        }

        private void ShowLoginForm_Click(object sender, RoutedEventArgs e)
        {
            RegisterGrid.Visibility = Visibility.Collapsed;
            LoginGrid.Visibility = Visibility.Visible;
        }

        private string usersFile = "users.json";

        private List<UserAdd> LoadUsers()
        {
            if (!File.Exists(usersFile))
                return new List<UserAdd>();
            string json = File.ReadAllText(usersFile);
            return JsonSerializer.Deserialize<List<UserAdd>>(json) ?? new List<UserAdd>();
        }

        private void SaveUsers(List<UserAdd> users)
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(usersFile, json);
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            string username = regUsername.Text.Trim();
            string email = regEmail.Text.Trim();
            string password = regPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Wypełnij wszystkie pola!");
                return;
            }

            var users = LoadUsers();

            if (users.Any(u => u.Username == username))
            {
                MessageBox.Show("Nazwa użytkownika już istnieje!");
                return;
            }
            if (users.Any(u => u.E_mail == email))
            {
                MessageBox.Show("Ten e-mail już istnieje!");
                return;
            }

            int newId = users.Count > 0 ? users.Max(u => u.ID) + 1 : 1;

            users.Add(new UserAdd
            {
                ID = newId,
                Username = username,
                E_mail = email,
                Password = HashPassword(password)
            });

            SaveUsers(users);

            MessageBox.Show("Konto utworzone! Możesz się zalogować.");
            ShowLoginForm_Click(null, null);
        }

        private void log_btn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Wypełnij wszystkie pola!");
                return;
            }

            var users = LoadUsers();
            string hashedPassword = HashPassword(password);

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

            if (user != null)
            {
                Biblioteka.Models.Session.CurrentUser = user.Username;
                Biblioteka.Models.Session.CurrentUserEmail = user.E_mail;

                log_form.Visibility = Visibility.Collapsed;
                borrow_button.IsEnabled = true;
                return_button.IsEnabled = true;
                add_button.IsEnabled = true;

                greetingLabel.Content = $"Cześć {user.Username}!";

                MessageBox.Show($"Zalogowano jako {user.Username}!");

                borrow_button.IsEnabled = true;
                add_button.IsEnabled = true;
                return_button.IsEnabled = true;

            }
            else
            {
                MessageBox.Show("Nieprawidłowa nazwa użytkownika lub hasło!");
            }
        }
    }
}