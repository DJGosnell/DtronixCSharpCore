using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace dtxCore {
	/// <summary>
	/// Class to aid in the parsing of CSS files.
	/// </summary>
	public class CssDocument {

		public class Property {
			/// <summary>
			/// Property name.
			/// </summary>
			public string Name { get; set; }

			public string _value;
			/// <summary>
			/// Value for the property.
			/// </summary>
			public string Value {
				get {
					return _value;
				}
				set {
					_value = value;

					// Reset all cache in the event that this value is changed from when it was last parsed.
					_value_color = null;
					_value_string = null;
				}
			}

			private Nullable<Color> _value_color = null;
			/// <summary>
			/// Gets or Sets the color value for this property.  Get parses HEX and RGB. Set stores value in RGB
			/// </summary>
			public Color ValueColor {
				get {
					// Check to see if we have the value already cached.  If so, just return the already parsed value.
					if(_value_color.HasValue)
						return _value_color.Value;

					// Remove any spaces and whitespace.
					string value = Value.Trim().Replace(" ", "");
					_value_color = Color.FromName(value);

					// Determine if the parsing succeeded or failed.  If it failed, the returned color will be ARGB 0,0,0,0.
					if(_value_color.Value.A == 0 && _value_color.Value.R == 0 && _value_color.Value.G == 0 && _value_color.Value.B == 0) {
						_value_color = null;
					} else {
						return _value_color.Value;
					}

					// Determine if we have a RGB color or a HEX color.
					if(value.IndexOf("rgb", StringComparison.CurrentCultureIgnoreCase) == 0) { // RGB
						Match found_matches = Regex.Match(value, @"rgb\(([0-9]{1,3}),([0-9]{1,3}),([0-9]{1,3})\)", RegexOptions.IgnoreCase);

						// Determine if we found anything.  Should only find four matches.
						if(found_matches.Groups.Count == 4) {
							try {
								int r = int.Parse(found_matches.Groups[1].Value);
								int g = int.Parse(found_matches.Groups[2].Value);
								int b = int.Parse(found_matches.Groups[3].Value);

								_value_color = Color.FromArgb(r, g, b);
							} catch { }
						}

					} else if(value.IndexOf('#') == 0) { // HEX
						try {
							// Parse the string to the corresponding RGB integers.
							int r = int.Parse(value.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
							int g = int.Parse(value.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
							int b = int.Parse(value.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

							_value_color = Color.FromArgb(r, g, b);
						} catch { } // No need to catch this error because all we need to know is that it could not parse..
					}

					return _value_color.GetValueOrDefault(Color.Empty);
				}

				set {
					// Set the cached value.
					_value_color = value;

					// Set the value string to the corresponding RGB color value.
					Value = String.Format("rgb({0},{1},{2})", value.R, value.G, value.B);
				}
			}

			private string _value_string = null;
			/// <summary>
			/// Gets or sets the string value for this property.  Get parses the string and removes any quotes. Set stores the string in quotes.
			/// </summary>
			public string ValueString {
				get {
					// Check to see if we have the value already cached.  If so, just return the already parsed value.
					if(_value_string != null)
						return _value_string;

					// Remove any whitespace around the string.
					string parsed_string = Value.Trim();

					// Check to see if we have an actual string or not.
					if(parsed_string.IndexOf('"') == 0 && parsed_string.LastIndexOf('"') == (parsed_string.Length - 1)) {
						_value_string = parsed_string.Substring(1, parsed_string.Length - 2);

					} else {

						// If we don't have a string, then set the cache to have save value as the Value property.
						_value_string = Value;
					}

					return _value_string;
				}
				set {
					// Set the cache variable
					_value_string = value;

					// Set the Value to the proper value.
					Value = string.Format("\"{0}\"", value);
				}
			}
		}

		private StreamReader stream;
		private char current_token;
		private bool eof = false;
		private Dictionary<string, Property[]> rule_set = new Dictionary<string, Property[]>();

		/// <summary>
		/// Parses a CSS stream into a useable format.  Closes the stream on completion.  Will override previous rule sets.
		/// </summary>
		/// <param name="stream">File stream to parse.</param>
		public CssDocument(Stream stream) {
			using(this.stream = new StreamReader(stream)) {
				parse();
			}
		}

		/// <summary>
		/// Parses a CSS stream into a useable format.  Will override previous rule sets.
		/// </summary>
		/// <param name="text">String of CSS characters to parse.</param>
		public CssDocument(string text) {
			using(this.stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(text)))) {
				parse();
			}
		}


		/// <summary>
		/// Combines existing CssDocuments into one new document.
		/// </summary>
		/// <param name="documents">Documents to combine into this instance.</param>
		public CssDocument(CssDocument[] documents) {
			combineDocuments(documents);
		}


		private void parse() {
			string[] selectors;
			Property[] properties;
			Property[] property_set;
			advanceToken();

			while(eof == false) {

				selectors = parseSelectors();
				properties = parseProperties();

				for(int i = 0; i < selectors.Length; i++) {

					if(rule_set.ContainsKey(selectors[i])) {
						// Selector already exists.  Override/add properties.
						List<Property[]> new_rule_set = new List<Property[]>();
						new_rule_set.Add(rule_set[selectors[i]]);
						new_rule_set.Add(properties);
						property_set = combineRules(new_rule_set);

						rule_set[selectors[i]] = property_set;
					} else {
						// Copy the array so that we are not modifying other rule's arrays.
						property_set = new Property[properties.Length];
						properties.CopyTo(property_set, 0);

						// Add it to the dictionary.
						rule_set.Add(selectors[i], property_set);
					}

				}
			}
		}

		/// <summary>
		/// Searches and combines any found rules for the selected selectors. Automatically adds * to selector list. (Case Insensitive)
		/// </summary>
		/// <param name="selector">Selectors to search through and combine.  Separate by a space. e.g. "* image .img_books"</param>
		/// <returns>Combined rules if any found.</returns>
		public Property[] select(string selector) {
			return select(selector, false);
		}

		/// <summary>
		/// Searches and combines any found rules for the selected selectors. (Case Insensitive)
		/// </summary>
		/// <param name="selector">Selectors to search through and combine.  Separate by a space. e.g. "* image .img_books"</param>
		/// <param name="ignore_all">If true, the combining of rule sets will ignore the "*" selector that should apply to all elements;  False if it should not ignore.</param>
		/// <returns>Combined rules if any found.</returns>
		public Property[] select(string selector, bool ignore_all) {
			List<Property[]> all_selected_properties = new List<Property[]>();
			string[] selectors = selector.Split(' ');
			string cleaned_selector;

			if(ignore_all == false && rule_set.ContainsKey("*"))
				all_selected_properties.Add(rule_set["*"]);

			foreach(string select in selectors) {

				cleaned_selector = select.Trim().ToLower();
				if(cleaned_selector == "") // If we don't have a valid selector, then just pass on.
					continue;

				if(rule_set.ContainsKey(cleaned_selector)) {
					all_selected_properties.Add(rule_set[cleaned_selector]);
				}
			}

			return combineRules(all_selected_properties);
		}


		private void combineDocuments(CssDocument[] documents) {
			Dictionary<string, Property[]> new_rule_set = new Dictionary<string, Property[]>();
			List<Property[]> properties_list = new List<Property[]>();

			// Loop through all the documents.
			foreach(CssDocument document in documents) {

				// Loop through each rule in the rule sets.
				foreach(KeyValuePair<string, Property[]> rule in document.rule_set) {

					if(new_rule_set.ContainsKey(rule.Key)) {
						// Key already exists.  So, just combine both rules together.
						properties_list.Clear();
						properties_list.Add(new_rule_set[rule.Key]);
						properties_list.Add(rule.Value);

						new_rule_set[rule.Key] = combineRules(properties_list);

					} else {
						// Key does not exist, so just add it.
						new_rule_set.Add(rule.Key, rule.Value);
					}
				}
			}

			rule_set = new_rule_set;
		}

		private Property[] combineRules(List<Property[]> property_sets) {
			Dictionary<string, Property> final_properties = new Dictionary<string, Property>();

			foreach(Property[] property_set in property_sets) {
				for(int y = 0; y < property_set.Length; y++) {
					if(final_properties.ContainsKey(property_set[y].Name)) {
						// Property exists, so overwrite.
						final_properties[property_set[y].Name] = property_set[y];
					} else {
						// It is a new property so just add it to the original.

						final_properties.Add(property_set[y].Name, property_set[y]);
					}
				}
			}

			Property[] properties = new Property[final_properties.Count];
			final_properties.Values.CopyTo(properties, 0);

			return properties;
		}


		private string[] parseSelectors() {
			List<string> selectors = new List<string>();
			StringBuilder selector = new StringBuilder();
			eatWhitespace();

			while(eof == false) {
				if(current_token == '{') {
					if(selector.ToString() != "")
						selectors.Add(selector.ToString().ToLower());
					// We have reached the properties section.  Quit out of the selector loop.
					break;

				} else if(current_token == ',' || isWhitespace(current_token)) {
					selectors.Add(selector.ToString().ToLower());
					selector.Remove(0, selector.Length);

					// Check to see if we actually did eat anything.  If not, we still need to advance.
					if(eatWhitespace() == false) {
						advanceToken();
						eatWhitespace();
					}

				} else {
					selector.Append(current_token);
					advanceToken();
				}
			}

			return selectors.ToArray();
		}

		private Property[] parseProperties() {
			List<Property> properties = new List<Property>();
			StringBuilder prop = new StringBuilder();
			StringBuilder val = new StringBuilder();

			if(current_token != '{')
				throw new Exception("Unexpected character @" + stream.BaseStream.Position + ".  Unable to continue parsing.");

			advanceToken();
			eatWhitespace();
			while(eof == false && current_token != '}') {

				// Parse the property name.
				while(current_token != ':' && eof == false) {
					prop.Append(current_token);
					advanceToken();
					eatWhitespace();
				}

				// Advance to the value.
				advanceToken();
				eatWhitespace();

				// Parse the property value.
				while(current_token != ';' && eof == false) {
					val.Append(current_token);
					advanceToken();
				}


				// Add the properties to the list.
				properties.Add(new Property() {
					Name = prop.ToString(),
					Value = val.ToString().Trim()
				});

				// Clear out the buffers.
				prop.Remove(0, prop.Length);
				val.Remove(0, val.Length);

				advanceToken();
				eatWhitespace();
			}

			advanceToken();

			return properties.ToArray();
		}

		private char advanceToken() {
			int current_char = stream.Read();
			if(current_char == -1) {
				eof = true;
			}

			current_token = (char)current_char;

			// Check to see if we ran into a comment.
			if(current_token == '/' && (char)stream.Peek() == '*') {
				// This is a comment.  Eat everything now!
				int ate_char = stream.Read();
				while(ate_char != -1) {

					// See if this is the end of the comment.
					if((char)ate_char == '*' && (char)stream.Peek() == '/')
						break;

					// This is still the comment.  Continue eating.
					ate_char = stream.Read();
				}

				// Advance to the "/"
				stream.Read();
				// Advance to right after the comment.
				current_char = stream.Read();

				// Ensure that we have not reached the end of the file yet.
				if(current_char == -1) {
					eof = true;
				}

				current_token = (char)current_char;
			}

			return current_token;
		}

		private bool isWhitespace(char character) {
			return (character == ' ' || character == '\n' || character == '\r' || character == '\t') ? true : false;
		}


		/// <summary>
		/// Advance in the stream  and when we hit a character, stop.
		/// </summary>
		private bool eatWhitespace() {
			bool ate = false;

			while(isWhitespace(current_token)) {
				ate = true;
				advanceToken();
			}

			return ate;
		}
	}
}
