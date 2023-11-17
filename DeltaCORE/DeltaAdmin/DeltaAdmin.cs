using DeltaPackage;
using DeltaPackage.Services;
using DeltaPackage.Utilities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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
\________________________/ Administration Plugin
 */

namespace DeltaAdmin
{
	[RequireUserPermission(GuildPermission.Administrator, Group = "Permission")]
	[RequireOwner(Group = "Permission")]
	public class DeltaAdmin : DeltaSocketModule
	{
		private readonly IDataService _dataserv;

		public DeltaAdmin(IDataService dataserv)
		{
			_dataserv = dataserv;
		}

		[Command("Kick")]
		[Summary("Kicks a User")]
		public async Task KickAsync(SocketGuildUser userin)
		{
			await userin.KickAsync();
		}

		[Command("Ban")]
		[Summary("Bans a User (User, Reason(in quotations), (OPTIONAL) Days of Messages to Prune (default = 0)")]
		public async Task BanAsync(SocketGuildUser user, string reason, int prune = 0)
		{
			await user.BanAsync(prune, reason);
		}

		[Command("RoleSet")]
		[Summary("Make Role User Settable")]
		public async Task RoleSetAsync(params SocketRole[] roles)
		{
			string name = Context.Guild.Id.ToString();
			GuildData data = new GuildData();

			if (!_dataserv.CheckFile(name))
			{
				_dataserv.MakeGuildFile(name);
				data.Name = name;
			}
			else
			{
				data = _dataserv.LoadGuildData(name);
			}

			foreach (SocketRole role in roles)
			{
				data.Roles.Add(role.Id);
				await ReplyAsync($"Set Role {role.Name} to self-apply!");
			}
			_dataserv.SaveGuildData(data);
		}

		[Command("RoleUnset")]
		[Summary("Make Role User Settable")]
		public async Task RoleUnSetAsync(params SocketRole[] roles)
		{
			string name = Context.Guild.Id.ToString();
			GuildData data = new GuildData();

			if (!_dataserv.CheckFile(name))
			{
				await ReplyAsync("Error! No Server Configuration Detected! Please add a role to server configuration!");
			}
			else
			{
				data = _dataserv.LoadGuildData(name);
			}

			foreach (SocketRole role in roles)
			{
				if (data.Roles.Contains(role.Id))
				{
					data.Roles.Remove(role.Id);
					await ReplyAsync($"Unset Role {role.Name} from self-apply!");
				}
			}
			_dataserv.SaveGuildData(data);
		}

		[Command("RoleAdd")]
		[Summary("Create a new role")]
		public async Task RoleAddAsync(string name)
		{
			await Context.Guild.CreateRoleAsync(name, null, null, false, false, null);
			await ReplyAsync($"Role {name} Created!");
		}
	}
}
