using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

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
\________________________/ General User Utility Module
 */
namespace DeltaCORE
{
	public class UtilityModule :ModuleBase<SocketCommandContext>
	{
		private readonly DataService _dataserv;

		public UtilityModule(DataService dataService)
		{
			_dataserv = dataService;
		}

		[Command("Role",RunMode = RunMode.Async)]
		[Summary("Adds or Removes a Setable Role")]
		public async Task RoleAsync(SocketRole role)
		{
			string servID = Context.Guild.Id.ToString();
			List<ulong> userRoles = new List<ulong>();

			foreach(SocketRole usrRole in Context.Guild.GetUser(Context.User.Id).Roles)
			{
				userRoles.Add(usrRole.Id);
			}
			

			if (_dataserv.CheckFile(servID))
			{
				GuildData data;
				data = _dataserv.LoadGuildData(servID);
				foreach(ulong roleID in data.roles)
				{
					if(roleID == role.Id)
					{

							if (userRoles.Contains(role.Id))
							{
								await Context.Guild.GetUser(Context.User.Id).RemoveRoleAsync(role);
								await ReplyAsync($"Removed {role.Name}!");

							}
							else
							{
								await Context.Guild.GetUser(Context.User.Id).AddRoleAsync(role);
								await ReplyAsync($"Added {role.Name}!");
							}
					}
				}
			}
			else
			{
				await ReplyAsync("Error! No Server Configuration Detected! Please add a role to server configuration!");
			}
		}
	}
}
