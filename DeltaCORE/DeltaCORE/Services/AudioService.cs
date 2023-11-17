using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using Discord.Audio;
using Discord;

/*
		   ____
		  /    \
		 /      \
		/  _     \
	   /  / \     \
	  /  /   \     \
	 /  /     \     \
	/  /       \     \
   /  /         \     \
  /  /           \     \
 /  /             \     \
/  /_______________\     \ DeltaCORE
\________________________/ Audio Service
 */

namespace DeltaCORE
{
	public class AudioService : DeltaPackage.Services.IAudioService
	{
		readonly string FFmpegFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DeltaCORE/ffmpeg/";

		private Process CreateStream(string path)
		{
			return Process.Start(new ProcessStartInfo
			{
				FileName = $"{FFmpegFolder}ffmpeg.exe",
				Arguments = $"-hide_banner -loglevel info -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
				UseShellExecute = false,
				RedirectStandardOutput = true,
			});
		}

		public async Task SendAsync(IAudioClient client, string path)
		{
			LogMessage msg = new LogMessage(LogSeverity.Verbose, "AudioServ", "Sending Audio File " + path + " For " + client.ToString());
			await client.SetSpeakingAsync(true);
			using var ffmpeg = CreateStream(path);
			using var output = ffmpeg.StandardOutput.BaseStream;
			using var discord = client.CreatePCMStream(AudioApplication.Mixed, 48000, 500);
			try
			{
				await output.CopyToAsync(discord);
			}
			finally
			{
				await discord.FlushAsync();
				await output.DisposeAsync();
				output.Close();
				await client.SetSpeakingAsync(false);
			}
		}

		public bool FFmpegInstalled()
		{
			if (File.Exists($"{FFmpegFolder}ffmpeg.exe"))
			{
				return true;
			}
			else
			{
				LogMessage msg = new LogMessage(LogSeverity.Warning, "AudioServ", "FFmpeg not found!");
				Program.Log(msg);
				return false;
			}
		}
	}
}
