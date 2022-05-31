using Chat.Core;
using Chat.MVVM.Model;
using Chat.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }

        public RelayCommand ConnectToServerCommand { get; set; }

        private Server _server;
        public string Username { get; set; }
        public RelayCommand SendCommand { get; set; }

        private ContactModel _selectedContact;

        public ContactModel SelectedContact
        {
            get { return _selectedContact; }
            set 
            { 
                _selectedContact = value;
                OnPropertyChanged();
            }
        }


        private string _message;

        public string Message
        {
            get { return _message; }
            set 
            { 
                _message = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            _server = new Server();
            _server.connectedEvent += UserConnected;
            _server.messageReceivedEvent += MesssageReceived;
            _server.disconnectedEvent += RemoveUser;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));
            SendCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();
        }

        private void RemoveUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Contacts.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Contacts.Remove(user));
        }

        private void MesssageReceived()
        {
            var msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(new MessageModel
            {
                Username = "Участник",
                Message = msg
            }));
        }

        private void UserConnected()
        {
            var user = new ContactModel
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage(),
            };

            if(!Contacts.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Contacts.Add(user));
            }
        }
    }
}
