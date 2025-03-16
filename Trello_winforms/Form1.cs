using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trello_winforms
{
    public partial class Form1 : Form
    {
        private Sections section;
        public Form1()
        {
            InitializeComponent();
            section = new Sections(this);
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip2.Show(this, e.Location);
            }
        }

        private void toolStripTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            section.AddSection();
            contextMenuStrip2.Visible = false;
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
