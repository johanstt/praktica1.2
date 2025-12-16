using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace praktica
{
    public partial class UsersForm : Form
    {
        private readonly User _currentUser;

        public UsersForm(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            UiTheme.Apply(this);
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                dgvUsers.DataSource = global::praktica.Database.GetDataTable(
                    "SELECT Id, UserName AS 'Логин', Password AS 'Пароль', Role AS 'Роль' FROM Users");
                if (dgvUsers.Columns["Id"] != null)
                {
                    dgvUsers.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки пользователей: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new UserEditDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!dialog.ValidateInput())
                        return;

                    try
                    {
                        const string sql = "INSERT INTO Users (UserName, Password, Role) VALUES (@u, @p, @r)";
                        global::praktica.Database.ExecuteNonQuery(sql,
                            new SQLiteParameter("@u", dialog.UserName),
                            new SQLiteParameter("@p", dialog.Password),
                            new SQLiteParameter("@r", dialog.Role));
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка добавления пользователя: " + ex.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show("Выберите пользователя.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvUsers.CurrentRow.Cells["Id"].Value);
            string login = dgvUsers.CurrentRow.Cells["Логин"].Value.ToString();
            string password = dgvUsers.CurrentRow.Cells["Пароль"].Value.ToString();
            string role = dgvUsers.CurrentRow.Cells["Роль"].Value.ToString();

            using (var dialog = new UserEditDialog(login, password, role))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!dialog.ValidateInput())
                        return;

                    try
                    {
                        const string sql = "UPDATE Users SET UserName=@u, Password=@p, Role=@r WHERE Id=@id";
                        global::praktica.Database.ExecuteNonQuery(sql,
                            new SQLiteParameter("@u", dialog.UserName),
                            new SQLiteParameter("@p", dialog.Password),
                            new SQLiteParameter("@r", dialog.Role),
                            new SQLiteParameter("@id", id));
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка обновления пользователя: " + ex.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class UserEditDialog : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private ComboBox cbRole;
        private Button btnOk;
        private Button btnCancel;

        public string UserName => txtLogin.Text.Trim();
        public string Password => txtPassword.Text.Trim();
        public string Role => cbRole.SelectedItem != null ? cbRole.SelectedItem.ToString() : string.Empty;

        public UserEditDialog(string login = "", string password = "", string role = "Guest")
        {
            InitializeComponent();
            UiTheme.Apply(this);
            txtLogin.Text = login;
            txtPassword.Text = password;
            cbRole.Items.Add("Guest");
            cbRole.Items.Add("Operator");
            cbRole.Items.Add("Admin");
            cbRole.SelectedItem = role;
        }

        private void InitializeComponent()
        {
            this.txtLogin = new TextBox();
            this.txtPassword = new TextBox();
            this.cbRole = new ComboBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            var lblLogin = new Label();
            var lblPassword = new Label();
            var lblRole = new Label();

            lblLogin.Text = "Логин:";
            lblLogin.Left = 10;
            lblLogin.Top = 15;
            lblLogin.AutoSize = true;

            txtLogin.Left = 80;
            txtLogin.Top = 12;
            txtLogin.Width = 220;

            lblPassword.Text = "Пароль:";
            lblPassword.Left = 10;
            lblPassword.Top = 45;
            lblPassword.AutoSize = true;

            txtPassword.Left = 80;
            txtPassword.Top = 42;
            txtPassword.Width = 220;

            lblRole.Text = "Роль:";
            lblRole.Left = 10;
            lblRole.Top = 75;
            lblRole.AutoSize = true;

            cbRole.Left = 80;
            cbRole.Top = 72;
            cbRole.Width = 220;
            cbRole.DropDownStyle = ComboBoxStyle.DropDownList;

            btnOk.Text = "OK";
            btnOk.Left = 80;
            btnOk.Top = 110;
            btnOk.DialogResult = DialogResult.OK;

            btnCancel.Text = "Отмена";
            btnCancel.Left = 170;
            btnCancel.Top = 110;
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Text = "Пользователь";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new System.Drawing.Size(320, 150);
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Controls.Add(lblLogin);
            this.Controls.Add(txtLogin);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblRole);
            this.Controls.Add(cbRole);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        public bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                MessageBox.Show("Логин обязателен.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Пароль обязателен.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}


