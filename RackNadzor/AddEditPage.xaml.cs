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
        private int devid;
        public AddEditPage(Devices SelectDevice)
        {
            InitializeComponent();

            Manager.PortFrame = PortFrame;
            

            if (SelectDevice != null)
            {
                _currentDevice = SelectDevice;
                ManufacturerCombo.Text=_currentDevice.DeviceManufacturer;
                if (_currentDevice.DeviceID != 0) // Если редактирование
                {
                    TypeCombo.SelectedIndex = _currentDevice.DeviceTypeID - 1; // -1 если индексы начинаются с 0
                }
                RaydCombo.Text = _currentDevice.DeviceRackName[0].ToString();
                NomerCombo.Text = _currentDevice.DeviceRackName[1].ToString();
                DataInspect.SelectedDate = _currentDevice.DeviceInspectionDate;

                devid = _currentDevice.DeviceID;
                PortFrame.Navigate(new PortPage(devid));

            }
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
            else
            {
                // Проверяем, существует ли уже такой серийный номер
                bool serialExists = RackBDEntities1.GetContext().Devices
                    .Any(d => d.DeviceSerialNumber == _currentDevice.DeviceSerialNumber &&
                             d.DeviceID != _currentDevice.DeviceID); // Исключаем текущее устройство при редактировании

                if (serialExists)
                    errors.AppendLine("Устройство с таким серийным номером уже существует");
            }

            if (string.IsNullOrEmpty(_currentDevice.DeviceIP))
            {
                errors.AppendLine("Укажите IP-Адрес");
            }
            else if (!System.Net.IPAddress.TryParse(_currentDevice.DeviceIP, out _))
            {
                errors.AppendLine("Некорректный формат IP-адреса. Пример: 192.168.1.1");
            }

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
                RackBDEntities1.GetContext().Devices.Add(_currentDevice);
            }

            try
            {
                RackBDEntities1.GetContext().SaveChanges();
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
