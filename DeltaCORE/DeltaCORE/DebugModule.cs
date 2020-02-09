using System;
using System.Diagnostics;
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

			await ReplyAsync("```Name: " + guild.Name + "\n" + "ID: " + guild.Id + "\n" + "Owner: " + guild.Owner.Username + "\nUser Count: " + guild.MemberCount + "```");
		}

		[Command("UserInfo")]
		[Summary("Gathers User Information")]
		public async Task UsrInfoAsync(SocketGuildUser userin = null)
		{
			var guild = Context.Guild;
			var id = Context.User.Id;
			var defuser = guild.GetUser(id);

			var userout = userin ?? defuser;

			await ReplyAsync("```Name: " + userout.Username + "\n" + "Guild Nickname: " + userout.Nickname + "\n" + "User ID: " + userout.Id + "\n" + "Bot?: " + userout.IsBot + "\n" + "Status: " + userout.Status + "\n" + "Joined At: " + userout.JoinedAt + "\n" + "```");
		}

		[Command("RoleInfo")]
		[Summary("Displays who has a role")]
		public async Task RoleInfoAsync(SocketRole role)
		{
			string outlist = "";
			foreach (SocketGuildUser user in role.Members) { outlist += (user.Nickname + " (" + user.Username + "#" + user.Discriminator + ")" + "\n"); }
			await ReplyAsync("Users with Role " + role.Name + "\n```" + outlist + "```");
		}
		
		[Command("BotInfo")]
		[Summary("Displays info on bot")]
		public async Task BotInfoAsync()
		{

			var vernum = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			var app = await Context.Client.GetApplicationInfoAsync();
			await ReplyAsync("```\n" +
@"           ____
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
\________________________/ DeltaCORE"+"\n"+
"General Info:\n"+
"Version "+vernum+"\n"+"Owned by "+ app.Owner+app.Owner.DiscriminatorValue+"\n"+"Built With Discord.NET version "+DiscordConfig.Version+"\n"+"Running on "+RuntimeInformation.FrameworkDescription+" "+RuntimeInformation.ProcessArchitecture+" On "+RuntimeInformation.OSDescription+" "+RuntimeInformation.OSArchitecture+"\n"+
"Stats: \n"+
"Heap Size: "+GetHeapSize()+"MiB\n"+"Guilds Connected: "+Context.Client.Guilds.Count+"\n"+"Channels: "+Context.Client.Guilds.Sum(g=>g.Channels.Count)+"\n"+"Users: "+Context.Client.Guilds.Sum(g=>g.Users.Count)+"\n"+"Uptime: "+GetUptime()+"\n"+
"\n```");
		}
		private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
		private static string GetUptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
	}
}
