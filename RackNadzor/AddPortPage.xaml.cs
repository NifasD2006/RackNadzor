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
    /// Логика взаимодействия для AddPortPage.xaml
    /// </summary>
    public partial class AddPortPage : Page
    {
        private Ports _currentPort = new Ports();

        private int _currentDevice;
        public AddPortPage( int DeviceID )
        {
            InitializeComponent();
            _currentDevice = DeviceID;
        }

        private void SavePortBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            // Проверка номера порта
            if (string.IsNullOrEmpty(PortNumberTextBox.Text))
            {
                errors.AppendLine("Укажите номер порта");
            }
            else
            {
                // ПРАВИЛЬНО: используем значение из TextBox, а не из _currentPort
                string portNumber = PortNumberTextBox.Text;

                // Проверяем, существует ли уже такой номер порта на этом устройстве
                bool portExists = RackBDEntities1.GetContext().Ports
                    .Any(p => p.PortNumber == portNumber && p.DeviceID == _currentDevice);

                if (portExists)
                    errors.AppendLine($"Порт с номером {portNumber} уже существует на этом устройстве");
            }

            // Проверка скорости порта
            if (PortSpeedComboBox.SelectedItem == null)
            {
                errors.AppendLine("Выберите скорость порта");
            }

            // Проверка статуса порта
            if (PortStatusComboBox.SelectedItem == null)
            {
                errors.AppendLine("Выберите статус порта");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка! Перепроверьте данные!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _currentPort.PortNumber = PortNumberTextBox.Text;
                _currentPort.PortSpeed = (PortSpeedComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? PortSpeedComboBox.Text;
                _currentPort.PortStatus = (PortStatusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? PortStatusComboBox.Text;
                _currentPort.PortType = "Ethernet";
                _currentPort.MacAdress = "00:1A:2B:3C:4D:01";
                _currentPort.DeviceID = _currentDevice;

                if (_currentPort.PortID == 0)
                {
                    RackBDEntities1.GetContext().Ports.Add(_currentPort);
                }

                RackBDEntities1.GetContext().SaveChanges();
                MessageBox.Show("Порт успешно добавлен!");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelPortBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
