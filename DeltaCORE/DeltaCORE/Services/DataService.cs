﻿using System;
using System.IO;
using System.Text.Json;
using DeltaPackage.Utilities;

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
	public class DataService : DeltaPackage.Services.IDataService
	{
		readonly string guildFolder;
		readonly string userFolder;
		readonly string mediaFolder;
		readonly string rootFolder;
		readonly JsonSerializerOptions options;

		public DataService()
		{
			rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			guildFolder = rootFolder + "/DeltaCORE/Guild/";
			userFolder = rootFolder + "/DeltaCORE/User/";
			mediaFolder = rootFolder + "/DeltaCORE/Media/";
			options = new JsonSerializerOptions
			{
				WriteIndented = true // write pretty json
			};
		}

		public string GetMediaFolder()
		{
			return mediaFolder;
		}

		public bool CheckFile(string name)
		{
			return (File.Exists(guildFolder + name + ".json") || File.Exists(userFolder + name + ".json") || File.Exists(mediaFolder + name));
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
		public void SaveGuildData(GuildData data)
		{
			string jsonSavString = JsonSerializer.Serialize(data, options);
			File.WriteAllText(guildFolder + data.Name + ".json", jsonSavString);
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

		public void DelFile(string path)
		{
			File.Delete(path);
		}
	}
}
