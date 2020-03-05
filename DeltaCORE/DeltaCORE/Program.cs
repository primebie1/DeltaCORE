using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using Discord.API;
using Discord.Audio;
using Discord.Rest;
using Discord.Webhook;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection; 
using System.Runtime.InteropServices;

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
\________________________/ A Discord Bot
 */

namespace DeltaCORE
{

    class Program
    {
        public static void Main()
            => new Program().MainAsync().GetAwaiter().GetResult();



        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
		private DeltaData config = new DeltaData();
		private readonly string dataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DeltaCORE\";


		public async Task MainAsync()
        {
			
            await InitCommands();

            await _client.LoginAsync(TokenType.Bot, config.token);
            await _client.StartAsync();


			// Block this task until the program is closed.
			await Task.Delay(-1);
        }
        private Program()
        {
			WriteLogo(); 
			string confFile = dataFolder + "config.json";
			if (File.Exists(confFile))
			{
				ConfLoad();
			}
			else
			{
				ConfSetup();


			}
            //configure SocketClient
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
				
                // If you or another service needs to do anything with messages
                // (eg. checking Reactions, checking the content of edited/deleted messages),
                // you must set the MessageCacheSize. You may adjust the number as needed.
                //MessageCacheSize = 50,

            });
            //Create and configure Command Service
            _commands = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,

                CaseSensitiveCommands = false,
            });

            _client.Log += Log;
            _commands.Log += Log;
            _services = ConfigureServices();
        }


        //First Time Setup
		void ConfSetup()
		{	
			string confFile = dataFolder + "config.json";
            //create DeltaCORE file structure in "AppData\Roaming\DeltaCORE\"
			Directory.CreateDirectory(dataFolder);
			Directory.CreateDirectory(dataFolder + @"\Guild\");
			Directory.CreateDirectory(dataFolder + @"\User\");
            Directory.CreateDirectory(dataFolder + @"\Media\");
			//create config from user input
			Console.WriteLine("Welcome to DeltaCORE! This first run configuration will setup DeltaCORE to run on your system!");
			Console.WriteLine("Please input your Bot Token:");
			config.token = Console.ReadLine();
			Console.WriteLine("Please Input your prefered prefix for commands:");
			config.prefix = Console.ReadLine()[0];
			
			JsonSerializerOptions options = new JsonSerializerOptions
			{      
				WriteIndented = true // write pretty json
			};
            //serialise and save settings for future launches
			string jsonstring = JsonSerializer.Serialize(config,options);
			File.WriteAllText(confFile, jsonstring);
		}

        //load configuration from file
		void ConfLoad()
		{
			string confFile = dataFolder + "config.json";
			//load master config

			string jsonstring = File.ReadAllText(confFile);
			config = JsonSerializer.Deserialize<DeltaData>(jsonstring);
		}

		private static IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection()
                // Repeat this for all the service classes
                // and other dependencies that your commands might need.
                .AddSingleton(new CommandService())
                .AddSingleton(new DataService())
                .AddSingleton(new AudioService())
                .AddSingleton(new DECTalkService())
                .AddSingleton(new YoutubeService());
	

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
            return map.BuildServiceProvider();
        }

        private async Task InitCommands()
        {
            // Either search the program and add all Module classes that can be found.
            // Module classes MUST be marked 'public' or they will be ignored.
            // You also need to pass your 'IServiceProvider' instance now,
            // so make sure that's done before you get here.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
           
            // Subscribe a handler to see if a message invokes a command.
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Bail out if it's a System Message.
            if (!(arg is SocketUserMessage msg)) return;

            // We don't want the bot to respond to itself or other bots.
            if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) return;

            // Create a number to track where the prefix ends and the command begins
            int pos = 0;
            //check for prefix
            if (msg.HasCharPrefix(config.prefix, ref pos))
            {
                // Create a Command Context.
                var context = new SocketCommandContext(_client, msg);
                
                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed successfully).
                var result = await _commands.ExecuteAsync(context, pos, _services);

                // Uncomment the following lines if you want the bot
                // to send a message if it failed.
                // This does not catch errors from commands with 'RunMode.Async',
                // subscribe a handler for '_commands.CommandExecuted' to see those.
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private static Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }


		public void WriteLogo()
		{
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine(@"           ____");
			Console.WriteLine(@"          /    \");
			Console.WriteLine(@"         /      \");
			Console.WriteLine(@"        /  _     \");
			Console.WriteLine(@"       /  / \     \");
			Console.WriteLine(@"      /  /   \     \");
			Console.WriteLine(@"     /  /     \     \");
			Console.WriteLine(@"    /  /       \     \");
			Console.WriteLine(@"   /  /         \     \");
			Console.WriteLine(@"  /  /           \     \");
			Console.WriteLine(@" /  /             \     \");
			Console.Write(    @"/  /_______________\     \");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine($" DeltaCORE V{ Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.Write(    @"\________________________/");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine(" © Harri Cound 2019-2020");

		}

	}
    public static class OperatingSystem
    {
        public static bool IsWindows() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }

}
