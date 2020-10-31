using DeltaPackage;
using DeltaPackage.Services;
using Discord;
using Discord.Commands;
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
\________________________/ Audio Plugin
 */
namespace DeltaAudio
{
	public class DeltaAudio : DeltaSocketModule
	{

		private readonly IAudioService _audserv;
		private readonly IDECTalkService _DECserv;
		private readonly IDataService _dataserv;
		private readonly IYoutubeService _ytserv;
		public DeltaAudio(IAudioService audioService, IDECTalkService dECTalkService, IDataService dataService, IYoutubeService YTServ)
		{
			_audserv = audioService;
			_DECserv = dECTalkService;
			_dataserv = dataService;
			_ytserv = YTServ;
		}


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

		[Command("Leave", RunMode = RunMode.Async)]
		[Summary("Disconnects from current voice channel")]
		public async Task LeaveAsync()
		{
			await Context.Guild.AudioClient.StopAsync();
		}

		[Command("DECSay", RunMode = RunMode.Async)]
		[Summary("Uses DECTalk to say a sentence")]
		public async Task DECSayAsync(string inpt)
		{
			if (_DECserv.DECInstalled())
			{
				if (_audserv.FFmpegInstalled())
				{
					if (Context.Guild.AudioClient != null)
					{
						string wavLoc = _DECserv.DECGenWav(Context.Guild.Id.ToString(), inpt);

						await _audserv.SendAsync(Context.Guild.AudioClient, wavLoc);

						//_dataserv.DelFile(wavLoc);
					}
					else
					{
						await ReplyAsync("Please join a voice channel!");
					}
				}
				else
				{
					await ReplyAsync(@"FFmpeg not present in DeltaCORE\ffmpeg\");
				}
			}
			else
			{
				await ReplyAsync("DECTalk not present in DeltaCORE/DECTalk/");
			}
		}

		//[Command("TestAud",RunMode = RunMode.Async)]
		//[Summary("Plays a Test Audio File")]
		//public async Task TestAud()
		//{
		//	if (_audserv.FFmpegInstalled())
		//	{
		//		if (Context.Guild.AudioClient != null)
		//		{
		//			await _audserv.SendAsync(Context.Guild.AudioClient, $"{_dataserv.GetMediaFolder()}Electricity.mp3");
		//		}
		//		else
		//		{
		//			await ReplyAsync("Please join a voice channel!");
		//		}
		//	}
		//	else
		//	{
		//		await ReplyAsync(@"FFmpeg not present in DeltaCORE\ffmpeg\");

		//	}
		//}

		[Command("Play", RunMode = RunMode.Async)]
		[Summary("Plays a video from YouTube")]
		public async Task PlayAsync(string url)
		{
			if (_audserv.FFmpegInstalled())
			{
				if (Context.Guild.AudioClient != null)
				{
					string file = _ytserv.YTDownload(url);

					await _audserv.SendAsync(Context.Guild.AudioClient, file);
				}
				else
				{
					await ReplyAsync("Please join a voice channel!");
				}
			}
			else
			{
				await ReplyAsync(@"FFmpeg not present in DeltaCORE\ffmpeg\");

			}
		}

	}
}

