using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord.API;
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
\________________________/ Audio Module
 */
namespace DeltaCORE
{
	public class AudSystem : ModuleBase<SocketCommandContext>
	{

		[Command("Join", RunMode = RunMode.Async)]
		[Summary("Joins current voice channel")]
		public async Task JoinAsync(IVoiceChannel channel = null)
		{
			channel ??= (Context.User as IGuildUser)?.VoiceChannel;
			if (channel == null) 
			{
				await Context.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument");
				return;
			}

			await channel.ConnectAsync();
		}
		[Command("Disconnect", RunMode = RunMode.Async)]
		[Summary("Disconnects from current voice channel")]
		public async Task LeaveAsync()
		{
			await Context.Guild.AudioClient.StopAsync();
		}

	}
}
