using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Principal;
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
using System.Xml;

namespace BankClients
{
    public partial class DataClient : Window
    {
        private AccountsViewModel accountsViewModel;
        public DataClient()
        {
            InitializeComponent();
            accountsViewModel = new AccountsViewModel("C:\\Users\\ekost\\source\\repos\\BankClients\\BankClients\\BankClients.json");
            DataContext = accountsViewModel;
        }

        private void ListBoxElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxElements.SelectedItem is Account selectedAccount)
            {
                accountsViewModel.SelectedAccount = selectedAccount;
                ListBoxElements.Items.Refresh();
            }
        }
        public void AddAccount(object sender, RoutedEventArgs e)
        {
            DateTime nonullableDate = DateRegistration.SelectedDate.Value;
            accountsViewModel.AddAccount(LastName.Text, FirstName.Text, SurName.Text, AccountNumber.Text, int.Parse(Balance.Text), nonullableDate);
        }

        public void DeleteAccount(object sender, RoutedEventArgs e)
        {
            if (ListBoxElements.SelectedItem is Account selectedAccount)
            {
                accountsViewModel.RemoveItem(selectedAccount);
                ListBoxElements.Items.Refresh();
            }
        }

        public void Transfer(object sender, RoutedEventArgs e)
        {
            if (ListBoxElements.SelectionMode == SelectionMode.Multiple)
            {
                
                if (ListBoxElements.SelectedItems.Count == 2)
                {
                    Account accountSender = ListBoxElements.SelectedItems[0] as Account;
                    Account accountRecipient = ListBoxElements.SelectedItems[1] as Account;
                    //WindowTransfer windowTransfer = new WindowTransfer (accountSender, accountRecipient, Accounts);
                    //windowTransfer.Show ();
                    accountsViewModel.ShowWindowTransfer(accountSender, accountRecipient);
                }
                else
                {
                    MessageBox.Show("Выберите только два счёта!", "Ошибка");
                }

                ListBoxElements.SelectionMode = SelectionMode.Single;
                ButtonMode.Content = "Включить режим перевода";
            }
            else 
            {
                MessageBox.Show("Включите режим перевода и выберите два счёта!", "Ошибка");
            }
        }

        public void ChangeMode(object sender, RoutedEventArgs e)
        {
            if (ListBoxElements.SelectionMode == SelectionMode.Single)
            {
                ListBoxElements.SelectionMode = SelectionMode.Multiple;
                ButtonMode.Content = "Включить режим просмотра";
            }
            else
            {
                ListBoxElements.SelectionMode = SelectionMode.Single;
                ButtonMode.Content = "Включить режим перевода";
            }
            
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Укажите путь к файлу для сохранения
            string filePath = "C:\\Users\\ekost\\source\\repos\\BankClients\\BankClients\\BankClients.json";

            // Сохраните данные в JSON файл
            SaveDataToJson(filePath, accountsViewModel);
        }

        public void SaveDataToJson(string filePath, AccountsViewModel accountsViewModel)
        {
            string json = JsonConvert.SerializeObject(accountsViewModel.Accounts, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void DataClient_Actevated(object sender, EventArgs e)
        {
            ListBoxElements.Items.Refresh();
        }
    }

    public class Account
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string AccountNumber { get; set; }
        public int Balance { get; set; }
        public DateTime DateRegistration { get; set; }
    }

    public class AccountsViewModel
    {
        public ObservableCollection<Account> Accounts { get; set; } = new ObservableCollection<Account>();
        private Account selectedAccount;

        public Account SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
            }
        }
        public AccountsViewModel (string jsonFilePath)
        {
            // Загрузка данных из JSON файла (реализация зависит от вашего метода загрузки)
            Accounts = LoadPhonesFromJson(jsonFilePath);
        }

        private ObservableCollection<Account> LoadPhonesFromJson(string jsonFilePath)
        {
            // Проверяем, существует ли файл
            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException("JSON file not found", jsonFilePath);
            }

            // Читаем содержимое файла
            var jsonData = File.ReadAllText(jsonFilePath);

            // Десериализуем JSON в список телефонов
            var phonesAccounts = JsonConvert.DeserializeObject<List<Account>>(jsonData);

            // Возвращаем ObservableCollection
            return new ObservableCollection<Account>(phonesAccounts);
        }

        public void AddAccount(string LastName, string FirstName, string SurName, string AccountNumber, int Balance, DateTime DateRegistration)
        {
            Accounts.Add(new Account { LastName = LastName, FirstName = FirstName, SurName = SurName, AccountNumber = AccountNumber, Balance = Balance, DateRegistration = DateRegistration });
        }

        public void RemoveItem(Account item)
        {
            Accounts.Remove(item);
        }

        public void ShowWindowTransfer(Account accountSender, Account accountRecipient)
        {
            WindowTransfer windowTransfer = new WindowTransfer(accountSender, accountRecipient, Accounts);
            windowTransfer.Show();
        }
    }
}
