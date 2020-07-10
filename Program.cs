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

namespace ProxyScraper {
	
	internal class Program {
		
		public static int proxiesScraped = 0;
		public static Status status = Status.IDLE;
		
		internal static ConsoleManager cm = null;
		private static Scraper s = null;
		internal static ProxyManager pm = null;
		
		//TODO:: optional search query settings?
		//TODO:: release both ProxyScraper dev(Debug) and ProxyScraper release(Release) and HtmlAgilityPack.dll in release
		
		public static void Main (string[] args) {
			
			init();
			cm.printBase();
			new Thread(s.scrape).Start();
			
		}
		
		private static void init () {
			
			pm = new ProxyManager();
			cm = new ConsoleManager();
			s = new Scraper();
			
			if (!(Directory.Exists("./proxies/"))) {
				
				Directory.CreateDirectory("./proxies/");
				
				if (!(File.Exists("./proxies/scraped.txt"))) File.Create("./proxies/scraped.txt");
				
				#if DEBUG
				if (!(File.Exists("./debug.txt"))) File.Create("./debug.txt");
				#endif
				
			}
			
		}
		
		public static void debug (string info) {
			
			#if DEBUG
			using (StreamWriter sw = File.AppendText("debug.txt"))
				sw.WriteLine(info);
			#endif
			
		}
		
	}
	
}