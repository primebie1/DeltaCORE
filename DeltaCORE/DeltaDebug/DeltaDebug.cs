using DeltaPackage;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Interactions;
using System.Data;

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
\________________________/ Debug Plugin
 */

namespace DeltaDebug
{
	[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
	[RequireOwner(Group = "Permission")]
	public class DeltaDebug : DeltaSlashModule
	{
		[SlashCommand("servinfo", "Gathers Server Information")]
		public async Task ServInfoAsync()
		{
			SocketGuild guild = Context.Guild;
			await RespondAsync($"```Name: {guild.Name}\nID: {guild.Id}\nOwner: {guild.Owner.Username}\nUser Count: {guild.MemberCount}```");
		}

		[SlashCommand("userinfo", "Gathers User Information")]
		public async Task UsrInfoAsync(SocketGuildUser userin = null)
		{
			var guild = Context.Guild;
			var id = Context.User.Id;
			var defuser = guild.GetUser(id);
			var userout = userin ?? defuser;
			await RespondAsync($"```Name: {userout.Username}  \nGuild Nickname: {userout.Nickname}\nUser ID: {userout.Id}\nBot?: {userout.IsBot}\nStatus: {userout.Status}\nJoined At: {userout.JoinedAt}```");
        }

		[SlashCommand("roleinfo", "Displays who has a role")]
		public async Task RoleInfoAsync(SocketRole role)
		{
			string outlist = "";
			foreach (SocketGuildUser user in role.Members) { outlist += (user.Nickname + " (" + user.Username + "#" + user.Discriminator + ")" + "\n"); }
			await RespondAsync($"Users with Role {role.Name} \n ```{outlist}```");
		}
	}
}
