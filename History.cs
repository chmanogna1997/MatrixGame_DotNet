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
    public partial class History : Form
    {
        public string User { set; get; }

        public List<string> History_list { set; get; }
        public History()
        {
            InitializeComponent();
        }

        private void History_Load(object sender, EventArgs e)
        {
            foreach(string g in History_list)
            {
                listBox1.Items.Add(g);
            }
            
        }
    }
}
