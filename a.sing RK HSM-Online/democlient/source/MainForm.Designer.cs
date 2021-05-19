namespace DemoClient
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button13 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button14 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.tbsessionkey = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbsessionid = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tbpartneruser = new System.Windows.Forms.TextBox();
            this.button12 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbpartnerpwd = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button17 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.betrag_besonders = new System.Windows.Forms.TextBox();
            this.betrag_ermaessigt_2 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.betrag_null = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.betrag_ermaessigt_1 = new System.Windows.Forms.TextBox();
            this.betrag_normal = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtAes = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBelegnum = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tb_zertifikat = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL: ";
            // 
            // tbURL
            // 
            this.tbURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbURL.Location = new System.Drawing.Point(99, 12);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(654, 20);
            this.tbURL.TabIndex = 1;
            this.tbURL.Text = "https://hs-abnahme.a-trust.at/RegistrierkasseMobile/v2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(587, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Signieren";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 91);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(741, 203);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button13);
            this.tabPage1.Controls.Add(this.button6);
            this.tabPage1.Controls.Add(this.button5);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(733, 177);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Endkunden Befehle";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(6, 6);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(130, 23);
            this.button13.TabIndex = 21;
            this.button13.Text = "Signieren JWS";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(142, 64);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(130, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = "Change Password";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(142, 35);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(130, 23);
            this.button5.TabIndex = 10;
            this.button5.Text = "Zertifkat";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(142, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(130, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "ZDA Info";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(587, 64);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Signieren PlainText";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(587, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Signieren Hash ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button14);
            this.tabPage4.Controls.Add(this.button9);
            this.tabPage4.Controls.Add(this.button10);
            this.tabPage4.Controls.Add(this.button11);
            this.tabPage4.Controls.Add(this.tbsessionkey);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.tbsessionid);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.button8);
            this.tabPage4.Controls.Add(this.button7);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(733, 177);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Endkunden Befehle (Session)";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(142, 6);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(130, 23);
            this.button14.TabIndex = 32;
            this.button14.Text = "Signieren JWS";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click_1);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(587, 64);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(130, 23);
            this.button9.TabIndex = 31;
            this.button9.Text = "Signieren PlainText";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(587, 35);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(130, 23);
            this.button10.TabIndex = 30;
            this.button10.Text = "Signieren Hash ";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click_1);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(587, 6);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(130, 23);
            this.button11.TabIndex = 29;
            this.button11.Text = "Signieren";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click_1);
            // 
            // tbsessionkey
            // 
            this.tbsessionkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbsessionkey.Location = new System.Drawing.Point(80, 96);
            this.tbsessionkey.Name = "tbsessionkey";
            this.tbsessionkey.Size = new System.Drawing.Size(318, 20);
            this.tbsessionkey.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "sessionkey:";
            // 
            // tbsessionid
            // 
            this.tbsessionid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbsessionid.Location = new System.Drawing.Point(80, 71);
            this.tbsessionid.Name = "tbsessionid";
            this.tbsessionid.Size = new System.Drawing.Size(318, 20);
            this.tbsessionid.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "sessionid:";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(6, 35);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(130, 23);
            this.button8.TabIndex = 24;
            this.button8.Text = "Close Session";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click_1);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(6, 6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(130, 23);
            this.button7.TabIndex = 23;
            this.button7.Text = "Start Session";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbpartneruser);
            this.tabPage2.Controls.Add(this.button12);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.tbpartnerpwd);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(733, 177);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Partner Befehle";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tbpartneruser
            // 
            this.tbpartneruser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbpartneruser.Location = new System.Drawing.Point(130, 6);
            this.tbpartneruser.Name = "tbpartneruser";
            this.tbpartneruser.Size = new System.Drawing.Size(590, 20);
            this.tbpartneruser.TabIndex = 8;
            this.tbpartneruser.Text = "test";
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(9, 72);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(130, 23);
            this.button12.TabIndex = 11;
            this.button12.Text = "Konto anlegen";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Partner Benutzername: ";
            // 
            // tbpartnerpwd
            // 
            this.tbpartnerpwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbpartnerpwd.Location = new System.Drawing.Point(130, 32);
            this.tbpartnerpwd.Name = "tbpartnerpwd";
            this.tbpartnerpwd.Size = new System.Drawing.Size(590, 20);
            this.tbpartnerpwd.TabIndex = 10;
            this.tbpartnerpwd.Text = "test1234";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Partner Passwort:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.button17);
            this.tabPage5.Controls.Add(this.button16);
            this.tabPage5.Controls.Add(this.button15);
            this.tabPage5.Controls.Add(this.betrag_besonders);
            this.tabPage5.Controls.Add(this.betrag_ermaessigt_2);
            this.tabPage5.Controls.Add(this.label18);
            this.tabPage5.Controls.Add(this.label19);
            this.tabPage5.Controls.Add(this.betrag_null);
            this.tabPage5.Controls.Add(this.label16);
            this.tabPage5.Controls.Add(this.betrag_ermaessigt_1);
            this.tabPage5.Controls.Add(this.betrag_normal);
            this.tabPage5.Controls.Add(this.label15);
            this.tabPage5.Controls.Add(this.label14);
            this.tabPage5.Controls.Add(this.txtAes);
            this.tabPage5.Controls.Add(this.label13);
            this.tabPage5.Controls.Add(this.txtBelegnum);
            this.tabPage5.Controls.Add(this.label12);
            this.tabPage5.Controls.Add(this.label11);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(733, 177);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "durchgängiges Beispiel";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(467, 142);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(241, 23);
            this.button17.TabIndex = 17;
            this.button17.Text = "export cryptographicMaterialContainer";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(316, 142);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(145, 23);
            this.button16.TabIndex = 16;
            this.button16.Text = "export DEP";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(6, 142);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(145, 23);
            this.button15.TabIndex = 15;
            this.button15.Text = "Beleg hinzufügen";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // betrag_besonders
            // 
            this.betrag_besonders.Location = new System.Drawing.Point(529, 72);
            this.betrag_besonders.Name = "betrag_besonders";
            this.betrag_besonders.Size = new System.Drawing.Size(100, 20);
            this.betrag_besonders.TabIndex = 14;
            this.betrag_besonders.Text = "0,00";
            // 
            // betrag_ermaessigt_2
            // 
            this.betrag_ermaessigt_2.Location = new System.Drawing.Point(529, 46);
            this.betrag_ermaessigt_2.Name = "betrag_ermaessigt_2";
            this.betrag_ermaessigt_2.Size = new System.Drawing.Size(100, 20);
            this.betrag_ermaessigt_2.TabIndex = 13;
            this.betrag_ermaessigt_2.Text = "0,00";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(361, 75);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(144, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "Betrag-Satz-Besonders (19%)";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(361, 49);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(148, 13);
            this.label19.TabIndex = 11;
            this.label19.Text = "Betrag-Satz-Ermässigt-2 (13%)";
            // 
            // betrag_null
            // 
            this.betrag_null.Location = new System.Drawing.Point(171, 98);
            this.betrag_null.Name = "betrag_null";
            this.betrag_null.Size = new System.Drawing.Size(100, 20);
            this.betrag_null.TabIndex = 10;
            this.betrag_null.Text = "0,00";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 101);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(106, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Betrag-Satz-Null (0%)";
            // 
            // betrag_ermaessigt_1
            // 
            this.betrag_ermaessigt_1.Location = new System.Drawing.Point(171, 72);
            this.betrag_ermaessigt_1.Name = "betrag_ermaessigt_1";
            this.betrag_ermaessigt_1.Size = new System.Drawing.Size(100, 20);
            this.betrag_ermaessigt_1.TabIndex = 8;
            this.betrag_ermaessigt_1.Text = "0,00";
            // 
            // betrag_normal
            // 
            this.betrag_normal.Location = new System.Drawing.Point(171, 46);
            this.betrag_normal.Name = "betrag_normal";
            this.betrag_normal.Size = new System.Drawing.Size(100, 20);
            this.betrag_normal.TabIndex = 7;
            this.betrag_normal.Text = "35,32";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 75);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(148, 13);
            this.label15.TabIndex = 6;
            this.label15.Text = "Betrag-Satz-Ermässigt-1 (10%)";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 49);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(127, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Betrag-Satz-Normal (20%)";
            // 
            // txtAes
            // 
            this.txtAes.Location = new System.Drawing.Point(561, 7);
            this.txtAes.Name = "txtAes";
            this.txtAes.ReadOnly = true;
            this.txtAes.Size = new System.Drawing.Size(169, 20);
            this.txtAes.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(481, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "AES Schlüssel:";
            // 
            // txtBelegnum
            // 
            this.txtBelegnum.Location = new System.Drawing.Point(248, 7);
            this.txtBelegnum.Name = "txtBelegnum";
            this.txtBelegnum.ReadOnly = true;
            this.txtBelegnum.Size = new System.Drawing.Size(169, 20);
            this.txtBelegnum.TabIndex = 2;
            this.txtBelegnum.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(168, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Belegnummer:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Kassen ID: ";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tb_zertifikat);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(733, 177);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "temp Daten";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tb_zertifikat
            // 
            this.tb_zertifikat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_zertifikat.Location = new System.Drawing.Point(70, 15);
            this.tb_zertifikat.Name = "tb_zertifikat";
            this.tb_zertifikat.Size = new System.Drawing.Size(643, 20);
            this.tb_zertifikat.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Zertifikat: ";
            // 
            // tbResult
            // 
            this.tbResult.AcceptsReturn = true;
            this.tbResult.AcceptsTab = true;
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Location = new System.Drawing.Point(12, 300);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResult.Size = new System.Drawing.Size(741, 243);
            this.tbResult.TabIndex = 8;
            // 
            // tbUser
            // 
            this.tbUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUser.Location = new System.Drawing.Point(99, 38);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(650, 20);
            this.tbUser.TabIndex = 38;
            this.tbUser.Text = "u943578651";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Benutzername: ";
            // 
            // tbPwd
            // 
            this.tbPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPwd.Location = new System.Drawing.Point(99, 64);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.Size = new System.Drawing.Size(650, 20);
            this.tbPwd.TabIndex = 40;
            this.tbPwd.Text = "nv66nx";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 39;
            this.label10.Text = "Passwort:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 555);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbPwd);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tbURL);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "A-Trust Registrierkasse Mobile - Demo Client";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox tbpartneruser;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbpartnerpwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox tb_zertifikat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.TextBox tbsessionkey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbsessionid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox txtAes;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBelegnum;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.TextBox betrag_besonders;
        private System.Windows.Forms.TextBox betrag_ermaessigt_2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox betrag_null;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox betrag_ermaessigt_1;
        private System.Windows.Forms.TextBox betrag_normal;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.Label label10;
    }
}

