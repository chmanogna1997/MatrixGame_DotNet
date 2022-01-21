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
    public partial class Form3 : Form
    {
        int Text_changed = 0;
        string User;
        public int From_Game { set; get; }
        public Form3()
        {
            InitializeComponent();
        }
         
        private void Game_1(object sender, EventArgs e)
        {
            this.Hide();
            History_btn Game1 = new History_btn();
            string user = Name_TXT.Text;
            Game1.User = user;
            Game1.ShowDialog();
        }

        private void Game2(object sender, EventArgs e)
        {
            this.Hide();
            Game2 Game_2 = new Game2();
            string user = Name_TXT.Text;
            Game_2.User = user;
            Game_2.ShowDialog();
        }

        private void NameChanged(object sender, EventArgs e)
        {
            if(Name_TXT.Text != "" && Name_TXT.Text != null && this.From_Game != 1) { Text_changed = 1; User = Name_TXT.Text; }
            else { Text_changed = 0;  }
            Enable_Games();
        }

    public void Enable_Games()
        {
         if(Text_changed == 1)
            {
                Game1_BTN.Enabled = true;
                Game2_BTN.Enabled = true;
                Game3_BTN.Enabled = true;
            }
            else
            {
                Game1_BTN.Enabled = false;
                Game2_BTN.Enabled = false;
                Game3_BTN.Enabled = false;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if(this.From_Game == 1)
            {
                Game1_BTN.Enabled = true;
                Game2_BTN.Enabled = true;
                Game3_BTN.Enabled = true;
                Name_TXT.Visible = false;
            }

        }

        private void BTN_Game3(object sender, EventArgs e)
        {
            this.Hide();
            Game_3 Game_3 = new Game_3();
            string user = Name_TXT.Text;
            Game_3.User = user;
            Game_3.ShowDialog();
        }
    }
}
