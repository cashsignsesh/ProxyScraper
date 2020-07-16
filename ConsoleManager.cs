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
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace ProxyScraper {
	
	/// <summary>
	/// Manage console
	/// </summary>
	public class ConsoleManager {
		
		private readonly string user = null;
		private readonly string title = null;
		private int secondsElapsed = 0;
		
		public readonly byte[] queriesDefault;
		public Timer tm;
		public Timer tm0;
		
		public ConsoleManager () {
			
			this.user = Environment.UserName;
			this.title = "Proxy Scraper - " + this.user;
			Console.Title = title;
			#if !DEBUG
			Console.CursorVisible = false;
			#endif
			queriesDefault = Encoding.ASCII.GetBytes(@"inurl:proxies.txt
inurl:socks4.txt
inurl:socks5.txt
proxies
socks list");
				
			this.tm = new Timer(state => updateTime(), null, 1000, 1000);
			this.tm0 = new Timer(state => fixConsole(), null, 30000, 30000);
			
		}
		
		public void printBase () {
			
			Console.WriteLine(title + " - " + DateTime.Now + " (Time Elapsed: " + secondsElapsed + "s)");
			Console.WriteLine("Proxy scraping will commence at 20 seconds. It will finish in ~" + (Program.settingsQueries.Count*9) + "min.");
			Console.WriteLine("Any data in ./proxies/scraped.txt will be overwritten.");
			
			Thread.Sleep(20745);
			
			Console.SetCursorPosition(0, 4);
			Console.Write("Proxies: " + Program.proxiesScraped);
			Console.Write("\nStatus: " + Program.status); //TODO:: minor issue here, will be displayed in all caps at first
			
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
			
			Console.SetCursorPosition(0, 4);
			Console.Write(new string(' ', Console.WindowWidth)); 
			Console.SetCursorPosition(0, 4);
			Console.Write("Proxies: " + Program.proxiesScraped);
			
		}
		
		protected internal void updateStatus (Status status) {
			
			Program.status = status;
			Console.SetCursorPosition(0, 5);
			Console.Write(new string(' ', Console.WindowWidth)); 
			Console.SetCursorPosition(0, 5);
			Console.Write("Status: " + ((Program.status == Status.IDLE) ? "Idle" : (Program.status == Status.SEARCHING ? "Searching for links" : (Program.status == Status.SCRAPING ? "Scraping links for proxies" : (Program.status == Status.SAVING ? "Saving proxies to computer" : (Program.status == Status.PINGING ? "Pinging proxies" : "Done"))))));
			
		}
		
		public void disposeTimers () {
			
			this.tm.Dispose();
			this.tm0.Dispose();
			
		}
		
		protected internal void fixConsole () {
			
			DateTime dt = DateTime.Now;
			Console.SetCursorPosition(0, 0);
			Console.Write(new string(' ', Console.WindowWidth)); 
			Console.SetCursorPosition(0, 0);
			Console.Write(title + " - " + dt + " (Time Elapsed: " + secondsElapsed + "s)");
			Console.SetCursorPosition(0, 1);
			Console.Write(new string(' ', Console.WindowWidth)); 
			Console.SetCursorPosition(0, 1);
			Console.Write("Proxy scraping will commence at 20 seconds. It will finish in ~" + (Program.settingsQueries.Count*9) + "min.");
			Console.SetCursorPosition(0, 2);
			Console.Write(new string(' ', Console.WindowWidth)); 
			Console.SetCursorPosition(0, 2);
			Console.Write("Any data in ./proxies/scraped.txt will be overwritten.");
			
		}
		
	}
	
}