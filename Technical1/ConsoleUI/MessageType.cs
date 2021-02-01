using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical1.ConsoleUI
{
    class MessageType
    {
        #region Constructor
        private MessageType(string value) { Value = value; }
        #endregion
        #region Variables
        public string Value { get; set; }
        #endregion

        #region Properties
        public static MessageType Success { get { return new MessageType("Success"); } }
        public static MessageType Failure { get { return new MessageType("Failure"); } }
        public static MessageType CallToAction { get { return new MessageType("CallToAction"); } }
        public static MessageType Status { get { return new MessageType("Status"); } }

        #endregion
    }
}
