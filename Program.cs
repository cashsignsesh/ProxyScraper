/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 7/9/2020
 * Time: 9:06 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace ProxyScraper {
	
	internal class Program {
		
		public static int proxiesScraped = 0;
		public static Status status = Status.IDLE;
		
		internal static ConsoleManager cm = null;
		private static Scraper s = null;
		internal static ProxyManager pm = null;
		
		public static List<string> settingsQueries = null;
		public static List<string> blacklist = null;
		
		public static int initialSearches = 3;
		
		//IMPORTANT TODO::
		//TODO PRIORITY #1: fix pinger, try different timeouts and multi thread
		
		//TODO:: fix formatted scraping (Try to go line by line of raw htm and detect ip then port on the next line, it will probably help) & fix that some connections don't work to websites(exceptions)
		//TODO:: add proxy checking after pinging, or replace entirely?
		//TODO:: make forms gui??? maybe implement ConsoleManager.cs into a richtextbox
		//TODO:: optimization tweaks, i.e no double checking dupes for no reason.
		
		//TODO:: comment out or #if DEBUG out getProxies() @ ProxyManager.cs at release
		//TODO:: release both ProxyScraper dev(Debug) and ProxyScraper release(Release) and HtmlAgilityPack.dll in release
		
		public static void Main (string[] args) {
			
			if (args.Length == 1) initialSearches = Int32.Parse(args[0]);
			
			debug("--- Started ProxyScraper: " + DateTime.Now + " ---");
			init();
			cm.printBase();
			new Thread(s.scrape).Start();
			while(true){}
			
		}
		
		private static void init () {
			
			pm = new ProxyManager();
			cm = new ConsoleManager();
			s = new Scraper();
			
			if (!(Directory.Exists("./proxies/"))) {
				
				Directory.CreateDirectory("./proxies/");
				
				if (!(File.Exists("./proxies/scraped.txt"))) File.Create("./proxies/scraped.txt").Close();
				if (!(File.Exists("./links.txt"))) File.Create("./links.txt").Close();
				if (!(File.Exists("./queries.txt"))) File.Create("./queries.txt").Close();
				if (!(File.Exists("./blacklist.txt"))) File.Create("./blacklist.txt").Close();
				
				#if DEBUG
				if (!(File.Exists("./debug.txt"))) File.Create("./debug.txt").Close();
				if (!(File.Exists("./proxies/debug_scraped.txt"))) File.Create("./proxies/debug_scraped.txt").Close();
				#endif
				
				File.WriteAllText("./queries.txt", Encoding.ASCII.GetString(cm.queriesDefault));
				
			}
			
			settingsQueries = new List<String>(File.ReadAllLines("./queries.txt"));
			blacklist = new List<String>(File.ReadAllLines("./blacklist.txt"));
			
		}
		
		public static void debug (string info) {
			
			#if DEBUG
			using (StreamWriter sw = File.AppendText("debug.txt"))
				sw.WriteLine(info);
			#endif
			
		}
		
	}
	
}