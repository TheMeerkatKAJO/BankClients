using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace BankClients
{
    /// <summary>
    /// Логика взаимодействия для WindowTransfer.xaml
    /// </summary>
    public partial class WindowTransfer : Window, INotifyPropertyChanged
    {
        public Account accountSenderMain { get; set; }
        public Account accountRecipientMain { get; set; }
        public ObservableCollection<Account> AccountsMain { get; set; } = new ObservableCollection<Account>();

        public WindowTransfer(Account accountSender, Account accountRecipient, ObservableCollection<Account> Accounts)
        {
            InitializeComponent();
            accountSenderMain = accountSender;
            accountRecipientMain = accountRecipient;
            AccountsMain = Accounts;
            this.DataContext = this;

        }

        private void WindowTransfer_Loaded(object sender, RoutedEventArgs e)
        {
            //AccountSenderList.SelectedItem = accountSenderMain;
        }

        public void ChangeAccount(object sender, RoutedEventArgs e)
        {
            Account Prom;
            Prom = accountSenderMain;
            accountSenderMain = accountRecipientMain;
            accountRecipientMain = Prom;
            
            // Обновляем привязки
            OnPropertyChanged(nameof(accountSenderMain));
            OnPropertyChanged(nameof(accountRecipientMain));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

        public void TransferMoney(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(Summa.Text, out int summaint))
            {
                if (summaint <= accountSenderMain.Balance)
                {
                    accountSenderMain.Balance = accountSenderMain.Balance - summaint;
                    accountRecipientMain.Balance = accountRecipientMain.Balance + summaint;
                    MessageBox.Show("Перевод совершен!", "");
                    Close();
                }

                else
                {
                    MessageBox.Show("У отправителя недостаточно средств!", "Ошибка");
                }
            }

            else
            {
                MessageBox.Show("введите число в поле сумма!", "Ошибка");
            }
        }
    }
 }
