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
			_rtbOutput.Text = "";
		}

		private void openFormIfNotOpen() {
			this.Show();
		}

		public void writeLine(string text) {
			openFormIfNotOpen();
			writeLine(text, "white");
		}

		public void writeLine(string text, string color) {
			openFormIfNotOpen();
			this.Invoke((MethodInvoker)delegate {
				int current_len = this._rtbOutput.TextLength;
				this._rtbOutput.AppendText(text);
				this._rtbOutput.AppendText("\n");

				this._rtbOutput.Select(current_len, text.Length + 1);
				this._rtbOutput.SelectionColor = Color.FromName(color);
				this._rtbOutput.DeselectAll();
			});
		}

		public void write(string text) {
			openFormIfNotOpen();
			write(text, "white");
		}

		public void write(string text, string color) {
			openFormIfNotOpen();
			this.Invoke((MethodInvoker)delegate {
				int current_len = this._rtbOutput.TextLength;
				this._rtbOutput.AppendText(text);

				this._rtbOutput.Select(current_len, text.Length);
				this._rtbOutput.SelectionColor = Color.FromName(color);
				this._rtbOutput.DeselectAll();
			});
		}

		private void Console_FormClosing(object sender, FormClosingEventArgs e) {
			e.Cancel = true;
			Hide();
		}

		private void Console_Load(object sender, EventArgs e) {

		}

	}
}