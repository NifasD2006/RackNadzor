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
    /// Логика взаимодействия для DevicePage.xaml
    /// </summary>
    public partial class DevicePage : Page
    {
        public DevicePage()
        {


            InitializeComponent();
            var currentDevices = RackBDEntities.GetContext().Devices.ToList();

            DeviceListView.ItemsSource = currentDevices;
            int MaxCountDevices = DeviceListView.Items.Count;
            int CountDevices = DeviceListView.Items.Count;
            WendorComboBox.SelectedIndex = 0;
            MaxCountTBlock.Text = MaxCountDevices.ToString();
            UpdateDevices();
        }


        private void UpdateDevices()
        {
            var currentDevices = RackBDEntities.GetContext().Devices.ToList();

            if (WendorComboBox.SelectedIndex == 1)
                currentDevices = currentDevices.Where(p => p.DeviceManufacturer== "Dell").ToList();
            if (WendorComboBox.SelectedIndex == 2)
                currentDevices = currentDevices.Where(p => p.DeviceManufacturer == "Cisco").ToList();
            if (WendorComboBox.SelectedIndex == 3)
                currentDevices = currentDevices.Where(p => p.DeviceManufacturer == "Supermicro").ToList();
            if (WendorComboBox.SelectedIndex == 4)
                currentDevices = currentDevices.Where(p => p.DeviceManufacturer == "HP").ToList();
            if (WendorComboBox.SelectedIndex == 5)
                currentDevices = currentDevices.Where(p => p.DeviceManufacturer == "Lenovo").ToList();

            //currentDevices=currentDevices.Where(p => p.DeviceManufacturer.ToLower().Contains(TextBoxSearch.Text.ToLower() || p.DeviceDomainName.ToLower().Contains(TextBoxSearch.Text.ToLower())).ToList();

            //currentDevices = currentDevices.Where(p => p.DeviceManufacturer.ToLower().Contains(TextBoxSearch.Text.ToLower()) || p.DeviceRackName.ToLower().Contains(TextBoxSearch.Text.ToLower()) || p.DeviceModel.ToLower().Contains(TextBoxSearch.Text.ToLower()) || p.DeviceTypes.DeviceTypeName.ToLower().Contains(TextBoxSearch.Text.ToLower()) ||
            //    p.DeviceSerialNumber.ToLower().Contains(TextBoxSearch.Text.ToLower())  || p.DeviceDomainName.ToLower().Contains(TextBoxSearch.Text.ToLower()) || p.DeviceIP.ToLower().Contains(TextBoxSearch.Text.ToLower())).ToList();

            currentDevices = currentDevices.Where(p => p.DeviceModel.ToLower().Contains(TextBoxSearch.Text.ToLower()) || p.DeviceTypes.DeviceTypeName.ToLower().Contains(TextBoxSearch.Text.ToLower()) ||
             p.DeviceSerialNumber.ToLower().Contains(TextBoxSearch.Text.ToLower()) || (p.DeviceDomainName != null && p.DeviceDomainName.ToLower().Contains(TextBoxSearch.Text.ToLower())) || p.DeviceIP.ToLower().Contains(TextBoxSearch.Text.ToLower())).ToList();

            if (RButtonDown.IsChecked.Value)
            {
                currentDevices = currentDevices.OrderByDescending(p => p.DeviceInspectionDate).ToList();
            }

            if (RButtonUP.IsChecked.Value)
            {
                currentDevices = currentDevices.OrderBy(p => p.DeviceInspectionDate).ToList();
            }

            DeviceListView.ItemsSource = currentDevices;
            int CountDevices = DeviceListView.Items.Count;
            CountTBlock.Text = CountDevices.ToString();
        }

        private void RButtonUP_Checked(object sender, RoutedEventArgs e)
        {
            UpdateDevices();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateDevices();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDevices();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDevices();
        }

        private void RefrashBtn_Click(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Clear();
            WendorComboBox.SelectedIndex = 0;
            RButtonUP.IsChecked = false;
            RButtonDown.IsChecked = false;
            UpdateDevices();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

            Manager.MainFrame.Navigate(new AddEditPage());

        }
    }
}
