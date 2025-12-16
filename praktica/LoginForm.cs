using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace praktica
{
    public partial class LoginForm : Form
    {
        public User LoggedInUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            UiTheme.Apply(this);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUserName.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                const string sql = "SELECT Id, UserName, Password, Role FROM Users WHERE UserName = @name AND Password = @pass";
                var table = global::praktica.Database.GetDataTable(sql,
                    new SQLiteParameter("@name", username),
                    new SQLiteParameter("@pass", password));

                if (table.Rows.Count == 0)
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var row = table.Rows[0];
                LoggedInUser = new User
                {
                    Id = Convert.ToInt32(row["Id"]),
                    UserName = row["UserName"].ToString(),
                    Password = row["Password"].ToString(),
                    Role = ParseRole(row["Role"].ToString())
                };

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при авторизации: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static UserRole ParseRole(string role)
        {
            switch (role)
            {
                case "Guest":
                    return UserRole.Guest;
                case "Operator":
                    return UserRole.Operator;
                case "Admin":
                    return UserRole.Admin;
                default:
                    return UserRole.Guest;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}


