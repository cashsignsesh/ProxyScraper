/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 7/9/2020
 * Time: 9:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;

namespace ProxyScraper {
	
	/// <summary>
	/// Manage console
	/// </summary>
	public class ConsoleManager {
		
		private readonly string user = null;
		private readonly string title = null;
		private int secondsElapsed = 0;
		
		public ConsoleManager () {
			
			this.user = Environment.UserName;
			this.title = "Proxy Scraper - " + this.user;
			Console.Title = title;
			Console.CursorVisible = false;
			
			new Timer(state => updateTime(), null, 1000, 1000);
			
		}
		
		public void printBase () {
			
			Console.WriteLine(title + " - " + DateTime.Now + " (Time Elapsed: " + secondsElapsed + "s)");
			Console.WriteLine("Proxy scraping will commence in 20 seconds.");
			Console.WriteLine("Any data in ./proxies/scraped.txt will be overwritten.");
			
			Thread.Sleep(20745);
			
			Console.SetCursorPosition(0, 4);
			Console.Write("Proxies: " + Program.proxiesScraped);
			Console.Write("\nStatus: " + Program.status);
			
		}
		
		protected internal void updateTime () {
			
			DateTime dt = DateTime.Now;
			Console.SetCursorPosition(title.Length + 3, 0);
			Console.Write(dt);
			++secondsElapsed;
			Console.SetCursorPosition(title.Length + (3 + (dt.ToString()).Length + 16), 0);
			Console.Write(secondsElapsed + "s)");
			
		}
		
		protected internal void updateProxies () {
			
			Console.SetCursorPosition(10, 4);
			Console.Write(Program.proxiesScraped);
			
		}
		
		protected internal void updateStatus (Status status) {
			
			Program.status = status;
			Console.SetCursorPosition(8, 5);
			Console.Write((Program.status == Status.IDLE) ? "Idle" : (Program.status == Status.SEARCHING ? "Searching for links" : (Program.status == Status.SCRAPING ? "Scraping links for proxies" : (Program.status == Status.SAVING ? "Saving proxies to computer" : "Pinging proxies"))));
			
		}
		
	}
	
}