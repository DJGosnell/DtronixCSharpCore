using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace dtxCore {

	/// <summary>
	/// Class to aid in the quick creation of encoded strings.
	/// </summary>
	public class TripleDESCrypto {

		private TripleDESCryptoServiceProvider crypto_tdes = new TripleDESCryptoServiceProvider();
		private UTF8Encoding utf8 = new UTF8Encoding();
		private object lock_object = new object();

		public TripleDESCrypto(string IV, string Key) {
			this.IV = IV;
			this.Key = Key;
		}

		/// <summary>
		/// (Set Only) IV for cypher.  Truncates after 8 bytes and padds with 0's.
		/// </summary>
		public string IV {
			set {
				byte[] buffer = new byte[crypto_tdes.IV.Length];
				byte[] bytes = utf8.GetBytes(value);
				Array.Copy(bytes, buffer, (bytes.Length > crypto_tdes.IV.Length) ? crypto_tdes.IV.Length : bytes.Length);
				crypto_tdes.IV = buffer;
			}
		}

		/// <summary>
		/// (Set Only) Key for cypher.  Truncates after 24 bytes and padds with 0's.
		/// </summary>
		public string Key {
			set {
				byte[] buffer = new byte[crypto_tdes.Key.Length];
				byte[] bytes = utf8.GetBytes(value);
				Array.Copy(bytes, buffer, (bytes.Length > crypto_tdes.Key.Length) ? crypto_tdes.Key.Length : bytes.Length);
				crypto_tdes.Key = buffer;
			}
		}

		/// <summary>
		/// Encrypt a string of utf8 characters into a base64 string of characters.
		/// </summary>
		/// <param name="value">String to encode.</param>
		/// <returns>Base64 string of characters.</returns>
		public string encrypt(string value) {
			lock(lock_object) {
				MemoryStream memory_stream = new MemoryStream();
				byte[] input = utf8.GetBytes(value);
				byte[] buffer = new byte[512];
				List<byte[]> byte_list = new List<byte[]>();
				byte[] output_buffer;
				int buffer_len;

				memory_stream.Write(input, 0, input.Length);
				memory_stream.Position = 0;

				CryptoStream cs_write = new CryptoStream(memory_stream, crypto_tdes.CreateEncryptor(), CryptoStreamMode.Read);

				while((buffer_len = cs_write.Read(buffer, 0, buffer.Length)) > 0) {
					byte[] temp_buffer = new byte[buffer_len];
					Array.Copy(buffer, temp_buffer, buffer_len);
					byte_list.Add(temp_buffer);
				}
				memory_stream.Close();
				cs_write.Close();

				int total_bytes = (byte_list.Count > 1) ? (((byte_list.Count - 1) * buffer.Length) + byte_list[byte_list.Count - 1].Length) : byte_list[0].Length;
				output_buffer = new byte[total_bytes];

				for(int i = 0; i < byte_list.Count; i++) {
					Array.Copy(byte_list[i], 0, output_buffer, (i * buffer.Length), byte_list[i].Length);
				}

				return Convert.ToBase64String(output_buffer);
			}
		}

		/// <summary>
		/// Decrypt a string of Base64 characters to a string of utf8 characters.
		/// </summary>
		/// <param name="value">Base64 string.</param>
		/// <returns>utf8 decoded string.</returns>
		public string decrypt(string value) {
			lock(lock_object) {
				MemoryStream memory_stream = new MemoryStream();
				byte[] input = Convert.FromBase64String(value);
				byte[] buffer = new byte[512];
				List<byte[]> byte_list = new List<byte[]>();
				byte[] output_buffer;
				int buffer_len;

				memory_stream.Write(input, 0, input.Length);
				memory_stream.Position = 0;

				CryptoStream cs_write = new CryptoStream(memory_stream, crypto_tdes.CreateDecryptor(), CryptoStreamMode.Read);

				while((buffer_len = cs_write.Read(buffer, 0, buffer.Length)) > 0) {
					byte[] temp_buffer = new byte[buffer_len];
					Array.Copy(buffer, temp_buffer, buffer_len);
					byte_list.Add(temp_buffer);
				}

				memory_stream.Close();
				cs_write.Close();

				int total_bytes = (byte_list.Count > 1) ? (((byte_list.Count - 1) * buffer.Length) + byte_list[byte_list.Count - 1].Length) : byte_list[0].Length;
				output_buffer = new byte[total_bytes];

				for(int i = 0; i < byte_list.Count; i++) {
					Array.Copy(byte_list[i], 0, output_buffer, (i * buffer.Length), byte_list[i].Length);
				}

				return utf8.GetString(output_buffer);
			}
		}
	}
}
