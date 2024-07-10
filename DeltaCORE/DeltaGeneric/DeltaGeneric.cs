using DeltaPackage;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Discord.Interactions;

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
\________________________/ Generic Plugin
 */

namespace DeltaGeneric
{
	public class DeltaGeneric : DeltaSlashModule
	{

		[SlashCommand("say", "Echoes a message.")]
		public Task SayAsync(string echo)
		=> RespondAsync(echo);

		[SlashCommand("roll", "Rolls dice")]
		public async Task RollAsync(int dice = 6, int dicenum = 1)
		{
			int die = Convert.ToInt32(dice);
			int diecount = Convert.ToInt32(dicenum);
			Random r = new Random();
			int[] result = new int[diecount];
			for (int i = 0; i < diecount; i++)
			{
				result[i] = 0;
			}

			string outpt = "";
			int tot = 0;
			for (int i = 0; i < diecount; i++)
			{
				result[i] = r.Next(1, die + 1);
				outpt += Convert.ToString(result[i]) + " ";
				tot += result[i];
			}

			outpt += " Totaling " + Convert.ToString(tot);
			await RespondAsync(outpt);
		}

		[SlashCommand("s", "Squek")]
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
			await RespondAsync(res);
		}

		//[SlashCommand("Choose","Choose between multiple options")]
		//public async Task ChooseAsync([Summary("Choices")] string[] choices)
		//{
		//	int length = choices.Length;
		//	Random r = new Random();
		//	int result = r.Next(0, length + 1);
		//	string res = "Error! Invalid switch case!";
		//	for (int i = 0; i <= length; i++)
		//	{
		//		if (result == i)
		//		{
		//			res = choices[i];
		//			i = length + 1;
		//		}
		//	}
		//	await RespondAsync($"I Choose {res}!");
		//}

		[SlashCommand("boop", "Boop!")]
		public async Task BoopAsync()
		{
			await RespondAsync("Boop!");
		}

		[SlashCommand("hug", "Hug!")]
		public async Task HugAsync(SocketGuildUser userin = null)
		{
			var guild = Context.Guild;
			var id = Context.User.Id;
			var defuser = guild.GetUser(id);
			var userout = userin ?? defuser;
			await RespondAsync($"*Hugs {userout.Nickname}!*");
		}

		[SlashCommand("owo", "OwO Whats This?")]
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
			await RespondAsync(res);
		}
	}
}

