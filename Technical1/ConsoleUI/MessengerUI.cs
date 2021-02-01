using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Technical1.ConsoleUI
{
    class MessageUI
    {
        public MessageType MessageType { get; set; }

        #region Functions

        public void ConsoleMessage(MessageType messengeType, string message)
        {
            switch (messengeType.Value)
            {
                case "Success":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "Failure":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "CallToAction":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "Status":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(message);
            Console.WriteLine(Environment.NewLine);
            Console.ResetColor();


        }

        public void ConsolePause()
        {
            int milliseconds = 1000;
            Thread.Sleep(milliseconds);
        }
        #endregion
    }
}
