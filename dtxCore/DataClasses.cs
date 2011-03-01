using System;
using System.Collections.Generic;
using System.Text;

namespace dtxCore {
	public class OperatingSystemInfo {
		public int architecture = -1;
		public string os = null;
		public string service_pack = null;
		public override string ToString() {
			if(os == null) return null;

			string arch = (architecture == -1) ? "" : " " + architecture.ToString() + "-bit";
			string sp = (service_pack == null) ? "" : " " + service_pack;
			return os.TrimEnd(new char[] {' '}) + sp  + arch;

			
		}
	}
}
