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

namespace ProxyScraper {
	
	internal class Program {
		
		public static int proxiesScraped = 0;
		
		public static void Main (string[] args) {
			
			init();
			
			Console.WriteLine("Proxy Scraper - " + DateTime.Now);
			Console.WriteLine("Press any key to begin");
			Console.WriteLine("Any data in ./proxies/scraped.txt will be overwritten.");
			Console.ReadKey();
			
		}
		
		private static void init () {
			
			Console.Title = "Proxy Scraper";
			
			if (!(Directory.Exists("./proxies/"))) {
				
				Directory.CreateDirectory("./proxies/");
				
				if (!(File.Exists("./proxies/scraped.txt"))) File.Create("./proxies/scraped.txt");
				
			}
			
		}
		
	}
	
}