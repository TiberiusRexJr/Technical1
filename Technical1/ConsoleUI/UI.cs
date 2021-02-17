using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technical1.FileOps;
using Technical1.Model;
using Technical1.Database;
namespace Technical1.ConsoleUI
{

    class UI
    {
        #region Variables
        private Parsing _parse = new Parsing();
        private Io _io = new Io();
        private FileOpsUtil _ut = new FileOpsUtil();
        private BillHeader _bh = new BillHeader();
        private MessageUI ui = new MessageUI();
        private Db _db = new Db();

        #endregion

        #region Main Functionality

        /// <summary>
        /// Main Entry Point for the Program. Gives the user Functionality choices.
        /// </summary>
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
                    RPT_To_DB();
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
        /// <summary>
        /// Contains all the logic accross many classes and functions to Parse the xml file to a RPT file
        /// </summary>
        public void XML_To_RPT()
        {

            string outputDir = string.Empty;
            string successMessage = string.Empty;

            
            ui.ConsoleMessage(MessageType.CallToAction,"Choose a XML file to parse");

            (string FilePath,string FileFormat) fileData = GetFileAndFormat(FilterFileExt.xml);

            ui.ConsoleMessage(MessageType.CallToAction, "Choose a Output Folder");
            outputDir = GetOutputDir();

            if(fileData.FilePath==null||outputDir==null)
            {
                
                ui.ConsoleMessage(MessageType.Failure, "Please select a valid File and Output directory and Try Again");

                MainLoop();
            }

           

            XmlNodeList nodes = _io.GetXMLData(fileData.FilePath,_bh.NodeName);

            if (nodes != null)
            {
                List<BillHeader> billHeadersList = _parse.ParseXMLData(nodes);

                if (billHeadersList != null)
                {
                    var rawData = _ut.FillInExtraData(billHeadersList);

                    var writeData = _io.CreateWriteDataRPT(rawData);
                    
                    if(writeData.Header!=null||writeData.WriteData!=null)
                    {

                        MessageType messengeType = default;
                       bool successStatus= _io.WriteToRPT(outputDir,writeData.Header, writeData.WriteData);
                        if(successStatus)
                        {
                            successMessage = "Operation Completed Successfully!";
                            messengeType = MessageType.Success;
                            ui.ConsoleMessage(messengeType, successMessage);
                            MainLoop();
                        }
                        else
                        {
                            successMessage = "Operation Failed!";
                            messengeType = MessageType.Failure;
                            ui.ConsoleMessage(messengeType, successMessage);
                            MainLoop();
                        }
                        
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
        /// <summary>
        /// Conatins all the logic to Parse a RPT File and save the Extracted Data in the Database
        /// </summary>
        public void RPT_To_DB()
        {
            string filePath = string.Empty;
            

            ui.ConsoleMessage(MessageType.CallToAction, "Choose A Folder to Save the File Too");


            var fileData = GetFileAndFormat(FilterFileExt.rpt);

          List<BillHeader> parsedDataList=  _parse.ParseRPT(fileData.FilePath);

            if(parsedDataList == null||parsedDataList.Count==0)
            {
                ui.ConsoleMessage(MessageType.Failure, "Failed to Parse the RPT File!");
                MainLoop();
            }

            if(_db.PutData(parsedDataList))
            {
                ui.ConsoleMessage(MessageType.Success, "File Successfully parsed and Saved to The Database");
                ui.ConsolePause();
                MainLoop();
            }
            else
            {
                ui.ConsoleMessage(MessageType.Failure, "Failed to save the Data to the Database!");
                ui.ConsolePause();
                MainLoop();
            }

        }

        /// <summary>
        /// Conatins the logic to Create a CSV file from the data in the Billing database
        /// </summary>
        public void CSV_From_DB()
        {
            string outputDirectory = string.Empty;

            ui.ConsoleMessage(MessageType.CallToAction, "Choose a output folder");
            outputDirectory = GetOutputDir();

            List<BillHeader> dataList= _db.GetData();

            if(dataList==null)
            {
                ui.ConsoleMessage(MessageType.Failure, "Failed to get Data from the Database!");
                MainLoop();
            }    

           (string Header,List<string> LineData) writeData= _io.CreateWriteDataCSV(dataList);

           if(writeData.Header==null||writeData.LineData==null)
            {
                ui.ConsoleMessage(MessageType.Failure, "Failed to get Write Data!");
                MainLoop();
            }

            if(_io.WriteToCSV(outputDirectory, writeData.Header, writeData.LineData))
            {
                ui.ConsoleMessage(MessageType.Success, "Operation Completed Successfully!");
                MainLoop();
            }
            else
            {
                ui.ConsoleMessage(MessageType.Failure, "Operation Failed!!");
                MainLoop();
            }
           

            MainLoop();
        }


        #endregion

        #region Get Output Dir & File Path
        public string GetOutputDir()
        {
            string outputDir = string.Empty;

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a File Destination";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;

            try
            {
                if (fbd.ShowDialog(new Form() { TopMost = true }) == DialogResult.OK)
                {
                    outputDir = fbd.SelectedPath;
                };
                
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            if (string.IsNullOrEmpty(outputDir))
            {

                ui.ConsoleMessage(MessageType.Failure, "Please select a valid Directory and Try Again");

                MainLoop();
            };

            return outputDir;
        }

        public (string FilePath,string FileFormat) GetFileAndFormat(FilterFileExt filter)
        {
            string FilePath = string.Empty;
            string FileFormat = string.Empty;

            OpenFileDialog fdb = new OpenFileDialog();

            fdb.Filter = filter.Value;
            fdb.FilterIndex = 1;
            fdb.Multiselect = false;

            if (fdb.ShowDialog() == DialogResult.OK)
            {
                FilePath = fdb.FileName;
                FileFormat = Path.GetExtension(fdb.FileName);

            };
           

            if (string.IsNullOrEmpty(FilePath))
            {

                ui.ConsoleMessage(MessageType.Failure, "Please select a valid File and Try Again");

                MainLoop();
            }

            return (FilePath, FileFormat);
        }

        #endregion

        

        
    }
}
