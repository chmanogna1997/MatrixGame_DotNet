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
    public partial class History_btn : Form
    {
        TextBox[,] TB;
        System.Timers.Timer t;
        int HR, MIN, SEC;
        string User;
        int LostFocus = 0;
        int TextChanged = 0;
        int a = 14;
        int b = 10;
        int initial_x;
        int initial_y;
        string timer, Date_Time_Now;
        List<TextBox> Adjacent_box = new List<TextBox>();
        //List<int> value_in_box = new List<int>();
        List<Button> History_BTN = new List<Button>();
        List<Label> History_LB = new List<Label>();
        List<KeyValuePair<string, string>> occupied_boxs = new List<KeyValuePair<string, string>>();
        int[,] Filled_boxs = new int[5, 5];
        public History_btn()
        {
            InitializeComponent();

        }
        private void On_Form_Load(object sender, EventArgs e)
        {
            StartTimer();
            Date_Time_Now = DateTime.Now.ToString();
            Reset_btn.Enabled = false;
            TB = new TextBox[,]
            {
                {this.T00, this.T01, this.T02, this.T03, this.T04 },
                { this.T10, this.T11, this.T12, this.T13, this.T14 },
                { this.T20, this.T21, this.T22, this.T23, this.T24 },
                { this.T30, this.T31, this.T32, this.T33, this.T34 },
                { this.T40, this.T41, this.T42, this.T43, this.T44 }
            };
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    TB[i, j].Text = " ";
                    TB[i, j].TextChanged += this.textBox_TextChanged;
                    TB[i, j].Leave += this.textBoX_LostFocus;
                }
            }
           
        }

        public void StartTimer()
        {
            t = new System.Timers.Timer();
            t.Interval = 1000;//1sec;
            t.Elapsed += OnTimeEvent;
        }

        private void OnTimeEvent(Object Sender, System.Timers.ElapsedEventArgs e)
        {
            string newLine = Environment.NewLine;
            Invoke(new Action(() =>
            {
                SEC += 1;
                if (SEC == 60)
                {
                    SEC = 0;
                    MIN += 1;
                }
                if (MIN == 60)
                {
                    MIN = 0;
                    HR += 1;
                }
                timer = String.Format("{0}:{1}:{2}", HR.ToString().PadLeft(2, '0'), MIN.ToString().PadLeft(2, '0'), SEC.ToString().PadLeft(2, '0'));
                Timer_TXT.Text = "Date" + newLine + Date_Time_Now + newLine + "Timer" + newLine + timer;
            }));

        }

        private void textBoX_LostFocus(object sender, EventArgs e)
        {
            LostFocus = 1;
            //if_TXT_changed(sender);
            Validate(sender);
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "")
            {
                
                TextChanged = 1;
            }

        }

        public void if_TXT_changed(object sender)
        {
            if (Filled_boxs[(int)Char.GetNumericValue(((TextBox)sender).Name[1]),
                                (int)Char.GetNumericValue(((TextBox)sender).Name[2])] != 0  
                                && ((TextBox)sender) != null )
            {
                
                DialogResult dialogResult = MessageBox.Show("Do you wanna change this", "Change Confirm", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //do something
                    Filled_boxs[(int)Char.GetNumericValue(((TextBox)sender).Name[1]), (int)Char.GetNumericValue(((TextBox)sender).Name[2])] = 0;
                    foreach (var i in occupied_boxs.ToList()) { if (i.Key == ((TextBox)sender).Name) { occupied_boxs.Remove(i); } }
                    string G = occupied_boxs[occupied_boxs.Count() - 1].Key;
                    Adjacent_boxes((int)Char.GetNumericValue(G[1]), (int)Char.GetNumericValue(G[2]));
                    ((TextBox)sender).Text = null;
                }
                else if (dialogResult == DialogResult.No)
                {
                    int P = Filled_boxs[(int)Char.GetNumericValue(((TextBox)sender).Name[1]),
                        (int)Char.GetNumericValue(((TextBox)sender).Name[2])];
                    ((TextBox)sender).Text = P.ToString();
                    //Adjacent_boxes((int)Char.GetNumericValue(((TextBox)sender).Name[1]),
                       // (int)Char.GetNumericValue(((TextBox)sender).Name[2])); 

                }

            }
            Console.WriteLine(" in the lopp");
            foreach(var i in Adjacent_box) { Console.WriteLine(" bbbbbbbbbbbb {0}", i.Name); }
        }
        private static readonly Random rand = new Random();

        public void Validate(object sender)

        {

            if (LostFocus == 1 && TextChanged == 1)
            {
                if_TXT_changed(sender);

                if (Adjacent_box.Contains(sender))
                {
                    AddValue(sender);
                }
                else
                {
                    MessageBox.Show("Cannot place the number here: check rule number 6");
                    ((TextBox)sender).Text = " ";
                }
            }
            LostFocus = 0;
            TextChanged = 0;
        }

        public void AddValue(object sender)
        {
            int a;
            
            if (int.TryParse(((TextBox)sender).Text, out a)) 
            {
                //Console.WriteLine(" value of a {0} and count is{1} ", a, occupied_boxs.Count());
                if (a == (occupied_boxs.Count() + 1) && a <= 25 && a > 1)
                {

                    Adjacent_boxes((int)Char.GetNumericValue(((TextBox)sender).Name[1]),
                        (int)Char.GetNumericValue(((TextBox)sender).Name[2]));

                    Check_adjacent_boxs(sender);

                    occupied_boxs.Add(new KeyValuePair<string, string>(((TextBox)sender).Name,
                        ((TextBox)sender).Text));

                    Filled_boxs[(int)Char.GetNumericValue(((TextBox)sender).Name[1]),
                        (int)Char.GetNumericValue(((TextBox)sender).Name[2])] = Convert.ToInt32(((TextBox)sender).Text);
                    //showHistory();
                    if (occupied_boxs.Count() == 25) { MessageBox.Show(" YOU WIN", "WIN"); }
                }
                else
                {
                    MessageBox.Show("Cannot skip a number, Can add only succesive number. Please check rule number 1", "Error");
                    ((TextBox)sender).Text = " ";
                }
            }
            else 
            { 
                if(((TextBox)sender).Text != null && ((TextBox)sender).Text != "")
                {
                    MessageBox.Show(" Invalid Input only number between 2 to 25", " Error");
                    ((TextBox)sender).Text = " ";
                }
               
            }
        }

        public void Check_adjacent_boxs(object sender)
        {
            int Adjacent_filled = 0;
            foreach (var e in Adjacent_box)
            {
                if (occupied_boxs.Any(l => l.Key == e.Name))
                {
                    Adjacent_filled = Adjacent_filled + 1;
                }

            }
            if(Adjacent_filled == Adjacent_box.Count && occupied_boxs.Count != 25)
            {
                MessageBox.Show("YOU LOSE \n" + "You can reset the Game \n" + "   OR \n" + "Revret back few steps from History to continue the game", "YOU LOOSE");
            }
        }

        private void StartGame_Btn(object sender, EventArgs e)
        {
            //welcome welcome_FRM = new welcome();
            //welcome_FRM.Show();
            // User = welcome_FRM.user;
            // Console.WriteLine("hellloooo hereee {0}", welcome_FRM.user);
            
            t.Start();
            initial_x = rand.Next(0, 5);
            initial_y = rand.Next(0, 5);
            TB[initial_x, initial_y].Text = "1";
            TB[initial_x, initial_y].ReadOnly = true;
            occupied_boxs.Add(new KeyValuePair<string, string>(TB[initial_x,initial_y].Name, TB[initial_x,initial_y].Text));
           //showHistory();
            Adjacent_boxes(initial_x, initial_y);
           
            startBTN.Enabled = false;
            Reset_btn.Enabled = true;
        }

        public void Adjacent_boxes(int x, int y)
        {
            Adjacent_box.Clear();

            if((x-1)>= 0 && (y - 1) >= 0) { Adjacent_box.Add(TB[x - 1, y-1]); }
            if ((x - 1) >= 0) { Adjacent_box.Add(TB[x - 1, y]); }
            if ((x - 1) >= 0 && (y + 1) < 5) { Adjacent_box.Add(TB[x - 1, y+1]); }

            if ((y - 1) >= 0) { Adjacent_box.Add(TB[x, y-1]); }
            if ((y + 1) < 5) { Adjacent_box.Add(TB[x, y + 1]); }


            if ((x + 1) < 5 && (y - 1) >= 0) { Adjacent_box.Add(TB[x + 1, y - 1]); }
            if ((x + 1) < 5 ) { Adjacent_box.Add(TB[x + 1, y]); }
            if ((x + 1) < 5 && (y + 1) < 5) { Adjacent_box.Add(TB[x + 1, y + 1]); }

            
        }

        private void GameRules_btn(object sender, EventArgs e)
        {
            Form2 Rules_FRM = new Form2();
            Rules_FRM.Show();
        }
        public void showHistory()
            {
            int i = occupied_boxs.Count - 1;
                History_LB.Add(new Label());
                History_BTN.Add( new Button());
                History_BTN[i].Name = i.ToString();
                History_LB[i].Name = i.ToString();

            History_BTN[i].Enabled = false;
               
                History_BTN[i].Text = "X";
                History_BTN[i].ForeColor = Color.Red;
                History_LB[i].Font = new Font(FontFamily.GenericSansSerif, 9.0F);
                History_LB[i].AutoSize = true;
                History_BTN[i].Font = new Font(FontFamily.GenericSansSerif, 10.0F, FontStyle.Bold);
               
                History_BTN[i].BackColor = Color.FromArgb(0, 255, 255, 255);
                History_BTN[i].FlatAppearance.BorderSize = 0;
                History_BTN[i].Click += this.HistoryBTN;
                History_BTN[i].AutoSize = true;
                History_BTN[i].TabStop = false;
                History_BTN[i].FlatStyle = FlatStyle.Flat;
                History_BTN[i].Width = 10;
                History_LB[i].Location = new Point(14, a);
                History_BTN[i].Location = new Point(305, b);
                History_panel.Controls.Add(History_LB[i]);
                History_panel.Controls.Add(History_BTN[i]);
                a = a + 20;
                b = b + 20;
            History_LB[i].Text = "Step - " + (i+1).ToString() + "   BOX NO :" + (occupied_boxs[i].Key).ToString() + "    Value:" + (occupied_boxs[i].Value).ToString();
          for(int j = 1; j <History_BTN.Count; j++)
            {
                if (j == (occupied_boxs.Count - 1)) { History_BTN[j].Enabled = true; }
                else { History_BTN[j].Enabled = false; 
                }            }
        }

        private void HistoryBTN(object sender, EventArgs e)
        {
            if (((Button)sender).Enabled)
            {
                int  V = (int)Char.GetNumericValue(((Button)sender).Name[0]);
                    int x = (int)Char.GetNumericValue(occupied_boxs[V].Key[1]);
                    int y = (int)Char.GetNumericValue(occupied_boxs[V].Key[2]);
                int B = 0;
                int L = 0;
                History_BTN[V].Text = "";
                History_LB[V].Text = "";
                foreach (Button l in History_BTN) { if (l.Text == ((Button)sender).Text) { B = History_BTN.IndexOf(l); }  }
                foreach (Label l in History_LB) { if (l.Name == ((Button)sender).Name) {L = History_LB.IndexOf(l); } }
                if ((B - 1) != 0) { History_BTN[B - 1].Enabled = true; }
                History_BTN.RemoveAt(B);
                History_LB.RemoveAt(L);
                occupied_boxs.RemoveAt(V);
                TB[x, y].Text = " ";
               
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            t.Stop();
            this.Close();
        }

        private void History_btn_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Stop();
        }

        private void Pause(object sender, EventArgs e)
        {
            t.Stop();
        }

        private void Reset_game(object sender, EventArgs e)
        {
            //t.Stop();
            HR = 0; MIN = 0; SEC = 0;
            t.Start();
            //StartTimer();
            int LostFocus = 0;
            int TextChanged = 0;

            foreach(var T in TB) { T.Text = null; T.ReadOnly = false; }
            Array.Clear(Filled_boxs, 0, Filled_boxs.Length);
            foreach (Button bb in History_BTN) { bb.Text = ""; }
            foreach(Label l in History_LB) { l.Text = ""; }
            History_BTN.Clear();
            History_LB.Clear();

            occupied_boxs.Clear();
            initial_x = rand.Next(0, 5);
            initial_y = rand.Next(0, 5);
            //foreach(TextBox T in TB) { T.ReadOnly = true; }
            TB[initial_x, initial_y].Text = "1";
            TB[initial_x, initial_y].ReadOnly = true;
            occupied_boxs.Add(new KeyValuePair<string, string>(TB[initial_x, initial_y].Name, TB[initial_x, initial_y].Text));
            Adjacent_boxes(initial_x, initial_y);
            a = 14;
            b = 14;
            //showHistory();
            
             

        }

        private void Solution_Btn_Click(object sender, EventArgs e)
        {
            //Adjacent_boxes(x, y);

        }
    }
}
