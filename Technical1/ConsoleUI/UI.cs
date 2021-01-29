using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technical1.Main;

namespace Technical1.ConsoleUI
{
    
    class UI
    {
        
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

        }
    }
}
