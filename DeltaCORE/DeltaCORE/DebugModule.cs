using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Discord.Commands;
using Discord.WebSocket;
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
\________________________/ Debug Module
 */
namespace DeltaCORE
{
	[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
	[RequireOwner(Group = "Permission")]
	public class DebugModule : ModuleBase<SocketCommandContext>
	{

		[Command("ServInfo")]
		[Summary("Gathers Server Information")]
		public async Task ServInfoAsync()
		{
			SocketGuild guild = Context.Guild;

			await ReplyAsync($"```Name: {guild.Name}\nID: {guild.Id}\nOwner: {guild.Owner.Username}\nUser Count: {guild.MemberCount}```");
		}

		[Command("UserInfo")]
		[Summary("Gathers User Information")]
		public async Task UsrInfoAsync(SocketGuildUser userin = null)
		{
			var guild = Context.Guild;
			var id = Context.User.Id;
			var defuser = guild.GetUser(id);

			var userout = userin ?? defuser;

			await ReplyAsync($"```Name: {userout.Username}  \nGuild Nickname: {userout.Nickname}\nUser ID: {userout.Id}\nBot?: {userout.IsBot}\nStatus: {userout.Status}\nJoined At: {userout.JoinedAt}```");
		}

		[Command("RoleInfo")]
		[Summary("Displays who has a role")]
		public async Task RoleInfoAsync(SocketRole role)
		{
			string outlist = "";
			foreach (SocketGuildUser user in role.Members) { outlist += (user.Nickname + " (" + user.Username + "#" + user.Discriminator + ")" + "\n"); }
			await ReplyAsync($"Users with Role {role.Name} \n ```{outlist}```");
		}

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
			await ReplyAsync($"```\n{deltaLogo} DeltaCORE\n\n/========General Info========\\ \nVersion {vernum}\nOwned by {app.Owner}\nBuilt With Discord.NET version {DiscordConfig.Version}\nRunning on {RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture} On {RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}\n\n/========Stats========\\ \nHeap Size: {GetHeapSize()}MiB\nGuilds Connected: {Context.Client.Guilds.Count}\nChannels: {Context.Client.Guilds.Sum(g=>g.Channels.Count)}\nUsers: {Context.Client.Guilds.Sum(g=>g.Users.Count)}\nUptime: {GetUptime()}\n```");
		}
		private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
		private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
	}
}
