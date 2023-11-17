using System;
using System.IO;
using VideoLibrary;

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
\________________________/ Youtube Downloader Service
 */

namespace DeltaCORE
{
	public class YoutubeService : DeltaPackage.Services.IYoutubeService
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
