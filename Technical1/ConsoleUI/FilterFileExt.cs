﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical1.ConsoleUI
{ /// <summary>
  /// A Class created to be used as a Puesdo Enumeration for file types
  /// <remarks>
  /// Can be expanded upon and saves time and typos.
  /// </remarks>
  /// </summary>
  
    class FilterFileExt
    {
        #region Constructor

       
        private FilterFileExt(string value) { Value = value; }
        #endregion
        #region Variables
        public string Value { get; set; }
        #endregion

        #region Properties
        public static FilterFileExt xml { get { return new FilterFileExt("xml files (*.xml)|*.xml"); } }
        public static FilterFileExt rpt { get { return new FilterFileExt("rpt Files (*.rpt)|*.rpt"); } }
       

        #endregion
    }
}
