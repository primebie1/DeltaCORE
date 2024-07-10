using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using IResult = Discord.Interactions.IResult;

namespace DeltaCORE
{
    internal class SlashCommandSystem
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _services;
        private readonly DeltaData _config;


        public SlashCommandSystem(DiscordSocketClient client, InteractionService interactionService, DeltaData data)
        {
            _client = client;
            _interactionService = interactionService;
            _config = data;
            _services = ConfigureServices();
        }

        public async Task InitInteractions()
        {
            _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            foreach (Assembly asm in PluginManager.PluginList)
            {
                await _interactionService.AddModulesAsync(asm, _services);
            }

            _client.InteractionCreated += HandleInteractionAsync;
            _client.Ready += _client_Ready;
            _interactionService.InteractionExecuted += InteractionExecuted;
        }

        private async Task _client_Ready()
        {
            LogMessage lm = new LogMessage(LogSeverity.Verbose, "SlashCommandSystem", "Registering Commands...");
            Program.Log(lm);
            await _interactionService.RegisterCommandsGloballyAsync();
        }

        private async Task HandleInteractionAsync(SocketInteraction arg)
        {
            var context = new SocketInteractionContext(_client, arg);
            var result = await _interactionService.ExecuteCommandAsync(context, _services);
            if (!result.IsSuccess)
                await arg.Channel.SendMessageAsync(result.ErrorReason);
        }

        private Task InteractionExecuted(ICommandInfo arg1, IInteractionContext arg2, IResult arg3)
        {
            string guildName = String.Empty;
            if (arg2.Guild != null)
            {
                guildName = $" In Guild: {arg2.Guild.Name}";
            }
            else
            {
                guildName = $" In DMs";
            }

            if (arg3.IsSuccess)
            {
                StringBuilder stringBuilder = new StringBuilder().Append("Module: ").Append(arg1.Module.Name).Append(" Command: ").Append(arg1.Name).Append(guildName).Append(" Channel: ").Append(arg2.Channel.Name).Append(" Executed Successfuly!");
                LogMessage msg = new LogMessage(LogSeverity.Info, "SlashCommandSystem", stringBuilder.ToString());
                return Program.Log(msg);
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder().Append("Command: ").Append(arg1.Name).Append(guildName).Append(" Channel: ").Append(arg2.Channel.Name).Append(" Failed! Reason: ").Append(arg3.ErrorReason);
                LogMessage msg = new LogMessage(LogSeverity.Info, "SlashCommandSystem", stringBuilder.ToString());
                return Program.Log(msg);
            }
        }


        private static IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection()
                // Repeat this for all the service classes
                // and other dependencies that commands might need.
                .AddSingleton((DeltaPackage.Services.IDataService)new DataService())
                .AddSingleton((DeltaPackage.Services.IAudioService)new AudioService())
                .AddSingleton((DeltaPackage.Services.IDECTalkService)new DECTalkService())
                .AddSingleton((DeltaPackage.Services.IYoutubeService)new YoutubeService());

            return map.BuildServiceProvider();
        }

    }
}
