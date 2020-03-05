using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using VideoLibrary;

namespace DeltaCORE
{
	public class YoutubeService
	{

		readonly string MediaFolder = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\DeltaCORE\Media\";
		YouTube youTube = YouTube.Default;
		public string YTDownload(string url)
		{
			var video = youTube.GetVideo(url);
			string fullName = video.FullName;
			byte[] vidByte = video.GetBytes();

			File.WriteAllBytes(MediaFolder + fullName, vidByte);

			return MediaFolder + fullName;
		}
	}
}
