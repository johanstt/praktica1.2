using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace praktica
{
    public partial class OrdersForm : Form
    {
        private readonly User _currentUser;

        public OrdersForm(User currentUser)
        {
            _currentUser = currentUser;
            InitializeComponent();
            UiTheme.Apply(this);
            InitStatusCombo();
            LoadClientsToCombo();
            ApplyRole();
            LoadOrders();
        }

        private void ApplyRole()
        {
            bool canEdit = _currentUser.Role != UserRole.Guest;
            btnAdd.Enabled = canEdit;
            btnEdit.Enabled = canEdit;
        }

        private void InitStatusCombo()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Новый");
            cbStatus.Items.Add("В работе");
            cbStatus.Items.Add("Завершён");
            cbStatus.SelectedIndex = 0;
        }

        private void LoadClientsToCombo()
        {
            try
            {
                var table = global::praktica.Database.GetDataTable("SELECT Id, Name FROM Clients ORDER BY Name");
                cbClient.DisplayMember = "Name";
                cbClient.ValueMember = "Id";
                cbClient.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrders()
        {
            try
            {
                const string sql = @"
SELECT o.Id,
       c.Name AS 'Клиент',
       o.Description AS 'Описание',
       o.Status AS 'Статус',
       o.CreatedAt AS 'Создан'
FROM Orders o
JOIN Clients c ON c.Id = o.ClientId
ORDER BY o.CreatedAt DESC";

                dgvOrders.DataSource = global::praktica.Database.GetDataTable(sql);
                if (dgvOrders.Columns["Id"] != null)
                {
                    dgvOrders.Columns["Id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказов: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbClient.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string description = txtDescription.Text.Trim();
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Введите описание заказа.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int clientId = Convert.ToInt32(cbClient.SelectedValue);
            string status = cbStatus.SelectedItem.ToString();

            try
            {
                const string sql = "INSERT INTO Orders (ClientId, Description, Status, CreatedAt) VALUES (@c, @d, @s, @dt)";
                global::praktica.Database.ExecuteNonQuery(sql,
                    new SQLiteParameter("@c", clientId),
                    new SQLiteParameter("@d", description),
                    new SQLiteParameter("@s", status),
                    new SQLiteParameter("@dt", DateTime.Now.ToString("s")));

                txtDescription.Clear();
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления заказа: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow == null)
            {
                MessageBox.Show("Выберите заказ.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvOrders.CurrentRow.Cells["Id"].Value);
            string currentStatus = dgvOrders.CurrentRow.Cells["Статус"].Value.ToString();

            cbStatus.SelectedItem = currentStatus;

            if (MessageBox.Show("Изменить статус выбранного заказа на " + cbStatus.SelectedItem + "?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    const string sql = "UPDATE Orders SET Status=@s WHERE Id=@id";
                    global::praktica.Database.ExecuteNonQuery(sql,
                        new SQLiteParameter("@s", cbStatus.SelectedItem.ToString()),
                        new SQLiteParameter("@id", id));
                    LoadOrders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления заказа: " + ex.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}


