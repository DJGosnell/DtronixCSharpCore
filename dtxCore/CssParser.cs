using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace dtxCore {
	/// <summary>
	/// Class to aid in the parsing of CSS files.
	/// </summary>
	public class CssParser {

		public class Property {
			/// <summary>
			/// Property name.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Value for the property.
			/// </summary>
			public string Value { get; set; }
		}

		private StreamReader stream;
		private char current_token;
		private bool eof = false;
		private Dictionary<string, Property[]> rule_set = new Dictionary<string, Property[]>();

		/// <summary>
		/// Parses a CSS stream into a useable format.  Closes the stream on completion.
		/// </summary>
		/// <param name="css_stream">File stream to parse.</param>
		public void parse(Stream css_stream) {
			eof = false;
			rule_set.Clear();

			stream = new StreamReader(css_stream);

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

			css_stream.Close();
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
			return current_token = (char)current_char;
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
				current_token = (char)stream.Read();
			}

			return ate;
		}
	}
}
