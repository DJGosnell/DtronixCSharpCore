using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dtxCore.Forms {
	public partial class frmConsole : Form {
		public frmConsole() {
			InitializeComponent();
		}

		private void frmConsole_Load(object sender, EventArgs e) {
			_rtbOutput.Text = "";
		}

		public void writeLine(string text) {
			writeLine(text, "white");
		}

		public void writeLine(string text, string color) {
			this.Invoke((MethodInvoker)delegate {
				int current_len = _rtbOutput.TextLength;
				_rtbOutput.AppendText(text);
				_rtbOutput.AppendText("\n");

				_rtbOutput.Select(current_len, text.Length + 1);
				_rtbOutput.SelectionColor = Color.FromName(color);
				_rtbOutput.DeselectAll();
			});
		}

		public void write(string text) {
			write(text, "white");
		}

		public void write(string text, string color) {
			this.Invoke((MethodInvoker)delegate {
				int current_len = _rtbOutput.TextLength;
				_rtbOutput.AppendText(text);

				_rtbOutput.Select(current_len, text.Length);
				_rtbOutput.SelectionColor = Color.FromName(color);
				_rtbOutput.DeselectAll();
			});
		}

	}
}