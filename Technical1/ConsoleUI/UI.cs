using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technical1.FileOps;
using Technical1.Model;
namespace Technical1.ConsoleUI
{

    class UI
    {
        private Parsing _parse = new Parsing();
        private Io _io = new Io();
        private BillHeader _bh = new BillHeader();
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

        #region Main Functionality
        public void XML_To_RPT()
        {


            string filePath = string.Empty;
            string outputDir = string.Empty;

            string StatusMessage = "Failure";

            string _ = "Choose a XML file to parse";
            Messenger(MessengeType.CallToAction,_);

            filePath = GetFile();
            outputDir = GetOutputDir();

            if(filePath==null||outputDir==null)
            {
                string msg = "Please select a valid File and Output directory";
                Messenger(MessengeType.Failure, msg);

                MainLoop();
            }
           

            XmlNodeList nodes = _io.GetXMLData(filePath, _bh.NodeName);

            if (nodes != null)
            {
                List<BillHeader> billHeadersList = _parse.ParseXML(nodes, _bh);

                if (billHeadersList != null)
                {
                    var writeData = _io.CreateWriteData(billHeadersList);

                }

            }



            StatusMessage = "Success";


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


        #endregion
        #region Get Output Dir & File Path
        public string GetOutputDir()
        {
            string outputDir = string.Empty;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a File Destination";


            if (fbd.ShowDialog() == DialogResult.OK)
            {
                outputDir = fbd.SelectedPath;
            }

            return outputDir;
        }

        public string GetFile()
        {
            string filePath = string.Empty;

            OpenFileDialog fdb = new OpenFileDialog();

            fdb.Filter = "xml files (*.xml)|*.xml";
            fdb.FilterIndex = 1;
            fdb.Multiselect = false;

            if (fdb.ShowDialog() == DialogResult.OK)
            {
                filePath = fdb.FileName;


            };

            return filePath;
        }

        #endregion

        #region Messenger
        public void Messenger(MessengeType messengeType,string message)
        {
            switch(messengeType.Value)
            {
                case "Success": Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "Failure":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "CallToAction":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(message);
            Console.WriteLine(Environment.NewLine);
            Console.ResetColor();

        }
        #endregion

        #region Messenger Enumeration Class
        public class MessengeType
        {
            #region Constructor
            private MessengeType(string value) { Value = value; }
            #endregion
            #region Variables
            public string Value { get; set; }
            #endregion

            #region Properties
            public static MessengeType Success { get { return new MessengeType("Success"); } }
            public static MessengeType Failure { get { return new MessengeType("Failure"); } }
            public static MessengeType CallToAction { get { return new MessengeType("CallToAction"); } }

            #endregion
        }
        #endregion
    }
}
