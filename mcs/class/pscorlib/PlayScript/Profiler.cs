using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;

#if PLATFORM_MONOTOUCH
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif

namespace PlayScript
{
	public static class Profiler
	{
		public static bool Enabled = true;

		public static void Begin(string name)
		{
			if (!Enabled)
				return;

			Section section;
			if (!sSections.TryGetValue(name, out section)) {
				section = new Section();
				section.Name = name;
				sSections[name] = section;
			}

			section.Timer.Start();
			section.Stats.Subtract(PlayScript.Stats.CurrentInstance);
		}
		
		public static void End(string name)
		{
			if (!Enabled)
				return;

			Section section;
			if (!sSections.TryGetValue(name, out section)) {
				return;
			}

			section.Timer.Stop();
			section.Stats.Add(PlayScript.Stats.CurrentInstance);
		}

		public static void Reset()
		{
			if (!Enabled)
				return;

			// reset all sections
			foreach (Section section in sSections.Values) {
				section.TotalTime = new TimeSpan();
				section.Timer.Reset();
				section.Stats.Reset();
				section.History.Clear();
			}

			// reset all counters
			sFrameCount = 0;
		}

		public static void OnFrame()
		{
			if (!Enabled)
				return;

			// update all sections
			foreach (Section section in sSections.Values) {
				section.TotalTime += section.Timer.Elapsed;
				if (sDoReport) 
				{
					section.History.Add(section.Timer.Elapsed);
				}
				section.Timer.Reset();
			}

			sFrameCount++;
			if (!sDoReport) {
				// normal profiling, just print out every so often
				if ((sPrintFrameCount!=0) && (sFrameCount >= sPrintFrameCount)) {
					PrintTimes(System.Console.Out);
					Reset();
				}
			} else {
				// report generation, accumulate a specified number of frames and then print report
				if (sFrameCount >= sReportFrameCount) {
					// print out report
					DoReport();
					Reset();
					sDoReport = false;
				}
			}

			// check start report countdown
			if (sReportStartDelay > 0) {
				if (--sReportStartDelay == 0) {
					// reset counters
					Reset();

					// enable the report
					sDoReport = true;

					// start global timer
					sReportTime = Stopwatch.StartNew();
				}
			}
		}

		public static void StartSession(string reportName, int frameCount, int reportStartDelay = 5)
		{
			if (!Enabled)
				return;

			sReportName = reportName;
			sReportFrameCount = frameCount;
			sReportStartDelay = reportStartDelay;

			Console.WriteLine("Starting profiling session: {0} frames:{1}", reportName, frameCount);
		}

		
		public static void PrintTimes(TextWriter tw)
		{
			var str = "profiler: ";
			foreach (Section section in sSections.Values) {
				str += section.Name + ":";
				str += (section.TotalTime.TotalMilliseconds / sFrameCount).ToString("0.00");
				str += " ";
			}
			tw.WriteLine(str);
		}

		public static void PrintFullTimes(TextWriter tw)
		{
			foreach (Section section in sSections.Values) {
				tw.WriteLine("{0,-12} total:{1,6} average:{2,6}ms",
				             section.Name,
				             section.TotalTime,
				             (section.TotalTime.TotalMilliseconds / sFrameCount).ToString("0.00")
				             );
			}
		}

		public static void PrintHistory(TextWriter tw)
		{
			tw.Write("{0,-4} ", "");
			foreach (Section section in sSections.Values) 
			{
				tw.Write("{0,12} ", section.Name);
			}
			tw.WriteLine();

			tw.WriteLine("---------------------------");
			for (int frame=0; frame < sFrameCount; frame++)
			{
				tw.Write("{0,4}:", frame);
				foreach (Section section in sSections.Values) 
				{
					tw.Write("{0,12} ", section.History[frame].TotalMilliseconds.ToString("0.00") );
				}
				tw.WriteLine();
			}
		}

		public static void PrintStats(TextWriter tw)
		{
			foreach (Section section in sSections.Values) {
				var dict = section.Stats.ToDictionary(true);
				if (dict.Count > 0) {
					tw.WriteLine("---------- {0} ----------", section.Name);

					// create sorted list of stats results
					var list = dict.ToList();
					list.Sort((a,b) => {return b.Value.CompareTo(a.Value);} );
					foreach (var entry in list) {
						tw.WriteLine(" {0}: {1}", entry.Key, entry.Value);
					}
				}
			}
		}

		#region Private
		private static void PrintReport(TextWriter tw)
		{
			tw.WriteLine("******** Profiling report *********");
			tw.WriteLine("ReportName:    {0}", sReportName);

			#if PLATFORM_MONOTOUCH
			tw.WriteLine("Device:        {0}", UIDevice.CurrentDevice.Name);
			tw.WriteLine("Model:         {0}", UIDevice.CurrentDevice.Model);
			tw.WriteLine("SystemVersion: {0}", UIDevice.CurrentDevice.SystemVersion);
			tw.WriteLine("Screen Size:   {0}", UIScreen.MainScreen.Bounds);
			tw.WriteLine("Screen Scale:  {0}", UIScreen.MainScreen.Scale);
			#endif
			tw.WriteLine("Total Frames:  {0}", sFrameCount);
			tw.WriteLine("Total Time:    {0}", sReportTime.Elapsed);
			tw.WriteLine("Average FPS:   {0}",  ((double)sFrameCount / sReportTime.Elapsed.TotalSeconds).ToString("0.00") );

			tw.WriteLine("*********** Timing (ms) ***********");
			PrintFullTimes(tw);

			tw.WriteLine("***** Dynamic Runtime Stats ******");
			PrintStats(tw);

			tw.WriteLine("************ History *************");
			PrintHistory(tw);

			tw.WriteLine("**********************************");
		}

		private static void DoReport()
		{
			sReportTime.Stop();

			#if PLATFORM_MONOTOUCH
			// dump profile to file
			var dirs = NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, true);
			if (dirs.Length > 0) {
				string id = DateTime.Now.ToString("u").Replace(' ', '-');
				var path = Path.Combine(dirs[0], "profile-" + id + ".log");
				Console.WriteLine("Writing profiling report to: {0}", path);
				using (var sw = new StreamWriter(path)) {
					PrintReport(sw);
				}
			}
			#endif

			// print to console 
			PrintReport(System.Console.Out);

			// pause forever
//			Console.WriteLine("Pausing...");
//			for (;;) {
//				System.Threading.Thread.Sleep(1000);
//			}
		}

		// info for a single section
		class Section
		{
			public string               Name;
			public Stopwatch 			Timer = new Stopwatch();
			public TimeSpan				TotalTime;
			public List<TimeSpan>		History = new List<TimeSpan>();
			public Stats 				Stats = new PlayScript.Stats();	
		};

		private static Dictionary<string, Section> sSections = new Dictionary<string, Section>();
		private static int sFrameCount  = 0;

		// the frequency to print profiiling info
		private static int sPrintFrameCount  = 60;

		// report handling
		private static bool sDoReport = false;
		private static int  sReportStartDelay = 0;
		private static int  sReportFrameCount = 0;
		private static string sReportName;
		private static Stopwatch sReportTime;
		#endregion


	}
}

