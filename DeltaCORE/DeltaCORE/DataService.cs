using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Text;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
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
\________________________/ Data Storage Service
 */
namespace DeltaCORE
{
	public class DataService
	{
		private readonly string guildFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DeltaCORE\Guild\";
		private readonly string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DeltaCORE\User\";
		private readonly string mediaFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DeltaCORE\Media\";
		readonly JsonSerializerOptions options = new JsonSerializerOptions
		{
			WriteIndented = true // write pretty json
			
			
		};
		
		public bool CheckFile(string name)
		{
			return (File.Exists(guildFolder + name + ".json") || File.Exists(userFolder + name+".json") || File.Exists(mediaFolder + name));
		}
		public void MakeGuildFile(string name)
		{
			File.Create(guildFolder + name + ".json").Close();
		}		
		public void MakeUserFile(string name)
		{
			File.Create(userFolder + name + ".json").Close();
		}

		//save given GuildData to "DeltaCORE\Guild\" in its JSON file
		public async Task SaveGuildData(GuildData data)
		{
			//Console.WriteLine(data.name + " " + data.roles);
			string jsonSavString = JsonSerializer.Serialize(data, options);
			//Console.WriteLine(jsonSavString);
			File.WriteAllText(guildFolder + data.name + ".json", jsonSavString);			
		}
		//load guild data from its JSON file in "DeltaCORE\Guild\"
		public GuildData LoadGuildData(string name)
		{
			string jsonReadString = File.ReadAllText(guildFolder + name + ".json");

			return JsonSerializer.Deserialize<GuildData>(jsonReadString);

		}

		public Stream LoadMediaData(string name)
		{
			return File.Open(mediaFolder + name, FileMode.Open);
		}
	}
	public class GuildData
	{
		public string name { get; set; }
		public List<ulong> roles { get; set; } = new List<ulong>();

	}
}
