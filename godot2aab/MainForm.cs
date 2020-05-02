using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using IniParser;
using IniParser.Model;

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
            proc.WaitForExit();
            proc.Dispose();
            return ret;
        }

        public string FirstLine(string str)
        {
            return str.Split(Environment.NewLine.ToCharArray())[0];
        }

        public void InitializeOther()
        {
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
            print("godot2aab ready for you, user! meow.");
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

        private void Warning()
        {
            if (!File.Exists(MyDir + "config.txt"))
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

        private void DoBuild()
        {
            // oh no.

            buttonDoit.Enabled = false;
            cleanTempDirsButton.Enabled = false;

            print("Starting build...");

            string tempDir = Path.GetTempPath() + GetRandomNumber().ToString() + Path.DirectorySeparatorChar;

            gradlePath = ProjectDirPath + @"android\build";
            var iniparser = new FileIniDataParser();
            IniData data = iniparser.ReadFile(ProjectDirPath + "export_presets.cfg");

            print("Parsing export_presets.cfg...");

            foreach (var section in data.Sections)
            {
                if (section.Keys.ContainsKey("platform"))
                {
                    if (section.Keys["platform"] == "\"Android\"")
                    {
                        // Names
                        packageName = data[section.SectionName + ".options"]["package/unique_name"].Replace("\"", string.Empty);
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
            ReCreateDir(gradlePath + "aab");
            if (checkBoxBackup.Checked)
            {
                DirectoryCopy(gradlePath, gradlePath + "aab", true);
                gradlePath += @"aab\";
            }
            else gradlePath += @"\";

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

            print("Writing patched files...");
            File.WriteAllText(gradlePath + "config.gradle", configGradle);
            WriteAllList(gradlePath + "build.gradle", buildGradle);
            File.WriteAllText(gradlePath + "AndroidManifest.xml", manifest_xml.ToString());

            print("Patching GodotApp.java...");
            File.WriteAllText(gradlePath + @"src\com\godot\game\GodotApp.java",
                                File.ReadAllText(gradlePath + @"src\com\godot\game\GodotApp.Java").Replace("package com.godot.game;","package " + packageName + ";")
                             );

            print("Removing 'build' folder...");
            try
            {
                Directory.Delete(gradlePath + "build", true);
            }
            catch { }

            print("Renaming subdirs in 'src' directory...");
            var splitname = packageName.Split('.');
            Directory.Move(gradlePath + @"src\com\godot\game", gradlePath + @"src\com\godot\" + splitname[2]);
            Directory.Move(gradlePath + @"src\com\godot", gradlePath + @"src\com\" + splitname[1]);
            Directory.Move(gradlePath + @"src\com", gradlePath + @"src\" + splitname[0]);


            print("Unpacking your game apk file for resources...");
            print("Invoking apktool.jar...");

            string output = ReadString("java", "-jar " + APKToolPath + " d " + GameAPKPath + " -o " + tempDir);
            print(output);

            print("Copying game assets to Gradle project folder...");
            DirectoryCopy(tempDir + "res", gradlePath + "res", true);
            DirectoryCopy(tempDir + "assets", gradlePath + "assets", true);

            print("Fixing assets '.import' directory...");
            Directory.Move(gradlePath + @"assets\.import", gradlePath + @"assets\dotimport");
            foreach (string path in Directory.EnumerateFiles(gradlePath + @"assets\", "*.import", SearchOption.AllDirectories))
            {
                Application.DoEvents();
                File.WriteAllText(path, File.ReadAllText(path).Replace(".import", "dotimport"));
            }

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

            attr_xml.Save(gradlePath + @"res\values\attrs.xml");

            buttonDoit.Enabled = true;
            cleanTempDirsButton.Enabled = true;

            if (checkBoxStudio.Checked) LaunchAndroidStudio(gradlePath);
            else BuildGradleAAB(gradlePath);

            print("Build finished! Enjoy!");
        }

        private void BuildGradleAAB(string arg)
        {
            print("Trying to invoke gradlew.bat...");
            var gradle = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c \".\\gradlew.bat bundleRelease & pause\"",
                WorkingDirectory = arg,
                UseShellExecute = true
            };
            var proc = Process.Start(gradle);
            proc.WaitForExit();
            proc.Dispose();

            Process.Start("explorer.exe", arg + @"build\outputs\bundle\release\");
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
            MessageBox.Show("Your path seems to contain spaces.\nThis *will* cause issues.\nPlease change it to a different folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        
        private void PrintStringList(List<string> my_arr)
        {
            for (int i = 0; i < my_arr.Count; i++)
            {
                print(my_arr[i]);
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
                File.Delete(MyDir + "config.txt");
            }
            catch { }

            MessageBox.Show("Config removed, the program will exit.", "Hello!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DoSaveConfig = false;
            Application.Exit();
        }
    }
}
