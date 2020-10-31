using Discord.Audio;
using System.Threading.Tasks;

namespace DeltaCORE
{
	public interface IAudioService
	{
		bool FFmpegInstalled();
		Task SendAsync(IAudioClient client, string path);
	}
}