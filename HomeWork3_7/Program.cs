using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;



namespace HomeWork3_7
{
	using System;
	using System.Threading.Tasks;

	public class App
	{
		private static async Task Main(string[] args)
		{
			string logFilePath = "log.txt"; // Шлях до файлу журналу
			int maxEntriesPerBackup = ReadMaxEntriesFromConfig(); // Зчитуємо з конфігураційного JSON
			string backupFolderPath = "Backup"; // Шлях до папки для резервних копій

			Logger logger = new Logger(logFilePath, maxEntriesPerBackup, backupFolderPath);
			logger.LogBackupRequired += Logger_LogBackupRequired;

			async Task LogEntriesAsync(int count)
			{
				for (int i = 0; i < count; i++)
				{
					await logger.LogAsync($"Запис журналу {i + 1}");
				}
			}

			await Task.WhenAll(
				 LogEntriesAsync(50),
				 LogEntriesAsync(50)
			);
		}

		private static void Logger_LogBackupRequired(object sender, EventArgs e)
		{
			Console.WriteLine("Потрібно резервне копіювання журналу!");
		}

		private static int ReadMaxEntriesFromConfig()
		{
			// Реалізуйте логіку зчитування N з вашого конфігураційного JSON-файлу.
			// Поверніть значення за замовчуванням, якщо конфігурація недоступна.
			return 100; // Значення за замовчуванням
		}
	}
}
