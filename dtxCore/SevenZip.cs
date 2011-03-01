using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace dtxCore {
	class SevenZip {
		private string seven_zip_r;

		public bool zip_shared_file = true;

		public SevenZip() {
			seven_zip_r = Path.GetFullPath(Assembly.GetExecutingAssembly().FullName) + @"\7zr.exe";

			if(!File.Exists(seven_zip_r)) {
				seven_zip_r = null;
				throw new Exception("Unable to open 7zr.exe");
			}
			
		}

		public SevenZip(string location) {

			if(!File.Exists(location)) {
				seven_zip_r = null;
				throw new Exception("Unable to open 7zr.exe");

			} else {
				seven_zip_r = location;
			}
		}

		private string buildZipArguments(string zip_file, string[] input_files) {
			StringBuilder sb = new StringBuilder();

			sb.Append("A ");
			if(zip_shared_file)
				sb.Append(" -SSW");

			return sb.ToString();
		}

		public bool createZip(string zip_file, string[] input_files) {
			return false;
			ProcessStartInfo p = new ProcessStartInfo();
			p.FileName = seven_zip_r;

			//p.Arguments = "a \"" + targetName + "\" \"" + sourceName + "\"";
			//p.WindowStyle = ProcessWindowStyle.Hidden;

			// 3.
			// Start process and wait for it to exit
			//
			Process x = Process.Start(p);
			x.WaitForExit();

		}
	}

	class SevenZipMethods{

	}
}
