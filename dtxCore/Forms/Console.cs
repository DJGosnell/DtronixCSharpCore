using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dtxCore {
	public partial class Console : Form {

		private static Console open_form;
		public static bool debug_mode = false;

		public Console() {
			InitializeComponent();
			_rtbOutput.Text = "";
		}

		private static void openFormIfNotOpen() {
			if(open_form == null) {
				open_form = new Console();
				open_form.Show();
			} else {
				open_form.Show();
			}
		}

		public static void writeLine(string text) {
			if(!debug_mode)
				return;

			openFormIfNotOpen();
			writeLine(text, "white");
		}

		public static void writeLine(string text, string color) {
			if(!debug_mode)
				return;

			openFormIfNotOpen();
			open_form.Invoke((MethodInvoker)delegate {
				int current_len = open_form._rtbOutput.TextLength;
				open_form._rtbOutput.AppendText(text);
				open_form._rtbOutput.AppendText("\n");

				open_form._rtbOutput.Select(current_len, text.Length + 1);
				open_form._rtbOutput.SelectionColor = Color.FromName(color);
				open_form._rtbOutput.DeselectAll();
			});
		}

		public static void write(string text) {
			if(!debug_mode)
				return;

			openFormIfNotOpen();
			write(text, "white");
		}

		public static void write(string text, string color) {
			if(!debug_mode)
				return;

			openFormIfNotOpen();
			open_form.Invoke((MethodInvoker)delegate {
				int current_len = open_form._rtbOutput.TextLength;
				open_form._rtbOutput.AppendText(text);

				open_form._rtbOutput.Select(current_len, text.Length);
				open_form._rtbOutput.SelectionColor = Color.FromName(color);
				open_form._rtbOutput.DeselectAll();
			});
		}

		private void Console_FormClosing(object sender, FormClosingEventArgs e) {
			e.Cancel = true;
			Hide();
		}

	}
}