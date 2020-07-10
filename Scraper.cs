/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 7/9/2020
 * Time: 9:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace ProxyScraper {
	
	/// <summary>
	/// Proxy scraper
	/// If needs to be reused then reinitialize
	/// </summary>
	public class Scraper {
		
		private Searcher s;
		private int searchIterations = 0;
		private int switchIterations = 0;
		
		public Scraper () {
				
			this.s = new Searcher("proxy list");
			
		}
		
		private List<String> search (int start, int end) {
			
			Program.status = "Searching for links";
			
			return s.search(start, end);
			
		}
		
		public void scrape () {
			
			int iter = this.searchIterations * 10;
			List<String> sites = this.search(iter + 1, iter + 10);
			++this.searchIterations;
			
			Program.status = "Scraping";
			
			//foreach (string s in sites) this._scrape(s);
			foreach (string s in sites) Console.WriteLine(s);
			
			if (this.searchIterations == 5) {
				
				++switchIterations;
				
				this.searchIterations = 0;
				if (switchIterations == 1) this.s = new Searcher("proxies");
				if (switchIterations == 2) this.s = new Searcher("socks list");
				if (switchIterations == 3) return;
				// Probably shouldn't continue bc google will ban your ip
				
			}
			
			this.scrape();
			
		}
		
		private void _scrape (string site) {
			
			
			
		}
		
	}
	
}
