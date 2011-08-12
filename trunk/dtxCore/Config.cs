using System;
using System.Collections.Generic;
using System.Text;
using dtxCore.Json;
using System.IO;
using System.Threading;

namespace dtxCore {

	/// <summary>
	/// Delegate for creation of initial configuration file.
	/// </summary>
	/// <param name="instance">Instance of the current config class.</param>
	public delegate void ConfigInitDelegate(Config instance);


	/// <summary>
	/// Delegate to handle the ValueChanged event.
	/// </summary>
	/// <param name="new_value"></param>
	public delegate void ConfigValueChangedDelegate();
	
	/// <summary>
	/// Class to aid in the storage and retrieval of values and classes in a plain text file.
	/// </summary>
	public class Config {

		private string save_file;
		private Dictionary<string, string> properties = new Dictionary<string, string>();
		private Dictionary<string, List<ConfigValueChangedDelegate>> value_changed_events = new Dictionary<string, List<ConfigValueChangedDelegate>>();
		private bool changed = false;
		private int configuration_version;

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

			ThreadPool.QueueUserWorkItem(saveConfigurations);
		}

		/// <summary>
		/// Method to internally save the data to the configuration file.  Called via the ThreadPool.QueueUserWorkItem method.
		/// </summary>
		private void saveConfigurations(object thread){
			if (!Directory.Exists(save_file))
				Directory.CreateDirectory(Path.GetDirectoryName(save_file));

			StreamWriter sw = new StreamWriter(save_file);

			sw.WriteLine("//Dtronix Configuration File v1");
			foreach(string key in properties.Keys) {
				sw.Write(key.ToLower());
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
			StreamReader file = new StreamReader(save_file);
			string version = file.ReadLine();

			if(version == null) // if the file is null, then we have nothing to load.
				return;

			switch(version.Trim()){
				case "//Dtronix Configuration File v1":
					loadConfigV1(file);
					break;

				default: // If we can not determine which file we are dealing with, then try to parse it with the latest parser. We might get lucky.
					loadConfigV1(file);
					break;
			}

			// Close the file
			file.Close();
		}

		/// <summary>
		/// Load version one of the configuration file standard.
		/// </summary>
		/// <remarks>
		/// Do not close the stream inside this method.  This will be handled from the method that called this method.
		/// </remarks>
		/// <param name="file">File stream to be readign from.</param>
		private void loadConfigV1(StreamReader file) {
			configuration_version = 1;
			string line;
			
			while((line = file.ReadLine()) != null) {
				int split = line.IndexOf('=');
				if(split != -1) {
					properties.Add(line.Substring(0, split), line.Substring(split + 1));
				}
			}
		}

		/// <summary>
		/// Add a event delegate to be called any time the specified value changes.
		/// </summary>
		/// <param name="name">Name of the property to add the event to.</param>
		/// <param name="callback">Event delegate to call when the value has changed.</param>
		public void addValueChangedEvent(string name, ConfigValueChangedDelegate callback){
			if(value_changed_events.ContainsKey(name)) { // Check to see if there is already another delegate added on this key.
				value_changed_events[name].Add(callback);

			} else { // Key does not exist so we have to add the List to the Dict.
				var event_list = new List<ConfigValueChangedDelegate>();
				event_list.Add(callback);

				value_changed_events.Add(name, event_list);
			}
		}

		/// <summary>
		/// Removes an event that is added to the event list.
		/// </summary>
		/// <param name="name">Name of the property to find the event inside.</param>
		/// <param name="callback">Event delegate to remove from the queue of callable events.</param>
		/// <returns>True of successful removal; False otherwise.</returns>
		public bool removeValueChangedEvent(string name, ConfigValueChangedDelegate callback) {
			bool removed_callback = false;
			if(value_changed_events.ContainsKey(name)) {
				if(value_changed_events[name].Contains(callback)){
					return value_changed_events[name].Remove(callback);
				}
			}

			return removed_callback;
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
		/// Set a value to a proeprty.
		/// </summary>
		/// <remarks>
		/// Values are not case sensitive.  All stored values are automatically converted to lowercase.
		/// </remarks>
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

			// Check to see if we have any events queued.
			if(value_changed_events.ContainsKey(name)) {
				// Call all the events on another thread.
				ThreadPool.QueueUserWorkItem(fireValueChangedEvents, new DC_ValueChangedEventVariables(name, value));
			}

			changed = true;
		}


		/// <summary>
		/// Data Class to handle the passing of the changed values to the threaded method.
		/// </summary>
		private class DC_ValueChangedEventVariables {
			public DC_ValueChangedEventVariables(string name, object value) {
				this.name = name;
				this.value = value;
			}
			/// <summary>
			/// Name of the property that changed.
			/// </summary>
			public string name;

			/// <summary>
			/// Value of the property that changed.
			/// </summary>
			public object value;
		}

		private void fireValueChangedEvents(object info) {
			DC_ValueChangedEventVariables event_info = info as DC_ValueChangedEventVariables;
			if(event_info == null) // We don't want null reference exceptions now do we?
				return;

			foreach(var vc_event in value_changed_events[event_info.name]) {
				vc_event();
			}

		}

		/// <summary>
		/// Sets a value to a property if it has not already been set.
		/// </summary>
		/// <param name="name">Property name.</param>
		/// <param name="value">Value to save to the property if the property is not set already.</param>
		public void setIfEmpty(string name, object value) {
			if(!properties.ContainsKey(name)) {
				set(name, value);
			}
		}
	}
}
