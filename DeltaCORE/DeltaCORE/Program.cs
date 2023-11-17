﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
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
        private readonly DeltaData config;
        private CommandHandler commandHandler;

        public async Task MainAsync()
        {
            commandHandler = new CommandHandler(_client, _commands, config);
            await commandHandler.InitCommands();
            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Program()
        {
            WriteLogo();
            config = Startup.ConfigStart();
            PluginManager.LoadPlugins();
            PluginManager.ListPlugins();

            //configure SocketClient
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.GuildPresences
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
        }

        public static Task Log(LogMessage message)
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
            Console.Write(@"/  /_______________\     \");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($" DeltaCORE V{Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(@"\________________________/");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" © " + Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright);
        }
    }
}
