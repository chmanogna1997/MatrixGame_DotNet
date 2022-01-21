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
    public partial class Game2 : Form
    {
        System.Timers.Timer t;
        int HR, MIN, SEC;
        string timer, Date_Time_Now;
        int win_Flag = 0;
        int Fail_Flag = 0;
        public string User { set; get; }
        List<string> History_List = new List<string>();

        int pause = 0;

        public Form Parent { set; get; }
        TextBox[,] TB;
        List<int> Rand_25 = new List<int>();
        int TextChanged = 0;
        int LostFocus = 0;
        List<string> Number_List = new List<string>();
        List<int> Game_2_Solution = new List<int>();
        int initial_x;
        int initial_y;
        int[,] Solution = new int[5, 5];
        List<KeyValuePair<string, string>> occupied_boxs = new List<KeyValuePair<string, string>>();




        public Game2()
        {
            InitializeComponent();
        }

        private void Game2_Load(object sender, EventArgs e)
        {
            User_Label.Text = User;
            StartTimer();
            Date_Time_Now = DateTime.Now.ToString();
            t.Start();

            TB = new TextBox[,]
            {
                {this.D0,this.U00,this.U01,this.U02,this.U03,this.U04,this.D2 },
                {this.S00,this.T00, this.T01, this.T02, this.T03, this.T04, this.S01 },
                {this.S10, this.T10, this.T11, this.T12, this.T13, this.T14, this.S11 },
                {this.S20, this.T20, this.T21, this.T22, this.T23, this.T24,this.S21 },
                {this.S30, this.T30, this.T31, this.T32, this.T33, this.T34,this.S31 },
                {this.S40, this.T40, this.T41, this.T42, this.T43, this.T44,this.S41 },
                {this.D1,this.U10,this.U11,this.U12,this.U13,this.U14,this.D3 },

            };
            foreach (var T in TB)
            {
                T.TextAlign = HorizontalAlignment.Center;
                T.Font = new Font("Arial", 20, FontStyle.Bold);
                string Name = T.Name;
                if (Name[0] != 'T')
                {

                    T.TextChanged += this.textBox_TextChanged;
                    T.Leave += this.textBoX_LostFocus;
                    T.DoubleClick += this.Remove_Pause;

                }
                else
                {
                    T.BackColor = Color.Pink;

                }

            }
            TwentyFive_Rand();
        }

        // start timer
        public void StartTimer()
        {
            t = new System.Timers.Timer();
            t.Interval = 1000;//1sec;
            t.Elapsed += OnTimeEvent;
        }
        // on time event 
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
        // If text box lost focus
        
        private void textBoX_LostFocus(object sender, EventArgs e)
        {

            if (((TextBox)sender).Text != null && ((TextBox)sender).Text != "")
            {
                LostFocus = 1;
                Console.WriteLine(" fail flag is {0}", Fail_Flag);
                if(Fail_Flag == 0 && win_Flag == 0)
                {
                    Validate(sender);
                }
            }
            else
            {
                removing(sender);
            }


                }
        // IF text changed
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text != null && ((TextBox)sender).Text != " " && ((TextBox)sender).Text != "")
            {
                TextChanged = 1;
                int I = Convert.ToInt32(((TextBox)sender).Text);
                Console.WriteLine(" input value is {0}", I);
                if(I == 25)
                {
                    win_check();
                }
            }
        }
        // blocking all the values
        public void Block_TB()
        {
            if (occupied_boxs.Count() > 1 )
            {
                string lastTB = occupied_boxs[occupied_boxs.Count() - 1].Key;
                List<string> OTB = new List<string>();
                foreach (var o in occupied_boxs)
                {
                    OTB.Add(o.Key);
                }
                foreach (var T in TB)
                {
                    if (T.Name[0] != 'T')
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
        }
        // removing the values :

        public void removing(object sender)
        {
            for (int i = 0; i < occupied_boxs.Count(); i++)
            {
                if (occupied_boxs[i].Key == ((TextBox)sender).Name)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you wanna change this", "Change Confirm", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        occupied_boxs.RemoveAt(i);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        ((TextBox)sender).Text = occupied_boxs[i].Value;
                    }
                }
            }
            Block_TB();

        }


        // Validating the input

        public void Validate(object sender)
        {
            if (LostFocus == 1 && TextChanged == 1)
            {

                Console.WriteLine(" in validate func");
                int a;
                if (int.TryParse(((TextBox)sender).Text, out a))
                {
                    Console.WriteLine("next number is {0}", a + 1);
                    Console.WriteLine(" count in occupied count {0}", occupied_boxs.Count());

                    int C = 2 + occupied_boxs.Count();
                    Console.WriteLine(" while validation {0}, {1}",a, C);
                    if (a == C)
                    {
                        Num_List_Func(sender);
                        foreach (string s in Number_List)
                        {
                            Console.WriteLine(" NN {0}", s);
                        }


                            string input = ((TextBox)sender).Text;
                            if (Number_List.Contains(input))
                            {
                                Console.WriteLine(" in if SS");
                                win_check();
                                fail_check(a + 1);
                                occupied_boxs.Add(new KeyValuePair<string, string>(((TextBox)sender).Name,
                                ((TextBox)sender).Text));
                                Console.WriteLine(" occupied_boxs count {0}", occupied_boxs.Count());

                            }
                            else
                            {
                                MessageBox.Show("Cannot place the number here per rule : ", "ERROR");
                                ((TextBox)sender).Text = "";
                            }
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Please enter number between 2 to 25 and successivley from number 2", "ERROR");
                        ((TextBox)sender).Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Invalid inputs", "ERROR");
                    ((TextBox)sender).Text = "";
                }
            }
            LostFocus = 0;
            TextChanged = 0;
            Block_TB();
        }
        // win check
        public void win_check()
        {
            Console.WriteLine(" in win check, {0} ", occupied_boxs.Count());
            if (win_Flag == 0)
            {
                if (occupied_boxs.Count() == 23)
                {
                    win_Flag = 1;
                    string Msg = "You Win Game : 2 !!" + User + Environment.NewLine +
                                  "start Time ::" + Date_Time_Now + Environment.NewLine +
                                  "Play Time::" + timer;
                    History_List.Add("Game: Game 2 you Win  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);

                    MessageBox.Show(Msg, "WIN");
                    // Level_UP.Enabled = true;
                    foreach (var T in TB)
                    {
                        T.Enabled = false;
                        T.BackColor = Color.Red;
                    }
                    t.Stop();
                }
            }
        }
        // check if user is failed
        public void fail_check(int next_num)
        {
                string Box_Name = "";
                foreach (var T in TB)
                {
                    string Name = T.Name;
                    if (Name[0] == 'T')
                    {
                        if (Convert.ToInt32(T.Text) == next_num)
                        {
                            Box_Name = T.Name;
                        }
                    }
                }
                Console.WriteLine("number is in box {0} ", Box_Name);
                if (Valid_adjacent_boxs(Box_Name) == 0)
                {
                    //Level_UP.Enabled = true;
                    foreach (var T in TB)
                    {
                        T.Enabled = false;
                        T.BackColor = Color.Red;
                    }
                    t.Stop();
                    string Msg = "You Loose Game : 2 !!" + User + Environment.NewLine +
                                  "start Time ::" + Date_Time_Now + Environment.NewLine +
                                  "Play Time::" + timer;
                    MessageBox.Show(Msg, " LOOSE");
                    History_List.Add("Game: Game 2 You Loose  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);
                Fail_Flag = 1;
            }

            }
        // getiing adjacent boxes where num can be placed

        public int Valid_adjacent_boxs(string Box_Name)
        {
            int Empty = 0;
            List<string> Adjacent_box = new List<string>();
            Adjacent_box.Clear();
            List<string> D0D3 = new List<string>() { "T00", "T11", "T22", "T33", "T44" };
            List<string> D1D2 = new List<string>() { "T04", "T13", "T22", "T31", "T40" };
            if (D0D3.Contains(Box_Name))
            {
                Adjacent_box.Add("D0");
                Adjacent_box.Add("D3");
            }
            if (D1D2.Contains(Box_Name))
            {
                Adjacent_box.Add("D1");
                Adjacent_box.Add("D2");
            }
            int X = (int)Char.GetNumericValue(Box_Name[1]);
            int Y = (int)Char.GetNumericValue(Box_Name[2]);
            string S1 = "S" + X.ToString() + "0"; Adjacent_box.Add(S1);
            string S2 = "S" + X.ToString() + "1"; Adjacent_box.Add(S2);
            string U1 = "U" + "0" + Y.ToString(); Adjacent_box.Add(U1);
            string U2 = "U" + "1" + Y.ToString(); Adjacent_box.Add(U2);

            foreach (string e in Adjacent_box) { Console.WriteLine("{0}", e); }


            // now checking if the boxes are filled or not 

            foreach (TextBox T in TB)
            {
                if (T.Name[0] != 'T')
                {
                    if (Adjacent_box.Contains(T.Name))
                    {
                        Console.WriteLine(" chcking if box {0} is empty", T.Name);
                        if (T.Text == "" || T.Text == null)
                        {
                            Empty = Empty + 1;
                        }
                    }
                }
            }

            return Empty;
        }

        // List for each box

        public void Num_List_Func(object sender)
        {
            Number_List.Clear();
            String Name = ((TextBox)sender).Name;
            if (Name[0] == 'S')
            {

                for (int y = 0; y < 5; y++)
                {
                    foreach (var T in TB)
                    {

                        if (T.Name[0] == 'T' && T.Name == 'T' + Name[1].ToString() + y.ToString())
                        {
                            Number_List.Add(T.Text);

                        }
                    }
                }
            }
            else if (Name[0] == 'U')
            {
                for (int x = 0; x < 5; x++)
                {
                    foreach (var T in TB)
                    {
                        if (T.Name[0] == 'T' && T.Name == 'T' + x.ToString() + Name[2].ToString())
                        {
                            Console.WriteLine(" bb {0}", T);
                            Number_List.Add(T.Text);
                        }
                    }
                }

            }
            else
            {
                if (Name == "D0" || Name == "D3")
                {
                    Number_List.Add(T00.Text);
                    Number_List.Add(T11.Text);
                    Number_List.Add(T22.Text);
                    Number_List.Add(T33.Text);
                    Number_List.Add(T44.Text);
                }
                if (Name == "D1" || Name == "D2")
                {
                    Number_List.Add(T04.Text);
                    Number_List.Add(T13.Text);
                    Number_List.Add(T22.Text);
                    Number_List.Add(T31.Text);
                    Number_List.Add(T40.Text);
                }
            }
        }


        // Generate random Numbers

        private static readonly Random rand = new Random();

        // generating 25 random numbers
        public void TwentyFive_Rand()
        {
            initial_x = 0;
                initial_y = 0;
            //initial_x = rand.Next(0, 5);
            //initial_y = rand.Next(0, 5);
            Center_Array();

        }

        private void Center_Array()
        {
            if (initial_x == 0 && initial_y == 0)
            {
                Solution = new int[5, 5] { { 1, 2, 25, 24, 23 }, { 3, 4, 20, 21, 22 }, { 5, 6, 19, 18, 17 }, { 7, 8, 13, 14, 16 }, { 9, 10, 11, 12, 15 } };
            }
            if (initial_x == 0 && initial_y == 1)
            {
                Solution = new int[5, 5] { { 4, 1, 2, 9, 10 }, { 5, 3, 8, 11, 12 }, { 6, 7, 21, 20, 13 }, { 23, 22, 19, 15, 14 }, { 25, 24, 18, 17, 16 } };
            }
            if (initial_x == 0 && initial_y == 2)
            {
                Solution = new int[5, 5] { { 4, 3, 1, 23, 25 }, { 5, 2, 22, 24, 19 }, { 8, 6, 21, 20, 18 }, { 9, 7, 12, 14, 17 }, { 10, 11, 13, 15, 16 } };
            }
            if (initial_x == 0 && initial_y == 3)
            {
                Solution = new int[5, 5] { { 17, 16, 15, 1, 2 }, { 18, 20, 14, 3, 5 }, { 19, 21, 13, 6, 4 }, { 22, 23, 12, 10, 7 }, { 24, 25, 11, 9, 8 } };
            }
            if (initial_x == 0 && initial_y == 4)
            {
                Solution = new int[5, 5] { { 24, 25, 11, 2, 1 }, { 23, 13, 12, 10, 3 }, { 22, 20, 14, 9, 4 }, { 21, 19, 15, 8, 5 }, { 18, 17, 16, 7, 6 } };
            }
            if (initial_x == 1 && initial_y == 0)
            {
                Solution = new int[5, 5] { { 2, 3, 15, 16, 17 }, { 1, 4, 14, 25, 18 }, { 5, 12, 13, 24, 19 }, { 6, 9, 11, 23, 20 }, { 7, 8, 10, 22, 21 } };
            }
            if (initial_x == 1 && initial_y == 1)
            {
                Solution = new int[5, 5] { { 3, 2, 13, 14, 15 }, { 4, 1, 12, 25, 16 }, { 6, 5, 11, 24, 17 }, { 7, 10, 23, 21, 18 }, { 8, 9, 22, 20, 19 } };
            }
            if (initial_x == 1 && initial_y == 2)
            {
                Solution = new int[5, 5] { { 21, 20, 2, 4, 5 }, { 22, 19, 1, 3, 6 }, { 23, 18, 17, 7, 9 }, { 24, 16, 13, 8, 10 }, { 25, 15, 14, 12, 11 } };
            }
            if (initial_x == 1 && initial_y == 3)
            {
                Solution = new int[5, 5] { { 19, 20, 21, 2, 3 }, { 18, 25, 22, 1, 4 }, { 17, 24, 23, 6, 5 }, { 16, 14, 12, 10, 7 }, { 15, 13, 11, 9, 8 } };
            }
            if (initial_x == 1 && initial_y == 4)
            {
                Solution = new int[5, 5] { { 25, 16, 15, 3, 2 }, { 24, 17, 14, 4, 1 }, { 23, 18, 13, 5, 6 }, { 22, 19, 12, 10, 7 }, { 21, 20, 11, 9, 8 } };
            }
            if (initial_x == 2 && initial_y == 0)
            {
                Solution = new int[5, 5] { { 4, 5, 6, 8, 9 }, { 2,3,17,7,10 }, {1,23,19,15,12 }, {24,22,19,15,12 }, {25,21,20,14,13} };
            }
            if (initial_x == 2 && initial_y == 1)
            {
                Solution = new int[5, 5] { { 3, 4, 5, 8, 9 }, { 2, 6, 7, 11, 10 }, { 25, 1, 18, 17, 12 }, { 24, 22, 19, 16, 13 }, { 23, 21, 20, 15, 14 } };
            }
            if (initial_x == 2 && initial_y == 2)
            {
                Solution = new int[5, 5] { { 25, 24, 3, 4, 5 }, { 23, 22, 2, 6, 7 }, { 20, 21, 1, 9, 8 }, { 19, 17, 15, 12, 10 }, { 18, 16, 14, 13, 11 } };
            }
            if (initial_x == 2 && initial_y == 3)
            {
                Solution = new int[5, 5] { { 17, 16, 15, 3, 4 }, { 19, 18, 14, 2, 5 }, { 25, 20, 13, 1, 6 }, { 24, 21, 12, 10, 7 }, { 23, 22, 11, 9, 8 } };
            }
            if (initial_x == 2 && initial_y == 4)
            {
                Solution = new int[5, 5] { { 17, 16, 15, 4, 3 }, { 19, 18, 14, 5, 2 }, { 15, 13, 21, 6, 1 }, { 23, 22, 12, 10, 7 }, { 24, 25, 11, 9, 8 } };
            }
            if (initial_x == 3 && initial_y == 0)
            {
                Solution = new int[5, 5] { { 4, 5, 7, 8, 11 }, { 3, 6, 9, 10, 12 }, { 2, 22, 21, 13, 14 }, { 1, 23, 20, 18, 15 }, { 25, 24, 19, 17, 16 } };
            }
            if (initial_x == 3 && initial_y == 1)
            {
                Solution = new int[5, 5] { { 7, 8, 10, 12, 13 }, { 6, 5, 9, 11, 14 }, { 4, 3, 21, 20, 15 }, { 2, 1, 22, 19, 16 }, { 25, 24, 23, 18, 17 } };
            }
            if (initial_x == 3 && initial_y == 2)
            {
                Solution = new int[5, 5] { { 21, 20, 4, 5, 6 }, { 22, 19, 3, 7, 8 }, { 23, 18, 2, 9, 10 }, { 24, 17, 1, 11, 12 }, { 25, 16, 15, 14, 13 } };
            }
            if (initial_x == 3 && initial_y == 3)
            {
                Solution = new int[5, 5] { { 21, 19, 18, 8, 7 }, { 22, 20, 17, 9, 6 }, { 23, 16, 11, 10, 5 }, { 24, 5, 12, 1, 4 }, { 25, 14, 13, 2, 3 } };
            }
            if (initial_x == 3 && initial_y == 4)
            {
                Solution = new int[5, 5] { { 15, 16, 24, 23, 22 }, { 12, 14, 17, 25, 21 }, { 11, 13, 18, 19, 20 }, { 10, 8, 5, 3, 1 }, { 9, 7, 6, 4, 2 } };
            }
            if (initial_x == 4 && initial_y == 0)
            {
                Solution = new int[5, 5] { { 21, 20, 19, 14, 13 }, { 22, 23, 18, 15, 12 }, { 25, 24, 17, 16, 11 }, { 2, 4, 5, 7, 10 }, { 1, 3, 6, 8, 9 } };
            }
            if (initial_x == 4 && initial_y == 1)
            {
                Solution = new int[5, 5] { { 9, 10, 11, 12, 13 }, { 7, 8, 25, 16, 14 }, { 5, 6, 24, 17, 15 }, { 3, 4, 23, 18, 19 }, { 2, 1, 22, 21, 20 } };
            }
            if (initial_x == 4 && initial_y == 2)
            {
                Solution = new int[5, 5] { { 9, 10, 12, 13, 25 }, { 8, 11, 14, 15, 24 }, { 7, 6, 16, 17, 23 }, { 5, 3, 18, 19, 22 }, { 4, 2, 1, 20, 21 } };
            }
            if (initial_x == 4 && initial_y == 3)
            {
                Solution = new int[5, 5] { { 18, 16, 15, 7, 6 }, { 19, 17, 14, 8, 5 }, { 21, 20, 13, 9, 4 }, { 25, 22, 12, 10, 3 }, { 24, 23, 11, 1, 2 } };
            }
            if (initial_x == 4 && initial_y == 4)
            {
                Solution = new int[5, 5] { { 25, 23, 22, 15, 14 }, { 24, 21, 16, 13, 7 }, { 20, 17, 12, 8, 6 }, { 19, 11, 9, 5, 2 }, { 18, 10, 4, 3, 1 } };
            }
            Assign_To_25_Boxes();

        }

        // assigning it to the 25 text boxes:

        public void Assign_To_25_Boxes()
        {
            String B_Name = "";
            int k = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    B_Name = "T" + i.ToString() + j.ToString();
                    foreach (var T in TB)
                    {
                        if (T.Name == B_Name) { T.Text = Solution[i, j].ToString(); T.ReadOnly = true; }
                    }
                }
            }

        }
        private void Remove_Pause(object sender, EventArgs e)
        {
            if (pause == 1)
            {
                t.Start();
                foreach (var T in TB)
                {
                    if (T.Name[0] != 'T')
                    {
                        T.ReadOnly = false;
                        T.BackColor = Color.White;
                    }

                }
                pause = 0;
                Block_TB();
            }
        }
        private void Pause(object sender, EventArgs e)
        {
            t.Stop();
            pause = 1;
            foreach (var T in TB)
            {
                if (T.Name[0] != 'T')
                {
                    T.ReadOnly = true;
                    T.BackColor = Color.Aquamarine;
                }
            }
            MessageBox.Show("Double click on any outer textboxs to resume the game !!!", "PAUSE");
        }

        private void Solution_BTN_Click(object sender, EventArgs e)
        {
            t.Stop();
            if (initial_x == 0 && initial_y == 0)
            {
                Game_2_Solution = new List<int>() { 4, 7, 2, 25, 18, 16, 19, 24, 23, 20, 21, 17, 5, 13, 8, 10, 12, 9, 3, 6, 11, 14, 22, 15 };
            }
            if (initial_x == 0 && initial_y == 1)
            {
                Game_2_Solution = new List<int>() { 3, 23, 24, 2, 15, 14, 25, 4, 10, 5, 12, 20, 13, 19, 22, 18, 17, 11, 6, 7, 8, 9, 16, 21 };
            }
            if (initial_x == 0 && initial_y == 2)
            {
                Game_2_Solution = new List<int>() { 2, 10, 3, 22, 20, 18, 24, 4, 23, 5, 19, 6, 8, 7, 17, 13, 15, 25, 9, 11, 12, 14, 16, 21 };
            }
            if (initial_x == 0 && initial_y == 3)
            {
                Game_2_Solution = new List<int>() { 20, 18, 16, 14, 10, 4, 24, 17, 15, 5, 3, 6, 21, 22, 7, 25, 11, 13, 19, 23, 12, 9, 2, 8 };
            }
            if (initial_x == 0 && initial_y == 4)
            {
                Game_2_Solution = new List<int>() { 8, 23, 20, 16, 2, 3, 10, 25, 11, 13, 12, 22, 9, 19, 5, 18, 6, 14, 21, 17, 15, 7, 4, 24 };
            }
            if (initial_x == 1 && initial_y == 0)
            {
                Game_2_Solution = new List<int>() { 4, 2, 3, 11, 24, 17, 13, 15, 16, 14, 18, 9, 20, 7, 21, 25, 6, 8, 10, 22, 19, 23 };
            }
            if (initial_x == 1 && initial_y == 1)
            {
                Game_2_Solution = new List<int>() { 21, 3, 2, 23, 24, 15, 25, 13, 14, 4, 12, 5, 17, 7, 18, 9, 19, 8, 6, 10, 22, 20, 16, 11 };
            }
            if (initial_x == 1 && initial_y == 2)
            {
                Game_2_Solution = new List<int>() { 17, 23, 19, 24, 5, 3, 21, 20, 22, 6, 18, 7, 16, 10, 14, 11, 25, 24, 15, 13, 12, 9, 8 };
            }
            if (initial_x == 1 && initial_y == 3)
            {
                Game_2_Solution = new List<int>() { 25, 18, 24, 21, 6, 3, 14, 20, 2, 22, 4, 17, 5, 12, 7, 15, 9, 23, 16, 13, 11, 10, 8, 19 };
            }
            if (initial_x == 1 && initial_y == 4)
            {
                Game_2_Solution = new List<int>() { 25, 23, 16, 14, 4, 2, 13, 15, 3, 17, 24, 18, 5, 12, 7, 21, 9, 19, 22, 20, 11, 10, 6, 8 };
            }
            if (initial_x == 2 && initial_y == 0)
            {
                Game_2_Solution = new List<int>() { 3, 4, 22, 6, 8, 11, 7, 5, 9, 2, 10, 18, 23, 15, 19, 20, 14, 25, 24, 21, 17, 16, 12, 13 };
            }

            if (initial_x == 2 && initial_y == 1)
            {
                Game_2_Solution = new List<int>() { 6, 24, 4, 5, 8, 10, 18, 3, 9, 2, 7, 17, 25, 16, 13, 20, 15, 22, 23, 21, 19, 11, 12, 14 };
            }
            if (initial_x == 2 && initial_y == 2)
            {
                Game_2_Solution = new List<int>() { 22, 25, 21, 2, 4, 5, 6, 24, 3, 23, 7, 20, 8, 19, 10, 16, 13, 17, 18, 15, 14, 9, 11, 12 };
            }
            if (initial_x == 2 && initial_y == 3)
            {
                Game_2_Solution = new List<int>() { 17, 19, 16, 12, 9, 4, 2, 15, 3, 14, 5, 20, 6, 24, 7, 23, 22, 21, 25, 18, 11, 10, 8, 13 };
            }
            if (initial_x == 2 && initial_y == 4)
            {
                Game_2_Solution = new List<int>() { 17, 21, 16, 12, 4, 2, 13, 15, 3, 14, 5, 20, 6, 23, 7, 24, 9, 22, 19, 25, 11, 10, 8, 18 };
            }
            if (initial_x == 3 && initial_y == 0)
            {
                Game_2_Solution = new List<int>() { 21, 3, 5, 7, 8, 12, 10, 4, 11, 6, 9, 22, 13, 20, 15, 25, 16, 23, 2, 24, 19, 17, 14, 18 };
            }
            if (initial_x == 3 && initial_y == 1)
            {
                Game_2_Solution = new List<int>() { 5, 25, 8, 9, 11, 16, 13, 7, 12, 6, 14, 3, 15, 2, 22, 23, 18, 21, 4, 24, 10, 20, 17, 19 };
            }
            if (initial_x == 3 && initial_y == 2)
            {
                Game_2_Solution = new List<int>() { 21, 22, 18, 4, 5, 12, 25, 20, 6, 19, 3, 10, 9, 11, 24, 14, 13, 17, 23, 16, 15, 7, 8, 2 };
            }
            if (initial_x == 3 && initial_y == 3)
            {
                Game_2_Solution = new List<int>() { 21, 23, 16, 17, 9, 3, 11, 18, 7, 22, 6, 10, 5, 24, 12, 14, 2, 15, 25, 19, 13, 8, 4, 20 };
            }
            if (initial_x == 3 && initial_y == 4)
            {
                Game_2_Solution = new List<int>() { 3, 15, 13, 17, 19, 20, 25, 16, 22, 12, 14, 11, 18, 10, 5, 6, 4, 8, 9, 7, 24, 23, 21, 2 };
            }
            if (initial_x == 4 && initial_y == 0)
            {
                Game_2_Solution = new List<int>() { 23, 22, 20, 8, 14, 13, 17, 21, 19, 15, 12, 25, 24, 7, 10, 8, 6, 4, 2, 3, 5, 16, 11, 9 };
            }
            if (initial_x == 4 && initial_y == 1)
            {
                Game_2_Solution = new List<int>() { 24, 9, 8, 25, 12, 13, 16, 10, 11, 7, 14, 5, 15, 3, 18, 22, 21, 4, 2, 6, 23, 17, 19, 20 };
            }
            if (initial_x == 4 && initial_y == 2)
            {
                Game_2_Solution = new List<int>() { 11, 9, 6, 16, 19, 25, 15, 12, 13, 8, 14, 7, 23, 3, 24, 2, 20, 4, 5, 10, 18, 17, 22, 21 };
            }
            if (initial_x == 4 && initial_y == 3)
            {
                Game_2_Solution = new List<int>() { 18, 21, 22, 12, 7, 6, 13, 15, 16, 14, 5, 20, 9, 25, 3, 23, 2, 24, 19, 17, 11, 8, 4, 10 };
            }
            if (initial_x == 4 && initial_y == 4)
            {
                Game_2_Solution = new List<int>() { 12, 25, 17, 9, 8, 14, 13, 23, 15, 16, 7, 20, 6, 19, 5, 18, 4, 11, 24, 10, 22, 3, 2, 21 };
            }
            int i = 0;
            foreach (var T in TB)
            {
                if (T.Name[0] != 'T')
                {
                    T.Text = Game_2_Solution[i].ToString();
                    T.Enabled = false;
                    T.BackColor = Color.Red;
                    i = i + 1;
                }
            }
        }

        private void Next_Click(object sender, EventArgs e)
        {
            this.Hide();
            Game_3 Game_3 = new Game_3();
            Game_3.User = User;
            Game_3.ShowDialog();
        }

        private void History_Click(object sender, EventArgs e)
        {
            t.Stop();
            History History_Page = new History();
            History_Page.User = User;
            History_Page.History_list = History_List;
            History_Page.ShowDialog();
        }

        private void Home_Click(object sender, EventArgs e)
        {
            t.Stop();
            ////this.Close();
            //Application.Exit();
            this.Hide();
            Form4 Home = new Form4();
            Home.User = User;
            Home.ShowDialog();
        }

        private void New_Game_1_Click(object sender, EventArgs e)
        {
            HR = 0; MIN = 0; SEC = 0;
            Console.WriteLine(" the timer value is ", t);
            t.Start();
            TextChanged = 0;
            LostFocus = 0;
            win_Flag = 0;
            Fail_Flag = 0;
            Number_List.Clear();
            occupied_boxs.Clear();
            foreach (var T in TB)
            {
                T.Text = "";
                T.ReadOnly = false;
                T.Enabled = true;
                if(T.Name[0] != 'T')
                {
                    T.BackColor = Color.White;
                }
            }
            TwentyFive_Rand();
            History_List.Add("Game: You choose new Game : 2  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);
        }

        private void Game2_Exit(object sender, EventArgs e)
        {
            t.Stop();
            this.Close();
            Application.Exit();
        }

        private void Initial(object sender, EventArgs e)
        {
            HR = 0; MIN = 0; SEC = 0;
            Console.WriteLine(" the timer value is ", t);
            t.Start();
            TextChanged = 0;
            LostFocus = 0;
            win_Flag = 0;
            Fail_Flag = 0;
            //Number_List.Clear();
            occupied_boxs.Clear();
            foreach (var T in TB)
            {
                string TB_name = T.Name;
                if (TB_name[0] != 'T')
                {
                    T.Text = "";
                    T.ReadOnly = false;
                    T.Enabled = true;
                    T.BackColor = Color.White;
                }
            }
            History_List.Add("Game: Restarted Game 2  " + User + " Time ::" + Date_Time_Now + "Total_Play_Time::" + timer);

        }

        private void Game2_Rules(object sender, EventArgs e)
        {
            Rules_Game2 Rule_Form = new Rules_Game2();
            Rule_Form.ShowDialog();

        }
    }
}
