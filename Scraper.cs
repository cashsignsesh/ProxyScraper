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
using HtmlAgilityPack;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProxyScraper {
	
	/// <summary>
	/// Proxy scraper
	/// If needs to be reused then reinitialize, and Searcher.scrapedLinksArchive reset
	/// </summary>
	public class Scraper {
		
		private Searcher s;
		private int searchIterations = 0;
		private int switchIterations = 0;
		
		public Scraper () {
				
			this.s = new Searcher("proxy list");
			
		}
		
		private List<String> search (int start, int end) {
			
			Program.cm.updateStatus(Status.SEARCHING);
			
			return s.search(start, end);
			
		}
		
		public void scrape () {
			
			int iter = this.searchIterations * 10;
			List<String> sites = this.search(iter + 1, iter + 10);
			++this.searchIterations;
			Program.cm.updateStatus(Status.SCRAPING);
			
			foreach (string s in sites) { Program.debug(s); this._scrape(s); }
			
			if (this.searchIterations == 5) {
				
				++switchIterations;
				
				this.searchIterations = 0;
				if (switchIterations == 1) this.s = new Searcher("proxies");
				if (switchIterations == 2) this.s = new Searcher("socks list");
				if (switchIterations == 3) { Program.pm.pingProxies(); Program.pm.save(); Program.cm.updateStatus(Status.DONE); return; }
				// Probably shouldn't continue bc google will ban your ip, but the code would work if expanded on
				// Some ideas for new search queries: 'github proxies', 'proxy txt'
				
			}
			
			this.scrape();
			
		}
		
		private void _scrape (string site) {
			
			byte[] res = new Byte[16384];
			Stream s = null;
			try {
					
				HttpWebRequest rq = (HttpWebRequest)(WebRequest.Create(site));
				rq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
				HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
				s = rp.GetResponseStream();
					
			}
			catch (Exception e) {	
								Program.debug(e.ToString());
								}
			
			StringBuilder b = new StringBuilder();
			int i = 0;
			try {
				
				while (true) {
					
					i = s.Read(res, 0, res.Length);
					if (i == 0) break;
					b.Append(Encoding.ASCII.GetString(res, 0, i));
					
				}
				
			}
			catch (Exception e) { Program.debug(e.ToString()); return; }
			
			HtmlDocument d = new HtmlDocument();
			d.OptionOutputAsXml = true;
			d.LoadHtml(b.ToString());
			HtmlNode n = d.DocumentNode;
			HtmlNodeCollection children = n.ChildNodes;
			List<String> txt = new List<String>();
			
			foreach (HtmlTextNode xt in n.Descendants().OfType<HtmlTextNode>())
				txt.Add(Regex.Replace(xt.InnerHtml, @"\s+", ""));
			
			foreach (string tx in txt) {
				
				foreach (string txx in tx.Split(' ')) {
				
					if (txx.Length < 6) continue;
					
					string[] bx = txx.Split(':');
					
					if (bx.Length == 2) {
						
						try { IPAddress.Parse(bx[0]); if (!(Int32.Parse(bx[1]) > 79)) throw new Exception(); }
						catch { continue; }
						
						Program.pm.inputProxy(bx[0] + ":" + bx[1]);
						
					}
					
				}
						
			}
					                                
			
		}
		
	}
	
}
