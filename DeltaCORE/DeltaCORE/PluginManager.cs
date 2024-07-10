using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
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
\________________________/ Plugin Manager
 */

namespace DeltaCORE
{
	public class PluginManager
	{
		public static List<Assembly> PluginList = new List<Assembly>();
		static readonly string PluginDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/DeltaCORE/Plugins/";

		public static void LoadPlugins()
		{
			Console.WriteLine("Loading Plugins! Please Wait...");
			foreach(string f in Directory.EnumerateFiles(PluginDirectory))
			{
				PluginList.Add(Assembly.LoadFrom(f));
			}
			Console.WriteLine("Plugins Loaded!");
		}

		public static void ListPlugins()
		{
			Console.WriteLine("Current Plugins:");
			foreach(Assembly a in PluginList)
			{
				Console.WriteLine(a.GetName().Name + " V" + a.GetName().Version);
			}
		}

		public static string GetPluginList()
		{
			StringBuilder list = new StringBuilder();
			foreach(Assembly a in PluginList)
			{
				list.Append(a.GetName().Name).Append(" V").Append(a.GetName().Version).Append("\n");
			}
			return list.ToString();
		}
	}
}
