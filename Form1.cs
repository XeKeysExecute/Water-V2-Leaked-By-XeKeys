using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XDevkit;
using JRPC_Client;
using DevExpress.XtraEditors;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics; 
using System.Security.Cryptography;
using DevExpress.Utils.Extensions;
using System.Timers;

namespace DXApplication6
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        IXboxConsole console;
        private string Length;
        private uint BO2CMD;
        private uint PrestigeOffset;
        private uint MicOffset;
        private uint LevelOffset;
        private uint MachineOffset;
        private readonly string string_0;
        private readonly uint BO2SV;
        private readonly object ixboxConsole_0;
        private static readonly object LockPreGameBo2;
        private readonly object ini_0;
        private readonly object SMCReturn;

        public bool bool_0 { get; private set; }
        public static object TRAY_STATE { get; private set; }

        public Form1()
        {

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (console.Connect(out console))
            {
                button1.Text = "Re/Connected To console";
                console.XNotify("Welcome To Water CR v2!", 5);
            }
            else
            {
                button1.Text = "Error Connecting!";
                button1.ForeColor = Color.Red;
                if (MessageBox.Show("ERROR: Please check that you have JRPC2 as a Plugin!\nMake Sure your console is set as Default on Xbox 360 Neighborhood\nMake sure all .dlls are in the Water File", "Need Help?", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) ;
            }
        }

        public void offhostkicknop()
        {
            console.WriteUInt32(0x822781D8, 0x60000000);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            textEdit97.Text = "CPU Key: " + console.GetCPUKey();
            textEdit93.Text = "Kernel: " + console.GetKernalVersion();
            textEdit96.Text = "console IP: " + console.XboxIP();
            textEdit95.Text = "console Type: " + console.ConsoleType();
            textEdit94.Text = "GPU Temperature: " + console.GetTemperature(JRPC.TemperatureType.GPU).ToString();
            textEdit90.Text = "CPU Temperature: " + console.GetTemperature(JRPC.TemperatureType.CPU).ToString();
            textEdit92.Text = "MotherBoard Temperature: " + console.GetTemperature(JRPC.TemperatureType.MotherBoard);
            textEdit91.Text = "RAM Temperature: " + console.GetTemperature(JRPC.TemperatureType.EDRAM);
            textEdit89.Text = "Hash: " + console.GetHashCode();
            textEdit86.Text = "console Name: " + console.Name;
            textEdit88.Text = "Title ID: " + JRPC.XamGetCurrentTitleId(console).ToString();
            textEdit87.Text = "Game ID: " + JRPC.XamGetCurrentTitleId(console).ToString("X");
            textEdit83.Text = "Gamertag: " + Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 2175412476U, 30U)).Trim().Trim(new char[1]);
            uint num = this.console.ResolveFunction("xam.xex", 2601U) + 12288U;
            int num2 = 0;
            textEdit6.Text = "XUID: " + console.ReadUInt64(num).ToString("X16");
            textEdit114.Text = "Connect Timeout: " + console.ConnectTimeout;
            textEdit135.Text = "Manager: " + console.XboxManager;
            textEdit133.Text = "IPAddressTitle: " + console.IPAddressTitle;
        }


        public string GetGSCMenu()
        {
            string detectedMenu = "";
            string MenuDetected = "";
            MenuDetected = console.ReadString(0x82C5B975, 0x1FFFF).ToLower();
            string MenuDetected6 = console.ReadString(0x82C5F3D6, 0xFFFF).ToLower();
            string MenuDetected2 = console.ReadString(0x82C5F32E, 0xFFFF).ToLower();
            string MenuDetected3 = console.ReadString(0x82C5E6FE, 0xFFFF).ToLower();
            string MenuDetected4 = console.ReadString(0x82C5E2F6, 0xFFFF).ToLower();
            string MenuDetected5 = console.ReadString(0x82C5E27E, 0xFFFF).ToLower();
            string MenuDetectedMatrix = console.ReadString(0x82C5F776, 0xFFFF).ToLower();
            if (MenuDetected.Equals(""))
            {
                detectedMenu = "No Menu";
            }
            else if (MenuDetected.Contains("^1b^5at^1m^5an") || MenuDetected2.Contains("^1b^5at^1m^5an") || MenuDetected3.Contains("^1b^5at^1m^5an"))
            {
                if (MenuDetected.Contains("^1v^517"))
                {
                    detectedMenu = "Batman V17";
                }
                else if (MenuDetected.Contains("^1v^513"))
                {
                    detectedMenu = "Batman V13";
                }
                else if (MenuDetected.Contains("^1v^515"))
                {
                    detectedMenu = "Batman V15";
                }
            }
            else if (MenuDetected.Contains("bossam") || MenuDetected2.Contains("bossam") || MenuDetected3.Contains("bossam"))
            {
                detectedMenu = "Bossam V6, V5, V4";
            }
            else if (MenuDetected2.Contains("elegance") || MenuDetected3.Contains("elegance") || MenuDetected.Contains("elegance"))
            {
                detectedMenu = "ELeGanCe";
            }
            else if (MenuDetected.Contains("Matrix") || MenuDetected2.Contains("Matrix") || MenuDetected3.Contains("Matrix") || MenuDetected5.Contains("Matrix") || MenuDetectedMatrix.Contains("Matrix"))
            {
                detectedMenu = "Black Ops 2 by: Matrix";
            }
            else if (MenuDetectedMatrix.Contains("black ops 2 by: matrix main menu main mods"))
            {
                detectedMenu = "Matrix";
            }
            else if (MenuDetected.Contains("Black Ops 2 by:") || MenuDetected2.Contains("Black Ops 2 by:") || MenuDetected3.Contains("Black Ops 2 by:") || MenuDetected5.Contains("Black Ops 2 by:") || MenuDetectedMatrix.Contains("Black Ops 2 by:"))
            {
                detectedMenu = "Matrix";
            }
            else if (MenuDetected2.Contains("^6j^5i^6g^5g^6y") || MenuDetected3.Contains("^6j^5i^6g^5g^6y") || MenuDetected2.Contains("xbox360lsbest") || MenuDetected.Contains("xbox360lsbest") || MenuDetected.Contains("^5j^1i^5g^1g^5y") || MenuDetected2.Contains("^5j^1i^5g^1g^5y"))
            {
                if (MenuDetected2.Contains("^64^5.^62") || MenuDetected.Contains("^64^5.^62") || MenuDetected3.Contains("^64^5.^62"))
                {
                    if (MenuDetected2.Contains("xbox360lsbest") || MenuDetected.Contains("xbox360lsbest"))
                    {
                        detectedMenu = "Jiggy Menu V4.2 (INFECTION)";
                    }
                    else
                    {
                        detectedMenu = "Jiggy Menu V4.2";
                    }
                }
                else
                {
                    detectedMenu = "Jiggy Menu";
                }
            }
            else if (MenuDetected.Contains("velocity") || MenuDetected2.Contains("velocity") || MenuDetected3.Contains("velocity"))
            {
                detectedMenu = "Velocity V2";
            }
            else if (MenuDetected2.Contains("tcm") || MenuDetected3.Contains("tcm") || MenuDetected.Contains("tcm"))
            {
                if (MenuDetected2.Contains("v15") || MenuDetected3.Contains("v15") || MenuDetected.Contains("v15"))
                {
                    detectedMenu = "Project TCM V15";
                }
                else
                {
                    detectedMenu = "Project TCM";
                }
            }
            else if (MenuDetected2.Contains("ttm") || MenuDetected3.Contains("ttm") || MenuDetected.Contains("ttm"))
            {
                detectedMenu = "TTM Trickshot Menu";
            }
            else if (MenuDetected2.Contains("xbl^5pony") || MenuDetected3.Contains("xbl^5pony") || MenuDetected.Contains("xbl^5pony"))
            {
                detectedMenu = "XBL Pony";
            }
            else if (MenuDetected2.Contains("kamil_modz") || MenuDetected3.Contains("kamil_modz") || MenuDetected.Contains("kamil_modz"))
            {
                detectedMenu = "Kamil Modz V10.2";
            }
            else if (MenuDetected2.Contains("o^6c^5m^6k^5s^6_^54^6_^5l^6i^5f^6e'^5s ^2p^1r^2i^1v^2a^1t^2e ^6p^5a^6t^5c^6") || MenuDetected3.Contains("o^6c^5m^6k^5s^6_^54^6_^5l^6i^5f^6e'^5s ^2p^1r^2i^1v^2a^1t^2e ^6p^5a^6t^5c^6") || MenuDetected.Contains("o^6c^5m^6k^5s^6_^54^6_^5l^6i^5f^6e'^5s ^2p^1r^2i^1v^2a^1t^2e ^6p^5a^6t^5c^6"))
            {
                detectedMenu = "eCmKs_4_Life's Private Patch";
            }
            else if (MenuDetected2.Contains("^5m^3ain ^5m^3enu") && MenuDetected2.Contains("^5c^3h^5e^3a^5t^3s") || MenuDetected3.Contains("^5m^3ain ^5m^3enu") && MenuDetected3.Contains("^5c^3h^5e^3a^5t^3s") || MenuDetected.Contains("^5m^3ain ^5m^3enu") && MenuDetected.Contains("^5c^3h^5e^3a^5t^3s"))
            {
                detectedMenu = "Simple Aimbot Menu (GSC Infection V1)";
            }
            else if (MenuDetected2.Contains("^5sharks ^1zombieland") || MenuDetected3.Contains("^5sharks ^1zombieland") || MenuDetected.Contains("^5sharks ^1zombieland"))
            {
                detectedMenu = "Zombieland";
            }
            else if (MenuDetected2.Contains("conversion") || MenuDetected3.Contains("conversion") || MenuDetected.Contains("conversion") || MenuDetected4.Contains("conversion") || MenuDetected5.Contains("conversion"))
            {
                detectedMenu = "Conversion V1";
            }
            else if (MenuDetected2.Contains("project iconic") || MenuDetected3.Contains("project iconic") || MenuDetected.Contains("project iconic") || MenuDetected4.Contains("project iconic") || MenuDetected5.Contains("project iconic"))
            {
                detectedMenu = "Project Iconic [SENTINEL]";
            }
            else if (MenuDetected.Contains("^6j^5i^6g^5g^6y ^5m^6e^5n^6u ^5v^65") || MenuDetected2.Contains("^6j^5i^6g^5g^6y ^5m^6e^5n^6u ^5v^65") || MenuDetected3.Contains("^6j^5i^6g^5g^6y ^5m^6e^5n^6u ^5v^65") || MenuDetected4.Contains("^6j^5i^6g^5g^6y ^5m^6e^5n^6u ^5v^65") || MenuDetected5.Contains("^6j^5i^6g^5g^6y ^5m^6e^5n^6u ^5v^65"))
            {
                detectedMenu = "Jiggy Menu V5";
            }
            else if (MenuDetected.Contains("^5s^1h^5i^1tt^5y ^1m^5e^1n^5u") || MenuDetected2.Contains("^5s^1h^5i^1tt^5y ^1m^5e^1n^5u") || MenuDetected3.Contains("^5s^1h^5i^1tt^5y ^1m^5e^1n^5u") || MenuDetected4.Contains("^5s^1h^5i^1tt^5y ^1m^5e^1n^5u") || MenuDetected5.Contains("^5s^1h^5i^1tt^5y ^1m^5e^1n^5u"))
            {
                detectedMenu = "SHITTY Menu V666";
            }
            else if (MenuDetected.Contains("^8xepixtvx's 'unreleased menu'^7") || MenuDetected2.Contains("^8xepixtvx's 'unreleased menu'^7") || MenuDetected3.Contains("^8xepixtvx's 'unreleased menu'^7") || MenuDetected4.Contains("^8xepixtvx's 'unreleased menu'^7") || MenuDetected5.Contains("^8xepixtvx's 'unreleased menu'^7"))
            {
                detectedMenu = "8xePixTvx's 'Unreleased Menu (Unnamed Menu)";
            }
            else if (MenuDetected.Contains("serenity") || MenuDetected2.Contains("serenity") || MenuDetected3.Contains("serenity") || MenuDetected4.Contains("serenity") || MenuDetected5.Contains("serenity") || MenuDetected6.Contains("serenity"))
            {
                detectedMenu = "Project Serenity";
            }
            else if (MenuDetected.Contains("pahbu and dtmf") || MenuDetected2.Contains("pahbu and dtmf") || MenuDetected3.Contains("pahbu and dtmf") || MenuDetected4.Contains("pahbu and dtmf") || MenuDetected5.Contains("pahbu and dtmf") || MenuDetected6.Contains("pahbu and dtmf"))
            {
                detectedMenu = "Project Serenity";
            }
            else
            {
                detectedMenu = "Undetected";
            }
            return detectedMenu;
        }


        private void simpleButton570_Click(object sender, EventArgs e)
        {
            offhostkicknop();
        }

        private void simpleButton30_Click(object sender, EventArgs e)
        {
            console.WriteUInt32(0x82273C88, 0x60000000);
        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            console.WriteUInt16(0x8227BBDC, 0x4800);
        }

        private void simpleButton574_Click(object sender, EventArgs e)
        {
            textEdit43.Text = GetGSCMenu();
        }

        private void simpleButton32_Click(object sender, EventArgs e)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            for (int i = 0; i < 18; i++)
            {

                uint address = console.ReadUInt32(2210837880U);
                uint address3 = console.ReadUInt32(2210837592U);
                uint address4 = console.ReadUInt32(2210837400U);
                textEdit3.Text = "Host: " + console.ReadString(address, 15U);
                textEdit4.Text = "Mapname: " + console.ReadString(address3, 15U).Split(new char[] { "\0".First() }).First<string>();
                textEdit5.Text = "Gametype: " + console.ReadString(address4, 15U).Split(new char[] { "\0".First() }).First<string>();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            for (uint num = 0U; num < 50U; num += 1U)
            {
                listView1.Items.Add(new ListViewItem(new string[]
                {
                    console.ReadUInt32(num * 160U - 2089822420U).ToString(),
                    Encoding.ASCII.GetString(console.GetMemory(num * 160U - 2089822420U + 4U, 32U)),
                    console.ReadInt64(num * 160U - 2089822432U).ToString(),
                    console.ReadInt64((num - 1U) * 160U - 2089822420U + 132U).ToString("X")
                }));
            }
        }

        public static byte[] hexString(string hex)
        {
            return (from x in Enumerable.Range(0, hex.Length)
                    where x % 2 == 0
                    select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray<byte>();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            console.WriteByte(2175795328U, 1);
            console.WriteString(2175795329U, spoofgttextbox.Text);
            console.SetMemory(2175795345U, Form1.hexString(spoofxuidtextbox.Text));
            if (XtraMessageBox.Show(styleController1.LookAndFeel, "XUID/GT Spoofed", "Spoofed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) == DialogResult.OK) ;
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            StreamReader streamReader = File.OpenText("xuids.txt");
            string text;
            while ((text = streamReader.ReadLine()) != null)
            {
                console.WriteByte(2175795328U, 1);
                console.WriteString(2175795329U, multispoofgtbox.Text);
                console.SetMemory(2175795345U, Form1.hexString(text));
                console.CallVoid(2185237984U, new object[] { 0, "startmultiplayer" });
                XtraMessageBox.Show(styleController1.LookAndFeel, string.Concat(new string[] { "XUID [", text, "] GT [", multispoofgtbox.Text, "] Spoofed :)\nOnce You have played a game,\nyou may click OK to move onto the next XUID!" }), "Wait!");
            }
        }

        private void setgamertag(string gt)
        {
            console.SetMemory(0x82C55D00, new byte[] { 0x7C, 0x83, 0x23, 0x78, 0x3D, 0x60, 0x82, 0xC5, 0x38, 0x8B, 0x5D, 0x60, 0x3D, 0x60, 0x82, 0x4A, 0x39, 0x6B, 0xDC, 0xA0, 0x38, 0xA0, 0x00, 0x20, 0x7D, 0x69, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x20 });//Injects hook into empty memory
            console.SetMemory(0x8293D724, new byte[] { 0x3D, 0x60, 0x82, 0xC5, 0x39, 0x6B, 0x5D, 0x00, 0x7D, 0x69, 0x03, 0xA6, 0x4E, 0x80, 0x04, 0x20 });//PatchInJumps XamGetUserName to the hook previously written
            console.SetMemory(0x8259B6A7, new byte[] { 0x00 });//Patch 1
            console.SetMemory(0x822D1110, new byte[] { 0x40 });//Patch 2
            console.SetMemory(0x82C55D60, Encoding.UTF8.GetBytes($"{gt}\0"));
        }


        internal class styleController1
        {
            internal static IWin32Window LookAndFeel;
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {

        }

        public static int strlen(string s)
        {
            int num = 0;
            foreach (char ch in s)
                ++num;
            return num;
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            setgamertag(textEdit29.Text);
            string str = console.ReadString(0x81AA285C, 8);
            console.CallVoid(0x822786E0, 0, "userinfo \"\\name\\" + textEdit29.Text + "\\xuid\\");
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            int num = Form1.strlen(textEdit30.Text);
            if (num == 1)
                Length = "\u0001";
            if (num == 2)
                Length = "\u0002";
            if (num == 3)
                Length = "\u0003";
            if (num == 4)
                Length = "\u0004";
            if (num == 5)
                Length = "\u0005";
            if (num == 6)
                Length = "\u0006";
            if (num == 7)
                Length = "\a";
            if (num == 8)
                Length = "\b";
            if (num == 9)
                Length = "\t";
            if (num == 10)
                Length = "\n";
            if (num == 11)
                Length = "\v";
            if (num == 12)
                Length = "\f";
            if (num == 13)
                Length = "\r";
            if (num == 14)
                Length = "\u000E";
            if (num == 15)
                Length = "\u000F";
            if (num == 16)
                Length = "\u0010";
            if (num == 17)
                Length = "\u0011";
            if (num == 18)
                Length = "\u0012";
            if (num == 19)
                Length = "\u0013";
            if (num == 20)
                Length = "\u0014";
            if (num == 21)
                Length = "\u0015";
            if (num == 22)
                Length = "\u0016";
            if (num == 23)
                Length = "\u0017";
            if (num == 24)
                Length = "\u0018";
            if (num == 25)
                Length = "\u0019";
            if (num == 26)
                Length = "\u001A";
            if (num == 27)
                Length = "\u001B";
            if (checkEdit2.Checked)
            {
                if (num > 27)
                    return;
                setgamertag("^H\u007F?" + Length + textEdit30.Text);
            }
            else if (checkEdit5.Checked)
            {
                if (num > 27)
                    return;
                setgamertag("^H//" + Length + textEdit30.Text);
            }
            else if (num <= 27)
                setgamertag("^H\u007F\u007F" + Length + textEdit30.Text);
        }


        private void Angleup()
        {
            console.WriteFloat(0x83C3FE98, 360);
        }

        private void Angledown()
        {
            console.WriteFloat(0x83C3FE38, 360);
        }

        private void Ladder360()
        {
            console.WriteFloat(0x83C40078, 360);
        }


        private void simpleButton19_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x824015E0, new object[] { 0, ";statwriteddl clantagstats clanname " + textEdit31.Text + ";" });
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            setgamertag("" + Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
            string str = console.ReadString(0x81AA285C, 8);
            console.CallVoid(0x822786E0, 0, "userinfo \"\\name\\" + Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]) + "\\xuid\\");
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            string str = console.ReadString(0x81AA285C, 8);
            console.CallVoid(0x822786E0, 0, "userinfo \"\\name\\" + "[{+}][{+}][{+}][{+}][{+}][{+}][[S]" + "\\xuid\\");
            setgamertag("[{+}][{+}][{+}][{+}][{+}][{+}][[S]");
        }

        private void simpleButton61_Click(object sender, EventArgs e)
        {
            Angleup();
            Angledown();
            Ladder360();
        }


        private string method_0(string string_1)
        {
            byte[] data = new byte[8];
            console.SetMemory(2175409456U, data);
            console.CallVoid(2172820832U, new object[]
            {
                2533274907391075L,
                0,
                string_1,
                24,
                2175409456U,
                0
            });
            Thread.Sleep(1000);
            string result = BitConverter.ToString(console.GetMemory(2175409456U, 8U)).Replace("-", "");
            console.SetMemory(2175409456U, data);
            return result;
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            console.CallVoid(2185237984U, new object[] { 0, "set party_maxplayers 18;set party_minplayers 12" });
        }

        private void simpleButton58_Click(object sender, EventArgs e)
        {
            console.CallVoid(console.ResolveFunction("xam.xex", 2812U), (object)0);
        }

        private void simpleButton43_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton307_Click(object sender, EventArgs e)
        {
            byte[] Blue = { 0x1F, 0xFF, 0x00, 0x00, 0x3F, 0xFF, 0x00 };
            console.SetMemory(0x83C56038, Blue);
        }

        private void simpleButton66_Click(object sender, EventArgs e)
        {
            if (simpleButton66.Text == "Set HostName/FPS [OFF]")
            {
                simpleButton66.Text = "Set HostName/FPS [ON]";
                JRPC.WriteString(console, 2181054320U, textEdit37.Text + "\0");
                JRPC.WriteBool(console, 2210767803U, true);
            }
            else
            {
                simpleButton66.Text = "Set HostName/FPS [OFF]";
                JRPC.WriteString(console, 2181054320U, textEdit37.Text + "\0");
                JRPC.WriteBool(console, 2210767803U, false);
            }
        }

        private void simpleButton64_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x824015e0, 0, $"cg_fov {textEdit35.Text}");
        }

        private void simpleButton47_Click(object sender, EventArgs e)
        {
            JRPC.CallVoid(console, 2185237984U, new object[2]
       {
          (object) 0,
          (object) ("cmd mr " + (object) JRPC.ReadUInt32(console, 2193708888U) + " -1 changeteam;")
       });
        }

        private void simpleButton56_Click(object sender, EventArgs e)
        {
            JRPC.CallVoid(console, 2185427824U, new object[3]
    {
      (object) -1,
      (object) 0,
      (object) ("5 \"" + textEdit34.Text + "\"")
 });
        }

        private void simpleButton65_Click(object sender, EventArgs e)
        {
            JRPC.SetMemory(console, 3279259388U, new byte[66]);
            JRPC.SetMemory(console, 3279363892U, new byte[66]);
            JRPC.SetMemory(console, 3279300380U, new byte[66]);
            JRPC.SetMemory(console, 3279472444U, new byte[66]);
            JRPC.SetMemory(console, 3279408932U, new byte[66]);
            JRPC.SetMemory(console, 3279386412U, new byte[66]);
            JRPC.SetMemory(console, 3279322900U, new byte[66]);
            JRPC.SetMemory(console, 3279173356U, new byte[66]);
            JRPC.SetMemory(console, 3279236868U, new byte[66]);
            JRPC.SetMemory(console, 3279281908U, new byte[66]);
            JRPC.SetMemory(console, 3279345420U, new byte[66]);
            JRPC.SetMemory(console, 3279449924U, new byte[66]);
            byte[] bytes = Encoding.ASCII.GetBytes(textEdit36.Text);
            JRPC.SetMemory(console, 3279259388U, bytes);
            JRPC.SetMemory(console, 3279363892U, bytes);
            JRPC.SetMemory(console, 3279300380U, bytes);
            JRPC.SetMemory(console, 3279472444U, bytes);
            JRPC.SetMemory(console, 3279408932U, bytes);
            JRPC.SetMemory(console, 3279386412U, bytes);
            JRPC.SetMemory(console, 3279322900U, bytes);
            JRPC.SetMemory(console, 3279173356U, bytes);
            JRPC.SetMemory(console, 3279236868U, bytes);
            JRPC.SetMemory(console, 3279281908U, bytes);
            JRPC.SetMemory(console, 3279345420U, bytes);
            JRPC.SetMemory(console, 3279449924U, bytes);
        }

        private void simpleButton199_Click(object sender, EventArgs e)
        {
            if (simpleButton199.Text == "Long Knife [OFF]")
            {
                simpleButton199.Text = "Long Knife [ON]";
                console.CallVoid(BO2CMD, new object[]
               {
                0,
                "player_meleeRange 999; cg_meleerange 999"
               });
                console.CallVoid(BO2SV, new object[]
                {
                -1,
                0,
                "< \"LongKnife [^2ON^7]\""
                });
            }
            else
            {
                simpleButton199.Text = "Long Knife [OFF]";
                console.CallVoid(BO2CMD, new object[]
            {
                0,
                "player_meleeRange 1; cg_meleerange 1"
            });
                console.CallVoid(BO2SV, new object[]
                {
                -1,
                0,
                "< \"^1LongKnife OFF :(\""
                });
            }
        }

        private void simpleButton227_Click(object sender, EventArgs e)
        {
            console.SetMemory(2203393598U, new byte[]
           {
                byte.MaxValue,
                byte.MaxValue
           });
            console.SetMemory(2203393602U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            });
            console.SetMemory(2203393606U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            });
        }

        private void simpleButton6_Click_1(object sender, EventArgs e)
        {

        }

        private void simpleButton523_Click(object sender, EventArgs e)
        {
            if (!(this.simpleButton523.Text == "No Recoil [OFF]"))
            {
                this.simpleButton523.Text = "No Recoil [OFF]";
                this.console.SetMemory(2183502792U, new byte[]
                {
                    72,
                    70,
                    19,
                    65
                });
            }
            else
            {
                this.simpleButton523.Text = "No Recoil [ON]";
                IXboxConsole console = this.console;
                byte[] array = new byte[4];
                array[0] = 96;
                console.SetMemory(2183502792U, array);
            }
        }

        private void simpleButton529_Click(object sender, EventArgs e)
        {
            this.console.XNotify("No Spread enabled, to disable relaunch Black Ops II!");
            this.console.WriteUInt32(2188097220U, 1610612736U);
            this.console.WriteUInt32(2181041612U, 962592768U);
            this.console.WriteUInt32(2188097220U, 962592768U);
            this.console.WriteUInt32(2188096780U, 962592770U);
        }

        private void simpleButton525_Click(object sender, EventArgs e)
        {
            if (this.simpleButton525.Text == "ESP [OFF]")
            {
                this.simpleButton525.Text = "ESP [ON]";
                this.console.WriteByte(2183093119U, 1);
            }
            else
            {
                this.simpleButton525.Text = "ESP [OFF]";
                this.console.WriteByte(2183093119U, 0);
            }
        }

        private void simpleButton530_Click(object sender, EventArgs e)
        {
            this.console.XNotify("Unlimited class items is enabled!");
            byte[] data = new byte[]
            {
                59,
                85,
                85,
                85
            };
            this.console.SetMemory(2188009404U, data);
        }

        private void simpleButton532_Click(object sender, EventArgs e)
        {
            if (this.simpleButton532.Text == "Wallhack [OFF]")
            {
                this.simpleButton532.Text = "Wallhack [ON]";
                this.console.SetMemory(2183118924U, new byte[]
                {
                    56,
                    192,
                    byte.MaxValue,
                    byte.MaxValue
                });
            }
            else
            {
                this.simpleButton532.Text = "Wallhack [OFF]";
                this.console.SetMemory(2183118924U, new byte[]
                {
                    127,
                    166,
                    235,
                    120
                });
            }
        }

        private void simpleButton531_Click(object sender, EventArgs e)
        {
            if (!(this.simpleButton531.Text == "Spoof Mic [OFF]"))
            {
                this.simpleButton531.Text = "Spoof Mic [OFF]";
                this.console.WriteUInt32(2183713032U, 945815552U);
            }
            else
            {
                this.simpleButton531.Text = "Spoof Mic [ON]";
                this.console.WriteUInt32(2183713032U, 945815553U);
            }
        }

        private void simpleButton527_Click(object sender, EventArgs e)
        {
            if (!(this.simpleButton527.Text == "Laser [OFF]"))
            {
                this.simpleButton527.Text = "Laser [OFF]";
                IXboxConsole console = this.console;
                byte[] array = new byte[4];
                array[0] = 43;
                array[1] = 11;
                console.SetMemory(2183487004U, array);
            }
            else
            {
                this.simpleButton527.Text = "Laser [ON]";
                this.console.SetMemory(2183487004U, new byte[]
                {
                    43,
                    11,
                    0,
                    1
                });
            }
        }

        private void simpleButton524_Click(object sender, EventArgs e)
        {
            if (!(this.simpleButton524.Text == "UAV [OFF]"))
            {
                this.simpleButton524.Text = "UAV [OFF]";
                this.console.SetMemory(2182844371U, new byte[1]);
            }
            else
            {
                this.simpleButton524.Text = "UAV [ON]";
                this.console.SetMemory(2182844371U, new byte[]
                {
                    1
                });
            }
        }

        private void simpleButton528_Click(object sender, EventArgs e)
        {
            if (this.simpleButton528.Text == "Show Host [OFF]")
            {
                this.simpleButton528.Text = "Show Host [ON]";
                byte[] array = new byte[4];
                array[0] = 18;
                this.console.SetMemory(2181054320U, array);
                this.console.WriteBool(2210767803U, true);
            }
            else
            {
                this.simpleButton528.Text = "Show Host [OFF]";
                byte[] array2 = new byte[4];
                array2[0] = 18;
                this.console.SetMemory(2181054320U, array2);
                this.console.WriteBool(2210767803U, false);
            }
        }

        private void simpleButton536_Click(object sender, EventArgs e)
        {
            this.console.CallVoid(2183628512U, new object[]
            {
                0,
                "userinfo \"\\name\\^6GCT\\clanabbrev\\^1\\xuid\\1E0200F003F252F7\\invited\\1\""
            });
        }

        private void simpleButton185_Click(object sender, EventArgs e)
        {
            this.console.CallVoid(2183628512U, new object[]
            {
                0,
                "userinfo \"\\clanabbrev\\{+}\\name\\[{+}][{+}][{+}][{+}][{+}][{+}][[S]\\xuid\\"
            });
        }

        private void simpleButton684_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton685_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton606_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2792842450U, Encoding.ASCII.GetBytes(this.textEdit_116.Text + "\0"));
        }

        private void simpleButton607_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2793000498U, Encoding.ASCII.GetBytes(this.textEdit_112.Text + "\0"));
        }

        private void simpleButton610_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2792817654U, Encoding.ASCII.GetBytes(this.textEdit_108.Text + "\0"));
        }

        private void simpleButton604_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2793175320U, Encoding.ASCII.GetBytes(this.textEdit_113.Text + "\0"));
        }

        private void simpleButton608_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2792955692U, Encoding.ASCII.GetBytes(this.textEdit_106.Text + "\0"));
        }

        private void simpleButton609_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2792956305U, Encoding.ASCII.GetBytes(this.textEdit_107.Text + "\0"));
        }

        private void simpleButton603_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2793024692U, Encoding.ASCII.GetBytes(this.textEdit_114.Text + "\0"));
        }

        private void simpleButton614_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2793181806U, Encoding.ASCII.GetBytes(this.textEdit_104.Text + "\0"));
        }

        private void simpleButton601_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2793014864U, Encoding.ASCII.GetBytes(this.textEdit_110.Text + "\0"));
        }

        private void simpleButton615_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2792853398U, Encoding.ASCII.GetBytes(this.textEdit_101.Text + "\0"));
        }

        private void simpleButton602_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2793140019U, Encoding.ASCII.GetBytes(this.textEdit_111.Text + "\0"));
        }

        private void simpleButton612_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2793002652U, Encoding.ASCII.GetBytes(this.textEdit_102.Text + "\0"));
        }

        private void simpleButton600_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2793202160U, Encoding.ASCII.GetBytes(this.textEdit_109.Text + "\0"));

        }

        private void simpleButton613_Click(object sender, EventArgs e)
        {
            this.console.SetMemory(2792995775U, Encoding.ASCII.GetBytes(this.textEdit_103.Text + "\0"));
        }

        private void simpleButton678_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton679_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton510_Click(object sender, EventArgs e)
        {
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 0 " + this.textEdit_88.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 1 " + this.textEdit_84.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 2 " + this.textEdit_85.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 3 " + this.textEdit_87.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 4 " + this.textEdit_86.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 5 " + this.textEdit_83.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 6 " + this.textEdit_79.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 7 " + this.textEdit_80.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 8 " + this.textEdit_82.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString cacloadouts customclassname 9 " + this.textEdit_81.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString custommatchcacloadouts customclassname 0 " + this.textEdit_88.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString custommatchcacloadouts customclassname 1 " + this.textEdit_84.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString custommatchcacloadouts customclassname 2 " + this.textEdit_85.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString custommatchcacloadouts customclassname 3 " + this.textEdit_87.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "setStatFromLocString custommatchcacloadouts customclassname 4 " + this.textEdit_86.Text.Replace(" ", "_") + "\0"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "updatestats"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "updategamerprofile"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "updatestats"
            });
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                "updategamerprofile"
            });
            this.console.XNotify("Custom Class Names Set!");
        }

        private void method_76(string string_3)
        {
            this.console.WriteInt32(2185854736U, 962592768);
            this.NopIt(2185854640U);
            this.NopIt(2185854664U);
            this.NopIt(2185854752U);
            this.console.CallVoid(2185237984U, new object[]
            {
                0,
                string_3
            });
        }

        public void NopIt(uint address)
        {
            byte[] array = new byte[4];
            array[0] = 96;
            this.console.SetMemory(address, array);
        }

        private void simpleButton511_Click(object sender, EventArgs e)
        {
            if (!this.bool_0)
            {
                string str = Encoding.BigEndianUnicode.GetString(this.console.GetMemory(2175412476U, 30U)).Trim().Trim(new char[1]) ?? "";
                this.textEdit_88.Text = "^1" + str;
                this.textEdit_84.Text = "^2" + str;
                this.textEdit_85.Text = "^3" + str;
                this.textEdit_87.Text = "^4" + str;
                this.textEdit_86.Text = "^5" + str;
                this.textEdit_83.Text = "^1" + str;
                this.textEdit_79.Text = "^2" + str;
                this.textEdit_80.Text = "^3" + str;
                this.textEdit_82.Text = "^4" + str;
                this.textEdit_81.Text = "^5" + str;
                this.simpleButton510_Click(sender, null);
            }
            else
            {
                string text = this.textEdit_108.Text;
                this.textEdit_88.Text = "^1" + text;
                this.textEdit_84.Text = "^2" + text;
                this.textEdit_85.Text = "^3" + text;
                this.textEdit_87.Text = "^4" + text;
                this.textEdit_86.Text = "^5" + text;
                this.textEdit_83.Text = "^1" + text;
                this.textEdit_79.Text = "^2" + text;
                this.textEdit_80.Text = "^3" + text;
                this.textEdit_82.Text = "^4" + text;
                this.textEdit_81.Text = "^5" + text;
                this.simpleButton510_Click(sender, null);
            }
        }




        private void simpleButton260_Click(object sender, EventArgs e)
        {
            this.method_76("setStatFromLocString cacloadouts customclassname 0 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 1 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 2 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 3 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 4 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 5 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 6 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 7 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 8 \"^H^H\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 9 \"^H^H\"");
            this.method_76("updategamerprofile;uploadstats");
            this.console.XNotify("Froze Classes!");
        }

        private void simpleButton261_Click(object sender, EventArgs e)
        {
            this.method_76("setStatFromLocString cacloadouts customclassname 0 \"Custom 1\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 1 \"Custom 2\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 2 \"Custom 3\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 3 \"Custom 4\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 4 \"Custom 5\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 5 \"Custom 6\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 6 \"Custom 7\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 7 \"Custom 8\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 8 \"Custom 9\"");
            this.method_76("setStatFromLocString cacloadouts customclassname 9 \"Custom 10\"");
            this.method_76("updategamerprofile;uploadstats");
            this.console.XNotify("Unfroze Classes!");
        }

        public static string ReadNat(byte input)
        {
            switch (input)
            {
                case 0x0: return "N/A";
                case 0x1: return "Open";
                case 0x2: return "Moderate";
                case 0x3: return "Strict";
                default: return "N/A";
            }
        }

        private void simpleButton6_Click_2(object sender, EventArgs e)
        {
            setgamertag("^Hÿÿÿ\u0001\u0002\n\u0015SHÿÿÿ\0");
            setgamertag("^Hÿÿÿ\u0001\u0002\n\u0015SIÿÿÿ\0");
            Thread.Sleep(250);
            console.XNotify("Pre-Game Host Frozen!");
            setgamertag(Encoding.BigEndianUnicode.GetString(console.GetMemory(0x81AA28FC, 0x1E)).Trim().Trim(new char[1]));
        }

        private static string ReadPrestige(byte[] input)
        {
            string Prestige = BitConverter.IsLittleEndian ? BitConverter.ToInt32(new byte[] { input[3], input[2], input[1], input[0] }, 0).ToString() : BitConverter.ToInt32(input, 0).ToString();
            return Prestige.Contains("-") ? "N/A" : Prestige;
        }


        private void simpleButton10_Click(object sender, EventArgs e)
        {
            console.WriteUInt32(0x82273C88, 0x60000000);
            console.WriteUInt32(0x822781D8, 0x60000000);
            console.WriteUInt32(0x822A06B0, 0x60000000);
            console.WriteUInt32(0x8222a894, 0x60000000);
            console.WriteUInt32(0x82717d48, 0x60000000);
            console.WriteUInt32(0x825bab6c, 0x60000000);
            console.WriteUInt32(0x8232349c, 0x60000000);
            console.WriteUInt32(0x82412950, 0x3860FFFF);
            console.WriteUInt32(0x821DDE88, 0x60000000);
            console.WriteUInt16(0x82334BF4, 0x4800);
            console.WriteUInt16(0x8227BBDC, 0x4800);
        }

        public static string convertIp(byte[] ip)
        {
            return string.Format("{0}.{1}.{2}.{3}", ip[0], ip[1], ip[2], ip[3]);
        }

        private static string ReadMic(byte input)
        {
            switch (input)
            {
                case 0x0: return "Off";
                case 0x1: return "On";
                default: return "N/A";
            }
        }

        public class Client
        {
            public int Index { get; set; }
            public string Gamertag { get; set; }
            public string XUID { get; set; }
            public string InternalIP { get; set; }
            public string ExternalIP { get; set; }
            public string Port { get; set; }
            public string Mac { get; set; }
        }


        public static byte[] Reverse(byte[] sArray)
        {
            for (int i = 0; i < sArray.Length / 2; i++)
            {
                byte b = sArray[i];
                sArray[i] = sArray[sArray.Length - 1 - i];
                sArray[sArray.Length - 1 - i] = b;
            }
            return sArray;
        }

        private static string ReadLevel(byte input)
        {
            return Convert.ToByte(input + 0x1).ToString();
        }

        public static List<Client> GrabbedClients = new List<Client>();
        private byte[] byte_0;
        private object selectedIndex;
        public static uint addr = 0xc24313e0;
        private Array SMCMessage;

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            gridControl.DataSource = null;

            GrabbedClients.Clear();

            for (int i = 0; i < 18; i++)
            {
                string Gamertag = Encoding.ASCII.GetString(console.GetMemory((0xc403c368 + 0x40) + ((uint)(0x148 * i)), 15)).TrimEnd(new char[1]);
                string ExternalIP = new IPAddress(console.GetMemory((0xc403c368 + 180) + ((uint)(0x148 * i)), 4)).ToString();
                string InternalIP = new IPAddress(console.GetMemory((0xc403c418) + ((uint)(0x148 * i)), 4)).ToString();
                string XUID = BitConverter.ToString(console.GetMemory((0xc403c368 + 0x38) + ((uint)(0x148 * i)), 8)).Replace("-", "");
                string Port = int.Parse(BitConverter.ToString(console.GetMemory((0xc403c368 + 0xb7) + ((uint)(0x148 * i)), 2)).Replace("-", ""), System.Globalization.NumberStyles.HexNumber).ToString();
                string Mac = BitConverter.ToString(console.GetMemory((0xc403c422) + ((uint)(0x148 * i)), 6)).Replace("-", "");
                WebClient webClient = new WebClient();

                if (XUID != "0000000000000000")
                    GrabbedClients.Add(new Client
                    {
                        Index = i,
                        Gamertag = Gamertag,
                        XUID = XUID,
                        ExternalIP = ExternalIP,
                        InternalIP = InternalIP,
                        Port = Port,
                        Mac = Mac,
                    });
            }

            gridControl1.DataSource = GrabbedClients;

        }

        public void SetPlayerGamertag(int SelectedIndex, string Gamertag)
        {
            {
                try
                {
                    console.SetMemory((uint)(-0x7CAAE5F0 + SelectedIndex * 0x57F8 + 0x5534), new byte[32]);
                    console.SetMemory((uint)(-0x7CAAE5F0 + SelectedIndex * 0x57F8 + 0x5534), Encoding.ASCII.GetBytes(Gamertag));
                }
                catch
                {

                }
            }
        }

        private void simpleButton26_Click(object sender, EventArgs e)
        {
            for (int num9 = 0; num9 < 18; num9++)
            {
                string gamerTag = Encoding.ASCII.GetString(console.GetMemory((0xc403c368 + 0x40) + ((uint)(0x148 * num9)), 15)).TrimEnd(new char[1]);
                string ipAddress = new IPAddress(console.GetMemory((0xc403c368 + 180) + ((uint)(0x148 * num9)), 4)).ToString();
                SetPlayerGamertag(num9, gamerTag + " =^5 " + ipAddress);
            }
        }

        private void method_59(string string_3)
        {
            this.console.SetMemory(2193972480U, new byte[]
            {
                124,
                131,
                35,
                120,
                61,
                96,
                130,
                197,
                56,
                139,
                93,
                96,
                61,
                96,
                130,
                74,
                57,
                107,
                220,
                160,
                56,
                160,
                0,
                32,
                125,
                105,
                3,
                166,
                78,
                128,
                4,
                32
            });
            this.console.SetMemory(2190726948U, new byte[]
            {
                61,
                96,
                130,
                197,
                57,
                107,
                93,
                0,
                125,
                105,
                3,
                166,
                78,
                128,
                4,
                32
            });
            this.console.SetMemory(2186917543U, new byte[1]);
            this.console.SetMemory(2183991568U, new byte[]
            {
                64
            });
            console.SetMemory(2193972576U, Encoding.UTF8.GetBytes(string_3 + "\0"));
        }


        private void simpleButton25_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            if (!(this.listBox6.SelectedItem.ToString() == ""))
            {
                string text = this.listBox6.SelectedItem.ToString().Split(new char[]
                {
                    " ".Last<char>()
                }).First<string>();
                string text2 = this.listBox6.SelectedItem.ToString().Split(new char[]
                {
                    text.Last<char>()
                }).Last<string>();
                string text3 = text2.Split(new char[]
                {
                    " : ".Last<char>()
                }).Last<string>();
                console.CallVoid(2183628512U, new object[]
                {
                    0,
                    string.Concat(new string[]
                    {
                        "userinfo \"\\name\\",
                        text,
                        " = ",
                        text3,
                        "\\xuid\\"
                    })
                });
                this.method_59(text + " = " + text3);
                this.textEdit29.Text = text + " = " + text3;
            }
            else
            {
                XtraMessageBox.Show("No Client Selected", "Guido's console Tool", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void simpleButton11_Click_1(object sender, EventArgs e)
        {

        }

        private async void simpleButton168_Click(object sender, EventArgs e)
        {
            setgamertag("8m===D");
            await Task.Delay(500);
            setgamertag("8=m==D");
            await Task.Delay(500);
            setgamertag("8==m=D");
            await Task.Delay(500);
            setgamertag("8===mD");
            await Task.Delay(500);
            setgamertag("8==m=D");
            await Task.Delay(500);
            setgamertag("8=m==D");
            await Task.Delay(500);
            setgamertag("8m===D");
            await Task.Delay(500);
            setgamertag("8=m==D");
            await Task.Delay(500);
            setgamertag("8==m=D ~");
            await Task.Delay(500);
            setgamertag("8===mD ~ ~");
            await Task.Delay(500);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton178_Click(object sender, EventArgs e)
        {
            setgamertag("Hey catch! ^H\u007F\u007F\vhud_icon_c4");
        }

        private void simpleButton177_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007F?\u0014headiconyouinkillcamSUCK!");
        }

        private void simpleButton172_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007F\u007F\fmenu_mp_esrb< cheat codes");
        }

        private void simpleButton165_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton176_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007F\u007F\vhint_mantle^5is a virgin");
        }

        private void simpleButton175_Click(object sender, EventArgs e)
        {
            setgamertag("Official^H\u007F\u007F\u0004logoAdmin");
        }

        private void simpleButton174_Click(object sender, EventArgs e)
        {
            setgamertag("Marry Me^H\u007F\u007F\u0010dualoptic_down_9");
        }

        private void simpleButton166_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007f\u007f\u0013cac_mods_dual_wield");
        }

        private void simpleButton173_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007f\u007f\bui_globe");
        }

        private void simpleButton171_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007F\u007F\breflex_8Hi im gay");
        }

        private async void simpleButton169_Click(object sender, EventArgs e)
        {
            var proc = Process.GetProcessesByName("Spotify").FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.MainWindowTitle));

            if (proc == null)
            {
            }
            if (string.Equals(proc.MainWindowTitle, "Spotify", StringComparison.InvariantCultureIgnoreCase))
            {

            }
            setgamertag("^6L");
            await Task.Delay(250);
            setgamertag("^5Li");
            await Task.Delay(250);
            setgamertag("^6Lis");
            await Task.Delay(250);
            setgamertag("^5List");
            await Task.Delay(250);
            setgamertag("^6Liste");
            await Task.Delay(250);
            setgamertag("^5Listen");
            await Task.Delay(250);
            setgamertag("^6Listeni");
            await Task.Delay(250);
            setgamertag("^5Listenin");
            await Task.Delay(250);
            setgamertag("^6Listening");
            await Task.Delay(250);
            setgamertag("^5Listening T");
            await Task.Delay(250);
            setgamertag("^6Listening To");
            await Task.Delay(250);
            setgamertag("^5Listening To.");
            await Task.Delay(250);
            setgamertag("^6Listening To..");
            await Task.Delay(250);
            setgamertag("^5Listening To...");
            await Task.Delay(250);
            setgamertag("^6Listening To.");
            await Task.Delay(250);
            setgamertag("^5Listening To..");
            await Task.Delay(250);
            setgamertag("^6Listening To...");
            await Task.Delay(250);
            setgamertag("^5" + proc.MainWindowTitle);
            await Task.Delay(250);
            setgamertag("^6" + proc.MainWindowTitle);
            await Task.Delay(250);
            setgamertag("^5" + proc.MainWindowTitle);
            await Task.Delay(250);
            setgamertag("^6" + proc.MainWindowTitle);
            await Task.Delay(250);
            setgamertag("^5" + proc.MainWindowTitle);
            await Task.Delay(250);
            setgamertag("^6" + proc.MainWindowTitle);
            await Task.Delay(250);
            setgamertag("^5" + proc.MainWindowTitle);
            await Task.Delay(250);
            setgamertag("^6" + proc.MainWindowTitle);
            await Task.Delay(250);
            setgamertag("on ^2Spotify");
            await Task.Delay(2500);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton159_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007F\u007F\u0017menu_lobby_icon_twitter");
        }

        private void simpleButton170_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007f\u007f\bthumbsup");
        }

        private void simpleButton160_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007F\u007F\fyoutube_logo");
        }

        private void simpleButton158_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007f\u007f\u0017playlist_search_destroy");
        }

        private void simpleButton157_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007F\u007F\vhint_mantlethis fgt tho");
        }

        private void simpleButton156_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007F\u007F\bui_smoke");
        }

        private void simpleButton155_Click(object sender, EventArgs e)
        {
            setgamertag("^H\u007f\u007f\u0013compass_map_flicker");
        }

        private void simpleButton153_Click(object sender, EventArgs e)
        {
            setgamertag("[{+forward}] FAGS [{+back}]");
        }

        private void simpleButton186_Click(object sender, EventArgs e)
        {
            console.XNotify($"Troll Activated Against: \"{textEdit73.Text}\"");
            setgamertag($"^6Hey, ^5 \"{textEdit73.Text}\"");
            System.Threading.Thread.Sleep(2200);
            setgamertag("^1Liking that Wifi??");
            System.Threading.Thread.Sleep(2200);
            setgamertag("^5Keep being a ^6Retard...");
            System.Threading.Thread.Sleep(2200);
            setgamertag("^3And that shit's gonna go");
            System.Threading.Thread.Sleep(1750);
            setgamertag("^Hping_bar_04");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hping_bar_03");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hping_bar_02");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hping_bar_01");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hnet_new_animation");
            System.Threading.Thread.Sleep(2750); //^Hcompass_static
            setgamertag("^Hcompass_static");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hcompass_static #OFFLINE");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hcompass_static");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hcompass_static #OFFLINE");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hcompass_static");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hcompass_static #OFFLINE");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hcompass_static");
            System.Threading.Thread.Sleep(750);
            setgamertag("^Hcompass_static #OFFLINE");
            System.Threading.Thread.Sleep(750);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton148_Click(object sender, EventArgs e)
        {
            setgamertag("Listen here ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("gotta tape up my speakers cuz..");
            Thread.Sleep(2000);
            setgamertag("ur queer ass smells like cheese");
            Thread.Sleep(2000);
            setgamertag("go drown in a bowl of soup");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton147_Click(object sender, EventArgs e)
        {
            setgamertag("Aye ^1" + textEdit72.Text + "!");
            Thread.Sleep(2000);
            setgamertag("ima stick my pp in ur fat bitch");
            Thread.Sleep(2000);
            setgamertag("then gonna pee in her butthole");
            Thread.Sleep(2000);
            setgamertag("hoe got dinner plate nipples");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton145_Click(object sender, EventArgs e)
        {
            setgamertag("Dis nigga ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("is the type of dumbass to..");
            Thread.Sleep(2000);
            setgamertag("lick his fingers before");
            Thread.Sleep(2000);
            setgamertag("flippin the page on an ipad");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton142_Click(object sender, EventArgs e)
        {
            setgamertag("Hey ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("go slit ur wrists and");
            Thread.Sleep(2000);
            setgamertag("do a handstand in lemonade");
            Thread.Sleep(2000);
            setgamertag("u smelly crosseyed dickwad");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton149_Click(object sender, EventArgs e)
        {
            setgamertag("Stfu ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("go papercut ur entire body");
            Thread.Sleep(2000);
            setgamertag("and roll around in piss");
            Thread.Sleep(2000);
            setgamertag("you flappy titt'd slob");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton152_Click(object sender, EventArgs e)
        {
            setgamertag("kill urself ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("go slit your eyes");
            Thread.Sleep(2000);
            setgamertag("and stare at the sun");
            Thread.Sleep(2000);
            setgamertag("you clearly have foreskin");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton141_Click(object sender, EventArgs e)
        {
            setgamertag(" ^1" + textEdit72.Text + " is..");
            Thread.Sleep(2000);
            setgamertag("that one nigga that..");
            Thread.Sleep(2000);
            setgamertag("sat at the back of class with..");
            Thread.Sleep(2000);
            setgamertag("a pencil up his nose");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton146_Click(object sender, EventArgs e)
        {
            setgamertag("Listen here ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("ur big ass nose looks like..");
            Thread.Sleep(2000);
            setgamertag("the iceburg that..");
            Thread.Sleep(2000);
            setgamertag("sunk the titanic");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton143_Click(object sender, EventArgs e)
        {
            setgamertag("Hey ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("Stfu u smelly fucking..");
            Thread.Sleep(2000);
            setgamertag("empty jar of peanutbutter..");
            Thread.Sleep(2000);
            setgamertag("rusty guitar string lookin ass..");
            Thread.Sleep(2000);
            setgamertag("bag of doughnuts");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton144_Click(object sender, EventArgs e)
        {
            setgamertag("OI ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("Your secrets are always safe..");
            Thread.Sleep(2000);
            setgamertag("With me");
            Thread.Sleep(2000);
            setgamertag("I never even listen when");
            Thread.Sleep(2000);
            setgamertag("you tell me them.");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton151_Click(object sender, EventArgs e)
        {
            setgamertag("Hey ^1" + textEdit72.Text);
            Thread.Sleep(2000);
            setgamertag("If your brain was dynamite");
            Thread.Sleep(2000);
            setgamertag("there wouldn’t be enough..");
            Thread.Sleep(2000);
            setgamertag("to blow your hat off");
            Thread.Sleep(2000);
            setgamertag("You sad fuck");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton189_Click(object sender, EventArgs e)
        {
            setgamertag("Yoohoo ^1" + textEdit72.Text + "!");
            Thread.Sleep(2000);
            setgamertag("U FCKN INDIAN RESTROOM SCENTED..");
            Thread.Sleep(2000);
            setgamertag("ABORTION TUBESOCK SIMULATOR");
            Thread.Sleep(2000);
            setgamertag("HEAR MY ICE FISHING FAGGOT");
            Thread.Sleep(2000);
            setgamertag(Encoding.BigEndianUnicode.GetString(JRPC.GetMemory(console, 0x81AA28FC, 30U)).Trim().Trim(new char[1]));
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton203_Click(object sender, EventArgs e)
        {
            if (!(comboBoxEdit10.Text == ""))
            {
                if (comboBoxEdit10.SelectedIndex == 0)
                {
                    console.XNotify(textEdit82.Text, 1U);
                }
                if (comboBoxEdit10.SelectedIndex == 1)
                {
                    console.XNotify(textEdit82.Text, 2U);
                }
                if (comboBoxEdit10.SelectedIndex == 2)
                {
                    console.XNotify(textEdit82.Text, 3U);
                }
                if (comboBoxEdit10.SelectedIndex == 3)
                {
                    console.XNotify(textEdit82.Text, 4U);
                }
                if (comboBoxEdit10.SelectedIndex == 4)
                {
                    console.XNotify(textEdit82.Text, 5U);
                }
                if (comboBoxEdit10.SelectedIndex == 5)
                {
                    console.XNotify(textEdit82.Text, 6U);
                }
                if (comboBoxEdit10.SelectedIndex == 6)
                {
                    console.XNotify(textEdit82.Text, 7U);
                }
                if (comboBoxEdit10.SelectedIndex == 7)
                {
                    console.XNotify(textEdit82.Text, 8U);
                }
                if (comboBoxEdit10.SelectedIndex == 8)
                {
                    console.XNotify(textEdit82.Text, 9U);
                }
                if (comboBoxEdit10.SelectedIndex == 9)
                {
                    console.XNotify(textEdit82.Text, 10U);
                }
                if (comboBoxEdit10.SelectedIndex == 10)
                {
                    console.XNotify(textEdit82.Text, 11U);
                }
                if (comboBoxEdit10.SelectedIndex == 11)
                {
                    console.XNotify(textEdit82.Text, 12U);
                }
                if (comboBoxEdit10.SelectedIndex == 12)
                {
                    console.XNotify(textEdit82.Text, 13U);
                }
                if (comboBoxEdit10.SelectedIndex == 13)
                {
                    console.XNotify(textEdit82.Text, 14U);
                }
                if (comboBoxEdit10.SelectedIndex == 14)
                {
                    console.XNotify(textEdit82.Text, 15U);
                }
                if (comboBoxEdit10.SelectedIndex == 15)
                {
                    console.XNotify(textEdit82.Text, 16U);
                }
                if (comboBoxEdit10.SelectedIndex == 16)
                {
                    console.XNotify(textEdit82.Text, 18U);
                }
                if (comboBoxEdit10.SelectedIndex == 17)
                {
                    console.XNotify(textEdit82.Text, 19U);
                }
                if (comboBoxEdit10.SelectedIndex == 18)
                {
                    console.XNotify(textEdit82.Text, 20U);
                }
                if (comboBoxEdit10.SelectedIndex == 19)
                {
                    console.XNotify(textEdit82.Text, 21U);
                }
                if (comboBoxEdit10.SelectedIndex == 20)
                {
                    console.XNotify(textEdit82.Text, 22U);
                }
                if (comboBoxEdit10.SelectedIndex == 21)
                {
                    console.XNotify(textEdit82.Text + " ", 27U);
                }
                if (comboBoxEdit10.SelectedIndex == 22)
                {
                    console.XNotify(textEdit82.Text, 31U);
                }
            }
            else
            {
                console.XNotify(textEdit82.Text);
            }
        }

        private void simpleButton205_Click(object sender, EventArgs e)
        {
            console.Reboot(null, null, null, XboxRebootFlags.Cold);
        }

        private void simpleButton209_Click(object sender, EventArgs e)
        {
            console.CallVoid("xboxkrnl.exe", 434, 69);
        }

        private void simpleButton208_Click(object sender, EventArgs e)
        {
            console.ShutDownConsole();
        }



        private void simpleButton269_Click(object sender, EventArgs e)
        {

        }

        public void dashmessage(string string_9, string string_10, string string_11)
        {
            object[] objArray1 = new object[] { 0xff, 1 };
            uint num = JRPC.Call<uint>(console, "xam.xex", 0x489, objArray1);
            object[] objArray2 = new object[] { 0x400, 1 };
            uint num2 = JRPC.Call<uint>(console, "xam.xex", 0x489, objArray2);
            object[] objArray3 = new object[] { 8, 1 };
            uint num3 = JRPC.Call<uint>(console, "xam.xex", 0x489, objArray3);
            object[] objArray4 = new object[] { 12, 1 };
            uint num4 = JRPC.Call<uint>(console, "xam.xex", 0x489, objArray4);
            object[] objArray5 = new object[] { 0x20, 1 };
            uint num5 = JRPC.Call<uint>(console, "xam.xex", 0x489, objArray5);
            object[] objArray6 = new object[] { 0x20, 1 };
            uint num6 = JRPC.Call<uint>(console, "xam.xex", 0x489, objArray6);
            JRPC.SetMemory(console, num, messagemethod(string_9));
            JRPC.SetMemory(console, num2, messagemethod(string_10));
            JRPC.WriteUInt32(console, num3, num6);
            JRPC.SetMemory(console, num6, messagemethod(string_11));
            uint num7 = JRPC.ResolveFunction(console, "xam.xex", 0x2ca);
            object[] objArray7 = new object[] { 0, num, num2, 1, num3, 0, 2, num4, num5 };
            JRPC.CallVoid(console, num7, objArray7);
            object[] objArray8 = new object[] { num, 1 };
            JRPC.Call<uint>(console, "xam.xex", 0x48b, objArray8);
            object[] objArray9 = new object[] { num2, 1 };
            JRPC.Call<uint>(console, "xam.xex", 0x48b, objArray9);
            object[] objArray10 = new object[] { num3, 1 };
            JRPC.Call<uint>(console, "xam.xex", 0x48b, objArray10);
            object[] objArray11 = new object[] { num4, 1 };
            JRPC.Call<uint>(console, "xam.xex", 0x48b, objArray11);
            object[] objArray12 = new object[] { num5, 1 };
            JRPC.Call<uint>(console, "xam.xex", 0x48b, objArray12);
            object[] objArray13 = new object[] { num6, 1 };
            JRPC.Call<uint>(console, "xam.xex", 0x48b, objArray13);
        }

        private byte[] messagemethod(string string_9)
        {
            byte[] buffer = new byte[(string_9.Length * 2) + 2];
            int index = 1;
            buffer[0] = 0;
            foreach (char ch in string_9)
            {
                buffer[index] = Convert.ToByte(ch);
                index += 2;
            }
            return buffer;
        }

        private void simpleButton364_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton200_Click(object sender, EventArgs e)
        {
            dashmessage(textEdit80.Text, textBox2.Text, textEdit98.Text);
        }

        private void simpleButton346_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            setgamertag("^H==\u0013gfx_laser_viewmodel\r\n");
        }

        private void simpleButton139_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton138_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton137_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton126_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x8242FB70, new object[]
            {
                -1,
                0,
                "; \"" + textEdit46.Text + "\""
            });
        }

        private void simpleButton125_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x8242FB70, new object[]
                        {
                -1,
                0,
                "< \"" + textEdit42.Text + "\""
                        });
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x8242FB70, new object[]
            {
                -1,
                0,
                "; \"" + textEdit46.Text + "\""
            });
            console.CallVoid(0x8242FB70, new object[]
            {
                -1,
                0,
                "< \"" + textEdit42.Text + "\""
            });
        }

        private void simpleButton140_Click(object sender, EventArgs e)
        {
            bool flag = simpleButton140.Text == "Send Center + Kill Feed [OFF]";
            if (flag)
            {
                simpleButton140.Text = "Send Center + Kill Feed [ON]";
                timer4.Start();
            }
            else
            {
                simpleButton140.Text = "Send Center + Kill Feed [OFF]";
                timer4.Stop();
            }
        }

        private void simpleButton127_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x822786E0, new object[]
            {
                0,
                "callvote map \"mp_hijacked\rstatsetbyname RANKXP 1\rstatsetbyname PLEVEL 0\rstatsetbyname RANK 1\r\""
            });
            console.CallVoid(0x824015E0, new object[]
            {
                0,
                "disconnect"
            });
        }

        private void simpleButton129_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x822786E0, new object[]
            {
                0,
                "callvote map \"mp_hijacked\rquit\r\""
            });
            console.CallVoid(0x824015E0, new object[]
            {
                0,
                "disconnect"
            });
        }

        private void simpleButton130_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x822786E0, new object[]
            {
                0,
                "callvote map \"mp_hijacked\rstartZombies\r\""
            });
            console.CallVoid(0x824015E0, new object[]
            {
                0,
                "disconnect"
            });
        }

        private void simpleButton133_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x822786E0, new object[]
        {
                0,
                "userinfo \"\\name\\^H^2UTR <3\\clanabbrev\\^H69^1\""
        });
            console.CallVoid(0x824015E0, new object[]
            {
                0,
                "disconnect"
            });
        }

        private void simpleButton128_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x822786E0, new object[]
        {
                0,
                "callvote map \"mp_hijacked\runbindall\runbindallaxis\r\""
        });
            console.CallVoid(0x824015E0, new object[]
            {
                0,
                "disconnect"
            });
        }

        private void simpleButton132_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x822786E0, new object[]
            {
                0,
                "callvote map \"mp_hijacked\rstartSingleplayer\r\""
            });
            console.CallVoid(0x824015E0, new object[]
            {
                0,
                "disconnect"
            });
        }

        private void simpleButton131_Click(object sender, EventArgs e)
        {
            console.CallVoid(0x822786E0, new object[]
            {
                0,
                "callvote map \"mp_hijackedÜg_fov 90\r\""
            });
            console.CallVoid(0x824015E0, new object[]
            {
                0,
                "disconnect"
            });
        }

        private void simpleButton134_Click(object sender, EventArgs e)
        {
            bool flag = XtraMessageBox.Show(styleController1.LookAndFeel, "Go into a private match with the victim you want as host, Just have it be you and him, Choose any option once you are in the game, It will kick you from the game and pass the vote to them!", "Thanks To Heaventh For These Options!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) == DialogResult.OK;
            if (flag)
            {
            }
        }

        private void simpleButton135_Click(object sender, EventArgs e)
        {


        }

        private void simpleButton284_Click(object sender, EventArgs e)
        {
            bool flag = comboBoxEdit2.Text == "Default";
            if (flag)
            {
                console.SetMemory(2210750520U, new byte[]
                {
                    63,
                    128,
                    0,
                    0,
                    63,
                    128,
                    0,
                    0,
                    63,
                    128
                });
            }
        }

        private void simpleButton138_Click_1(object sender, EventArgs e)
        {
            int num = listView1.SelectedItems[0].Index;
            bool flag = num == 21;
            bool flag2 = !flag;
            if (flag2)
            {
                try
                {
                    bool flag3 = num < 19 && num > -1;
                    bool flag4 = flag3;
                    if (flag4)
                    {
                        console.CallVoid(0x8242FB70, new object[]
                        {
                            num,
                            0,
                            "; \"^2Nigger Frozen By ^1Water CR!\""
                        });
                        console.CallVoid(0x8242FB70, new object[]
                        {
                            num,
                            0,
                            "< \"^1Nigger Frozen By ^2Water CR!\""
                        });
                        Thread.Sleep(200);
                        console.CallVoid(0x8242FB70, new object[]
                        {
                            num,
                            0,
                            "7 30 90"
                        });
                    }
                }
                catch
                {
                }
            }
        }

        // Token: 0x04000124 RID: 292


        // Token: 0x04000125 RID: 293
        public bool TimerEnabled;

        // Token: 0x04000126 RID: 294


        // Token: 0x04000127 RID: 295
        private INI ini_1;

        // Token: 0x04000128 RID: 296
        private DiscordRpcClient discordRpcClient_0;
        private object Timestamps;

        private void simpleButton137_Click_1(object sender, EventArgs e)
        {
            int num = listView1.SelectedItems[0].Index;
            bool flag = num == 21;
            bool flag2 = !flag;
            if (flag2)
            {
                try
                {
                    bool flag3 = num < 19 && num > -1;
                    bool flag4 = flag3;
                    if (flag4)
                    {
                        console.CallVoid(0x8242FB70, new object[]
                        {
                            selectedIndex,
                            0,
                            "5 \"" + textEdit47.Text + "\""
                        });
                    }
                }
                catch
                {
                }
            }
        }

        private void simpleButton139_Click_1(object sender, EventArgs e)
        {


        }

        private void simpleButton275_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "GSC Files (*.gsc)|*.gsc|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
                bool flag2 = flag;
                if (flag2)
                {
                    uint num = 0x40300000U;
                    string safeFileName;
                    bool flag3 = (safeFileName = openFileDialog.SafeFileName) != null;
                    if (flag3)
                    {
                        bool flag4 = !(safeFileName == "_clientids.gsc");
                        if (flag4)
                        {
                            bool flag5 = !(safeFileName == "_development_dvars.gsc");
                            if (flag5)
                            {
                                bool flag6 = !(safeFileName == "_ambientpackage.gsc");
                                if (flag6)
                                {
                                    bool flag7 = !(safeFileName == "_sticky_grenade.gsc");
                                    if (flag7)
                                    {
                                        bool flag8 = !(safeFileName == "_rank.gsc");
                                        if (flag8)
                                        {
                                            bool flag9 = safeFileName == "_acousticsensor.gsc";
                                            if (flag9)
                                            {
                                                console.WriteUInt32(0x831EBBB4U, num);
                                            }
                                        }
                                        else
                                        {
                                            console.WriteUInt32(2199830484U, num);
                                        }
                                    }
                                    else
                                    {
                                        console.WriteUInt32(2199831900U, num);
                                    }
                                }
                                else
                                {
                                    console.WriteUInt32(2199829440U, num);
                                }
                            }
                            else
                            {
                                console.WriteUInt32(2199829632U, num);
                            }
                        }
                        else
                        {
                            console.WriteUInt32(2199830136U, num);
                        }
                    }
                    console.SetMemory(num, File.ReadAllBytes(openFileDialog.FileName));
                    console.XNotify("Menu Injected\nPointer: 0x" + num.ToString("X"));
                    bool flag10 = XtraMessageBox.Show(styleController1.LookAndFeel, "This was made by supitstom", "Credits!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) == DialogResult.OK;
                    if (flag10)
                    {
                    }
                }
            }
        }

        private void method_16(object sender, ElapsedEventArgs e)
        {
            string @string = Form1.RandomShitGamertag(32);
            this.console.WriteString(2206967844U, @string);
            Thread.Sleep(20);
        }

        public static string RandomShitGamertag(int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%&*()-+=[]{};:.,></?".Length);
                stringBuilder.Append("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%&*()-+=[]{};:.,></?"[index]);
            }
            return stringBuilder.ToString();
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }


        public string GameName()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/927961141664178270/1085243771517665400/titleids.txt", "./titleids.txt");
            string[] array = File.ReadAllLines("./titleids.txt");
            foreach (string text in array)
            {
                string[] array3 = text.Split(new char[]
                {
                    ",".Last<char>()
                });
                string b = array3.First<string>();
                string result = array3[1];
                if (this.console.XamGetCurrentTitleId().ToString("X") == b)
                {
                    return result;
                }
            }
            File.Delete("./titleids.txt");
            return "Unknown Title";
        }

        // Token: 0x04000128 RID: 296


        private void simpleButton13_Click_1(object sender, EventArgs e)
        {

        }

        private void simpleButton31_Click(object sender, EventArgs e)
        {



        }

        private void simpleButton259_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton251_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton250_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton249_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton248_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton247_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton246_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton245_Click_1(object sender, EventArgs e)
        {

        }

        private void simpleButton244_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton243_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton231_Click_1(object sender, EventArgs e)
        {

        }

        private void simpleButton228_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton242_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton8_Click_1(object sender, EventArgs e)
        {

        }

        private void simpleButton298_Click(object sender, EventArgs e)
        {
            numericUpDown147.Text = BitConverter.ToInt32(console.GetMemory(2215176189U, 4U), 0).ToString();
            numericUpDown149.Text = BitConverter.ToInt32(console.GetMemory(2215176197U, 4U), 0).ToString();
            numericUpDown152.Text = BitConverter.ToInt32(console.GetMemory(2215175153U, 4U), 0).ToString();
            numericUpDown153.Text = BitConverter.ToInt32(console.GetMemory(2215176217U, 4U), 0).ToString();
            numericUpDown150.Text = BitConverter.ToInt32(console.GetMemory(2215175497U, 4U), 0).ToString();
            numericUpDown134.Text = BitConverter.ToInt32(console.GetMemory(2215175189U, 4U), 0).ToString();
            numericUpDown128.Text = BitConverter.ToInt32(console.GetMemory(2215176317U, 4U), 0).ToString();
            numericUpDown118.Text = BitConverter.ToInt32(console.GetMemory(2215175677U, 4U), 0).ToString();
            numericUpDown127.Text = BitConverter.ToInt32(console.GetMemory(2215175089U, 4U), 0).ToString();
            numericUpDown133.Text = BitConverter.ToInt32(console.GetMemory(2215175469U, 4U), 0).ToString();
            numericUpDown138.Text = BitConverter.ToInt32(console.GetMemory(2215175489U, 4U), 0).ToString();
            numericUpDown140.Text = BitConverter.ToInt32(console.GetMemory(2215175993U, 4U), 0).ToString();
            int num = BitConverter.ToInt32(console.GetMemory(2215176261U, 4U), 0);
            int num2 = num / 86400;
            int num3 = (num - num2 * 86400) / 3600;
            int num4 = (num - num2 * 86400 - num3 * 3600) / 60;
            numericUpDown156.Text = num2.ToString();
            numericUpDown155.Text = num3.ToString();
            numericUpDown154.Text = num4.ToString();
            int num5 = BitConverter.ToInt32(console.GetMemory(2215176265U, 4U), 0);
            int num6 = num5 / 86400;
            int num7 = (num5 - num6 * 86400) / 3600;
            int num8 = (num5 - num6 * 86400 - num7 * 3600) / 60;
            numericUpDown141.Text = num6.ToString();
            numericUpDown143.Text = num7.ToString();
            numericUpDown146.Text = num8.ToString();
            int num9 = BitConverter.ToInt32(console.GetMemory(2215176269U, 4U), 0);
            int num10 = num9 / 86400;
            int num11 = (num9 - num10 * 86400) / 3600;
            int num12 = (num9 - num10 * 86400 - num11 * 3600) / 60;
            numericUpDown151.Text = num10.ToString();
            numericUpDown145.Text = num11.ToString();
            numericUpDown142.Text = num12.ToString();
            int num13 = BitConverter.ToInt32(console.GetMemory(2215176273U, 4U), 0);
            int num14 = num13 / 86400;
            int num15 = (num13 - num14 * 86400) / 3600;
            int num16 = (num13 - num14 * 86400 - num15 * 3600) / 60;
            numericUpDown139.Text = num14.ToString();
            numericUpDown137.Text = num15.ToString();
            numericUpDown135.Text = num16.ToString();
            numericUpDown147.Enabled = true;
            numericUpDown149.Enabled = true;
            numericUpDown152.Enabled = true;
            numericUpDown153.Enabled = true;
            numericUpDown150.Enabled = true;
            numericUpDown134.Enabled = true;
            numericUpDown128.Enabled = true;
            numericUpDown118.Enabled = true;
            numericUpDown127.Enabled = true;
            numericUpDown133.Enabled = true;
            numericUpDown156.Enabled = true;
            numericUpDown155.Enabled = true;
            numericUpDown154.Enabled = true;
            numericUpDown141.Enabled = true;
            numericUpDown143.Enabled = true;
            numericUpDown146.Enabled = true;
            numericUpDown151.Enabled = true;
            numericUpDown145.Enabled = true;
            numericUpDown142.Enabled = true;
            numericUpDown139.Enabled = true;
            numericUpDown137.Enabled = true;
            numericUpDown135.Enabled = true;
            numericUpDown138.Enabled = true;
            numericUpDown140.Enabled = true;
            simpleButton272.Enabled = true;
            simpleButton298.Enabled = false;
            simpleButton270.Enabled = true;
        }

        private void simpleButton272_Click(object sender, EventArgs e)
        {
            byte[] bytes = BitConverter.GetBytes(Convert.ToInt32(numericUpDown147.Text));
            console.SetMemory(2215176189U, bytes);
            byte[] bytes2 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown149.Text));
            console.SetMemory(2215176197U, bytes2);
            byte[] bytes3 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown152.Text));
            console.SetMemory(2215175153U, bytes3);
            byte[] bytes4 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown153.Text));
            console.SetMemory(2215176217U, bytes4);
            byte[] bytes5 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown150.Text));
            console.SetMemory(2215175497U, bytes5);
            byte[] bytes6 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown134.Text));
            console.SetMemory(2215175189U, bytes6);
            byte[] bytes7 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown128.Text));
            console.SetMemory(2215176317U, bytes7);
            byte[] bytes8 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown118.Text));
            console.SetMemory(2215175677U, bytes8);
            byte[] bytes9 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown127.Text));
            console.SetMemory(2215175089U, bytes9);
            byte[] bytes10 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown133.Text));
            console.SetMemory(2215175469U, bytes10);
            byte[] bytes11 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown138.Text));
            console.SetMemory(2215175489U, bytes11);
            byte[] bytes12 = BitConverter.GetBytes(Convert.ToInt32(numericUpDown140.Text));
            console.SetMemory(2215175993U, bytes12);
            int num = Convert.ToInt32(numericUpDown156.Text);
            int num2 = Convert.ToInt32(numericUpDown155.Text);
            int num3 = Convert.ToInt32(numericUpDown154.Text);
            int value = num * 86400 + num2 * 3600 + num3 * 60;
            byte[] bytes13 = BitConverter.GetBytes(value);
            console.SetMemory(2215176261U, bytes13);
            int num4 = Convert.ToInt32(numericUpDown141.Text);
            int num5 = Convert.ToInt32(numericUpDown143.Text);
            int num6 = Convert.ToInt32(numericUpDown146.Text);
            int value2 = num4 * 86400 + num5 * 3600 + num6 * 60;
            byte[] bytes14 = BitConverter.GetBytes(value2);
            console.SetMemory(2215176265U, bytes14);
            int num7 = Convert.ToInt32(numericUpDown151.Text);
            int num8 = Convert.ToInt32(numericUpDown145.Text);
            int num9 = Convert.ToInt32(numericUpDown142.Text);
            int value3 = num7 * 86400 + num8 * 3600 + num9 * 60;
            byte[] bytes15 = BitConverter.GetBytes(value3);
            console.SetMemory(2215176269U, bytes15);
            int num10 = Convert.ToInt32(numericUpDown139.Text);
            int num11 = Convert.ToInt32(numericUpDown137.Text);
            int num12 = Convert.ToInt32(numericUpDown135.Text);
            int value4 = num10 * 86400 + num11 * 3600 + num12 * 60;
            byte[] bytes16 = BitConverter.GetBytes(value4);
            console.SetMemory(2215176273U, bytes16);
            numericUpDown147.Enabled = false;
            numericUpDown149.Enabled = false;
            numericUpDown152.Enabled = false;
            numericUpDown153.Enabled = false;
            numericUpDown150.Enabled = false;
            numericUpDown134.Enabled = false;
            numericUpDown128.Enabled = false;
            numericUpDown118.Enabled = false;
            numericUpDown127.Enabled = false;
            numericUpDown133.Enabled = false;
            numericUpDown156.Enabled = false;
            numericUpDown155.Enabled = false;
            numericUpDown154.Enabled = false;
            numericUpDown141.Enabled = false;
            numericUpDown143.Enabled = false;
            numericUpDown146.Enabled = false;
            numericUpDown151.Enabled = false;
            numericUpDown145.Enabled = false;
            numericUpDown142.Enabled = false;
            numericUpDown139.Enabled = false;
            numericUpDown137.Enabled = false;
            numericUpDown135.Enabled = false;
            numericUpDown138.Enabled = false;
            numericUpDown140.Enabled = false;
            simpleButton272.Enabled = false;
            simpleButton298.Enabled = true;
            simpleButton30.Enabled = false;
            console.XNotify("Stats saved");
        }

        private void simpleButton270_Click(object sender, EventArgs e)
        {
            console.WriteUInt32(2215141031U, 4294967055U);
            console.SetMemory(2215145697U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            console.SetMemory(2215145793U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data = new byte[]
            {
                192
            };
            console.SetMemory(2215146178U, data);
            byte[] data2 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215148128U, data2);
            byte[] data3 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215148380U, data3);
            byte[] data4 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215148967U, data4);
            byte[] data5 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215149051U, data5);
            byte[] data6 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215149303U, data6);
            byte[] data7 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215149135U, data7);
            byte[] data8 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215149386U, data8);
            byte[] data9 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215149470U, data9);
            byte[] data10 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215149554U, data10);
            byte[] data11 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215149638U, data11);
            byte[] data12 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215149647U, data12);
            byte[] data13 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215148128U, data13);
            byte[] data14 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150141U, data14);
            byte[] data15 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150225U, data15);
            byte[] data16 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150309U, data16);
            byte[] data17 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150393U, data17);
            byte[] data18 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150477U, data18);
            byte[] data19 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150561U, data19);
            byte[] data20 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150645U, data20);
            byte[] data21 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150728U, data21);
            byte[] data22 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215150812U, data22);
            byte[] data23 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215151148U, data23);
            byte[] data24 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215151232U, data24);
            byte[] data25 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215151316U, data25);
            byte[] data26 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215151567U, data26);
            byte[] data27 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215151651U, data27);
            byte[] data28 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215151735U, data28);
            byte[] data29 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215151987U, data29);
            byte[] data30 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215151903U, data30);
            byte[] data31 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215152154U, data31);
            byte[] data32 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215152490U, data32);
            byte[] data33 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215152574U, data33);
            byte[] data34 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215152658U, data34);
            byte[] data35 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215152741U, data35);
            byte[] data36 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215153161U, data36);
            byte[] data37 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215153329U, data37);
            byte[] data38 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215153412U, data38);
            byte[] data39 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215153664U, data39);
            byte[] data40 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215153748U, data40);
            byte[] data41 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215153832U, data41);
            byte[] data42 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215153916U, data42);
            byte[] data43 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215154083U, data43);
            byte[] data44 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215154167U, data44);
            byte[] data45 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215154251U, data45);
            byte[] data46 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215154335U, data46);
            byte[] data47 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215154419U, data47);
            byte[] data48 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215154503U, data48);
            console.SetMemory(2215156935U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data49 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215160541U, data49);
            console.SetMemory(2215160621U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data50 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215160709U, data50);
            byte[] data51 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215160793U, data51);
            byte[] data52 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215160877U, data52);
            byte[] data53 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215160961U, data53);
            byte[] data54 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161045U, data54);
            byte[] data55 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161129U, data55);
            byte[] data56 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215161212U, data56);
            byte[] data57 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161297U, data57);
            byte[] data58 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215161380U, data58);
            byte[] data59 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161464U, data59);
            byte[] data60 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161548U, data60);
            byte[] data61 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161632U, data61);
            byte[] data62 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161716U, data62);
            byte[] data63 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161800U, data63);
            byte[] data64 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215161883U, data64);
            byte[] data65 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215161968U, data65);
            byte[] data66 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215162051U, data66);
            byte[] data67 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215162135U, data67);
            byte[] data68 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215162218U, data68);
            byte[] data69 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215162303U, data69);
            byte[] data70 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215162386U, data70);
            byte[] data71 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215162471U, data71);
            byte[] data72 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215162554U, data72);
            byte[] data73 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215162639U, data73);
            byte[] data74 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215162722U, data74);
            byte[] data75 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215162806U, data75);
            byte[] data76 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215162890U, data76);
            byte[] data77 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215164316U, data77);
            byte[] data78 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215164400U, data78);
            byte[] data79 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215164568U, data79);
            byte[] data80 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215164736U, data80);
            byte[] data81 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215164819U, data81);
            byte[] data82 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215164899U, data82);
            byte[] data83 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215164903U, data83);
            byte[] data84 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215165155U, data84);
            byte[] data85 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215165239U, data85);
            byte[] data86 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215165323U, data86);
            byte[] data87 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215165407U, data87);
            byte[] data88 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215165490U, data88);
            byte[] data89 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215165658U, data89);
            byte[] data90 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215176914U, data90);
            byte[] data91 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215176932U, data91);
            byte[] data92 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215176949U, data92);
            byte[] data93 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215176967U, data93);
            byte[] data94 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215176985U, data94);
            byte[] data95 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177110U, data95);
            console.SetMemory(2215177115U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data96 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177128U, data96);
            console.SetMemory(2215177133U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data97 = new byte[]
            {
                byte.MaxValue,
                15
            };
            console.SetMemory(2215177146U, data97);
            console.SetMemory(2215177151U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data98 = new byte[]
            {
                byte.MaxValue,
                1
            };
            console.SetMemory(2215177170U, data98);
            console.SetMemory(2215177168U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data99 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177182U, data99);
            byte[] data100 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177200U, data100);
            console.SetMemory(2215177204U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data101 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177212U, data101);
            byte[] data102 = new byte[]
            {
                byte.MaxValue,
                1
            };
            console.SetMemory(2215177218U, data102);
            console.SetMemory(2215177222U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data103 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177230U, data103);
            byte[] data104 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177235U, data104);
            console.SetMemory(2215177240U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data105 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177253U, data105);
            console.SetMemory(2215177258U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data106 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177361U, data106);
            console.SetMemory(2215177365U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            console.SetMemory(2215177378U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            console.SetMemory(2215177383U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data107 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177396U, data107);
            console.SetMemory(2215177401U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data108 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177414U, data108);
            console.SetMemory(2215177419U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data109 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177432U, data109);
            console.SetMemory(2215177437U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data110 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177450U, data110);
            console.SetMemory(2215177454U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data111 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177468U, data111);
            console.SetMemory(2215177472U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data112 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177480U, data112);
            byte[] data113 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177486U, data113);
            byte[] data114 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177480U, data114);
            console.SetMemory(2215177490U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data115 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177504U, data115);
            console.SetMemory(2215177508U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            console.SetMemory(2215177521U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            console.SetMemory(2215177526U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data116 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177557U, data116);
            console.SetMemory(2215177562U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data117 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177575U, data117);
            console.SetMemory(2215177580U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data118 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177593U, data118);
            console.SetMemory(2215177597U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data119 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177611U, data119);
            console.SetMemory(2215177615U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data120 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177647U, data120);
            console.SetMemory(2215177651U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data121 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177659U, data121);
            byte[] data122 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177664U, data122);
            console.SetMemory(2215177669U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data123 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177677U, data123);
            byte[] data124 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177682U, data124);
            console.SetMemory(2215177687U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data125 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177694U, data125);
            byte[] data126 = new byte[]
            {
                byte.MaxValue,
                byte.MaxValue
            };
            console.SetMemory(2215177700U, data126);
            console.SetMemory(2215177704U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data127 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177712U, data127);
            byte[] data128 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177736U, data128);
            console.SetMemory(2215177740U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data129 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177748U, data129);
            byte[] data130 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177754U, data130);
            console.SetMemory(2215177758U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data131 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177766U, data131);
            console.SetMemory(2215177776U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data132 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177784U, data132);
            byte[] data133 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177790U, data133);
            console.SetMemory(2215177794U, new byte[]
            {
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue
            });
            byte[] data134 = new byte[]
            {
                byte.MaxValue
            };
            console.SetMemory(2215177802U, data134);
            console.XNotify("Successfully unlocked all.");
        }

        private void simpleButton28_Click(object sender, EventArgs e)
        {
            console.WriteByte(0xB4269000, Properties.Resources.Moon);
            console.XNotify("Moon Theme - Loaded!");
        }
    }
}
            


