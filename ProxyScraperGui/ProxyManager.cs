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
using ProxyScraperGui;
using System.Net.Sockets;
using System.Threading;

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
												MainForm.instance.debug(proxy);
												++MainForm.instance.proxiesScraped;
												}
		
		private void removeDupes () {
			
			List<String> noDupes = new List<String>();
			
			foreach (string s in this.proxies)
				if (!(noDupes.Contains(s)) && !(MainForm.instance.blacklist.Contains(s)))
					noDupes.Add(s);
			
			this.proxies = noDupes;
			
		}
		
		public void save () {
			
			MainForm.instance.progressValue = 0;
			this.removeDupes();
			this.pingProxies();
			MainForm.instance.updateStatus(Status.SAVING);
			using (StreamWriter sw = new StreamWriter("./proxies/scraped.txt", false)) {
				
				foreach (string s in this.proxies)
					sw.WriteLine(s);
				
			}
			
			#if DEBUG
			
			MainForm.instance.updateStatus(Status.SAVING);
			this.removeDupes();
			using (StreamWriter sw = new StreamWriter("./proxies/debug_scraped.txt", false)) {
				
				foreach (string s in this.proxiesDebug)
					sw.WriteLine(s);
				
			}
			
			MainForm.instance.debug("--- Finished ProxyScraper: " + DateTime.Now + " ---");
			
			#endif
			
			MainForm.instance.disposeTimers();
			//MainForm.instance.fixConsole();
			
		}
		
		public void pingProxies () {
			
			
			MainForm.instance.updateStatus(Status.PINGING);
			List<String> pinged = new List<String>();
			
			int proxyNo = 0;
			int threads = 0;
			
			while (threads < MainForm.instance.threads) {
				
				new Thread(() => {
				
					while (true) {
						
						try {
						
							string proxy = this.proxies[proxyNo];
							++proxyNo;
							
							string[] sp = proxy.Split(':');
							if (this.ping.Send(sp[0], 13600).Status == IPStatus.Success)
								pinged.Add(proxy);
							
							MainForm.instance.progressValue = (int)Math.Floor((decimal)proxyNo/this.proxies.Count);
							
						}
						catch {break;}
						
					}
				           	
				 }).Start();
				
			}
			
			proxies = pinged;
			MainForm.instance.proxiesScraped = proxies.Count;
			
		}
		
		public void inputDebugProxy (string proxy) { this.proxiesDebug.Add(proxy); }
		
		#if DEBUG
		/// <summary>
		/// For debug
		/// </summary>
		/// <returns>The proxy list</returns>
		public List<string> getProxies () { return this.proxies; }
		#endif
		
	}
	
}