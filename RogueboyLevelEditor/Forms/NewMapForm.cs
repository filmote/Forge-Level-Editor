﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using RogueboyLevelEditor.map;
using RogueboyLevelEditor.map.Component;
using System.Text.RegularExpressions;

namespace RogueboyLevelEditor.Forms
{
    public delegate void Callback(NewMapForm form);
    public partial class NewMapForm : Form
    {
        // Strictly speaking the range of valid characters is greater than this,
        // but restricting to just alphanumerics is probably wise for sensible map names.
        private static Regex identifierRegex = new Regex("^[_a-zA-Z][_a-zA-Z0-9]*$");

        public event Callback callback;
        public Map Output { get; private set; }
        string[] Taken;
        public bool Valid = false;
        string Filepath;
        public NewMapForm(string Filepath = "", string[] TakenNames = null)
        {
            Taken = TakenNames;
            this.Filepath = Filepath;
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.textBox1.Text))
            {
                this.errorProvider1.SetError(this.textBox1, "Map name cannot be empty");
                return;
            }

            if ((this.Taken != null) && (this.Taken.Contains(this.textBox1.Text)))
            {
                this.errorProvider1.SetError(textBox1, "Two maps cannot have the same name");
                return;
            }

            if(!identifierRegex.IsMatch(textBox1.Text))
            {
                this.errorProvider1.SetError(this.textBox1, "Map name is not a valid identifier");
                return;
            }

            //if (string.IsNullOrWhiteSpace(Filepath)||(Filepath == ""))
            //{
            //    folderBrowserDialog1.SelectedPath = Directory.GetCurrentDirectory() + "\\Maps";
            //    DialogResult result = folderBrowserDialog1.ShowDialog();

            //    if (result == DialogResult.Cancel)
            //    {
            //        return;
            //    }
            //    Filepath = folderBrowserDialog1.SelectedPath;
            //} 
            Output = new Map(new BaseMapComponent(-1),textBox1.Text, Filepath, (int)numericUpDown1.Value,(int)numericUpDown2.Value,(int)numericUpDown3.Value);
            Valid = true;
            callback?.Invoke(this);
        }
        public void CloseForm()
        {
            this.Close();
        }

        private void NewMapForm_Load(object sender, EventArgs e)
        {
            this.FormClosing += NewMapForm_FormClosing;
        }

        private void NewMapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            //if (e.CloseReason == CloseReason.UserClosing)
            //{
            //    Valid = false;
            //    if (!Closing)
            //    {
            //        Closing = true;
            //        callback?.Invoke(this);
            //    }
            //}
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Valid = false;
            callback?.Invoke(this);
        }
    }
}
