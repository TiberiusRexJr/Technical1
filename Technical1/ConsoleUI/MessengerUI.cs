using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Technical1.ConsoleUI
{
    /// <summary>
    /// A Utility class meant to be used to give user feedback from the program to the user on the sonole
    /// <remarks>
    /// Rather then constantly having to call <example>Console.Writline()</example>. Simply provide a <typeparam>Messagetype</typeparam> static object and a custom text to the <c>ConsoleMessage</c> function and it will ake care of the rest.
    /// </remarks>
    /// </summary>
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
