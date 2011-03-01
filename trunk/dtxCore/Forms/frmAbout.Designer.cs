namespace dtxCore.Forms {
	partial class frmAbout {
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
			this._tabContainer = new System.Windows.Forms.TabControl();
			this._tabMainProgram = new System.Windows.Forms.TabPage();
			this._lblMainProgramDescription = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this._rtbMainProgramLicense = new System.Windows.Forms.RichTextBox();
			this._tabDtxCore = new System.Windows.Forms.TabPage();
			this._lblDtxCoreCopyright = new System.Windows.Forms.Label();
			this._lblDtxCoreVersion = new System.Windows.Forms.Label();
			this._lblDtxCoreTitle = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this._rtbDtxCoreLicense = new System.Windows.Forms.RichTextBox();
			this._lstdtxCoreLiscenseSelector = new System.Windows.Forms.ListBox();
			this._picMainProjectLogo = new System.Windows.Forms.PictureBox();
			this._btnClose = new System.Windows.Forms.Button();
			this._tabContainer.SuspendLayout();
			this._tabMainProgram.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this._tabDtxCore.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picMainProjectLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// _tabContainer
			// 
			this._tabContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tabContainer.Controls.Add(this._tabMainProgram);
			this._tabContainer.Controls.Add(this._tabDtxCore);
			this._tabContainer.Location = new System.Drawing.Point(1, 91);
			this._tabContainer.Name = "_tabContainer";
			this._tabContainer.SelectedIndex = 0;
			this._tabContainer.Size = new System.Drawing.Size(354, 435);
			this._tabContainer.TabIndex = 0;
			// 
			// _tabMainProgram
			// 
			this._tabMainProgram.Controls.Add(this._lblMainProgramDescription);
			this._tabMainProgram.Controls.Add(this.groupBox2);
			this._tabMainProgram.Location = new System.Drawing.Point(4, 22);
			this._tabMainProgram.Name = "_tabMainProgram";
			this._tabMainProgram.Padding = new System.Windows.Forms.Padding(3);
			this._tabMainProgram.Size = new System.Drawing.Size(346, 409);
			this._tabMainProgram.TabIndex = 0;
			this._tabMainProgram.Text = "Main Project Name";
			this._tabMainProgram.UseVisualStyleBackColor = true;
			// 
			// _lblMainProgramDescription
			// 
			this._lblMainProgramDescription.AutoSize = true;
			this._lblMainProgramDescription.Location = new System.Drawing.Point(8, 3);
			this._lblMainProgramDescription.Name = "_lblMainProgramDescription";
			this._lblMainProgramDescription.Size = new System.Drawing.Size(60, 13);
			this._lblMainProgramDescription.TabIndex = 6;
			this._lblMainProgramDescription.Text = "Description";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this._rtbMainProgramLicense);
			this.groupBox2.Location = new System.Drawing.Point(3, 54);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(340, 352);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "License";
			// 
			// _rtbMainProgramLicense
			// 
			this._rtbMainProgramLicense.BackColor = System.Drawing.Color.White;
			this._rtbMainProgramLicense.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._rtbMainProgramLicense.Cursor = System.Windows.Forms.Cursors.Arrow;
			this._rtbMainProgramLicense.Location = new System.Drawing.Point(6, 19);
			this._rtbMainProgramLicense.Name = "_rtbMainProgramLicense";
			this._rtbMainProgramLicense.ReadOnly = true;
			this._rtbMainProgramLicense.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this._rtbMainProgramLicense.Size = new System.Drawing.Size(328, 311);
			this._rtbMainProgramLicense.TabIndex = 4;
			this._rtbMainProgramLicense.Text = "";
			this._rtbMainProgramLicense.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this._rtbMainProgramLicense_LinkClicked);
			// 
			// _tabDtxCore
			// 
			this._tabDtxCore.Controls.Add(this._lblDtxCoreCopyright);
			this._tabDtxCore.Controls.Add(this._lblDtxCoreVersion);
			this._tabDtxCore.Controls.Add(this._lblDtxCoreTitle);
			this._tabDtxCore.Controls.Add(this.groupBox1);
			this._tabDtxCore.Location = new System.Drawing.Point(4, 22);
			this._tabDtxCore.Name = "_tabDtxCore";
			this._tabDtxCore.Padding = new System.Windows.Forms.Padding(3);
			this._tabDtxCore.Size = new System.Drawing.Size(346, 409);
			this._tabDtxCore.TabIndex = 1;
			this._tabDtxCore.Text = "dtxCore";
			this._tabDtxCore.UseVisualStyleBackColor = true;
			// 
			// _lblDtxCoreCopyright
			// 
			this._lblDtxCoreCopyright.AutoSize = true;
			this._lblDtxCoreCopyright.Location = new System.Drawing.Point(8, 29);
			this._lblDtxCoreCopyright.Name = "_lblDtxCoreCopyright";
			this._lblDtxCoreCopyright.Size = new System.Drawing.Size(51, 13);
			this._lblDtxCoreCopyright.TabIndex = 3;
			this._lblDtxCoreCopyright.Text = "Copyright";
			// 
			// _lblDtxCoreVersion
			// 
			this._lblDtxCoreVersion.AutoSize = true;
			this._lblDtxCoreVersion.Location = new System.Drawing.Point(8, 16);
			this._lblDtxCoreVersion.Name = "_lblDtxCoreVersion";
			this._lblDtxCoreVersion.Size = new System.Drawing.Size(45, 13);
			this._lblDtxCoreVersion.TabIndex = 2;
			this._lblDtxCoreVersion.Text = "Version ";
			// 
			// _lblDtxCoreTitle
			// 
			this._lblDtxCoreTitle.AutoSize = true;
			this._lblDtxCoreTitle.Location = new System.Drawing.Point(6, 3);
			this._lblDtxCoreTitle.Name = "_lblDtxCoreTitle";
			this._lblDtxCoreTitle.Size = new System.Drawing.Size(65, 13);
			this._lblDtxCoreTitle.TabIndex = 1;
			this._lblDtxCoreTitle.Text = "Dtronix Core";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this._rtbDtxCoreLicense);
			this.groupBox1.Controls.Add(this._lstdtxCoreLiscenseSelector);
			this.groupBox1.Location = new System.Drawing.Point(3, 54);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(340, 352);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Dtronix Core technologies and licences";
			// 
			// _rtbDtxCoreLicense
			// 
			this._rtbDtxCoreLicense.BackColor = System.Drawing.Color.White;
			this._rtbDtxCoreLicense.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._rtbDtxCoreLicense.Cursor = System.Windows.Forms.Cursors.Arrow;
			this._rtbDtxCoreLicense.Location = new System.Drawing.Point(6, 105);
			this._rtbDtxCoreLicense.Name = "_rtbDtxCoreLicense";
			this._rtbDtxCoreLicense.ReadOnly = true;
			this._rtbDtxCoreLicense.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this._rtbDtxCoreLicense.Size = new System.Drawing.Size(328, 241);
			this._rtbDtxCoreLicense.TabIndex = 3;
			this._rtbDtxCoreLicense.Text = "";
			this._rtbDtxCoreLicense.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this._rtbDtxCoreLicense_LinkClicked);
			// 
			// _lstdtxCoreLiscenseSelector
			// 
			this._lstdtxCoreLiscenseSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._lstdtxCoreLiscenseSelector.FormattingEnabled = true;
			this._lstdtxCoreLiscenseSelector.Location = new System.Drawing.Point(6, 19);
			this._lstdtxCoreLiscenseSelector.Name = "_lstdtxCoreLiscenseSelector";
			this._lstdtxCoreLiscenseSelector.ScrollAlwaysVisible = true;
			this._lstdtxCoreLiscenseSelector.Size = new System.Drawing.Size(328, 80);
			this._lstdtxCoreLiscenseSelector.TabIndex = 1;
			this._lstdtxCoreLiscenseSelector.SelectedIndexChanged += new System.EventHandler(this._lstdtxCoreLiscenseSelector_SelectedIndexChanged);
			// 
			// _picMainProjectLogo
			// 
			this._picMainProjectLogo.Dock = System.Windows.Forms.DockStyle.Top;
			this._picMainProjectLogo.Location = new System.Drawing.Point(0, 0);
			this._picMainProjectLogo.Name = "_picMainProjectLogo";
			this._picMainProjectLogo.Size = new System.Drawing.Size(354, 90);
			this._picMainProjectLogo.TabIndex = 1;
			this._picMainProjectLogo.TabStop = false;
			// 
			// _btnClose
			// 
			this._btnClose.Location = new System.Drawing.Point(267, 532);
			this._btnClose.Name = "_btnClose";
			this._btnClose.Size = new System.Drawing.Size(75, 23);
			this._btnClose.TabIndex = 2;
			this._btnClose.Text = "O&K";
			this._btnClose.UseVisualStyleBackColor = true;
			this._btnClose.Click += new System.EventHandler(this._btnClose_Click);
			// 
			// frmAbout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(354, 563);
			this.Controls.Add(this._tabContainer);
			this.Controls.Add(this._btnClose);
			this.Controls.Add(this._picMainProjectLogo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(360, 591);
			this.MinimumSize = new System.Drawing.Size(360, 591);
			this.Name = "frmAbout";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About";
			this.Load += new System.EventHandler(this.frmAbout_Load);
			this._tabContainer.ResumeLayout(false);
			this._tabMainProgram.ResumeLayout(false);
			this._tabMainProgram.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this._tabDtxCore.ResumeLayout(false);
			this._tabDtxCore.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._picMainProjectLogo)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl _tabContainer;
		private System.Windows.Forms.TabPage _tabMainProgram;
		private System.Windows.Forms.TabPage _tabDtxCore;
		private System.Windows.Forms.Label _lblDtxCoreVersion;
		private System.Windows.Forms.Label _lblDtxCoreTitle;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox _lstdtxCoreLiscenseSelector;
		private System.Windows.Forms.PictureBox _picMainProjectLogo;
		private System.Windows.Forms.Label _lblDtxCoreCopyright;
		private System.Windows.Forms.RichTextBox _rtbDtxCoreLicense;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RichTextBox _rtbMainProgramLicense;
		private System.Windows.Forms.Label _lblMainProgramDescription;
		private System.Windows.Forms.Button _btnClose;
	}
}