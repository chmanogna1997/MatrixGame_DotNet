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
    public partial class Form4 : Form
    {
        public string User { set; get; }
        public List<string> History_list { set; get; }
        public Form4()
        {
            InitializeComponent();
        }

        private void Game1_BTN_Click(object sender, EventArgs e)
        {
            this.Hide();
            History_btn Game11 = new History_btn();
            Game11.User = User; 
        Game11.ShowDialog();
        }

        private void Game2_BTN_Click(object sender, EventArgs e)
        {
            this.Hide();
            Game2 Game_22 = new Game2();
            Game_22.User = User;
            Game_22.ShowDialog();
        }

        private void Game3_BTN_Click(object sender, EventArgs e)
        {
            this.Hide();
            Game_3 Game_33 = new Game_3();
            Game_33.User = User;      
            Game_33.ShowDialog();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            Hi_Label.Text = " HI  " + User;
        }
    }
}
