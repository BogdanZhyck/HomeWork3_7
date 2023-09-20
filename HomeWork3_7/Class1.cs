using System;
using System.IO;
using Newtonsoft.Json;

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
		string configFile = "appConfig.json"; // Шлях до конфігураційного JSON-файлу

		if (!File.Exists(configFile))
		{
			Console.WriteLine("Помилка: Файл конфігурації не знайдено.");
			return 100; // Повернемо значення за замовчуванням, якщо файл не знайдено.
		}

		try
		{
			string json = File.ReadAllText(configFile);
			var config = JsonConvert.DeserializeObject<Config>(json);

			if (config != null)
			{
				return config.MaxEntriesPerBackup;
			}
			else
			{
				Console.WriteLine("Помилка: Не вдалося розпізнати конфігураційний файл.");
				return 100; // Повернемо значення за замовчуванням в разі невдачі зчитування файлу.
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Помилка при зчитуванні конфігураційного файлу: {ex.Message}");
			return 100; // Повернемо значення за замовчуванням в разі помилки.
		}
	}

	// Модель для зчитування JSON-конфігурації.
	private class Config
	{
		public int MaxEntriesPerBackup { get; set; }
	}
}
