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
		
		public ProxyManager () {
			
			this.proxies = new List<String>();
			this.ping = new Ping();
			
		}
		
		public void inputProxy (string proxy) { proxies.Add(proxy); 
												Program.debug(proxy);
												++Program.proxiesScraped;
												Program.cm.updateProxies();
												}
		
		private void removeDupes () {
			
			List<String> noDupes = new List<String>();
			
			foreach (string s in this.proxies)
				if (!(noDupes.Contains(s)))
					noDupes.Add(s);
			
			this.proxies = noDupes;
			
		}
		
		public void save () {
			
			Program.cm.updateStatus(Status.SAVING);
			this.removeDupes();
			using (StreamWriter sw = new StreamWriter("./proxies/scraped.txt", false)) {
				
				foreach (string s in this.proxies)
					sw.WriteLine(s);
				
			}			
			
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
		
		
	}
	
}