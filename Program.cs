using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TermProj
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // Application.Run(new History_btn()); //for game 1
            Application.Run(new Form3());// for starting page use this to host
            //Application.Run(new Game2()); //for game 2
            // for game 3
           //Application.Run(new Game_3());



        }
    }
}
