
using Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
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

namespace Shop
{

    public partial class MainWindow : Window
    {
        MegaBookShop context;
        public MainWindow()
        {
            InitializeComponent();
            context = new MegaBookShop();
        }

     

        private void showBooks(object sender, RoutedEventArgs e)
        {
            //Generate();
            grid.ItemsSource = context.Books.ToList();
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();

            if (context.Books.Count() < grid.Items.Count - 1)
            {
                int oldBooksCount = context.Books.Count();
                int newBooksCount = grid.Items.Count - 1;

                for (int i = oldBooksCount; i < newBooksCount; i++)
                {
                    context.Books.Add((Book)grid.Items[i]);
                }
            }

         

            context.SaveChanges();

        }

        
        private List<Book> GetRemovedBooks()
        {
            List<Book> removedBooks = new List<Book>();

            foreach (Book book in context.Books)
            {
                if (!grid.Items.Contains(book))
                {
                    removedBooks.Add(book);
                }
            }

            return removedBooks;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                context.Books.Remove((Book)grid.SelectedItem);
            }
            catch { MessageBox.Show("Can delete only books"); }
            
            context.SaveChanges();
        }

        private void showDisc(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource=context.Books.Where(book=>book.IsDiscount!=null).ToList();
        }

        private void SearchByTitle_Click(object sender, RoutedEventArgs e)
        {
            string searchTitle = titleTextBox.Text;
            context.Books.Load(); // Завантаження даних у DbSet
            grid.ItemsSource = context.Books.Local.ToObservableCollection().Where(book => book.Title == searchTitle);
        }

       
        private void SearchByGenre_Click(object sender, RoutedEventArgs e)
        {
            if (Enum.TryParse(genreTextBox.Text, out Genre searchGenre))
            {
                context.Books.Load(); // Завантаження даних у DbSet
                grid.ItemsSource = context.Books.Local.ToObservableCollection().Where(book => book.Genre == searchGenre);
            }
            else
            {
                MessageBox.Show("Invalid genre format.");
            }
        }

        private void SearchByAuthor_Click(object sender, RoutedEventArgs e)
        {
            string searchAuthor = authorTextBox.Text;
            context.Books.Load(); // Завантаження даних у DbSet
            grid.ItemsSource = context.Books.Local.ToObservableCollection().Where(book => book.Author == searchAuthor);
        }

        private void Buy(object sender, RoutedEventArgs e)
        {



            if (grid.SelectedItem != null && grid.SelectedItem is Book selectedBook)
            {
                Dialog dialog = new Dialog();

                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    // Отримання даних покупця з діалогового вікна
                    string customerName = dialog.CustomerName;


                    Customer existingCustomer = context.Customers.FirstOrDefault(c => c.Name == customerName);
                    if(existingCustomer == null) {
                        int lastCustomerId = context.Customers.OrderByDescending(c => c.Id).Select(c => c.Id).FirstOrDefault();

                        // Створюємо новий Id, який є останнім значенням Id плюс один
                        int newCustomerId = lastCustomerId + 1;

                        // Створюємо нового користувача з новим Id
                        Customer newCustomer = new Customer { Id = newCustomerId, Name = customerName };

                        // Додаємо нового користувача до бази даних
                        context.Customers.Add(newCustomer);
                        context.SaveChanges(); // Збереження змін у базі даних

                        // Використовуємо нове значення Id для створення покупки
                        int lastPurchaseId = context.Purchases.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault();

                        // Створюємо новий Id, який є останнім значенням Id плюс один
                        int newPurchaseId = lastPurchaseId + 1;

                        // Створюємо новий об'єкт покупки з новим Id
                        Purchase purchase = new Purchase
                        {
                            Id = newPurchaseId,
                            CustomerId = newCustomerId,
                            BookId = selectedBook.Id,
                            Total = selectedBook.Price ?? 0,
                            DateTime = DateTime.Now
                        };

                        // Додаємо нову покупку до бази даних
                        context.Purchases.Add(purchase);
                        context.SaveChanges();
                    }
                    else
                    {
                        int lastPurchaseId = context.Purchases.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefault();

                        // Створюємо новий Id, який є останнім значенням Id плюс один
                        int newPurchaseId = lastPurchaseId + 1;

                        // Створюємо новий об'єкт покупки з новим Id
                        Purchase purchase = new Purchase
                        {
                            Id = newPurchaseId,
                            CustomerId = existingCustomer.Id,
                            BookId = selectedBook.Id,
                            Total = selectedBook.Price ?? 0,
                            DateTime = DateTime.Now
                        };

                        // Додаємо нову покупку до бази даних
                        context.Purchases.Add(purchase);
                        context.SaveChanges();
                    }

                    MessageBox.Show("Покупку здійснено успішно!");
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть книгу зі списку.");
            }

        }

        private void showCustomers(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource=context.Customers.ToList();
        }

        private void showPurchases(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource=context.Purchases.ToList();
        }
    }


      


    }




    




