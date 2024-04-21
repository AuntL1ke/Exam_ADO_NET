using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logic;
namespace Shop
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public string CustomerName { get; set; }
        public Dialog()
        {
            InitializeComponent();
        }
        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            // Отримання даних з полів введення
            CustomerName = txtCustomerName.Text;
            

            // Закриття вікна
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Закриття вікна без збереження даних
            DialogResult = false;
        }
    }
}
