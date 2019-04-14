using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisitorForm
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        

        private void buttonStart_Click_1(object sender, EventArgs e)
        {
            //this.Hide();
            Form1 f = new Form1();
            f.ShowDialog();
        }
    }
}
