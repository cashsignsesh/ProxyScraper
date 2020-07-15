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
		
		public static List<string> settingsQueries;
		
		//TODO:: release both ProxyScraper dev(Debug) and ProxyScraper release(Release) and HtmlAgilityPack.dll in release
		//TODO:: fix timer for update time (see TODO:: @ ConsoleManager.cs)
		//TODO:: (maybe) search links already stored for more proxy-related links (href=contains("proxy"))?? if(node.tolower.contains("next page")) this._scrape(node.href)????
		//TODO:: fix pinger, try different timeouts and multi thread
		//TODO:: get better at scraping proxies from websites
		
		public static void Main (string[] args) {
			
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
				
				#if DEBUG
				if (!(File.Exists("./debug.txt"))) File.Create("./debug.txt").Close();
				if (!(File.Exists("./proxies/debug_scraped.txt"))) File.Create("./proxies/debug_scraped.txt").Close();
				#endif
				
				File.WriteAllText("./queries.txt", Encoding.ASCII.GetString(cm.queriesDefault));
				
			}
			
			settingsQueries = new List<String>(File.ReadAllLines("./queries.txt"));
			
		}
		
		public static void debug (string info) {
			
			#if DEBUG
			using (StreamWriter sw = File.AppendText("debug.txt"))
				sw.WriteLine(info);
			#endif
			
		}
		
	}
	
}