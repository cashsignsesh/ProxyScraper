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
using System.Threading;
using ProxyScraperGui;

namespace ProxyScraper {
	
	/// <summary>
	/// Proxy scraper
	/// If needs to be reused then reinitialize, and Searcher.scrapedLinksArchive reset
	/// </summary>
	public class Scraper {
		
		private Searcher s;
		private int searchIterations = 0;
		private int switchIterations = 0;
		public static readonly Regex ipRegex = new Regex(@"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b");
		
		public Scraper () {
				
			this.s = new Searcher("proxy list");
			ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
			
		}
		
		private List<String> search (int start, int end) {
			
			MainForm.instance.updateStatus(Status.SEARCHING);
			
			return s.search(start, end);
			
		}
		
		public void scrape () {
			
			int iter = this.searchIterations * 10;
			bool isDefaultSearchQuery = this.s.getSearchQuery() == "proxy list";
			List<String> sites = this.search((isDefaultSearchQuery) ? 1 : iter + 1, (isDefaultSearchQuery) ? MainForm.instance.initialSearches : iter + 10);
			++this.searchIterations;
			MainForm.instance.updateStatus(Status.SCRAPING);
				
			foreach (string s in sites) { MainForm.instance.debug(s); if (!(MainForm.instance.blacklist.Contains(s))) new Thread(()=>{this._scrape(s);}).Start(); }
			
			if (this.searchIterations == 2 || isDefaultSearchQuery) {
				
				++this.switchIterations;
				MainForm.instance.progressValue = (int)Math.Floor((decimal)this.switchIterations/MainForm.instance.settingsQueries.Count+1);
				
				this.searchIterations = 0;
				
				if (this.switchIterations == MainForm.instance.settingsQueries.Count+1) { /*Program.pm.pingProxies();*/ MainForm.instance.pm.save(); MainForm.instance.updateStatus(Status.DONE);  return; }
				this.s = new Searcher(MainForm.instance.settingsQueries[switchIterations-1]);
				
				// Probably shouldn't continue too much bc google will ban your ip, but the code would work if expanded on
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
				rq.ServicePoint.Expect100Continue = true;
				HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
				s = rp.GetResponseStream();
					
			}
			catch (Exception e) {	
								MainForm.instance.debug(e.ToString());
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
			catch (Exception e) { MainForm.instance.debug(e.ToString()); return; }
			
			HtmlDocument d = new HtmlDocument();
			d.OptionOutputAsXml = true;
			d.LoadHtml(b.ToString());
			HtmlNode n = d.DocumentNode;
			HtmlNodeCollection children = n.ChildNodes;
			
			string se = null;
			foreach (HtmlTextNode xt in n.Descendants().OfType<HtmlTextNode>()) {
				
				string tx = Regex.Replace(xt.InnerHtml, @"\s+", "");
				
				foreach (string txx in tx.Split(' ')) {
					
					if (txx.Length < 6) continue;
					
					string[] bx = txx.Split(':');
					
					#region Formatted proxy
					
					if (!(se == null)) {
						
						if (txx == ":") continue;
						
						try { Int32.Parse(bx[0]); }
						catch { continue; }
						MainForm.instance.pm.inputProxy(se + ":" + bx[0]);
						MainForm.instance.pm.inputDebugProxy(se + ":" + bx[0] + ":Formatted");
						se = null;
						
					}
					
					if (bx.Length == 1) {
						
						try { IPAddress.Parse(bx[0]); }
						catch { continue; }
						se = bx[0];
						
					}
					
					#endregion
					
					#region Plain text proxy
					
					if (bx.Length > 0) {
				
						MatchCollection mc = ipRegex.Matches(txx);
						char[] chars = txx.ToCharArray();
						
						if (mc.Count == 0) continue;
						
						bool ismatch = false;
						int cont = 0;
						bool curCheck = false;
						int iters = 0;
						
						foreach (Match m in mc) {
								
							ismatch = false;
							string matchString = m.ToString();
							int mLength = matchString.ToCharArray().Length;
							StringBuilder port = new StringBuilder();
							
							curCheck = false;
							iters = 0;
						
							foreach (char txxChar in chars) {
								
								++iters;
								
								if (cont > mLength - 1) {
									
									curCheck = true;
									
									if (cont == mLength && (txxChar == ':' || txxChar == ' ')) { ++cont; continue; }
									else if (cont == mLength) break;
									
									try {  port.Append(Int32.Parse(txxChar.ToString()));  }
									catch { 
										
										if (String.IsNullOrEmpty(port.ToString())) break;
										else ismatch = true; break;
										
									}
									
									if (!(String.IsNullOrEmpty(port.ToString()) && iters == chars.Length))
										ismatch = true;
									
									++cont;
									
								}
								
								if (curCheck) continue;
								
								if (txxChar == matchString[cont]) ++cont;
								else cont = 0;
								
							}
							
							if (!(ismatch)) continue;
							cont = 0;
							
							try { IPAddress.Parse(matchString); if (!(Int32.Parse(port.ToString()) > 79)) throw new Exception(); }
							catch { continue; }
							
							MainForm.instance.pm.inputProxy(matchString + ":" + port as string);
							MainForm.instance.pm.inputDebugProxy(matchString + ":" + port as string + ":Plain text");
				
						
						}
					
					}
					
					#endregion
					
				}
						
			}
			
			#region Raw lines proxy
			
			string[] inhtmlSplit = {""};
			try { inhtmlSplit = n.InnerHtml.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries); }
			catch (Exception e) { /*blank link?*/ MainForm.instance.debug(e.ToString()); return; }
			foreach (string rls in inhtmlSplit) {
				
				string[] zz = rls.Split(':');
				
				if (zz.Length != 2) continue;
				
				try { IPAddress.Parse(zz[0]); if (!(Int32.Parse(zz[1]) > 79)) throw new Exception(); }
				catch { continue; }
				
				MainForm.instance.pm.inputProxy(zz[0] + ":" + zz[1]);
				MainForm.instance.pm.inputDebugProxy(zz[0] + ":" + zz[1] + ":Raw lines");
				
			}
				
			
			#endregion
					                                
			
		}
		
	}
	
}
