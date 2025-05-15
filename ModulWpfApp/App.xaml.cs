using System.Configuration;
using System.Data;
using System.Windows;

namespace ModulWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DataFolder = "Data";
        private const string DbFileName = "TriggerSettings.db";
        private const string DbBackupFileName = "TriggerSettings.db.bak";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Создать папку Data, если не существует
            string dataDir = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, DataFolder);
            if (!System.IO.Directory.Exists(dataDir))
            {
                System.IO.Directory.CreateDirectory(dataDir);
            }
            string dbPath = System.IO.Path.Combine(dataDir, DbFileName);
            string bakPath = System.IO.Path.Combine(dataDir, DbBackupFileName);

            // Если основной файл отсутствует, но есть бэкап — восстановить
            if (!System.IO.File.Exists(dbPath) && System.IO.File.Exists(bakPath))
            {
                System.IO.File.Copy(bakPath, dbPath, overwrite: true);
            }
            // Если основной файл есть — сделать бэкап
            if (System.IO.File.Exists(dbPath))
            {
                System.IO.File.Copy(dbPath, bakPath, overwrite: true);
            }
        }
    }
}
