using System.Windows.Forms;

namespace praktica
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnClients;
        private Button btnOrders;
        private Button btnUsers;
        private Button btnExit;
        private Label lblCurrentUser;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnClients = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblCurrentUser = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnClients
            // 
            this.btnClients.Location = new System.Drawing.Point(25, 25);
            this.btnClients.Name = "btnClients";
            this.btnClients.Size = new System.Drawing.Size(150, 40);
            this.btnClients.TabIndex = 0;
            this.btnClients.Text = "Клиенты";
            this.btnClients.UseVisualStyleBackColor = true;
            this.btnClients.Click += new System.EventHandler(this.btnClients_Click);
            // 
            // btnOrders
            // 
            this.btnOrders.Location = new System.Drawing.Point(25, 80);
            this.btnOrders.Name = "btnOrders";
            this.btnOrders.Size = new System.Drawing.Size(150, 40);
            this.btnOrders.TabIndex = 1;
            this.btnOrders.Text = "Заказы";
            this.btnOrders.UseVisualStyleBackColor = true;
            this.btnOrders.Click += new System.EventHandler(this.btnOrders_Click);
            // 
            // btnUsers
            // 
            this.btnUsers.Location = new System.Drawing.Point(25, 135);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(150, 40);
            this.btnUsers.TabIndex = 2;
            this.btnUsers.Text = "Пользователи";
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(25, 190);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(150, 40);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblCurrentUser
            // 
            this.lblCurrentUser.AutoSize = true;
            this.lblCurrentUser.Location = new System.Drawing.Point(12, 245);
            this.lblCurrentUser.Name = "lblCurrentUser";
            this.lblCurrentUser.Size = new System.Drawing.Size(83, 13);
            this.lblCurrentUser.TabIndex = 4;
            this.lblCurrentUser.Text = "Пользователь:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 271);
            this.Controls.Add(this.lblCurrentUser);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnUsers);
            this.Controls.Add(this.btnOrders);
            this.Controls.Add(this.btnClients);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Учет клиентов и заказов";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}


