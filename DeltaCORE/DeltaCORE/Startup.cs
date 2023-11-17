using System;
using System.IO;
using System.Text.Json;

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
\________________________/ Startup and Config loader
 */

namespace DeltaCORE
{
	public class Startup
	{
		static readonly string confFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DeltaCORE/config.json";
		static readonly string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DeltaCORE/";

		static public DeltaData ConfigStart()
		{
			if (File.Exists(confFile))
			{
				return ConfLoad();
			}
			else
			{
				return ConfSetup();
			}
		}

		//First Time Setup
		static DeltaData ConfSetup()
		{
			DeltaData config = new DeltaData();
			string confFile = dataFolder + "config.json";

			//Create DeltaCORE file structure in "AppData\Roaming\DeltaCORE\" (Windows, will create in equivelent directories on Linux and MacOS)
			Directory.CreateDirectory(dataFolder);
			Directory.CreateDirectory(dataFolder + "/Guild/");
			Directory.CreateDirectory(dataFolder + "/User/");
			Directory.CreateDirectory(dataFolder + "/Media/");
			Directory.CreateDirectory(dataFolder + "/Plugins/");

			//Create config from user input
			Console.WriteLine("Welcome to DeltaCORE! This first run configuration will setup DeltaCORE to run on your system!");
			Console.WriteLine("Please input your Bot Token:");
			config.Token = Console.ReadLine();

			Console.WriteLine("Please Input your prefered prefix for commands:");
			config.Prefix = Console.ReadLine()[0];

			JsonSerializerOptions options = new JsonSerializerOptions
			{
				WriteIndented = true // write pretty json
			};

			//serialise and save settings for future launches
			string jsonstring = JsonSerializer.Serialize(config, options);
			File.WriteAllText(confFile, jsonstring);
			return config;
		}

		//load configuration from file
		static DeltaData ConfLoad()
		{
			//load master config
			string jsonstring = File.ReadAllText(confFile);
			return JsonSerializer.Deserialize<DeltaData>(jsonstring);
		}
	}
}
