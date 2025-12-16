using System;
using System.Windows.Forms;

namespace praktica
{
    public partial class MainForm : Form
    {
        private readonly User _currentUser;

        public MainForm(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            UiTheme.Apply(this);
            ApplyRole();
        }

        private void ApplyRole()
        {
            lblCurrentUser.Text = $"Пользователь: {_currentUser.UserName} ({_currentUser.Role})";

            // Гостю закрываем доступ к редактированию (но оставляем просмотр)
            btnClients.Enabled = _currentUser.Role != UserRole.Guest;
            btnOrders.Enabled = _currentUser.Role != UserRole.Guest;

            // Только админ может управлять пользователями
            btnUsers.Enabled = _currentUser.Role == UserRole.Admin;
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            using (var f = new ClientsForm(_currentUser))
            {
                f.ShowDialog();
            }
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            using (var f = new OrdersForm(_currentUser))
            {
                f.ShowDialog();
            }
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            using (var f = new UsersForm(_currentUser))
            {
                f.ShowDialog();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}


