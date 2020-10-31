using System.IO;

namespace DeltaCORE
{
	public interface IDataService
	{
		bool CheckFile(string name);
		void DelFile(string path);
		GuildData LoadGuildData(string name);
		Stream LoadMediaData(string name);
		void MakeGuildFile(string name);
		void MakeUserFile(string name);
		void SaveGuildData(GuildData data);
		string GetMediaFolder();
	}
}