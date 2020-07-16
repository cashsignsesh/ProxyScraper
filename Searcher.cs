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

namespace ProxyScraper {
	
	/// <summary>
	/// Search for proxy list websites
	/// </summary>
	public class Searcher {
		
		private string searchQuery = null;
		public static List<string> scrapedLinksArchive = new List<string>();
		private int iterCt = 0;
		
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
				
				Program.debug(link);
				
				Thread.Sleep(2500);
				
				byte[] res = new Byte[16384];
				Stream s = null;
				try {
					
					HttpWebRequest rq = (HttpWebRequest)(WebRequest.Create(link));
					HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
					s = rp.GetResponseStream();
					
				}
				catch (Exception ex) { /*Banned(too many requests)probably*/ Program.debug(ex.ToString()); Program.cm.updateStatus(Status.IDLE); Thread.Sleep(30000);
					
					Program.cm.updateStatus(Status.SEARCHING);
					
					try {
					
						HttpWebRequest rq = (HttpWebRequest)(WebRequest.Create(link));
						rq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
						HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
						s = rp.GetResponseStream();
					
					}
					catch (Exception ex0) { Program.debug(ex0.ToString()); Program.cm.updateStatus(Status.IDLE); Thread.Sleep(60000); return new List<String>(); }
					
				}
				
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
				
				foreach (HtmlNode htn in n.SelectNodes("//a[@href]")) {
					
					string v = htn.GetAttributeValue("href", "");
					if (!(v.ToLower().Contains("google")) && v.Contains("/url?q=") && v.ToLower().Contains("http://")) {
					    	
	                    int x = v.IndexOf("&");
	                    if (x == 0) continue;
	                    string p = v.Substring(0, x).Replace("/url?q=", "");
	                    
	                    this.inputLink(p, results);
	                    foreach (string p0 in this.searchForMorePages(p))
	                    	this.inputLink(p0, results);
	                    
	                }
					
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
			catch (Exception ex) { /*Website doesnt want us on for some reason, probably doesnt matter*/ Program.debug(ex.ToString());  }
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
			
			foreach (HtmlNode hnd in n.Descendants()) {
				
				HtmlAttributeCollection atribs = hnd.Attributes;
				
				hreflink = "";
				
				string innerText = hnd.InnerText.ToString().ToLower();
				
				/*if ( !( ( innerText.Contains("next") || innerText.Contains("\u00BB") || innerText.Contains(((char)8594).ToString()) || innerText.Contains(">") ) )
				    || ( (this.iterCt == 0) && ( !(innerText == "1") || !(innerText == "2") || !(innerText == "3") || !(innerText == "4") || !(innerText == "5") ))) continue;
				CANT GET THIS TO WORK SO I GAVE UP TRYING TO FIX
				
				if ( !innerText.Contains("next") && !innerText.Contains("\u00BB") && !innerText.Contains(((char)8594).ToString()) && ! innerText.Contains(">") &&
				    ((this.iterCt != 0) && !(innerText == "1") && !(innerText == "2") && !(innerText == "3") && !(innerText == "4") && !(innerText == "5"))) continue;*/
				
				
				/*
				if (!innerText.Contains("next"))
					if (!innerText.Contains("\u00BB"))
						if (!innerText.Contains(((char)8594).ToString()))
							if (!innerText.Contains(">"))
								if (this.iterCt == 0)
									if (!(innerText == "1"))
										if (!(innerText == "2"))
												if (!(innerText == "3"))
													if (!(innerText == "4"))
														if (!(innerText == "5"))
															continue;*/
				
				
				
				if (innerText.Contains("next")) goto GoodInnerText;
				if (innerText.Contains("\u00BB")) goto GoodInnerText;
				if (innerText.Contains(((char)8549).ToString())) goto GoodInnerText;
				if (innerText.Contains(">")) goto GoodInnerText;
				if (this.iterCt == 0) {
					
					if (innerText == "1") goto GoodInnerText;
					if (innerText == "2") goto GoodInnerText;
					if (innerText == "3") goto GoodInnerText;
					if (innerText == "4") goto GoodInnerText;
					if (innerText == "5") goto GoodInnerText;
										
				}
				
				continue;
				GoodInnerText:
				
				foreach (HtmlAttribute ha in atribs) {
					
					if (ha.Name == "href" || ha.Name == "onclick") {
						
						hreflink = ha.Value;
						if (hreflink == "#") continue;
						
						Uri tmpUri = null;
						foreach (string hreflinksplitstring in hreflink.Split(' ')) {
							
							if (Uri.TryCreate(hreflinksplitstring, UriKind.Absolute, out tmpUri) && (tmpUri.Scheme == Uri.UriSchemeHttp || tmpUri.Scheme == Uri.UriSchemeHttps)) {
								
								newPages.Add(hreflinksplitstring);
								goto DoneSplitChecks;
								
							}
							else
								if (Uri.TryCreate(hreflinksplitstring.Replace("\"", ""), UriKind.Absolute, out tmpUri) && (tmpUri.Scheme == Uri.UriSchemeHttp || tmpUri.Scheme == Uri.UriSchemeHttps)) {
									
									newPages.Add(hreflinksplitstring);
									goto DoneSplitChecks;
									
								}
							
							
						}
						
						if (hreflink.StartsWith("/")) {
							
							newPages.Add(new Uri(link).GetLeftPart(UriPartial.Authority) + hreflink);
							continue;
							
						}
						
					}
					
				}
				
				DoneSplitChecks:;
				
			}
				
			++this.iterCt;
			
			if (!(this.iterCt == 0)) return newPages;
			
			if (!(this.iterCt == 4)) {
				
				foreach (string pg in newPages)
					foreach (string pg0 in this.searchForMorePages(pg))
						newPages.Add(pg0);
				
			}
			this.iterCt = 0;
			//File.WriteAllLines("html.txt", newPages);
			return newPages;
			
		}
		
		private void inputLink (string link, List<String> currResults) {
			
			if (!(currResults.Contains(link)) && !(scrapedLinksArchive.Contains(link))) {
				
				Program.debug(link);
			    currResults.Add(link);
			    scrapedLinksArchive.Add(link); 
		        
	        }
			
		}
		
	}
}	