using DeltaPackage;
using DeltaPackage.Services;
using DeltaPackage.Utilities;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
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
\________________________/ Utility Plugin
 */
namespace DeltaUtility
{
	public class DeltaUtility : DeltaSocketModule
	{
		private readonly IDataService _dataserv;

		public DeltaUtility(IDataService dataService)
		{
			_dataserv = dataService;
		}

		[Command("Role", RunMode = RunMode.Async)]
		[Summary("Adds or Removes a Setable Role")]
		public async Task RoleAsync(SocketRole role)
		{
			string servID = Context.Guild.Id.ToString();
			List<ulong> userRoles = new List<ulong>();

			foreach (SocketRole usrRole in Context.Guild.GetUser(Context.User.Id).Roles)
			{
				userRoles.Add(usrRole.Id);
			}


			if (_dataserv.CheckFile(servID))
			{
				GuildData data;
				data = _dataserv.LoadGuildData(servID);
				foreach (ulong roleID in data.Roles)
				{
					if (roleID == role.Id)
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
