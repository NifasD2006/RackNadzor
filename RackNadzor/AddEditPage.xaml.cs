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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {

        private Devices _currentDevice = new Devices();
        public AddEditPage()
        {
            InitializeComponent();
            DataContext = _currentDevice;
        }



        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DataInspect.SelectedDate = DateTime.Now;
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(_currentDevice.DeviceModel))
                errors.AppendLine("Укажите название модели");

            if (string.IsNullOrEmpty(_currentDevice.DeviceSerialNumber))
                errors.AppendLine("Укажите серийный номер");

            if (string.IsNullOrEmpty(_currentDevice.DeviceIP))
                errors.AppendLine("Укажите IP-Адрес");
            else if (!System.Net.IPAddress.TryParse(_currentDevice.DeviceIP, out _))
                errors.AppendLine("Некорректный формат IP-адреса");

            if (string.IsNullOrEmpty(_currentDevice.DevicePositionInUnits.ToString()) && _currentDevice.DevicePositionInUnits>0 && _currentDevice.DevicePositionInUnits > 48)
                errors.AppendLine("Укажите название позицию в стойке");

            if (TypeCombo.SelectedItem == null)
                errors.AppendLine("Выберите тип устройства");

            if (ManufacturerCombo.SelectedItem == null)
                errors.AppendLine("Выберите производителя");

            if (RaydCombo.SelectedItem == null)
                errors.AppendLine("Выберите ряд");

            if (NomerCombo.SelectedItem == null)
                errors.AppendLine("Выберите номер");

            if (!DataInspect.SelectedDate.HasValue)
                errors.AppendLine("Укажите дату технического осмотра");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentDevice.DeviceManufacturer = ManufacturerCombo.Text;
            _currentDevice.DeviceTypeID = TypeCombo.SelectedIndex + 1;
            _currentDevice.DeviceRackName = RaydCombo.Text + NomerCombo.Text;
            _currentDevice.DeviceInspectionDate = DataInspect.SelectedDate;
            _currentDevice.DeviceSizeInUnits = 1;
            _currentDevice.DeviceDataCenters = 1;

            if (_currentDevice.DeviceID == 0)
            {
                RackBDEntities.GetContext().Devices.Add(_currentDevice);
            }

            try
            {
                RackBDEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
