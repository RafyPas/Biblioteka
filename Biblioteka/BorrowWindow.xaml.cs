using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Biblioteka.Models;
using Newtonsoft.Json;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Biblioteka
{
    public partial class BorrowWindow : Window
    {
        private IImageBackgroundChanger backgroundChanger;

        private const string GoogleBooksApiKey = "AIzaSyCqkRW8SMvRYN4Qqc_rpHulR4s7IBPv8rc";
        private readonly List<string> genres = new List<string>
        {
            "fiction", "fantasy", "science fiction", "mystery", "romance", "horror"
        };

        public BorrowWindow()
        {
            InitializeComponent();
            LoadGenres();

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

        private void LoadGenres()
        {
            GenresListBox.ItemsSource = genres;
        }

        private async void GenresListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedGenre = GenresListBox.SelectedItem as string;
            if (selectedGenre != null)
            {
                await LoadBooksByGenre(selectedGenre);
            }
        }

        private async Task LoadBooksByGenre(string genre)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(
                        $"https://www.googleapis.com/books/v1/volumes?q=subject:{genre}&maxResults=20&key={GoogleBooksApiKey}");

                    var result = JsonConvert.DeserializeObject<GoogleBooksResponse>(response);

                    var books = new List<Book>();
                    foreach (var item in result.items)
                    {
                        var book = new Book
                        {
                            Title = item.volumeInfo.title,
                            Authors = item.volumeInfo.authors,
                            Genre = genre,
                            ISBN = item.volumeInfo.industryIdentifiers?[0]?.identifier
                        };
                        books.Add(book);
                    }

                    BooksListBox.ItemsSource = books;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania książek: {ex.Message}");
            }
        }

        private async void BorrowButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BooksListBox.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Wybierz książkę!");
                return;
            }

            if (string.IsNullOrEmpty(Biblioteka.Models.Session.CurrentUserEmail))
            {
                MessageBox.Show("Brak adresu e-mail użytkownika. Nie można wysłać przypomnienia.");
                return;
            }

            selectedBook.BorrowedBy = Session.CurrentUser;
            selectedBook.BorrowedUntil = DateTime.Now.AddMinutes(1);
            MessageBox.Show($"Wypożyczono: {selectedBook.Title} do {selectedBook.BorrowedUntil}");

            await ScheduleReminderEmail(selectedBook);
        }

        private async Task ScheduleReminderEmail(Book book)
        {
            await Task.Delay(TimeSpan.FromMinutes(1));

            try
            {
                SendEmail(
                    to: Session.CurrentUserEmail,
                    subject: "Przypomnienie o zwrocie książki",
                    body: $"Prosimy o zwrot książki: {book.Title}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd wysyłania maila: {ex.Message}");
            }
        }

        private void SendEmail(string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Biblioteka", "przypomnienieooddaniu@gmail.com"));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Połącz się z serwerem SMTP Gmail
                    client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

                    // Uwierzytelnij się przy użyciu hasła aplikacji
                    client.Authenticate("przypomnienieooddaniu@gmail.com", "gaus leov ifpe tqtb");

                    // Wyślij wiadomość
                    client.Send(message);

                    // Rozłącz się
                    client.Disconnect(true);
                }

                MessageBox.Show("Mail został wysłany!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd wysyłania maila: {ex.Message}");
            }
        }
    }
    public class GoogleBooksResponse
    {
        public List<GoogleBookItem> items { get; set; }
    }

    public class GoogleBookItem
    {
        public VolumeInfo volumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string title { get; set; }
        public List<string> authors { get; set; }
        public List<string> categories { get; set; }
        public List<IndustryIdentifier> industryIdentifiers { get; set; }
    }

    public class IndustryIdentifier
    {
        public string identifier { get; set; }
    }
}