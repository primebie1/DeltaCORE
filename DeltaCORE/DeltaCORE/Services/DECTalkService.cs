using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DeltaCORE
{
	public class DECTalkService
	{

		readonly string DECFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\DeltaCORE\DECTalk\";

		public bool DECInstalled()
		{
			return (File.Exists($"{DECFolder}say.exe"));
		}

		public string DECGenWav(string ID, string input)
		{
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
