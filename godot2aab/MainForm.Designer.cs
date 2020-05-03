namespace godot2aab
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TutorialLabel = new System.Windows.Forms.LinkLabel();
            this.ProjectPathLabel = new System.Windows.Forms.Label();
            this.BuiltAPKLabel = new System.Windows.Forms.Label();
            this.projPathTextbox = new System.Windows.Forms.TextBox();
            this.builtAPKTextbox = new System.Windows.Forms.TextBox();
            this.askForProjectButton = new System.Windows.Forms.Button();
            this.builtAPKPath = new System.Windows.Forms.Button();
            this.apktoolLabel = new System.Windows.Forms.Label();
            this.apktoolPathTextbox = new System.Windows.Forms.TextBox();
            this.apktoolChoosePath = new System.Windows.Forms.Button();
            this.coolLabel = new System.Windows.Forms.Label();
            this.checkBoxStudio = new System.Windows.Forms.CheckBox();
            this.checkBoxBackup = new System.Windows.Forms.CheckBox();
            this.buttonDoit = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.hintTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.ClearLogButton = new System.Windows.Forms.Button();
            this.askForDialog = new System.Windows.Forms.OpenFileDialog();
            this.randomQuoteLabel = new System.Windows.Forms.Label();
            this.AboutButton = new System.Windows.Forms.Button();
            this.cleanTempDirsButton = new System.Windows.Forms.Button();
            this.androidStudioTextbox = new System.Windows.Forms.TextBox();
            this.studioAskButton = new System.Windows.Forms.Button();
            this.removeConfigButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.killJavaButton = new System.Windows.Forms.Button();
            this.heyHintLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TutorialLabel
            // 
            this.TutorialLabel.AutoSize = true;
            this.TutorialLabel.Location = new System.Drawing.Point(13, 425);
            this.TutorialLabel.Name = "TutorialLabel";
            this.TutorialLabel.Size = new System.Drawing.Size(383, 13);
            this.TutorialLabel.TabIndex = 0;
            this.TutorialLabel.TabStop = true;
            this.TutorialLabel.Text = "Make sure you follow \"Custom Android Build\" guide by Godot before continuing!";
            this.TutorialLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.TutorialLabel_LinkClicked);
            // 
            // ProjectPathLabel
            // 
            this.ProjectPathLabel.AutoSize = true;
            this.ProjectPathLabel.Location = new System.Drawing.Point(13, 88);
            this.ProjectPathLabel.Name = "ProjectPathLabel";
            this.ProjectPathLabel.Size = new System.Drawing.Size(68, 13);
            this.ProjectPathLabel.TabIndex = 1;
            this.ProjectPathLabel.Text = "Project Path:";
            this.hintTooltip.SetToolTip(this.ProjectPathLabel, "Path to your Godot project");
            // 
            // BuiltAPKLabel
            // 
            this.BuiltAPKLabel.AutoSize = true;
            this.BuiltAPKLabel.Location = new System.Drawing.Point(2, 115);
            this.BuiltAPKLabel.Name = "BuiltAPKLabel";
            this.BuiltAPKLabel.Size = new System.Drawing.Size(79, 13);
            this.BuiltAPKLabel.TabIndex = 2;
            this.BuiltAPKLabel.Text = "Built APK Path:";
            this.hintTooltip.SetToolTip(this.BuiltAPKLabel, "Your built apk with game assets");
            // 
            // projPathTextbox
            // 
            this.projPathTextbox.Location = new System.Drawing.Point(88, 85);
            this.projPathTextbox.Name = "projPathTextbox";
            this.projPathTextbox.ReadOnly = true;
            this.projPathTextbox.Size = new System.Drawing.Size(670, 20);
            this.projPathTextbox.TabIndex = 3;
            // 
            // builtAPKTextbox
            // 
            this.builtAPKTextbox.Location = new System.Drawing.Point(88, 112);
            this.builtAPKTextbox.Name = "builtAPKTextbox";
            this.builtAPKTextbox.ReadOnly = true;
            this.builtAPKTextbox.Size = new System.Drawing.Size(670, 20);
            this.builtAPKTextbox.TabIndex = 4;
            // 
            // askForProjectButton
            // 
            this.askForProjectButton.Location = new System.Drawing.Point(764, 83);
            this.askForProjectButton.Name = "askForProjectButton";
            this.askForProjectButton.Size = new System.Drawing.Size(24, 23);
            this.askForProjectButton.TabIndex = 5;
            this.askForProjectButton.Text = "...";
            this.askForProjectButton.UseVisualStyleBackColor = true;
            this.askForProjectButton.Click += new System.EventHandler(this.askForProjectButton_Click);
            // 
            // builtAPKPath
            // 
            this.builtAPKPath.Location = new System.Drawing.Point(764, 110);
            this.builtAPKPath.Name = "builtAPKPath";
            this.builtAPKPath.Size = new System.Drawing.Size(24, 23);
            this.builtAPKPath.TabIndex = 6;
            this.builtAPKPath.Text = "...";
            this.builtAPKPath.UseVisualStyleBackColor = true;
            this.builtAPKPath.Click += new System.EventHandler(this.builtAPKPath_Click);
            // 
            // apktoolLabel
            // 
            this.apktoolLabel.AutoSize = true;
            this.apktoolLabel.Location = new System.Drawing.Point(11, 141);
            this.apktoolLabel.Name = "apktoolLabel";
            this.apktoolLabel.Size = new System.Drawing.Size(70, 13);
            this.apktoolLabel.TabIndex = 7;
            this.apktoolLabel.Text = "apktool Path:";
            this.hintTooltip.SetToolTip(this.apktoolLabel, "Path to apktool.jar");
            // 
            // apktoolPathTextbox
            // 
            this.apktoolPathTextbox.Location = new System.Drawing.Point(88, 138);
            this.apktoolPathTextbox.Name = "apktoolPathTextbox";
            this.apktoolPathTextbox.ReadOnly = true;
            this.apktoolPathTextbox.Size = new System.Drawing.Size(670, 20);
            this.apktoolPathTextbox.TabIndex = 8;
            // 
            // apktoolChoosePath
            // 
            this.apktoolChoosePath.Location = new System.Drawing.Point(764, 136);
            this.apktoolChoosePath.Name = "apktoolChoosePath";
            this.apktoolChoosePath.Size = new System.Drawing.Size(24, 23);
            this.apktoolChoosePath.TabIndex = 9;
            this.apktoolChoosePath.Text = "...";
            this.apktoolChoosePath.UseVisualStyleBackColor = true;
            this.apktoolChoosePath.Click += new System.EventHandler(this.apktoolChoosePath_Click);
            // 
            // coolLabel
            // 
            this.coolLabel.AutoSize = true;
            this.coolLabel.Font = new System.Drawing.Font("Comic Sans MS", 24F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coolLabel.Location = new System.Drawing.Point(12, 9);
            this.coolLabel.Name = "coolLabel";
            this.coolLabel.Size = new System.Drawing.Size(176, 45);
            this.coolLabel.TabIndex = 10;
            this.coolLabel.Text = "godot2aab";
            this.coolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.hintTooltip.SetToolTip(this.coolLabel, "ah, comic sans, t r u e   b e a u t y");
            // 
            // checkBoxStudio
            // 
            this.checkBoxStudio.AutoSize = true;
            this.checkBoxStudio.Location = new System.Drawing.Point(88, 165);
            this.checkBoxStudio.Name = "checkBoxStudio";
            this.checkBoxStudio.Size = new System.Drawing.Size(428, 17);
            this.checkBoxStudio.TabIndex = 11;
            this.checkBoxStudio.Text = "Checked - Open project in Android Studio. Unchecked - Invoke Gradle from cmd line" +
    ".";
            this.checkBoxStudio.UseVisualStyleBackColor = true;
            // 
            // checkBoxBackup
            // 
            this.checkBoxBackup.AutoSize = true;
            this.checkBoxBackup.Checked = true;
            this.checkBoxBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBackup.Enabled = false;
            this.checkBoxBackup.Location = new System.Drawing.Point(88, 188);
            this.checkBoxBackup.Name = "checkBoxBackup";
            this.checkBoxBackup.Size = new System.Drawing.Size(514, 17);
            this.checkBoxBackup.TabIndex = 12;
            this.checkBoxBackup.Text = "Backup your template project and do all operations in a copy of it? (causing prob" +
    "lems when unchecked)";
            this.checkBoxBackup.UseVisualStyleBackColor = true;
            // 
            // buttonDoit
            // 
            this.buttonDoit.Location = new System.Drawing.Point(712, 222);
            this.buttonDoit.Name = "buttonDoit";
            this.buttonDoit.Size = new System.Drawing.Size(76, 23);
            this.buttonDoit.TabIndex = 13;
            this.buttonDoit.Text = "Build .aab";
            this.buttonDoit.UseVisualStyleBackColor = true;
            this.buttonDoit.Click += new System.EventHandler(this.buttonDoit_Click);
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(12, 252);
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(776, 170);
            this.LogBox.TabIndex = 14;
            this.LogBox.Text = "";
            this.LogBox.TextChanged += new System.EventHandler(this.LogBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "A. Studio Path:";
            this.hintTooltip.SetToolTip(this.label1, "(Optional) Android Studio path.");
            // 
            // ClearLogButton
            // 
            this.ClearLogButton.Location = new System.Drawing.Point(648, 222);
            this.ClearLogButton.Name = "ClearLogButton";
            this.ClearLogButton.Size = new System.Drawing.Size(58, 23);
            this.ClearLogButton.TabIndex = 15;
            this.ClearLogButton.Text = "Clear log";
            this.ClearLogButton.UseVisualStyleBackColor = true;
            this.ClearLogButton.Click += new System.EventHandler(this.ClearLogButton_Click);
            // 
            // askForDialog
            // 
            this.askForDialog.FileName = "apktool.jar";
            // 
            // randomQuoteLabel
            // 
            this.randomQuoteLabel.AutoSize = true;
            this.randomQuoteLabel.Location = new System.Drawing.Point(195, 28);
            this.randomQuoteLabel.Name = "randomQuoteLabel";
            this.randomQuoteLabel.Size = new System.Drawing.Size(171, 13);
            this.randomQuoteLabel.TabIndex = 16;
            this.randomQuoteLabel.Text = " - made by cats, with cats, for cats.";
            // 
            // AboutButton
            // 
            this.AboutButton.Location = new System.Drawing.Point(551, 222);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(91, 23);
            this.AboutButton.TabIndex = 17;
            this.AboutButton.Text = "About this tool...";
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // cleanTempDirsButton
            // 
            this.cleanTempDirsButton.Location = new System.Drawing.Point(455, 222);
            this.cleanTempDirsButton.Name = "cleanTempDirsButton";
            this.cleanTempDirsButton.Size = new System.Drawing.Size(90, 23);
            this.cleanTempDirsButton.TabIndex = 18;
            this.cleanTempDirsButton.Text = "Clean temp dirs";
            this.cleanTempDirsButton.UseVisualStyleBackColor = true;
            this.cleanTempDirsButton.Click += new System.EventHandler(this.cleanTempDirsButton_Click);
            // 
            // androidStudioTextbox
            // 
            this.androidStudioTextbox.Location = new System.Drawing.Point(88, 59);
            this.androidStudioTextbox.Name = "androidStudioTextbox";
            this.androidStudioTextbox.ReadOnly = true;
            this.androidStudioTextbox.Size = new System.Drawing.Size(670, 20);
            this.androidStudioTextbox.TabIndex = 19;
            // 
            // studioAskButton
            // 
            this.studioAskButton.Location = new System.Drawing.Point(764, 57);
            this.studioAskButton.Name = "studioAskButton";
            this.studioAskButton.Size = new System.Drawing.Size(24, 23);
            this.studioAskButton.TabIndex = 20;
            this.studioAskButton.Text = "...";
            this.studioAskButton.UseVisualStyleBackColor = true;
            this.studioAskButton.Click += new System.EventHandler(this.studioAskButton_Click);
            // 
            // removeConfigButton
            // 
            this.removeConfigButton.Location = new System.Drawing.Point(361, 222);
            this.removeConfigButton.Name = "removeConfigButton";
            this.removeConfigButton.Size = new System.Drawing.Size(88, 23);
            this.removeConfigButton.TabIndex = 22;
            this.removeConfigButton.Text = "Remove config";
            this.removeConfigButton.UseVisualStyleBackColor = true;
            this.removeConfigButton.Click += new System.EventHandler(this.removeConfigButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 223);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(265, 23);
            this.progressBar.TabIndex = 23;
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(12, 204);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(51, 13);
            this.progressLabel.TabIndex = 24;
            this.progressLabel.Text = "Progress:";
            // 
            // killJavaButton
            // 
            this.killJavaButton.Location = new System.Drawing.Point(283, 222);
            this.killJavaButton.Name = "killJavaButton";
            this.killJavaButton.Size = new System.Drawing.Size(72, 23);
            this.killJavaButton.TabIndex = 25;
            this.killJavaButton.Text = "Kill java.exe";
            this.killJavaButton.UseVisualStyleBackColor = true;
            this.killJavaButton.Click += new System.EventHandler(this.killJavaButton_Click);
            // 
            // heyHintLabel
            // 
            this.heyHintLabel.AutoSize = true;
            this.heyHintLabel.Location = new System.Drawing.Point(478, 9);
            this.heyHintLabel.Name = "heyHintLabel";
            this.heyHintLabel.Size = new System.Drawing.Size(310, 13);
            this.heyHintLabel.TabIndex = 26;
            this.heyHintLabel.Text = "You can hover your mouse on some labels to see additional info!";
            this.heyHintLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(672, 428);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(116, 13);
            this.versionLabel.TabIndex = 27;
            this.versionLabel.Text = "godot2aab version: 9.8";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.heyHintLabel);
            this.Controls.Add(this.killJavaButton);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.removeConfigButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.studioAskButton);
            this.Controls.Add(this.androidStudioTextbox);
            this.Controls.Add(this.cleanTempDirsButton);
            this.Controls.Add(this.AboutButton);
            this.Controls.Add(this.randomQuoteLabel);
            this.Controls.Add(this.ClearLogButton);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.buttonDoit);
            this.Controls.Add(this.checkBoxBackup);
            this.Controls.Add(this.checkBoxStudio);
            this.Controls.Add(this.coolLabel);
            this.Controls.Add(this.apktoolChoosePath);
            this.Controls.Add(this.apktoolPathTextbox);
            this.Controls.Add(this.apktoolLabel);
            this.Controls.Add(this.builtAPKPath);
            this.Controls.Add(this.askForProjectButton);
            this.Controls.Add(this.builtAPKTextbox);
            this.Controls.Add(this.projPathTextbox);
            this.Controls.Add(this.BuiltAPKLabel);
            this.Controls.Add(this.ProjectPathLabel);
            this.Controls.Add(this.TutorialLabel);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(816, 489);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "godot2aab: Main Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel TutorialLabel;
        private System.Windows.Forms.Label ProjectPathLabel;
        private System.Windows.Forms.Label BuiltAPKLabel;
        private System.Windows.Forms.TextBox projPathTextbox;
        private System.Windows.Forms.TextBox builtAPKTextbox;
        private System.Windows.Forms.Button askForProjectButton;
        private System.Windows.Forms.Button builtAPKPath;
        private System.Windows.Forms.Label apktoolLabel;
        private System.Windows.Forms.TextBox apktoolPathTextbox;
        private System.Windows.Forms.Button apktoolChoosePath;
        private System.Windows.Forms.Label coolLabel;
        private System.Windows.Forms.CheckBox checkBoxStudio;
        private System.Windows.Forms.CheckBox checkBoxBackup;
        private System.Windows.Forms.Button buttonDoit;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.ToolTip hintTooltip;
        private System.Windows.Forms.Button ClearLogButton;
        private System.Windows.Forms.OpenFileDialog askForDialog;
        private System.Windows.Forms.Label randomQuoteLabel;
        private System.Windows.Forms.Button AboutButton;
        private System.Windows.Forms.Button cleanTempDirsButton;
        private System.Windows.Forms.TextBox androidStudioTextbox;
        private System.Windows.Forms.Button studioAskButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button removeConfigButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button killJavaButton;
        private System.Windows.Forms.Label heyHintLabel;
        private System.Windows.Forms.Label versionLabel;
    }
}

