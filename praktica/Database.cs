using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace praktica
{
    /// <summary>
    /// Простой статический класс для работы с SQLite.
    /// Создаёт файл БД и таблицы при первом запуске.
    /// </summary>
    public static class Database
    {
        private static readonly string DbFilePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "service_company.db");

        // Строка подключения формируется в методе GetConnection на основе DbFilePath,
        // это поле здесь не требуется, но оставляем для наглядности.
        private static readonly string ConnectionString =
            "Data Source=" + "service_company.db" + ";Version=3;";

        public static void Initialize()
        {
            try
            {
                if (!File.Exists(DbFilePath))
                {
                    SQLiteConnection.CreateFile(DbFilePath);
                }

                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserName TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL,
    Role TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Clients (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Phone TEXT,
    Email TEXT
);

CREATE TABLE IF NOT EXISTS Orders (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER NOT NULL,
    Description TEXT NOT NULL,
    Status TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    FOREIGN KEY (ClientId) REFERENCES Clients(Id)
);

INSERT OR IGNORE INTO Users (UserName, Password, Role) VALUES
('guest', 'guest', 'Guest'),
('operator', 'operator', 'Operator'),
('admin', 'admin', 'Admin');
";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Ошибка инициализации базы данных: " + ex.Message,
                    "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static SQLiteConnection GetConnection()
        {
            // Явно указываем полный путь к файлу БД
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = DbFilePath,
                Version = 3
            };
            return new SQLiteConnection(builder.ToString());
        }

        public static DataTable GetDataTable(string sql, params SQLiteParameter[] parameters)
        {
            var table = new DataTable();
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand(sql, conn))
            using (var adapter = new SQLiteDataAdapter(cmd))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                conn.Open();
                adapter.Fill(table);
            }
            return table;
        }

        public static int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}


