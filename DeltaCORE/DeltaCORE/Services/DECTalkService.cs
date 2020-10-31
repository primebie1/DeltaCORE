using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Discord;
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
\________________________/ DECTalk Text To Speech Service
 */

namespace DeltaCORE
{
	public class DECTalkService : DeltaPackage.Services.IDECTalkService
	{

		readonly string DECFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DeltaCORE/DECTalk/";

		public bool DECInstalled()
		{

			if (File.Exists($"{DECFolder}say.exe"))
			{
				return true;
			}
			else
			{
				LogMessage msg = new LogMessage(LogSeverity.Warning, "DECTalkServ", "DECTalk not found!");
				Program.Log(msg);
				return false;
			}

		}

		public string DECGenWav(string ID, string input)
		{
			LogMessage msg = new LogMessage(LogSeverity.Verbose, "DECTalkServ", "DECTalk Generating Speech for " + ID);
			Program.Log(msg);
			//Console.WriteLine($"{DECFolder}{ID}.wav");
			//Console.WriteLine($"{DECFolder}say.exe");
			using (Process DECProcess = new Process())
			{
				DECProcess.StartInfo.WorkingDirectory = DECFolder;
				DECProcess.StartInfo.UseShellExecute = false;
				DECProcess.StartInfo.FileName = $"{DECFolder}say.exe";
				DECProcess.StartInfo.Arguments = $" -w {ID}.wav {input}";
				DECProcess.StartInfo.CreateNoWindow = true;

				DECProcess.Start();
				//Console.WriteLine($"{DECProcess.StartInfo.FileName}{DECProcess.StartInfo.Arguments}");
				DECProcess.WaitForExit();
			}
			return $"{DECFolder}{ID}.wav";
		}


	}
}
