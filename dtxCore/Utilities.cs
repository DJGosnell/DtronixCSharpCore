using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;

namespace dtxCore {

	public static class Utilities {
		/// <summary>
		/// Creates a random hash with specified length.
		/// </summary>
		/// <param name="length">Length of string to return.</param>
		/// <param name="uppercase">True: Allow uppercase characters to be present in the hash.  False: Just use lowercase letters and numbers.</param>
		/// <returns>Hash with specified length.</returns>
		public static string createHash(int length, bool uppercase) {
			int max_value = 35;
			char[] buffer_return = new char[length];
			Random random_seed = new Random();
			char[] character_array = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

			if(uppercase) {
				max_value = character_array.Length;
			}

			for(int i = 0; i < length; i++) {
				buffer_return[i] = character_array[random_seed.Next(0, max_value)];
			}
			return new string(buffer_return);
		}

		/// <summary>
		/// Decode a base64 encoded string to its original text.
		/// </summary>
		/// <param name="input_string">Base64 encoded string.</param>
		/// <returns></returns>
		public static string base64Decode(string input_string) {
			 UTF8Encoding utf8_encoding = new UTF8Encoding();
			Decoder utf8Decode = utf8_encoding.GetDecoder();
			byte[] to_decode_byte = Convert.FromBase64String(input_string);
			char[] decoded_char = new char[utf8Decode.GetCharCount(to_decode_byte, 0, to_decode_byte.Length)];
			utf8Decode.GetChars(to_decode_byte, 0, to_decode_byte.Length, decoded_char, 0);
			return new string(decoded_char);
		}

		/// <summary>
		/// Encode a string in base64 to enable easy transport over web protocols.
		/// </summary>
		/// <param name="input_string">String to encode in base64.</param>
		/// <returns>Base64 encoded string.</returns>
		public static string base64Encode(string input_string) {
			UTF8Encoding utf8_encoding = new UTF8Encoding();
			return Convert.ToBase64String(utf8_encoding.GetBytes(input_string));
		}

		/// <summary>
		/// Return a MD5 hash for an input string.
		/// </summary>
		/// <param name="str">String to have hashed.</param>
		/// <returns>MD5 hashed string.</returns>
		public static string md5Hash(string str) {
			byte[] unicodeText = Encoding.UTF8.GetBytes(str);

			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(unicodeText);

			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < result.Length; i++) {
				sb.Append(result[i].ToString("X2"));
			}

			// Ensure to return a lower string to match standards
			return sb.ToString().ToLower();
		}

		/// <summary>
		/// Outputs a formatted string of bytes.
		/// </summary>
		/// <param name="bytes">Int64 of bytes to parse.</param>
		/// <returns>String of formatted bytes.</returns>
		public static string formattedSize(long bytes) {
			const int scale = 1024;
			string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
			long max = (long)Math.Pow(scale, orders.Length - 1);

			foreach(string order in orders) {
				if(bytes > max)
					return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

				max /= scale;
			}
			return "0 Bytes";
		}

		public static DateTime unixToDateTime(long timestamp) {
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return origin.AddSeconds(timestamp);
		}


		public static long dateTimeToUnix(DateTime date) {
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeSpan diff = date - origin;
			return Convert.ToInt64(diff.TotalSeconds);
		}

		/// <summary>
		/// Gets Operating System Name, Service Pack, and Architecture using WMI with the legacy methods as a fallback
		/// </summary>
		/// <see cref="http://andrewensley.com/2009/10/c-detect-windows-os-version-%E2%80%93-part-2-wmi/"/>
		/// <returns>Class that contains all the related information.</returns>
		public static OperatingSystemInfo getOSInfo() {
			ManagementObjectSearcher objMOS = new ManagementObjectSearcher("SELECT * FROM  Win32_OperatingSystem");

			OperatingSystemInfo os_info = new OperatingSystemInfo();

			try {
				foreach(ManagementObject objManagement in objMOS.Get()) {
					// Get OS version from WMI - This also gives us the edition
					object osCaption = objManagement.GetPropertyValue("Caption");
					if(osCaption != null) {
						// Remove all non-alphanumeric characters so that only letters, numbers, and spaces are left.
						string osC = Regex.Replace(osCaption.ToString(), "[^A-Za-z0-9 ]", "");

						// If the OS starts with "Microsoft," remove it.  We know that already
						if(osC.StartsWith("Microsoft")) {
							osC = osC.Substring(9);
						}

						// If the OS now starts with "Windows," again... useless.  Remove it.
						if(osC.Trim().StartsWith("Windows")) {
							osC = osC.Trim().Substring(7);
						}
						// Remove any remaining beginning or ending spaces.
						os_info.os = osC.Trim();

						// Only proceed if we actually have an OS version - service pack is useless without the OS version.
						if(!String.IsNullOrEmpty(os_info.os)) {
							object osSP = null;

							try {
								// Get OS service pack from WMI
								osSP = objManagement.GetPropertyValue("ServicePackMajorVersion");
								if(osSP != null && osSP.ToString() != "0") {
									os_info.service_pack += " Service Pack " + osSP.ToString();

								} else {
									// Service Pack not found.  Try built-in Environment class.
									os_info.service_pack += getOSServicePackLegacy();
								}

							} catch(Exception) {
								// There was a problem getting the service pack from WMI.  Try built-in Environment class.
								os_info.service_pack += getOSServicePackLegacy();
							}
						}

						object osA = null;
						try {
							// Get OS architecture from WMI
							osA = objManagement.GetPropertyValue("OSArchitecture");

							if(osA != null) {
								string osAString = osA.ToString();
								// If "64" is anywhere in there, it's a 64-bit architectore.
								os_info.architecture = (osAString.Contains("64") ? 64 : 32);
							}

						} catch(Exception) { }
					}
				}
			} catch(Exception) { }

			// If WMI couldn't tell us the OS, use our legacy method.
			// We won't get the exact OS edition, but something is better than nothing.
			if(os_info.os == "") {
				os_info = getOSLegacy();
			}
			// If WMI couldn't tell us the architecture, use our legacy method.
			if(os_info.architecture == 0) {
				os_info.architecture = getOSArchitectureLegacy();
			}

			return os_info;
		}

		/// <summary>
		/// Gets Operating System Name using .Net's Environment class.
		/// </summary>
		/// <see cref="http://andrewensley.com/2009/10/c-detect-windows-os-version-%E2%80%93-part-2-wmi/"/>
		/// <returns>Class that contains all the related information.</returns>
		private static OperatingSystemInfo getOSLegacy() {
			OperatingSystem os = Environment.OSVersion;
			Version vs = os.Version;

			//Variable to hold our return value.
			OperatingSystemInfo os_info = new OperatingSystemInfo();

			if(os.Platform == PlatformID.Win32Windows) {
				//This is a pre-NT version of Windows
				switch(vs.Minor) {
					case 0:
						os_info.os = "95";
						break;
					case 10:
						if(vs.Revision.ToString() == "2222A") {
							os_info.os = "98SE";
						} else {
							os_info.os = "98";
						}
						break;
					case 90:
						os_info.os = "Me";
						break;
					default:
						break;
				}

			} else if(os.Platform == PlatformID.Win32NT) {
				switch(vs.Major) {
					case 3:
						os_info.os = "NT 3.51";
						break;
					case 4:
						os_info.os = "NT 4.0";
						break;
					case 5:
						if(vs.Minor == 0) {
							os_info.os = "2000";
						} else {
							os_info.os = "XP";
						}
						break;
					case 6:
						if(vs.Minor == 0) {
							os_info.os = "Vista";
						} else {
							os_info.os = "7";
						}
						break;
					default:
						break;
				}
			}
			//Make sure we actually got something in our OS check
			//We don't want to just return " Service Pack 2"
			//That information is useless without the OS version.

			if(os_info.os != "") {
				//Got something.  Let's see if there's a service pack installed.
				os_info.os += getOSServicePackLegacy();
			}

			//Return the information we've gathered.
			return os_info;
		}

		/// <summary>
		/// Gets the installed Operating System Service Pack using .Net's Environment class.
		/// </summary>
		/// <see cref="http://andrewensley.com/2009/10/c-detect-windows-os-version-%E2%80%93-part-2-wmi/"/>
		/// <returns>String containing the operating system's installed service pack (if any)</returns>
		private static string getOSServicePackLegacy() {
			// Get service pack from Environment Class
			string sp = Environment.OSVersion.ServicePack;

			if(sp != null && sp.ToString() != "" && sp.ToString() != " ") {
				// If there's a service pack, return it.
				return sp.ToString();
			}

			// No service pack.
			return null;
		}

		/// <summary>
		/// Gets Operating System Architecture.  This does not tell you if the program in running in
		/// 32- or 64-bit mode or if the CPU is 64-bit capable.  It tells you whether the actual Operating
		/// System is 32- or 64-bit.
		/// </summary>
		/// <see cref="http://andrewensley.com/2009/10/c-detect-windows-os-version-%E2%80%93-part-2-wmi/"/>
		/// <returns>Int containing 32 or 64 representing the number of bits in the OS Architecture</returns>
		private static int getOSArchitectureLegacy() {
			string pa = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
			return ((String.IsNullOrEmpty(pa) || String.Compare(pa, 0, "x86", 0, 3, true) == 0) ? 32 : 64);
		}


		/// <summary>
		/// Reads an entire stream into a byte array of known length.
		/// </summary>
		/// <param name="stream">Input stream to read.</param>
		/// <param name="buffer">Buffer of bytes to read the stream into.</param>
		/// <returns>True of success, False on failure to read entire stream.</returns>
		public static bool readStreamToEnd(Stream stream, byte[] buffer) {
			int offset = 0;
			int remaining = buffer.Length;
			while(remaining > 0) {
				int read = stream.Read(buffer, offset, remaining);
				if(read <= 0)
					return false;
				remaining -= read;
				offset += read;
			}
			return true;
		}

		static readonly int[] empty_int_array = new int[0];

		/// <summary>
		/// Matches all occurances of a bytes inside a byte[].
		/// </summary>
		/// <param name="haystack"></param>
		/// <param name="needle"></param>
		/// <returns></returns>
		public static int[] byteIndexOfAll(byte[] haystack, byte[] needle) {
			if(isEmptyLocate(haystack, needle))
				return empty_int_array;

			var list = new List<int>();

			for(int i = 0; i < haystack.Length; i++) {
				if(!isMatch(haystack, i, needle))
					continue;

				list.Add(i);
			}

			return list.Count == 0 ? empty_int_array : list.ToArray();
		}


		/// <summary>
		/// Finds the first occurance of a pattern of bytes inside byte[].
		/// </summary>
		/// <param name="haystack">Byte[] to search through</param>
		/// <param name="needle">Byte[] to search for.</param>
		/// <returns></returns>
		public static int byteIndexOf(byte[] haystack, byte[] needle) {
			if(isEmptyLocate(haystack, needle))
				return -1;

			for(int i = 0; i < haystack.Length; i++) {
				if(!isMatch(haystack, i, needle))
					continue;

				return i;
			}

			return -1;
		}

		private static bool isMatch(byte[] array, int position, byte[] needle) {
			if(needle.Length > (array.Length - position))
				return false;

			for(int i = 0; i < needle.Length; i++)
				if(array[position + i] != needle[i])
					return false;

			return true;
		}

		private static bool isEmptyLocate(byte[] array, byte[] needle) {
			return array == null
					|| needle == null
					|| array.Length == 0
					|| needle.Length == 0
					|| needle.Length > array.Length;
		}
	}
}
