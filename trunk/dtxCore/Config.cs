using System;
using System.Collections.Generic;
using System.Text;
using dtxCore.Json;
using System.IO;

namespace dtxCore {

	/// <summary>
	/// Delegate for creation of initial configuration file.
	/// </summary>
	/// <param name="instance">Instance of the current config class.</param>
	public delegate void ConfigInitDelegate(Config instance);
	
	/// <summary>
	/// Class to aid in the storage and retrieval of values and classes in a plain text file.
	/// </summary>
	public class Config {

		private string save_file;
		private Dictionary<string, string> properties = new Dictionary<string, string>();
		private bool changed = false;

		/// <summary>
		/// Constructor for Config class.
		/// </summary>
		/// <param name="callback">Delegate that is called when the config file does not exist.</param>
		/// <param name="settings_file">Location of the file to save the program settings file.</param>
		public Config(ConfigInitDelegate callback, string settings_file) {
			save_file = settings_file;
			if(!File.Exists(save_file)) {
				callback(this);
				save();
			} else {
				load();
			}
		}

		/// <summary>
		/// Save the settings to the settings file only if there are changes to be made.
		/// </summary>
		public void save() {
			// No need to save if the file has not changed.
			if(!changed)
				return;

			if (!Directory.Exists(save_file))
				Directory.CreateDirectory(Path.GetDirectoryName(save_file));

			StreamWriter sw = new StreamWriter(save_file);

			sw.WriteLine("//Dtronix Configuration File v1");
			foreach(string key in properties.Keys) {
				sw.Write(key);
				sw.Write("=");
				sw.Write(properties[key]);
				sw.Write(Environment.NewLine);
			}
			sw.Close();

			// Reset the changed status.
			changed = false;
		}

		/// <summary>
		/// Load all the settings from the cfg file.
		/// </summary>
		public void load() {
			StreamReader sr = new StreamReader(save_file);
			string line;

			while((line = sr.ReadLine()) != null) {
				int split = line.IndexOf('=');
				if(split != -1){
					properties.Add(line.Substring(0, split), line.Substring(split + 1));
				}
			}

			sr.Close();
		}

		/// <summary>
		/// Gets value assigned to the property.
		/// </summary>
		/// <typeparam name="T">Type to cast the config property to.</typeparam>
		/// <param name="name">Name of the property to retrieve.</param>
		/// <returns>Value that was requested. If property does not exist, return default(T) value.</returns>
		public T get<T>(string name) {
			name = name.ToLower();

			if(properties.ContainsKey(name)) {
				JsonReader reader = new JsonReader(properties[name]);
				try {
					return reader.Deserialize<T>();

				} catch {
					return default(T);
				}

			} else {
				return default(T);
			}
		}

		/// <summary>
		/// Gets value assigned to the property and sets the property if it DNE.
		/// </summary>
		/// <typeparam name="T">Type to cast the config property to.</typeparam>
		/// <param name="name">Name of the property to retrieve.</param>
		/// <param name="default_value">Sets the default value for the object if the property does not exist.</param>
		/// <returns>Value that was requested. If property does not exist, return the default_value.</returns>
		public T get<T>(string name, T default_value) {
			name = name.ToLower();

			if(properties.ContainsKey(name)) {
				JsonReader reader = new JsonReader(properties[name]);
				try {
					return reader.Deserialize<T>();

				} catch {
					set(name, default_value);
					return default_value;
				}

			} else {
				set(name, default_value);
				return default_value;
			}
		}

		/// <summary>
		/// Checks to see if the requested property exists.
		/// </summary>
		/// <param name="name">Name of the property to verify exists.</param>
		/// <returns>True if the property exists, false if it does not.</returns>
		public bool propertyExist(string name) {
			return properties.ContainsKey(name);
		}

		/// <summary>
		/// Get the value of the property and increments the value afterword.
		/// </summary>
		/// <param name="name">Name of the property to retrieve.</param>
		/// <returns>Value that was requested.</returns>
		public int getAndIncrement(string name) {
			int current_num = get<int>(name);
			set(name, current_num + 1);
			save();
			return current_num;
		}

		/// <summary>
		/// Set a value to a proeprty.
		/// </summary>
		/// <param name="name">Property name.</param>
		/// <param name="value">Value to save to the property.</param>
		public void set(string name, object value) {
			name = name.ToLower();
			StringBuilder sb = new StringBuilder();
			JsonWriter writer = new JsonWriter(sb);
			writer.Write(value);

			if(properties.ContainsKey(name)) {
				properties[name] = sb.ToString();
			} else {
				properties.Add(name, sb.ToString());
			}

			changed = true;
		}

		/// <summary>
		/// Sets a value to a property if it has not already been set.
		/// </summary>
		/// <param name="name">Property name.</param>
		/// <param name="value">Value to save to the property if the property is not set already.</param>
		public void setIfNotSet(string name, object value) {
			if(!properties.ContainsKey(name)) {
				set(name, value);
			}
		}
	}
}
