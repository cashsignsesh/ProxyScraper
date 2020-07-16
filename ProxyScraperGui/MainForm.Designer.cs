/*
 * Created by SharpDevelop.
 * User: Elite
 * Date: 7/16/2020
 * Time: 8:07 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using ProxyScraper;
using System.Runtime.InteropServices;
using System.Windows.Forms;
 
namespace ProxyScraperGui {
 	
	partial class MainForm {
 		
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing) {
			
			if (disposing) {
				
				if (components != null) {
					
					components.Dispose();
					
				}
				
			}
			
			base.Dispose(disposing);
			
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent () {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.timeLabel = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.proxiesLabel = new System.Windows.Forms.Label();
			this.statusLabel = new System.Windows.Forms.Label();
			this.startScrapingBtn = new System.Windows.Forms.Button();
			this.debugInfoLabel = new System.Windows.Forms.Label();
			this.proxiesSaveLocationInfoLabel = new System.Windows.Forms.Label();
			this.errorLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// timeLabel
			// 
			this.timeLabel.Location = new System.Drawing.Point(12, 9);
			this.timeLabel.Name = "timeLabel";
			this.timeLabel.Size = new System.Drawing.Size(394, 17);
			this.timeLabel.TabIndex = 0;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(12, 218);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(394, 23);
			this.progressBar.TabIndex = 1;
			// 
			// proxiesLabel
			// 
			this.proxiesLabel.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.proxiesLabel.Location = new System.Drawing.Point(12, 188);
			this.proxiesLabel.Name = "proxiesLabel";
			this.proxiesLabel.Size = new System.Drawing.Size(361, 24);
			this.proxiesLabel.TabIndex = 3;
			this.proxiesLabel.Text = "Proxies:";
			// 
			// statusLabel
			// 
			this.statusLabel.Font = new System.Drawing.Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusLabel.Location = new System.Drawing.Point(12, 163);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(360, 25);
			this.statusLabel.TabIndex = 4;
			this.statusLabel.Text = "Status:";
			// 
			// startScrapingBtn
			// 
			this.startScrapingBtn.Font = new System.Drawing.Font("Candara", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.startScrapingBtn.Location = new System.Drawing.Point(12, 30);
			this.startScrapingBtn.Name = "startScrapingBtn";
			this.startScrapingBtn.Size = new System.Drawing.Size(394, 64);
			this.startScrapingBtn.TabIndex = 5;
			this.startScrapingBtn.Text = "Start Scraping";
			this.startScrapingBtn.UseVisualStyleBackColor = true;
			this.startScrapingBtn.Click += new System.EventHandler(this.StartScrapingBtnClick);
			// 
			// debugInfoLabel
			// 
			this.debugInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.debugInfoLabel.Location = new System.Drawing.Point(13, 97);
			this.debugInfoLabel.Name = "debugInfoLabel";
			this.debugInfoLabel.Size = new System.Drawing.Size(554, 20);
			this.debugInfoLabel.TabIndex = 7;
			this.debugInfoLabel.Text = "Debug logs are saved to ./debug.txt";
			// 
			// proxiesSaveLocationInfoLabel
			// 
			this.proxiesSaveLocationInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.proxiesSaveLocationInfoLabel.Location = new System.Drawing.Point(13, 117);
			this.proxiesSaveLocationInfoLabel.Name = "proxiesSaveLocationInfoLabel";
			this.proxiesSaveLocationInfoLabel.Size = new System.Drawing.Size(553, 20);
			this.proxiesSaveLocationInfoLabel.TabIndex = 8;
			this.proxiesSaveLocationInfoLabel.Text = "Proxies will be saved to ./proxies/scraped.txt when done";
			// 
			// errorLabel
			// 
			this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.errorLabel.Location = new System.Drawing.Point(14, 137);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(391, 20);
			this.errorLabel.TabIndex = 9;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(417, 258);
			this.Controls.Add(this.errorLabel);
			this.Controls.Add(this.proxiesSaveLocationInfoLabel);
			this.Controls.Add(this.debugInfoLabel);
			this.Controls.Add(this.startScrapingBtn);
			this.Controls.Add(this.statusLabel);
			this.Controls.Add(this.proxiesLabel);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.timeLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "ProxyScraperGui";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.Label proxiesSaveLocationInfoLabel;
		private System.Windows.Forms.Label debugInfoLabel;
		private System.Windows.Forms.Button startScrapingBtn;
		private System.Windows.Forms.Label statusLabel;
		private System.Windows.Forms.Label proxiesLabel;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label timeLabel;
		
		private string user = null;
		private string title = null;
		private int secondsElapsed = 0;
		
		public System.Windows.Forms.Timer tm;
		public System.Windows.Forms.Timer tm0;
		
		public byte[] queriesDefault;
		
		public int proxiesScraped = 0;
		public Status status = Status.IDLE;
		
		private  Scraper s = null;
		internal  ProxyManager pm = null;
		
		public List<string> settingsQueries = null;
		public List<string> blacklist = null;
		
		public int initialSearches = 3;
		public int threads = 1000;
		
		public static MainForm instance;
		public int progressValue = 0;

		
		private void MainFormLoad (object sender, System.EventArgs e) {
			
			try {
			
			MainForm.instance = this;
			this.debug("--- Started ProxyScraper: " + DateTime.Now + " ---");
			this.init();
			
			this.user = Environment.UserName;
			this.title = "Proxy Scraper - " + this.user;
			this.Text = title;
			#if !DEBUG
			Console.CursorVisible = false;
			#endif
			this.queriesDefault = Encoding.ASCII.GetBytes(@"inurl:proxies.txt
inurl:socks4.txt
inurl:socks5.txt
proxies
socks list");
			
			this.tm = new System.Windows.Forms.Timer();
			this.tm.Tick += new EventHandler(updateStatus);
			this.tm.Interval = 10;
			this.tm.Tick += new EventHandler(updateProxies);
			this.tm.Tick += new EventHandler(updateProgress);
			this.tm.Start();
			
			this.tm0 = new System.Windows.Forms.Timer();
			this.tm0.Tick += new EventHandler(updateTime);
			this.tm0.Interval = 1000;
			this.tm0.Start();
			
        	progressBar.Style = ProgressBarStyle.Continuous;
        	progressBar.Maximum = 100;
				
			}
			catch(Exception ehhgsd){try{this.debug(ehhgsd.ToString());}catch{}}
			
		}
		
		private void updateTime (Object DGSDSGOIUTGOUIPUTOESJTYUJSDUIOGDSIOGJDSLKFJGDSOIGHSDPUIOTGUHJSOGDSJHOPIGHSDJOGPHUISHJTGOUIHJGDOIGJSDOIGJSDOIGDSJOIGSDJOIYTGEWUTIPOEWHPTOFASPOIFSDAPIOFGJASDOPIUJSAGDSJMLGKSDJHOITEWJ, EventArgs IDCIDCIDCIDCIDCIDCODIUFSDOUFGHSDAKUGDHSLKGSDJHLFHDDFOKJGYHSDFUIGHSDFUIOYSDHFIUSDATHFHUYIASTFDGHFAYUIGFHSAIFGHASKLJFGHASDKJGADJLADFSGHSHDFSHJDFSHSDFSDATSYSDHDFH) {
			
			this.timeLabel.Text = (title + " - " + DateTime.Now + " (Time Elapsed: " + secondsElapsed + "s)");
			++secondsElapsed;
			
		}
		
		public void debug (string info) {
			
			#if DEBUG
			retry:
			try {
				using (StreamWriter sw = File.AppendText("debug.txt"))
					sw.WriteLine(info);
			}
			catch(Exception){Thread.Sleep(5);goto retry;}
			#endif
			
		}
		
		private void init () {
			
			this.pm = new ProxyManager();
			this.s = new Scraper();
			
			if (!(Directory.Exists("./proxies/"))) {
				
				Directory.CreateDirectory("./proxies/");
				
				if (!(File.Exists("./proxies/scraped.txt"))) File.Create("./proxies/scraped.txt").Close();
				if (!(File.Exists("./queries.txt"))) File.Create("./queries.txt").Close();
				if (!(File.Exists("./blacklist.txt"))) File.Create("./blacklist.txt").Close();
				
				#if DEBUG
				if (!(File.Exists("./debug.txt"))) File.Create("./debug.txt").Close();
				if (!(File.Exists("./proxies/debug_scraped.txt"))) File.Create("./proxies/debug_scraped.txt").Close();
				#endif
				
				File.AppendAllText("./queries.txt", Encoding.ASCII.GetString(this.queriesDefault));
				
			}
			
			settingsQueries = new List<String>(File.ReadAllLines("./queries.txt"));
			blacklist = new List<String>(File.ReadAllLines("./blacklist.txt"));
			
		}
		
		protected internal void updateProxies (object eeee, EventArgs idc) {
			try{
			this.proxiesLabel.Text = "Proxies: " + this.proxiesScraped;
			/*this.proxyList.Lines = this.pm.getProxies().ToArray();*/}
			catch{/*idc,bound to happen, no reprecussions, just an eye sore to see exception on screen*/}
			
		}
		
		protected internal void updateStatus (object pp, EventArgs qq) {
			
			try{this.statusLabel.Text = ("Status: " + ((this.status == Status.IDLE) ? "Idle" : (this.status == Status.SEARCHING ? "Searching for links" : (this.status == Status.SCRAPING ? "Scraping links for proxies" : (this.status == Status.SAVING ? "Saving proxies to computer" : (this.status == Status.PINGING ? "Pinging proxies" : "Done"))))));
			}catch{}
		}
		
		public void disposeTimers () {
			
			this.tm.Stop();
			this.tm0.Stop();
			
			this.tm.Dispose();
			this.tm0.Dispose();
			
		}
		
		protected internal void updateStatus (Status status) {
			
			try{this.status = status;}catch{}
			
		}
		
		
		private void StartScrapingBtnClick (object sender, EventArgs e)	{
			
			this.startScrapingBtn.Enabled = false;
			try{new Thread(s.scrape).Start();}catch{}
			
		}
		
		protected internal void updateProgress (object irdc, EventArgs idc) {
			
			try{this.progressBar.Value = this.progressValue;}catch{}
			
		}
		
		protected internal void setErrorLabel (string error) { this.errorLabel.Text = error; }
		
	}
	
}
