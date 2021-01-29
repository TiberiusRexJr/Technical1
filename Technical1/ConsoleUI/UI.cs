using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Technical1.ConsoleUI
{
    
    class UI
    {
        public void MainLoop()
        {
            UI ui = new UI();
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("1. Parse a .XML File to .RPT File");
            Console.WriteLine("2. Save the contents of a .RPT File to the Database");
            Console.WriteLine("3. Export the Contents of the Databse to a CSV File");
            Console.WriteLine("4. Quit The Program");

            Console.WriteLine(Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine("Enter a Number Now..");
            Console.ForegroundColor = ConsoleColor.White;

            string input = Console.ReadLine();
            int choice = default;

            if (!Int32.TryParse(input, out choice))
            {
                Console.WriteLine(Environment.NewLine);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Make a Valid Selection Dude");
                Console.WriteLine(Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.White;


                MainLoop();
            }
            // Use a switch statement to do the math.
            switch (choice)
            {
                case 1:
                    ui.XML_To_RPT();
                    break;
                case 2:
                    ui.XML_To_DB();
                    break;
                case 3:
                    ui.CSV_From_DB();
                    break;
                case 4:
                    Environment.Exit(1);
                    break;
                default:
                    MainLoop();
                    break;
            }
            // Wait for the user to respond before closing.

        }

        public void XML_To_RPT()
        {
            OpenFileDialog fdb = new OpenFileDialog();

            fdb.Filter = "xml files (*.xml)|*.xml";
            fdb.FilterIndex = 1;
            fdb.Multiselect = false;

            string filePath = string.Empty;
            string StatusMessage = string.Empty;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Choose a XML File to Parse");
            Console.ResetColor();

            if (fdb.ShowDialog()==DialogResult.OK)
            {
                filePath = fdb.FileName;
                Console.WriteLine(filePath);
                Console.ReadLine();

                StatusMessage = "Success";
            }

            Console.WriteLine(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(StatusMessage);
            Console.WriteLine(Environment.NewLine);
            Console.ResetColor();

            MainLoop();

        }

        public void XML_To_DB()
        {
            string outputDirectory = string.Empty;
            string StatusMessage = string.Empty;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose a Output Location";


            if (fbd.ShowDialog() == DialogResult.OK)
            {
                outputDirectory = fbd.SelectedPath;
                Console.WriteLine(outputDirectory);
                Console.ReadLine();


                StatusMessage = "Success";

            }

            Console.WriteLine(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(StatusMessage);
            Console.WriteLine(Environment.NewLine);
            Console.ResetColor();

            MainLoop();
        }

        public void CSV_From_DB()
        {
            string outputDirectory = string.Empty;
            string StatusMessage = string.Empty;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose a Output Location";


            if (fbd.ShowDialog() == DialogResult.OK)
            {
                outputDirectory = fbd.SelectedPath;
                Console.WriteLine(outputDirectory);

                Console.ReadLine();

                StatusMessage = "Success";

            }
            Console.WriteLine(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(StatusMessage);
            Console.WriteLine(Environment.NewLine);
            Console.ResetColor();


            MainLoop();
        }
    }
}
