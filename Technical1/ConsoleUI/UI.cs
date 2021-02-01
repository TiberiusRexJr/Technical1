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
        private MessageUI ui = new MessageUI();

        #region Main Functionality

        public void MainLoop()
        {
            
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("1. Parse a .XML File to .RPT File");
            Console.WriteLine("2. Save the contents of a .RPT File to the Database");
            Console.WriteLine("3. Export the Contents of the Databse to a CSV File");
            Console.WriteLine("4. Quit The Program");

            ui.ConsoleMessage(MessageType.CallToAction, "Enter a Number Now");


            string input = Console.ReadLine();

            int choice = default;

            if (!Int32.TryParse(input, out choice))
            {
                ui.ConsoleMessage(MessageType.Failure, "Make a Valid Selection");
                MainLoop();
            }

            // Use a switch statement to do the math.
            switch (choice)
            {
                case 1:
                    XML_To_RPT();
                    break;
                case 2:
                    XML_To_DB();
                    break;
                case 3:
                    CSV_From_DB();
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

            string filePath = string.Empty;
            string outputDir = string.Empty;
            string errorMessage = string.Empty;
            string successMessage = string.Empty;

            ui.ConsoleMessage(MessageType.CallToAction,"Choose a XML file to parse");

            filePath = GetFile(FilterFileExt.xml);
            outputDir = GetOutputDir();

            if(filePath==null||outputDir==null)
            {
                
                ui.ConsoleMessage(MessageType.Failure, "Please select a valid File and Output directory and Try Again");

                MainLoop();
            }
           

            XmlNodeList nodes = _io.GetXMLData(filePath, _bh.NodeName);

            if (nodes != null)
            {
                List<BillHeader> billHeadersList = _parse.ParseXMLData(nodes);

                if (billHeadersList != null)
                {
                    var writeData = _io.CreateWriteDataRPT(billHeadersList);
                    
                    if(writeData.Header!=null||writeData.WriteData!=null)
                    {

                        MessageType messengeType = default;
                       bool successStatus= _io.WriteToRPT(filePath, writeData.Header, writeData.WriteData);
                        if(successStatus)
                        {
                            successMessage = "Operation Completed Successfully!";
                            messengeType = MessageType.Success;
                            MainLoop();
                        }
                        else
                        {
                            successMessage = "Operation Failed!";
                            messengeType = MessageType.Failure;
                            MainLoop();
                        }
                        ui.ConsoleMessage(messengeType, successMessage);
                    }

                }
                else
                {
                    
                    ui.ConsoleMessage(MessageType.Failure, "Failed! to get Data to WRite to file");
                    MainLoop();

                }

            }
            else
            {
               
                ui.ConsoleMessage(MessageType.Failure, "Failed! to get Node DAta from XML File");
                MainLoop();

            }

        }

        public void XML_To_DB()
        {
            string outputDirectory = string.Empty;
           

            ui.ConsoleMessage(MessageType.CallToAction, "Choose A Folder to Save the File Too");
            ui.ConsolePause();

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose a Output Location";


            if (fbd.ShowDialog() == DialogResult.OK)
            {
                outputDirectory = fbd.SelectedPath;
                if(string.IsNullOrEmpty(outputDirectory))
                {

                ui.ConsoleMessage(MessageType.Status, "Directory Accepted");
                }
                else
                {
                    ui.ConsoleMessage(MessageType.Failure, "Directory Not Selected!");
                    MainLoop();
                }


            }
            else
            {
                ui.ConsoleMessage(MessageType.Failure, "Failed to Select a Output Directy, Try again Please");
                MainLoop();
            }

            _parse.ParseRPT(outputDirectory);
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

            if (string.IsNullOrEmpty(outputDir))
            {

                ui.ConsoleMessage(MessageType.Failure, "Please select a valid Directory and Try Again");

                MainLoop();
            }

            return outputDir;
        }

        public string GetFile(FilterFileExt filter)
        {
            string filePath = string.Empty;

            OpenFileDialog fdb = new OpenFileDialog();

            fdb.Filter = filter.Value;
            fdb.FilterIndex = 1;
            fdb.Multiselect = false;

            if (fdb.ShowDialog() == DialogResult.OK)
            {
                filePath = fdb.FileName;


            };

            if (string.IsNullOrEmpty(filePath))
            {

                ui.ConsoleMessage(MessageType.Failure, "Please select a valid File and Try Again");

                MainLoop();
            }

            return filePath;
        }

        #endregion

        

        
    }
}
