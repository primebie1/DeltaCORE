using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
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
\________________________/ Meme Module
 */
namespace DeltaCORE
{
	public class MemeModule :ModuleBase<SocketCommandContext>
	{
        private readonly DataService _dataserv;


        public MemeModule(DataService dataserv)
        {
            _dataserv = dataserv;
        }



        [Command("Smug")]
        [Summary("Smug Hat Kid")]
        public async Task SmugAsync()
        {
            Stream image = _dataserv.LoadMediaData("smug.gif");
            await Context.Channel.SendFileAsync(image, "smug.gif");
            image.Close();
        }
        
        [Command("Unlimited")]
        [Summary("Unlimited POWAH")]
        public async Task UnilmitedAsync()
        {
            Stream image = _dataserv.LoadMediaData("unlimited.gif");
            await Context.Channel.SendFileAsync(image, "unlimited.gif");
            image.Close();
        }
        



	}
}
