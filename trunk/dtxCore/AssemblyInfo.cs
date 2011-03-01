using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;


namespace dtxCore {
	public class AssemblyInfo {
		public string copyright;
		public string version;
		public string company;
		public string description;
		public string title;
		public string trademark;
		public string product;

		private Assembly current_assembly; 

		public AssemblyInfo(Assembly current_assembly) {
			this.current_assembly = current_assembly;

			AssemblyName name = current_assembly.GetName();


			copyright = getAttribute<AssemblyCopyrightAttribute>().Copyright;
			version = name.Version.ToString();
			company = getAttribute<AssemblyCompanyAttribute>().Company;
			description = getAttribute<AssemblyDescriptionAttribute>().Description;
			title = getAttribute<AssemblyTitleAttribute>().Title;
			trademark = getAttribute<AssemblyTrademarkAttribute>().Trademark;
			product = getAttribute<AssemblyProductAttribute>().Product;
		}



		private T getAttribute<T>() {
			Attribute found_att = Attribute.GetCustomAttribute(current_assembly, typeof(T));
			
			if(found_att == null)
				return (T)Activator.CreateInstance(typeof(T), "");

			// Never do the following:
			T converted_att = (T)((object)found_att);

			return converted_att;
		}
	}
}
