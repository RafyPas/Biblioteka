using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using Biblioteka.Models;

namespace Biblioteka
{
    public partial class ReturnWindow : Window
    {
        private IImageBackgroundChanger backgroundChanger;
        private List<Book> allBooks;
        private List<Book> userBooks;
        private string booksFile = "books.json"; // Plik z książkami

        public ReturnWindow()
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

            LoadUserBooks();
        }

        private void LoadUserBooks()
        {
            // Wczytaj wszystkie książki z pliku
            if (File.Exists(booksFile))
            {
                string json = File.ReadAllText(booksFile);
                allBooks = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
            }
            else
            {
                allBooks = new List<Book>();
            }

            // Filtruj książki wypożyczone przez aktualnego użytkownika
            userBooks = allBooks
                .Where(b => b.BorrowedBy == Session.CurrentUser)
                .ToList();

            BorrowedBooksListBox.ItemsSource = userBooks;
        }

        private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BorrowedBooksListBox.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Wybierz książkę do zwrotu!");
                return;
            }

            // Oznacz książkę jako zwróconą
            selectedBook.BorrowedBy = null;
            selectedBook.BorrowedUntil = null;

            // Zapisz zmiany do pliku
            SaveBooks();

            MessageBox.Show($"Zwrócono książkę: {selectedBook.Title}");

            // Odśwież listę
            LoadUserBooks();
        }

        private void SaveBooks()
        {
            string json = JsonSerializer.Serialize(allBooks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(booksFile, json);
        }
    }
}
