using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technical1.ConsoleUI;
using Technical1.ConsoleUI;

namespace Technical1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args=null)
        {
            UI ui = new UI(); ;
            ui.MainLoop();
        }
    }
}
