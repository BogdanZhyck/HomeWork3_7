using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class Logger
{
	private readonly string logFilePath;
	private readonly int maxEntriesPerBackup;
	private readonly string backupFolderPath;

	private List<string> logEntries = new List<string>();
	private int entryCount = 0;

	public event EventHandler LogBackupRequired;

	public Logger(string logFilePath, int maxEntriesPerBackup, string backupFolderPath)
	{
		this.logFilePath = logFilePath;
		this.maxEntriesPerBackup = maxEntriesPerBackup;
		this.backupFolderPath = backupFolderPath;

		if (!Directory.Exists(backupFolderPath))
		{
			Directory.CreateDirectory(backupFolderPath);
		}
	}

	public async Task LogAsync(string message)
	{
		logEntries.Add($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
		entryCount++;

		if (entryCount >= maxEntriesPerBackup)
		{
			entryCount = 0;
			await BackupLogAsync();
			OnLogBackupRequired();
		}
	}

	private async Task BackupLogAsync()
	{
		string backupFileName = $"{DateTime.Now:yyyyMMddHHmmss}.log";
		string backupFilePath = Path.Combine(backupFolderPath, backupFileName);

		await File.WriteAllLinesAsync(backupFilePath, logEntries);
		logEntries.Clear();
	}

	protected virtual void OnLogBackupRequired()
	{
		LogBackupRequired?.Invoke(this, EventArgs.Empty);
	}
}