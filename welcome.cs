using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TermProj
{
    public partial class welcome : Form
    {
        public welcome()
        {
            InitializeComponent();
        }
        public int start = 0;
        public string user;
        private void Start_btn(object sender, EventArgs e)
        {
            user = Name_txt.Text;
            Console.WriteLine(" user name is {0}", this);
            start = 1;
            this.Close();
        }
    }
}
