using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Discord.Interactions;

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
\________________________/ Base Module
 */

namespace DeltaCORE.Modules
{
	public class BaseModule : InteractionModuleBase<SocketInteractionContext>
	{
		[SlashCommand("botinfo", "Displays info on bot")]
		public async Task BotInfoAsync()
		{
			var vernum = Assembly.GetEntryAssembly().GetName().Version;
			var app = await Context.Client.GetApplicationInfoAsync();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("```");
            sb.AppendLine($"           ____            /========General Info========\\ ");
			sb.AppendLine($"          /    \\           Version {vernum}");
            sb.AppendLine($"         /      \\          Owned by {app.Owner}");
            sb.AppendLine($"        /  _     \\         Built With Discord.NET version {DiscordConfig.Version}");
            sb.AppendLine($"       /  / \\     \\        Running on {RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture} On {RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}");
            sb.AppendLine($"      /  /   \\     \\");
            sb.AppendLine($"     /  /     \\     \\       /========Stats========\\ ");
            sb.AppendLine($"    /  /       \\     \\      Heap Size: {GetHeapSize()}MiB");
            sb.AppendLine($"   /  /         \\     \\     Guilds Connected: {Context.Client.Guilds.Count}");
            sb.AppendLine($"  /  /           \\     \\    Channels: {Context.Client.Guilds.Sum(g => g.Channels.Count)}");
            sb.AppendLine($" /  /             \\     \\   Users: {Context.Client.Guilds.Sum(g => g.Users.Count)}");
            sb.AppendLine($"/  /_______________\\     \\  Uptime: {GetUptime()}");
            sb.AppendLine($"\\________________________/  Plugin Number: {PluginManager.PluginList.Count()}");
            sb.AppendLine($"Plugins Installed:");
            sb.AppendLine($"{PluginManager.GetPluginList()}");
            sb.Append($"```");
			await RespondAsync(sb.ToString());
		}

		private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

		private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
	}
}
