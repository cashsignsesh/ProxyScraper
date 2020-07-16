/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 7/10/2020
 * Time: 10:43 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.IO;
using System.Net;

namespace ProxyScraper {
	
	/// <summary>
	/// Proxy manager
	/// </summary>
	public class ProxyManager {
		
		private List<String> proxies = null;
		private Ping ping = null;
		private List<String> proxiesDebug = null;
		
		public ProxyManager () {
			
			this.proxies = new List<String>();
			this.ping = new Ping();
			this.proxiesDebug = new List<String>();
			
		}
		
		public void inputProxy (string proxy) { if (Int32.Parse(proxy.Split(':')[1]) > 65535) return;
												if (this.proxies.Contains(proxy)) return;
												this.proxies.Add(proxy);
												Program.debug(proxy);
												++Program.proxiesScraped;
												Program.cm.updateProxies();
												}
		
		private void removeDupes () {
			
			List<String> noDupes = new List<String>();
			
			foreach (string s in this.proxies)
				if (!(noDupes.Contains(s)) && !(Program.blacklist.Contains(s)))
					noDupes.Add(s);
			
			this.proxies = noDupes;
			
		}
		
		public void save () {
			
			Program.cm.updateStatus(Status.SAVING);
			this.removeDupes();
			//this.pingProxies();
			using (StreamWriter sw = new StreamWriter("./proxies/scraped.txt", false)) {
				
				foreach (string s in this.proxies)
					sw.WriteLine(s);
				
			}
			
			#if DEBUG
			
			Program.cm.updateStatus(Status.SAVING);
			this.removeDupes();
			using (StreamWriter sw = new StreamWriter("./proxies/debug_scraped.txt", false)) {
				
				foreach (string s in this.proxiesDebug)
					sw.WriteLine(s);
				
			}
			
			Program.debug("--- Finished ProxyScraper: " + DateTime.Now + " ---");
			
			#endif
			
			Program.cm.disposeTimers();
			Program.cm.fixConsole();
			
		}
		
		public void pingProxies () {
			
			Program.cm.updateStatus(Status.PINGING);
			List<String> pinged = new List<String>();
			
			foreach (string s in this.proxies) {
				
				string[] sp = s.Split(':');
				if (this.ping.Send(sp[0], Int32.Parse(sp[1])).Status == IPStatus.Success)
					pinged.Add(s);
				
			}
			
			proxies = pinged;
			
		}
		
		public void inputDebugProxy (string proxy) { this.proxiesDebug.Add(proxy); }
		
		/// <summary>
		/// For debug
		/// </summary>
		/// <returns>The proxy list</returns>
		public List<string> getProxies () { return this.proxies; }
		
	}
	
}