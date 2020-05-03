using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using IniParser;
using IniParser.Model;

/*
 * Hello, this is the whole source code of godot2aab!
 * Feel free to tinker as much as you want, send issues, pull requests, I won't mind!
 * Huge thanks to iBotPeaches for APKTool and Godot team for Godot.
 * meow.
 */


namespace godot2aab
{
    public partial class MainForm : Form
    {
        public string MyDir = AppDomain.CurrentDomain.BaseDirectory;
        public const string ConfigFname = "config.txt";
        public string JavaPath;
        public string SDKPath;
        public string ProjectDirPath = "";
        public string GameAPKPath = "";
        public string APKToolPath = "";
        public string StudioPath = "";
        public bool   DoSaveConfig = true;

        // This function is pretty bad, it's designed to read output from process.
        public string ReadString(string path, string arg = "")
        {
            var procinfo = new ProcessStartInfo
            {
                FileName = path,
                Arguments = arg,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var proc = Process.Start(procinfo);
            string ret = proc.StandardOutput.ReadToEnd() + proc.StandardError.ReadToEnd();
            while (!proc.HasExited) Application.DoEvents();
            //proc.WaitForExit();
            proc.Dispose();
            return ret;
        }

        public string FirstLine(string str)
        {
            return str.Split(Environment.NewLine.ToCharArray())[0];
        }

        public void InitializeOther()
        {
            // Detect our version.
            string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 3);
            versionLabel.Text = versionLabel.Text.Replace("9.8", Version);

            if (Debugger.IsAttached) Text += " (Running inside Visual Studio or debugger is attached)";

            print("Trying to find SDKs and Java...");

            var supposed_path = ReadString("where", "java");

            if (supposed_path.EndsWith(Environment.NewLine))
                supposed_path = supposed_path.TrimEnd(Environment.NewLine.ToCharArray());

            if (!File.Exists(supposed_path))
                print("CANNOT FIND JAVA!");
            else
            {
                print("Found Java at: " + supposed_path);
                JavaPath = supposed_path;
                var java_version = FirstLine(ReadString("java", "-version"));
                print("Java version: " + java_version);
                var sdk_supposed_path = FirstLine(Environment.ExpandEnvironmentVariables("%ANDROID_HOME%"));
                bool valid_sdk_path = Directory.Exists(sdk_supposed_path);
                print("Android SDK Path: " + (valid_sdk_path ? sdk_supposed_path : "NO ANDROID SDK FOUND!!"));
            }

            LoadConfig();

            print();
            print("godot2aab version " + Version + " is ready for you, user! meow.");
        }

        public void print(string text = "")
        {
            LogBox.Text += text + Environment.NewLine;
        }

        public MainForm()
        {
            InitializeComponent();
            InitializeOther();
            Warning(); // TODO: remove later.
        }

        // TODO: remove this later.
        private void Warning()
        {
            if (!File.Exists(MyDir + ConfigFname))
            MessageBox.Show("This tool is in development. Always make backups of your projects. This warning will show only once.", "Please read this:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void TutorialLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://docs.godotengine.org/en/stable/getting_started/workflow/export/android_custom_build.html");
        }

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            // Clear all text in LogBox :(
            LogBox.Text = string.Empty;
        }

        private void apktoolChoosePath_Click(object sender, EventArgs e)
        {
            askForDialog.Filter = "APKTool File|apktool*.jar";
            askForDialog.InitialDirectory = MyDir;
            askForDialog.CheckFileExists = true;
            askForDialog.FileName = "apktool.jar";
            askForDialog.Title = "Choose your apktool jar file.";
            var result = askForDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                APKToolPath = askForDialog.FileName;
                CheckPath(APKToolPath);
                apktoolPathTextbox.Text = APKToolPath;
            }
        }

        private void builtAPKPath_Click(object sender, EventArgs e)
        {
            askForDialog.Filter = "APK File|*.apk";
            askForDialog.InitialDirectory = MyDir;
            askForDialog.CheckFileExists = true;
            askForDialog.FileName = "mycoolgame.apk";
            askForDialog.Title = "Choose your game apk file.";
            var result = askForDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                GameAPKPath = askForDialog.FileName;
                CheckPath(GameAPKPath);
                builtAPKTextbox.Text = GameAPKPath;
            }
        }

        private void askForProjectButton_Click(object sender, EventArgs e)
        {
            askForDialog.Filter = "Project file|project.godot";
            askForDialog.InitialDirectory = MyDir;
            askForDialog.CheckFileExists = true;
            askForDialog.FileName = "project.godot";
            askForDialog.Title = "Choose your project.godot file.";
            var result = askForDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ProjectDirPath = Path.GetDirectoryName(askForDialog.FileName) + Path.DirectorySeparatorChar;
                CheckPath(ProjectDirPath);
                projPathTextbox.Text = ProjectDirPath;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DoSaveConfig) SaveConfig();
        }

        private int BoolToInt(bool value)
        {
            return Convert.ToInt32(value);
        }

        private bool IntToBool(int value)
        {
            return (value > 0) ? true : false;
        }

        private void SaveConfig()
        {
            string[] arr = new string[6];
            arr[0] = "This is godot2aab configuration file, line 2 - project path, line 3 - apk path, line 4 - apktool path, line 5 - Android Studio path, line 6 - checkbox state.";
            arr[1] = ProjectDirPath;
            arr[2] = GameAPKPath;
            arr[3] = APKToolPath;
            arr[4] = StudioPath;
            arr[5] = BoolToInt(checkBoxStudio.Checked).ToString() + BoolToInt(checkBoxBackup.Checked).ToString();
            File.WriteAllLines(MyDir + ConfigFname, arr);
            arr = new string[0];
        }

        private void LoadConfig()
        {
            if (!File.Exists(MyDir + ConfigFname)) return;

            string[] arr = File.ReadAllLines(MyDir + ConfigFname);
            ProjectDirPath = arr[1];
            GameAPKPath = arr[2];
            APKToolPath = arr[3];
            StudioPath = arr[4];
            checkBoxStudio.Checked = IntToBool(int.Parse(arr[5].Substring(0, 1)));
            checkBoxBackup.Checked = IntToBool(int.Parse(arr[5].Substring(1)));
            arr = new string[0];

            projPathTextbox.Text = ProjectDirPath;
            builtAPKTextbox.Text = GameAPKPath;
            apktoolPathTextbox.Text = APKToolPath;
            androidStudioTextbox.Text = StudioPath;
            checkBoxBackup.Checked = true; // TODO: fix no-backup mode later.
        }

        private void buttonDoit_Click(object sender, EventArgs e)
        {
            DoBuild();
        }

        private int GetRandomNumber()
        {
            var rnd = new Random();
            int ret = rnd.Next(1000, 9999);
            return ret;
        }

        //Taken from https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        private /*static*/ void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                Application.DoEvents();
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    Application.DoEvents();
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public string packageName = "";
        public string appName = "";
        public string versionCode = "";
        public string versionName = "";
        public string keystorePath = "";
        public string keystoreUser = "";
        public string keystorePass = "";
        public string keystoreDbgPath = "";
        public string keystoreDbgUser = "";
        public string keystoreDbgPass = "";
        public string gradlePath = "";

        private void ReCreateDir(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);

            Directory.CreateDirectory(path);
        }


        // Taken from https://github.com/godotengine/godot/blob/39f7a4092533a600eeada6638016af8bd4bd2271/platform/android/export/export.cpp#L424 and rewritten in C#
        private string PatchPackageName(string p_package)
        {
            if (!p_package.Contains("$genname")) return p_package;
            else
            {
                string pname = p_package;
                string basename = string.Empty;
                string name = string.Empty;
                string[] cfg = File.ReadAllLines(ProjectDirPath + "project.godot");
                bool found = false;
                for (int i = 0; i < cfg.Length; i++)
                {
                    string s = cfg[i];
                    if (s.StartsWith("config/name=\""))
                    {
                        found = true;
                        s = s.Replace("config/name=\"", string.Empty).TrimEnd('"').ToLower();
                        basename = s;
                        break;
                    }
                }
                if (!found || basename == string.Empty) basename = "noname";

                var basecharname = basename.ToCharArray();
                bool first = true;
                for (int j = 0; j < basecharname.Length; j++)
                {
                    var c = basecharname[j];
                    if (c >= '0' && c <= '9' && first) {
                        continue;
                    }
                    if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')) {
                        name += c.ToString();
                        first = false;
                    }
                }

                pname = pname.Replace("$genname", name);

                return pname;
            }
        }

        private bool CheckBeforeBuild()
        {
            bool pass = true;

            // Try to check everything.
            if (!File.Exists(APKToolPath))
            {
                MessageBox.Show("Cannot find APKTool jar file, make sure your path is correct!", "Oh no.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pass = false;
            }
            else if (!File.Exists(GameAPKPath))
            {
                MessageBox.Show("Cannot find game apk file, make sure your path is correct!", "Oh no.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pass = false;
            }
            else if (!File.Exists(ProjectDirPath + "project.godot"))
            {
                MessageBox.Show("Cannot find project file, make sure your path is correct!", "Oh no.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pass = false;
            }
            else if (!File.Exists(StudioPath) && checkBoxStudio.Checked) // it's okay if you want to use Gradle from command line, but not okay when you WANT to use Android Studio.
            {
                MessageBox.Show("Cannot find Android Studio executable, make sure your path is correct!", "Oh no.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pass = false;
            }

            return pass;
        }

        private void DoBuild()
        {
            // oh no.

            if (!CheckBeforeBuild()) return;

            buttonDoit.Enabled = false;
            cleanTempDirsButton.Enabled = false;

            InitProgressBar(15);

            print("Starting build...");
            DoProgress(1);

            string tempDir = Path.GetTempPath() + GetRandomNumber().ToString() + Path.DirectorySeparatorChar;

            gradlePath = ProjectDirPath + @"android\build";
            var iniparser = new FileIniDataParser();
            IniData data = iniparser.ReadFile(ProjectDirPath + "export_presets.cfg", Encoding.UTF8);

            print("Parsing export_presets.cfg...");
            DoProgress(2);

            foreach (var section in data.Sections)
            {
                Application.DoEvents();
                if (section.Keys.ContainsKey("platform"))
                {
                    if (section.Keys["platform"] == "\"Android\"")
                    {
                        // Names
                        packageName = data[section.SectionName + ".options"]["package/unique_name"].Replace("\"", string.Empty);
                        packageName = PatchPackageName(packageName);
                        appName = data[section.SectionName + ".options"]["package/name"].Replace("\"", string.Empty);

                        // Version info
                        versionCode = data[section.SectionName + ".options"]["version/code"].Replace("\"", string.Empty);
                        versionName = data[section.SectionName + ".options"]["version/name"].Replace("\"", string.Empty);

                        // Release Keystore
                        keystorePath = data[section.SectionName + ".options"]["keystore/release"].Replace("\"", string.Empty);
                        keystoreUser = data[section.SectionName + ".options"]["keystore/release_user"].Replace("\"", string.Empty);
                        keystorePass = data[section.SectionName + ".options"]["keystore/release_password"].Replace("\"", string.Empty);

                        // Debug Keystore
                        keystoreDbgPath = data[section.SectionName + ".options"]["keystore/debug"].Replace("\"", string.Empty);
                        keystoreDbgUser = data[section.SectionName + ".options"]["keystore/debug_user"].Replace("\"", string.Empty);
                        keystoreDbgPass = data[section.SectionName + ".options"]["keystore/debug_password"].Replace("\"", string.Empty);


                        print("Package details:");
                        print("Package id: " + packageName);
                        print("Package name: " + appName);
                        print("Package version code: " + versionCode);
                        print("Package version name: " + versionName);

                        print();
                        print("Keystore details:");
                        print("Release path: " + keystorePath);
                        print("Release user: " + keystoreUser);
                        print("Release pass: " + keystorePass);
                    }
                }
            }

            print("Patching gradle files...");
            DoProgress(3);

            //Trying to recreate our project directory.
            bool fail;
            try
            {
                ReCreateDir(gradlePath + "aab");
                fail = false;
            }
            catch
            {
                Application.DoEvents();
                MessageBox.Show("Cannot recreate 'buildaab' directory. Make sure no process is blocking godot2aab from deleting it.", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progressBar.Value = 0;
                buttonDoit.Enabled = true;
                cleanTempDirsButton.Enabled = true;
                fail = true;
            }
            if (fail) return;

            if (checkBoxBackup.Checked)
            {
                DirectoryCopy(gradlePath, gradlePath + "aab", true);
                gradlePath += @"aab\";
            }
            else gradlePath += Path.DirectorySeparatorChar;

            DoProgress(4);
            string configGradle = File.ReadAllText(gradlePath + "config.gradle");
            var buildGradle = ReadAllList(gradlePath + "build.gradle");
            configGradle = configGradle.Replace("com.godot.game", packageName);
            buildGradle = PatchBuildGradle(buildGradle);
            var manifest_xml = XDocument.Load(gradlePath + "AndroidManifest.xml");
            //print("build.gradle contents...");
            //PrintStringList(buildGradle);

            print("Patching AndroidManifest.xml...");
            manifest_xml.Element("manifest").Attribute("package").Value = packageName;
            var man_declaration = manifest_xml.Element("manifest").GetNamespaceOfPrefix("android");
            manifest_xml.Element("manifest").Attribute(man_declaration + "versionCode").Value = versionCode;
            manifest_xml.Element("manifest").Attribute(man_declaration + "versionName").Value = versionName;
            DoProgress(5);

            print("Writing patched files...");
            File.WriteAllText(gradlePath + "config.gradle", configGradle);
            WriteAllList(gradlePath + "build.gradle", buildGradle);
            File.WriteAllText(gradlePath + "AndroidManifest.xml", manifest_xml.ToString());
            DoProgress(6);

            print("Patching GodotApp.java...");
            File.WriteAllText(gradlePath + @"src\com\godot\game\GodotApp.java",
                                File.ReadAllText(gradlePath + @"src\com\godot\game\GodotApp.Java").Replace("package com.godot.game;","package " + packageName + ";")
                             );
            DoProgress(7);

            print("Removing 'build' folder...");
            try
            {
                Directory.Delete(gradlePath + "build", true);
            }
            catch { }
            DoProgress(8);

            print("Renaming subdirs in 'src' directory...");
            var splitname = packageName.Split('.');
            if (splitname[2] != "game") Directory.Move(gradlePath + @"src\com\godot\game", gradlePath + @"src\com\godot\" + splitname[2]);
            if (splitname[1] != "godot") Directory.Move(gradlePath + @"src\com\godot", gradlePath + @"src\com\" + splitname[1]);
            if (splitname[0] != "com") Directory.Move(gradlePath + @"src\com", gradlePath + @"src\" + splitname[0]);
            DoProgress(9);


            print("Unpacking your game apk file for resources...");
            print("Invoking apktool.jar...");

            string output = ReadString("java", "-jar \"" + APKToolPath + "\" d \"" + GameAPKPath + "\" -o " + tempDir);
            print(output);

            DoProgress(10);

            print("Copying game assets to Gradle project folder...");
            DirectoryCopy(tempDir + "res", gradlePath + "res", true);
            DirectoryCopy(tempDir + "assets", gradlePath + "assets", true);
            DoProgress(11);

            print("Fixing assets '.import' directory...");
            Directory.Move(gradlePath + @"assets\.import", gradlePath + @"assets\dotimport");
            foreach (string path in Directory.EnumerateFiles(gradlePath + @"assets\", "*.import", SearchOption.AllDirectories))
            {
                Application.DoEvents();
                File.WriteAllText(path, File.ReadAllText(path).Replace(".import", "dotimport"));
            }
            DoProgress(12);

            print("Fixing attrs.xml in 'res' directory...");
            var attr_xml = XDocument.Load(gradlePath + @"res\values\attrs.xml");

            //fontProviderFetchStrategy
            //fontProviderFetchTimeout
            //fontStyle
            List<XElement> to_remove = new List<XElement>();

            foreach (var element in attr_xml.Root.Elements())
            {
                string n = element.Attribute("name").Value; // name.
                if ((n == "fontProviderFetchStrategy")
                || (n == "fontProviderFetchTimeout")
                || (n == "fontStyle"))
                    to_remove.Add(element);
            }
            for (int e = 0; e < to_remove.Count; e++)
            {
                var xel = to_remove[e];
                xel.Remove();
            }

            //Save our patched AndroidManifest.xml
            attr_xml.Save(gradlePath + @"res\values\attrs.xml");

            buttonDoit.Enabled = true;
            cleanTempDirsButton.Enabled = true;

            DoProgress(13);

            if (checkBoxStudio.Checked) LaunchAndroidStudio(gradlePath);
            else BuildGradleAAB(gradlePath);

            DoProgress(14);

            print("Build finished! Enjoy!");

            DoProgress(15);
        }

        private void BuildGradleAAB(string arg)
        {
            print("Trying to invoke gradlew.bat...");
            var gradle = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c \".\\gradlew.bat bundleRelease && explorer \"" + arg + @"build\outputs\bundle\release" + "\" & pause\"",
                WorkingDirectory = arg,
                UseShellExecute = true
            };
            var proc = Process.Start(gradle);
            proc.Dispose();
        }

        private void LaunchAndroidStudio(string arg)
        {
            print("Launching Android Studio...");
            Process.Start(StudioPath, arg + "build.gradle");
        }

        private List<string> PatchBuildGradle(List<string> gradletext)
        {
            string padding = @"    ";
            int buildtype_ind = FindIndex(gradletext, "// Both signing and zip-aligning will be done at export time");
            gradletext.Insert(++buildtype_ind, padding + "/*");
            int end_ind = FindIndex(gradletext, "}", ++buildtype_ind);
            gradletext.Insert(++end_ind, padding + "*/");
            int btype_ind = end_ind;

            // construct buildTypes section
            gradletext.Insert(++btype_ind, padding);
            gradletext.Insert(++btype_ind, padding + "buildTypes {");
            gradletext.Insert(++btype_ind, padding + padding + "release {");
            gradletext.Insert(++btype_ind, padding + padding + padding + "zipAlignEnabled true");
            gradletext.Insert(++btype_ind, padding + padding + padding + "signingConfig signingConfigs.release");
            gradletext.Insert(++btype_ind, padding + padding + "}");
            gradletext.Insert(++btype_ind, padding + padding + "debug {");
            gradletext.Insert(++btype_ind, padding + padding + padding + "zipAlignEnabled true");
            gradletext.Insert(++btype_ind, padding + padding + padding + "signingConfig signingConfigs.debug");
            gradletext.Insert(++btype_ind, padding + padding + "}");
            gradletext.Insert(++btype_ind, padding + "}");

            //print(gradletext[buildtype_ind]);
            int signing_ind = FindIndex(gradletext, "exclude 'META-INF/NOTICE'") + 2;

            gradletext.Insert(++signing_ind, padding + "signingConfigs {");
            gradletext.Insert(++signing_ind, padding + padding + "release {");
            gradletext.Insert(++signing_ind, padding + padding + padding + "keyAlias '" + keystoreUser + "'");
            gradletext.Insert(++signing_ind, padding + padding + padding + "keyPassword '" + keystorePass + "'");
            gradletext.Insert(++signing_ind, padding + padding + padding + "storePassword '" + keystorePass + "'");
            gradletext.Insert(++signing_ind, padding + padding + padding + "storeFile file(\"" + keystorePath + "\")");
            gradletext.Insert(++signing_ind, padding + padding + "}");
            gradletext.Insert(++signing_ind, padding + padding + "debug {");
            gradletext.Insert(++signing_ind, padding + padding + padding + "keyAlias '" + keystoreDbgUser + "'");
            gradletext.Insert(++signing_ind, padding + padding + padding + "keyPassword '" + keystoreDbgPass + "'");
            gradletext.Insert(++signing_ind, padding + padding + padding + "storePassword '" + keystoreDbgPass + "'");
            gradletext.Insert(++signing_ind, padding + padding + padding + "storeFile file(\"" + keystoreDbgPath + "\")");
            gradletext.Insert(++signing_ind, padding + padding + "}");
            gradletext.Insert(++signing_ind, padding + "}");
            gradletext.Insert(++signing_ind, padding);

            return gradletext;
        }

        private void CheckPath(string path)
        {
            if (path.Contains(' '))
                MessageBox.Show("Your path seems to contain spaces.\nThis *may* cause issues.\nIf you can, please change it to a different folder.",
                                "Uhm...",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            // Seems to work with spaces actually, but still.
        }

        private int FindIndex(List<string> my_list, string need, int start = 0)
        {
            for (int i = start; i < my_list.Count; i++)
            {
                if (my_list[i].Contains(need))
                {
                    return i;
                }
            }
            return -1; // ??????????
        }

        private List<string> ReadAllList(string path)
        {
            string[] my_arr = File.ReadAllLines(path, Encoding.UTF8);
            var ret_list = new List<string>();
            for (int i = 0; i < my_arr.Length; i++)
                ret_list.Add(my_arr[i]);
            return ret_list;
        }

        private void WriteAllList(string path, List<string> contents)
        {
            string[] to_write = new string[contents.Count];
            for (int i = 0; i < contents.Count; i++)
            {
                to_write[i] = contents[i];
            }
            File.WriteAllLines(path, to_write);
            to_write = new string[0];
        }

        // Used for printing build.gradle when something breaks :p
        private void PrintStringList(List<string> my_arr)
        {
            for (int i = 0; i < my_arr.Count; i++)
            {
                print(my_arr[i]);
                Application.DoEvents();
            }
        }
        
        // Exactly.
        private void AboutButton_Click(object sender, EventArgs e)
        {
            string my_text =
@"Credits:
Main creator - nik the cat
Main tester - Андрей Брусник
person who said to remove all owos - YellowAfterlife
thanks to - Godot team";

            try
            {
                MessageBox.Show(my_text, "godot2aab: About...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch { }
        }

        private void cleanTempDirsButton_Click(object sender, EventArgs e)
        {
            print("Cleaning...");
            string myTemp = Path.GetTempPath();
            for (int i = 1000; i < 9999 + 1; i++)
            {
                string dirpath = myTemp + i.ToString();
                Application.DoEvents();
                if (Directory.Exists(dirpath))
                {
                    Directory.Delete(dirpath, true);
                }
            }
            print("Temp directories were deleted!");
        }

        private void studioAskButton_Click(object sender, EventArgs e)
        {
            askForDialog.Filter = "Studio exe|studio*.exe";
            askForDialog.InitialDirectory = MyDir;
            askForDialog.CheckFileExists = true;
            askForDialog.FileName = "studio64.exe";
            askForDialog.Title = "Choose your Android Studio executable.";
            var result = askForDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                StudioPath = askForDialog.FileName;
                CheckPath(StudioPath);
                androidStudioTextbox.Text = StudioPath;
            }
        }

        private void removeConfigButton_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(MyDir + ConfigFname);
            }
            catch { }

            MessageBox.Show("Config removed, the program will exit.", "Hello!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DoSaveConfig = false;
            Application.Exit();
        }

        private void DoProgress(int step)
        {
            progressBar.Value = step;
        }

        private void InitProgressBar(int maximum)
        {
            progressBar.Maximum = maximum;
        }

        // Auto-scroll our LogBox.
        private void LogBox_TextChanged(object sender, EventArgs e)
        {
            LogBox.SelectionStart = LogBox.Text.Length;
            LogBox.ScrollToCaret();
        }

        private void killJavaButton_Click(object sender, EventArgs e)
        {
            print("Killing all java.exe processes...");
            int count = 0;
            foreach (var proc in Process.GetProcessesByName("java"))
            {
                proc.Kill();
                count++;
                Application.DoEvents();
            }
            print("Killed " + count.ToString() + " processes!");
        }
    }
}
