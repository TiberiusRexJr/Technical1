using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Technical1.ConsoleUI;


namespace Technical1
{
    class Program
    {
       

        [STAThread]
        static void Main(string[] args=null)
        {
             string dbFile = "Billing.mdb";
            var _ = AppDomain.CurrentDomain.BaseDirectory;
            string newstring = _.Replace(@"\bin\Debug\", "/App_Data/" + dbFile);



            Console.WriteLine(newstring);
            Console.ReadLine();

            UI _ui = new UI();
             _ui.MainLoop();
        }
    }
}
