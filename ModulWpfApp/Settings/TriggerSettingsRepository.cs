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
                CornerType TEXT
            )";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public void SaveSettings(string title, int width, int height, string font, string textColor, string borderColor, double borderThickness, string cornerType, string? id = null)
        {
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();
            string triggerId = id ?? string.Empty;
            if (string.IsNullOrWhiteSpace(triggerId))
            {
                do
                {
                    triggerId = Guid.NewGuid().ToString();
                    string checkSql = "SELECT COUNT(*) FROM TriggerSettings WHERE Id = @Id";
                    using var checkCmd = new SQLiteCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@Id", triggerId);
                    var count = (long)checkCmd.ExecuteScalar();
                    if (count == 0) break;
                } while (true);
            }
            string sql = @"INSERT INTO TriggerSettings (Id, Title, Width, Height, Font, TextColor, BorderColor, BorderThickness, CornerType) VALUES (@Id, @Title, @Width, @Height, @Font, @TextColor, @BorderColor, @BorderThickness, @CornerType)";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", triggerId);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Width", width);
            cmd.Parameters.AddWithValue("@Height", height);
            cmd.Parameters.AddWithValue("@Font", font);
            cmd.Parameters.AddWithValue("@TextColor", textColor);
            cmd.Parameters.AddWithValue("@BorderColor", borderColor);
            cmd.Parameters.AddWithValue("@BorderThickness", borderThickness);
            cmd.Parameters.AddWithValue("@CornerType", cornerType);
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
        }

        public IEnumerable<TriggerViewModel> GetAllTriggers()
        {
            var triggers = new List<TriggerViewModel>();
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();
            string sql = "SELECT Id, Title, Width, Height, Font, TextColor, BorderColor, BorderThickness, CornerType FROM TriggerSettings";
            using var cmd = new SQLiteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var vm = new TriggerViewModel(reader["Title"].ToString() ?? "")
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
