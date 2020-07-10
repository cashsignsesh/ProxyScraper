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
		
		public Searcher (string searchQuery) {
			
			this.searchQuery = searchQuery;
			
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
				
				Program.debug(link);
				
				Thread.Sleep(2500);
				
				byte[] res = new Byte[16384];
				Stream s = null;
				try {
					
					HttpWebRequest rq = (HttpWebRequest)(WebRequest.Create(link));
					rq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36";
					HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
					s = rp.GetResponseStream();
					
				}
				catch (Exception ex) { /*Banned(too many requests)probably*/ Program.debug(ex.ToString()); Thread.Sleep(30000);
					
					try {
					
						HttpWebRequest rq = (HttpWebRequest)(WebRequest.Create(link));
						rq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36";
						HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
						s = rp.GetResponseStream();
					
					}
					catch (Exception ex0) { Program.debug(ex0.ToString()); Thread.Sleep(60000); return new List<String>(); }
					
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
	                    if (!(results.Contains(p)) && !(scrapedLinksArchive.Contains(p))) {
	                    	
		                    results.Add(p);
		                    scrapedLinksArchive.Add(p); 
		                    
	                    }
	                    
	                }
					
				}
				
			}
			
			return results;
			
		}
		
	}
}