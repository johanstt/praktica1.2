using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace praktica
{
    public partial class ClientsForm : Form
    {
        private readonly User _currentUser;

        public ClientsForm(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            UiTheme.Apply(this);
            ApplyRole();
            LoadClients();
        }

        private void ApplyRole()
        {
            bool canEdit = _currentUser.Role != UserRole.Guest;
            btnAdd.Enabled = canEdit;
            btnEdit.Enabled = canEdit;
        }

        private void LoadClients()
        {
            try
            {
                dgvClients.DataSource = global::praktica.Database.GetDataTable(
                    "SELECT Id, Name AS 'Имя', Phone AS 'Телефон', Email AS 'Email' FROM Clients");
                if (dgvClients.Columns["Id"] != null)
                {
                    dgvClients.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadClients();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new ClientEditDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(dialog.ClientName))
                    {
                        MessageBox.Show("Имя клиента обязательно.", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        const string sql = "INSERT INTO Clients (Name, Phone, Email) VALUES (@n, @p, @e)";
                        global::praktica.Database.ExecuteNonQuery(sql,
                            new SQLiteParameter("@n", dialog.ClientName),
                            new SQLiteParameter("@p", dialog.Phone ?? string.Empty),
                            new SQLiteParameter("@e", dialog.Email ?? string.Empty));
                        LoadClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка добавления клиента: " + ex.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvClients.CurrentRow == null)
            {
                MessageBox.Show("Выберите клиента.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvClients.CurrentRow;
            int id = Convert.ToInt32(row.Cells["Id"].Value);
            string name = row.Cells["Имя"].Value.ToString();
            string phone = row.Cells["Телефон"].Value.ToString();
            string email = row.Cells["Email"].Value.ToString();

            using (var dialog = new ClientEditDialog(name, phone, email))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(dialog.ClientName))
                    {
                        MessageBox.Show("Имя клиента обязательно.", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        const string sql = "UPDATE Clients SET Name=@n, Phone=@p, Email=@e WHERE Id=@id";
                        global::praktica.Database.ExecuteNonQuery(sql,
                            new SQLiteParameter("@n", dialog.ClientName),
                            new SQLiteParameter("@p", dialog.Phone ?? string.Empty),
                            new SQLiteParameter("@e", dialog.Email ?? string.Empty),
                            new SQLiteParameter("@id", id));
                        LoadClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка обновления клиента: " + ex.Message,
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

    /// <summary>
    /// Небольшое диалоговое окно для ввода/редактирования клиента.
    /// </summary>
    public class ClientEditDialog : Form
    {
        private TextBox txtName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private Button btnOk;
        private Button btnCancel;

        public string ClientName => txtName.Text.Trim();
        public string Phone => txtPhone.Text.Trim();
        public string Email => txtEmail.Text.Trim();

        public ClientEditDialog(string name = "", string phone = "", string email = "")
        {
            InitializeComponent();
            UiTheme.Apply(this);
            txtName.Text = name;
            txtPhone.Text = phone;
            txtEmail.Text = email;
        }

        private void InitializeComponent()
        {
            this.txtName = new TextBox();
            this.txtPhone = new TextBox();
            this.txtEmail = new TextBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            var lblName = new Label();
            var lblPhone = new Label();
            var lblEmail = new Label();

            lblName.Text = "Имя:";
            lblName.Left = 10;
            lblName.Top = 15;
            lblName.AutoSize = true;

            txtName.Left = 80;
            txtName.Top = 12;
            txtName.Width = 220;

            lblPhone.Text = "Телефон:";
            lblPhone.Left = 10;
            lblPhone.Top = 45;
            lblPhone.AutoSize = true;

            txtPhone.Left = 80;
            txtPhone.Top = 42;
            txtPhone.Width = 220;

            lblEmail.Text = "Email:";
            lblEmail.Left = 10;
            lblEmail.Top = 75;
            lblEmail.AutoSize = true;

            txtEmail.Left = 80;
            txtEmail.Top = 72;
            txtEmail.Width = 220;

            btnOk.Text = "OK";
            btnOk.Left = 80;
            btnOk.Top = 110;
            btnOk.DialogResult = DialogResult.OK;

            btnCancel.Text = "Отмена";
            btnCancel.Left = 170;
            btnCancel.Top = 110;
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Text = "Клиент";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new System.Drawing.Size(320, 150);
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhone);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }
    }
}


