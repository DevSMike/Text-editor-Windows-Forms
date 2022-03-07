
using System;
using System.Drawing;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class Form3
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.group = new System.Windows.Forms.GroupBox();
            this.rbutton1 = new System.Windows.Forms.RadioButton();
            this.rbutton2 = new System.Windows.Forms.RadioButton();
            this.rbutton3 = new System.Windows.Forms.RadioButton();
            this.rbutton4 = new System.Windows.Forms.RadioButton();
            this.group.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(190, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Отрыть файл в HEX";
            this.button1.Click += new System.EventHandler(this.loadBytesFromFile);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(198, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(190, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Очистить";
            this.button2.Click += new System.EventHandler(this.clearBytes);
            // 
            // group
            // 
            this.group.Controls.Add(this.rbutton1);
            this.group.Controls.Add(this.rbutton2);
            this.group.Controls.Add(this.rbutton3);
            this.group.Controls.Add(this.rbutton4);
            this.group.Location = new System.Drawing.Point(418, 3);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(220, 36);
            this.group.TabIndex = 2;
            this.group.TabStop = false;
            this.group.Text = "Режим";
            // 
            // rbutton1
            // 
            this.rbutton1.Checked = true;
            this.rbutton1.Location = new System.Drawing.Point(6, 15);
            this.rbutton1.Name = "rbutton1";
            this.rbutton1.Size = new System.Drawing.Size(46, 16);
            this.rbutton1.TabIndex = 0;
            this.rbutton1.TabStop = true;
            this.rbutton1.Text = "Auto";
            this.rbutton1.Click += new System.EventHandler(this.changeByteMode);
            // 
            // rbutton2
            // 
            this.rbutton2.Location = new System.Drawing.Point(54, 15);
            this.rbutton2.Name = "rbutton2";
            this.rbutton2.Size = new System.Drawing.Size(50, 16);
            this.rbutton2.TabIndex = 1;
            this.rbutton2.Text = "ANSI";
            this.rbutton2.Click += new System.EventHandler(this.changeByteMode);
            // 
            // rbutton3
            // 
            this.rbutton3.Location = new System.Drawing.Point(106, 15);
            this.rbutton3.Name = "rbutton3";
            this.rbutton3.Size = new System.Drawing.Size(46, 16);
            this.rbutton3.TabIndex = 2;
            this.rbutton3.Text = "Hex";
            this.rbutton3.Click += new System.EventHandler(this.changeByteMode);
            // 
            // rbutton4
            // 
            this.rbutton4.Location = new System.Drawing.Point(152, 15);
            this.rbutton4.Name = "rbutton4";
            this.rbutton4.Size = new System.Drawing.Size(64, 16);
            this.rbutton4.TabIndex = 3;
            this.rbutton4.Text = "Unicode";
            this.rbutton4.Click += new System.EventHandler(this.changeByteMode);
            // 
            // Form3
            // 
            this.ClientSize = new System.Drawing.Size(664, 401);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.group);
            this.MinimumSize = new System.Drawing.Size(660, 400);
            this.Name = "Form3";
            this.Text = "HexEditor";
            this.group.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void clearBytes(object sender, EventArgs e)
        {
            byteviewer.SetBytes(new byte[] { });
        }

        private void changeByteMode(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton rbutton =
                 (System.Windows.Forms.RadioButton)sender;

            DisplayMode mode;
            switch (rbutton.Text)
            {
                case "ANSI":
                    mode = DisplayMode.Ansi;
                    break;
                case "Hex":
                    mode = DisplayMode.Hexdump;
                    break;
                case "Unicode":
                    mode = DisplayMode.Unicode;
                    break;
                default:
                    mode = DisplayMode.Auto;
                    break;
            }

            // Sets the display mode.
            byteviewer.SetDisplayMode(mode);
        }

        private void loadBytesFromFile(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            byteviewer.SetFile(ofd.FileName);
        }

        #endregion
        private System.Windows.Forms.Button button1;
       // private System.Windows.Forms.RichTextBox richTextBox1;
       // private System.Windows.Forms.RichTextBox richTextBox2;
        private GroupBox group;
        private RadioButton rbutton1;
        private RadioButton rbutton2;
        private RadioButton rbutton3;
        private RadioButton rbutton4;
        private Button button2;
        // private Button button2;

    }
   

   
}