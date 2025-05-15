using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;

namespace ModulWpfApp.Settings
{
    public class TriggerSettingsRepository
    {
        private readonly string _dbPath;
        public TriggerSettingsRepository()
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);
            _dbPath = Path.Combine(dataDir, "TriggerSettings.db");
            if (!File.Exists(_dbPath))
                CreateDatabase();
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(_dbPath);
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();
            string sql = @"CREATE TABLE IF NOT EXISTS TriggerSettings (
                Id TEXT PRIMARY KEY,
                Title TEXT,
                Width INTEGER,
                Height INTEGER,
                Font TEXT,
                TextColor TEXT,
                BorderColor TEXT,
                BorderThickness REAL,
                CornerType TEXT,
                Category TEXT
            )";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public void SaveSettings(string title, int width, int height, string font, string textColor, string borderColor, double borderThickness, string cornerType, string category = "Settings", string? id = null)
        {
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();
            string triggerId = id ?? string.Empty;
            if (string.IsNullOrWhiteSpace(triggerId))
            {
                // Получить максимальный числовой id
                string maxIdSql = "SELECT MAX(CAST(Id AS INTEGER)) FROM TriggerSettings WHERE Id GLOB '[0-9]*'";
                using var maxIdCmd = new SQLiteCommand(maxIdSql, conn);
                object? maxIdObj = maxIdCmd.ExecuteScalar();
                int nextId = 1;
                if (maxIdObj != DBNull.Value && maxIdObj != null)
                {
                    int.TryParse(maxIdObj.ToString(), out nextId);
                    nextId++;
                }
                triggerId = nextId.ToString("D4");
                string insertSql = @"INSERT INTO TriggerSettings (Id, Title, Width, Height, Font, TextColor, BorderColor, BorderThickness, CornerType, Category) VALUES (@Id, @Title, @Width, @Height, @Font, @TextColor, @BorderColor, @BorderThickness, @CornerType, @Category)";
                using var insertCmd = new SQLiteCommand(insertSql, conn);
                insertCmd.Parameters.AddWithValue("@Id", triggerId);
                insertCmd.Parameters.AddWithValue("@Title", title);
                insertCmd.Parameters.AddWithValue("@Width", width);
                insertCmd.Parameters.AddWithValue("@Height", height);
                insertCmd.Parameters.AddWithValue("@Font", font);
                insertCmd.Parameters.AddWithValue("@TextColor", textColor);
                insertCmd.Parameters.AddWithValue("@BorderColor", borderColor);
                insertCmd.Parameters.AddWithValue("@BorderThickness", borderThickness);
                insertCmd.Parameters.AddWithValue("@CornerType", cornerType);
                insertCmd.Parameters.AddWithValue("@Category", category);
                insertCmd.ExecuteNonQuery();
            }
            else
            {
                string updateSql = @"UPDATE TriggerSettings SET Title = @Title, Width = @Width, Height = @Height, Font = @Font, TextColor = @TextColor, BorderColor = @BorderColor, BorderThickness = @BorderThickness, CornerType = @CornerType, Category = @Category WHERE Id = @Id";
                using var updateCmd = new SQLiteCommand(updateSql, conn);
                updateCmd.Parameters.AddWithValue("@Id", triggerId);
                updateCmd.Parameters.AddWithValue("@Title", title);
                updateCmd.Parameters.AddWithValue("@Width", width);
                updateCmd.Parameters.AddWithValue("@Height", height);
                updateCmd.Parameters.AddWithValue("@Font", font);
                updateCmd.Parameters.AddWithValue("@TextColor", textColor);
                updateCmd.Parameters.AddWithValue("@BorderColor", borderColor);
                updateCmd.Parameters.AddWithValue("@BorderThickness", borderThickness);
                updateCmd.Parameters.AddWithValue("@CornerType", cornerType);
                updateCmd.Parameters.AddWithValue("@Category", category);
                updateCmd.ExecuteNonQuery();
            }
        }

        public void DeleteTriggerById(string id)
        {
            using var conn = new System.Data.SQLite.SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();
            string sql = "DELETE FROM TriggerSettings WHERE Id = @Id";
            using var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public void DeleteAllTriggers()
        {
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();
            string sql = "DELETE FROM TriggerSettings";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public class TriggerData
        {
            public string Id { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public int Width { get; set; }
            public int Height { get; set; }
            public string Font { get; set; } = "Segoe UI";
            public string TextColor { get; set; } = "#FF000000";
            public string BorderColor { get; set; } = "#FF808080";
            public double BorderThickness { get; set; }
            public string CornerType { get; set; } = "Закругленные";
            public string Category { get; set; } = "Settings";
        }

        public IEnumerable<TriggerViewModel> GetAllTriggers(string categoryFilter = "")
        {
            var triggers = new List<TriggerViewModel>();
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();
            string sql = "SELECT Id, Title, Width, Height, Font, TextColor, BorderColor, BorderThickness, CornerType, Category FROM TriggerSettings";
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                sql += " WHERE Category = @Category";
            }
            using var cmd = new SQLiteCommand(sql, conn);
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                cmd.Parameters.AddWithValue("@Category", categoryFilter);
            }
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string id = reader["Id"].ToString() ?? Guid.NewGuid().ToString();
                string category = reader["Category"].ToString() ?? "";
                System.Diagnostics.Debug.WriteLine($"Trigger category in DB: '{category}'");
                var vm = new TriggerViewModel(reader["Title"].ToString() ?? "", id)
                {
                    Width = reader.GetInt32(reader.GetOrdinal("Width")),
                    Height = reader.GetInt32(reader.GetOrdinal("Height")),
                    Font = reader["Font"].ToString() ?? "Segoe UI",
                    TextColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(reader["TextColor"].ToString() ?? "#FF000000"),
                    BorderColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(reader["BorderColor"].ToString() ?? "#FF808080"),
                    BorderThickness = reader.GetDouble(reader.GetOrdinal("BorderThickness")),
                    CornerType = reader["CornerType"].ToString() ?? "Закругленные"
                };
                triggers.Add(vm);
            }
            return triggers;
        }
    }
}
