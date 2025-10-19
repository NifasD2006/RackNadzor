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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RackNadzor
{
    /// <summary>
    /// Логика взаимодействия для PortPage.xaml
    /// </summary>
    public partial class PortPage : Page
    {
        private int DevID;


        public PortPage( int Dev)
        {
            InitializeComponent();

            DevID = Dev;
            UpdatePorts();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePorts();
        }

        public void UpdatePorts()
        {
            var currentPorts = RackBDEntities1.GetContext().Ports.Where(p => p.DeviceID == DevID).ToList();
            PortsListView.ItemsSource = currentPorts;
        }

        private void AddPortBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.PortFrame.Navigate(new AddPortPage(DevID));
            UpdatePorts();
        }

        private void DelPortBtn_Click(object sender, RoutedEventArgs e)
        {

            var currentPort = (sender as Button).DataContext as Ports;

            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    RackBDEntities1.GetContext().Ports.Remove(currentPort);
                    RackBDEntities1.GetContext().SaveChanges();
                    PortsListView.ItemsSource = RackBDEntities1.GetContext().Ports.ToList();
                    UpdatePorts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            UpdatePorts();

        }
    }
}
