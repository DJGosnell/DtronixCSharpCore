using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace dtxCore.Forms {
	public partial class frmAbout : Form {
		public frmAbout(Assembly parent_assembly, string license, Image logo) {
			InitializeComponent();

			AssemblyInfo info = new AssemblyInfo(parent_assembly);

			_lblMainProgramDescription.Text = info.title + '\n';
			_lblMainProgramDescription.Text += "Version " + info.version + '\n';
			_lblMainProgramDescription.Text += info.copyright + '\n';

			_tabMainProgram.Text = info.title;

			_rtbMainProgramLicense.Text = license;

			_picMainProjectLogo.Image = logo;

			// Make sure the form is the correct size.  Windows XP's form widths are less than 7's.
			OperatingSystemInfo osi = Utilities.getOSInfo();
			if(osi.os.Contains("XP")) {
				Size min_size = new Size() {
					Height = this.MinimumSize.Height - 8,
					Width = this.MinimumSize.Width - 8
				};

				Size max_size = new Size() {
					Height = this.MaximumSize.Height - 8,
					Width = this.MaximumSize.Width - 8
				};

				this.MinimumSize = min_size;
				this.MaximumSize = max_size;
				this.Size = min_size;
			}
		}

		// In order of inclusion.
		private Dictionary<string, string> dtxCore_licenses = new Dictionary<string, string>(){
			{"Dtronix C-Sharp Core",  Properties.Resources.Licence_Dtronix_Core},
			{"Special Thanks", Properties.Resources.Special_Thanks},
			{"JsonFx.NET JSON Serializer", Properties.Resources.License_JsonFx_NET_JSON_Serializer},
			{"Sweetie Icons", Properties.Resources.License_Sweetie_Icons},
			{"User Activity Hook", Properties.Resources.License_User_Activity_Hook },
			{"7-Zip", Properties.Resources.License_Seven_Zip},	
			{"Vista Menu", Properties.Resources.License_Vista_Menu }
		};

		private void frmAbout_Load(object sender, EventArgs e) {

			foreach(KeyValuePair<string, string> license in dtxCore_licenses) {
				_lstdtxCoreLiscenseSelector.Items.Add(license.Key);
			}

			AssemblyInfo info = new AssemblyInfo(Assembly.GetExecutingAssembly());
			_lblDtxCoreVersion.Text += info.version;
			_lblDtxCoreCopyright.Text = info.copyright;
			_lblDtxCoreTitle.Text = info.title;

			_lstdtxCoreLiscenseSelector.SelectedIndex = 0;
		}

		private void _rtbDtxCoreLicense_LinkClicked(object sender, LinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(e.LinkText);
		}

		private void _lstdtxCoreLiscenseSelector_SelectedIndexChanged(object sender, EventArgs e) {
			_rtbDtxCoreLicense.Text = dtxCore_licenses[_lstdtxCoreLiscenseSelector.Text];
		}

		private void _rtbMainProgramLicense_LinkClicked(object sender, LinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(e.LinkText);
		}

		private void _btnClose_Click(object sender, EventArgs e) {
			this.Close();
		}



	}
}
