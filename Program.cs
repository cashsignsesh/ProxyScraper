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
		public static string status = "Idle";
		
		private static ConsoleManager cm = null;
		private static Scraper s = null;
		private static List<String> proxies = null;
		
		public static void Main (string[] args) {
			
			init();
			cm.printBase();
			new Thread(s.scrape).Start();
			
		}
		
		private static void init () {
			
			proxies = new List<String>();
			cm = new ConsoleManager();
			s = new Scraper();
			
			if (!(Directory.Exists("./proxies/"))) {
				
				Directory.CreateDirectory("./proxies/");
				
				if (!(File.Exists("./proxies/scraped.txt"))) File.Create("./proxies/scraped.txt");
				
			}
			
		}
		
	}
	
}