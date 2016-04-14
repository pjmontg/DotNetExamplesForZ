using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin_WorkAssignment.ModelAPI;

namespace Plugin_WorkAssignment.View.ViewAbstraction
{
    public interface IWorkOrderGridControl
    {
        IList<ACTIVITY> Activities { set; }

        string CurrentShop { get; }

        DateTime? CurrentWorkDate { get; }

        /// <summary>
        /// Currently selected filter of area field
        /// </summary>
        string CurrentAreaFilter { get; }

        /// <summary>
        /// Current activity ID for check box click
        /// </summary>
        decimal CurrentActivityID { get; }

        /// <summary>
        /// Currently states if overtime requirement is checked or not by current click
        /// </summary>
        bool CurrentIsOTReq { get; }

        /// <summary>
        /// When any field that has a list of display field data, it will 
        /// be stored in here during binding of row data to be processed
        /// </summary>
        string CurrentListFieldValue { get; }

        /// <summary>
        /// When any field that has a list of display field data, this is the processed
        /// display for it
        /// </summary>
        string CurrentListFieldDisplay { set; }

        /// <summary>
        /// Name of current field being processed
        /// </summary>
        string CurrentProcessedField { get; }
    }
}
