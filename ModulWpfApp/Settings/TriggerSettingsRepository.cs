using System;
using System.Data.SQLite;
using System.IO;

namespace ModulWpfApp.Settings
{
    public class TriggerSettingsRepository
    {
        private readonly string _dbPath;
        public TriggerSettingsRepository()
        {
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TriggerSettings.db");
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

        public void SaveSettings(int width, int height, string font, string textColor, string borderColor, double borderThickness, string cornerType, string? id = null)
        {
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();
            // Генерируем уникальный id, если не передан
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
            string sql = @"INSERT INTO TriggerSettings (Id, Width, Height, Font, TextColor, BorderColor, BorderThickness, CornerType) VALUES (@Id, @Width, @Height, @Font, @TextColor, @BorderColor, @BorderThickness, @CornerType)";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", triggerId);
            cmd.Parameters.AddWithValue("@Width", width);
            cmd.Parameters.AddWithValue("@Height", height);
            cmd.Parameters.AddWithValue("@Font", font);
            cmd.Parameters.AddWithValue("@TextColor", textColor);
            cmd.Parameters.AddWithValue("@BorderColor", borderColor);
            cmd.Parameters.AddWithValue("@BorderThickness", borderThickness);
            cmd.Parameters.AddWithValue("@CornerType", cornerType);
            cmd.ExecuteNonQuery();
        }
    }
}
