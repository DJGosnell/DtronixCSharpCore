namespace dtxCore.Forms {
	partial class frmConsole {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this._rtbOutput = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// _rtbOutput
			// 
			this._rtbOutput.BackColor = System.Drawing.Color.Black;
			this._rtbOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._rtbOutput.DetectUrls = false;
			this._rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this._rtbOutput.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._rtbOutput.ForeColor = System.Drawing.Color.White;
			this._rtbOutput.Location = new System.Drawing.Point(0, 0);
			this._rtbOutput.Name = "_rtbOutput";
			this._rtbOutput.ReadOnly = true;
			this._rtbOutput.Size = new System.Drawing.Size(612, 333);
			this._rtbOutput.TabIndex = 0;
			this._rtbOutput.Text = "Loading...";
			// 
			// frmConsole
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(612, 333);
			this.Controls.Add(this._rtbOutput);
			this.Name = "frmConsole";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Console Output";
			this.Load += new System.EventHandler(this.frmConsole_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox _rtbOutput;

	}
}