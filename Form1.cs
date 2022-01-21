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
        public string User { set; get; }

        List<string> History_List = new List<string>();
        TextBox[,] TB;
        System.Timers.Timer t;
        int HR, MIN, SEC;
        int LostFocus = 0;
        int TextChanged = 0;
        int pause = 0;
        int win_flag = 0;
        int D = 0;
        int initial_x;
        int initial_y;
        string timer, Date_Time_Now;
        List<TextBox> Adjacent_box = new List<TextBox>();
        List<KeyValuePair<string, string>> occupied_boxs = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> Solution_list = new List<KeyValuePair<string, string>>(25);
        int[,] Solution = new int[5, 5];

       
        public History_btn()
        {
            InitializeComponent();

        }
      
        private void On_Form_Load(object sender, EventArgs e)
        {
            User_Label.Text = User;
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
                    TB[i, j].DoubleClick += this.Remove_Pause;
                    TB[i,j].Font = new Font("Arial", 20, FontStyle.Bold);
                }
            }
            StartGame();


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
                Timer_TXT.Text = "Date " + "::  " + Date_Time_Now + "     Timer " + "::  " + timer;
            }));

        }
        private void textBoX_LostFocus(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "" && ((TextBox)sender).Text != " ")
            {

                LostFocus = 1;
                Validate(sender);

            }
            else
            {
                if_TXT_changed(sender);
            }

            //LostFocus = 1;
            //if_TXT_changed(sender);
            //Validate(sender);
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != null && (((TextBox)sender).Text != " "))
            {

                TextChanged = 1;
                //int B = Convert.ToInt32(((TextBox)sender).Text);
                


                if (int.TryParse(((TextBox)sender).Text, out int s))
                {
                    Console.WriteLine("count {0} v {1}", occupied_boxs.Count(), s);
                    if (s == 25 && occupied_boxs.Count() == 24)
                    {
                        Console.WriteLine("hheeee");
                        if (win_flag == 0)
                        {
                            win_flag = 1;
                            string Msg = "Congragulations!!! YOU WIN GAME : 1 !!" + User + Environment.NewLine +
                                  "start Time ::" + Date_Time_Now + Environment.NewLine +
                                  "Play Time::" + timer;
                            MessageBox.Show(Msg, "YOU WIN");
                            History_List.Add("Game: Game 1 you Win  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);

                            foreach (var T in TB)
                            {
                                T.ReadOnly = true;
                                T.BackColor = Color.Red;
                            }
                            t.Stop();
                        }
                    }
                }
                
            }
        }

        
        // blocking all the values

        public void Block_TB()
        {
            if (occupied_boxs.Count() > 1 && D == 0)
            {
                string lastTB = occupied_boxs[occupied_boxs.Count() - 1].Key;
                List<string> OTB = new List<string>();
                foreach (var o in occupied_boxs)
                {
                    OTB.Add(o.Key);
                }

                foreach (var T in TB)
                {
                    
                        if (OTB.Contains(T.Name))
                        {
                            if (T.Name != lastTB)
                            {
                                T.Enabled = false;
                                T.BackColor = Color.Red;
                            }
                            if (T.Name == lastTB && T.Enabled == false)
                            {
                                T.Enabled = true;
                                T.BackColor = Color.White;
                            }
                        }
                        else
                        {
                            T.Enabled = true;
                            T.BackColor = Color.White;
                        }
                    }
            }
        }

       
        public void if_TXT_changed(object sender)
        {
            for (int i = 0; i < occupied_boxs.Count(); i++)
            {
                if (occupied_boxs[i].Key == ((TextBox)sender).Name && D == 0)
                {
                    {

                        DialogResult dialogResult = MessageBox.Show("Do you wanna change this", "Change Confirm", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            occupied_boxs.RemoveAt(i);
                            int C = occupied_boxs.Count() - 1;                            
                            Adjacent_boxes((int)Char.GetNumericValue(occupied_boxs[C].Key[1]), (int)Char.GetNumericValue(occupied_boxs[C].Key[2]));
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            ((TextBox)sender).Text = occupied_boxs[i].Value;
                        }

                    }
                }
            }

            Block_TB();
        }
      
        private static readonly Random rand = new Random();

        public void Validate(object sender)

        {
            
            
                if (LostFocus == 1 && TextChanged == 1 && D == 0)
                {
                    //if_TXT_changed(sender);

                    if (Adjacent_box.Contains(sender))
                    {
                        AddValue(sender);
                        Block_TB();
                    }
                    else
                    {
                      //check_win();
                    if (win_flag == 0)
                    {
                        MessageBox.Show("Cannot place the number here !!!");
                        ((TextBox)sender).Text = " ";
                    }
                    }
                }
                LostFocus = 0;
                TextChanged = 0;
            
        }

        public void End()
        {
            if (D == 1 || D == 2)
            {
                if (D == 1)
                {
                    string Msg = "You Loose : 1 !!" + User + Environment.NewLine +
                                  "start Time ::" + Date_Time_Now + Environment.NewLine +
                                  "Play Time::" + timer;
                    MessageBox.Show(Msg, "YOU LOOSE");
                }
                if (D == 2)
                {
                 
                }

                foreach (var T in TB)
                {
                    T.Enabled = false;
                    T.BackColor = Color.Red;
                }
                t.Stop();
            }
        }


        public void AddValue(object sender)
        {
            int a;
            
            if (int.TryParse(((TextBox)sender).Text, out a)) 
            {
                if (a == (occupied_boxs.Count() + 1) && a <= 25 && a > 1)
                {

                    Adjacent_boxes((int)Char.GetNumericValue(((TextBox)sender).Name[1]),
                        (int)Char.GetNumericValue(((TextBox)sender).Name[2]));

                    Check_adjacent_boxs(sender);

                    occupied_boxs.Add(new KeyValuePair<string, string>(((TextBox)sender).Name,
                        ((TextBox)sender).Text));

                    //if (occupied_boxs.Count() == 25)
                    //{
                        //if (win_flag == 0)
                        //{
                           // win_flag = 1;
                            //string Msg = "Congragulations!!! YOU WIN GAME : 1 !!" + User + Environment.NewLine +
                                  //"start Time ::" + Date_Time_Now + Environment.NewLine +
                                  //"Play Time::" + timer;
                            //MessageBox.Show(Msg, "YOU WIN");
                           // History_List.Add("Game: Game 2 you Win  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);


                            //foreach (var T in TB)
                            //{
                               // T.ReadOnly = true;
                                //T.BackColor = Color.Red;
                            //}
                            //t.Stop();
                       // }
                   // }
                }
                else
                {
                    if (win_flag == 0)
                    {
                        MessageBox.Show("Cannot skip a number, Can add only succesive number. Please check rule number 1", "Error");
                        ((TextBox)sender).Text = " ";
                    }
                }
            }
            else 
            { 
                if(((TextBox)sender).Text != null && ((TextBox)sender).Text != " ")
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
            if(Adjacent_filled == Adjacent_box.Count && occupied_boxs.Count != 24)
            {
                string Msg = "You Loose : 1 !!" + User + Environment.NewLine +
                              "start Time ::" + Date_Time_Now + Environment.NewLine +
                              "Play Time::" + timer;
                MessageBox.Show(Msg, "YOU LOOSE");
                History_List.Add("Game: Game 1 You Loose  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);

                //Level_UP.Enabled = true;
                D = 1;
                foreach(var T in TB) { T.Enabled = false; }
               //End();
            }
        }

        private void StartGame()
        {
            t.Start();
          initial_x = rand.Next(0, 5);
            initial_y = rand.Next(0, 5);

            TB[initial_x, initial_y].Text = "1";
            TB[initial_x, initial_y].Enabled = false ;
            occupied_boxs.Add(new KeyValuePair<string, string>(TB[initial_x,initial_y].Name, TB[initial_x,initial_y].Text));
            Adjacent_boxes(initial_x, initial_y);
            //startBTN.Enabled = false;
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
            Rules_FRM.ShowDialog();
        }
        
         private void Exit(object sender, EventArgs e)
        {
            t.Stop();
            this.Close();
            Application.Exit();
        }

        private void History_btn_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Stop();
        }

        private void Pause(object sender, EventArgs e)
        {
            pause = 1;
            t.Stop();
            foreach(var T in TB)
            {
                T.Enabled = true;
                T.ReadOnly = true;
                T.BackColor = Color.Aquamarine;
            }
            MessageBox.Show("Double click on any box to resume the game !!!", "PAUSE");
        }

        private void New_game(object sender, EventArgs e)
        {
            HR = 0; MIN = 0; SEC = 0;
            t.Start();
            D = 0;
            win_flag = 0;
            int LostFocus = 0;
            int TextChanged = 0;
            foreach (var T in TB)
            {
                    T.Text = " "; T.ReadOnly = false;  T.Enabled = true; T.BackColor = Color.White;
            }
            occupied_boxs.Clear();
            initial_x = rand.Next(0, 5);
            initial_y = rand.Next(0, 5);
            TB[initial_x, initial_y].Text = "1";
            TB[initial_x, initial_y].Enabled = false;
            occupied_boxs.Add(new KeyValuePair<string, string>(TB[initial_x, initial_y].Name, TB[initial_x, initial_y].Text));
            Adjacent_boxes(initial_x, initial_y);
            History_List.Add("Game: You choose new Game : 1  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);
        }

        private void Solution_Btn_Click(object sender, EventArgs e)
        {
            //Level_UP.Enabled = true;
            if (initial_x == 0 && initial_y == 0)
            {
                Solution = new int[5, 5] { { 1,2,25,24,23 },{3,4,20,21,22},{5,6,19,18,17},{7,8,13,14,16},{9,10,11,12,15} };
            }
            if (initial_x == 0 && initial_y == 1)
            {
                Solution = new int[5, 5] { {4,1,2,9,10}, {5,3,8,11,12}, {6,7,21,20,13}, {23,22,19,15,14}, {25,24,18,17,16} };
            }
            if (initial_x == 0 && initial_y == 2)
            {
                Solution = new int[5, 5] { {4,3,1,23,25}, {5,2,22,24,19}, {8,6,21,20,18}, {9,7,12,14,17}, {10,11,13,15,16} };
            }
            if (initial_x == 0 && initial_y == 3)
            {
                Solution = new int[5, 5] { {17,16,15,1,2}, {18,20,14,3,5}, {19,21,13,6,4}, {22,23,12,10,7}, {24,25,11,9,8} };
            }
            if (initial_x == 0 && initial_y == 4)
            {
                Solution = new int[5, 5] { {24,25,11,2,1}, {23,13,12,10,3}, {22,20,14,9,4}, {21,19,15,8,5}, {18,17,16,7,6} };
            }
            if (initial_x == 1 && initial_y == 0)
            {
                Solution = new int[5, 5] { {2,3,15,16,17}, {1,4,14,25,18}, {5,12,13,24,19}, {6,9,11,23,20}, {7,8,10,22,21} };
            }
            if (initial_x == 1 && initial_y == 1)
            {
                Solution = new int[5, 5] { {3,2,13,14,15}, {4,1,12,25,16}, {6,5,11,24,17}, {7,10,23,21,18}, {8,9,22,20,19} };
            }
            if (initial_x == 1 && initial_y == 2)
            {
                Solution = new int[5, 5] { {21,20,2,4,5}, {22,19,1,3,6}, {23,18,17,7,9}, {24,16,13,8,10}, {25,15,14,12,11} };
            }
            if (initial_x == 1 && initial_y == 3)
            {
                Solution = new int[5, 5] { {19,20,21,2,3},{18,25,22,1,4},{17,24,23,6,5},{16,14,12,10,7}, {15,13,11,9,8} };
            }
            if (initial_x == 1 && initial_y == 4)
            {
                Solution = new int[5, 5] { { 25, 16, 15, 3, 2 }, { 24, 17, 14, 4, 1 }, { 23, 18, 13, 5, 6 }, { 22, 19, 12, 10, 7 }, { 21, 20, 11, 9, 8 } };
            }
            if (initial_x == 2 && initial_y == 0)
            {
                Solution = new int[5, 5] { { 4, 5, 6, 8, 9 }, { 2, 3, 17, 7, 10 }, { 1, 23, 19, 15, 12 }, { 24, 22, 19, 15, 12 }, { 25, 21, 20, 14, 13 } };
            }
            if (initial_x == 2 && initial_y == 1)
            {
                Solution = new int[5, 5] { {3,4,5,8,9}, {2,6,7,11,10 }, {25,1,18,17,12 }, {24,22,19,16,13}, {23,21,20,15,14} };
            }
            if (initial_x == 2 && initial_y == 2)
            {
                Solution = new int[5, 5] { {25,24,3,4,5}, {23,22,2,6,7}, {20,21,1,9,8}, {19,17,15,12,10}, {18,16,14,13,11} };
            }
            if (initial_x == 2 && initial_y == 3)
            {
                Solution = new int[5, 5] { {17,16,15,3,4},{19,18,14,2,5},{25,20,13,1,6},{24,21,12,10,7},{23,22,11,9,8} };
            }
            if (initial_x == 2 && initial_y == 4)
            {
                Solution = new int[5, 5] { {17,16,15,4,3}, {19,18,14,5,2}, {15,13,21,6,1}, {23,22,12,10,7}, {24,25,11,9,8} };
            }
            if (initial_x == 3 && initial_y == 0)
            {
                Solution = new int[5, 5] { {4,5,7,8,11}, {3,6,9,10,12}, {2,22,21,13,14}, {1,23,20,18,15}, {25,24,19,17,16} };
            }
            if (initial_x == 3 && initial_y == 1)
            {
                Solution = new int[5, 5] { { 7, 8, 10, 12, 13 }, { 6, 5, 9, 11, 14 }, { 4, 3, 21, 20, 15 }, { 2, 1, 22, 19, 16 }, { 25, 24, 23, 18, 17 } };
            }
            if (initial_x == 3 && initial_y == 2)
            {
                Solution = new int[5, 5] { {21,20,4,5,6 }, { 22,19,3,7,8}, {23,18,2,9,10 }, {24,17,1,11,12 }, {25,16,15,14,13} };
            }
            if (initial_x == 3 && initial_y == 3)
            {
                Solution = new int[5, 5] { {21,19,18,8,7}, {22,20,17,9,6}, {23,16,11,10,5}, {24,15,12,1,4}, {25,14,13,2,3} };
            }
            if (initial_x == 3 && initial_y == 4)
            {
                Solution = new int[5, 5] { {15,16,24,23,22},{12,14,17,25,21},{11,13,18,19,20},{10,8,5,3,1},{9,7,6,4,2} };
            }
            if (initial_x == 4 && initial_y == 0)
            {
                Solution = new int[5, 5] { {21,20,19,14,13},{22,23,18,15,12},{25,24,17,16,11},{2,4,5,7,10},{1,3,6,8,9} };
            }
            if (initial_x == 4 && initial_y == 1)
            {
                Solution = new int[5, 5] {{9,10,11,12,13},{7,8,25,16,14},{5,6,24,17,15},{3,4,23,18,19},{2,1,22,21,20} };
            }
            if (initial_x == 4 && initial_y == 2)
            {
                Solution = new int[5, 5] {{9,10,12,13,25},{8,11,14,15,24},{7,6,16,17,23},{5,3,18,19,22},{4,2,1,20,21} };
            }
            if (initial_x == 4 && initial_y == 3)
            {
                Solution = new int[5, 5] { {18,16,15,7,6}, {19,17,14,8,5}, {21,20,13,9,4}, {25,22,12,10,3}, {24,23,11,1,2} };
            }
            if (initial_x == 4 && initial_y == 4)
            {
                Solution = new int[5, 5] {{25,23,22,15,14},{24,21,16,13,7},{20,17,12,8,6},{19,11,9,5,2},{18,10,4,3,1} };
            }
            Display_Solution();

        }

        public void Display_Solution()
        {
            t.Stop();
            for(int i = 0; i<5; i++)
            {
                for(int j = 0; j< 5; j++)
                {
                    TB[i, j].Text = Solution[i, j].ToString();
                    TB[i, j].Enabled = false;
                    TB[i, j].BackColor = Color.LightSkyBlue;
                 }
            }
        }
      


        private void Reset(object sender, EventArgs e)
        {
            foreach(var T in TB)
            {
                if(T.Text != "1")
                {
                    T.Text = "";
                    T.Enabled = true;
                }
            }
        }

        private void Level_UP_Click(object sender, EventArgs e)
        {
            this.Hide();
            Game2 Game_2 = new Game2();
            Game_2.User = User;
            Game_2.ShowDialog();
        }

        private void Home_Click(object sender, EventArgs e)
        {
            t.Stop();
            ////this.Close();
            //Application.Exit();
            this.Hide();
            Form4 Home = new Form4();
            Home.User = User;
            Home.History_list = History_List;
          
            Home.ShowDialog();
        }

        private void New_Game_1_Click(object sender, EventArgs e)
        {

            HR = 0; MIN = 0; SEC = 0;
            t.Start();
            D = 0;
            win_flag = 0;
            int LostFocus = 0;
            int TextChanged = 0;
            foreach (var T in TB)
            {
                if (T.Text != 1.ToString())
                {
                    T.Text = " "; T.Enabled = true; T.BackColor = Color.White;
                }
            }
            occupied_boxs.Clear();
            //initial_x = rand.Next(0, 5);
            //initial_y = rand.Next(0, 5);
            TB[initial_x, initial_y].Text = "1";
            TB[initial_x, initial_y].Enabled = false;
            occupied_boxs.Add(new KeyValuePair<string, string>(TB[initial_x, initial_y].Name, TB[initial_x, initial_y].Text));
            Adjacent_boxes(initial_x, initial_y);
            History_List.Add("Game: Restarted Game 1  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);
        }

        private void History_Click(object sender, EventArgs e)
        {
            t.Stop();
            History History_Page = new History();
            History_Page.User = User;
            History_Page.History_list = History_List;
            History_Page.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 F3 = new Form3();
            //F3.From_Game = 1;
            F3.ShowDialog();
            

        }

        private void Remove_Pause(object sender, EventArgs e)
        {
            if (pause == 1)
            {
                t.Start();
                foreach (var T in TB)
                {
                    if (T.Text != "1")
                    {
                        T.ReadOnly = false;
                    }
                    T.BackColor = Color.White;
                }
                pause = 0;
                Block_TB();
            }
        }

        public int Adjacent_empty_box_count(int x, int y)
        {
            int C = 0;
            int i = -1;
            int j = -1;
            Adjacent_boxes(x, y);
            foreach (var E in Adjacent_box)
            {
                 i = (int)Char.GetNumericValue(E.Name[1]);
                 j = (int)Char.GetNumericValue(E.Name[2]);
                if(Solution[i, j] < 1)
                {
                    C = C + 1;
                }
            }
            return C;
        }

    }
}
