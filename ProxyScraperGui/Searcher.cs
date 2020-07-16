/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 7/9/2020
 * Time: 10:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.IO;
using HtmlAgilityPack;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProxyScraperGui;

namespace ProxyScraper {
	
	/// <summary>
	/// Search for proxy list websites
	/// </summary>
	public class Searcher {
		
		private string searchQuery = null;
		public static List<string> scrapedLinksArchive = new List<string>();
		private readonly Regex rg = new Regex(@"""([^""]*)&");
		
		public Searcher (string searchQuery) {
			
			this.searchQuery = searchQuery;
			ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
			
		}
		
		public List<String> search (int start, int end) {
			
			List<String> results = new List<string>();
			
			int iter = start;
			string link = "";
			
			while (iter != end) {
				
				if (iter == 1) link = "https://www.google.com/search?q=" + searchQuery;
				else link = "https://www.google.com/search?q=" + searchQuery + "&start=" + ((iter-1)*10);
				if (iter != end) ++iter;
				
				
				link = link.Replace(' ', '+');
				link = "https://" + link.Substring(8).Replace(":", "%3A");
				
				MainForm.instance.debug(link);
				
				Thread.Sleep(2500);
				
				byte[] res = new Byte[16384];
				Stream s = null;
				retry:
				try {
					
					HttpWebRequest rq = (HttpWebRequest)(WebRequest.Create(link));
					HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
					s = rp.GetResponseStream();
					
				}
				catch (Exception ex) { /*Banned(too many requests)probably*/ MainForm.instance.debug(ex.ToString()); MainForm.instance.setErrorLabel("You are currently softbanned from google. Retrying in 30s."); MainForm.instance.updateStatus(Status.IDLE); Thread.Sleep(30000);
					
					MainForm.instance.updateStatus(Status.SEARCHING);
					
					try {
					
						HttpWebRequest rq = (HttpWebRequest)(WebRequest.Create(link));
						rq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
						HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
						s = rp.GetResponseStream();
					
					}
					catch (Exception ex0) { MainForm.instance.debug(ex0.ToString()); MainForm.instance.updateStatus(Status.IDLE); MainForm.instance.setErrorLabel("You are currently softbanned from google. Retrying in 60s.");  Thread.Sleep(60000); goto retry; }
					
				}
				MainForm.instance.setErrorLabel(""); 
				int i = 0;
				StringBuilder b = new StringBuilder();
				while (true) {
					
					i = s.Read(res, 0, res.Length);
					if (i == 0) break;
					b.Append(Encoding.ASCII.GetString(res, 0, i));
					
				}
				
				HtmlDocument d = new HtmlDocument();
				d.OptionOutputAsXml = true;
				d.LoadHtml(b.ToString());
				HtmlNode n = d.DocumentNode;
				
				foreach (HtmlNode htn in n.Descendants("a").Where(ppeater3000 => ppeater3000.GetAttributeValue("href", "").StartsWith("/url?q="))) {
					
					string p = this.rg.Match(htn.OuterHtml).ToString();
					p = p.Substring(8).Split('&')[0];
					if (p.Contains("accounts.google.com")) continue;
					
					this.inputLink(p, results);
                    foreach (string p0 in this.searchForMorePages(p))
                    	this.inputLink(p0, results);
                    
				}
				
			}
			
			Thread.Sleep(4300); //Sleep a bit to avoid getting banned, doesn't mess up gui since this is in a new thread presumably
			return results;
			
		}
		
		private List<string> searchForMorePages (string link) {
			
			List<String> newPages = new List<String>();
			
			byte[] res = new Byte[16384];
			Stream s = null;
			try {
				
				HttpWebRequest rq = (HttpWebRequest)(WebRequest.Create(link));
				rq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
				rq.ServicePoint.Expect100Continue = true;
				HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
				s = rp.GetResponseStream();
					
			}
			catch (Exception ex) { /*Website doesnt want us on for some reason, probably doesnt matter*/ MainForm.instance.debug(ex.ToString());  }
			StringBuilder b = new StringBuilder();
			int i = 0;
			try{
			while (true) {
				
				i = s.Read(res, 0, res.Length);
				if (i == 0) break;
				b.Append(Encoding.ASCII.GetString(res, 0, i));
				
			}
			}
			catch(Exception){return newPages; /*idc*/}
			
			HtmlDocument d = new HtmlDocument();
			d.OptionOutputAsXml = true;
			d.LoadHtml(b.ToString());
			HtmlNode n = d.DocumentNode;
			string hreflink = "";
			
			foreach (HtmlNode hnd in n.Descendants().Where(KYSKYSKYSKYSKYSKYSKYSKYSKYSKYKYSKYSKYS => KYSKYSKYSKYSKYSKYSKYSKYSKYSKYKYSKYSKYS.HasAttributes &&
			                                               KYSKYSKYSKYSKYSKYSKYSKYSKYSKYKYSKYSKYS.Name.ToLower() == "a" &&
			                                              (
			                                               	KYSKYSKYSKYSKYSKYSKYSKYSKYSKYKYSKYSKYS.InnerText.ToLower().Contains("next") ||
			                                               	KYSKYSKYSKYSKYSKYSKYSKYSKYSKYKYSKYSKYS.InnerText.Equals("3") ||
			                                               	KYSKYSKYSKYSKYSKYSKYSKYSKYSKYKYSKYSKYS.InnerText.ToLower().Contains("proxies") ||
			                                               	KYSKYSKYSKYSKYSKYSKYSKYSKYSKYKYSKYSKYS.InnerText.ToLower().Contains("list")
			                                               )
			                                              )) {
				
				//File.AppendAllText("./html.txt", Regex.Match(hnd.OuterHtml, ">(.*?)<").Value.Replace("<","").Replace(">","") + "\n");
				
				HtmlAttributeCollection atribs = hnd.Attributes;
				
				hreflink = "";
				
				foreach (HtmlAttribute ha in atribs) {
					
					if (ha.Name != "href") continue;
					
					hreflink = ha.Value;
					if (hreflink.StartsWith("#")) continue;
					
					Uri tmpUri = null;
						
					if (Uri.TryCreate(hreflink, UriKind.Absolute, out tmpUri) && (tmpUri.Scheme == Uri.UriSchemeHttp || tmpUri.Scheme == Uri.UriSchemeHttps)) {
						
						if (!(newPages.Contains(hreflink)) && !(hreflink == link))
							newPages.Add(hreflink);
						goto DoneSplitChecks;
						
					}
					else {
						if (Uri.TryCreate(hreflink.Replace("\"", ""), UriKind.Absolute, out tmpUri) && (tmpUri.Scheme == Uri.UriSchemeHttp || tmpUri.Scheme == Uri.UriSchemeHttps)) {
							
							if (!(newPages.Contains(hreflink)) && !(hreflink == link))
								newPages.Add(hreflink);
							goto DoneSplitChecks;
							
						}
					}
				
					if (hreflink.StartsWith("/") || hreflink.StartsWith(@"\")) {
						
						string ppgg = new Uri(link).GetLeftPart(UriPartial.Authority) + hreflink;
						
						if (!(newPages.Contains(ppgg)) && !(ppgg == link))
							newPages.Add(ppgg);
						
						continue;
						
					}
					else /*Idk if this will generate junk, debug sample was small(1 website) and it worked fine..*/ {
						
						string ppgg = new Uri(link).GetLeftPart(UriPartial.Authority) + "/" + hreflink;
						
						if (Uri.TryCreate(ppgg, UriKind.Absolute, out tmpUri) && (tmpUri.Scheme == Uri.UriSchemeHttp || tmpUri.Scheme == Uri.UriSchemeHttps))
							if (!(newPages.Contains(ppgg)) && !(ppgg == link))
								newPages.Add(ppgg);
						
					}
					
					break;
					
				}
				
				DoneSplitChecks:;
				
			}
			
			return newPages;
			
		}
		
		private void inputLink (string link, List<String> currResults) {
			
			if (!(currResults.Contains(link)) && !(scrapedLinksArchive.Contains(link))) {
				
				MainForm.instance.debug(link);
			    currResults.Add(link);
			    scrapedLinksArchive.Add(link); 
		        
	        }
			
		}
		
		public string getSearchQuery () { return this.searchQuery; }
		
		public List<String> threadedSearch (int start, int end) {
			
			List<String> results = new List<String>();
			List<Task> tasks = new List<Task>();
				
			while (start != end+1) {
				
				tasks.Add(Task.Factory.StartNew(()=>{
				                                              	
				                                              	foreach (string s in search(start, start+1))
				                                              		results.Add(s);
				                                              
				                                              }));
				++start;
				
			}
			
			Task.WaitAll(tasks.ToArray());
			foreach (Task tsk in tasks) tsk.Dispose();
			return results;
			
		}
		
	}
}	