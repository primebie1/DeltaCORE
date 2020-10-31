using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
	class BaseModule : ModuleBase<SocketCommandContext>
	{
		[Command("BotInfo")]
		[Summary("Displays info on bot")]
		public async Task BotInfoAsync()
		{
			string deltaLogo = @"           ____
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
/  /_______________\     \
\________________________/";
			var vernum = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
			var app = await Context.Client.GetApplicationInfoAsync();
			await ReplyAsync($"```\n{deltaLogo} DeltaCORE\n\n/========General Info========\\ \nVersion {vernum}\nOwned by {app.Owner}\nBuilt With Discord.NET version {DiscordConfig.Version}\nRunning on {RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture} On {RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}\n\n/========Stats========\\ \nHeap Size: {GetHeapSize()}MiB\nGuilds Connected: {Context.Client.Guilds.Count}\nChannels: {Context.Client.Guilds.Sum(g => g.Channels.Count)}\nUsers: {Context.Client.Guilds.Sum(g => g.Users.Count)}\nUptime: {GetUptime()}\nPlugin Number: {PluginManager.PluginList.Count()}\nPlugins Installed:\n{PluginManager.GetPluginList()}\n```");
		}
		private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
		private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

	}
}
