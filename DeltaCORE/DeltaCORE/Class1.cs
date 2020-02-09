using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
\________________________/ General Module
 */
namespace DeltaCORE
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        => ReplyAsync(echo);

        /*
        [Command("Roll")]
        [Summary("Rolls dice")]
        public async Task RollAsync([Remainder] [Summary("Dice to roll")] string dice)
        {
            
            int die = Convert.ToInt32(dice);
            Random r = new Random();
            int result = r.Next(1,die+1);
            string outpt = Convert.ToString(result);
            await Context.Channel.SendMessageAsync(outpt);
            
        }
        */

        [Command("Roll")]
        [Summary("Rolls dice")]
        public async Task RollAsync( [Summary("Dice to roll")] int dice = 6, int dicenum = 1)
        {
            int die = Convert.ToInt32(dice);
            //Console.WriteLine(die);
            int diecount = Convert.ToInt32(dicenum);
            //Console.WriteLine(dicenum);
            Random r = new Random();
            int[] result = new int[diecount];
            //Console.WriteLine(result[0]);
            for (int i = 0; i < diecount; i++)
            {
                //Console.WriteLine(i);
                result[i] = 0;
            }
            string outpt = "";
            int tot = 0;
            for (int i=0; i < diecount; i++)
            {
                result[i] = r.Next(1, die+1);
                outpt += Convert.ToString(result[i])+" ";
                tot += result[i];
            }
            outpt += " Totaling " + Convert.ToString(tot);
                //= r.Next(1, die);
            
            //string outpt = Convert.ToString(result);
            await Context.Channel.SendMessageAsync(outpt);
        }

        [Command("s")]
        [Summary("Squek")]
        public async Task SquekAsync()
        {
            Random r = new Random();
            int result = r.Next(0, 6);
            var res = result switch
            {
                0 => "Squek",
                1 => "Squheck",
                2 => "Squek 2.0",
                3 => "MAXIMUM SQUEK",
                4 => "Squekh",
                5 => "Squok",
                6 => "smoll squek",
                _ => "Error! Invalid switch case!",
            };
            await Context.Channel.SendMessageAsync(res);
        }

        [Command("Choose")]
        [Summary("Choose between two options")]
        public async Task ChooseAsync([Summary("Choices")] string choice1, string choice2)
        {
            Random r = new Random();
            int result = r.Next(0, 2);
            var res = result switch
            {
                0 => choice1,
                1 => choice2,
                _ => "Error! Invalid switch case!",
            };
            await Context.Channel.SendMessageAsync("I Choose "+res);
        }

        [Command("Choose")]
        [Summary("Choose between multiple options")]
        public async Task ChooseAsync([Summary("Choices")] params string[] choices)
        {
            int length = choices.Length;
            Random r = new Random();
            int result = r.Next(0, length+1);
            string res = "Error! Invalid switch case!";
            for (int i = 0; i <= length; i++)
            {
                if (result == i)
                {
                    res = choices[i];
                    i = length + 1;
                }
            }
            /*
            switch (result)
            {
                case 0:
                    res = choice1;
                    break;
                case 1:
                    res = choice2;
                    break;
                default:
                    res = "shit done fucked mate";
                    break;
            }
            */
            await Context.Channel.SendMessageAsync("I Choose " + res);
        }
        [Command("Boop")]
        [Summary("Boop!")]
        public async Task BoopAsync()
        {
           await ReplyAsync("Boop!");
        }

		[Command("Hug")]
		[Summary("Hug!")]
		public async Task HugAsync(SocketGuildUser userin = null)
		{
			var guild = Context.Guild;
			var id = Context.User.Id;
			var defuser = guild.GetUser(id);

			var userout = userin ?? defuser;

			await ReplyAsync("*Hugs "+userout.Nickname+"!*");
		}
    
        [Command("OwO")]
        [Summary("OwO Whats This?")]
        public async Task OwOAsync()
        {
            Random r = new Random();
            int result = r.Next(0, 5);
            var res = result switch
            {
                0 => "OwO",
                1 => "UwU",
                2 => "UmU",
                3 => "Lowod",
                4 => "OnO",
                5 => ":3",
                _ => "Error! Invalid switch case!",
            };
            await Context.Channel.SendMessageAsync(res);
        }

	}
}
